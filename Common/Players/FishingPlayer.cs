using FishingReborn.Common.Systems;
using FishingReborn.Content.StatusEffects.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishingReborn.Common.Players {
    /// <summary>
    /// Player that handles the main fishing process.
    /// </summary>
    public class FishingPlayer : ModPlayer {
        /// <summary>
        /// The current fishing attempt in progress, if applicable. Null if not currently catching a fish.
        /// </summary>
        public FishingAttempt? currentFishingAttempt;

        /// <summary>
        /// Public accessory for whether or not this player is currently
        /// catching a fish.
        /// </summary>
        public bool IsCatchingFish => _isCatchingFish;

        /// <summary>
        /// Whether or not the player is currently in the waiting period between the
        /// "HIT!" and minigame.
        /// </summary>
        private bool _isWaitingForGame;

        /// <summary>
        /// The time left until the minigame starts after getting a successful
        /// "HIT!"
        /// </summary>
        private int _hitTimer;

        /// <summary>
        /// The fish type that is going to be attempted to be caught or is in the process of being caught
        /// by the player at a given moment. -1 if no fish or if not fishing.
        /// </summary>
        private int _fishToCatch;

        /// <summary>
        /// Whether or not this player is catching a fish.
        /// </summary>
        private bool _isCatchingFish;

        public override bool CanUseItem(Item item) => !_isCatchingFish;

        public override bool PreItemCheck() {
            // If the player is catching a fish, prevent item update code from happening and prevent item switching
            if (_isCatchingFish) {
                Player.itemAnimation = 2;

                return false;
            }

            return true;
        }

        public override void PostItemCheck() {
            Item selectedItem = Player.inventory[Player.selectedItem];

            if (_isCatchingFish && selectedItem.fishingPole > 0) {
                //Adapted vanilla code for moving the fishing pole, since it breaks when we return false in PreItemCheck. Excuse the magic numbers
                Rectangle heldItemFrame = Item.GetDrawHitbox(selectedItem.type, Player);

                Player.itemLocation.X = Player.position.X + Player.width * 0.5f + heldItemFrame.Width * 0.18f * Player.direction;
                Player.itemLocation.Y = Player.position.Y + 28f + Player.HeightOffsetHitboxCenter;
            }
        }

        public override void PostUpdate() {
            // If the player is catching a fish, then add the reel debuff which debilitates movement
            if (_isCatchingFish) {
                Player.AddBuff(ModContent.BuffType<ReelingDebuff>(), 5);
            }

            // End minigame if player dies during game
            if (_isCatchingFish && Player.dead) {
                FinishCatchingFish(false, false);
            }

            if (!_isWaitingForGame) {
                return;
            }

            if (_isCatchingFish && _hitTimer >= 0) {
                _hitTimer--;
            }
            else if (_isCatchingFish && _hitTimer <= 0) {
                _hitTimer = 0;
                _isWaitingForGame = false;

                FishingUISystem fishingUISystem = ModContent.GetInstance<FishingUISystem>();
                MinigameFishDataSystem minigameFishDataSystem = ModContent.GetInstance<MinigameFishDataSystem>();

                fishingUISystem.BeginMinigame(minigameFishDataSystem.GetDataOrDefault(_fishToCatch), Player);
            }
        }

        /// <summary>
        /// Triggers the catching fish process.
        /// </summary>
        /// <param name="bobber"> The bobber this catch is attributed to. </param>
        public void StartCatchingFish(Projectile bobber) {
            _isCatchingFish = true;

            _hitTimer = 60; // 1 second wait between HIT! and starting the game
            _isWaitingForGame = true;
            _fishToCatch = (int)bobber.ai[1];
            SoundEngine.PlaySound(SoundID.Item129);
            CombatText.NewText(new Rectangle((int)bobber.position.X, (int)bobber.position.Y, bobber.width, bobber.height), Color.Orange, "HIT!", true);
        }

        /// <summary>
        /// Finishes the process of catching a fish.
        /// </summary>
        /// <param name="successful"> Whether or not the player has caught the fish successfully. </param>
        /// <param name="caughtCrate"> Whether or not the player has caught a crate, if applicable. </param>
        public void FinishCatchingFish(bool successful, bool caughtCrate) {
            FishingUISystem fishingUISystem = ModContent.GetInstance<FishingUISystem>();

            //Update bobber projectiles to reel or kill them if player is unsuccessful
            for (int i = 0; i < Main.maxProjectiles; i++) {
                Projectile projectile = Main.projectile[i];
                if (!projectile.active || projectile.owner != Player.whoAmI || !projectile.bobber) {
                    continue;
                }

                if (successful) {
                    // Trigger reel in
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = _fishToCatch;
                }
                else {
                    //Self-explanatory
                    projectile.Kill();
                }
            }

            //Spawn crate on player if they were successful in catching it
            //If you were wondering where currentFishingAttempt is set, it is set during our FishingCheck method detour, since that is the only place we can access it.
            //Check it out in Common/Patches/FishingPatches.cs
            if (caughtCrate && currentFishingAttempt is not null) {
                TreasureDeterminationSystem treasureDeterminationSystem = ModContent.GetInstance<TreasureDeterminationSystem>();

                Player.QuickSpawnItem(treasureDeterminationSystem.RetrieveTreasure(Player, currentFishingAttempt.Value));
            }

            //Then, reset all applicable fields
            _isCatchingFish = false;
            _fishToCatch = -1;
            currentFishingAttempt = null;
            Player.itemAnimation = 0;

            //Finally, close UI
            fishingUISystem.EndMinigame();
        }
    }
}