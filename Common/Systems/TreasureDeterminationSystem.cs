using FishingReborn.Custom.Structs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace FishingReborn.Common.Systems {
    /// <summary>
    /// System that separately handles which treasure/crate will be pulled up
    /// during the minigame if one appears.
    /// </summary>
    public class TreasureDeterminationSystem : ModSystem {
        /// <summary>
        /// List of ALL possible treasure.
        /// </summary>
        private List<TreasureData> _possibleTreasure;

        public override void Load() {
            _possibleTreasure = new List<TreasureData>() {
                //First, the "general" crates (wood/iron/gold):
                new TreasureData(ItemID.WoodenCrate, ItemID.WoodenCrateHard, 1f, (player, attempt) => !attempt.inLava),
                new TreasureData(ItemID.IronCrate, ItemID.IronCrateHard, 0.6f, (player, attempt) => !attempt.inLava),
                new TreasureData(ItemID.GoldenCrate, ItemID.GoldenCrateHard, 0.25f, (player, attempt) => !attempt.inLava),
                //Second, biome/region specific crates:
                new TreasureData(ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard, 0.5f, (player, attempt) => player.ZoneJungle),
                new TreasureData(ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard, 0.5f, (player, attempt) => player.ZoneSkyHeight || attempt.heightLevel == 0),
                new TreasureData(ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard, 0.5f, (player, attempt) => player.ZoneCorrupt),
                new TreasureData(ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard, 0.5f, (player, attempt) => player.ZoneCrimson),
                new TreasureData(ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard, 0.5f, (player, attempt) => player.ZoneHallow),
                new TreasureData(ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard, 0.5f, (player, attempt) => player.ZoneDungeon),
                new TreasureData(ItemID.FrozenCrate, ItemID.FrozenCrateHard, 0.5f, (player, attempt) => player.ZoneSnow),
                new TreasureData(ItemID.OasisCrate, ItemID.OasisCrateHard, 0.5f, (player, attempt) => player.ZoneDesert || player.ZoneUndergroundDesert),
                new TreasureData(ItemID.LavaCrate, ItemID.LavaCrateHard, 0.5f, (player, attempt) => attempt.inLava && attempt.CanFishInLava),
                new TreasureData(ItemID.OceanCrate, ItemID.OceanCrateHard, 0.5f, (player, attempt) => attempt.heightLevel <= 1 && (attempt.X < 380 || attempt.X > Main.maxTilesX - 380) && attempt.waterTilesCount > 1000),
                //Finally, non-crate treasures
                new TreasureData(ItemID.AlchemyTable, ItemID.AlchemyTable, 0.05f, (player, attempt) => player.ZoneDungeon)
            };
        }

        /// <summary>
        /// Retrieves a suitable treasure based on the passed in player.
        /// </summary>
        /// <param name="player"> The player in question. </param>
        /// <param name="attempt"> The current fishing attempt object in question. </param>
        public int RetrieveTreasure(Player player, FishingAttempt attempt) {
            WeightedRandom<int> randomizer = new WeightedRandom<int>();

            foreach (TreasureData possibleTreasure in _possibleTreasure) {
                if (possibleTreasure.catchRequirement.Invoke(player, attempt)) {
                    randomizer.Add(Main.hardMode ? possibleTreasure.hardModeType : possibleTreasure.normalModeType, possibleTreasure.treasureWeight);
                }
            }

            return randomizer;
        }
    }
}