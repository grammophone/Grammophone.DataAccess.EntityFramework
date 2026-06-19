using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Entity Framework implementation of query shaping methods.
	/// </summary>
	public class EFShapingMethodsAdapter : ShapingMethodsAdapter
	{
		#region Public methods

		/// <inheritdoc/>
		public override IQueryable<T> Include<T>(IQueryable<T> nativeQuery, string path)
		{
			if (nativeQuery == null) throw new ArgumentNullException(nameof(nativeQuery));
			if (path == null) throw new ArgumentNullException(nameof(path));

			return nativeQuery.Include(path);
		}

		/// <inheritdoc/>
		public override IQueryable<T> Include<T, TProperty>(
			IQueryable<T> nativeQuery,
			Expression<Func<T, TProperty>> pathExpression)
		{
			if (nativeQuery == null) throw new ArgumentNullException(nameof(nativeQuery));
			if (pathExpression == null) throw new ArgumentNullException(nameof(pathExpression));

			return nativeQuery.Include(pathExpression);
		}

		/// <inheritdoc/>
		public override IQueryable<T> AsNoTracking<T>(IQueryable<T> nativeQuery)
		{
			if (nativeQuery == null) throw new ArgumentNullException(nameof(nativeQuery));

			return nativeQuery.AsNoTracking();
		}

		#endregion
	}
}
