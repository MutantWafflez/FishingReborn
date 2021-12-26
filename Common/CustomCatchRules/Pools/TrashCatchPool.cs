using System.Collections.Generic;
using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Catch pool that contains trash, when the player is fishing in a small pond
    /// or otherwise has low fishing power.
    /// </summary>
    public class TrashCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 0;

        public bool CompleteOverride => false;

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => Main.rand.Next(50) > attempt.fishingLevel && attempt.waterTilesCount < attempt.waterNeededToFish;
    }
}