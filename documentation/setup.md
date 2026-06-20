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

## Set-Based Mutations

The vanilla EF6 implementation does not include set-based mutation support. Calls such as `ExecuteDeleteAsync` and `ExecuteUpdateAsync` throw unless a provider extension supplies an implementation.

Use the optional `Grammophone.DataAccess.EntityFramework.Plus` package for EF6 set-based delete and update operations backed by Entity Framework Plus.

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

Use `IDomainContainer.Create<T>()` or `IEntitySet<T>.Create()` when application code needs a new entity instance. The EF6 implementation will create a proxy instance when proxy creation is enabled and the entity type satisfies the provider requirements.

For reliable proxy behavior, every mapped entity property must be `virtual` without exception, including scalar properties, key properties, reference navigations and collection navigations. This keeps newly created entities consistent with entities materialized by the provider.

## Transactions

`EFDomainContainer` supports `TransactionMode.Real` and `TransactionMode.Deferred`. Use `BeginTransaction()` from application code and call `Commit()` or `Pass()` before disposing the transaction.
