using Devsoft.Core;
namespace Devsoft.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Constantes.Servidor = new Model.ServidorModel()
            {
                IP = "MARKANTHONYARRO",
                Puerto = "-",
                TipoBD = "SQL2012",
                ServidorLicencia = "markanthonyarro",
                PuertoLicencia = "40000",
                UsuarioServ = "sa",
                ContraServ = "sql"
            };
            Util.Constantes.Compania = new Model.CompaniaModel()
            {
                ID =1,
                BaseCia = "SBO_LOCA_V1",
                UsuarioCia = "manager",
                Contrasena ="1234"
            };
            Console.WriteLine
        }
    }
}
