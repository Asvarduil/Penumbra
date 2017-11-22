using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class LoanedShip
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string ShipTemplateName { get; set; }
        public string ShipName { get; set; }
        public int Value { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
