using Devsoft.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Core.Helper
{
    public sealed class FileHelper
    {
        private FileHelper() { }

        static private String nameFileLog;

        public static void Write(string linea = null)
        {
            StreamWriter sw = new StreamWriter(fileNameLog(), true);
            if (linea != null)
            {
                sw.WriteLine(linea);
            }
            sw.Close();
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

            String ruta = Constantes.GetRutaArchivosPendientes();

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
        public static void startFile(String name, bool newFile = false)
        {
            nameFileLog = String.Format("{0}_{1}{2}"
                , DateTime.Now.ToString("yyyy-MM-dd")
                , Convert.ToString(name).Trim()
                , ".txt"); ;

            if (newFile) { DeleteFile(fileNameLog()); }
        }
        public static void closeFile()
        {
            nameFileLog = null;
        }
        public static void MoveFile(string origin, string destination)
        {
            if (File.Exists(origin))
            {
                if (File.Exists(destination)) { DeleteFile(destination); }

                try
                {
                    File.Move(origin, destination);
                    DeleteFile(origin);
                }
                catch (IOException)
                {
                    throw;
                }

            }
        }
        public static void DeleteFile(string ruta)
        {
            if (File.Exists(ruta))
            {
                try
                {
                    File.Delete(ruta);
                }
                catch (IOException)
                {
                    throw;
                }
            }

        }
        public static void CreateDirectory(string ruta)
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
        }
    }
}
