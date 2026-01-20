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
            {
                // workaround cuz pentacle doesnt convert IUnit args into IUnitHolder
                if(args is not IUnit u2)
                    return;

                u = u2;
            }

            u.SimpleSetStoredValue(sv, CS.CurrentPlayerTurn());
        }
    }
}
