# Grammophone.DataAccess.EntityFramework
This is the Entity Framework implementation of the data access layer for the integrated session system.
The data access layer can also be used independently. It offers options implementing the data access contract
set in [Grammophone.DataAccess](https://github.com/grammophone/Grammophone.DataAccess) library.
It is part of the 3rd generation of the integrated session system which will support SaaS, 
workflow and accounting scenarios.

The library offers two base classes to implement the centerpiece `IDomainContainer` interface
defined in [Grammophone.DataAccess](https://github.com/grammophone/Grammophone.DataAccess), depending on the
level of desired abstraction level from Entity Framework's types.

### Partial abstraction with `EFDomainContainer` base class
This base class is derived directly from Entity Framework's `DbContext` and it is the easiest way
to define a domain container. In your domain container interfaces, define
entity sets properties using Entity Framework's `IDbSet<E>` and proceed as with a usual `DbContext`.
This will work directly using injection by Entity Framework automatically.
Interface `IDbSet<E>` can be implemented by other
technologies as well, but has the drawback that it would require those implementations
to reference Entity Framework.

### Total abstraction with `EFDomainContainerAdapter<D>` base class
Use this base class to define your entities repository when you want total abstraction
of your entity sets from Entity Framework types. This class adapts a
domain container of type `D` derived from `EFDomainContainer` which you created as outlined above.
In your domain container interfaces, define
entity sets properties using `IEntitySet<E>`
and implement them using `EFEntitySet<E>` in your domain container derived from `EFDomainContainerAdapter`.

In either choice, you would typically use a dependency injection framework to create
the appropriate implementation of domain container. Any implementation would also typically implement
an interface which defined the entity sets and is derived from `IDomainContainer`.

This library requires [Grammophone.DataAccess](https://github.com/grammophone/Grammophone.DataAccess) library
to be in a sibling forder.
