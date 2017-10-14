using System;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public class Feedback
    {
        public int Id;
        public int PlayerId;
        public DateTime? FeedbackDate;
        public int Rating;
        public string Details;
    }
}
