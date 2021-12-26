using FishingReborn.Common.CustomCatchRules.Conditions;
using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Common.CustomCatchRules.Pools {
    /// <summary>
    /// Pool that pertains to fishing in the ocean.
    /// </summary>
    public class OceanCatchPool : ICatchPool {
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        public uint Priority => 2;

        public bool CompleteOverride => false;

        private readonly HeightLevelCondition _heightLevelCondition = new HeightLevelCondition(1, false);

        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber) => _heightLevelCondition.IsConditionMet(attempt, bobber) && (attempt.X < 380 || attempt.X > Main.maxTilesX - 380) && attempt.waterTilesCount > 1000;
    }
}