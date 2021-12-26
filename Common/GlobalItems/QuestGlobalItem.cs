using Terraria.ID;
using Terraria.ModLoader;

namespace FishingReborn.Common.GlobalItems {
    /// <summary>
    /// Global item that is used for changes to vanilla quest fish.
    /// </summary>
    public class QuestGlobalItem : GlobalItem {
        public override void AnglerChat(int type, ref string chat, ref string catchLocation) {
            //First: Dirt fish can now only be caught on the surface, and no longer in the underground
            if (type == ItemID.Dirtfish) {
                catchLocation = "(Caught in Surface)";
            }
        }
    }
}