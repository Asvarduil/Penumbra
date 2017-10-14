using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class BountyMapping : ISQLiteModelMapper<Bounty>
    {
        public Bounty MapResult(SQLiteDataReader reader)
        {
            var bounty = new Bounty
            {
                Id = reader.ReadInteger("Id"),
                TargetPlayerId = reader.ReadInteger("TargetPlayerId"),
                PostedDate = reader.ReadDateTime("PostedDate"),
                Value = reader.ReadInteger("Value"),
                ClaimingPlayerId = reader.ReadNullableInteger("ClaimingPlayerId"),
                ClaimedDate = reader.ReadNullableDateTime("ClaimedDate")
            };

            return bounty;
        }
    }
}
