using BepInEx;
using System;

namespace BOSpecialFools
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string MOD_GUID = "SpecialAPI.BOSpecialFools";
        public const string MOD_NAME = "SpecialAPI's Fools";
        public const string MOD_VERSION = "0.0.0";

        public void Awake()
        {
        }
    }
}
