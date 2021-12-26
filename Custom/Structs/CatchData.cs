using FishingReborn.Custom.Enums;

namespace FishingReborn.Custom.Structs {
    /// <summary>
    /// Struct that holds data for a given catch, including the catch's difficulty
    /// and its movement type.
    /// </summary>
    public readonly struct CatchData {
        /// <summary>
        /// Scale of how difficult a catch is, to, well, catch, during the minigame.
        /// </summary>
        public readonly int fishDifficulty;

        /// <summary>
        /// How the catch/fish will move during the minigame.
        /// </summary>
        public readonly FishMovementType movementType;

        public CatchData(int fishDifficulty, FishMovementType movementType) {
            this.fishDifficulty = fishDifficulty;
            this.movementType = movementType;
        }

        /// <summary>
        /// Returns a new CatchData struct that is considered the "default" fallback values for Catches.
        /// </summary>
        public static CatchData CreateDefaultCatchData() => new CatchData(15, FishMovementType.Mixed);
    }
}