using System.Collections.Generic;
using System.Linq;
using FishingReborn.Common.CustomCatchRules.Conditions;
using FishingReborn.Common.CustomCatchRules.Pools;
using FishingReborn.Custom.Interfaces;
using FishingReborn.Custom.Structs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace FishingReborn.Common.Systems {
    /// <summary>
    /// System that handles the overriding of vanilla's fish determination method, or in other words,
    /// the method that determines which fish gets selected when a nibble event occurs.
    /// </summary>
    public class CatchDeterminationSystem : ModSystem {
        private List<ICatchPool> _possiblePools;

        public override void Load() {
            _possiblePools = new List<ICatchPool>();

            // Normally I'd add summaries for all of these methods, however I think they are pretty self-explanatory this time
            PopulateLavaCatches();
            PopulateHoneyCatches();
            PopulateTrashCatches();
            PopulateGeneralCatches();
            PopulateCorruptionCatches();
            PopulateCrimsonCatches();
            PopulateHallowCatches();
            PopulateSnowCatches();
            PopulateJungleCatches();
            PopulateGlowingMushroomCatches();
            PopulateOceanCatches();
            PopulateOasisCatches();
            PopulateSpaceCatches();
            PopulateSpaceOrSurfaceCatches();
            PopulateSurfaceCatches();
            PopulateUndergroundCatches();
            PopulateCavernsCatches();
        }

        /// <summary>
        /// Based on the list of possible catches and the given circumstances, select a catch.
        /// </summary>
        /// <param name="attempt"> The current fishing attempt. </param>
        /// <param name="bobber"> The bobber connected to this fish attempt. </param>
        public int DetermineCatch(FishingAttempt attempt, Projectile bobber) {
            //First, get a list of all active pools
            List<ICatchPool> activePools = _possiblePools.Where(pool => pool.IsPoolActive(attempt, bobber)).ToList();
            List<ICatchPool> poolsToAdd = new List<ICatchPool>();

            //Next, go through each of them to see if there are any complete overrides
            foreach (ICatchPool pool in activePools) {
                if (pool.CompleteOverride) {
                    poolsToAdd.Clear();

                    poolsToAdd.Add(pool);
                    break;
                }

                poolsToAdd.Add(pool);
            }

            //Create a "randomizer"
            WeightedRandom<int> randomizer = new WeightedRandom<int>();

            //Calculate the additional bonus for catching rare-er catches depending on fishing power
            float fishingPowerMult = MathHelper.Clamp(attempt.fishingLevel / 100f, 1f, 2.34f);

            //Finally, go through each of the valid pools, and for each of them, if a given element meets their conditions (if applicable), then add them to the randomizer
            foreach (ICatchPool validPool in poolsToAdd) {
                foreach (CatchWeight potentialCatch in validPool.PotentialCatches) {
                    if (!potentialCatch.AreConditionsMet(attempt, bobber)) {
                        continue;
                    }

                    //This is also where we apply the priority function. Very bare bones, but it works in this context.
                    randomizer.Add(
                        potentialCatch.catchID,
                        (potentialCatch.catchWeight <= 0.8f ? potentialCatch.catchWeight * fishingPowerMult : potentialCatch.catchWeight) * (validPool.Priority + 1)
                    );
                }
            }

            return randomizer;
        }

        private void PopulateLavaCatches() {
            LavaCatchPool lavaPool = new LavaCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            lavaPool.PotentialCatches.AddRange(new CatchWeight[] {
                    new CatchWeight(ItemID.ObsidianSwordfish, 0.05f, new HardmodeCondition()),
                    new CatchWeight(ItemID.DemonConch, 0.05f),
                    new CatchWeight(ItemID.BottomlessLavaBucket, 0.05f),
                    new CatchWeight(ItemID.LavaAbsorbantSponge, 0.05f),
                    new CatchWeight(ItemID.FlarefinKoi, 0.67f),
                    new CatchWeight(ItemID.Obsidifish)
                }
            );

            _possiblePools.Add(lavaPool);
        }

        private void PopulateHoneyCatches() {
            HoneyCatchPool honeyPool = new HoneyCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            honeyPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Honeyfin),
                new CatchWeight(ItemID.BumblebeeTuna, additionalConditions: new QuestFishCondition(ItemID.BumblebeeTuna))
            });

            _possiblePools.Add(honeyPool);
        }

        private void PopulateTrashCatches() {
            TrashCatchPool trashPool = new TrashCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            trashPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.OldShoe),
                new CatchWeight(ItemID.FishingSeaweed),
                new CatchWeight(ItemID.TinCan)
            });

            _possiblePools.Add(trashPool);
        }

        private void PopulateGeneralCatches() {
            GeneralCatchPool generalCatchPool = new GeneralCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            generalCatchPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.FrogLeg, 0.015f),
                new CatchWeight(ItemID.BalloonPufferfish, 0.015f),
                new CatchWeight(ItemID.ZephyrFish, 0.015f),
                new CatchWeight(ItemID.BombFish, 0.45f),
                new CatchWeight(ItemID.Slimefish, 0.65f, new QuestFishCondition(ItemID.Slimefish)),
                new CatchWeight(ItemID.Salmon, 1f, new PoolSizeCondition(1001, true)),
                new CatchWeight(ItemID.Bass)
            });

            _possiblePools.Add(generalCatchPool);
        }

        private void PopulateCorruptionCatches() {
            CorruptionCatchPool corruptionPool = new CorruptionCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            corruptionPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Toxikarp, 0.15f, new HardmodeCondition()),
                new CatchWeight(ItemID.PurpleClubberfish, 0.34f),
                new CatchWeight(ItemID.Cursedfish, 0.65f, new QuestFishCondition(ItemID.Cursedfish)),
                new CatchWeight(ItemID.EaterofPlankton, 0.65f, new QuestFishCondition(ItemID.EaterofPlankton)),
                new CatchWeight(ItemID.InfectedScabbardfish, 0.65f, new QuestFishCondition(ItemID.InfectedScabbardfish)),
                new CatchWeight(ItemID.Ebonkoi, 0.75f)
            });

            _possiblePools.Add(corruptionPool);
        }

        private void PopulateCrimsonCatches() {
            CrimsonCatchPool crimsonPool = new CrimsonCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            crimsonPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Bladetongue, 0.15f, new HardmodeCondition()),
                new CatchWeight(ItemID.BloodyManowar, 0.65f, new QuestFishCondition(ItemID.BloodyManowar)),
                new CatchWeight(ItemID.Ichorfish, 0.65f, new QuestFishCondition(ItemID.Ichorfish)),
                new CatchWeight(ItemID.Hemopiranha, 0.85f),
                new CatchWeight(ItemID.CrimsonTigerfish)
            });

            _possiblePools.Add(crimsonPool);
        }

        private void PopulateHallowCatches() {
            HallowCatchPool hallowPool = new HallowCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            hallowPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.CrystalSerpent, 0.15f, new HardmodeCondition()),
                new CatchWeight(ItemID.ChaosFish, 0.34f, new HeightLevelCondition(2, true)),
                new CatchWeight(ItemID.MirageFish, 0.65f, new HeightLevelCondition(2, true), new QuestFishCondition(ItemID.MirageFish)),
                new CatchWeight(ItemID.Pixiefish, 0.65f, new HeightLevelCondition(1, false), new QuestFishCondition(ItemID.Pixiefish)),
                new CatchWeight(ItemID.Prismite, 0.67f),
                new CatchWeight(ItemID.UnicornFish, 0.65f, new QuestFishCondition(ItemID.UnicornFish)),
                new CatchWeight(ItemID.PrincessFish, 0.8f)
            });

            _possiblePools.Add(hallowPool);
        }

        private void PopulateSnowCatches() {
            SnowCatchPool snowPool = new SnowCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            snowPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Pengfish, 0.65f, new HeightLevelCondition(1, false), new QuestFishCondition(ItemID.Pengfish)),
                new CatchWeight(ItemID.TundraTrout, 0.65f, new HeightLevelCondition(1, null), new QuestFishCondition(ItemID.TundraTrout)),
                new CatchWeight(ItemID.Fishron, 0.65f, new HeightLevelCondition(2, true), new QuestFishCondition(ItemID.Fishron)),
                new CatchWeight(ItemID.MutantFlinxfin, 0.65f, new HeightLevelCondition(2, true), new QuestFishCondition(ItemID.MutantFlinxfin)),
                new CatchWeight(ItemID.FrostDaggerfish, 0.75f),
                new CatchWeight(ItemID.FrostMinnow, 0.85f),
                new CatchWeight(ItemID.AtlanticCod)
            });

            _possiblePools.Add(snowPool);
        }

        private void PopulateJungleCatches() {
            JungleCatchPool junglePool = new JungleCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            junglePool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Catfish, 0.65f, new HeightLevelCondition(1, null), new QuestFishCondition(ItemID.Catfish)),
                new CatchWeight(ItemID.Derpfish, 0.65f, new HeightLevelCondition(1, null), new QuestFishCondition(ItemID.Derpfish)),
                new CatchWeight(ItemID.TropicalBarracuda, 0.65f, new HeightLevelCondition(1, null), new QuestFishCondition(ItemID.TropicalBarracuda)),
                new CatchWeight(ItemID.Mudfish, 0.65f, new HeightLevelCondition(1, true), new QuestFishCondition(ItemID.Mudfish)),
                new CatchWeight(ItemID.VariegatedLardfish, 0.75f, new HeightLevelCondition(2, true)),
                new CatchWeight(ItemID.DoubleCod, 0.75f),
                new CatchWeight(ItemID.NeonTetra)
            });

            _possiblePools.Add(junglePool);
        }

        private void PopulateGlowingMushroomCatches() {
            GlowshroomCatchPool glowshroomPool = new GlowshroomCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            glowshroomPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.AmanitaFungifin, 0.65f, new QuestFishCondition(ItemID.AmanitaFungifin))
            });

            _possiblePools.Add(glowshroomPool);
        }

        private void PopulateOceanCatches() {
            OceanCatchPool oceanPool = new OceanCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            oceanPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.ReaverShark, 0.15f),
                new CatchWeight(ItemID.SawtoothShark, 0.15f),
                new CatchWeight(ItemID.PinkJellyfish, 0.45f),
                new CatchWeight(ItemID.Swordfish, 0.45f),
                new CatchWeight(ItemID.CapnTunabeard, 0.65f, new QuestFishCondition(ItemID.CapnTunabeard)),
                new CatchWeight(ItemID.Clownfish, 0.65f, new QuestFishCondition(ItemID.Clownfish)),
                new CatchWeight(ItemID.Shrimp, 0.75f),
                new CatchWeight(ItemID.Tuna),
                new CatchWeight(ItemID.RedSnapper),
                new CatchWeight(ItemID.Trout)
            });

            _possiblePools.Add(oceanPool);
        }

        private void PopulateOasisCatches() {
            DesertCatchPool desertPool = new DesertCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            desertPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.ScarabFish, 0.65f, new QuestFishCondition(ItemID.ScarabFish)),
                new CatchWeight(ItemID.ScorpioFish, 0.65f, new QuestFishCondition(ItemID.ScorpioFish)),
                new CatchWeight(ItemID.Oyster, 0.75f),
                new CatchWeight(ItemID.Flounder),
                new CatchWeight(ItemID.RockLobster)
            });

            _possiblePools.Add(desertPool);
        }

        private void PopulateSpaceCatches() {
            SpaceCatchPool spacePool = new SpaceCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            spacePool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Cloudfish, 0.65f, new QuestFishCondition(ItemID.Cloudfish)),
                new CatchWeight(ItemID.Wyverntail, 0.65f, new QuestFishCondition(ItemID.Wyverntail)),
                new CatchWeight(ItemID.Angelfish, 0.65f, new QuestFishCondition(ItemID.Angelfish)),
                new CatchWeight(ItemID.Damselfish, 0.85f)
            });

            _possiblePools.Add(spacePool);
        }

        private void PopulateSpaceOrSurfaceCatches() {
            SpaceOrSurfaceCatchPool spaceOrSurfacePool = new SpaceOrSurfaceCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            spaceOrSurfacePool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Harpyfish, 0.65f, new QuestFishCondition(ItemID.Harpyfish)),
                new CatchWeight(ItemID.FallenStarfish, 0.65f, new QuestFishCondition(ItemID.FallenStarfish)),
                new CatchWeight(ItemID.TheFishofCthulu, 0.65f, new QuestFishCondition(ItemID.TheFishofCthulu))
            });

            _possiblePools.Add(spaceOrSurfacePool);
        }

        private void PopulateSurfaceCatches() {
            SurfaceCatchPool surfacePool = new SurfaceCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            surfacePool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Bunnyfish, 0.65f, new QuestFishCondition(ItemID.Bunnyfish)),
                new CatchWeight(ItemID.DynamiteFish, 0.65f, new QuestFishCondition(ItemID.DynamiteFish)),
                new CatchWeight(ItemID.ZombieFish, 0.65f, new QuestFishCondition(ItemID.ZombieFish)),
                new CatchWeight(ItemID.Dirtfish, 0.65f, new QuestFishCondition(ItemID.Dirtfish))
            });

            _possiblePools.Add(surfacePool);
        }

        private void PopulateUndergroundCatches() {
            UndergroundCatchPool undergroundPool = new UndergroundCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            undergroundPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.GoldenCarp, 0.15f),
                new CatchWeight(ItemID.Rockfish, 0.25f),
                new CatchWeight(ItemID.BlueJellyfish, 0.45f, new NegationCondition(new HardmodeCondition())),
                new CatchWeight(ItemID.GreenJellyfish, 0.45f, new HardmodeCondition()),
                new CatchWeight(ItemID.Stinkfish, 0.67f),
                new CatchWeight(ItemID.Bonefish, 0.65f, new QuestFishCondition(ItemID.Bonefish)),
                new CatchWeight(ItemID.Batfish, 0.65f, new QuestFishCondition(ItemID.Batfish)),
                new CatchWeight(ItemID.Jewelfish, 0.65f, new QuestFishCondition(ItemID.Jewelfish)),
                new CatchWeight(ItemID.Spiderfish, 0.65f, new QuestFishCondition(ItemID.Spiderfish)),
                new CatchWeight(ItemID.ArmoredCavefish, 0.8f),
                new CatchWeight(ItemID.SpecularFish, 0.9f)
            });

            _possiblePools.Add(undergroundPool);
        }

        private void PopulateCavernsCatches() {
            CavernsCatchPool cavernsPool = new CavernsCatchPool {
                PotentialCatches = new List<CatchWeight>()
            };

            cavernsPool.PotentialCatches.AddRange(new CatchWeight[] {
                new CatchWeight(ItemID.Hungerfish, 0.65f, new QuestFishCondition(ItemID.Hungerfish)),
                new CatchWeight(ItemID.DemonicHellfish, 0.65f, new QuestFishCondition(ItemID.DemonicHellfish)),
                new CatchWeight(ItemID.GuideVoodooFish, 0.65f, new QuestFishCondition(ItemID.GuideVoodooFish)),
                new CatchWeight(ItemID.Fishotron, 0.65f, new QuestFishCondition(ItemID.Fishotron))
            });

            _possiblePools.Add(cavernsPool);
        }
    }
}