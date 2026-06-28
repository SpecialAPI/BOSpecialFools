using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public abstract class OutputTargetValueEffectBase : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;
                exitAmount += GetValue(u, t, stats, caster, areTargetSlots, entryVariable);
            }

            return exitAmount > 0;
        }

        public abstract int GetValue(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable);
    }
}
