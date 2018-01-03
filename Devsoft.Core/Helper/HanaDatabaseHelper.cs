using Devsoft.Core.Util;
using Sap.Data.Hana;
using System.Collections.Generic;
using System.Data;

namespace Devsoft.Core.Helper
{
    /// <summary>
    /// Clase que permite manejar las ejecución de métodos ADO.NET
    /// </summary>
    /// <remarks>
    /// Autor: Mark Anthony Arroyo Garcia
    /// Versión: 1.6.0
    /// Fecha: 03/01/2018
    /// </remarks>
    public class HanaDatabaseHelper
    {
        public delegate T RowMapper<T>(HanaDataReader dataReader) where T : class;

        #region Methods Private
        private static HanaConnection OpenConnection()
        {
            HanaConnection connection = new HanaConnection();
            connection.ConnectionString = Constantes.CNX_STRING;
            connection.Open();
            return connection;
        }

        private static void CloseConnection(HanaConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private static void CloseCommand(HanaCommand command)
        {
            if (command != null)
            {
                command.Parameters.Clear();
                command.Dispose();
            }
        }

        private static void CloseDataReader(HanaDataReader dataReader)
        {
            if (dataReader != null)
            {
                dataReader.Close();
                dataReader.Dispose();
            }
        }

        private static void CloseDataAdapter(HanaDataAdapter dataAdapter)
        {
            if (dataAdapter != null)
            {
                dataAdapter.Dispose();
            }
        }

        private static HanaCommand PrepareCommand(string commandText
            , HanaConnection connection
            , CommandType cmdType = CommandType.StoredProcedure
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , HanaTransaction transaction = null)
        {
            HanaCommand command = new HanaCommand();
            command.Connection = connection;
            command.CommandType = cmdType;
            command.CommandText = commandText;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (parameter != null)
            {
                if (parameters == null) { parameters = new List<HanaParameter>(); }
                parameters.Add(parameter);
            }

            if (parameters != null)
            {
                foreach (HanaParameter x in parameters)
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
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure) where T : class
        {
            HanaConnection connection = null;
            HanaCommand command = null;
            HanaDataReader dataReader = null;

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
            catch (HanaException)
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
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure) where T : class
        {
            HanaConnection connection = null;
            HanaCommand command = null;
            HanaDataReader dataReader = null;

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
            catch (HanaException)
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
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure)
        {
            HanaConnection connection = null;
            HanaCommand command = null;
            T result;

            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                result = (T)command.ExecuteScalar();
            }
            catch (HanaException)
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
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , Dictionary<string, object> paramsOutputValue = null
            , CommandType type = CommandType.StoredProcedure
            , HanaConnection connection = null
            , HanaTransaction transaction = null)
        {
            HanaCommand command = null;

            try
            {
                if (connection == null) { connection = OpenConnection(); }

                command = PrepareCommand(commandText, connection, type, parameter, parameters, transaction);
                command.ExecuteNonQuery();
                if (parameter != null || parameters != null)
                {
                    foreach (HanaParameter x in parameters)
                    {
                        if (x.Direction == ParameterDirection.Output)
                        {
                            paramsOutputValue.Add(x.ParameterName, command.Parameters[x.ParameterName].Value);
                        }
                    }
                }
            }
            catch (HanaException)
            {
                throw;
            }
            finally
            {
                CloseCommand(command);
                if (transaction == null) { CloseConnection(connection); }

            }
        }

        public static void BeginTransaction(ref HanaConnection sqlConnection, ref HanaTransaction sqlTransaction)
        {
            sqlConnection = OpenConnection();
            sqlTransaction = sqlConnection.BeginTransaction();
        }
        public static void EndTransaction(ref HanaConnection sqlConnection, ref HanaTransaction sqlTransaction)
        {
            if (sqlTransaction != null) { sqlTransaction.Dispose(); }
            CloseConnection(sqlConnection);
        }

        public static DataSet GetDataSet(string commandText
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure)
        {
            HanaConnection connection = null;
            HanaCommand command = null;
            HanaDataAdapter da = new HanaDataAdapter();

            DataSet ds = new DataSet();
            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (HanaException)
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
