using BOSpecialFools.CustomTrigger.Args;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class ModifyCanProducePigmentFromDamage_Anyone
    {
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Damage))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Damage))]
        [HarmonyILManipulator]
        private static void ModifyCanProducePigmentFromDamage_Anyone_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpBeforeNext(x => x.MatchBrfalse(out _), 4))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.Emit(OpCodes.Ldarg_2);
            crs.Emit(OpCodes.Ldloc, 5);
            crs.Emit(OpCodes.Ldarg, 8);
            crs.Emit(OpCodes.Ldarg, 6);
            crs.Emit(OpCodes.Ldarg, 7);
            crs.Emit(OpCodes.Ldloc_0);
            crs.Emit(OpCodes.Ldloc_1);
            crs.EmitStaticDelegate(ModifyCanProducePigmentFromDamage_Anyone_Modify);
        }

        private static bool ModifyCanProducePigmentFromDamage_Anyone_Modify(bool curr, IUnit damagedUnit, IUnit possibleSourceUnit, int amount, string damageTypeID, bool directDamage, bool ignoreShield, int affectedStartSlot, int affectedEndSlot)
        {
            var modifyCanProduceRef = new ModifyCanProducePigmentFromDamageReference(curr, damagedUnit, possibleSourceUnit, amount, damageTypeID, directDamage, ignoreShield, affectedStartSlot, affectedEndSlot);
            CombatManager.Instance.ProcessImmediateAction(new TriggerUnitGeneralEventAction(LocalCustomTriggers.ModifyCanProducePigmentFromDamage_Anyone, modifyCanProduceRef, false, false, null));
            
            return modifyCanProduceRef.canProducePigmentFromDamage;
        }
    }
}
