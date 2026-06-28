using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class OutputTargetLastDamageEffect : OutputTargetValueEffectBase
    {
        public override int GetValue(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable)
        {
            return unit.LastDamage();
        }
    }
}
