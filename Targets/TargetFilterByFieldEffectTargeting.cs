using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public class TargetFilterByFieldEffectTargeting : TargetFilterTargetingBase
    {
        public FieldEffect_SO field;
        public bool needsToHaveField;

        protected override bool FilterTarget(TargetSlotInfo target, SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var hasField = target.GetSlot().ContainsFieldEffect(field.FieldID);

            return hasField == needsToHaveField;
        }
    }
}
