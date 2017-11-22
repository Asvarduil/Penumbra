using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;
using System;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class LoanedShipMapper : ISQLiteModelMapper<LoanedShip>
    {
        public LoanedShip MapResult(SQLiteDataReader reader)
        {
            var loanedShip = new LoanedShip
            {
                Id = reader.ReadInteger("Id"),
                PlayerId = reader.ReadInteger("PlayerId"),
                ShipTemplateName = reader.ReadString("ShipTemplateName"),
                ShipName = reader.ReadString("ShipName"),
                Value = reader.ReadInteger("Value"),
                IsPaid = reader.ReadBoolean("IsPaid"),
                UpdatedDate = reader.ReadNullableDateTime("UpdatedDateTime")
            };

            return loanedShip;
        }
    }
}
