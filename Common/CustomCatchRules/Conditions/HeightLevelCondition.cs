using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// Condition that checks if the attempt is taking place in the specified
    /// height level.
    /// </summary>
    public class HeightLevelCondition : ICatchCondition {
        private readonly int _heightLevel;
        private readonly bool? _lessOrEqualOrGreater;

        /// <param name="heightLevel"> The height level, which should be a value from 0-4, inclusive. </param>
        /// <param name="lessOrEqualOrGreater">
        /// If false, then the condition will pass if the attempt is less than or equal to the passed in height level.
        /// If null, then the condition will pass if the attempt is equal to the passed in height level.
        /// If true, then the condition will pass if the attempt is greater than or equal to the passed in height level.
        /// </param>
        public HeightLevelCondition(int heightLevel, bool? lessOrEqualOrGreater) {
            _heightLevel = heightLevel;
            _lessOrEqualOrGreater = lessOrEqualOrGreater;
        }

        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) {
            switch (_lessOrEqualOrGreater) {
                case false:
                    return attempt.heightLevel <= _heightLevel;
                case null:
                    return attempt.heightLevel == _heightLevel;
                default:
                    return attempt.heightLevel >= _heightLevel;
            }
        }
    }
}