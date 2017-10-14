using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class NetWorth
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Value { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
