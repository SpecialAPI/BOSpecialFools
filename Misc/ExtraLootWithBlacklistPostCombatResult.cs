using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    public class ExtraLootWithBlacklistPostCombatResult(bool isTreasure, List<string> blacklist, int numItems = 1, bool canBeLocked = false) : AdvancedModdedPostCombatResult
    {
        public override void AdvancedApplyPostCombatResult(GameInformationHolder info, PostCombatResultsExtraData extraData)
        {
            for (var i = 0; i < numItems; i++)
            {
                var item = info.GetRandomItemWithBlacklist(isTreasure, blacklist, canBeLocked);

                if (item != null)
                    extraData.AddItem(item);
            }
        }
    }
}
