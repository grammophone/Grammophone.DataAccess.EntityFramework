using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// An <see cref="IEntitySet{E}"/> implementation based on
	/// Entity Framework's <see cref="DbSet{E}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entities.</typeparam>
	public class EFSet<E> : EFQuery<E, DbSet<E>>, IEntitySet<E>
		where E : class
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="dbSet">The entity framework set.</param>
		public EFSet(DbSet<E> dbSet)
			: base(dbSet)
		{
		}

		#endregion

		#region IEntitySet<E> Members

		/// <summary>
		/// Adds the given entity to the container underlying the set 
		/// in the Added state such that it will be inserted into the database when 
		/// <see cref="IDomainContainer.SaveChanges"/> is called.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void Add(E entity)
		{
			dbQuery.Add(entity);
		}

		/// <summary>
		/// Adds the given collection of entities into the set 
		/// with each entity being put into the Added state such that it 
		/// will be inserted into the database 
		/// when <see cref="IDomainContainer.SaveChanges"/> is called.
		/// </summary>
		/// <param name="entities"></param>
		public void AddRange(IEnumerable<E> entities)
		{
			dbQuery.AddRange(entities);
		}

		/// <summary>
		/// Attaches the given entity to the container underlying the set.
		/// That is, the entity is placed into the container in the Unchanged state,
		/// just as if it had been read from the database.
		/// </summary>
		/// <param name="entity">The entity to attach.</param>
		public void Attach(E entity)
		{
			dbQuery.Attach(entity);
		}

		/// <summary>
		/// Create an entity of type <typeparamref name="E"/>.
		/// Note that this instance is NOT added or attached to the set.
		/// The instance returned will be a proxy if the underlying container 
		/// is configured to create proxies and the entity type meets 
		/// the requirements for creating a proxy. 
		/// </summary>
		/// <returns>Returns the new entity.</returns>
		public E Create()
		{
			return dbQuery.Create();
		}

		/// <summary>
		/// Create an entity of type <typeparamref name="E"/>
		/// or a descendant type.
		/// Note that this instance is NOT added or attached to the set.
		/// The instance returned will be a proxy if the underlying container 
		/// is configured to create proxies and the entity type meets 
		/// the requirements for creating a proxy. 
		/// </summary>
		/// <typeparam name="T">
		/// The type of entity to create.
		/// It must be derived from <typeparamref name="E"/>.
		/// </typeparam>
		/// <returns>Returns the new entity.</returns>
		public T Create<T>() where T : class, E
		{
			return dbQuery.Create<T>();
		}

		/// <summary>
		/// Finds an entity with the given primary key values.
		/// If an entity with the given primary key values exists in the container,
		/// then it is returned immediately without making a request to the store.
		/// Otherwise, a request is made to the store for an entity with 
		/// the given primary key values and this entity, if found,
		/// is attached to the container and returned.
		/// If no entity is found in the container or the store, then null is returned.
		/// </summary>
		/// <param name="keys">The values of the primary key for the entity to be found.</param>
		/// <returns>The entity found, or null.</returns>
		public E Find(params object[] keys)
		{
			return dbQuery.Find(keys);
		}

		/// <summary>
		/// Marks the given entity as Deleted such that it will be deleted 
		/// from the database when SaveChanges is called.
		/// Note that the entity must exist in the container in some other state
		/// before this method is called. 
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public void Remove(E entity)
		{
			dbQuery.Remove(entity);
		}

		/// <summary>
		/// Removes the given collection of entities from the container underlying 
		/// the set with each entity being put into the Deleted state such that
		/// it will be deleted from the database when SaveChanges is called.
		/// </summary>
		/// <param name="entities">The entities to remove.</param>
		public void RemoveRange(IEnumerable<E> entities)
		{
			dbQuery.RemoveRange(entities);
		}

		#endregion
	}
}
