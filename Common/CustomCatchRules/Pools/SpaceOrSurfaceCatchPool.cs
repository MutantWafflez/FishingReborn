using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Catch pool that has catches pertaining to space/sky OR the surface.
    /// </summary>
    public class SpaceOrSurfaceCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 1;

        public bool CompleteOverride => false;

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => Main.player[bobber.owner].ZoneSkyHeight || attempt.heightLevel <= 1;
    }
}