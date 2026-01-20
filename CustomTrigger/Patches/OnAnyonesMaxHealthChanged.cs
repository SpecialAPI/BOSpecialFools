using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class OnAnyonesMaxHealthChanged
    {
        private static readonly MethodInfo oamhc_te = AccessTools.Method(typeof(OnAnyonesMaxHealthChanged), nameof(OnAnyonesMaxHealthChanged_TriggerEvent));

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.MaximizeHealth))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.MaximizeHealth))]
        [HarmonyILManipulator]
        private static void OnAnyonesMaxHealthChanged_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<CombatManager>(nameof(CombatManager.PostNotification))))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.Emit(OpCodes.Call, oamhc_te);
        }

        private static void OnAnyonesMaxHealthChanged_TriggerEvent(IUnit u)
        {
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.OnAnyonesMaxHealthChanged, u));
        }
    }
}
