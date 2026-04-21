using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class DamageByMissingHealthUpToEntryEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
                entryVariable *= PreviousExitValue;

            exitAmount = 0;
            var killed = false;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;
                var offs = t.TargetOffset(areTargetSlots);
                var amt = Mathf.Clamp(u.MaximumHealth - u.CurrentHealth, 0, entryVariable);

                DamageInfo dmgInfo;

                if (!_indirect)
                {
                    amt = caster.WillApplyDamage(amt, u);
                    dmgInfo = u.Damage(amt, caster, _DeathTypeID, offs, true, true, _ignoreShield);
                }
                else
                    dmgInfo = u.Damage(amt, null, _DeathTypeID, offs, false, false, true);

                exitAmount += dmgInfo.damageAmount;
                killed |= dmgInfo.beenKilled;
            }

            if (!_indirect && exitAmount > 0)
                caster.DidApplyDamage(exitAmount);

            if (_returnKillAsSuccess)
                return killed;
            else
                return exitAmount > 0;
        }
    }
}
