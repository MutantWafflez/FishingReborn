using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace FishingReborn.Custom.Interfaces {
    /// <summary>
    /// This interface pertains to a set of catches that can only be, well, caught, if the
    /// pool is considered to be "active."
    /// </summary>
    public interface ICatchPool {
        /// <summary>
        /// All of the possible non-treasure/crate catches from this pool.
        /// </summary>
        public List<CatchWeight> PotentialCatches {
            get;
            set;
        }

        /// <summary>
        /// The priority that this pool has. Pools with higher priority will have their
        /// catches appear more often if multiple pools are active at once.
        /// </summary>
        public uint Priority {
            get;
        }

        /// <summary>
        /// Whether or not this pool should override ALL other pools and only have its catches
        /// be caught, regardless of priority. If two pools have complete override and are active,
        /// the one that appears first will take override "priority."
        /// </summary>
        public bool CompleteOverride {
            get;
        }

        /// <summary>
        /// Whether or not this pool is active.
        /// </summary>
        /// <param name="attempt"> The current fishing attempt. </param>
        /// <param name="bobber"> The bobber connected to this fishing attempt. </param>
        public bool IsPoolActive(FishingAttempt attempt, Projectile bobber);
    }
}