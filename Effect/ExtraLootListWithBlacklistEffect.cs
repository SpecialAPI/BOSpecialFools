using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class ExtraLootListWithBlacklistEffect : EffectSO
    {
        public List<string> blacklist = [];
        public ExtraLootListEffect lootPool;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if(lootPool == null)
                return false;

            if (entryVariable <= 0)
                return false;

            var nothingPerc = lootPool._nothingPercentage;
            var shopPerc = lootPool._shopPercentage;
            var treasurePerc = lootPool._treasurePercentage;
            var lockedItems = lootPool._lockedLootableItems;
            var unlockedItems = lootPool._lootableItems;

            var totalProb = nothingPerc + shopPerc + treasurePerc;
            foreach(var lockedItem in lockedItems)
            {
                if (!stats.IsGameRun || !stats.InfoHolder.Game.IsItemUnlocked(lockedItem.itemName))
                    continue;

                if(blacklist != null && blacklist.Contains(lockedItem.itemName))
                    continue;

                totalProb += lockedItem.probability;
            }
            foreach (var unlockedItem in unlockedItems)
            {
                if (blacklist != null && blacklist.Contains(unlockedItem.itemName))
                    continue;

                totalProb += unlockedItem.probability;
            }

            for(var i = 0; i < entryVariable; i++)
            {
                var roll = Random.Range(0, totalProb);

                if (roll < nothingPerc)
                    continue;

                roll -= nothingPerc;
                if(roll < shopPerc)
                {
                    stats.ModdedPostCombatResults.Add(new ExtraLootWithBlacklistPostCombatResult(isTreasure: false, blacklist, 1, canBeLocked: false));
                    exitAmount++;
                    continue;
                }

                roll -= shopPerc;
                if(roll < treasurePerc)
                {
                    stats.ModdedPostCombatResults.Add(new ExtraLootWithBlacklistPostCombatResult(isTreasure: true, blacklist, 1, canBeLocked: false));
                    exitAmount++;
                    continue;
                }

                roll -= treasurePerc;
                var producedItem = false;
                foreach(var lockedItem in lockedItems)
                {
                    if (!stats.IsGameRun || !stats.InfoHolder.Game.IsItemUnlocked(lockedItem.itemName))
                        continue;

                    if (blacklist != null && blacklist.Contains(lockedItem.itemName))
                        continue;

                    if (roll < lockedItem.probability)
                    {
                        stats.AddExtraLootAddition(lockedItem.itemName);
                        exitAmount++;
                        producedItem = true;
                        break;
                    }

                    roll -= lockedItem.probability;
                }

                if (producedItem)
                    continue;

                foreach(var unlockedItem in unlockedItems)
                {
                    if (blacklist != null && blacklist.Contains(unlockedItem.itemName))
                        continue;

                    if (roll < unlockedItem.probability)
                    {
                        stats.AddExtraLootAddition(unlockedItem.itemName);
                        exitAmount++;
                        break;
                    }

                    roll -= unlockedItem.probability;
                }
            }

            return exitAmount > 0;
        }
    }
}
