using FishingReborn.Common.Players;
using FishingReborn.Content.UI.Elements;
using FishingReborn.Custom.Structs;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace FishingReborn.Content.UI {
    /// <summary>
    /// UIState class that handles the fishing minigame.
    /// </summary>
    public class FishingState : UIState {
        /// <summary>
        /// The background UIPanel, the farthest back.
        /// </summary>
        public UIPanel backdrop;

        /// <summary>
        /// The gradient that serves as the background for the fish.
        /// </summary>
        public VerticalGradientElement fishZoneBackground;

        /// <summary>
        /// Visual to show the visual progress of the catching progress.
        /// </summary>
        public VerticalProgressBarElement catchProgressBar;

        /// <summary>
        /// The movable/interactable bar that the player can move in order to catch the
        /// fish.
        /// </summary>
        public PlayerFishBarElement playerCatchBar;

        /// <summary>
        /// The fish that will move around and the player must keep bar behind.
        /// </summary>
        public FishIconElement fishIcon;

        /// <summary>
        /// The crate icon that can potentially appear as a side mission when catching
        /// a fish.
        /// </summary>
        public CrateIconElement crateIcon;

        /// <summary>
        /// The tracked sound instance of the fishing rod reeling sound effect.
        /// </summary>
        private SlotId _reelSoundInstance;

        /// <summary>
        /// The percentage that the the progress bar starts at when first
        /// starting the minigame.
        /// </summary>
        private readonly float _startingProgressPercent = 0.234f;

        public FishingState(CatchData catchData, Player catchingPlayer) {
            //First, add elements
            backdrop = new UIPanel {
                VAlign = 0.5f,
                HAlign = 0.5f,
                BackgroundColor = Color.White * 0.7f,
                BorderColor = Color.LightGray * 0.5f
            };
            backdrop.Width.Set(140f, 0f);
            backdrop.Height.Set(520f, 0f);
            backdrop.Left.Set(-160f, 0f);
            Append(backdrop);

            fishZoneBackground = new VerticalGradientElement(Color.SkyBlue, Color.DarkBlue) {
                VAlign = 0.5f,
                HAlign = 0.5f
            };
            fishZoneBackground.Height.Set(495f, 0f);
            fishZoneBackground.Width.Set(40f, 0f);
            backdrop.Append(fishZoneBackground);

            catchProgressBar = new VerticalProgressBarElement(_startingProgressPercent) {
                VAlign = 0.5f,
                HAlign = 0.5f
            };
            catchProgressBar.Left.Set(40f, 0f);
            catchProgressBar.Width.Set(20f, 0f);
            catchProgressBar.Height = fishZoneBackground.Height;
            backdrop.Append(catchProgressBar);

            float playerFishingSkill = catchingPlayer.GetFishingConditions().FinalFishingLevel;
            playerCatchBar = new PlayerFishBarElement {
                Width = fishZoneBackground.Width
            };
            playerCatchBar.Height.Set(0f, MathHelper.Clamp(playerFishingSkill / 300f * 0.9f, 0.175f, 0.55f));
            fishZoneBackground.Append(playerCatchBar);

            fishIcon = new FishIconElement(catchData) {
                HAlign = 0.5f,
                VAlign = 0.95f
            };
            fishZoneBackground.Append(fishIcon);

            //Crate spawn check
            if (Main.rand.NextFloat() <= playerFishingSkill / 305f + (catchingPlayer.cratePotion ? 0.2f : 0f)) {
                crateIcon = new CrateIconElement {
                    VAlign = Main.rand.NextFloat(0.25f, 0.55f),
                    HAlign = 0.5f,
                    isAppearing = true,
                    iconScale = 0f
                };
                fishZoneBackground.Append(crateIcon);

                SoundEngine.PlaySound(SoundID.Grab);
            }

            //Then, subscribe to events for the player catch bar
            OnMouseDown += delegate { playerCatchBar.playerHoldingDownLMB = true; };
            OnMouseUp += delegate { playerCatchBar.playerHoldingDownLMB = false; };
        }

        public override void Update(GameTime gameTime) {
            // Only update if the game has focus and in single player (will still keep going in multiplayer)
            if (!Main.hasFocus && Main.netMode == NetmodeID.SinglePlayer) {
                return;
            }
            base.Update(gameTime);

            // Get zone data
            Rectangle catchBarZone = playerCatchBar.GetDimensions().ToRectangle();
            catchBarZone.Height += 1; //  bit annoying here, but for some reason if both the bar and fish are at exactly VAlign = 1, they don't intersect unless you do this
            Rectangle fishIconZone = fishIcon.GetDimensions().ToRectangle();

            bool barContainsFish = catchBarZone.Contains(fishIconZone);

            //If the fish has entered catch bar, play "enter" sound
            if (barContainsFish && !playerCatchBar.barOverFish) {
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(ModContent.GetInstance<FishingReborn>(), "Assets/Audio/Sounds/EnterFishBar"));
            }
            //Otherwise, the fish has exited the catch bar, play "exit" sound
            else if (!barContainsFish && playerCatchBar.barOverFish) {
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(ModContent.GetInstance<FishingReborn>(), "Assets/Audio/Sounds/ExitFishBar"));
            }
            //HandleReelSFX(barContainsFish); to be re-added when sound_fix is merged

            //If the bar is intersecting the fish, update accordingly
            playerCatchBar.barOverFish = fishIcon.shouldShake = barContainsFish;
            //Do separate check for crates
            if (crateIcon is not null) {
                Rectangle crateIconZone = crateIcon.GetDimensions().ToRectangle();
                crateIcon.barOverCrate = catchBarZone.Contains(crateIconZone);
            }

            //Finally, update the progress bar
            UpdateProgressBar(barContainsFish);
        }

        /// <summary>
        /// Handles SFX for the minigame. Includes the reeling sound, namely.
        /// </summary>
        /// <param name="barContainsFish"> Whether or not the catch bar is over the fish. </param>
        private void HandleReelSFX(bool barContainsFish) {
            // To be re-added when sound_fix is merged
            ActiveSound reelingSound = SoundEngine.GetActiveSound(_reelSoundInstance);

            if (barContainsFish && !(reelingSound?.IsPlaying ?? false)) {
                // Play tracked sound here
            }
            else if (!barContainsFish && (reelingSound?.IsPlaying ?? false)) {
                reelingSound.Stop();
            }
        }

        /// <summary>
        /// Updates the progress bar either up or down by the passed in boolean value.
        /// </summary>
        /// <param name="barContainsFish"> Whether or not the catch bar is over the fish. </param>
        private void UpdateProgressBar(bool barContainsFish) {
            // 12.25% gain per second
            float positiveRateOfChange = 0.1225f / 60f;
            // 14.35% loss per second
            float negativeRateOfChange = -0.1435f / 60f;

            catchProgressBar.percentProgress += barContainsFish ? positiveRateOfChange : negativeRateOfChange;

            FishingPlayer fishingPlayer = Main.LocalPlayer.GetModPlayer<FishingPlayer>();
            if (catchProgressBar.percentProgress >= 1f) {
                fishingPlayer.FinishCatchingFish(true, crateIcon?.crateCaught ?? false);
            }
            else if (catchProgressBar.percentProgress <= 0f) {
                fishingPlayer.FinishCatchingFish(false, false);

                //Play fail sound
                SoundEngine.PlaySound(SoundID.Item130);
            }
        }
    }
}