using BOSpecialFools.Effect;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Characters
{
    public static class AStar
    {
        public static void Init()
        {
            var ch = NewCharacter("AStar_CH", "AStar")
                .SetBasicInformation("A-Star", Pigments.Purple, null, null, null);

            ch.RankedDataSetup(4, (rank, abRank) =>
            {
                var hp = 12;

                var ab1Damage = RankedValue(8, 10, 12, 14);
                var ab1Name = "Ability 1";
                var ab1Description = $"Deals {ab1Damage} reliable damage to the opposing enemy.";
                var ab1 = NewAbility($"AStar1_{abRank}_A")
                    .SetBasicInformation(ab1Name, ab1Description, null)
                    .SetEffects(new()
                    {
                        Effects.GenerateEffect(CreateScriptable<SpecialDamageEffect>(x => x.damageInfo = new()
                        {
                            ExtraDamageModifierPercentage = -100
                        }))
                    })
                    .AddIntent(Targeting.Slot_Front, IntentForDamage(ab1Damage), IntentType_GameIDs.Misc.ToString())
                    .SetVisuals(Visuals.Decimate, Targeting.Slot_Front)
                    .AddToCharacterDatabase()
                    .CharacterAbility(Pigments.Red, Pigments.Red, Pigments.Red);

                return new(hp, [ab1]);
            });
        }
    }
}
