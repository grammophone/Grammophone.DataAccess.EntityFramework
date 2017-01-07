using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Represents a reference property by 
	/// implementing <see cref="IReferenceEntry{E, P}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="P">The type of the property.</typeparam>
	public class EFReferenceEntry<E, P> : EFMemberEntry<E, P, DbReferenceEntry<E, P>>, IReferenceEntry<E, P>
		where E : class
	{
		#region Construction

		internal EFReferenceEntry(EFEntityEntry<E> entityEntry, DbReferenceEntry<E, P> underlyingMemberEntry) 
			: base(entityEntry, underlyingMemberEntry)
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// If true, the relation has been completely loaded from the database.
		/// </summary>
		public bool IsLoaded => underlyingMemberEntry.IsLoaded;

		#endregion

		#region Public methods

		/// <summary>
		/// Loads the relation from the database. 
		/// Note that entities that already exist in the <see cref="IDomainContainer"/> 
		/// are not overwritten with values from the database.
		/// </summary>
		public void Load()
		{
			underlyingMemberEntry.Load();
		}

		/// <summary>
		/// Asynchronously loads the relation from the database. 
		/// Note that entities that already exist in the <see cref="IDomainContainer"/> 
		/// are not overwritten with values from the database.
		/// </summary>
		public async Task LoadAsync()
		{
			await underlyingMemberEntry.LoadAsync();
		}

		/// <summary>
		/// Asynchronously loads the relation from the database. 
		/// Note that entities that already exist in the <see cref="IDomainContainer"/> 
		/// are not overwritten with values from the database.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
		public async Task LoadAsync(CancellationToken cancellationToken)
		{
			await underlyingMemberEntry.LoadAsync(cancellationToken);
		}

		/// <summary>
		/// Returns the query that would be used to load this entity from the database.
		/// The returned query can be modified using LINQ to perform filtering 
		/// or operations in the database.
		/// </summary>
		public IQueryable<P> Query() => underlyingMemberEntry.Query();

		#endregion
	}
}
