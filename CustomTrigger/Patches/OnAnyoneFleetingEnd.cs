using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class OnAnyoneFleetingEnd
    {
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.FinalizeFleeting))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.FinalizeFleeting))]
        [HarmonyPostfix]
        private static void OnAnyoneFleetingEnd_Postfix(IUnit __instance)
        {
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.OnAnyoneFleetingEnd, __instance));
        }
    }
}
