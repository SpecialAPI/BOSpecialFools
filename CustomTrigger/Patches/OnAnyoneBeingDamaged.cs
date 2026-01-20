using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class OnAnyoneBeingDamaged
    {
        private static MethodInfo oabd_te = AccessTools.Method(typeof(OnAnyoneBeingDamaged), nameof(OnAnyoneBeingDamaged_TriggerEvent));

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Damage))]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Damage))]
        [HarmonyILManipulator]
        private static void OnAnyoneBeingDamaged_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<CombatManager>(nameof(CombatManager.PostNotification))))
                return;

            crs.Emit(OpCodes.Ldloc_2);
            crs.Emit(OpCodes.Call, oabd_te);
        }

        private static void OnAnyoneBeingDamaged_TriggerEvent(DamageReceivedValueChangeException ex)
        {
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.OnAnyoneBeingDamaged, ex));
        }
    }
}
