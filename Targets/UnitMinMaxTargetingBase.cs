using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public abstract class UnitMinMaxTargetingBase : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO orig;
        public bool isMax;

        public override bool AreTargetAllies => orig.AreTargetAllies;
        public override bool AreTargetSlots => orig.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var origTargets = orig.GetTargets(slots, casterSlotID, isCasterCharacter);
            var newTargets = new List<TargetSlotInfo>();

            int? currentValue = null;
            var comparison = isMax ? IntComparison.GreaterThan : IntComparison.LessThan;

            foreach (var t in origTargets)
            {
                if (!t.HasUnit)
                    continue;

                var value = GetUnitValue(t.Unit, t, slots, casterSlotID, isCasterCharacter);

                if(currentValue is int currVal && CompareInts(value, currVal, comparison))
                {
                    newTargets.Clear();
                    newTargets.Add(t);

                    currentValue = value;
                }
                else if(!currentValue.HasValue || currentValue.Value == value)
                {
                    newTargets.Add(t);

                    currentValue = value;
                }
            }

            return [.. newTargets];
        }

        protected abstract int GetUnitValue(IUnit unit, TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter);
    }
}
