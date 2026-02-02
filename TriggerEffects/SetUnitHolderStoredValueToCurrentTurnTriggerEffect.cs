using BOSpecialFools.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.TriggerEffects
{
    public class SetUnitHolderStoredValueToCurrentTurnTriggerEffect(string sv, int unitIndex = 0) : TriggerEffect
    {
        public override void DoEffect(IUnit sender, object args, TriggerEffectInfo triggerInfo, TriggerEffectActivationExtraInfo extraInfo)
        {
            if (!ValueReferenceTools.TryGetUnitHolder(args, out var hold) || hold[unitIndex] is not IUnit u)
                return;

            u.SimpleSetStoredValue(sv, CS.CurrentPlayerTurn());
        }
    }
}
