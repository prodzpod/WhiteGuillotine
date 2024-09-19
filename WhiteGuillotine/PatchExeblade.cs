using BepInEx.Bootstrap;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteGuillotine
{
    internal class PatchExeblade
    {
        public static void Patch()
        {
            // oh my God which item mod template did this why is Nothing static i *LOVE* all of you
            vanillaVoid.Items.ItemBase exeblade = (Chainloader.PluginInfos[vanillaVoid.vanillaVoidPlugin.ModGuid].Instance as vanillaVoid.vanillaVoidPlugin).Items.Find(x => x.ItemLangTokenName == "EXEBLADE_ITEM");
            if (exeblade != null) exeblade.ItemDef._itemTierDef = ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1);
        }
    }
}
