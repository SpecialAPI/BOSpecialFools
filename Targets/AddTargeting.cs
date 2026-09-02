using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public class AddTargeting : BaseCombatTargettingSO
    {
        public List<BaseCombatTargettingSO> targeting = [];
        public bool areTargetAllies;
        public bool areTargetSlots;

        public override bool AreTargetAllies => areTargetAllies;

        public override bool AreTargetSlots => areTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var targets = new List<TargetSlotInfo>();

            foreach (var t in targeting)
                targets.AddRange(t.GetTargets(slots, casterSlotID, isCasterCharacter));

            return [..targets];
        }
    }
}
