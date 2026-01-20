using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class CasterPerformEffectsEffect : EffectSO
    {
        public List<EffectInfo> effects = [];

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            var act = new ImmediateEffectWithOutputAction([.. effects], caster);
            CombatManager.Instance.ProcessImmediateAction(act);
            exitAmount = act.exitValue;

            return exitAmount > 0;
        }
    }
}
