using Devsoft.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Core.Util
{
    public class Constantes
    {
        public class TipoBD
        {
            public const string HANA = "HANA";
            public const string SQL = "SQL";
            public const string SQL2005 = "SQL2005";
            public const string SQL2008 = "SQL2008";
            public const string SQL2012 = "SQL2012";
            public const string SQL2014 = "SQL2014";
            public const string SQL2016 = "SQL2016";
        }
        public class AppSettings
        {
            public const string PRIMERA_INSTALACION = "PRIMERA_INSTALACION";
            public const string RUTA_LOG = "RUTA_LOG";
            public const string LOG_ACTIVE = "LOG_ACTIVE";
            public const string PREFIX_CNX = "CNX_";
        }

        private static ServidorModel _servidor;
        public static ServidorModel Servidor
        {
            get
            {
                if (_servidor == null) { return new ServidorModel(); }
                return _servidor;
            }
            set { _servidor = value; }
        }

        private static CompaniaModel _compania;
        public static CompaniaModel Compania
        {
            get
            {
                if (_compania == null) { return new CompaniaModel(); }
                return _compania;
            }
            set { _compania = value; }
        }

        public static string CNX_STRING = "";

        internal static string GetRutaLog()
        {
            throw new NotImplementedException();
        }
        internal static string GetRutaArchivosPendientes()
        {
            throw new NotImplementedException();
        }
        internal static bool GetLogActived()
        {
            throw new NotImplementedException();
        }
        public static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings[AppSettings.PREFIX_CNX];
        }
    }
}
