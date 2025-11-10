using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    public class TurnDamageIntoHealingIntValueModifier(IUnit damageTarget, IUnit healingSource, BaseCombatTargettingSO healTargeting, int healingToDamagePercentageModifier = 0, bool direct = true) : IntValueModifier(24)
    {
        public override int Modify(int value)
        {
            if(value <= 0)
                return value;

            if (damageTarget == null)
                return 0;

            var targets = healTargeting.GetTargets(CombatManager.Instance._stats.combatSlots, damageTarget.SlotID, damageTarget.IsUnitCharacter);
            if (targets == null)
                return 0;

            foreach(var t in targets)
            {
                if(t == null || !t.HasUnit)
                    continue;

                var u = t.Unit;
                var amount = Mathf.Max(1, Mathf.FloorToInt(value * (healingToDamagePercentageModifier + 100f) / 100f));
                if (direct && healingSource != null)
                    amount = healingSource.WillApplyHeal(amount, u);

                if (direct)
                    u.Heal(amount, healingSource, true, CombatType_GameIDs.Heal_Basic.ToString());
                else
                    u.Heal(amount, null, false, CombatType_GameIDs.Heal_Basic.ToString());
            }

            return 0;
        }
    }
}
