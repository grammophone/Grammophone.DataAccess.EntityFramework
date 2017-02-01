﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// An Entity Framework <see cref="DbContext"/> which also 
	/// implements <see cref="IDomainContainer"/>. This means that the entity sets whould be defined as
	/// Entity Framework's <see cref="ISet{T}"/> rather than <see cref="IEntitySet{E}"/>.
	/// If <see cref="IEntitySet{E}"/>'s are required, adapt an intance of this class
	/// using <see cref="EFDomainContainerAdapter"/>.
	/// </summary>
	public abstract class EFDomainContainer : DbContext, IDomainContainer
	{
		#region Private fields

		private ICollection<IEntityListener> entityListeners =
			new List<IEntityListener>();

		private int transactionNestingLevel;

		private bool votedForRollback;

		private ICollection<EFTransaction> openTransactions = new List<EFTransaction>();

		#endregion

		#region Construction

		/// <summary>
		/// Constructs a new container instance using conventions to 
		/// create the name of the database to which a connection will be made.
		/// The by-convention name is the full name (namespace + class name)
		/// of the derived container class.
		/// </summary>
		/// <param name="transactionMode">The transaction behavior.</param>
		public EFDomainContainer(TransactionMode transactionMode)
		{
			this.TransactionMode = transactionMode;

			Initialize();

			WireEventHandlers();
		}

		/// <summary>
		/// Constructs a new container instance using conventions to 
		/// create the name of the database to which a connection will be made.
		/// The by-convention name is the full name (namespace + class name)
		/// of the derived container class.
		/// The <see cref="TransactionMode"/> is set to <see cref="TransactionMode.Real"/>.
		/// </summary>
		public EFDomainContainer() : this(TransactionMode.Real)
		{
		}

		/// <summary>
		/// Constructs a new container instance using the given string as the name
		/// or connection string for the database to which a connection will be made. 
		/// </summary>
		/// <param name="nameOrConnectionString">
		/// Either the database name or a connection string.
		/// </param>
		/// <param name="transactionMode">The transaction behavior.</param>
		public EFDomainContainer(string nameOrConnectionString, TransactionMode transactionMode)
			: base(nameOrConnectionString)
		{
			this.TransactionMode = transactionMode;

			Initialize();

			WireEventHandlers();
		}

		/// <summary>
		/// Constructs a new container instance using the given string as the name
		/// or connection string for the database to which a connection will be made. 
		/// The <see cref="TransactionMode"/> is set to <see cref="TransactionMode.Real"/>.
		/// </summary>
		/// <param name="nameOrConnectionString">
		/// Either the database name or a connection string.
		/// </param>
		public EFDomainContainer(string nameOrConnectionString)
			: this(nameOrConnectionString, TransactionMode.Real)
		{
		}

		/// <summary>
		/// Constructs a new container instance using a given connection.
		/// </summary>
		/// <param name="connection">The connection to use.</param>
		/// <param name="ownTheConnection">If true, hand over connection ownership to the container.</param>
		/// <param name="transactionMode">The transaction behavior.</param>
		public EFDomainContainer(System.Data.Common.DbConnection connection, bool ownTheConnection, TransactionMode transactionMode)
			: base(connection, ownTheConnection)
		{
			this.TransactionMode = transactionMode;

			Initialize();

			WireEventHandlers();
		}

		#endregion

		#region IDomainContainer members not already provided by DbContext

		/// <summary>
		/// Gets an <see cref="IEntityEntry{E}"/> object for the given entity 
		/// providing access to information about the entity 
		/// and the ability to perform actions on the entity.
		/// This method explictly implements the <see cref="IDomainContainer.Entry{E}(E)"/> method
		/// by wrapping and abstracting the <see cref="DbContext.Entry{TEntity}(TEntity)"/> method.
		/// </summary>
		/// <typeparam name="E">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns>Returns the entry for the entity.</returns>
		public IEntityEntry<E> GetEntry<E>(E entity)
			where E : class
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			var underlyingEntityEntry = this.Entry(entity);

			if (underlyingEntityEntry != null)
				return new EFEntityEntry<E>(underlyingEntityEntry);
			else
				return null;
		}

		/// <summary>
		/// Gets an <see cref="IEntityEntry{E}"/> object for the given entity 
		/// providing access to information about the entity 
		/// and the ability to perform actions on the entity.
		/// This method is implemented by a redirection to <see cref="GetEntry{E}(E)"/>.
		/// </summary>
		/// <typeparam name="E">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns>Returns the entry for the entity.</returns>
		IEntityEntry<E> IDomainContainer.Entry<E>(E entity) => GetEntry(entity);

		/// <summary>
		/// The transaction behavior.
		/// </summary>
		public TransactionMode TransactionMode { get; private set; }

		/// <summary>
		/// Save changes.
		/// </summary>
		/// <returns>Returns the number of objects written to the storage.</returns>
		/// <remarks>
		/// When in a transaction while <see cref="TransactionMode"/> is <see cref="TransactionMode.Deferred"/>, 
		/// this method does nothing and returns zero.
		/// </remarks>
		public override int SaveChanges()
		{
			if (TransactionMode == TransactionMode.Deferred && transactionNestingLevel >= 1) return 0;

			int changesCount = base.SaveChanges();

			return changesCount;
		}

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
		public override async Task<int> SaveChangesAsync()
		{
			if (TransactionMode == TransactionMode.Deferred && transactionNestingLevel >= 1) return 0;

			int changesCount = await base.SaveChangesAsync();

			return changesCount;
		}

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
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			if (TransactionMode == TransactionMode.Deferred && transactionNestingLevel >= 1) return 0;

			int changesCount = await base.SaveChangesAsync(cancellationToken);

			return changesCount;
		}

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
		{
			var entry = this.Entry(entity);

			if (entry != null) entry.State = EntityState.Modified;
		}

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
		public void AttachGraphAsModified<T>(T graphRoot)
			where T : class
		{
			if (graphRoot == null) throw new ArgumentNullException(nameof(graphRoot));

			var objectContext = ((IObjectContextAdapter)this).ObjectContext;

			var objectStateManager = objectContext.ObjectStateManager;

			var attachedEntities = new List<object>();

			CollectionChangeEventHandler stateChangeListener = delegate (object sender, CollectionChangeEventArgs e)
			{
				if (e.Action == CollectionChangeAction.Add)
					attachedEntities.Add(e.Element);
			};

			objectStateManager.ObjectStateManagerChanged += stateChangeListener;

			try
			{
				this.Set<T>().Attach(graphRoot);
			}
			finally
			{
				objectStateManager.ObjectStateManagerChanged -= stateChangeListener;
			}

			foreach (var entity in attachedEntities)
			{
				SetAsModified(entity);
			}
		}

		/// <summary>
		/// Detach a tracked entity.
		/// </summary>
		/// <param name="entity">The entity to detach.</param>
		/// <remarks>
		/// If the entity is not tracked, this method does nothing.
		/// </remarks>
		public void Detach(object entity)
		{
			var entry = this.Entry(entity);

			if (entry != null) entry.State = EntityState.Detached;
		}

		/// <summary>
		/// Begins a local transaction on the underlying store.
		/// </summary>
		/// <returns>Returns an <see cref="IDisposable"/> transaction object.</returns>
		public ITransaction BeginTransaction()
		{
			int currentTransactionNestingLevel = Interlocked.Increment(ref transactionNestingLevel);

			var transaction = CreateTransaction();

			this.openTransactions.Add(transaction);

			return transaction;
		}

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
		public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
		{
			int currentTransactionNestingLevel = Interlocked.Increment(ref transactionNestingLevel);

			var transaction = CreateTransaction(isolationLevel);

			this.openTransactions.Add(transaction);

			return transaction;
		}

		/// <summary>
		/// Collection of entity listeners.
		/// </summary>
		public ICollection<IEntityListener> EntityListeners
		{
			get { return entityListeners; }
		}

		/// <summary>
		/// Create a container proxy for a new object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object to be proxied.</typeparam>
		/// <returns>Returns a proxy for the new object.</returns>
		public T Create<T>() where T : class
		{
			return this.Set<T>().Create();
		}

		/// <summary>
		/// If set as true and all preconditions are met, the container
		/// will provide proxy classes wherever applicable. Default is true.
		/// </summary>
		public bool IsProxyCreationEnabled
		{
			get
			{
				return this.Configuration.ProxyCreationEnabled;
			}
			set
			{
				this.Configuration.ProxyCreationEnabled = value;
			}
		}

		/// <summary>
		/// If true, lazy loading is enabled. The default is true.
		/// </summary>
		public bool IsLazyLoadingEnabled
		{
			get
			{
				return this.Configuration.LazyLoadingEnabled;
			}
			set
			{
				this.Configuration.LazyLoadingEnabled = value;
			}
		}

		#endregion

		#region IContextOwner Members

		object IContextOwner.UnderlyingContext
		{
			get { return this; }
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Called first inside the constructors.
		/// </summary>
		/// <remarks>
		/// Default implementation does nothing.
		/// </remarks>
		protected virtual void Initialize()
		{
		}

		/// <summary>
		/// Add unwiring of event handlers to the disposing.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			UnwireEventHandlers();
			CleanupCurrentTransaction();

			base.Dispose(disposing);
		}

		#endregion

		#region Internal methods

		internal void VoteForCommitAndDisposeTransaction()
		{
			DisposeTransaction();
		}

		internal void VoteForRollbackAndDisposeTransaction()
		{
			votedForRollback = true;

			DisposeTransaction();
		}

		internal void OnCommitTransaction()
		{
			if (!votedForRollback)
			{
				if (TransactionMode == TransactionMode.Real || transactionNestingLevel == 1)
					base.SaveChanges();
			}
		}

		internal async Task OnCommitTransactionAsync()
		{
			if (!votedForRollback)
			{
				if (TransactionMode == TransactionMode.Real || transactionNestingLevel == 1)
					await base.SaveChangesAsync();
			}
		}

		#endregion

		#region Private methods

		private EFTransaction CreateTransaction()
		{
			DbContextTransaction dbContextTransaction;

			if (TransactionMode == TransactionMode.Real)
				dbContextTransaction = this.Database.BeginTransaction();
			else
				dbContextTransaction = null;

			return new EFTransaction(this, dbContextTransaction);
		}

		private EFTransaction CreateTransaction(System.Data.IsolationLevel isolationLevel)
		{
			DbContextTransaction dbContextTransaction;

			if (TransactionMode == TransactionMode.Real)
				dbContextTransaction = this.Database.BeginTransaction(isolationLevel);
			else
				dbContextTransaction = null;

			return new EFTransaction(this, dbContextTransaction);
		}

		private void CleanupCurrentTransaction()
		{
			votedForRollback = false;

			openTransactions.Clear();

			transactionNestingLevel = 0;
		}

		private void DisposeTransaction()
		{
			int currentTransactionNestingLevel =
				Interlocked.Decrement(ref transactionNestingLevel);

			if (currentTransactionNestingLevel < 0)
				throw new SystemException("Unmatched excessive Dispose.");

			if (currentTransactionNestingLevel == 0)
			{
				if (!votedForRollback)
				{
					try
					{
						FireTransactionSucceeding();
					}
					catch
					{
						FireTransactionRollingBack();
						throw;
					}
				}
				else
				{
					FireTransactionRollingBack();
				}

				CleanupCurrentTransaction();
			}
		}

		private void FireTransactionRollingBack()
		{
			foreach (var transaction in this.openTransactions)
			{
				try
				{
					transaction.OnRollingBack();
				}
				catch
				{
					// Ignore exceptions to allow all rollback listeners to be activated.
				}
			}
		}

		private void FireTransactionSucceeding()
		{
			foreach (var transaction in this.openTransactions)
			{
				try
				{
					transaction.OnSucceeding();
				}
				catch
				{
					// Ignore exceptions to allow all success listeners to be activated.
				}
			}
		}

		private void WireEventHandlers()
		{
			var objectContext = ((IObjectContextAdapter)this).ObjectContext;

			objectContext.SavingChanges += OnSavingChanges;
			objectContext.ObjectMaterialized += OnObjectMaterialized;
		}

		private void UnwireEventHandlers()
		{
			var objectContext = ((IObjectContextAdapter)this).ObjectContext;

			objectContext.SavingChanges -= OnSavingChanges;
			objectContext.ObjectMaterialized -= OnObjectMaterialized;
		}

		private void OnObjectMaterialized(object sender, System.Data.Entity.Core.Objects.ObjectMaterializedEventArgs e)
		{
			foreach (var entityListener in this.EntityListeners)
			{
				entityListener.OnRead(e.Entity);
			}
		}

		private void OnSavingChanges(object sender, EventArgs e)
		{
			var objectContext = ((IObjectContextAdapter)this).ObjectContext;

			var stateManager = objectContext.ObjectStateManager;

			var deletedEntries = stateManager.GetObjectStateEntries(EntityState.Deleted);
			var changedEntries = stateManager.GetObjectStateEntries(EntityState.Modified);
			var addedEntries = stateManager.GetObjectStateEntries(EntityState.Added);

			foreach (var entityListener in this.EntityListeners)
			{
				foreach (var deletedEntry in deletedEntries)
				{
					if (deletedEntry.Entity == null) continue;

					entityListener.OnDeleting(deletedEntry.Entity);
				}

				foreach (var changedEntry in changedEntries)
				{
					if (changedEntry.Entity == null) continue;

					entityListener.OnChanging(changedEntry.Entity);
				}

				foreach (var addedEntry in addedEntries)
				{
					if (addedEntry.Entity == null) continue;

					entityListener.OnAdding(addedEntry.Entity);
				}
			}

		}

		#endregion

	}
}
