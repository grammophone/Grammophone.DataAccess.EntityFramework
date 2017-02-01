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

		private DbContextTransaction dbContextTransaction;

		#endregion

		#region Construction

		internal EFTransaction(EFDomainContainer domainContainer, DbContextTransaction dbContextTransaction)
		{
			if (domainContainer == null) throw new ArgumentNullException("domainContainer");

			this.domainContainer = domainContainer;
			this.dbContextTransaction = dbContextTransaction;
			this.markedForCommit = false;
		}

		#endregion

		#region ITransaction Members

		/// <summary>
		/// Marks the transaction as valid for commit.
		/// Actual committing takes place when all nested transactions are
		/// disposed and marked as committed.
		/// If this method has not been called when 
		/// method <see cref="IDisposable.Dispose"/> is invoked, the
		/// transaction is marked for rollback. A <see cref="IDomainContainer.SaveChanges"/>
		/// call is implied calling this method when the transaction
		/// is not marked for rollback.
		/// </summary>
		public void Commit()
		{
			EnsureNotDisposed();

			this.domainContainer.OnCommitTransaction();

			dbContextTransaction?.Commit();

			markedForCommit = true;
		}

		/// <summary>
		/// Marks the transaction as valid for commit.
		/// Actual committing takes place when all nested transactions are
		/// disposed and marked as committed.
		/// If this method has not been called when 
		/// method <see cref="IDisposable.Dispose"/> is invoked, the
		/// transaction is marked for rollback. A <see cref="IDomainContainer.SaveChangesAsync()"/>
		/// call is implied calling this method when the transaction
		/// is not marked for rollback.
		/// </summary>
		public async Task CommitAsync()
		{
			EnsureNotDisposed();

			await this.domainContainer.OnCommitTransactionAsync();

			dbContextTransaction?.Commit();

			markedForCommit = true;
		}

		/// <summary>
		/// Fired when the whole transaction is committed successfully.
		/// </summary>
		public event Action Succeeding;

		/// <summary>
		/// Fired when the whole transaction is committed successfully.
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

			if (dbContextTransaction != null)
			{
				dbContextTransaction.Dispose();
				dbContextTransaction = null;
			}

			domainContainer = null;
		}

		#endregion

		#region Internal methods

		internal void OnSucceeding()
		{
			if (this.Succeeding != null) this.Succeeding();
		}

		internal void OnRollingBack()
		{
			if (this.RollingBack != null) this.RollingBack();
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
