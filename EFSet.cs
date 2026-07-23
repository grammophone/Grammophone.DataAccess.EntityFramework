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
		/// <param name="domainContainer">The domain container which the query pertains to.</param>
		public EFSet(DbSet<E> dbSet, IDomainContainer domainContainer)
			: base(dbSet, domainContainer)
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
		/// <remarks>
		/// Adding an entity which is already tracked changes nothing.
		/// </remarks>
		public void Add(E entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			if (!IsAddable(entity)) return;

			NativeQuery.Add(entity);
		}

		/// <summary>
		/// Adds the given collection of entities into the set 
		/// with each entity being put into the Added state such that it 
		/// will be inserted into the database 
		/// when <see cref="IDomainContainer.SaveChanges"/> is called.
		/// </summary>
		/// <param name="entities"></param>
		/// <remarks>
		/// Entities which are already tracked are left alone.
		/// </remarks>
		public void AddRange(IEnumerable<E> entities)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));

			NativeQuery.AddRange(entities.Where(IsAddable));
		}

		/// <summary>
		/// Attaches the given entity to the container underlying the set.
		/// That is, the entity is placed into the container in the Unchanged state,
		/// just as if it had been read from the database.
		/// </summary>
		/// <param name="entity">The entity to attach.</param>
		public void Attach(E entity)
		{
			NativeQuery.Attach(entity);
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
			return NativeQuery.Create();
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
			return NativeQuery.Create<T>();
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
			return NativeQuery.Find(keys);
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
			NativeQuery.Remove(entity);
		}

		/// <summary>
		/// Removes the given collection of entities from the container underlying 
		/// the set with each entity being put into the Deleted state such that
		/// it will be deleted from the database when SaveChanges is called.
		/// </summary>
		/// <param name="entities">The entities to remove.</param>
		public void RemoveRange(IEnumerable<E> entities)
		{
			NativeQuery.RemoveRange(entities);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Determines whether an entity still has to be handed to the underlying set in order to be added.
		/// </summary>
		/// <param name="entity">The entity being added.</param>
		/// <returns>Returns true when the entity is not tracked yet.</returns>
		/// <exception cref="DataAccessException">Thrown when the entity is being deleted.</exception>
		/// <remarks>
		/// Adding an entity which is already tracked must not change anything. With nested transaction scopes
		/// an entity may well have been stored by an inner commit before control returns to the caller which
		/// adds it, and that caller has no way of knowing. Entity Framework would otherwise store it a second
		/// time under a fresh key, silently leaving a duplicate row behind and moving the entity at hand onto it.
		/// </remarks>
		private bool IsAddable(E entity)
		{
			var state = this.DomainContainer.Entry(entity).State;

			if (state == TrackingState.Deleted)
			{
				throw new DataAccessException(
					$"Cannot add an entity of type '{typeof(E).FullName}' while it is being deleted: " +
					"whether that abandons the deletion or stores a second entity is not defined.");
			}

			return state == TrackingState.Detached;
		}

		#endregion
	}
}
