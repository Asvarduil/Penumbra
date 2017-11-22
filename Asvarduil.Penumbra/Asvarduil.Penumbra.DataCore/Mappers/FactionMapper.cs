using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;
using System.Data.SQLite;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class FactionMapper : ISQLiteModelMapper<Faction>
    {
        public Faction MapResult(SQLiteDataReader reader)
        {
            var faction = new Faction
            {
                Id = reader.ReadInteger("Id"),
                Name = reader.ReadString("Name"),
                CreateDate = reader.ReadNullableDateTime("CreateDate")
            };

            return faction;
        }
    }
}
