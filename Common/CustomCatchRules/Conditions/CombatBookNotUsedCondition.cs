using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// Condition that checks if the NPC Combat Techniques book has been used or not.
    /// Returns false if it has been used.
    /// </summary>
    public class CombatBookNotUsedCondition : ICatchCondition {
        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) => !NPC.combatBookWasUsed;
    }
}