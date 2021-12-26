using System;
using System.Linq;
using FishingReborn.Common.Players;
using FishingReborn.Common.Systems;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace FishingReborn.Common.Patches {
    /// <summary>
    /// Class that holds IL/On patches for fishing type matters.
    /// </summary>
    public class FishingPatches : ILoadable {
        public void Load(Mod mod) {
            IL.Terraria.Player.ItemCheck_CheckFishingBobber_PullBobber += PullBobberCheck;
            On.Terraria.Projectile.AI_061_FishingBobber += PreventBobberMovementWhileCatching;
            On.Terraria.Projectile.FishingCheck_RollItemDrop += RerouteItemDetermination;
        }

        public void Unload() { }

        private void PullBobberCheck(ILContext il) {
            //For this edit, we want to overtake the "pull bobber" effect of fishing rods when catching a fish and cause it
            // to freeze until the player finishes the fishing minigame. 
            ILCursor c = new ILCursor(il);

            // Navigate to bottom of method
            if (c.TryGotoNext(i => i == c.Instrs.Last())) {
                //Emit bobber projectile
                c.Emit(OpCodes.Ldarg_1);

                c.EmitDelegate<Action<Projectile>>(bobber => {
                    // If bobber is not going to snap (aka ai[0] != 2f) and is not trash, start minigame
                    if (bobber.ai[0] != 2f && (bobber.ai[1] < ItemID.OldShoe || bobber.ai[1] > ItemID.TinCan)) {
                        Main.player[bobber.owner].GetModPlayer<FishingPlayer>().StartCatchingFish(bobber);
                    }
                });
            }
        }

        private void PreventBobberMovementWhileCatching(On.Terraria.Projectile.orig_AI_061_FishingBobber orig, Projectile self) {
            // If player is catching a fish, just have the bobber continue to bob instead of reel in
            if (Main.player[self.owner].GetModPlayer<FishingPlayer>().IsCatchingFish) {
                self.ai[0] = 0;
                self.ai[1] = -10f;
            }

            orig(self);
        }

        private void RerouteItemDetermination(On.Terraria.Projectile.orig_FishingCheck_RollItemDrop orig, Projectile self, ref FishingAttempt fisher) {
            if (fisher.rolledEnemySpawn > 0) {
                return;
            }

            fisher.rolledItemDrop = ModContent.GetInstance<CatchDeterminationSystem>().DetermineCatch(fisher, self);
            Main.player[self.owner].GetModPlayer<FishingPlayer>().currentFishingAttempt = fisher;
        }
    }
}