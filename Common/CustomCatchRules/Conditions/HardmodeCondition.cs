using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// Condition that checks if the current world is in hardmode.
    /// </summary>
    public class HardmodeCondition : ICatchCondition {
        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) => Main.hardMode;
    }
}