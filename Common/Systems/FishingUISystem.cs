using System.Collections.Generic;
using FishingReborn.Content.UI;
using FishingReborn.Custom.Enums;
using FishingReborn.Custom.Structs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace FishingReborn.Common.Systems {
    /// <summary>
    /// ModSystem that communicates and holds data about the fishing UI.
    /// </summary>
    [Autoload(Side = ModSide.Client)]
    public class FishingUISystem : ModSystem {
        public UserInterface fishingInterface;
        public GameTime lastGameTime;

        public override void Load() {
            fishingInterface = new UserInterface();
        }

        public override void Unload() {
            fishingInterface = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Fishing Reborn: Mini-game",
                    delegate {
                        if (lastGameTime != null && fishingInterface?.CurrentState != null) {
                            fishingInterface.Draw(Main.spriteBatch, lastGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void UpdateUI(GameTime gameTime) {
            lastGameTime = gameTime;
            if (fishingInterface?.CurrentState != null) {
                fishingInterface.Update(gameTime);
            }
        }

        /// <summary>
        /// Begins the minigame by opening the UI and restarting it.
        /// </summary>
        public void BeginMinigame(CatchData catchData, Player catchingPlayer) {
            fishingInterface.SetState(new FishingState(catchData, catchingPlayer));
        }

        /// <summary>
        /// Ends the minigame by closing the UI.
        /// </summary>
        public void EndMinigame() {
            fishingInterface.SetState(null);
        }
    }
}