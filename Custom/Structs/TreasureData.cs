using System;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Custom.Structs {
    /// <summary>
    /// Struct that holds data on a given treasure/crate, such as its pre-hardmode and
    /// hardmode equivalent and what biome it needs in order to be caught.
    /// </summary>
    public readonly struct TreasureData {
        /// <summary>
        /// The type of the treasure acquired in normal mode.
        /// </summary>
        public readonly int normalModeType;

        /// <summary>
        /// The type of the treasure acquired in hard mode.
        /// </summary>
        public readonly int hardModeType;

        /// <summary>
        /// The weight this treasure will hold when being selected.
        /// </summary>
        public readonly float treasureWeight;

        /// <summary>
        /// The method/function that determines what this treasure needs in order
        /// to be caught in the first place.
        /// </summary>
        public readonly Func<Player, FishingAttempt, bool> catchRequirement;

        public TreasureData(int normalModeType, int hardModeType, float treasureWeight, Func<Player, FishingAttempt, bool> catchRequirement) {
            this.normalModeType = normalModeType;
            this.hardModeType = hardModeType;
            this.treasureWeight = treasureWeight;
            this.catchRequirement = catchRequirement;
        }
    }
}