using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class BranchEffect : EffectSO
    {
        public List<EffectInfo> conditionEffects = [];
        public List<EffectInfo> successEffects = [];
        public List<EffectInfo> failEffects = [];

        public bool usePreviousExitForCondition;
        public bool usePreviousExitForBranches;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if(conditionEffects == null)
                return false;

            var conditionAction = new ImmediateEffectWithOutputAction([..conditionEffects], caster, usePreviousExitForCondition ? PreviousExitValue : 0);
            CombatManager.Instance.ProcessImmediateAction(conditionAction);

            var condSuccess = conditionAction.successful;
            var effectsToPerform = condSuccess ? successEffects : failEffects;

            if(effectsToPerform == null)
                return false;

            var branchAction = new ImmediateEffectWithOutputAction([.. effectsToPerform], caster, usePreviousExitForBranches ? PreviousExitValue : 0);
            CombatManager.Instance.ProcessImmediateAction(branchAction);

            exitAmount = branchAction.exitValue;
            return branchAction.successful;
        }
    }
}
