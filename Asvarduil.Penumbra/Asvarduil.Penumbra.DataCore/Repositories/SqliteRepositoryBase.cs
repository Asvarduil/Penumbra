using System.Data.SQLite;
using System.Collections.Generic;
using Asvarduil.Penumbra.Services;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    /// <summary>
    /// Base class for all repositories that use SQLite technology to interact with a file database.
    /// </summary>
    /// <typeparam name="TModel">Model that the database will operate with</typeparam>
    /// <typeparam name="TMapping">Mapper that allows us to convert SQLite to a model</typeparam>
    public abstract class SqliteRepositoryBase<TModel, TMapping>
        where TModel : class
        where TMapping : ISQLiteModelMapper<TModel>, new()
    {
        #region Variables / Properties

        protected static string ConnectionString => SettingsService.ReadEnvironment("ConnectionString");

        protected static TMapping _mapper;
        protected static TMapping Mapper
        {
            get
            {
                if (_mapper == null)
                    _mapper = new TMapping();

                return _mapper;
            }
        }

        #endregion Variables / Properties

        #region Query Methods

        /// <summary>
        /// Runs the given SQL command against SQLite with the given parameters
        /// </summary>
        /// <param name="query">Query to run against SQLite</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <returns>List of results if found, empty collection if not.</returns>
        protected static IEnumerable<TModel> RunQuery(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the query...
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in parameters)
                            {
                                var commandParam = new SQLiteParameter(parameter.Key, parameter.Value);
                                command.Parameters.Add(commandParam);
                            }
                        }

                        var result = new List<TModel>();

                        using (var resultReader = command.ExecuteReader())
                        {
                            while (resultReader.Read())
                            {
                                TModel model = Mapper.MapResult(resultReader);
                                result.Add(model);
                            }
                        }

                        return result;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected abstract IEnumerable<TModel> RunFileQuery(string query, Dictionary<string, object> parameters);

        #endregion Query Methods

        #region Execute Methods

        /// <summary>
        /// Runs a non-SELECT query against the SQLite database, with the given parameters.
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        protected static void RunExecute(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        if(parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in parameters)
                            {
                                var commandParam = new SQLiteParameter(parameter.Key, parameter.Value);
                                command.Parameters.Add(commandParam);
                            }
                        }

                        int result = command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected abstract void RunFileExecute(string query, Dictionary<string, object> parameters);

        #endregion Execute Methods
    }
}
