using BepInEx;
using BOSpecialFools.Characters;
using BOSpecialFools.Items;
using Grimoire;
using System;

namespace BOSpecialFools
{
    [BepInDependency(BrutalAPI.BrutalAPI.GUID)]
    [BepInDependency(PentaclePlugin.MOD_GUID)]
    [BepInDependency(GrimoirePlugin.MOD_GUID)]
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string MOD_GUID        = "157.Fools";
        public const string MOD_NAME        = "157 FOOLS";
        public const string MOD_VERSION     = "0.0.2";
        public const string MOD_PREFIX      = "157Fools";

        public static readonly ModProfile Profile       = GenerateProfile();
        public static readonly AssetBundle Bundle       = Profile.AssetBundle;
        public static readonly Harmony HarmonyInstance  = new(MOD_GUID);

        public void Awake()
        {
            HarmonyInstance.PatchAll();
            
            // Characters
            Wreath.Init();

            // Items
            OldBleach.Init();
            TrueZeal.Init();
            Item19.Init();
            KillingMachine.Init();

            FreeFoolEvents.Init();
        }

        public void Start()
        {
            RankedAbilitiesDatabase.Init();
        }

        private static ModProfile GenerateProfile()
        {
            var prof = ProfileManager.RegisterMod(MOD_GUID, string.Empty);
            prof.LoadAssetBundle("157fools");

            return prof;
        }
    }
}
