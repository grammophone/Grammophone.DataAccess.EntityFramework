using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Entity Framework terminal methods adapter.
	/// </summary>
	public class EFTerminalMethodsAdapter : DefaultTerminalMethodsAdapter
	{
		#region Public methods

		/// <inheritdoc/>
		public override Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> AllAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<bool> AllAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AllAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<bool> AnyAsync<T>(IQueryable<T> query)
			=> AnyAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AnyAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> AnyAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AnyAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<int> CountAsync<T>(IQueryable<T> query)
			=> CountAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.CountAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> CountAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.CountAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<long> LongCountAsync<T>(IQueryable<T> query)
			=> LongCountAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long> LongCountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.LongCountAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<long> LongCountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> LongCountAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long> LongCountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.LongCountAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> FirstAsync<T>(IQueryable<T> query)
			=> FirstAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> FirstAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.FirstAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> FirstAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> FirstAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> FirstAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.FirstAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
			=> FirstOrDefaultAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> FirstOrDefaultAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.FirstOrDefaultAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> SingleAsync<T>(IQueryable<T> query)
			=> SingleAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> SingleAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SingleAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> SingleAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> SingleAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> SingleAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SingleAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
			=> SingleOrDefaultAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SingleOrDefaultAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate)
			=> SingleOrDefaultAsync(query, predicate, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SingleOrDefaultAsync(query, predicate, cancellationToken);

		/// <inheritdoc/>
		public override Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
			=> ToArrayAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.ToArrayAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<List<T>> ToListAsync<T>(IQueryable<T> query)
			=> ToListAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.ToListAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(IQueryable<T> query, Func<T, TKey> keySelector)
			=> ToDictionaryAsync(query, keySelector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(IQueryable<T> query, Func<T, TKey> keySelector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.ToDictionaryAsync(query, keySelector, cancellationToken);

		/// <inheritdoc/>
		public override Task<Dictionary<TKey, TValue>> ToDictionaryAsync<T, TKey, TValue>(IQueryable<T> query, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
			=> ToDictionaryAsync(query, keySelector, valueSelector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<Dictionary<TKey, TValue>> ToDictionaryAsync<T, TKey, TValue>(IQueryable<T> query, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.ToDictionaryAsync(query, keySelector, valueSelector, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> MinAsync<T>(IQueryable<T> query)
			=> MinAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> MinAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.MinAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<TResult> MinAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
			=> MinAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<TResult> MinAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.MinAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<T> MaxAsync<T>(IQueryable<T> query)
			=> MaxAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<T> MaxAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.MaxAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<TResult> MaxAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
			=> MaxAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<TResult> MaxAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.MaxAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<int> SumAsync(IQueryable<int> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int> SumAsync(IQueryable<int> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<int?> SumAsync(IQueryable<int?> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int?> SumAsync(IQueryable<int?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<long> SumAsync(IQueryable<long> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long> SumAsync(IQueryable<long> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<long?> SumAsync(IQueryable<long?> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long?> SumAsync(IQueryable<long?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<float> SumAsync(IQueryable<float> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float> SumAsync(IQueryable<float> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<float?> SumAsync(IQueryable<float?> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float?> SumAsync(IQueryable<float?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> SumAsync(IQueryable<double> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> SumAsync(IQueryable<double> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> SumAsync(IQueryable<double?> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> SumAsync(IQueryable<double?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal> SumAsync(IQueryable<decimal> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal> SumAsync(IQueryable<decimal> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal?> SumAsync(IQueryable<decimal?> query)
			=> SumAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal?> SumAsync(IQueryable<decimal?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<int> SumAsync<T>(IQueryable<T> query, Expression<Func<T, int>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int> SumAsync<T>(IQueryable<T> query, Expression<Func<T, int>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<int?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, int?>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<int?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, int?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<long> SumAsync<T>(IQueryable<T> query, Expression<Func<T, long>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long> SumAsync<T>(IQueryable<T> query, Expression<Func<T, long>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<long?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, long?>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<long?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, long?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<float> SumAsync<T>(IQueryable<T> query, Expression<Func<T, float>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float> SumAsync<T>(IQueryable<T> query, Expression<Func<T, float>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<float?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, float?>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, float?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> SumAsync<T>(IQueryable<T> query, Expression<Func<T, double>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> SumAsync<T>(IQueryable<T> query, Expression<Func<T, double>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, double?>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, double?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal?>> selector)
			=> SumAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal?> SumAsync<T>(IQueryable<T> query, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.SumAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<int> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<int> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<int?> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<int?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<long> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<long> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<long?> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<long?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<float> AverageAsync(IQueryable<float> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float> AverageAsync(IQueryable<float> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<float?> AverageAsync(IQueryable<float?> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float?> AverageAsync(IQueryable<float?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<double> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync(IQueryable<double> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<double?> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync(IQueryable<double?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal> AverageAsync(IQueryable<decimal> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal> AverageAsync(IQueryable<decimal> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal?> AverageAsync(IQueryable<decimal?> query)
			=> AverageAsync(query, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal?> AverageAsync(IQueryable<decimal?> query, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, int>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, int>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, int?>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, int?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, long>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, long>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, long?>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, long?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<float> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, float>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, float>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<float?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, float?>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<float?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, float?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, double>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, double>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, double?>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<double?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, double?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		/// <inheritdoc/>
		public override Task<decimal?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, decimal?>> selector)
			=> AverageAsync(query, selector, default(CancellationToken));

		/// <inheritdoc/>
		public override Task<decimal?> AverageAsync<T>(IQueryable<T> query, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken)
			=> System.Data.Entity.QueryableExtensions.AverageAsync(query, selector, cancellationToken);

		#endregion
	}
}
