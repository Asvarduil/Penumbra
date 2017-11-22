using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;
using System;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class PlayerDebtMapping : ISQLiteModelMapper<PlayerDebt>
    {
        public PlayerDebt MapResult(SQLiteDataReader reader)
        {
            var playerDebt = new PlayerDebt
            {
                Id = reader.ReadInteger("Id"),
                PlayerId = reader.ReadInteger("PlayerId"),
                Value = reader.ReadInteger("Value"),
                InterestRate = reader.ReadInteger("InterestRate"),
                IsPaid = reader.ReadBoolean("IsPaid"),
                UpdatedDate = reader.ReadNullableDateTime("UpdatedDate")
            };

            return playerDebt;
        }
    }
}
