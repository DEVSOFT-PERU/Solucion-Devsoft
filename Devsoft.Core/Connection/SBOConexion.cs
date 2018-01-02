using Devsoft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Devsoft.Core.Util.Constantes;

namespace Devsoft.Core.Connection
{
    public static class SBOConexion
    {
        public static SAPbobsCOM.Company getOCompany(ref String sResultMsg)
        {
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            if (!oCompany.Connected)
            {
                //// Set connection properties
                string TipoBD = Constantes.Servidor.TipoBD;
                oCompany.DbServerType = GetDbServerType(TipoBD);
                oCompany.Server = GetServer(TipoBD);
                oCompany.LicenseServer = GetLicenseServer(TipoBD);// change to your company server 
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish; // change to your language
                oCompany.UseTrusted = false;
                oCompany.DbUserName = Constantes.Servidor.UsuarioServ;
                oCompany.DbPassword = Constantes.Servidor.ContraServ;
                oCompany.CompanyDB = Constantes.Compania.BaseCia;
                oCompany.UserName = Constantes.Compania.UsuarioCia;
                oCompany.Password = Constantes.Compania.Contrasena;
                //Try to connect
                if (oCompany.FailOperation(oCompany.Connect(), out sResultMsg))
                {
                    return oCompany;
                }

            }
            if (oCompany.Connected) // if connected
            {
                sResultMsg = String.Format("Se conecto a {0} | {1} | {2} "
                    , oCompany.CompanyDB
                    , oCompany.CompanyName
                    , oCompany.Version);
            }
            return oCompany;
        }

        public static bool FailOperation(this SAPbobsCOM.Company oCompany, int lRetCode, out string sResultMsg)
        {
            bool error = false;
            sResultMsg = String.Empty;
            if (lRetCode != 0) // if the connection failed
            {
                error = true;
                //oCompany.GetLastError(out lErrCode, out sResultMsg);
                //sResultMsg = String.Format("{0} - {1}", lErrCode, sResultMsg);
                sResultMsg = String.Format("{0} | {1}", oCompany.GetLastErrorCode(), oCompany.GetLastErrorDescription());

            }
            return error;
        }
        #region Set Properties Connection SAP
        public static SAPbobsCOM.BoDataServerTypes GetDbServerType(string tipoBD)
        {
            switch (tipoBD)
            {
                case TipoBD.HANA:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    }
                case TipoBD.SQL:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL;
                    }
                case TipoBD.SQL2005:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
                    }
                case TipoBD.SQL2008:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    }
                case TipoBD.SQL2012:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    }
                case TipoBD.SQL2014:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    }
                case TipoBD.SQL2016:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                    }
                default:
                    {
                        return SAPbobsCOM.BoDataServerTypes.dst_MSSQL;
                    }
            }

        }
        public static string GetServer(string tipoBD)
        {
            switch (tipoBD)
            {
                case TipoBD.HANA:
                    {
                        return String.Format("{0}:{1}", Constantes.Servidor.IP, Constantes.Servidor.Puerto);
                    }
                default:
                    {
                        return Constantes.Servidor.IP;
                    }
            }

        }
        public static string GetLicenseServer(string tipoBD)
        {
            switch (tipoBD)
            {
                case TipoBD.HANA:
                    {
                        return null;
                    }
                case TipoBD.SQL2012:
                    {
                        return null;
                    }
                default:
                    {
                        return Constantes.Servidor.ServidorLicencia + GetPuertoLicensia();
                    }
            }
        }
        private static string GetPuertoLicensia()
        {
            return (!Constantes.Servidor.PuertoLicencia.Equals("-")
                            ? String.Format(":{0}", Constantes.Servidor.PuertoLicencia)
                            : String.Empty);
        }
        #endregion
    }




}
