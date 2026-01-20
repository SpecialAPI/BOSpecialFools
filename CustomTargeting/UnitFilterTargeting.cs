using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTargeting
{
    public class UnitFilterTargeting : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO orig;
        public Func<IUnit, bool> filter;

        public override bool AreTargetAllies => orig.AreTargetAllies;
        public override bool AreTargetSlots => orig.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return [.. orig.GetTargets(slots, casterSlotID, isCasterCharacter).Where(FilterTarget)];
        }

        private bool FilterTarget(TargetSlotInfo target)
        {
            if (filter == null)
                return false;

            if(!target.HasUnit)
                return false;

            return filter(target.Unit);
        }
    }
}
