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
        /// The method/function that returns what item is given to the player if all
        /// other conditions/requirements are satisfied.
        /// </summary>
        public readonly Func<Player, FishingAttempt, int> itemSelection;

        /// <summary>
        /// The weight this treasure will hold when being selected.
        /// </summary>
        public readonly float treasureWeight;

        /// <summary>
        /// The method/function that determines what this treasure needs in order
        /// to be caught in the first place.
        /// </summary>
        public readonly Func<Player, FishingAttempt, bool> catchRequirement;

        public TreasureData(Func<Player, FishingAttempt, int> itemSelection, float treasureWeight, Func<Player, FishingAttempt, bool> catchRequirement) {
            this.itemSelection = itemSelection;
            this.treasureWeight = treasureWeight;
            this.catchRequirement = catchRequirement;
        }
    }
}