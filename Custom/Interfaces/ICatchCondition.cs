using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Custom.Interfaces {
    /// <summary>
    /// Interface that is a condition for a Catch to be, well, caught.
    /// </summary>
    public interface ICatchCondition {
        /// <summary>
        /// Whether or not this specified condition is met.
        /// </summary>
        /// <param name="attempt"> Fishing data for this specific catch. </param>
        /// <param name="bobber"> The bobber that is being used for this specific catch. </param>
        public bool IsConditionMet(FishingAttempt attempt, Projectile bobber);
    }
}