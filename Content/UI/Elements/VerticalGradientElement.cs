using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace FishingReborn.Content.UI.Elements {
    /// <summary>
    /// Element used to create a rectangular, vertical gradient.
    /// </summary>
    public class VerticalGradientElement : UIElement {
        public Color topGradientColor;
        public Color bottomGradientColor;

        public VerticalGradientElement() {
            topGradientColor = Color.White;
            bottomGradientColor = Color.Black;
        }

        public VerticalGradientElement(Color topGradientColor, Color bottomGradientColor) {
            this.topGradientColor = topGradientColor;
            this.bottomGradientColor = bottomGradientColor;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            CalculatedStyle dimensions = GetDimensions();

            //Draws the gradient from bottom down.
            for (int i = 0; i < dimensions.Height; i++) {
                Vector2 drawPos = dimensions.Position() + new Vector2(0, i);
                Rectangle drawBox = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)dimensions.Width, 1);
                float percentCompletion = i / dimensions.Height;

                Color gradientColor = default;
                gradientColor.R = (byte)MathHelper.Lerp(topGradientColor.R, bottomGradientColor.R, percentCompletion);
                gradientColor.G = (byte)MathHelper.Lerp(topGradientColor.G, bottomGradientColor.G, percentCompletion);
                gradientColor.B = (byte)MathHelper.Lerp(topGradientColor.B, bottomGradientColor.B, percentCompletion);
                gradientColor.A = (byte)MathHelper.Lerp(topGradientColor.A, bottomGradientColor.A, percentCompletion);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawBox, gradientColor);
            }
        }
    }
}