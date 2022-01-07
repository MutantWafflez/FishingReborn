using FishingReborn.Common.Configs;
using FishingReborn.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishingReborn.Custom.Utils {
    /// <summary>
    /// Utils class that holds helper methods for Players.
    /// </summary>
    public static class PlayerUtils {
        /// <summary>
        /// Returns whether or not, at the current moment, the game should pause for this player
        /// during the fishing minigame.
        /// </summary>
        public static bool ShouldPauseGame(this Player player) => Main.netMode == NetmodeID.SinglePlayer && player.GetModPlayer<FishingPlayer>().IsPlayingMinigame && ModContent.GetInstance<FishingClientSideConfig>().pauseDuringMinigame;
    }
}