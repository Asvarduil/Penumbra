using System;
using System.Data.SQLite;

namespace Asvarduil.Penumbra.DataCore.Extensions
{
    public static class SqliteDataReaderExtensions
    {
        public static int ReadInteger(this SQLiteDataReader reader, string key)
        {
            var value = Convert.ToInt32(reader[key]);
            return value;
        }

        public static int? ReadNullableInteger(this SQLiteDataReader reader, string key)
        {
            if (reader[key] == DBNull.Value)
                return null;

            var value = Convert.ToInt32(reader[key]);
            return value;
        }

        public static string ReadString(this SQLiteDataReader reader, string key)
        {
            var value = Convert.ToString(reader[key]);
            return value;
        }

        public static DateTime ReadDateTime(this SQLiteDataReader reader, string key)
        {
            var value = Convert.ToDateTime(reader[key]);
            return value;
        }

        public static DateTime? ReadNullableDateTime(this SQLiteDataReader reader, string key)
        {
            if (reader[key] == DBNull.Value)
                return null;

            var value = Convert.ToDateTime(reader[key]);
            return value;
        }

        public static bool ReadBoolean(this SQLiteDataReader reader, string key)
        {
            var value = Convert.ToBoolean(reader[key]);
            return value;
        }
    }
}
