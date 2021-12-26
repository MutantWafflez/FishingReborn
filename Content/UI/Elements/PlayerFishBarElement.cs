using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace FishingReborn.Content.UI.Elements {
    /// <summary>
    /// Element that acts as the movement bar to catch the fish in the Fishing minigame.
    /// </summary>
    public class PlayerFishBarElement : UIPanel {
        /// <summary>
        /// The velocity of the bar, on the vertical scale. Remember negative is up, positive is down.
        /// It is measured in fraction moved per tick.
        /// </summary>
        public float verticalVelocity;

        /// <summary>
        /// Whether or not the player is holding down the Left Mouse Button.
        /// </summary>
        public bool playerHoldingDownLMB;

        /// <summary>
        /// Whether or not the bar is over the fish, used for visually changing the bar.
        /// </summary>
        public bool barOverFish;

        public PlayerFishBarElement() {
            HAlign = 0.5f;
            VAlign = 1f;
            BackgroundColor = Color.LimeGreen;
            BorderColor = Color.Black;
            Height.Set(0f, 0.175f);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            //Change bar visually if bar is not over the fish
            BackgroundColor = Color.LimeGreen * (barOverFish ? 1f : 0.8f);

            //Simple velocity clamping and adding here, nothing to write home about
            VAlign = MathHelper.Clamp(VAlign + verticalVelocity, 0f, 1f);

            // If falling downwards at fast enough speed and hit bottom with no player interference, bounce!
            if (VAlign == 1f && verticalVelocity >= 0.0025) {
                verticalVelocity = -verticalVelocity * 0.55f;
                // Make the tapping sound of hitting the bottom
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(ModContent.GetInstance<FishingReborn>(), "Assets/Audio/Sounds/GroundTap"));
            }

            //Reset Velocity if sitting at top or bottom
            if (VAlign == 1f && verticalVelocity > 0f || VAlign == 0f && verticalVelocity < 0f) {
                verticalVelocity = 0f;
            }
            else {
                verticalVelocity = MathHelper.Clamp(verticalVelocity, -0.05f, 0.05f);
            }

            // Finally, update velocity based on player input
            if (playerHoldingDownLMB) {
                verticalVelocity -= 1f / 1600f;
            }
            else if (VAlign != 1f) {
                verticalVelocity += 1f / 2400f;
            }
        }
    }
}