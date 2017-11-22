using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class ReputationMapper : ISQLiteModelMapper<Reputation>
    {
        public Reputation MapResult(SQLiteDataReader reader)
        {
            var reputation = new Reputation
            {
                Id = reader.ReadInteger("Id"),
                PlayerId = reader.ReadInteger("PlayerId"),
                FactionId = reader.ReadInteger("FactionId"),
                Standing = reader.ReadInteger("Reputation"),  // TODO: Rename DB field to Standing.
                UpdatedDate = reader.ReadNullableDateTime("UpdatedDate")
            };

            return reputation;
        }
    }
}
