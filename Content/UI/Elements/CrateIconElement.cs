using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;

namespace FishingReborn.Content.UI.Elements {
    /// <summary>
    /// UIElement that acts as a bare-bones UIImage for specifically being
    /// a crate icon with additional functionality.
    /// </summary>
    public class CrateIconElement : UIElement {
        /// <summary>
        /// Whether or not the catch bar is current over the crate.
        /// </summary>
        public bool barOverCrate;

        /// <summary>
        /// Whether or not the crate has been caught. Will become true and stay
        /// true permanently once progress has reached 100%.
        /// </summary>
        public bool crateCaught;

        /// <summary>
        /// Whether or not the crate is currently in the process of appearing.
        /// </summary>
        public bool isAppearing;

        /// <summary>
        /// The current scale of the icon.
        /// </summary>
        public float iconScale;

        /// <summary>
        /// The bar that shows the progress of "catching" this crate.
        /// </summary>
        private HorizontalProgressBarElement _catchProgressBar;

        /// <summary>
        /// The sprite of a crate.
        /// </summary>
        private readonly Asset<Texture2D> _crateSprite;

        /// <summary>
        /// An actual crate item object in order to use for drawing.
        /// </summary>
        private readonly Item _crateItem;

        public CrateIconElement() {
            Main.instance.LoadItem(ItemID.WoodenCrate);
            _crateSprite = TextureAssets.Item[ItemID.WoodenCrate];

            Width.Set(_crateSprite.Width(), 0f);
            Height.Set(_crateSprite.Height(), 0f);

            _crateItem = new Item(ItemID.WoodenCrate);

            _catchProgressBar = new HorizontalProgressBarElement(0f, false) {
                HAlign = 0.5f
            };
            _catchProgressBar.Width.Set(Width.Pixels * 1.5f, 0f);
            _catchProgressBar.Height.Set(10f, 0f);
            _catchProgressBar.Top.Set(-10f, 0f);
            Append(_catchProgressBar);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (isAppearing) {
                if ((iconScale += 5f / 60f) >= 1f) {
                    iconScale = 1f;
                    isAppearing = false;
                }
            }

            if (crateCaught) {
                return;
            }

            // roughly 45% gain per second
            float positiveRateOfChange = 0.45f / 60f;
            // roughly 60% loss per second
            float negativeRateOfChange = -0.6f / 60f;

            _catchProgressBar.percentProgress = MathHelper.Clamp(_catchProgressBar.percentProgress + (barOverCrate ? positiveRateOfChange : negativeRateOfChange), 0f, 1f);
            if (_catchProgressBar.percentProgress >= 1f) {
                crateCaught = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!crateCaught) {
                base.Draw(spriteBatch);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            Vector2 shakeDisplacement = barOverCrate
                ? new Vector2(0.5f * Main.rand.NextBool().ToDirectionInt(), 0.5f * Main.rand.NextBool().ToDirectionInt())
                : Vector2.Zero;

            Main.DrawItemIcon(spriteBatch, _crateItem, GetDimensions().Center() + shakeDisplacement, Color.White, _crateSprite.Width() * iconScale);
        }
    }
}