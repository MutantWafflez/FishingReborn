using FishingReborn.Custom.Enums;
using FishingReborn.Custom.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;

namespace FishingReborn.Content.UI.Elements {
    /// <summary>
    /// UIElement that acts as a bare-bones UIImage for specifically being
    /// a fish icon.
    /// </summary>
    public class FishIconElement : UIElement {
        /// <summary>
        /// Whether or not the fish should currently be shaking.
        /// </summary>
        public bool shouldShake;

        /// <summary>
        /// What movement pattern this fish has.
        /// </summary>
        private FishMovementType _fishMovementType;

        /// <summary>
        /// How difficult this given fish is to catch. The higher, the more difficult.
        /// </summary>
        private int _fishDifficulty;

        /// <summary>
        /// The position that the fish will move towards.
        /// </summary>
        private float _targetPosition = -1f;

        /// <summary>
        /// The movement of the fish in fractional coordinates, per tick.
        /// </summary>
        private float _fishVelocity;

        /// <summary>
        /// The movement of the fish's velocity in fractional coordinates, per tick.
        /// </summary>
        private float _fishAcceleration;

        /// <summary>
        /// Additional acceleration on top of the normal fish acceleration in fractional
        /// coordinates, per tick.
        /// </summary>
        private float _floaterSinkerAcceleration;

        /// <summary>
        /// The sprite of fish.
        /// </summary>
        private readonly Asset<Texture2D> _fishSprite;

        /// <summary>
        /// This is the pixel height of the fishing zone Stardew Valley actually uses for calculations and such.
        /// Used for converting from Stardew values to our fractional (0f-1f) position.
        /// </summary>
        private readonly float _stardewMaxZoneHeight = 548f;

        public FishIconElement(CatchData catchData) {
            _fishMovementType = catchData.movementType;
            _fishDifficulty = catchData.fishDifficulty;

            Main.instance.LoadItem(ItemID.Bass);
            _fishSprite = TextureAssets.Item[ItemID.Bass];

            Width.Set(_fishSprite.Width(), 0f);
            Height.Set(_fishSprite.Height(), 0f);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            float position = VAlign;

            // This code is all Adapted from Stardew's own fish movement code for our Terrarian circumstances
            // Several things have been changed to keep this code (hopefully) uniquely different, however the logic stays as close to vanilla Stardew as possible

            // Firstly, Depending on fish difficulty, calculate a new target position if one is not selected (aka targetPosition == -1), with a higher chance if the movement type is non-Smooth
            if (Main.rand.NextFloat() < _fishDifficulty * (float)(_fishMovementType != FishMovementType.Smooth ? 1 : 20) / 4000f && (_fishMovementType != FishMovementType.Smooth || _targetPosition == -1f)) {
                _targetPosition = position + Main.rand.NextFloat(-position, 1f - position) * Math.Min(99f, _fishDifficulty + Main.rand.NextFloat(10f, 45f)) / 100f;
            }

            // Add additional movement acceleration for floater and sinker types.
            _floaterSinkerAcceleration = _fishMovementType switch {
                FishMovementType.Floater => Math.Max(_floaterSinkerAcceleration - 0.01f / _stardewMaxZoneHeight, -1.5f / _stardewMaxZoneHeight),
                FishMovementType.Sinker => Math.Min(_floaterSinkerAcceleration + 0.01f / _stardewMaxZoneHeight, 1.5f / _stardewMaxZoneHeight),
                _ => _floaterSinkerAcceleration
            };

            // If the distance between the target position and position is greater than a given threshold, change acceleration and velocity as normal
            if (Math.Abs(position - _targetPosition) >= 3f / _stardewMaxZoneHeight && _targetPosition != -1f) {
                _fishAcceleration = (_targetPosition - position) / (Main.rand.NextFloat(10f, 30f) + (100f - Math.Min(100f, _fishDifficulty)));
                _fishVelocity += (_fishAcceleration - _fishVelocity) / 5f;
            }
            // If the distance between the target position and position isn't above the threshold, check for a "dart" change depending on fish difficulty.
            else if (_fishMovementType != FishMovementType.Smooth && Main.rand.NextFloat() < _fishDifficulty / 2000f) {
                _targetPosition = position + (Main.rand.NextBool()
                    ? Main.rand.NextFloat(-100f / _stardewMaxZoneHeight, -51f / _stardewMaxZoneHeight)
                    : Main.rand.NextFloat(50f / _stardewMaxZoneHeight, 101f / _stardewMaxZoneHeight));
            }
            // If all else fails, the fish should be not moving, thus targetPos will be -1
            else {
                _targetPosition = -1f;
            }

            // Do another "dart" check for specifically dart fish, which has a higher chance to occur and is 2x as powerful compared to non-dart movement types
            if (_fishMovementType == FishMovementType.Dart && Main.rand.NextFloat() < _fishDifficulty / 1000f) {
                _targetPosition = position + (Main.rand.NextBool()
                    ? Main.rand.NextFloat((-100f - _fishDifficulty * 2f) / _stardewMaxZoneHeight, -51f / _stardewMaxZoneHeight)
                    : Main.rand.NextFloat(50f / _stardewMaxZoneHeight, (101f + _fishDifficulty * 2f) / _stardewMaxZoneHeight));
            }

            // Finally, add to the position based on velocity and acceleration after all is said and done
            _targetPosition = Math.Max(-1f, Math.Min(_targetPosition, 1f));
            position += _fishVelocity + _floaterSinkerAcceleration;

            // Also, clamp position towards the bounds, and update the VAlign with the new position
            position = MathHelper.Clamp(position, 0f, 1f);

            VAlign = position;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            //Add shake if told to.
            Vector2 shakeDisplacement = shouldShake
                ? new Vector2(0.5f * Main.rand.NextBool().ToDirectionInt(), 0.5f * Main.rand.NextBool().ToDirectionInt())
                : Vector2.Zero;

            spriteBatch.Draw(_fishSprite.Value, GetDimensions().Position() + shakeDisplacement, Color.White);
        }

        /// <summary>
        /// Resets this element to default values.
        /// </summary>
        public void ResetElement(CatchData newCatchData) {
            VAlign = 0.95f;

            _fishDifficulty = newCatchData.fishDifficulty;
            _fishMovementType = newCatchData.movementType;
            _targetPosition = -1f;
            _fishVelocity = _fishAcceleration = _floaterSinkerAcceleration = 0f;
        }
    }
}