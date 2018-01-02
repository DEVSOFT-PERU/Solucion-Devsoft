using Devsoft.Core.Util;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Devsoft.Core.Helper
{
    /// <summary>
    /// Clase que permite manejar las ejecución de métodos ADO.NET
    /// </summary>
    /// <remarks>
    /// Autor: Mark Anthony Arroyo Garcia
    /// Versión: 1.5.0
    /// Fecha: 31/12/2017
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

        private static SqlCommand PrepareCommand(string commandText
            , SqlConnection connection
            , CommandType cmdType = CommandType.StoredProcedure
            , SqlParameter parameter = null
            , List<SqlParameter> parameters = null)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = cmdType;
            command.CommandText = commandText;

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
            , CommandType type = CommandType.StoredProcedure)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                command.ExecuteNonQuery();
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
        }

        public static void ExecuteOutput(string commandText
             , List<string> paramsOutput
             , List<object> paramsOutputValue
             , SqlParameter parameter = null
             , List<SqlParameter> parameters = null
             , CommandType type = CommandType.StoredProcedure)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                command.ExecuteNonQuery();

                for (var i = 0; i < paramsOutput.Count; i++)
                {
                    paramsOutputValue[i] = command.Parameters[paramsOutput[i]].Value;
                }

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
        }
    }
}
