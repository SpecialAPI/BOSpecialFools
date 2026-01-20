using BOSpecialFools.CustomTrigger.Args;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    public static class OnPassivePopup
    {
        [HarmonyPatch(typeof(ShowPassiveInformationUIAction), nameof(ShowPassiveInformationUIAction.Execute))]
        [HarmonyPostfix]
        public static IEnumerator OnPassivePopup_SinglePassive_PassThroughPostfix(IEnumerator enumerator, ShowPassiveInformationUIAction __instance, CombatStats stats)
        {
            yield return enumerator;

            if (stats.TryGetUnit(__instance._id, __instance._isUnitCharacter, out var unit))
                CombatManager.Instance.PostNotification(LocalCustomTriggers.OnPassivePopup, unit, new OnPassivePopupReference(__instance._passiveName, __instance._passiveSprite));
        }

        [HarmonyPatch(typeof(ShowMultiplePassiveInformationUIAction), nameof(ShowMultiplePassiveInformationUIAction.Execute))]
        [HarmonyPostfix]
        public static IEnumerator OnPassivePopup_MultiplePassives_PassThroughPostfix(IEnumerator enumerator, ShowMultiplePassiveInformationUIAction __instance, CombatStats stats)
        {
            yield return enumerator;

            for(var i = 0; i < __instance._id.Length; i++)
            {
                var id = __instance._id[i];
                var isCharacter = __instance._isUnitCharacter[i];
                var name = __instance._passiveName[i];
                var sprite = __instance._passiveSprite[i];

                if (stats.TryGetUnit(id, isCharacter, out var unit))
                    CombatManager.Instance.PostNotification(LocalCustomTriggers.OnPassivePopup, unit, new OnPassivePopupReference(name, sprite));
            }
        }
    }
}
