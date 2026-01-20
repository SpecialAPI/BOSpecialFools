using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Args
{
    public class OnBeforeAbilityUsedContext(int abilityIndex, CombatAbility cAbility, AbilitySO ability, FilledManaCost[] cost = null)
    {
        public readonly int abilityIndex = abilityIndex;
        public readonly CombatAbility combatAbility = cAbility;
        public readonly AbilitySO abilitySO = ability;
        public readonly FilledManaCost[] cost = cost;
    }
}
