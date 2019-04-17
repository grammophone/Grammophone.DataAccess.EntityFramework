using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Use this domain container implementation if you plan to expose 
	/// entity sets as <see cref="IEntitySet{E}"/>. It requires an underlying
	/// <see cref="EFDomainContainer"/> which exposes entity sets as <see cref="DbSet{E}"/>
	/// which you should adapt using <see cref="EFSet{E}"/>.
	/// </summary>
	public abstract class EFDomainContainerAdapter<D> : IDomainContainer
		where D : EFDomainContainer
	{
		#region Protected properties

		/// <summary>
		/// The adapted <see cref="EFDomainContainer"/>.
		/// </summary>
		protected D InnerDomainContainer { get; }

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="innerContainer">The adapted <see cref="EFDomainContainer"/>.</param>
		public EFDomainContainerAdapter(D innerContainer)
		{
			if (innerContainer == null) throw new ArgumentNullException(nameof(innerContainer));

			this.InnerDomainContainer = innerContainer;
		}

		#endregion

		#region IDomainContainer implementation

		/// <summary>
		/// Report and alter change tracking.
		/// </summary>
		public IChangeTracker ChangeTracker => ((IDomainContainer)this.InnerDomainContainer).ChangeTracker;

		/// <summary>
		/// Gets an <see cref="IEntityEntry{E}"/> object for the given entity 
		/// providing access to information about the entity 
		/// and the ability to perform actions on the entity.
		/// </summary>
		/// <typeparam name="E">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns>Returns the entry for the entity.</returns>
		public IEntityEntry<E> Entry<E>(E entity) where E : class
			=> this.InnerDomainContainer.GetEntry(entity);

		/// <summary>
		/// Collection of entity listeners.
		/// </summary>
		public ICollection<IEntityListener> EntityListeners
			=> this.InnerDomainContainer.EntityListeners;

		/// <summary>
		/// If true, lazy loading is enabled. The default is true.
		/// </summary>
		public bool IsLazyLoadingEnabled
		{
			get
			{
				return InnerDomainContainer.IsLazyLoadingEnabled;
			}
			set
			{
				InnerDomainContainer.IsLazyLoadingEnabled = value;
			}
		}

		/// <summary>
		/// If set as true and all preconditions are met, the container
		/// will provide proxy classes wherever applicable. Default is true.
		/// </summary>
		public bool IsProxyCreationEnabled
		{
			get
			{
				return InnerDomainContainer.IsProxyCreationEnabled;
			}
			set
			{
				InnerDomainContainer.IsProxyCreationEnabled = value;
			}
		}

		/// <summary>
		/// The transaction behavior.
		/// </summary>
		public TransactionMode TransactionMode
			=> this.InnerDomainContainer.TransactionMode;

		/// <summary>
		/// The underlying context which provides the access to the data.
		/// </summary>
		object IContextOwner.UnderlyingContext
			=> ((IDomainContainer)this.InnerDomainContainer).UnderlyingContext;

		/// <summary>
		/// Set the state of a detached object graph as 'modified'.
		/// If the graph elements  are in any other state than 'detached',
		/// their state is not changed.
		/// WARNING: Setting any of the graph's disconnected elements' 
		/// relationships to a connected object
		/// changes all the 'disconnected' states to 'added'. Therefore, set all relationships
		/// to connected objects AFTER the call to this method.
		/// </summary>
		/// <typeparam name="T">The type of the root of the graph.</typeparam>
		/// <param name="graphRoot">the root of the graph.</param>
		public void AttachGraphAsModified<T>(T graphRoot) where T : class
			=> this.InnerDomainContainer.AttachGraphAsModified(graphRoot);

		/// <summary>
		/// Begins a local transaction on the underlying store.
		/// </summary>
		/// <returns>Returns an <see cref="IDisposable"/> transaction object.</returns>
		public ITransaction BeginTransaction()
			=> this.InnerDomainContainer.BeginTransaction();

		/// <summary>
		/// Begins a local transaction on the underlying store 
		/// using the specified isolation level.
		/// </summary>
		/// <param name="isolationLevel">The requested isolation level.</param>
		/// <returns>Returns an <see cref="IDisposable"/> transaction object.</returns>
		/// <remarks>
		/// If the <see cref="TransactionMode"/> is <see cref="TransactionMode.Deferred"/>,
		/// the <paramref name="isolationLevel"/> parameter is ignored.
		/// </remarks>
		public ITransaction BeginTransaction(IsolationLevel isolationLevel)
			=> this.InnerDomainContainer.BeginTransaction(isolationLevel);

		/// <summary>
		/// Create a container proxy for a new object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object to be proxied.</typeparam>
		/// <returns>
		/// Returns a proxy for the new object if <see cref="IsProxyCreationEnabled"/>
		/// is true, else returns a pure object.
		/// </returns>
		public T Create<T>() where T : class
			=> this.InnerDomainContainer.Create<T>();

		/// <summary>
		/// Detach a tracked entity.
		/// </summary>
		/// <param name="entity">The entity to detach.</param>
		/// <remarks>
		/// If the entity is not tracked, this method does nothing.
		/// </remarks>
		public void Detach(object entity)
			=> this.InnerDomainContainer.Detach(entity);

		/// <summary>
		/// Save changes.
		/// </summary>
		/// <returns>Returns the number of objects written to the storage.</returns>
		/// <remarks>
		/// When in a transaction while <see cref="TransactionMode"/> is <see cref="TransactionMode.Deferred"/>, 
		/// this method does nothing and returns zero.
		/// </remarks>
		public int SaveChanges()
			=> this.InnerDomainContainer.SaveChanges();

		/// <summary>
		/// Save changes asynchronously.
		/// </summary>
		/// <returns>
		/// Returns a task whose result is the number of objects written to the storage.
		/// </returns>
		/// <remarks>
		/// When in a transaction while <see cref="TransactionMode"/> is <see cref="TransactionMode.Deferred"/>, 
		/// this method does nothing and returns zero.
		/// </remarks>
		public async Task<int> SaveChangesAsync()
			=> await this.InnerDomainContainer.SaveChangesAsync();

		/// <summary>
		/// Save changes asynchronously.
		/// </summary>
		/// <param name="cancellationToken">
		/// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
		/// </param>
		/// <returns>
		/// Returns a task whose result is the number of objects written to the storage.
		/// </returns>
		/// <remarks>
		/// When in a transaction while <see cref="TransactionMode"/> is <see cref="TransactionMode.Deferred"/>, 
		/// this method does nothing and returns zero.
		/// </remarks>
		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
			=> await this.InnerDomainContainer.SaveChangesAsync(cancellationToken);

		/// <summary>
		/// Set the state of an entity as 'modified'.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <remarks>
		/// This is useful for services data transfer,
		/// and it typically follows the attachment of
		/// a deserialized <paramref name="entity"/>.
		/// </remarks>
		public void SetAsModified(object entity)
			=> this.InnerDomainContainer.SetAsModified(entity);

		/// <summary>
		/// Transform any database-specific or provider-specific exception
		/// to descendants of <see cref="DataAccessException"/> when appropriate.
		/// </summary>
		/// <param name="exception">The exception to transform.</param>
		/// <returns>Returns the transformed exception or the same exception when no transformation is needed.</returns>
		public Exception TranslateException(SystemException exception)
			=> this.InnerDomainContainer.TranslateException(exception);

		#endregion

		#region IDisposable implementation

		/// <summary>
		/// Close and dispose the domain container.
		/// </summary>
		public void Dispose()
			=> this.InnerDomainContainer.Dispose();

		#endregion
	}
}
