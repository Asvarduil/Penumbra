using System;
using Asvarduil.Penumbra.DataCore.Models;
using Asvarduil.Penumbra.DataCore.Services;

namespace Asvarduil.Penumbra.QuestEngine
{
    /// <summary>
    /// The Quest Master generates procedural quests, based on a
    /// Quest Type.
    /// </summary>
    public class QuestMaster
    {
        public Quest GenerateQuest()
        {
            var newQuest = new Quest();

            // Step 1 - Generate a GUID to distinguish the quest from all others.
            Guid guid = Guid.NewGuid();
            newQuest.GUID = guid.ToString();

            // Step 2 - Determine what level of difficulty of Quest will be generated.
            //          There are three grades of difficulty:
            //          0 - Easy
            //          1 - Moderate
            //          2 - Hard
            //
            // TODO: In later iterations, determine what difficulty the player is
            //       best-suited to based on their history of completing quests.
            var rng = new Random();
            QuestDifficulty difficulty = (QuestDifficulty) rng.Next(0, 3);
            newQuest.Difficulty = difficulty;

            // Step 3 - Determine what type of quset will be generated.
            //          There are the following types of quests:
            //          0 - Patrol (go to these sectors)
            //          1 - Combat (Destroy the named ship)
            //
            // TODO: In later iterations, determine what quest types are available
            //       based on the entity that gives them out.
            QuestObjectiveType questType = (QuestObjectiveType) rng.Next(0, 2);

            // Step 4 - Generate a name for the quest.
            string questName = string.Empty;
            switch(questType)
            {
                case QuestObjectiveType.Patrol:
                    questName = "Patrol Duty";
                    break;

                case QuestObjectiveType.Combat:
                    questName = "Intercept Enemies";
                    break;
            }
            newQuest.Name = questName;

            // Step 5 - Generate quest steps based on difficulty.
            //          The difficulty of the quest has a number
            //          of effects on what the player has to do.
            // - Higher difficulty quests have more steps.
            // - Higher difficulty quests gives more rewards.
            // - Higher difficulty quests spawn more dangerous
            //   types of ships.
            // - Higher difficulty quests can be further from
            //   their point of origin, and thus take longer
            //   to get to.


            // Step _ - Generate a description for the quest.
            string questDescription = string.Empty;
            switch(questType)
            {
                case QuestObjectiveType.Patrol:
                    break;

                case QuestObjectiveType.Combat:
                    break;
            }
            newQuest.Description = questDescription;

            // TODO: Call QuestService to save the quest and all child objects.

            return newQuest;
        }
    }
}
