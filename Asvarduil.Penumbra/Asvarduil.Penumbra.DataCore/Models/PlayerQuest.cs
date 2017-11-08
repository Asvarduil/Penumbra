namespace Asvarduil.Penumbra.DataCore.Models
{
    public class PlayerQuest
    {
        public int Id;
        public int PlayerId;
        public int QuestId;
        public int OriginSectorX;
        public int OriginSectorY;
        public int OriginSectorZ;
        public bool InProgress;
        public bool DidPlayerComplete;

        public Quest Quest { get; set; }
    }
}
