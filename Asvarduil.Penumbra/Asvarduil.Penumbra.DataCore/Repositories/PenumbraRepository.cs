using System.IO;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    /// <summary>
    /// Base class for all Penumbra repositories.  Provides a single layer where the access
    /// technology can be defined (e.g. SQLite, SQL, others), and provides methods by which
    /// to run SQL queries from files.
    /// </summary>
    /// <typeparam name="TModel">Model that will be returned by queries</typeparam>
    /// <typeparam name="TMapping">Mapper that will be used to interpret outputs.</typeparam>
    public class PenumbraRepository<TModel, TMapping> : SqliteRepositoryBase<TModel, TMapping>
        where TModel : class
        where TMapping : ISQLiteModelMapper<TModel>, new()
    {
        #region General Methods

        /// <summary>
        /// Re-generates the database based on the configure database SQL file.
        /// </summary>
        public static void ConfigureDatabase()
        {
            string query = File.ReadAllText("Queries/ConfigureDatabase.sql");
            RunExecute(query, null);
        }

        /// <summary>
        /// Runs the query contained in the given filename.
        /// </summary>
        /// <param name="fileName">Filename to run</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <returns>List of returned models if found, otherwise empty collection.</returns>
        protected override IEnumerable<TModel> RunFileQuery(string fileName, Dictionary<string, object> parameters)
        {
            string query = File.ReadAllText(fileName);
            return RunQuery(query, parameters);
        }

        /// <summary>
        /// Runs the contents of a file with non-SELECT queries against the database, with the given parameters.
        /// </summary>
        /// <param name="fileName">File with query to execute.</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        protected override void RunFileExecute(string fileName, Dictionary<string, object> parameters)
        {
            string query = File.ReadAllText(fileName);
            RunExecute(query, parameters);
        }

        #endregion General Methods
    }
}
