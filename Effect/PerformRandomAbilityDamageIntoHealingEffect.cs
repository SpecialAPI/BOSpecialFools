using BOSpecialFools.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class PerformRandomAbilityDamageIntoHealingEffect : EffectSO
    {
        public BaseCombatTargettingSO healTargeting;
        public int healingToDamagePercentageModifier;
        public bool directHeal = true;
        public List<string> ignoredAbilityIDs = [];

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if(t == null || !t.HasUnit)
                    continue;

                var u = t.Unit;
                var abilities = new List<CombatAbility>(t.Unit.Abilities());
                if (ignoredAbilityIDs != null && ignoredAbilityIDs.Count > 0)
                {
                    for (var i = 0; i < abilities.Count; i++)
                    {
                        var ability = abilities[i];
                        if (ability == null || ability.ability == null)
                            continue;

                        if (!ignoredAbilityIDs.Contains(ability.ability.name))
                            continue;

                        abilities.RemoveAt(i);
                        i--;
                    }
                }

                if(abilities.Count <= 0)
                    continue;

                CombatManager.Instance.AddSubAction(new AddObserverAction(ApplyDamageModifier, TriggerCalls.OnWillApplyDamage.ToString(), u));
                for (var i = 0; i < entryVariable; i++)
                {
                    if (u.TryPerformRandomAbility(abilities.RandomElement().ability))
                        exitAmount++;
                }
                CombatManager.Instance.AddSubAction(new RemoveObserverAction(ApplyDamageModifier, TriggerCalls.OnWillApplyDamage.ToString(), u));
            }

            return exitAmount > 0;
        }

        public void ApplyDamageModifier(object sender, object args)
        {
            if(args is not DamageDealtValueChangeException ex)
                return;

            ex.AddModifier(new TurnDamageIntoHealingIntValueModifier(ex.damagedUnit, ex.casterUnit, healTargeting, healingToDamagePercentageModifier, directHeal));
        }
    }
}
