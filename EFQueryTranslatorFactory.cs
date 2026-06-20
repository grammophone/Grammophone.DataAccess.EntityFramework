using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Grammophone.DataAccess.QueryExtensions;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Factory for the Entity Framework query translator.
	/// </summary>
	public static class EFQueryTranslatorFactory
	{
		#region Private fields

		private static readonly QueryTranslator QueryTranslator = new QueryTranslator(
			new EFTerminalMethodsAdapter(),
			new EFShapingMethodsAdapter(),
			new SetOperationMethodsAdapter(),
			CreateMethodMappings());

		#endregion

		#region Public methods

		/// <summary>
		/// Get the shared Entity Framework query translator.
		/// </summary>
		/// <returns>Returns the shared Entity Framework query translator.</returns>
		public static QueryTranslator GetQueryTranslator()
		{
			return QueryTranslator;
		}

		#endregion

		#region Private methods

		private static IReadOnlyDictionary<MethodInfo, MethodMapping> CreateMethodMappings()
		{
			var mappings = new Dictionary<MethodInfo, MethodMapping>();

			AddMapping(mappings, QueryFunctionsMethodInfos.Like, GetDbFunction(nameof(DbFunctions.Like), typeof(string), typeof(string)));
			AddMapping(mappings, QueryFunctionsMethodInfos.LikeWithEscape, GetDbFunction(nameof(DbFunctions.Like), typeof(string), typeof(string), typeof(string)));
			AddMapping(mappings, QueryFunctionsMethodInfos.TruncateDateTime, GetDbFunction(nameof(DbFunctions.TruncateTime), typeof(DateTime?)));
			AddMapping(mappings, QueryFunctionsMethodInfos.TruncateDateTimeOffset, GetDbFunction(nameof(DbFunctions.TruncateTime), typeof(DateTimeOffset?)));

			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffYearsDateTime, nameof(DbFunctions.DiffYears));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffYearsDateTimeOffset, nameof(DbFunctions.DiffYears));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMonthsDateTime, nameof(DbFunctions.DiffMonths));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMonthsDateTimeOffset, nameof(DbFunctions.DiffMonths));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffDaysDateTime, nameof(DbFunctions.DiffDays));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffDaysDateTimeOffset, nameof(DbFunctions.DiffDays));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffHoursDateTime, nameof(DbFunctions.DiffHours));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffHoursDateTimeOffset, nameof(DbFunctions.DiffHours));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMinutesDateTime, nameof(DbFunctions.DiffMinutes));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMinutesDateTimeOffset, nameof(DbFunctions.DiffMinutes));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffSecondsDateTime, nameof(DbFunctions.DiffSeconds));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffSecondsDateTimeOffset, nameof(DbFunctions.DiffSeconds));
			AddDateTimeDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMillisecondsDateTime, nameof(DbFunctions.DiffMilliseconds));
			AddDateTimeOffsetDiffMapping(mappings, QueryFunctionsMethodInfos.DiffMillisecondsDateTimeOffset, nameof(DbFunctions.DiffMilliseconds));

			AddDateTimeAddMapping(mappings, QueryFunctionsMethodInfos.AddDays, nameof(DbFunctions.AddDays));
			AddDateTimeAddMapping(mappings, QueryFunctionsMethodInfos.AddMonths, nameof(DbFunctions.AddMonths));
			AddDateTimeAddMapping(mappings, QueryFunctionsMethodInfos.AddYears, nameof(DbFunctions.AddYears));

			return mappings;
		}

		private static void AddMapping(
			IDictionary<MethodInfo, MethodMapping> mappings,
			MethodInfo portableMethodInfo,
			MethodInfo nativeMethodInfo)
		{
			mappings.Add(portableMethodInfo, new IsomorphicMethodMapping(portableMethodInfo, nativeMethodInfo));
		}

		private static void AddDateTimeDiffMapping(
			IDictionary<MethodInfo, MethodMapping> mappings,
			MethodInfo portableMethodInfo,
			string nativeMethodName)
		{
			AddMapping(
				mappings,
				portableMethodInfo,
				GetDbFunction(nativeMethodName, typeof(DateTime?), typeof(DateTime?)));
		}

		private static void AddDateTimeOffsetDiffMapping(
			IDictionary<MethodInfo, MethodMapping> mappings,
			MethodInfo portableMethodInfo,
			string nativeMethodName)
		{
			AddMapping(
				mappings,
				portableMethodInfo,
				GetDbFunction(nativeMethodName, typeof(DateTimeOffset?), typeof(DateTimeOffset?)));
		}

		private static void AddDateTimeAddMapping(
			IDictionary<MethodInfo, MethodMapping> mappings,
			MethodInfo portableMethodInfo,
			string nativeMethodName)
		{
			AddMapping(
				mappings,
				portableMethodInfo,
				GetDbFunction(nativeMethodName, typeof(DateTime?), typeof(int?)));
		}

		private static MethodInfo GetDbFunction(string methodName, params Type[] parameterTypes)
		{
			return MethodInfoCatalog.GetMethodInfo(typeof(DbFunctions), methodName, parameterTypes);
		}

		#endregion
	}
}
