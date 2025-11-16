using BOSpecialFools.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class PerformRandomCharacterAbilitiesWithDealtDamageAndHealingMultiplierEffect : EffectSO
    {
        public int healMultiplier = 1;
        public int damageMultiplier = 1;
        public int abilitiesRank = -1;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if(t == null || !t.HasUnit)
                    continue;

                var u = t.Unit;
                var abilities = abilitiesRank < 0 ?
                    LoadedDBsHandler.AbilityDB.GetRandomCharacterAbilities(entryVariable) :
                    RankedAbilitiesDatabase.GetRandomAbilitiesOfRank(abilitiesRank, entryVariable);

                var hasHealingMod = healMultiplier != 1;
                var hasDamageMod = damageMultiplier != 1;

                if (hasHealingMod)
                    CombatManager.Instance.AddSubAction(new AddObserverAction(ApplyHealModifier, TriggerCalls.OnWillApplyHeal.ToString(), u));
                if(hasDamageMod)
                    CombatManager.Instance.AddSubAction(new AddObserverAction(ApplyDamageModifier, TriggerCalls.OnWillApplyDamage.ToString(), u));

                foreach (var ab in abilities)
                {
                    if (u.TryPerformRandomAbility(ab))
                        exitAmount++;
                }

                if (hasHealingMod)
                    CombatManager.Instance.AddSubAction(new RemoveObserverAction(ApplyHealModifier, TriggerCalls.OnWillApplyHeal.ToString(), u));
                if (hasDamageMod)
                    CombatManager.Instance.AddSubAction(new RemoveObserverAction(ApplyDamageModifier, TriggerCalls.OnWillApplyDamage.ToString(), u));
            }

            return exitAmount > 0;
        }

        public void ApplyHealModifier(object sender, object args)
        {
            if (!ValueReferenceTools.TryGetValueChangeException(args, out var ex))
                return;

            ex.AddModifier(new MultiplyIntValueModifier(ex.DamageDealt, healMultiplier));
        }

        public void ApplyDamageModifier(object sender, object args)
        {
            if(!ValueReferenceTools.TryGetValueChangeException(args, out var ex))
                return;

            ex.AddModifier(new MultiplyIntValueModifier(ex.DamageDealt, damageMultiplier));
        }
    }
}
