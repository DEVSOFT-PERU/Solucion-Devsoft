using Devsoft.Core.Util;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Core.Helper
{
    /// <summary>
    /// Clase que permite manejar las ejecución de métodos ADO.NET
    /// </summary>
    /// <remarks>
    /// Autor: Mark Anthony Arroyo Garcia
    /// Versión: 1.0.0
    /// Fecha: 20/12/2017
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
            , List<HanaParameter> parameters = null)
        {
            HanaCommand command = new HanaCommand();
            command.Connection = connection;
            command.CommandType = cmdType;
            command.CommandText = commandText;

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
            , CommandType type = CommandType.StoredProcedure)
        {
            HanaConnection connection = null;
            HanaCommand command = null;

            try
            {
                connection = OpenConnection();
                command = PrepareCommand(commandText, connection, type, parameter, parameters);
                command.ExecuteNonQuery();
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
        }

        public static void ExecuteOutput(string commandText
            , List<string> paramsOutput
            , List<object> paramsOutputValue
            , HanaParameter parameter = null
            , List<HanaParameter> parameters = null
            , CommandType type = CommandType.StoredProcedure)
        {
            HanaConnection connection = null;
            HanaCommand command = null;

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
            catch (HanaException)
            {
                throw;
            }
            finally
            {
                CloseCommand(command);
                CloseConnection(connection);
            }
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