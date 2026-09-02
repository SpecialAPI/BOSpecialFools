using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public abstract class TargetFilterTargetingBase : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO orig;

        public override bool AreTargetAllies => orig.AreTargetAllies;
        public override bool AreTargetSlots => orig.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var origTargets = orig.GetTargets(slots, casterSlotID, isCasterCharacter);
            var newTargets = new List<TargetSlotInfo>();

            foreach (var t in origTargets)
            {
                if (FilterTarget(t, slots, casterSlotID, isCasterCharacter))
                    newTargets.Add(t);
            }

            return [.. newTargets];
        }

        protected abstract bool FilterTarget(TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter);
    }
}
