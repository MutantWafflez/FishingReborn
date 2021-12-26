using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Pool that deals with fishing in lava.
    /// </summary>
    public class LavaCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 0;

        public bool CompleteOverride => true;

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => attempt.CanFishInLava && attempt.inLava;
    }
}