using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Characters
{
    public static class F23
    {
        public static readonly string ID = "23_CH".Prefix();
        public static readonly string EntityID = "23".Prefix();
        public static readonly string[] AbilityAIDs = "23A_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] AbilityBIDs = "23B_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] AbilityCIDs = "23C_{0}_A".Prefix().GenerateLevels();

        public static void Init()
        {
            var ch = NewCharacter(ID, EntityID)
                .SetBasicInformation("23", Pigments.Purple, "23Front", "23Back", "23OW")
                .AddUnitTypes(UnitTypes.FemaleLooking, UnitTypes.FemaleID, UnitTypes.Sandwich_NULL);

            ch.RankedDataSetup(4, (rank, abilityRank) =>
            {
                var health = RankedValue(9, 10, 11, 12);

                var abilityADamage = RankedValue(9, 12, 16, 19);
                var abilityAFog = 3;
                var abilityAReroll = 1;
                var abilityA = NewRankedAbility(AbilityAIDs)
                .SetBasicInformationCharacter($"Ability A {abilityRank}", $"Deal {abilityADamage} damage to the Opposing enemy. Apply {abilityAFog} Fog to the Opposing position.\nReroll {abilityAReroll} of the Opposing enemy's abilities.")
                .SetVisuals(Visuals.Equal, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CommonEffects.Damage, abilityADamage, Targeting.Slot_Front),
                    Effects.GenerateEffect(CommonEffects.ApplyField(StatusField.Shield), abilityAFog, Targeting.Slot_Front),
                    Effects.GenerateEffect(CommonEffects.RerollTargetAbilities, abilityAReroll, Targeting.Slot_Front),
                })
                .AddIntent(Targeting.Slot_Front, IntentForDamage(abilityADamage), IntentType_GameIDs.Field_Shield.ToString(), IntentType_GameIDs.Misc.ToString())
                .CharacterAbility(Pigments.Red, Pigments.Yellow, Pigments.Yellow);

                var abBTargeting = Targeting.Slot_OpponentAllSlots.FilterTargetByFieldEffect(StatusField.Shield);
                var abBIntentTargeting = abBTargeting.Join(Targeting.Slot_OpponentSides);
                var abilityBDamage = RankedValue(8, 10, 13, 15);
                var abilityBFog = 3;
                var abilityB = NewRankedAbility(AbilityBIDs)
                .SetBasicInformationCharacter($"Ability B {abilityRank}", $"Apply 3 Fog to the Left and Right enemy positions. Deal {abilityBDamage} damage to all enemy positions with Fog.")
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CommonEffects.ApplyField(StatusField.Shield), abilityBFog, Targeting.Slot_OpponentSides),
                    Effects.GenerateEffect(CommonEffects.Animation(Visuals.Conductor), 0, abBTargeting),
                    Effects.GenerateEffect(CommonEffects.Damage, abilityBDamage, abBTargeting)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Slot_OpponentSides, IntentType_GameIDs.Field_Shield.ToString()),
                    TargetIntent(abBIntentTargeting, IntentForDamage(abilityBDamage))
                })
                .CharacterAbility(Pigments.Red, Pigments.Red, Pigments.Red, Pigments.Blue);

                var abCOilTargeting = Targeting.GenerateSlotTarget([-1, 0], false, false);
                var abilityCDamage = RankedValue(5, 7, 9, 11);
                var abilityCOil = 3;
                var abilityC = NewRankedAbility(AbilityCIDs)
                .SetBasicInformationCharacter($"Ability C {abilityRank}", $"Deal {abilityCDamage} damage to the Opposing enemy. Inflict {abilityCOil} Oil-Slicked to the Left and Opposing enemies.")
                .SetVisuals(Visuals.Connection, Targeting.Slot_Front)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CommonEffects.Damage, abilityCDamage, Targeting.Slot_Front),
                    Effects.GenerateEffect(CommonEffects.ApplyOilSlicked, abilityCOil, abCOilTargeting)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targeting.Slot_Front, IntentForDamage(abilityCDamage)),
                    TargetIntent(abCOilTargeting, IntentType_GameIDs.Status_OilSlicked.ToString())
                })
                .CharacterAbility(Pigments.Yellow, Pigments.Blue);

                return new(health, [abilityA, abilityB, abilityC]);
            });

            ch.AddToDatabase();

            var menu = ch.GenerateMenuCharacter("23Unlocked", "23Locked");
            menu.SetAsFullDPS();
            menu.AddToDatabase();
        }
    }
}
