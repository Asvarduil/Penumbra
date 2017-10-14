using System;
using System.Linq;
using System.Collections.Generic;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Mappers;

namespace Asvarduil.Penumbra.DataCore.Repositories
{
    public class BountyRepository : PenumbraRepository<Bounty, BountyMapping>
    {
        #region Instance Accessor

        private static BountyRepository _instance;
        private static BountyRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BountyRepository();

                return _instance;
            }
        }

        #endregion Instance Accessor

        #region Methods

        public static void Create(Bounty bounty)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TargetPlayerId", bounty.TargetPlayerId },
                { "PostedDate", DateTime.Now },
                { "Value", bounty.Value }
            };

            Instance.RunFileExecute("Queries/AddNewBounty.sql", parameters);
        }

        public static void UpdateWithClaim(int targetPlayerId, int claimingPlayerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TargetPlayerId", targetPlayerId },
                { "ClaimingPlayerId", claimingPlayerId },
                { "ClaimedDate", DateTime.Now }
            };

            Instance.RunFileExecute("Queries/UpdateBountyClaimedByTargetPlayerId.sql", parameters);
        }

        public static List<Bounty> GetByTargetPlayerId(int targetPlayerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TargetPlayerId", targetPlayerId }
            };

            var result = Instance.RunFileQuery("Queries/GetBountyByTargetPlayerId.sql", parameters);
            return result.ToList();
        }

        public static void RemoveAllForPlayer(int playerId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TargetPlayerId", playerId }
            };

            Instance.RunFileExecute("Queries/RemoveBountiesByPlayerId.sql", parameters);
        }

        #endregion Methods
    }
}
