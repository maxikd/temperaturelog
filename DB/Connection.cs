using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DB
{
    public class Connection : IDisposable
    {
        private string _connectionString;
        private int _stack;
        private SqlConnection _connection;
        private object _lock = new object();

        public Connection(string connectionString)
        {
            _connectionString = connectionString;
            _stack = 0;
        }

        /// <summary>
        /// Open the conenction to database.
        /// </summary>
        private void Open()
        {
            lock (_lock)
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);
                if (_stack == 0)
                    _connection.Open();

                _stack++;
            }
        }
        /// <summary>
        /// Close the connection to database.
        /// </summary>
        private void Close()
        {
            lock (_lock)
            {
                if (_stack == 1 && _connection != null)
                    _connection.Close();

                _stack--;
            }
        }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            this.Close();
            _connection.Dispose();
            _connection = null;
        }

        /// <summary>
        /// Create a parameter.
        /// </summary>
        /// <param name="name">Parameter's name.</param>
        /// <param name="value">Parameter's value.</param>
        private SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }
        /// <summary>
        /// Create a command.
        /// </summary>
        /// <param name="query">The Sql query.</param>
        /// <param name="parameters">Command's parameters in pairs.</param>
        private SqlCommand CreateCommand(StringBuilder query, params object[] parameters)
        {
            if (query == null || query.Length == 0) throw new ArgumentNullException(nameof(query), "Query cannot be empty.");
            if (parameters.Length % 2 != 0) throw new ArgumentException("The parameters are invalid. They need to have a parameter name and parameter value.", nameof(parameters));

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query.ToString();
            cmd.Parameters.Clear();

            for (int i = 0; i < parameters.Length; i++)
                cmd.Parameters.Add(this.CreateParameter(parameters[i].ToString(), parameters[++i]));

            return cmd;
        }
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="parameters">Query's parameters.</param>
        /// <returns>Affected rows.</returns>
        public int ExecuteNonQuery(StringBuilder query, params object[] parameters)
        {
            try
            {
                this.Open();
                using (SqlCommand cmd = this.CreateCommand(query, parameters))
                    return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Close();
            }
        }
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="parameters">Query's parameters.</param>
        /// <returns>One single result.</returns>
        public object ExecuteScalar(StringBuilder query, params object[] parameters)
        {
            try
            {
                this.Open();
                using (SqlCommand cmd = this.CreateCommand(query, parameters))
                    return cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Close();
            }
        }
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="query">Sql query.</param>
        /// <param name="readerFunc">Func to execute the reader and create object.</param>
        /// <param name="parameters">Query's parameters.</param>
        /// <returns>The result object.</returns>
        public T ExecuteReader<T>(StringBuilder query, Func<SqlDataReader, T> readerFunc, params object[] parameters)
        {
            try
            {
                this.Open();

                using (SqlCommand cmd = this.CreateCommand(query, parameters))
                {
                    var reader = cmd.ExecuteReader();
                    return readerFunc(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Close();
            }
        }
    }
}
