using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Implementation of <see cref="IChangeTracker"/> for entity framework.
	/// </summary>
	public class EFChangeTracker : IChangeTracker
	{
		#region Private fields

		private DbContext dbContext;

		private DbChangeTracker dbChangeTracker;

		#endregion

		#region Construction

		internal EFChangeTracker(DbContext dbContext)
		{
			if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

			this.dbContext = dbContext;
			this.dbChangeTracker = dbContext.ChangeTracker;
		}

		#endregion

		/// <summary>
		/// Manually scan tracked entities for changes. Only necessary when <see cref="IDomainContainer.IsProxyCreationEnabled"/>
		/// is false or when an entity does not have all properties as virtual.
		/// </summary>
		public void DetectChanges()
			=> dbChangeTracker.DetectChanges();

		/// <summary>
		/// Get the entities being tracked.
		/// </summary>
		/// <returns>Returns a collection of the tracked entities.</returns>
		public IEnumerable<IEntityEntry<object>> Entries()
			=> dbChangeTracker.Entries<object>().Select(ne => new EFEntityEntry<object>(ne));

		/// <summary>
		/// Get the entities of type <typeparamref name="E"/> being tracked.
		/// </summary>
		/// <typeparam name="E">The type of the entities being tracked.</typeparam>
		/// <returns>Returns a collection of the specified tracked entities.</returns>
		public IEnumerable<IEntityEntry<E>> Entries<E>() where E : class
			=> dbChangeTracker.Entries<E>().Select(ne => new EFEntityEntry<E>(ne));

		/// <summary>
		/// Get the entities of type being tracked with specified tracking states.
		/// </summary>
		/// <param name="trackingState">Combination of <see cref="TrackingState"/> values via OR.</param>
		/// <returns>Returns a collection of the specified tracked entities.</returns>
		public IEnumerable<IEntityEntry<object>> Entries(TrackingState trackingState)
		{
			var entityState = TypeConversions.TrackingStateToEntityState(trackingState);

			return dbChangeTracker.Entries<object>().Where(ne => (ne.State & entityState) != 0).Select(ne => new EFEntityEntry<object>(ne));
		}

		/// <summary>
		/// Get the entities of type <typeparamref name="E"/> being tracked with specified tracking states.
		/// </summary>
		/// <typeparam name="E">The type of the entities being tracked.</typeparam>
		/// <param name="trackingState">Combination of <see cref="TrackingState"/> values via OR.</param>
		/// <returns>Returns a collection of the specified tracked entities.</returns>
		public IEnumerable<IEntityEntry<E>> Entries<E>(TrackingState trackingState) where E : class
		{
			var entityState = TypeConversions.TrackingStateToEntityState(trackingState);

			return dbChangeTracker.Entries<E>().Where(ne => (ne.State & entityState) != 0).Select(ne => new EFEntityEntry<E>(ne));
		}

		/// <summary>
		/// True when the tracked entities have unsaved changes or when new entities are to be saved or when entities are to be deleted.
		/// </summary>
		public bool HasChanges()
			=> dbChangeTracker.HasChanges();

		/// <summary>
		/// Undo any changes to tracked entities.
		/// In particular, revert values of changed entities, detach new entities, and cancel deleting entities.
		/// </summary>
		public void UndoChanges()
		{
			// Detach the added.
			var addedEntries = dbChangeTracker.Entries().Where(e => (e.State & EntityState.Added) != 0);

			foreach (var addedEntry in addedEntries)
			{
				addedEntry.State = EntityState.Detached;
			}

			// Undo the modified.
			var modifiedEntries = dbChangeTracker.Entries().Where(e => (e.State & EntityState.Modified) != 0);

			foreach (var modifiedEntry in modifiedEntries)
			{
				modifiedEntry.CurrentValues.SetValues(modifiedEntry.OriginalValues);

				modifiedEntry.State = (modifiedEntry.State & ~EntityState.Modified) | EntityState.Unchanged;
			}

			// Undelete.
			var deletedEntries = dbChangeTracker.Entries().Where(e => (e.State & EntityState.Deleted) != 0);

			foreach (var deletedEntry in deletedEntries)
			{
				deletedEntry.CurrentValues.SetValues(deletedEntry.OriginalValues);

				deletedEntry.State = EntityState.Unchanged;
			}
		}
	}
}
