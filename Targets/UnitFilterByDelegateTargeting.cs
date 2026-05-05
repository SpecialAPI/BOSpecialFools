using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public class UnitFilterByDelegateTargeting : UnitFilterTargetingBase
    {
        public Func<IUnit, SlotsCombat, int, bool, bool> filter;

        protected override bool FilterUnit(IUnit unit, SlotsCombat slots, int casterSlotID, bool isCasterCharacter) => filter(unit, slots, casterSlotID, isCasterCharacter);
    }
}
