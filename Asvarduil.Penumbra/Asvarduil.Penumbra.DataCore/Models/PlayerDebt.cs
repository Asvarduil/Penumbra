using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class PlayerDebt
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Value { get; set; }
        public int InterestRate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
