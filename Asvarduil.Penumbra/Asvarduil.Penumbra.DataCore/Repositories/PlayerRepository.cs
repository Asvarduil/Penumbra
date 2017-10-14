using System;
using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class PlayerRepository : PenumbraRepository<Player, PlayerMapping>
    {
        #region Instance Accessor

        private static PlayerRepository _instance;
        private static PlayerRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlayerRepository();

                return _instance;
            }
        }

        #endregion Instance Accessor

        public static void Create(Player player)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerName", player.Name },
                { "JoinDate", player.JoinDate },
                { "LastLoggedInDate", DateTime.Now }
            };

            Instance.RunFileExecute("Queries/AddNewPlayer.sql", parameters);
        }

        public static void Update(Player player)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Id", player.Id },
                { "IsAdmin", player.IsAdmin },
                { "LastLoggedInDate", player.LastLoggedInDate }
            };

            Instance.RunFileExecute("Queries/UpdatePlayerById.sql", parameters);
        }

        public static Player GetByName(string name)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerName", name }
            };
             
            var result = Instance.RunFileQuery("Queries/GetPlayerByName.sql", parameters);
            return result.FirstOrDefault();
        }

        public static List<Player> GetLoggedOnPlayers()
        {
            var result = Instance.RunFileQuery("Queries/GetAllLoggedInPlayers.sql", null);
            return result.ToList();
        }
    }
}
