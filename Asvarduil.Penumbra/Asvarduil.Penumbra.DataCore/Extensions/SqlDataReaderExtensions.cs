using System;
using System.Data.SqlClient;

namespace Asvarduil.Penumbra.DataCore.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static int ReadInteger(this SqlDataReader reader, string key)
        {
            var value = Convert.ToInt32(reader[key]);
            return value;
        }

        public static int? ReadNullableInteger(this SqlDataReader reader, string key)
        {
            if (reader[key] == DBNull.Value)
                return null;

            var value = Convert.ToInt32(reader[key]);
            return value;
        }

        public static string ReadString(this SqlDataReader reader, string key)
        {
            var value = Convert.ToString(reader[key]);
            return value;
        }

        public static DateTime ReadDateTime(this SqlDataReader reader, string key)
        {
            var value = Convert.ToDateTime(reader[key]);
            return value;
        }

        public static DateTime? ReadNullableDateTime(this SqlDataReader reader, string key)
        {
            if (reader[key] == DBNull.Value)
                return null;

            var value = Convert.ToDateTime(reader[key]);
            return value;
        }

        public static bool ReadBoolean(this SqlDataReader reader, string key)
        {
            var value = Convert.ToBoolean(reader[key]);
            return value;
        }
    }
}
