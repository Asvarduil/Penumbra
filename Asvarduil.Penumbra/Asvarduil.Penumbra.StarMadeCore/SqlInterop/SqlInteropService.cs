using System;
using System.Collections.Generic;
using System.IO;

namespace Asvarduil.Penumbra.StarMadeCore
{
    // Programmer's Note:
    // ------------------
    // /sql_query "QUERY" is a way to run a query through the
    // starmade chat interface to get specific bits of data.
    //
    // In and of itself, it's kind of a pain.  So, this service
    // will help provide a way to create reliable queries, while
    // also having a way to identify the results of a query, since
    // SQL query can take time to execute.

    public class SqlInteropService
    {
        #region Query Stack

        /// <summary>
        /// Internal list of queued up Sql Queries that have not yet been processed
        /// by StarMade.
        /// </summary>
        private static List<SqlQuery> QueuedQueries = new List<SqlQuery>();

        #endregion Query Stack

        #region Query Methods

        /// <summary>
        /// Generates a Starmade /sql_query command based on a SqlQuery object,
        /// and registers it with MicroMade's internal queue.
        /// </summary>
        /// <param name="query">Query object to generate a command for.</param>
        /// <returns>StarMade chat command to execute a sql_query</returns>
        private static string GenerateSqlQueryCommand(SqlQuery query)
        {
            query.IsQuery = true;
            QueuedQueries.Add(query);

            string command = $"/sql_query \"{query.FullCommand}\"";
            return command;
        }

        /// <summary>
        /// Generates a Starmade /sql_execute command based on a SqlQuery object,
        /// and registers it with MicroMade's internal queue.
        /// </summary>
        /// <param name="query">Query object to generate a command for.</param>
        /// <returns>StarMade chat command to execute a sql_execute</returns>
        private static string GenerateSqlExecuteCommand(SqlQuery query)
        {
            query.IsQuery = false;
            QueuedQueries.Add(query);

            string command = $"/sql_execute \"{query.FullCommand}\"";
            return command;
        }

        /// <summary>
        /// Produces a GUID-identifiable SqlQuery object used by the SQL query
        /// queue system.
        /// </summary>
        /// <param name="fileName">File of the query to load.</param>
        /// <returns>SQL Query object.</returns>
        private static SqlQuery GenerateQueryFromFile(string fileName)
        {
            string query = File.ReadAllText(fileName);
            var queryObject = new SqlQuery
            {
                Command = query,
                GUID = new Guid()
            };

            return queryObject;
        }

        /// <summary>
        /// Loads a query from a file, and generates a SqlQuery
        /// object for MicroMade to use.
        /// </summary>
        /// <param name="fileName">File to load a query from.</param>
        /// <returns>StarMade command to perform a SQL Query based on a file's contents.</returns>
        public static string IssueQueryFromFile(string fileName)
        {
            SqlQuery queryObject = GenerateQueryFromFile(fileName);

            string resultingCommand = GenerateSqlQueryCommand(queryObject);
            return resultingCommand;
        }

        /// <summary>
        /// Loads a query from a file, and generates a SqlQuery
        /// object for MicroMade to use.
        /// </summary>
        /// <param name="fileName">File to load a query from.</param>
        /// <returns>StarMade command to perform a SQL UPDATE, INSERT, DELETE, CREATE, or ALTER based on a file's contents.</returns>
        public static string ExecuteFromFile(string fileName)
        {
            SqlQuery queryObject = GenerateQueryFromFile(fileName);

            string resultingCommand = GenerateSqlExecuteCommand(queryObject);
            return resultingCommand;
        }

        /// <summary>
        /// Based on response text from the StarMade server, determines for a given query if any useful information
        /// was returned, and if the query can be safely removed from the working SQL Query queue.
        /// </summary>
        /// <typeparam name="TModel">Type of the data to resolve</typeparam>
        /// <param name="responseString">Data from the StarMade server</param>
        /// <returns>Fully hydrated object based on the return value from the server.</returns>
        public static TModel ResolveQuery<TModel>(string responseString)
            where TModel : class
        {
            // TODO: Resolve GUID
            // string resolvedGUID = (Magic that squesters the GUID from the query?)
            // SqlQuery query = QueuedQueries.FirstOrDefault(q => q.GUID.ToString() == resolvedGUID);
            // if(query == null)
            //    return null;

            // TODO: Depending on response text from StarMade,
            //       determine if the Query can be popped from
            //       the queue.

            // If the query returned data, then resolve that to a model.

            TModel result;  // TODO: Mapper?
            // TODO: Map the stuff to the model's properties?
            return null;
        }

        #endregion Query Methods
    }
}
