using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace FishingReborn.Common.Configs {
    /// <summary>
    /// Config that handles personalization and other client side matters.
    /// </summary>
    [Label("$Mods.FishingReborn.Configs.ClientSide.ConfigName")]
    public class FishingClientSideConfig : ModConfig {
        [Header("$Mods.FishingReborn.Configs.ClientSide.PreferenceSettingsHeader")]
        [Label("$Mods.FishingReborn.Configs.ClientSide.PauseDuringMinigameLabel")]
        [Tooltip("$Mods.FishingReborn.Configs.ClientSide.PauseDuringMinigameTooltip")]
        [DefaultValue(true)]
        public bool pauseDuringMinigame;

        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}