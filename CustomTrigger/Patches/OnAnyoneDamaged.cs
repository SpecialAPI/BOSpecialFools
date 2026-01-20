using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class OnAnyoneDamaged
    {
        private static readonly MethodInfo oad_te = AccessTools.Method(typeof(OnAnyoneDamaged), nameof(OnAnyoneDamaged_TriggerEvent));

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Damage))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Damage))]
        [HarmonyILManipulator]
        private static void OnAnyoneDamaged_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<CombatManager>(nameof(CombatManager.PostNotification)), 2))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.Emit(OpCodes.Ldarg, 6);
            crs.Emit(OpCodes.Call, oad_te);
        }

        private static void OnAnyoneDamaged_TriggerEvent(IUnit u, bool direct)
        {
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.OnAnyoneDamaged, u));

            if(direct)
                CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.OnAnyoneDirectDamaged, u));
        }
    }
}
