using BOSpecialFools.Effect;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Characters
{
    public static class Charline
    {
        public static void Init()
        {
            var ch = NewCharacter("Charline_CH", "Charline")
                .SetBasicInformation("Charline", Pigments.Blue, null, null, null);

            ch.RankedDataSetup(4, (rank, abRank) =>
            {
                var hp = RankedValue(7, 8, 10, 11);

                var ab1NumAbilitiesPerformed = RankedValue(3, 4, 5, 6);
                var ab1Healing = RankedValue(1, 1, 2, 2);
                var ab1Name = "Ability 1";
                var ab1Description = $"Perform {ab1NumAbilitiesPerformed} random level 1 party member abilities back to back. Prevent any damage that would be dealt by this party member while performing these abilities.\nHeal this party member {ab1Healing} health.";
                var ab1 = NewAbility($"Charline1_{abRank}_A")
                .SetBasicInformation(ab1Name, ab1Description)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<PerformRandomCharacterAbilitiesWithDealtDamageAndHealingMultiplierEffect>(x =>
                    {
                        x.damageMultiplier = 0;
                        x.abilitiesRank = 0;
                    }), ab1NumAbilitiesPerformed, Targeting.Slot_SelfSlot),
                    Effects.GenerateEffect(CreateScriptable<HealEffect>(), ab1Healing, Targeting.Slot_SelfSlot)
                })
                .AddIntent(Targeting.Slot_SelfSlot, IntentType_GameIDs.Misc_Hidden.ToString(), IntentType_GameIDs.Misc.ToString())
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Yellow, Pigments.Blue, Pigments.Red);

                var ab2HealScaling = RankedValue(-30, -15, 0, 15);
                var ab2HealScalingString = ab2HealScaling switch
                {
                    <0 => $"{-ab2HealScaling}% less",
                    >0 => $"{ab2HealScaling}% more",
                    _ => "an equivalent amount of"
                };
                var ab2Name = "Ability 2";
                var ab2Description = $"Make the left ally use a random one of their abilities (except Slap). Convert any damage that they would deal while performing this ability into {ab2HealScalingString} healing applied to the target's opposing position(s).";
                var ab2 = NewAbility($"Charline2_{abRank}_A")
                .SetBasicInformation(ab2Name, ab2Description)
                .SetEffects(new()
                {
                    Effects.GenerateEffect(CreateScriptable<PerformRandomAbilityDamageIntoHealingEffect>(x =>
                    {
                        x.directHeal = true;
                        x.healingToDamagePercentageModifier = ab2HealScaling;
                        x.healTargeting = Targeting.Slot_Front;
                        x.ignoredAbilityIDs = ["Slap_A"];
                    }), 1, Targeting.Slot_AllyLeft)
                })
                .AddIntent(Targeting.Slot_AllyLeft, IntentType_GameIDs.Misc_Hidden.ToString(), IntentType_GameIDs.Misc.ToString())
                .AddToCharacterDatabase()
                .CharacterAbility(Pigments.Blue, Pigments.Blue);

                return new(hp, [ab1, ab2]);
            });

            ch.AddToDatabase(true);
        }
    }
}
