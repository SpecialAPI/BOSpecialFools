using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    public class ImmediateEffectWithOutputAction(EffectInfo[] effects, IUnit caster, int startResult = 0) : IImmediateAction
    {
        public EffectInfo[] effects = effects;
        public IUnit caster = caster;
        public int startResult = startResult;

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
                    successful = false;
                    continue;
                }

                var targets = (e.targets != null) ? e.targets.GetTargets(stats.combatSlots, caster.SlotID, caster.IsUnitCharacter) : [];
                var targetSlots = (e.targets == null) || e.targets.AreTargetSlots;
                exitValue = e.StartEffect(stats, caster, targets, targetSlots, exitValue);
                successful = e.EffectSuccess;
            }
        }
    }
}
