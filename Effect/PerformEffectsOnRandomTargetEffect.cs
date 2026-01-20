using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class PerformEffectsOnRandomTargetEffect : EffectSO
    {
        public List<EffectInfo> effects = [];
        public bool entryIsNumberOfTargets;
        public bool canTargetsRepeat;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            var numTargets = entryIsNumberOfTargets ? entryVariable : 1;
            var remaining = targets.ToList();
            var picked = new List<TargetSlotInfo>();
            while (remaining.Count > 0 && picked.Count < numTargets)
            {
                var idx = Random.Range(0, remaining.Count);
                picked.Add(remaining[idx]);

                if(!canTargetsRepeat)
                    remaining.RemoveAt(idx);
            }

            var act = new ImmediateEffectOnSpecificTargetsAction([.. effects], caster, picked, areTargetSlots);
            CombatManager.Instance.ProcessImmediateAction(act);
            exitAmount = act.exitValue;

            return act.successful;
        }
    }
}
