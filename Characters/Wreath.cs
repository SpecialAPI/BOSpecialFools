using BOSpecialFools.CustomTrigger;
using BOSpecialFools.TriggerEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Characters
{
    public static class Wreath
    {
        public static void Init()
        {
            var ch = NewCharacter("Wreath_CH", "Wreath")
                .SetBasicInformation("Wreath", Pigments.Purple, "WreathFront", "WreathBack", "WreathOW");

            var trackSv = NewStoredValue<UnitStoreData_BasicSO>("WreathLastDamageTurn_USD", "WreathLastDamageTurn");
            var tracker = CreateScriptable<MultiCustomTriggerEffectHiddenPassiveEffect>();
            tracker.triggerEffects = new()
            {
                new()
                {
                    trigger = LocalCustomTriggers.OnAnyoneDamaged,
                    immediate = true,
                    
                    effect = new SetUnitHolderStoredValueToCurrentTurnTriggerEffect(trackSv._UnitStoreDataID, 1)
                }
            };
            ch.AddHiddenEffects(tracker);

            ch.RankedDataSetup(4, (rank, abilityRank) =>
            {
                var health = RankedValue(14, 16, 17, 18);

                var abATargeting = Targeting.Unit_AllOpponents.FilterUnit(x => x.SimpleGetStoredValue(trackSv._UnitStoreDataID) == CS.CurrentPlayerTurn());
                var abilityADamage = RankedValue(6, 8, 10, 12);
                var abilityA = NewAbility($"WreathA_{abilityRank}_A")
                .SetBasicInformation($"Ability A {abilityRank}", $"Deal {abilityADamage} damage to All enemies that received any damage this turn.")
                .SetVisuals(Visuals.Parry, abATargeting)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), abilityADamage, abATargeting)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Unit_AllOpponents, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(abATargeting, IntentForDamage(abilityADamage))
                })
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Yellow, Pigments.Blue, Pigments.Red);

                var abilityBDamageFullHealth = RankedValue(5, 6, 7, 8);
                var abilityBDamageNotFull = RankedValue(8, 11, 14, 16);
                var abilityB = NewAbility($"WreathB_{abilityRank}_A")
                .SetBasicInformation($"Ability B {abilityRank}", $"If the Opposing enemy is at full health, deal {abilityBDamageFullHealth} to them.\nOtherwise, deal {abilityBDamageNotFull} damage to the Opposing enemy and reduce their maximum health to their current health value.")
                .SetVisuals(Visuals.Burn, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<CheckTargetsAtMaxHealthEffect>(), 0, Targeting.Slot_Front),

                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), abilityBDamageFullHealth, Targeting.Slot_Front, Effects.CheckPreviousEffectCondition(true, 1)),

                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), abilityBDamageNotFull, Targeting.Slot_Front, Effects.CheckPreviousEffectCondition(false, 2)),
                    Effects.GenerateEffect(CreateScriptable<ChangeMaxHealthByCurrentHealthEffect>(), 0, Targeting.Slot_Front, Effects.CheckPreviousEffectCondition(false, 3)),
                })
                .AddIntent(Targeting.Slot_Front, IntentType_GameIDs.Misc_Hidden.ToString(), IntentForDamage(abilityBDamageFullHealth), IntentForDamage(abilityBDamageNotFull), IntentType_GameIDs.Other_MaxHealth.ToString())
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Red, Pigments.Yellow);

                var abilityCDamage = RankedValue(6, 7, 9, 10);
                var abCTargeting = Targeting.Unit_AllOpponents.FilterByHealth(true, true);
                var abCPerformEffect = CreateScriptable<CasterPerformEffectsEffect>();
                abCPerformEffect.effects = new()
                {
                    Effects.GenerateEffect(CreateScriptable<PerformEffectsOnRandomTargetEffect>(x => x.effects = new()
                    {
                        Effects.GenerateEffect(CreateScriptable<AnimationVisualsOnEffectTargetsEffect>(x2 => x2.visuals = Visuals.Decimate)),
                        Effects.GenerateEffect(CreateScriptable<DamageEffect>(x => x._returnKillAsSuccess = true), abilityCDamage)
                    }), 0, abCTargeting),
                    Effects.GenerateEffect(abCPerformEffect, condition: Effects.CheckPreviousEffectCondition(true, 1))
                };
                var abilityC = NewAbility($"WreathC_{abilityRank}_A")
                .SetBasicInformation($"Ability C {abilityRank}", $"Deal {abilityCDamage} damage to a Random enemy with the lowest health. If this kills, repeat this ability.")
                .SetEffects([Effects.GenerateEffect(abCPerformEffect)])
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Unit_AllOpponents, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(Targeting.Spec_Unit_AllOpponents_Weakest, IntentForDamage(abilityCDamage)),
                    TargetIntent(Targeting.Slot_SelfSlot, IntentType_GameIDs.Misc_Additional.ToString())
                })
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Red, Pigments.Red);

                return new(health, [abilityA, abilityB, abilityC]);
            });

            ch.AddToDatabase();
        }
    }
}
