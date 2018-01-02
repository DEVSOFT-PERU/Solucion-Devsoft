using Devsoft.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Core.Helper
{
    public sealed class LogHelper
    {
        private LogHelper() { }

        static private String nameFileLog;

        public static void Log(bool timestamp = false, String metodo = null, String descripcion = null, String linea = null, Dictionary<string, string> parametros = null, Exception exc = null, String respuesta = null)
        {

            bool debug = Constantes.GetLogActived();

            if (debug)
            {
                StreamWriter sw = new StreamWriter(fileNameLog(), true);

                if (timestamp)
                {
                    sw.WriteLine("********** {0} **********", DateTime.Now);
                }

                if (metodo != null)
                {
                    sw.WriteLine("Método: ", metodo);
                }
                if (descripcion != null)
                {
                    sw.WriteLine("Descripción: ", descripcion);
                }
                if (parametros != null)
                {
                    writeParameters(sw, parametros);
                    sw.WriteLine("********** {0} **********", "LOG");
                    sw.WriteLine("---");
                    sw.WriteLine();
                }


                if (linea != null)
                {
                    sw.WriteLine(String.Format("{0} :  {1}"
                        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        , linea));

                }

                if (respuesta != null)
                {
                    sw.WriteLine();
                    writeResponse(sw, respuesta);
                }

                if (exc != null)
                {
                    writeException(sw, exc);
                }

                sw.Close();
            }

            if (exc != null)
            {
                writeExceptionFile(exc);
            }
        }
        public static String fileNameLog()
        {
            String fileName = "";

            if (nameFileLog == null)
            {
                fileName = String.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd"), ".txt");
            }
            else
            {
                fileName = nameFileLog;
            }

            String logFile = @"C:\" + fileName;

            String ruta = Constantes.GetRutaLog();

            if (!String.IsNullOrEmpty(ruta))
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                logFile = ruta.ToString() + fileName;
            }


            if (!File.Exists(logFile))
                File.Create(@logFile).Close();

            return logFile;
        }
        public static void writeParameters(StreamWriter sw, Dictionary<string, string> parametros)
        {

            sw.WriteLine("********** {0} **********", "PARAMETROS");
            sw.WriteLine();
            foreach (KeyValuePair<string, string> pair in parametros)
            {
                sw.WriteLine(String.Format("Key: {0}", pair.Key));
                sw.WriteLine(String.Format("Value: {0}", pair.Value));
                sw.WriteLine();
            }
        }
        public static void writeException(StreamWriter sw, Exception exc)
        {

            sw.WriteLine("********** {0} **********", "EXCEPTION");
            sw.WriteLine("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Stack Trace: ");
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }

            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
        }
        public static void writeExceptionFile(Exception exc)
        {

            String ruta = Constantes.GetRutaLog();
            String fileName = "Excepciones.txt";
            String logFile = ruta.ToString() + fileName;

            StreamWriter sw = new StreamWriter(logFile, true);

            sw.WriteLine("********** {0} **********", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.WriteLine("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Stack Trace: ");
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }

            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }
        public static void writeResponse(StreamWriter sw, String respuesta)
        {
            sw.WriteLine("********** {0} **********", "Respuesta");
            sw.WriteLine();
            sw.WriteLine("Response :");
            sw.WriteLine(respuesta);
        }
        public static void startLog(String name)
        {
            nameFileLog = String.Format("{0}_{1}{2}"
                , DateTime.Now.ToString("yyyy-MM-dd")
                , Convert.ToString(name).Trim()
                , ".txt"); ;
        }
        public static void closeLog()
        {
            nameFileLog = null;
        }
    }
}
