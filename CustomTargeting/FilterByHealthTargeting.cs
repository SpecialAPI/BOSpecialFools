using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTargeting
{
    public class FilterByHealthTargeting : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO orig;
        public bool getWeakest;
        public bool ignoreDead;

        public override bool AreTargetAllies => orig.AreTargetAllies;
        public override bool AreTargetSlots => orig.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var origTargets = orig.GetTargets(slots, casterSlotID, isCasterCharacter);
            var targets = new List<TargetSlotInfo>();
            var h = -1;
            var comp = getWeakest ? IntComparison.LessThan : IntComparison.GreaterThan;

            foreach(var t in origTargets)
            {
                if (!t.HasUnit)
                    continue;

                var u = t.Unit;

                if(ignoreDead && (u.CurrentHealth <= 0 || !u.IsAlive))
                    continue;

                if(targets.Count == 0)
                    h = u.CurrentHealth;

                if (CompareInts(u.CurrentHealth, h, comp))
                {
                    targets.Clear();
                    h = u.CurrentHealth;
                }
                if(u.CurrentHealth == h)
                    targets.Add(t);
            }

            return [.. targets];
        }
    }
}
