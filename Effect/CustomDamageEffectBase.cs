using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public abstract class CustomDamageEffectBase : EffectSO
    {
        public bool indirect;
        public bool usePreviousExitValue;
        public bool returnKillAsSuccess;
        public bool ignoreShield;
        public string deathType = DeathType_GameIDs.Basic.ToString();
        public string specialDamage = string.Empty;

        protected static T Create<T>(bool indirect = false, bool usePreviousExit = false, bool successOnKill = false, bool ignoreShield = false, string deathType = nameof(DeathType_GameIDs.Basic), string specialDamage = "") where T : CustomDamageEffectBase
        {
            var e = CreateScriptable<T>();
            e.indirect = indirect;
            e.usePreviousExitValue = usePreviousExit;
            e.returnKillAsSuccess = successOnKill;
            e.ignoreShield = ignoreShield;
            e.deathType = deathType;
            e.specialDamage = specialDamage;

            return e;
        }

        /*
        public static EffectSO Create(bool indirect = false, bool usePreviousExit = false, bool successOnKill = false, bool ignoreShield = false, string deathType = nameof(DeathType_GameIDs.Basic), string specialDamage = "")
        {
            var e = Create<CLASSNAME>(indirect, usePreviousExit, successOnKill, ignoreShield, deathType, specialDamage);

            return e;
        }
        */

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            ModifyEntryParameters(ref targets, ref areTargetSlots, ref entryVariable);

            var killed = false;
            var indirect = 0;
            var direct = 0;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;

                if (!ShouldDamage(u, t, stats, caster, areTargetSlots, entryVariable))
                    continue;

                var dmgInfo = DamageUnit(u, t, stats, caster, areTargetSlots, entryVariable, out var isIndirect);

                exitAmount += dmgInfo.damageAmount;
                killed |= dmgInfo.beenKilled;

                if (isIndirect)
                    indirect += dmgInfo.damageAmount;
                else
                    direct += dmgInfo.damageAmount;
            }

            if(direct > 0)
                caster.DidApplyDamage(direct);

            return IsSuccessful(stats, caster, targets, areTargetSlots, entryVariable, exitAmount, killed, direct, indirect);
        }

        public virtual void ModifyEntryParameters(ref TargetSlotInfo[] targets, ref bool areTargetSlots, ref int entryVariable)
        {
            if (usePreviousExitValue)
                entryVariable *= PreviousExitValue;
        }

        public virtual bool ShouldDamage(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable)
        {
            return true;
        }

        public virtual bool IsSuccessful(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, int exitAmount, bool killed, int directDamage, int indirectDamage)
        {
            if (returnKillAsSuccess)
                return killed;
            else
                return exitAmount > 0;
        }

        public virtual DamageInfo DamageUnit(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, out bool indirect)
        {
            indirect = IsIndirect(unit, target, stats, caster, areTargetSlots, entryVariable);

            var amt = BaseDamageAmount(unit, target, stats, caster, areTargetSlots, entryVariable, indirect);
            amt = ModifyDamageAmount(amt, unit, target, stats, caster, areTargetSlots, entryVariable, indirect);

            var offs = target.TargetOffset(areTargetSlots);
            var ignoresShield = IgnoreShields(unit, target, stats, caster, areTargetSlots, entryVariable, indirect);
            var generatePigment = GeneratePigment(unit, target, stats, caster, areTargetSlots, entryVariable, indirect);

            var killer = indirect ? null : caster;

            return unit.Damage(amt, killer, deathType, offs, generatePigment, !indirect, ignoresShield, specialDamage);
        }

        public virtual bool IsIndirect(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable)
        {
            return indirect;
        }

        public virtual int BaseDamageAmount(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            return entryVariable;
        }

        public virtual int ModifyDamageAmount(int amount, IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            return indirect ? amount : caster.WillApplyDamage(amount, unit);
        }

        public virtual bool IgnoreShields(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            return indirect || ignoreShield;
        }

        public virtual bool GeneratePigment(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            return !indirect;
        }
    }
}
