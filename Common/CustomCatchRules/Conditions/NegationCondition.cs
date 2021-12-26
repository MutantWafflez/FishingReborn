using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// Condition that is only satisfied if the opposite of the passed in
    /// condition is true.
    /// </summary>
    public class NegationCondition : ICatchCondition {
        private readonly ICatchCondition _normalCatchCondition;

        public NegationCondition(ICatchCondition normalCatchCondition) {
            _normalCatchCondition = normalCatchCondition;
        }

        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) => !_normalCatchCondition.IsConditionMet(attempt, bobber);
    }
}