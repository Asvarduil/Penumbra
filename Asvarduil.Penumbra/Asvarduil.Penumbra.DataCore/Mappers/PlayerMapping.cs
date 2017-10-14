using System.Data.SQLite;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Extensions;

namespace Asvarduil.Penumbra.DataCore.Mappers
{
    public class PlayerMapping : ISQLiteModelMapper<Player>
    {
        public Player MapResult(SQLiteDataReader reader)
        {
            var player = new Player
            {
                Id = reader.ReadInteger("Id"),
                Name = reader.ReadString("Name"),
                JoinDate = reader.ReadDateTime("JoinDate"),
                LastLoggedInDate = reader.ReadNullableDateTime("LastLoggedInDate"),
                LastLoggedOutDate = reader.ReadNullableDateTime("LastLoggedOutDate"),
                IsAdmin = reader.ReadBoolean("IsAdmin")
            };

            return player;
        }
    }
}
