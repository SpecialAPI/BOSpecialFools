using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.StaticModifiers
{
    [HarmonyPatch]
    public class BleachStaticModifier : ItemModifierDataSetter
    {
        public static readonly string ID = "BleachStaticModifier".Prefix();

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.DefaultPassiveAbilityInitialization))]
        [HarmonyPrefix]
        public static bool PreventDefaultPassiveInitialization_Prefix(CharacterCombat __instance)
        {
            return __instance.CharacterWearableModifiers == null || !__instance.CharacterWearableModifiers.TryGetModdedDataSetter(ID, out var mod) || mod is not BleachStaticModifier;
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.TrySetUpNewItem))]
        [HarmonyILManipulator]
        public static void HandleFlagsChange_Transpiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            foreach (var m in crs.MatchAfter(x => x.MatchCallOrCallvirt<BaseWearableSO>(nameof(BaseWearableSO.OnTriggerDettached))))
            {
                crs.Emit(OpCodes.Ldarg_0);

                crs.EmitStaticDelegate(HandleFlagsChange_RecoverPassives);
            }

            crs.Index = 0;
            foreach (var m in crs.MatchAfter(x => x.MatchCallOrCallvirt<WearableStaticModifiers>(nameof(WearableStaticModifiers.ProcessModdedDataFromNewItem))))
            {
                crs.Emit(OpCodes.Ldarg_0);

                crs.EmitStaticDelegate(HandleFlagsChange_DisconnectPassives);
            }
        }

        public static void HandleFlagsChange_RecoverPassives(CharacterCombat cc)
        {
            if (cc.CharacterWearableModifiers == null || !cc.CharacterWearableModifiers.TryGetModdedDataSetter(ID, out var mod) || mod is not BleachStaticModifier)
                return;

            cc.CharacterWearableModifiers.RemoveModdedData(ID);
            cc.DefaultPassiveAbilityInitialization();
        }

        public static void HandleFlagsChange_DisconnectPassives(CharacterCombat cc)
        {
            if (cc.CharacterWearableModifiers == null || !cc.CharacterWearableModifiers.TryGetModdedDataSetter(ID, out var mod) || mod is not BleachStaticModifier)
                return;

            cc.RemoveAndDisconnectAllPassiveAbilities();
        }

        [HarmonyPatch(typeof(CharacterInGameData), nameof(CharacterInGameData.UpdateCurrentPassives))]
        [HarmonyPostfix]
        public static void RemoveOverworldDisplayedPassives_Postfix(CharacterInGameData __instance)
        {
            if (__instance.WearableModifiers == null)
                return;

            if(!__instance.WearableModifiers.TryGetModdedDataSetter(ID, out var mod) || mod is not BleachStaticModifier)
                return;

            __instance.CurrentPassives.Clear();
        }
    }
}
