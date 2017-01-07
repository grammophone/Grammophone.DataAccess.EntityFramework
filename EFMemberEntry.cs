using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Represents an entity member by implementing <see cref="IMemberEntry{E, P}"/>.
	/// </summary>
	/// <typeparam name="E">The type of the entity.</typeparam>
	/// <typeparam name="P">The type of the property.</typeparam>
	/// <typeparam name="U">The type of the Entity Framework implementation for the entry.</typeparam>
	public abstract class EFMemberEntry<E, P, U> : IMemberEntry<E, P>
		where E : class
		where U : DbMemberEntry<E, P>
	{
		#region Protected fields

		/// <summary>
		/// The underlying Entity Framework implementation of the member entry.
		/// </summary>
		protected readonly U underlyingMemberEntry;

		/// <summary>
		/// The entity entry where this member entry belongs.
		/// </summary>
		protected readonly EFEntityEntry<E> entityEntry;

		#endregion

		#region Construction

		internal EFMemberEntry(EFEntityEntry<E> entityEntry, U underlyingMemberEntry)
		{
			if (entityEntry == null) throw new ArgumentNullException(nameof(entityEntry));
			if (underlyingMemberEntry == null) throw new ArgumentNullException(nameof(underlyingMemberEntry));

			this.entityEntry = entityEntry;
			this.underlyingMemberEntry = underlyingMemberEntry;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The current value of the property.
		/// </summary>
		public P CurrentValue
		{
			get
			{
				return underlyingMemberEntry.CurrentValue;
			}
			set
			{
				underlyingMemberEntry.CurrentValue = value;
			}
		}

		/// <summary>
		/// The entity entry where this member entry belongs.
		/// </summary>
		public IEntityEntry<E> EntityEntry => entityEntry;

		/// <summary>
		/// The name of the property.
		/// </summary>
		public string Name => underlyingMemberEntry.Name;

		#endregion
	}
}
