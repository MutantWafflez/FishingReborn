using FishingReborn.Custom.Enums;
using FishingReborn.Custom.Structs;
using Hjson;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace FishingReborn.Common.Systems {
    /// <summary>
    /// System that handles the minigame catch data for all of the different types of fish/catches
    /// in the game.
    /// </summary>
    public class MinigameFishDataSystem : ModSystem {
        /// <summary>
        /// Holds all the <see cref="CatchData"/> for the mod.
        /// </summary>
        private Dictionary<int, CatchData> _catchData;

        public override void Load() {
            Stream catchDataStream = Mod.GetFileStream("Content/Data/CatchData.hjson");
            JsonObject jsonCatchData = HjsonValue.Load(catchDataStream)["CatchData"].Qo();
            catchDataStream.Close();

            _catchData = new Dictionary<int, CatchData>();

            foreach (string key in jsonCatchData.Keys) {
                if (!int.TryParse(key, out int intKey)) {
                    continue;
                }

                JsonObject specificFishData = jsonCatchData[key].Qo();

                if (specificFishData.TryGetValue("difficulty", out JsonValue difficulty) && specificFishData.TryGetValue("movementType", out JsonValue movementType)) {
                    _catchData[intKey] = new CatchData(difficulty.Qi(), (FishMovementType)movementType.Qi());
                }
                else {
                    _catchData[intKey] = CatchData.CreateDefaultCatchData();
                }
            }
        }

        /// <summary>
        /// Returns the <see cref="CatchData"/> associated with specified item/catch ID. If there no key of that ID or if the accessing fails for
        /// some other reason, a default <see cref="CatchData"/> is returned.
        /// </summary>
        /// <param name="itemID"> The ID of the item being caught. </param>
        public CatchData GetDataOrDefault(int itemID) => _catchData.TryGetValue(itemID, out CatchData catchData) ? catchData : CatchData.CreateDefaultCatchData();
    }
}