using BepInEx;
using BOSpecialFools.Characters;
using System;

namespace BOSpecialFools
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string MOD_GUID = "157.Fools";
        public const string MOD_NAME = "157 FOOLS";
        public const string MOD_VERSION = "0.0.0";
        public const string MOD_PREFIX = "BOSpecialFools";

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
