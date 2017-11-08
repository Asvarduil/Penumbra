using System.Collections.Generic;

namespace Asvarduil.Penumbra.DataCore.Models
{
    public enum QuestDifficulty
    {
        Easy,
        Moderate,
        Hard
    }

    public class Quest
    {
        public int Id;
        public string Name;
        public string GUID;
        public QuestDifficulty Difficulty;
        public string Description;
        public List<QuestObjective> Objectives;
        public int CreditReward;
        public int NetWorthReward;
        public List<QuestItemReward> ItemRewards;
    }
}
