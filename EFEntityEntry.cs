using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Provides information about and control of an entity 
	/// by implementing <see cref="IEntityEntry{E}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	public class EFEntityEntry<E> : IEntityEntry<E>
		where E : class
	{
		#region Private fields

		private readonly DbEntityEntry<E> underlyingEntityEntry;

		#endregion

		#region Construction

		internal EFEntityEntry(DbEntityEntry<E> underlyingEntry)
		{
			if (underlyingEntry == null) throw new ArgumentNullException(nameof(underlyingEntry));

			this.underlyingEntityEntry = underlyingEntry;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The entity.
		/// </summary>
		public E Entity => underlyingEntityEntry.Entity;

		/// <summary>
		/// The state of the <see cref="Entity"/>.
		/// </summary>
		public TrackingState State
		{
			get
			{
				return ConvertEntityStateToTrackingState(underlyingEntityEntry.State);
			}
			set
			{
				underlyingEntityEntry.State = ConvertTrackingStateToEntityState(value);
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Gets an object that represents a collection property of this entity.
		/// </summary>
		/// <typeparam name="I">The type of items in the collection.</typeparam>
		/// <param name="propertySelector">An expression representing the property.</param>
		/// <returns>Returns an object representing the property.</returns>
		public ICollectionEntry<E, I> Collection<I>(Expression<Func<E, ICollection<I>>> propertySelector)
			where I : class
		{
			if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

			var underlyingCollectionEntry = underlyingEntityEntry.Collection(propertySelector);

			if (underlyingEntityEntry != null)
				return new EFCollectionEntry<E, I>(this, underlyingCollectionEntry);
			else
				return null;
		}

		/// <summary>
		/// Gets an object that represents a complex property of this entity.
		/// </summary>
		/// <typeparam name="P">The type of the property.</typeparam>
		/// <param name="propertySelector">An expression representing the property.</param>
		/// <returns>Returns an object representing the property.</returns>
		public IComplexPropertyEntry<E, P> ComplexProperty<P>(Expression<Func<E, P>> propertySelector)
		{
			if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

			var underlyingComplexPropertyEntry = underlyingEntityEntry.ComplexProperty(propertySelector);

			if (underlyingComplexPropertyEntry != null)
				return new EFComplexPropertyEntry<E, P>(this, underlyingComplexPropertyEntry);
			else
				return null;
		}

		/// <summary>
		/// Gets an object that represents a scalar or complex property of this entity.
		/// </summary>
		/// <typeparam name="P">The type of the property.</typeparam>
		/// <param name="propertySelector">An expression representing the property.</param>
		/// <returns>Returns an object representing the property.</returns>
		public IPropertyEntry<E, P> Property<P>(Expression<Func<E, P>> propertySelector)
		{
			if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

			var underlyingPropertyEntry = underlyingEntityEntry.Property(propertySelector);

			if (underlyingPropertyEntry != null)
				return new EFPropertyEntry<E, P>(this, underlyingPropertyEntry);
			else
				return null;
		}

		/// <summary>
		/// Gets an object that represents a reference property of this entity.
		/// </summary>
		/// <typeparam name="P">The type of the property.</typeparam>
		/// <param name="propertySelector">An expression representing the property.</param>
		/// <returns>Returns an object representing the property.</returns>
		public IReferenceEntry<E, P> Reference<P>(Expression<Func<E, P>> propertySelector)
			where P : class
		{
			if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

			var underlyingReferenceEntry = underlyingEntityEntry.Reference(propertySelector);

			if (underlyingReferenceEntry != null)
				return new EFReferenceEntry<E, P>(this, underlyingReferenceEntry);
			else
				return null;
		}

		/// <summary>
		/// Reloads the entity from the database overwriting any property 
		/// values with values from the database.
		/// The entity will be in the <see cref="TrackingState.Unchanged"/> state after calling this method.
		/// </summary>
		public void Reload()
		{
			underlyingEntityEntry.Reload();
		}

		/// <summary>
		/// Asynchronously reloads the entity from the database overwriting any property 
		/// values with values from the database.
		/// The entity will be in the <see cref="TrackingState.Unchanged"/> state after calling this method.
		/// </summary>
		/// <returns>Returns a task completing the action.</returns>
		public async Task ReloadAsync()
		{
			await underlyingEntityEntry.ReloadAsync();
		}

		/// <summary>
		/// Asynchronously reloads the entity from the database overwriting any property 
		/// values with values from the database.
		/// The entity will be in the <see cref="TrackingState.Unchanged"/> state after calling this method.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
		/// <returns>Returns a task completing the action.</returns>
		public async Task ReloadAsync(CancellationToken cancellationToken)
		{
			await underlyingEntityEntry.ReloadAsync(cancellationToken);
		}

		#endregion

		#region Private methods

		private static TrackingState ConvertEntityStateToTrackingState(EntityState entityState)
		{
			switch (entityState)
			{
				case EntityState.Detached:
					return TrackingState.Detached;

				case EntityState.Unchanged:
					return TrackingState.Unchanged;

				case EntityState.Added:
					return TrackingState.Added;

				case EntityState.Deleted:
					return TrackingState.Deleted;

				case EntityState.Modified:
					return TrackingState.Modified;

				default:
					throw new ArgumentException($"Unsupported value {entityState}.", nameof(entityState));
			}
		}

		private static EntityState ConvertTrackingStateToEntityState(TrackingState trackingState)
		{
			switch (trackingState)
			{
				case TrackingState.Detached:
					return EntityState.Detached;

				case TrackingState.Added:
					return EntityState.Added;

				case TrackingState.Deleted:
					return EntityState.Deleted;

				case TrackingState.Modified:
					return EntityState.Modified;

				case TrackingState.Unchanged:
					return EntityState.Unchanged;

				default:
					throw new ArgumentException($"Unsupported value {trackingState}.", nameof(trackingState));
			}
		}

		#endregion
	}
}
