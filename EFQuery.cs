using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Implementatin of <see cref="IEntityQuery{E}"/> using
	/// Entity Framework.
	/// </summary>
	/// <typeparam name="E">
	/// The type of the entities.
	/// </typeparam>
	/// <typeparam name="Q">
	/// The type of the Entity Framework query object.
	/// Must be derived from <see cref="DbQuery{E}"/>.
	/// </typeparam>
	public class EFQuery<E, Q> : IEntityQuery<E>
		where E : class
		where Q : DbQuery<E>
	{
		#region Protected fields

		/// <summary>
		/// The underlying Entity Framework query object.
		/// </summary>
		protected readonly Q dbQuery;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="dbQuery">The entity framework query object.</param>
		/// <param name="domainContainer">The domain container which the query pertains to.</param>
		public EFQuery(Q dbQuery, IDomainContainer domainContainer)
		{
			if (dbQuery == null) throw new ArgumentNullException("dbQuery");
			if (domainContainer == null) throw new ArgumentNullException(nameof(domainContainer));

			this.dbQuery = dbQuery;
			this.DomainContainer = domainContainer;
		}

		#endregion

		#region IEntityQuery<E> Members

		/// <inheritdoc/>
		public IDomainContainer DomainContainer { get; }

		/// <inheritdoc/>
		public IQueryProvider NativeProvider => ((IQueryable)dbQuery).Provider;

		/// <summary>
		/// Returns a new query where the entities returned will not be cached in the
		/// container.
		/// </summary>
		/// <returns>A new query with NoTracking applied.</returns>
		public IEntityQuery<E> AsNoTracking()
		{
			return new EFQuery<E, DbQuery<E>>(dbQuery.AsNoTracking(), this.DomainContainer);
		}

		/// <summary>
		/// Specifies the related objects to include in the query results.
		/// </summary>
		/// <param name="path">
		/// The dot-separated list of related objects to return in the query results.
		/// </param>
		/// <returns>
		/// A new <see cref="IEntityQuery{E}"/>> with the defined query path.
		/// </returns>
		public IEntityQuery<E> Include(string path)
		{
			return new EFQuery<E, DbQuery<E>>(dbQuery.Include(path), this.DomainContainer);
		}

		/// <summary>
		/// Specifies the related objects to include in the query results.
		/// </summary>
		/// <typeparam name="P">The type of navigation property being included.</typeparam>
		/// <param name="pathExpression">A lambda expression representing the path to include.</param>
		/// <returns>A new <see cref="IQueryable{E}"/> with the defined query path.</returns>
		public IQueryable<E> Include<P>(Expression<Func<E, P>> pathExpression)
		{
			return dbQuery.Include(pathExpression);
		}

		#endregion

		#region IEnumerable<E> Members

		/// <summary>
		/// Executes the query and obtains an enumerator for the results.
		/// </summary>
		public IEnumerator<E> GetEnumerator()
		{
			return ((IEnumerable<E>)dbQuery).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<E>)dbQuery).GetEnumerator();
		}

		#endregion

		#region IQueryable Members

		Type IQueryable.ElementType
		{
			get { return ((IQueryable)dbQuery).ElementType; }
		}

		System.Linq.Expressions.Expression IQueryable.Expression
		{
			get { throw new NotImplementedException(); }
		}

		IQueryProvider IQueryable.Provider
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// The implementation is forwarded to the underlying
		/// Entity Framework query.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as EFQuery<E, Q>;

			if (other == null) return false;

			return dbQuery.Equals(other.dbQuery);
		}

		/// <summary>
		/// The implementation is forwarded to the underlying
		/// Entity Framework query.
		/// </summary>
		public override int GetHashCode()
		{
			return dbQuery.GetHashCode();
		}

		#endregion
	}
}
