using Devsoft.Core.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Devsoft.Core.Helper
{
    /// <summary>
    /// Clase que permite manejar las ejecución de métodos ADO.NET
    /// </summary>
    /// <remarks>
    /// Autor: Mark Anthony Arroyo Garcia
    /// Versión: 1.6.0
    /// Fecha: 03/01/2018
    /// REPLACE:    SqlDataReader   -   HanaDataReader
    ///             SqlConnection   -   HanaConnection
    ///             SqlCommand      -   HanaCommand
    ///             SqlParameter    -   HanaParameter
    ///             SqlTransaction  -   HanaTransaction
    ///             SqlDataAdapter  -   HanaDataAdapter
    ///             SqlException    -   HanaException
    /// </remarks>
    public class SqlDatabaseHelper
    {
        public delegate T RowMapper<T>(SqlDataReader dataReader) where T : class;

        #region Methods Private
        private static SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constantes.CNX_STRING;
            connection.Open();
            return connection;
        }

        private static void CloseConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private static void CloseCommand(SqlCommand command)
        {
            if (command != null)
            {
                command.Parameters.Clear();
                command.Dispose();
            }
        }

        private static void CloseDataReader(SqlDataReader dataReader)
        {
            if (dataReader != null)
            {
                dataReader.Close();
                dataReader.Dispose();
            }
        }

        private static void CloseDataAdapter(SqlDataAdapter dataAdapter)
        {
            if (dataAdapter != null)
            {
                dataAdapter.Dispose();
            }
        }

        private static SqlCommand PrepareCommand(string commandText
            , SqlConnection connection
            , CommandType cmdType = CommandType.StoredProcedure
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , SqlTransaction transaction = null)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = cmdType;
            command.CommandText = commandText;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (parameter != null)
            {
                if (parameters == null) { parameters = new List<SqlParameter>(); }
                parameters.Add(parameter);
            }

            if (parameters != null)
            {
                foreach (SqlParameter x in parameters)
                {
                    command.Parameters.Add(x);
                }
            }

            return command;
        }
        #endregion

        #region Public Methods RowMapper
        public static T ExecuteToEntity<T>(string commandText
            , RowMapper<T> rowMapper
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure) where T : class
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;

            T entity = null;
            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    entity = rowMapper(dataReader);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                CloseDataReader(dataReader);
                CloseCommand(command);
                CloseConnection(connection);
            }

            return entity;
        }

        public static List<T> ExecuteToList<T>(string commandText
            , RowMapper<T> rowMapper
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure) where T : class
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;

            List<T> listEntity = new List<T>();
            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    T entity = rowMapper(dataReader);
                    listEntity.Add(entity);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                CloseDataReader(dataReader);
                CloseCommand(command);
                CloseConnection(connection);
            }

            return listEntity;
        }
        #endregion

        public static T ExecuteScalar<T>(string commandText
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            T result;

            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                result = (T)command.ExecuteScalar();
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                CloseCommand(command);
                CloseConnection(connection);
            }

            return result;
        }

        public static void Execute(string commandText
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , Dictionary<string, object> paramsOutputValue = null
            , CommandType type = CommandType.StoredProcedure
            , SqlConnection connection = null
            , SqlTransaction transaction = null)
        {
            SqlCommand command = null;

            try
            {
                if (connection == null) { connection = OpenConnection(); }

                command = PrepareCommand(commandText, connection, type, parameter, parameters, transaction);
                command.ExecuteNonQuery();
                if (parameter != null || parameters != null)
                {
                    foreach (SqlParameter x in parameters)
                    {
                        if (x.Direction == ParameterDirection.Output)
                        {
                            paramsOutputValue.Add(x.ParameterName, command.Parameters[x.ParameterName].Value);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                CloseCommand(command);
                if (transaction == null) { CloseConnection(connection); }

            }
        }

        public static void BeginTransaction(ref SqlConnection sqlConnection, ref SqlTransaction sqlTransaction)
        {
            sqlConnection = OpenConnection();
            sqlTransaction = sqlConnection.BeginTransaction();
        }
        public static void EndTransaction(ref SqlConnection sqlConnection, ref SqlTransaction sqlTransaction)
        {
            if (sqlTransaction != null) { sqlTransaction.Dispose(); }
            CloseConnection(sqlConnection);
        }

        public static DataSet GetDataSet(string commandText
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                CloseDataAdapter(da);
                CloseCommand(command);
                CloseConnection(connection);
            }

            return ds;
        }
    }
}
