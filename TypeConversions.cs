using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Used to convert from and to Entity Framework types.
	/// </summary>
	internal static class TypeConversions
	{
		/// <summary>
		/// Convert from <see cref="EntityState"/> to <see cref="TrackingState"/>.
		/// </summary>
		public static TrackingState EntityStateToTrackingState(EntityState entityState)
			=> (TrackingState)entityState;

		/// <summary>
		/// Convert from <see cref="TrackingState"/> to <see cref="EntityState"/>.
		/// </summary>
		public static EntityState TrackingStateToEntityState(TrackingState trackingState)
			=> (EntityState)trackingState;
	}
}
