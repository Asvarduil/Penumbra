using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class NetWorthMapping : ISQLiteModelMapper<NetWorth>
    {
        public NetWorth MapResult(SQLiteDataReader reader)
        {
            var netWorth = new NetWorth
            {
                Id = reader.ReadInteger("Id"),
                PlayerId = reader.ReadInteger("PlayerId"),
                Value = reader.ReadInteger("Value"),
                LastUpdatedDate = reader.ReadNullableDateTime("LastUpdatedDate")
            };

            return netWorth;
        }
    }
}
