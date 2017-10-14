using System;
using System.Collections.Generic;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastLoggedInDate { get; set; }
        public DateTime? LastLoggedOutDate { get; set; }
        public bool IsAdmin { get; set; }

        public NetWorth NetWorth { get; set; }
        public List<Bounty> Bounties { get; set; }
    }
}
