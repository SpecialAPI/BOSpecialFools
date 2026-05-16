using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public class UnitMinMaxByPositionTargeting : UnitMinMaxTargetingBase
    {
        protected override int GetUnitValue(IUnit unit, TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return unit.SlotID + unit.LastSlotId();
        }
    }
}
