using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class NetWorthRepository : PenumbraRepository<NetWorth, NetWorthMapping>
    {
        #region Instance Accessor

        private static NetWorthRepository _instance;
        private static NetWorthRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NetWorthRepository();

                return _instance;
            }
        }

        #endregion Instance Accessor

        public static void Create(NetWorth netWorth)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", netWorth.PlayerId },
                { "LastUpdatedDate", netWorth.LastUpdatedDate }
            };

            Instance.RunFileExecute("Queries/CreateNetWorthByPlayerId.sql", parameters);
        }

        public static void Update(NetWorth netWorth)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Id", netWorth.Id },
                { "Value", netWorth.Value },
                { "LastUpdatedDate", netWorth.LastUpdatedDate }
            };

            Instance.RunFileExecute("Queries/UpdateNetWorthById.sql", parameters);
        }

        public static NetWorth GetByName(string playerName)
        {
            var player = PlayerRepository.GetByName(playerName);
            return GetByPlayerId(player.Id);
        }

        public static NetWorth GetByPlayerId(int playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", playerId }
            };

            var result = Instance.RunFileQuery("Queries/GetNetWorthByPlayerId.sql", parameters);
            return result.FirstOrDefault();
        }
    }
}
