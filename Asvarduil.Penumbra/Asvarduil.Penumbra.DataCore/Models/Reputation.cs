using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class Reputation
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int FactionId { get; set; }
        public int Standing { get; set; }  // TODO: Rename DB field to Standing.
        public DateTime? UpdatedDate { get; set; }
    }
}
