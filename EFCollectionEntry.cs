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
	/// Represents a collection property by 
	/// implementing <see cref="ICollectionEntry{E, I}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="I">The type of items in the collection.</typeparam>
	public class EFCollectionEntry<E, I> : EFMemberEntry<E, ICollection<I>, DbCollectionEntry<E, I>>, ICollectionEntry<E, I>
		where E : class
		where I : class
	{
		#region Construction

		internal EFCollectionEntry(EFEntityEntry<E> entityEntry, DbCollectionEntry<E, I> underlyingMemberEntry) 
			: base(entityEntry, underlyingMemberEntry)
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Determines or sets whether the relation has been loaded from the database.
		/// </summary>
		public bool IsLoaded
		{
			get => underlyingMemberEntry.IsLoaded;
			set => underlyingMemberEntry.IsLoaded = value;
		}

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
		/// Returns the query that would be used to load the collection from the database.
		/// The returned query can be modified using LINQ to perform filtering 
		/// or operations in the database.
		/// </summary>
		public IQueryable<I> Query() => underlyingMemberEntry.Query();

		#endregion
	}
}
