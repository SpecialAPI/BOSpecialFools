using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class DamageByEntryOrPrevExitIfFullHealth : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var amt = entryVariable;
                if (t.Unit.CurrentHealth >= t.Unit.MaximumHealth)
                    amt = PreviousExitValue;
                amt = caster.WillApplyDamage(amt, t.Unit);

                exitAmount += t.Unit.Damage(amt, caster, DeathType_GameIDs.Basic.ToString(), t.TargetOffset(areTargetSlots)).damageAmount;
            }

            if(exitAmount > 0)
                caster.DidApplyDamage(exitAmount);

            return exitAmount > 0;
        }
    }
}
