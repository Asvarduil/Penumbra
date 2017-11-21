namespace Asvarduil.Penumbra.DataCore.Models
{
    public class QuestReputationReward
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public int FactionId { get; set; }
        public int ReputationAmount { get; set; }
    }
}
