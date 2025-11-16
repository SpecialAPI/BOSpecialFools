using BepInEx;
using BOSpecialFools.Characters;
using System;

namespace BOSpecialFools
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string MOD_GUID = "SpecialAPI.BOSpecialFools";
        public const string MOD_NAME = "SpecialAPI's Fools";
        public const string MOD_VERSION = "0.0.0";
        public const string MOD_PREFIX = "BOSpecialFools";

        public void Awake()
        {
            ProfileManager.RegisterMod(MOD_GUID, MOD_PREFIX);

            AStar.Init();
            Charline.Init();
        }

        public void Start()
        {
            RankedAbilitiesDatabase.Init();
        }
    }
}
