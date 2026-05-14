using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class CheckTargetsHealthCompareToEntryEffect : CheckTargetsEffectBase
    {
        public IntComparison comparison = IntComparison.LessThanOrEqual;

        public override bool CheckUnit(IUnit target, CombatStats stats, IUnit caster, bool areTagetSlots, int entryVariable)
        {
            return CompareInts(target.CurrentHealth, entryVariable, comparison);
        }
    }
}
