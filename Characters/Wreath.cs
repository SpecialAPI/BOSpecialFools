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
                .SetBasicInformationCharacter($"Ability A {abilityRank}", $"Deal {abilityADamage} damage to All enemies that received any damage this turn.")
                .SetVisuals(Visuals.Clobber_Left, abATargeting)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), abilityADamage, abATargeting)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Unit_AllOpponents, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(abATargeting, IntentForDamage(abilityADamage))
                })
                .CharacterAbility(Pigments.Yellow, Pigments.Blue, Pigments.Red);

                var abilityBMaxDamage = RankedValue(8, 12, 15, 18);
                var abilityB = NewAbility($"WreathB_{abilityRank}_A")
                .SetBasicInformationCharacter($"Ability B {abilityRank}", $"Deal damage to the Opposing enemy equal to their missing health, up to a maximum of {abilityBMaxDamage}. If successful, reduce the Opposing enemy's maximum health to their current health.")
                .SetVisuals(Visuals.Flay, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<DamageByMissingHealthUpToEntryEffect>(), abilityBMaxDamage, Targeting.Slot_Front),
                    Effects.GenerateEffect(CreateScriptable<ChangeMaxHealthByCurrentHealthEffect>(), 0, Targeting.Slot_Front, Effects.CheckPreviousEffectCondition(true, 1)),
                })
                .AddIntent(Targeting.Slot_Front, IntentForDamage(abilityBMaxDamage), IntentType_GameIDs.Other_MaxHealth.ToString())
                .CharacterAbility(Pigments.Red, Pigments.Yellow);

                var abilityCThreshold = RankedValue(10, 14, 18, 22);
                var abilityCDamage = RankedValue(6, 7, 9, 11);
                var abilityC = NewAbility($"WreathC_{abilityRank}_A")
                .SetBasicInformationCharacter($"Ability C {abilityRank}", $"If the Opposing enemy has {abilityCThreshold} or less health, apply Focused to this party member.\nDeal {abilityCDamage} damage to the Opposing enemy.")
                .SetVisuals(Visuals.Writhe, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<CheckTargetsHealthCompareToEntryEffect>(), abilityCThreshold, Targeting.Slot_Front),
                    Effects.GenerateEffect(CreateScriptable<StatusEffect_Apply_Effect>(x => x._Status = StatusField.Focused), 0, Targeting.Slot_SelfSlot, Effects.CheckPreviousEffectCondition(true, 1)),

                    Effects.GenerateEffect(CreateScriptable<DamageEffect>(), abilityCDamage, Targeting.Slot_Front),
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Slot_Front, IntentType_GameIDs.Misc_Hidden.ToString()),
                    TargetIntent(Targeting.Slot_SelfSlot, IntentType_GameIDs.Status_Focused.ToString()),
                    TargetIntent(Targeting.Slot_Front, IntentForDamage(abilityCDamage)),
                })
                .CharacterAbility(Pigments.Red, Pigments.Red, Pigments.Blue);

                return new(health, [abilityA, abilityB, abilityC]);
            });

            ch.AddToDatabase();

            var menu = ch.GenerateMenuCharacter("WreathUnlocked", "WreathLocked");
            menu.SetAsFullDPS();
            menu.AddToDatabase();
        }
    }
}
