﻿using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Catch pool that has catches pertaining to the caverns and below.
    /// </summary>
    public class CavernsCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 1;

        public bool CompleteOverride => false;

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => attempt.heightLevel >= 3;
    }
}