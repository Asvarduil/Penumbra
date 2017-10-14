using System.Data.SqlClient;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Mappers;
using Asvarduil.Penumbra.Services;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public abstract class SqlRepositoryBase<TModel, TMapping>
        where TModel : class
        where TMapping : ISQLModelMapper<TModel>, new()
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

        #region General Methods

        protected static bool DatabaseExists()
        {
            return true;
        }

        #endregion General Methods

        #region Query Methods

        /// <summary>
        /// Runs the given SQL command against Sql with the given parameters
        /// </summary>
        /// <param name="query">Query to run against Sql</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <returns>List of results if found, empty collection if not.</returns>
        protected static IEnumerable<TModel> RunQuery(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the query...
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in parameters)
                            {
                                var commandParam = new SqlParameter(parameter.Key, parameter.Value);
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

                            resultReader.Close();
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
        /// Runs a non-SELECT query against the Sql database, with the given parameters.
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        protected static void RunExecute(string query, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in parameters)
                            {
                                var commandParam = new SqlParameter(parameter.Key, parameter.Value);
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
