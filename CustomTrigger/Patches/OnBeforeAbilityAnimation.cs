using BOSpecialFools.CustomTrigger.Args;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Patches
{
    [HarmonyPatch]
    internal static class OnBeforeAbilityAnimation
    {
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.UseAbility))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.UseAbility))]
        [HarmonyILManipulator]
        private static void OnBeforeAbilityAnimation_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpBeforeNext(x => x.MatchLdfld<CombatAbility>(nameof(CombatAbility.ability))))
                return;

            var cAbilityLoc = crs.DeclareLocal<CombatAbility>();
            crs.Emit(OpCodes.Ldloca, cAbilityLoc);
            crs.EmitStaticDelegate(OnBeforeAbilityAnimation_SaveCombatAbility);

            if (!crs.JumpToNext(x => x.MatchNewobj<PlayAbilityAnimationAction>()))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.Emit(OpCodes.Ldarg_1);
            crs.Emit(OpCodes.Ldloc, cAbilityLoc);
            crs.Emit(OpCodes.Ldloc_0);
            crs.EmitStaticDelegate(OnBeforeAbilityAnimation_TriggerEvent);
        }

        private static CombatAbility OnBeforeAbilityAnimation_SaveCombatAbility(CombatAbility curr, out CombatAbility save)
        {
            save = curr;
            return curr;
        }

        private static PlayAbilityAnimationAction OnBeforeAbilityAnimation_TriggerEvent(PlayAbilityAnimationAction curr, IUnit unit, int abilityIdx, CombatAbility cAbility, AbilitySO ability)
        {
            CombatManager.Instance.AddRootAction(new PerformDelegateAction(x =>
            {
                CombatManager.Instance.PostNotification(LocalCustomTriggers.OnBeforeAbilityAnimation, unit, new OnBeforeAbilityUsedContext(abilityIdx, cAbility, ability, null));
            }));

            return curr;
        }
    }
}
