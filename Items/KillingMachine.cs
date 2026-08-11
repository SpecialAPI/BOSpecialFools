using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Items
{
    public static class KillingMachine
    {
        public static readonly string ID = "KillingMachine_ExtraW";

        public static void Init()
        {
            var enemyDamage = 6;
            var partyDamage = 3;

            var name = "Killing Machine";
            var flav = "\"Kills people.\"";
            var desc = $"Deal {enemyDamage} damage to all enemies and {partyDamage} damage to all party members at the end of each turn. Damage is fully blocked against targets at full health.";

            var item = NewItem<MultiCustomTriggerEffectWearable>(ID)
                .SetBasicInformation(name, flav, desc, "KillingMachine")
                .SetPrice(6)
                .AddWithoutItemPools();

            item.SetTriggerEffects(new()
            {
                new()
                {
                    trigger = TriggerCalls.OnTurnFinished.ToString(),
                    doesPopup = true,
                    immediate = false,

                    effect = new PerformEffectTriggerEffect(new()
                    {
                        Effects.GenerateEffect(DamageFullHealthBlockedEffect.Create(), enemyDamage, Targeting.Unit_AllOpponents),
                        Effects.GenerateEffect(DamageFullHealthBlockedEffect.Create(), partyDamage, Targeting.Unit_AllAllies),
                    })
                }
            });
        }
    }
}
