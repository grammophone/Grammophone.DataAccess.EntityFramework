using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// An implementation of <see cref="ITransaction"/> using
	/// the Entity Framework's <see cref="DbContextTransaction"/>.
	/// </summary>
	public class EFTransaction : ITransaction
	{
		#region Private fields

		private EFDomainContainer domainContainer;

		private bool markedForCommit;

		#endregion

		#region Construction

		internal EFTransaction(EFDomainContainer domainContainer)
		{
			if (domainContainer == null) throw new ArgumentNullException("domainContainer");

			this.domainContainer = domainContainer;
			this.markedForCommit = false;
		}

		#endregion

		#region ITransaction Members

		/// <summary>
		/// Marks the transaction as valid for commit.
		/// Actual committing takes place when all nested transactions are
		/// disposed and marked as committed.
		/// If this method or <see cref="CommitAsync"/> or <see cref="Pass"/> has not been called when
		/// method <see cref="IDisposable.Dispose"/> is invoked, the
		/// transaction is marked for rollback. A <see cref="IDomainContainer.SaveChanges"/>
		/// call is implied calling this method when the transaction
		/// is not marked for rollback.
		/// </summary>
		public void Commit()
		{
			EnsureNotDisposed();

			this.domainContainer.OnCommitTransaction();

			markedForCommit = true;
		}

		/// <summary>
		/// Marks the transaction as valid for commit.
		/// Actual committing takes place when all nested transactions are
		/// disposed and marked as committed.
		/// If this method or <see cref="Commit"/> or <see cref="Pass"/> has not been called when
		/// method <see cref="IDisposable.Dispose"/> is invoked, the
		/// transaction is marked for rollback. A <see cref="IDomainContainer.SaveChangesAsync()"/>
		/// call is implied calling this method when the transaction
		/// is not marked for rollback.
		/// </summary>
		public async Task CommitAsync()
		{
			EnsureNotDisposed();

			await this.domainContainer.OnCommitTransactionAsync();

			markedForCommit = true;
		}

		/// <summary>
		/// Marks the transaction valid for commit but does not save.
		/// Prevents rollback of higher nesting transactions;
		/// thus passes the decision whether to save to the higher transactions.
		/// If this method or <see cref="Commit"/> or <see cref="CommitAsync"/> has not been called when
		/// method <see cref="IDisposable.Dispose"/> is invoked, the
		/// transaction is marked for rollback.
		/// </summary>
		public void Pass()
		{
			EnsureNotDisposed();

			markedForCommit = true;
		}

		/// <summary>
		/// Fired when the whole transaction is committed successfully.
		/// </summary>
		public event Action Succeeding;

		/// <summary>
		/// Fired when the whole transaction is rolled back.
		/// </summary>
		public event Action RollingBack;

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Cleans up the transaction. If the transaction
		/// has not been committed, it is rolled back.
		/// </summary>
		public void Dispose()
		{
			EnsureNotDisposed();

			if (markedForCommit)
			{
				domainContainer.VoteForCommitAndDisposeTransaction();
			}
			else
			{
				domainContainer.VoteForRollbackAndDisposeTransaction();
			}

			domainContainer = null;
		}

		#endregion

		#region Internal methods

		internal void OnSucceeding()
		{
			this.Succeeding?.Invoke();
		}

		internal void OnRollingBack()
		{
			this.RollingBack?.Invoke();
		}

		#endregion

		#region Private methods

		private void EnsureNotDisposed()
		{
			if (domainContainer == null)
				throw new SystemException("The transaction is already disposed.");
		}

		#endregion
	}
}
