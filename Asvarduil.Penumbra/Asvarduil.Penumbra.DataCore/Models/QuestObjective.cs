namespace Asvarduil.Penumbra.DataCore.Models
{
    public enum QuestObjectiveType
    {
        Patrol,
        Combat
    }

    public class QuestObjective
    {
        public int Id;
        public int QuestId;
        public string Description;
        public QuestObjectiveType ObjectiveType;
        public int? PatrolSectorX;
        public int? PatrolSectorY;
        public int? PatrolSectorZ;
        public string CombatShipName;
        public string CombatShipBlueprintName;

        public string PatrolSector => $"[{PatrolSectorX}, {PatrolSectorY}, {PatrolSectorZ}]";
    }
}
