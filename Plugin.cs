using BepInEx;
using BOSpecialFools.Characters;
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
        public const string MOD_GUID = "157.Fools";
        public const string MOD_NAME = "157 FOOLS";
        public const string MOD_VERSION = "0.0.1";
        public const string MOD_PREFIX = "157Fools";

        public static Harmony HarmonyInstance;

        public void Awake()
        {
            ProfileManager.RegisterMod(MOD_GUID, MOD_PREFIX);

            HarmonyInstance = new Harmony(MOD_GUID);
            HarmonyInstance.PatchAll();
            

            //AStar.Init();
            //Charline.Init();
            Wreath.Init();
        }

        public void Start()
        {
            RankedAbilitiesDatabase.Init();
        }
    }
}
