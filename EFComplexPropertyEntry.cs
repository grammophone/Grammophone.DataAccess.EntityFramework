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
	/// Represents a complex property by 
	/// implementing <see cref="IComplexPropertyEntry{E, P}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="P">The type of the property.</typeparam>
	public class EFComplexPropertyEntry<E, P> : EFPropertyEntry<E, P, DbComplexPropertyEntry<E, P>>, IComplexPropertyEntry<E, P>
		where E : class
	{
		#region Construction

		internal EFComplexPropertyEntry(EFEntityEntry<E> entityEntry, DbComplexPropertyEntry<E, P> underlyingMemberEntry) 
			: base(entityEntry, underlyingMemberEntry)
		{
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Gets an object that represents a scalar or complex subproperty of this complex property.
		/// </summary>
		/// <typeparam name="N">The type of the subproperty.</typeparam>
		/// <param name="subpropertySelector">An expression representing the subproperty.</param>
		/// <returns>Returns an object representing the subproperty.</returns>
		public IComplexPropertyEntry<E, N> ComplexProperty<N>(Expression<Func<P, N>> subpropertySelector)
		{
			if (subpropertySelector == null) throw new ArgumentNullException(nameof(subpropertySelector));

			var underlyingSubentry = underlyingMemberEntry.ComplexProperty(subpropertySelector);

			if (underlyingSubentry != null)
				return new EFComplexPropertyEntry<E, N>(entityEntry, underlyingSubentry);
			else
				return null;
		}

		/// <summary>
		/// Gets an object that represents a complex subproperty of this complex property.
		/// </summary>
		/// <typeparam name="N">The type of the subproperty.</typeparam>
		/// <param name="subpropertySelector">An expression representing the subproperty.</param>
		/// <returns>Returns an object representing the subproperty.</returns>
		public IPropertyEntry<E, N> Property<N>(Expression<Func<P, N>> subpropertySelector)
		{
			if (subpropertySelector == null) throw new ArgumentNullException(nameof(subpropertySelector));

			var underlyingSubentry = underlyingMemberEntry.Property(subpropertySelector);

			if (underlyingSubentry != null)
				return new EFPropertyEntry<E, N>(entityEntry, underlyingSubentry);
			else
				return null;
		}

		#endregion
	}
}
