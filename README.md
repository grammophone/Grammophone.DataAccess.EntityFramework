# Grammophone.DataAccess.EntityFramework

`Grammophone.DataAccess.EntityFramework` is the Entity Framework 6 implementation of the `Grammophone.DataAccess` abstractions.

It targets both .NET Framework and .NET Standard 2.1 and is intended for projects that use classic Entity Framework while exposing provider-neutral `IDomainContainer`, `IEntitySet<T>` and `IEntityQuery<T>` contracts to application logic.

## Main Features

- `EFDomainContainer` derives from EF6 `DbContext` and implements `IDomainContainer`.
- `EFDomainContainerAdapter<T>` adapts an EF6 domain container to a fully provider-neutral domain interface.
- `EFSet<T>` and `EFQuery<T, Q>` adapt EF6 `DbSet<T>` and queryables to `IEntitySet<T>` and `IEntityQuery<T>`.
- `EFTranslatingQueryProvider` preserves the query abstraction through standard LINQ composition.
- `EFQueryTranslatorFactory` maps portable query functions to EF6 `DbFunctions` and wires terminal and shaping adapters.
- `EFTerminalMethodsAdapter` delegates async terminal methods to EF6 async query APIs.
- `EFShapingMethodsAdapter` delegates executable query shaping operations such as `Include` and `AsNoTracking` to EF6.
- SQL Server exception transformers normalize provider errors into portable `DataAccessException` descendants.

## Usage Shape

Define an EF6 context:

```csharp
public class EFMusicDomainContainer : EFDomainContainer
{
	public DbSet<Artist> Artists { get; set; }
	public DbSet<Album> Albums { get; set; }
	public DbSet<Track> Tracks { get; set; }
	public DbSet<Genre> Genres { get; set; }
}
```

Define a provider-neutral domain contract:

```csharp
public interface IMusicDomainContainer : IDomainContainer
{
	IEntitySet<Artist> Artists { get; }
	IEntitySet<Album> Albums { get; }
	IEntitySet<Track> Tracks { get; }
	IEntitySet<Genre> Genres { get; }
}
```

Adapt the EF6 context explicitly:

```csharp
public class EFMusicDomainContainerAdapter :
	EFDomainContainerAdapter<EFMusicDomainContainer>,
	IMusicDomainContainer
{
	private IEntitySet<Album> albums;

	public IEntitySet<Album> Albums =>
		albums ??= new EFSet<Album>(this.InnerDomainContainer.Albums, this);
}
```

Application logic consumes `IMusicDomainContainer`, not EF6 `DbContext` or `DbSet<T>`.

## Documentation

- [Entity Framework 6 setup](documentation/setup.md)

## Related Projects

- `Grammophone.DataAccess` defines the provider-neutral contracts.
- `Grammophone.DataAccess.EntityFrameworkCore` provides the EF Core 8 implementation.
