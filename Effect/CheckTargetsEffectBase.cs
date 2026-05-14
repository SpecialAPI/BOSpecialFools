using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public abstract class CheckTargetsEffectBase : EffectSO
    {
        public bool allTargetsSuccessful;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var anySuccessful = false;
            var anyFail = false;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;

                if (CheckUnit(u, stats, caster, areTargetSlots, entryVariable))
                {
                    anySuccessful = true;
                    exitAmount++;
                }
                else
                    anyFail = true;
            }

            if (allTargetsSuccessful)
                return anySuccessful && !anyFail;
            else
                return anySuccessful;
        }

        public abstract bool CheckUnit(IUnit target, CombatStats stats, IUnit caster, bool areTagetSlots, int entryVariable);
    }
}
