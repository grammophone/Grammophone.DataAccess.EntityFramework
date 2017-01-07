using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Represents a scalar or complex property by 
	/// implementing <see cref="IPropertyEntry{E, P}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="P">The type of the property.</typeparam>
	/// <typeparam name="U">The type of the Entity Framework implementation for the entry.</typeparam>
	public abstract class EFPropertyEntry<E, P, U> : EFMemberEntry<E, P, U>, IPropertyEntry<E, P>
		where E : class
		where U : DbPropertyEntry<E, P>
	{
		#region Construction

		internal EFPropertyEntry(EFEntityEntry<E> entityEntry, U underlyingMemberEntry) 
			: base(entityEntry, underlyingMemberEntry)
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// If true, the property is marked as modified.
		/// </summary>
		public bool IsModified
		{
			get
			{
				return underlyingMemberEntry.IsModified;
			}

			set
			{
				underlyingMemberEntry.IsModified = value;
			}
		}

		/// <summary>
		/// The original value of the property, before any modification.
		/// </summary>
		public P OriginalValue => underlyingMemberEntry.OriginalValue;

		#endregion
	}

	/// <summary>
	/// Represents a scalar or complex entity property by 
	/// implementing <see cref="IPropertyEntry{E, P}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="P">The type of the property.</typeparam>
	public class EFPropertyEntry<E, P> : EFPropertyEntry<E, P, DbPropertyEntry<E, P>>
		where E : class
	{
		internal EFPropertyEntry(EFEntityEntry<E> entityEntry, DbPropertyEntry<E, P> underlyingEntry) 
			: base(entityEntry, underlyingEntry)
		{
		}
	}
}
