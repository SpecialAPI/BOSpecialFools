using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalItemTools
    {
        public static BaseWearableSO GetRandomItemWithBlacklist(this GameInformationHolder infoHolder, bool isTreasure, List<string> blacklist, bool canBeLocked = false)
        {
            var (uncheckedItems, itemsInRun) = infoHolder.GetItemPool(isTreasure);

            return GetItemFromPoolWithBlacklist(uncheckedItems, itemsInRun, blacklist, infoHolder.Game, canBeLocked);
        }

        public static (List<string> uncheckedItems, HashSet<string> itemsInRun) GetItemPool(this GameInformationHolder infoHolder, bool treasure)
        {
            var itempool = LoadedDBsHandler.ItemPoolDB;
            var uncheckedItems = treasure ? itempool._uncheckedTreasureItems : itempool._uncheckedShopItems;

            HashSet<string> itemsInRun = null;
            if (infoHolder.HasRunData)
                itemsInRun = treasure ? infoHolder.Run.PrizesInRun : infoHolder.Run.ShopItemsInRun;

            return (uncheckedItems, itemsInRun);
        }

        private static BaseWearableSO GetItemFromPoolWithBlacklist(List<string> uncheckedItems, HashSet<string> itemsInRun, List<string> blacklist, IGameCheckData gameDB, bool canBeLocked = false)
        {
            if (uncheckedItems == null || uncheckedItems.Count == 0)
                return null;

            BaseWearableSO res = null;
            var uncheckedItemsCopy = uncheckedItems.ToList();
            while (uncheckedItemsCopy.Count > 0)
            {
                var itemName = uncheckedItemsCopy.GetAndRemoveRandomElement();
                var item = GetWearable(itemName);

                if (item == null)
                    continue;

                if (itemsInRun != null && itemsInRun.Contains(itemName))
                    continue;

                if (blacklist != null && blacklist.Contains(itemName))
                    continue;

                if (!canBeLocked && item.startsLocked && !gameDB.IsItemUnlocked(itemName))
                    continue;

                res = item;
                break;
            }

            if (res != null && itemsInRun != null)
                itemsInRun.Add(res.name);

            return res;
        }
    }
}
