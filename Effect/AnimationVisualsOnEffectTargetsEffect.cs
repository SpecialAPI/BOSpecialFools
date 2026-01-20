using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class AnimationVisualsOnEffectTargetsEffect : EffectSO
    {
        public AttackVisualsSO visuals;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            CombatManager.Instance.AddUIAction(new PlayAbilityAnimationAction(visuals, targets, areTargetSlots));

            return true;
        }
    }

    public class PlayAbilityAnimationAction(AttackVisualsSO visuals, TargetSlotInfo[] targets, bool areTargetSlots) : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.PlayAbilityAnimation(visuals, targets, areTargetSlots);
        }
    }
}
