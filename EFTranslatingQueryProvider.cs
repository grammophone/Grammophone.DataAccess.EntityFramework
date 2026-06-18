using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	public class EFTranslatingQueryProvider : TranslatingQueryProvider
	{
		public EFTranslatingQueryProvider(IQueryProvider nativeQueryProvider, IDomainContainer domainContainer) : base(nativeQueryProvider, domainContainer)
		{
		}

		protected override IEntityQuery WrapNativeQuery(IQueryable nativeQueryable)
		{
			if (nativeQueryable == null) throw new ArgumentNullException(nameof(nativeQueryable));

			return new EFQuery<IQueryable>(nativeQueryable, this.DomainContainer);
		}

		protected override IEntityQuery<T> WrapNativeQuery<T>(IQueryable<T> nativeQueryable)
		{
			if (nativeQueryable == null) throw new ArgumentNullException(nameof(nativeQueryable));

			return new EFQuery<T, IQueryable<T>>(nativeQueryable, this.DomainContainer);
		}
	}
}
