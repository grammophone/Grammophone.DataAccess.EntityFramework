using System;
using System.Collections;
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
	/// Non-generic implementation of <see cref="IEntityQuery"/> using Entity Framework.
	/// </summary>
	/// <typeparam name="Q">The type of the Entity Framework query object.</typeparam>
	public class EFQuery<Q> : IEntityQuery
		where Q : IQueryable
	{
		#region Private fields

		private TranslatingQueryProvider translatingProvider;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="nativeQuery">The entity framework query object.</param>
		/// <param name="domainContainer">The domain container which the query pertains to.</param>
		public EFQuery(Q nativeQuery, IDomainContainer domainContainer)
		{
			if (nativeQuery == null) throw new ArgumentNullException("dbQuery");
			if (domainContainer == null) throw new ArgumentNullException(nameof(domainContainer));

			this.NativeQuery = nativeQuery;
			this.DomainContainer = domainContainer;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The underlying Entity Framework query object.
		/// </summary>
		public Q NativeQuery { get; }

		#endregion

		#region IEntityQuery<E> Members

		/// <inheritdoc/>
		IQueryable IEntityQuery.NativeQuery => this.NativeQuery;

		/// <inheritdoc/>
		public IDomainContainer DomainContainer { get; }

		/// <inheritdoc/>
		public IQueryProvider NativeProvider => NativeQuery.Provider;

		/// <summary>
		/// The translating provider associated with this query.
		/// </summary>
		public TranslatingQueryProvider TranslatingProvider
		{
			get
			{
				return translatingProvider ??= new EFTranslatingQueryProvider(this.NativeProvider, this.DomainContainer);
			}
		}

		#endregion

		#region Explicit IQueryable implementation

		IQueryProvider IQueryable.Provider
		{
			get
			{
				return translatingProvider ??= new EFTranslatingQueryProvider(this.NativeProvider, this.DomainContainer);
			}
		}

		Type IQueryable.ElementType => NativeQuery.ElementType;

		System.Linq.Expressions.Expression IQueryable.Expression => NativeQuery.Expression;

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			var translatedExpression = this.TranslatingProvider.TranslateExpression(NativeQuery.Expression);

			var translatedQuery = this.NativeProvider.CreateQuery(translatedExpression);

			return translatedQuery.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// Implementation of <see cref="IEntityQuery{E}"/> using
	/// Entity Framework.
	/// </summary>
	/// <typeparam name="E">
	/// The type of the entities.
	/// </typeparam>
	/// <typeparam name="Q">
	/// The type of the Entity Framework query object.
	/// Must be derived from <see cref="DbQuery{E}"/>.
	/// </typeparam>
	public class EFQuery<E, Q> : EFQuery<Q>, IEntityQuery<E>, IOrderedQueryable<E>
		where Q : IQueryable<E>
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="dbQuery">The entity framework query object.</param>
		/// <param name="domainContainer">The domain container which the query pertains to.</param>
		public EFQuery(Q dbQuery, IDomainContainer domainContainer) : base(dbQuery, domainContainer)
		{
		}

		#endregion

		#region IEnumerable<E> implementation

		/// <summary>
		/// Executes the query and obtains an enumerator for the results.
		/// </summary>
		public IEnumerator<E> GetEnumerator()
		{
			var translatedExpression = this.TranslatingProvider.TranslateExpression(NativeQuery.Expression);

			var translatedQuery = this.NativeProvider.CreateQuery<E>(translatedExpression);

			return translatedQuery.GetEnumerator();
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

			return NativeQuery.Equals(other.NativeQuery);
		}

		/// <summary>
		/// The implementation is forwarded to the underlying
		/// Entity Framework query.
		/// </summary>
		public override int GetHashCode()
		{
			return NativeQuery.GetHashCode();
		}

		#endregion
	}
}
