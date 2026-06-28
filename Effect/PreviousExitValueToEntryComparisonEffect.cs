using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class PreviousExitValueToEntryComparisonEffect : EffectSO
    {
        public IntComparison comparison;
        public bool outputEntry;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = outputEntry ? entryVariable : PreviousExitValue;

            return CompareInts(PreviousExitValue, entryVariable, comparison);
        }
    }
}
