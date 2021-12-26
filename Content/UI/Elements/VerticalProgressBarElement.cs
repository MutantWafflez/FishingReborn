using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace FishingReborn.Content.UI.Elements {
    /// <summary>
    /// Element that is a vertical progress bar.
    /// </summary>
    public class VerticalProgressBarElement : UIElement {
        public Color backgroundColor = Color.Black;

        public float percentProgress;
        public bool drawBackground;

        public VerticalProgressBarElement(float startingProgress, bool drawBackground = true) {
            percentProgress = startingProgress;
            this.drawBackground = drawBackground;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            CalculatedStyle dimensions = GetDimensions();
            //Draw background
            if (drawBackground) {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, dimensions.ToRectangle(), backgroundColor);
            }

            //Draw colored progress bar
            for (float i = 0; i < dimensions.Height * percentProgress; i++) {
                Vector2 drawPos = dimensions.Position() - new Vector2(0, i - dimensions.Height);
                Rectangle drawBox = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)dimensions.Width, 1);

                Color drawColor = default;
                Color redColor = Color.DarkRed;
                Color yellowColor = Color.Yellow;
                Color greenColor = Color.Lime;

                if (percentProgress <= 0.5f) {
                    float adjustedProgress = percentProgress / 0.5f;

                    drawColor.R = (byte)MathHelper.Lerp(redColor.R, yellowColor.R, adjustedProgress);
                    drawColor.G = (byte)MathHelper.Lerp(redColor.G, yellowColor.G, adjustedProgress);
                    drawColor.B = (byte)MathHelper.Lerp(redColor.B, yellowColor.B, adjustedProgress);
                    drawColor.A = (byte)MathHelper.Lerp(redColor.A, yellowColor.A, adjustedProgress);
                }
                else {
                    float adjustedProgress = (percentProgress - 0.5f) / 0.5f;

                    drawColor.R = (byte)MathHelper.Lerp(yellowColor.R, greenColor.R, adjustedProgress);
                    drawColor.G = (byte)MathHelper.Lerp(yellowColor.G, greenColor.G, adjustedProgress);
                    drawColor.B = (byte)MathHelper.Lerp(yellowColor.B, greenColor.B, adjustedProgress);
                    drawColor.A = (byte)MathHelper.Lerp(yellowColor.A, greenColor.A, adjustedProgress);
                }

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawBox, drawColor);
            }
        }
    }
}