using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements.StyleSheets.Syntax;
using static UnityEngine.UIElements.StyleSheets.Dimension;

namespace BOSpecialFools.Tools
{
    [HarmonyPatch]
    public static class UnitHistoryTools
    {
        private static readonly UnitStoreData_BasicSO StoreData = NewStoredValue<UnitStoreData_BasicSO>("UnitHistoryStorage_USD", "UnitHistoryStorage", Profile);

        public static int LastDamagedTurn(this IUnit u)
        {
            var history = u.DamageHistory();

            if (history == null || history.Count == 0)
                return -1;

            return history[history.Count - 1].turn;
        }

        public static bool DamagedThisTurn(this IUnit u)
        {
            var lastDamagedTurn = u.LastDamagedTurn();
            if(lastDamagedTurn < 0)
                return false;

            return lastDamagedTurn == CS.CurrentPlayerTurn();
        }

        public static IReadOnlyList<DamageInstance> DamageHistory(this IUnit u) => u.GetHistoryStorage()?.damageHistory;

        public static IReadOnlyList<HealingInstance> HealingHistory(this IUnit u) => u.GetHistoryStorage()?.healingHistory;

        private static UnitHistoryStorage GetHistoryStorage(this IUnit u)
        {
            if (u == null)
                return null;

            u.TryGetStoredData(StoreData._UnitStoreDataID, out var holder);
            if (holder.m_ObjectData is not UnitHistoryStorage storage)
                holder.m_ObjectData = storage = new();

            return storage;
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Damage))]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Damage))]
        [HarmonyILManipulator]
        private static void RegisterDamageInstance_Transpiler(ILContext ctx, MethodBase mthd)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt(mthd.DeclaringType, "set_CurrentHealth")))
                return;

                                            // curr: void
            crs.Emit(OpCodes.Ldarg_0);      // unit
            crs.Emit(OpCodes.Ldloc, 5);     // damageAmount
            crs.Emit(OpCodes.Ldloc_3);      // attemptedDamageAmount
            crs.Emit(OpCodes.Ldarg_1);      // incomingDamage
            crs.Emit(OpCodes.Ldarg_2);      // source
            crs.Emit(OpCodes.Ldarg_3);      // deathType
            crs.Emit(OpCodes.Ldloc_0);      // affectedStartSlot
            crs.Emit(OpCodes.Ldloc_1);      // affectedEndSlot
            crs.Emit(OpCodes.Ldarg, 5);     // producePigment
            crs.Emit(OpCodes.Ldarg, 6);     // direct
            crs.Emit(OpCodes.Ldarg, 7);     // ignoreShield
            crs.Emit(OpCodes.Ldarg, 8);     // specialDamage
            crs.EmitStaticDelegate(RegisterDamageInstance_Register); // push: void
        }

        private static void RegisterDamageInstance_Register(IUnit unit, int damageAmount, int attemptedDamageAmount, int incomingDamage, IUnit source, string deathType, int affectedStartSlot, int affectedEndSlot, bool producePigment, bool direct, bool ignoreShield, string specialDamage)
        {
            var turn = CS.CurrentPlayerTurn();

            unit.GetHistoryStorage().damageHistory.Add(new(damageAmount, attemptedDamageAmount, incomingDamage, source, deathType, affectedStartSlot, affectedEndSlot, producePigment, direct, ignoreShield, specialDamage, turn));
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.ManaDamage))]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.ManaDamage))]
        [HarmonyILManipulator]
        private static void RegisterDamageInstance_ManaDamage_Transpiler(ILContext ctx, MethodBase mthd)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt(mthd.DeclaringType, "set_CurrentHealth")))
                return;

                                            // curr: void
            crs.Emit(OpCodes.Ldarg_0);      // unit
            crs.Emit(OpCodes.Ldloc, 6);     // damageAmount
            crs.Emit(OpCodes.Ldloc, 4);     // attemptedDamageAmount
            crs.Emit(OpCodes.Ldarg_1);      // incomingDamage
            crs.Emit(OpCodes.Ldloc_3);      // ex
            crs.Emit(OpCodes.Ldarg_3);      // deathType
            crs.Emit(OpCodes.Ldloc_0);      // affectedStartSlot
            crs.Emit(OpCodes.Ldloc_1);      // affectedEndSlot
            crs.Emit(OpCodes.Ldloc_2);      // specialDamage
            crs.EmitStaticDelegate(RegisterDamageInstance_ManaDamage_Register); // push: void
        }

        private static void RegisterDamageInstance_ManaDamage_Register(IUnit unit, int damageAmount, int attemptedDamageAmount, int incomingDamage, DamageReceivedValueChangeException ex, string deathType, int affectedStartSlot, int affectedEndSlot, string specialDamage)
        {
            var source = ex.possibleSourceUnit;
            var producePigment = false;
            var direct = ex.directDamage;
            var ignoreShield = ex.ignoreShield;
            var turn = CS.CurrentPlayerTurn();

            unit.GetHistoryStorage().damageHistory.Add(new(damageAmount, attemptedDamageAmount, incomingDamage, source, deathType, affectedStartSlot, affectedEndSlot, producePigment, direct, ignoreShield, specialDamage, turn));
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Heal))]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Heal))]
        [HarmonyILManipulator]
        private static void RegisterHealingInstance_Transpiler(ILContext ctx, MethodBase mthd)
        {
            var crs = new ILCursor(ctx);
            
            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt(mthd.DeclaringType, "set_CurrentHealth")))
                return;
            
                                            // curr: void
            crs.Emit(OpCodes.Ldarg_0);      // unit
            crs.Emit(OpCodes.Ldloc, 4);     // healAmount
            crs.Emit(OpCodes.Ldloc_1);      // attemptedHealAmount
            crs.Emit(OpCodes.Ldarg_1);      // incomingAmount
            crs.Emit(OpCodes.Ldarg_2);      // source
            crs.Emit(OpCodes.Ldarg_3);      // direct
            crs.Emit(OpCodes.Ldarg, 4);     // healType
            crs.EmitStaticDelegate(RegisterHealingInstance_Register); // push: void
        }

        private static void RegisterHealingInstance_Register(IUnit unit, int healAmount, int attemptedHealAmount, int incomingAmount, IUnit source, bool direct, string healType)
        {
            var turn = CS.CurrentPlayerTurn();

            unit.GetHistoryStorage().healingHistory.Add(new(healAmount, attemptedHealAmount, incomingAmount, source, direct, healType, turn));
        }

        private class UnitHistoryStorage
        {
            public readonly List<DamageInstance> damageHistory = [];
            public readonly List<HealingInstance> healingHistory = [];
        }
    }

    public class DamageInstance(int damageAmount, int attemptedDamageAmount, int incomingDamage, IUnit source, string deathType, int affectedStartSlot, int affectedEndSlot, bool producePigment, bool direct, bool ignoreShield, string specialDamage, int turn)
    {
        public readonly int damageAmount            = damageAmount;
        public readonly int attemptedDamageAmount   = attemptedDamageAmount;
        public readonly int incomingDamage          = incomingDamage;

        public readonly IUnit source            = source;
        public readonly string deathType        = deathType;
        public readonly int affectedStartSlot   = affectedStartSlot;
        public readonly int affectedEndSlot     = affectedEndSlot;
        public readonly bool producePigment     = producePigment;
        public readonly bool direct             = direct;
        public readonly bool ignoreShield       = ignoreShield;
        public readonly string specialDamage    = specialDamage;

        public readonly int turn    = turn;
    }

    public class HealingInstance(int healAmount, int attemptedHealAmount, int incomingAmount, IUnit source, bool direct, string healType, int turn)
    {
        public readonly int healAmount              = healAmount;
        public readonly int attemptedHealAmount     = attemptedHealAmount;
        public readonly int incomingAmount          = incomingAmount;

        public readonly IUnit source        = source;
        public readonly bool direct         = direct;
        public readonly string healType     = healType;

        public readonly int turn    = turn;
    }
}
