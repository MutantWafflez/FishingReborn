using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishingReborn.Content.StatusEffects.Debuffs {
    public class ReelingDebuff : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.chilled = true;
            player.wingTime = 0f;
            player.moveSpeed *= 0.25f;
        }
    }
}