using FishingReborn.Custom.Interfaces;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Conditions {
    /// <summary>
    /// This condition is satisfied if the pool meets the passed in requirements.
    /// </summary>
    public class PoolSizeCondition : ICatchCondition {
        private readonly int _poolTileSize;
        private readonly bool? _lessOrEqualOrGreater;

        /// <param name="poolTileSize"> The size of the pool, in tiles. </param>
        /// <param name="lessOrEqualOrGreater">
        /// If false, then the condition will pass if the attempt is less than or equal to the passed in pool size.
        /// If null, then the condition will pass if the attempt is equal to the passed in pool size.
        /// If true, then the condition will pass if the attempt is greater than or equal to the passed in pool size.
        /// </param>
        public PoolSizeCondition(int poolTileSize, bool? lessOrEqualOrGreater) {
            _poolTileSize = poolTileSize;
            _lessOrEqualOrGreater = lessOrEqualOrGreater;
        }

        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber) {
            switch (_lessOrEqualOrGreater) {
                case false:
                    return attempt.waterTilesCount <= _poolTileSize;
                case null:
                    return attempt.waterTilesCount == _poolTileSize;
                default:
                    return attempt.waterTilesCount >= _poolTileSize;
            }
        }
    }
}