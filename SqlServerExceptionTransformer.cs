using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.DataAccess.EntityFramework
{
	/// <summary>
	/// Translates <see cref="DbUpdateException"/>s to descendans
	/// of <see cref="DataAccessException"/> when the data provider is SQL Server.
	/// </summary>
	public class SqlServerExceptionTransformer : IExceptionTransformer
	{
		#region Public methods

		/// <summary>
		/// Transform an exception from the database provider.
		/// </summary>
		/// <param name="dbException">The exception thrown from the database provider.</param>
		/// <returns>Returns the transformed exception.</returns>
		public DataAccessException TranslateDbException(DbException dbException)
		{
			var sqlException = dbException as SqlException;

			if (sqlException == null)
				return new DataAccessException(dbException.Message, dbException);

			var errors = sqlException.Errors.OfType<SqlError>().ToArray();

			if (errors.Any(e => e.Number == 2601 || e.Number == 2627))
				return new UniqueConstraintViolationException(sqlException);

			if (errors.Any(e => e.Number == 547))
				return new ReferentialConstraintViolationException(sqlException);

			return new IntegrityViolationException(sqlException);
		}

		#endregion

		#region Private methods

		#endregion
	}
}
