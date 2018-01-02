using System;
using System.Collections.Generic;
using System.Data;
using Devsoft.Consola.Examples;
using Devsoft.Core;
namespace Devsoft.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Util.Constantes.Servidor = new Core.Model.ServidorModel()
            {
                IP = "MARKANTHONYARRO",
                Puerto = "-",
                TipoBD = "SQL2012",
                ServidorLicencia = "markanthonyarro",
                PuertoLicencia = "40000",
                UsuarioServ = "sa",
                ContraServ = "sql"
            };
            Core.Util.Constantes.Compania = new Core.Model.CompaniaModel()
            {
                ID = 1,
                BaseCia = "SBO_LOCA_V1",
                UsuarioCia = "manager",
                Contrasena = "1234"
            };


            try
            {
                string resultado = String.Empty;
                Console.WriteLine("Conectando a la compañia");

                DateTime ahora = DateTime.Now;
                Console.WriteLine("Tiempo inicio    :  {0}", ahora);
                SAPbobsCOM.Company company = Core.Connection.SBOConexion.getOCompany(ref resultado);
                Console.WriteLine("resultado :  {0}", resultado);
                Console.WriteLine("Tiempo fin       :  {0}", ahora.Subtract(DateTime.Now));

                Console.WriteLine();

                ahora = DateTime.Now;
                Console.WriteLine("Tiempo inicio    :  {0}", ahora);
                SAPbobsCOM.Recordset recordSetobj = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetobj.DoQuery(@"SELECT * FROM OITM");
                if (recordSetobj.RecordCount > 0)
                {
                    while (!recordSetobj.EoF)
                    {
                        //Console.WriteLine("USER_CODE: {0} | U_NAME: {1}"
                        //    , recordSetobj.Fields.Item("ItemCode").Value
                        //    , recordSetobj.Fields.Item("ItemName").Value);
                        recordSetobj.MoveNext();
                    }
                }
                Console.WriteLine("Tiempo fin       :  {0}", ahora.Subtract(DateTime.Now));

                Console.WriteLine();

                ahora = DateTime.Now;
                Console.WriteLine("Tiempo inicio    :  {0}", ahora);
                new LogicCore().ejecutar();
                Console.WriteLine("Tiempo fin       :  {0}", ahora.Subtract(DateTime.Now));


            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}",e.Message);
            }
            Console.Read();
        }
    }
}
