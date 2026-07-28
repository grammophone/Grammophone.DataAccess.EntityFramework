using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Entity Framework implementation of the translating query provider.
	/// </summary>
	public class EFTranslatingQueryProvider : TranslatingQueryProvider
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="nativeQueryProvider">The underlying Entity Framework query provider.</param>
		/// <param name="domainContainer">The domain container which owns the query.</param>
		public EFTranslatingQueryProvider(IQueryProvider nativeQueryProvider, IDomainContainer domainContainer) : base(nativeQueryProvider, domainContainer)
		{
		}

		#endregion

		#region Public methods

		/// <inheritdoc/>
		/// <remarks>
		/// Entity Framework 6's LINQ-to-Entities translator rejects an explicit cast of a primitive to
		/// <see cref="object"/> in a member initialization, so after the portable translation this backend
		/// additionally strips such redundant boxing conversions via <see cref="RedundantBoxingNormalizerVisitor"/>.
		/// Entity Framework Core tolerates the box, so this pass is applied only here and not in the base pipeline.
		/// </remarks>
		public override Expression TranslateExpression(Expression expression)
		{
			var translatedExpression = base.TranslateExpression(expression);

			return new RedundantBoxingNormalizerVisitor().Visit(translatedExpression);
		}

		#endregion

		#region Protected methods

		/// <inheritdoc/>
		protected override IEntityQuery WrapNativeQuery(IQueryable nativeQueryable)
		{
			if (nativeQueryable == null) throw new ArgumentNullException(nameof(nativeQueryable));

			return new EFQuery<IQueryable>(nativeQueryable, this.DomainContainer);
		}

		/// <inheritdoc/>
		protected override IEntityQuery<T> WrapNativeQuery<T>(IQueryable<T> nativeQueryable)
		{
			if (nativeQueryable == null) throw new ArgumentNullException(nameof(nativeQueryable));

			return new EFQuery<T, IQueryable<T>>(nativeQueryable, this.DomainContainer);
		}

		#endregion
	}
}
