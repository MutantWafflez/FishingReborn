using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Catch pool that deals with fishing in honey.
    /// </summary>
    public class HoneyCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 0;

        public bool CompleteOverride => true;

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => attempt.inHoney;
    }
}