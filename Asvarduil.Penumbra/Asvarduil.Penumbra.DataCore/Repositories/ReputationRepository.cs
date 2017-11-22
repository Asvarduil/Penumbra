using System;
using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class ReputationRepository : PenumbraRepository<Reputation, ReputationMapper>
    {
        #region Instance

        private static ReputationRepository _instance;
        private static ReputationRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ReputationRepository();

                return _instance;
            }
        }

        #endregion Instance

        #region Accessors

        public static List<Reputation> GetByPlayerId(int playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", playerId }
            };

            return Instance.RunFileQuery("Queries/GetReputationByPlayerId.sql", parameters).ToList();
        }

        public static void Create(Reputation reputation)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", reputation.PlayerId },
                { "FactionId", reputation.FactionId },
                { "Reputation", reputation.Standing },
                { "UpdatedDate", DateTime.Now }
            };

            Instance.RunFileExecute("Queries/AddReputation.sql", parameters);
        }

        public static void Update(Reputation reputation)
        {
            var parameters = new Dictionary<string, object>
            {
                { "PlayerId", reputation.PlayerId },
                { "Reputation", reputation.Standing },
                { "UpdatedDate", DateTime.Now }
            };

            Instance.RunFileExecute("Queries/UpdateReputationByPlayerId.sql", parameters);
        }

        #endregion Accessors
    }
}
