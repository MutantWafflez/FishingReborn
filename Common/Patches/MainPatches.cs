using FishingReborn.Custom.Utils;
using Terraria;
using Terraria.ModLoader;

namespace FishingReborn.Common.Patches {
    /// <summary>
    /// Class that handles miscellaneous patches in the Main class.
    /// </summary>
    public class MainPatches : ILoadable {
        public void Load(Mod mod) {
            On.Terraria.Main.CanPauseGame += CanPauseGame;
        }

        public void Unload() { }

        private bool CanPauseGame(On.Terraria.Main.orig_CanPauseGame orig) => orig() || Main.LocalPlayer.ShouldPauseGame();
    }
}