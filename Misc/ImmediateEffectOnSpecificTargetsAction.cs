using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    public class ImmediateEffectOnSpecificTargetsAction(EffectInfo[] effects, IUnit caster, List<TargetSlotInfo> targets, bool targetSlots, int startResult = 0) : IImmediateAction
    {
        public int exitValue = startResult;
        public bool successful = false;

        public void Execute(CombatStats stats)
        {
            for (var i = 0; i < effects.Length; i++)
            {
                var e = effects[i];
                var condition = e.condition;
                if (condition != null && !condition.MeetCondition(caster, effects, i))
                {
                    exitValue = e.FailEffect(exitValue);
                    continue;
                }

                exitValue = e.StartEffect(stats, caster, [..targets], targetSlots, exitValue);
            }

            successful = effects[effects.Length - 1].EffectSuccess;
        }
    }
}
