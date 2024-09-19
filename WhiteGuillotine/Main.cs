using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2;
using UnityEngine;
using BepInEx.Bootstrap;

namespace WhiteGuillotine
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.Zenithrium.vanillaVoid", BepInDependency.DependencyFlags.SoftDependency)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "prodzpod";
        public const string PluginName = "WhiteGuillotine";
        public const string PluginVersion = "1.0.0";
        public static ManualLogSource Log;
        public static PluginInfo pluginInfo;
        public static ConfigFile Config;
        public static ConfigEntry<float> Threshold;
        public static ConfigEntry<float> ThresholdPerStack;
        private static AssetBundle _assetBundle;
        public static AssetBundle AssetBundle
        {
            get
            {
                if (_assetBundle == null)
                    _assetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pluginInfo.Location), "riskymonkey"));
                return _assetBundle;
            }
        }

        public void Awake()
        {
            pluginInfo = Info;
            Log = Logger;
            Config = new ConfigFile(System.IO.Path.Combine(Paths.ConfigPath, PluginGUID + ".cfg"), true);
            Threshold = Config.Bind("General", "Threshold", 13f, "yeah");
            ThresholdPerStack = Config.Bind("General", "Threshold per stack", 13f, "yeah");
            On.RoR2.ItemCatalog.SetItemDefs += (orig, defs) =>
            {
                RoR2Content.Items.ExecuteLowHealthElite._itemTierDef = ItemTierCatalog.GetItemTierDef(ItemTier.Tier1);
                RoR2Content.Items.ExecuteLowHealthElite.pickupIconSprite = AssetBundle.LoadAsset<Sprite>("Assets/texExecuteLowHealthElite.png");
                RoR2Content.Items.ExecuteLowHealthElite.unlockableDef.achievementIcon = AssetBundle.LoadAsset<Sprite>("Assets/unlocks/texItemExecuteLowHealthElite.png");
                orig(defs);
            };
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                orig(self);
                var count = self.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite);
                if (count > 0)
                    self.executeEliteHealthFraction = Util.ConvertAmplificationPercentageIntoReductionPercentage(Threshold.Value + (ThresholdPerStack.Value * (count - 1))) / 100f;
            };
            On.RoR2.ItemCatalog.SetItemDefs += (orig, defs) => { if (Chainloader.PluginInfos.ContainsKey("com.Zenithrium.vanillaVoid")) PatchExeblade.Patch(); orig(defs); };
        }
    }
}
