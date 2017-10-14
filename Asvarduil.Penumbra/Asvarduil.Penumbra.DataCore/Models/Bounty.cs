using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class Bounty
    {
        public int Id;
        public int TargetPlayerId;
        public DateTime PostedDate;
        public int Value;
        public int? ClaimingPlayerId;
        public DateTime? ClaimedDate;

        public bool IsClaimed => ClaimingPlayerId == null;
    }
}
