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

			AddMapping(
				mappings,
				QueryExtensionMethodInfos.IncludeString,
				GetGenericMethodDefinition(
					typeof(System.Data.Entity.QueryableExtensions),
					nameof(System.Data.Entity.QueryableExtensions.Include),
					typeof(IQueryable<>),
					typeof(string)));

			AddMapping(
				mappings,
				QueryExtensionMethodInfos.IncludeExpression,
				GetGenericMethodDefinition(
					typeof(System.Data.Entity.QueryableExtensions),
					nameof(System.Data.Entity.QueryableExtensions.Include),
					typeof(IQueryable<>),
					typeof(Expression<>)));

			AddMapping(
				mappings,
				QueryExtensionMethodInfos.AsNoTracking,
				GetGenericMethodDefinition(
					typeof(System.Data.Entity.QueryableExtensions),
					nameof(System.Data.Entity.QueryableExtensions.AsNoTracking),
					typeof(IQueryable<>)));

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
			return GetMethod(typeof(DbFunctions), methodName, parameterTypes);
		}

		private static MethodInfo GetGenericMethodDefinition(
			Type declaringType,
			string methodName,
			params Type[] parameterTypeDefinitions)
		{
			var methodInfo = GetMethod(
				declaringType,
				methodName,
				methodInfoCandidate => methodInfoCandidate.IsGenericMethodDefinition,
				parameterTypeDefinitions);

			return methodInfo;
		}

		private static MethodInfo GetMethod(
			Type declaringType,
			string methodName,
			params Type[] parameterTypes)
		{
			return GetMethod(
				declaringType,
				methodName,
				methodInfo => !methodInfo.IsGenericMethod,
				parameterTypes);
		}

		private static MethodInfo GetMethod(
			Type declaringType,
			string methodName,
			Func<MethodInfo, bool> methodPredicate,
			Type[] parameterTypes)
		{
			var methodInfo = TryGetMethod(declaringType, methodName, methodPredicate, parameterTypes);

			if (methodInfo == null)
			{
				throw new InvalidOperationException(
					$"Method '{methodName}' with the requested signature was not found in type '{declaringType.FullName}'.");
			}

			return methodInfo;
		}

		private static MethodInfo TryGetMethod(
			Type declaringType,
			string methodName,
			Func<MethodInfo, bool> methodPredicate,
			Type[] parameterTypes)
		{
			foreach (var methodInfo in declaringType.GetMethods(BindingFlags.Public | BindingFlags.Static))
			{
				if (methodInfo.Name != methodName || !methodPredicate(methodInfo)) continue;

				var parameters = methodInfo.GetParameters();

				if (parameters.Length != parameterTypes.Length) continue;

				if (parameters.Select(p => NormalizeParameterType(p.ParameterType)).SequenceEqual(parameterTypes))
				{
					return methodInfo;
				}
			}

			return null;
		}

		private static Type NormalizeParameterType(Type parameterType)
		{
			if (parameterType.IsGenericType)
			{
				return parameterType.GetGenericTypeDefinition();
			}

			return parameterType;
		}

		#endregion
	}
}
