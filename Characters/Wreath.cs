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

            ch.RankedDataSetup(4, (rank, abilityRank) =>
            {
                var health = RankedValue(14, 16, 17, 18);

                var abATargeting = Targeting.Unit_AllOpponents.FilterUnitByDamagedThisTurn(true);
                var abilityADamage = RankedValue(6, 8, 10, 12);
                var abilityA = NewAbility($"WreathA_{abilityRank}_A")
                .SetBasicInformationCharacter($"Ability A {abilityRank}", $"Deal {abilityADamage} damage to All enemies that received any damage this turn.")
                .SetVisuals(Visuals.Clobber_Left, abATargeting)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CommonEffects.Damage, abilityADamage, abATargeting)
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

                var abilityCDamage = RankedValue(5, 6, 7, 9);
                var abilityCTargetRightIfNoLeft = RankedValue(false, false, true, true);
                var abilityC = NewAbility($"WreathC_{abilityRank}_A")
                .SetBasicInformationCharacter($"Ability C {abilityRank}", $"Deal {abilityCDamage} damage to the Opposing enemy. If the Opposing enemy was already damaged this turn, inflict 1 Frail to the Left enemy." + (abilityCTargetRightIfNoLeft ?
                    " If there is no Left enemy, apply the Frail to the Right enemy instead." :
                    ""))
                .SetVisuals(Visuals.Writhe, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<CheckTargetsDamagedThisTurn>(), 0, Targeting.Slot_Front),

                    Effects.GenerateEffect(CommonEffects.Damage, abilityCDamage, Targeting.Slot_Front),
                    Effects.GenerateEffect(CommonEffects.ApplyFrail, 1, 
                        abilityCTargetRightIfNoLeft ?
                            Targeting.Slot_OpponentSides.MinMaxByPosition(false) :
                            Targeting.Slot_OpponentLeft,
                        Effects.CheckPreviousEffectCondition(true, 2))
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Slot_Front, IntentForDamage(abilityCDamage)),
                    TargetIntent(
                        abilityCTargetRightIfNoLeft ?
                            Targeting.Slot_OpponentSides :
                            Targeting.Slot_OpponentLeft,
                        IntentType_GameIDs.Status_Frail.ToString(), IntentType_GameIDs.Misc_Hidden.ToString())
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
