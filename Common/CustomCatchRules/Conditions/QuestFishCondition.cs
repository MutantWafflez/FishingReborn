using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// Condition that is satisfied if at the given moment, the current quest fish
    /// is equal to the passed in quest fish ID.
    /// </summary>
    public class QuestFishCondition : ICatchCondition {
        private readonly int _questFishID;

        public QuestFishCondition(int questFishID) {
            _questFishID = questFishID;
        }

        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) => attempt.questFish == _questFishID;
    }
}