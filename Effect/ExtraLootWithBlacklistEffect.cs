using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class ExtraLootWithBlacklistEffect : EffectSO
    {
        public List<string> blacklist = [];
        public bool treasure;
        public bool getLocked = false;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = entryVariable;

            stats.ModdedPostCombatResults.Add(new ExtraLootWithBlacklistPostCombatResult(treasure, blacklist, entryVariable, getLocked));

            return exitAmount > 0;
        }
    }
}
