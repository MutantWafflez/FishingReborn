using FishingReborn.Custom.Interfaces;
using System.Linq;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Custom.Structs {
    /// <summary>
    /// Struct that simply holds a given catch's default weight of being caught.
    /// </summary>
    public readonly struct CatchWeight {
        public readonly int catchID;

        public readonly float catchWeight;


        public readonly ICatchCondition[] additionalConditions;

        public CatchWeight(int catchID, float catchWeight = 1f, params ICatchCondition[] additionalConditions) {
            this.catchID = catchID;
            this.catchWeight = catchWeight;
            this.additionalConditions = additionalConditions;
        }

        /// <summary>
        /// Returns whether or not the additional conditions (if applicable) are all true.
        /// </summary>
        /// <param name="attempt"> The current fishing attempt. </param>
        /// <param name="bobber"> The bobber projectile connected to the fishing attempt. </param>
        /// <returns></returns>
        public bool AreConditionsMet(FishingAttempt attempt, Projectile bobber) => additionalConditions.All(condition => condition.IsConditionMet(attempt, bobber));
    }
}