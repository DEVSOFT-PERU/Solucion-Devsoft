using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Consola.Examples
{
    public class ReqCore
    {
        public ReqCore()
        {
            Core.Util.Constantes.CNX_STRING = "Data Source=MARKANTHONYARRO;Initial Catalog=BD_REQ;Persist Security Info=True;User ID=sa;Password=sql;";
        }
        public string ejecutar()
        {
            List<SqlParameter> parametrosIn = new List<SqlParameter>();
            parametrosIn.Add(new SqlParameter() { ParameterName = "@MSS_ID", Value = 5 });
            parametrosIn.Add(new SqlParameter() { ParameterName = "@MSS_NOMBRE", Value = "Devsoft" });
            parametrosIn.Add(new SqlParameter()
            {
                ParameterName = "@MSS_RESULT",
                Size = 50,
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output
            });

            Dictionary<string, object> parametrosValues = new Dictionary<string, object>();
            Devsoft.Core.Helper.SqlDatabaseHelper.Execute(commandText: "MSS_INSERT_TEMPORAL", parameters: parametrosIn, paramsOutputValue: parametrosValues);

            return parametrosValues["@MSS_RESULT"].ToString();
        }
        public string Transaction()
        {
            string msj = String.Empty;
            SqlConnection sqlConnection = null;
            SqlTransaction sqlTransaction = null;
            Devsoft.Core.Helper.SqlDatabaseHelper.BeginTransaction(ref sqlConnection, ref sqlTransaction);
            try
            {
                List<SqlParameter> parametrosIn = null;

                ///////////////////

                parametrosIn = new List<SqlParameter>();
                parametrosIn.Add(new SqlParameter() { ParameterName = "@MSS_NOMBRE", Value = DateTime.Now.ToString("mm:ss") });
                Devsoft.Core.Helper.SqlDatabaseHelper.Execute(commandText: "MSS_INSERT_TEMPORAL_DATO"
                   , parameters: parametrosIn
                   , connection: sqlConnection
                   , transaction: sqlTransaction);

                ///////////////////

                parametrosIn = new List<SqlParameter>();
                parametrosIn.Add(new SqlParameter() { ParameterName = "@MSS_ID", Value = 5 });
                parametrosIn.Add(new SqlParameter() { ParameterName = "@MSS_NOMBRE", Value = "Devsoft" });
                parametrosIn.Add(new SqlParameter()
                {
                    ParameterName = "@MSS_RESULT",
                    Size = 50,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Output
                });

                Dictionary<string, object> parametrosValues = new Dictionary<string, object>();
                Devsoft.Core.Helper.SqlDatabaseHelper.Execute(commandText: "MSS_INSERT_TEMPORAL"
                    , parameters: parametrosIn
                    , paramsOutputValue: parametrosValues
                    , connection: sqlConnection
                    , transaction: sqlTransaction);

                sqlTransaction.Commit();
                msj = "SUCCESS";
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                msj = e.Message;
            }
            finally
            {
                Devsoft.Core.Helper.SqlDatabaseHelper.BeginTransaction(ref sqlConnection, ref sqlTransaction);
            }

            return msj;
        }
    }
}
