using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Core.Helper
{
    public static class HanaDataReaderHelper
    {
        private static int ordinal;
        public static class HanaDataReaderHelperDefault
        {
            public static string STRING_DEF = "";
            public static int INT_DEF = -1;
            public static bool BOOL_DEF = false;
            public static decimal DECIMAL_DEF = -1;
            public static DateTime DATETIME_DEF = new DateTime(1996, 12, 13);
        }
        public static bool canRead(this HanaDataReader reader, string tag, out int ordinal)
        {
            ordinal = -1;
            bool can = true;
            try { ordinal = reader.GetOrdinal(tag); }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
            if (ordinal < 0) { can = false; }
            if (ordinal > 0) { if (reader.IsDBNull(ordinal)) { can = false; } }
            return can;
        }

        #region String
        public static string getString(this HanaDataReader reader, string tag)
        {
            return reader.getString(tag, HanaDataReaderHelperDefault.STRING_DEF);
        }
        public static string getString(this HanaDataReader reader, string tag, object def)
        {
            return reader.canRead(tag, out ordinal) ? reader.GetString(ordinal) : Convert.ToString(def);
        }
        #endregion

        #region Int32
        public static int getInt32(this HanaDataReader reader, string tag)
        {
            return reader.getInt32(tag, HanaDataReaderHelperDefault.INT_DEF);
        }
        public static int getInt32(this HanaDataReader reader, string tag, object def)
        {
            return reader.canRead(tag, out ordinal) ? reader.GetInt32(ordinal) : Convert.ToInt32(def);
        }
        #endregion

        #region Bool
        public static bool getBool(this HanaDataReader reader, string tag)
        {
            return reader.getBool(tag, HanaDataReaderHelperDefault.BOOL_DEF);
        }
        public static bool getBool(this HanaDataReader reader, string tag, object def)
        {
            return reader.canRead(tag, out ordinal) ? reader.GetBoolean(ordinal) : Convert.ToBoolean(def);
        }
        #endregion

        #region Decimal
        public static decimal getDecimal(this HanaDataReader reader, string tag)
        {
            return reader.getDecimal(tag, HanaDataReaderHelperDefault.DECIMAL_DEF);
        }
        public static decimal getDecimal(this HanaDataReader reader, string tag, object def)
        {
            return reader.canRead(tag, out ordinal) ? reader.GetDecimal(ordinal) : Convert.ToDecimal(def);
        }
        #endregion

        #region Datetime
        public static DateTime getDateTime(this HanaDataReader reader, string tag)
        {
            return reader.getDateTime(tag, HanaDataReaderHelperDefault.DATETIME_DEF);
        }
        public static DateTime getDateTime(this HanaDataReader reader, string tag, object def)
        {
            return reader.canRead(tag, out ordinal) ? reader.GetDateTime(ordinal) : Convert.ToDateTime(def);
        }
        #endregion  
    }
}