# Entity Framework 6 Setup

This project adapts EF6 `DbContext` and `DbSet<T>` to the `Grammophone.DataAccess` contracts.

## Domain Container

Start with an EF6 domain container deriving from `EFDomainContainer`:

```csharp
public class EFMusicDomainContainer : EFDomainContainer
{
	public EFMusicDomainContainer(string nameOrConnectionString)
		: base(nameOrConnectionString)
	{
	}

	public DbSet<Artist> Artists { get; set; }
	public DbSet<Album> Albums { get; set; }
	public DbSet<Track> Tracks { get; set; }
	public DbSet<Genre> Genres { get; set; }

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Artist>().HasKey(a => a.ID);
		modelBuilder.Entity<Artist>().Property(a => a.Name).IsRequired().HasMaxLength(200);

		modelBuilder.Entity<Album>()
			.HasRequired(a => a.Artist)
			.WithMany(a => a.Albums)
			.HasForeignKey(a => a.ArtistID);
	}
}
```

## Adapter

Expose provider-neutral sets through an adapter:

```csharp
public class EFMusicDomainContainerAdapter :
	EFDomainContainerAdapter<EFMusicDomainContainer>,
	IMusicDomainContainer
{
	private IEntitySet<Artist> artists;
	private IEntitySet<Album> albums;

	public EFMusicDomainContainerAdapter(EFMusicDomainContainer innerContainer)
		: base(innerContainer)
	{
	}

	public IEntitySet<Artist> Artists =>
		artists ??= new EFSet<Artist>(this.InnerDomainContainer.Artists, this);

	public IEntitySet<Album> Albums =>
		albums ??= new EFSet<Album>(this.InnerDomainContainer.Albums, this);
}
```

This explicit mapping is intentional. It keeps the public domain contract free of EF6 types and makes every adapted entity set visible in code.

## Query Extensions

Once the query originates from an adapted `IEntitySet<T>`, portable query extensions can be used:

```csharp
using Grammophone.DataAccess.QueryExtensions;

var album = await musicDomainContainer.Albums
	.Include(a => a.Tracks)
	.ThenInclude(t => t.Genre)
	.AsNoTracking()
	.SingleAsync(a => a.Name == "Blue Integration");
```

EF6 receives native `Include`, `AsNoTracking`, `DbFunctions` and async terminal calls through the adapter system.

## SQL Server Exception Translation

Set an exception transformer when SQL Server errors should be normalized:

```csharp
var innerContainer = new EFMusicDomainContainer("default")
{
	ExceptionTransformer = new SqlServerExceptionTransformer()
};

var domainContainer = new EFMusicDomainContainerAdapter(innerContainer);
```

If the project uses `Microsoft.Data.SqlClient`, use `MicrosoftSqlServerExceptionTransformer` instead.

## Proxy Creation

EF6 can create proxy entities through `DbSet<T>.Create()`. `EFDomainContainer.Create<T>()` and `EFSet<T>.Create()` use that behavior, subject to EF6 proxy requirements such as virtual properties and proxy creation being enabled.

## Transactions

`EFDomainContainer` supports `TransactionMode.Real` and `TransactionMode.Deferred`. Use `BeginTransaction()` from application code and call `Commit()` or `Pass()` before disposing the transaction.
