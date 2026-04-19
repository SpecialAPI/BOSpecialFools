using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class CheckTargetsAtMaxHealthEffect : CheckTargetsEffectBase
    {
        public override bool CheckUnit(IUnit target, CombatStats stats, IUnit caster, bool areTagetSlots, int entryVariable, ref int exitAmount)
        {
            if(target.CurrentHealth < target.MaximumHealth)
                return false;

            exitAmount += 1;
            return true;
        }
    }
}
