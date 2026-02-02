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
                var health = RankedValue(14, 16, 18, 20);

                var ab1Targeting = Targeting.Unit_AllOpponents.FilterUnit(x => x.SimpleGetStoredValue(trackSv._UnitStoreDataID) == CS.CurrentPlayerTurn());
                var ability1Damage = RankedValue(6, 8, 10, 12);
                var ability1 = NewAbility($"Wreath1_{abilityRank}_A")
                .SetBasicInformation("Ability 1", $"Deal {ability1Damage} damage to All enemies that received any damage this turn.")
                .SetVisuals(Visuals.Parry, ab1Targeting)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), ability1Damage, ab1Targeting)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Unit_AllOpponents, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(ab1Targeting, IntentForDamage(ability1Damage))
                })
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Red, Pigments.Red, Pigments.Red);

                var ability2DamageFullHealth = RankedValue(5, 6, 7, 8);
                var ability2DamageNotFull = RankedValue(8, 11, 14, 16);
                var ability2 = NewAbility($"Wreath2_{abilityRank}_A")
                .SetBasicInformation("Ability 2", $"Deal {ability2DamageFullHealth} damage to the Opposing damage. If the enemy is not at full health, deal {ability2DamageNotFull} damage instead.")
                .SetVisuals(Visuals.Burn, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<ExtraVariableForNextEffect>(), ability2DamageFullHealth),
                    Effects.GenerateEffect(CreateScriptable<DamageByEntryOrPrevExitIfFullHealth>(), ability2DamageNotFull, Targeting.Slot_Front)
                })
                .AddIntent(Targeting.Slot_Front, IntentType_GameIDs.Misc_Hidden.ToString(), IntentForDamage(ability2DamageFullHealth), IntentForDamage(ability2DamageNotFull))
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Red, Pigments.Yellow);

                var ability3Damage = RankedValue(6, 7, 9, 10);
                var ab3Targeting = Targeting.Unit_AllOpponents.FilterByHealth(true, true);
                var ab3PerformEffect = CreateScriptable<CasterPerformEffectsEffect>();
                ab3PerformEffect.effects = new()
                {
                    Effects.GenerateEffect(CreateScriptable<PerformEffectsOnRandomTargetEffect>(x => x.effects = new()
                    {
                        Effects.GenerateEffect(CreateScriptable<AnimationVisualsOnEffectTargetsEffect>(x2 => x2.visuals = Visuals.Decimate)),
                        Effects.GenerateEffect(CreateScriptable<DamageEffect>(x => x._returnKillAsSuccess = true), ability3Damage)
                    }), 0, ab3Targeting),
                    Effects.GenerateEffect(ab3PerformEffect, condition: Effects.CheckPreviousEffectCondition(true, 1))
                };
                var ability3 = NewAbility($"Wreath3_{abilityRank}_A")
                .SetBasicInformation("Ability 3", $"Deal {ability3Damage} damage to a Random enemy with the lowest health. If this kills, repeat this ability.")
                .SetEffects([Effects.GenerateEffect(ab3PerformEffect)])
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Unit_AllOpponents, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(Targeting.Spec_Unit_AllOpponents_Weakest, IntentForDamage(ability3Damage)),
                    TargetIntent(Targeting.Slot_SelfSlot, IntentType_GameIDs.Misc_Additional.ToString())
                })
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Red, Pigments.Red);

                return new(health, [ability1, ability2, ability3]);
            });

            ch.AddToDatabase();
        }
    }
}
