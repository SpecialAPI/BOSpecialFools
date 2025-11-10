using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class SpecialDamageEffect : EffectSO
    {
        public SpecialDamageInfo damageInfo;

        public bool indirect;

        public bool dryDamage;
        public bool wetDamage;

        public bool ignoreShield;
        public bool shieldHittingIndirect;

        public string deathType = DeathType_GameIDs.Basic.ToString();
        public string specialDamageType = "";

        public bool usePreviousExitValue;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (usePreviousExitValue)
                entryVariable *= PreviousExitValue;

            foreach (var t in targets)
            {
                if (t == null || !t.HasUnit)
                    continue;

                var targetOffset = areTargetSlots ? t.TargetOffset() : -1;

                if (indirect)
                    exitAmount += t.Unit.SpecialDamage(entryVariable, null, damageInfo, deathType, targetOffset, wetDamage, false, !shieldHittingIndirect, specialDamageType).damageAmount;

                else
                {
                    var modValue = (damageInfo == null || !damageInfo.DisableOnBeingDamagedCalls) ? caster.WillApplyDamage(entryVariable, t.Unit) : entryVariable;

                    if (damageInfo != null && damageInfo.ExtraDamageModifierPercentage != 0)
                        modValue = (int)Mathf.LerpUnclamped(entryVariable, modValue, 1f + (damageInfo.ExtraDamageModifierPercentage / 100f));

                    exitAmount += t.Unit.SpecialDamage(modValue, caster, damageInfo, deathType, targetOffset, !dryDamage, true, ignoreShield, specialDamageType).damageAmount;
                }
            }

            if (exitAmount > 0 && !indirect)
                caster.DidApplyDamage(exitAmount);

            return exitAmount > 0;
        }
    }
}
