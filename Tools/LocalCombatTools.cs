using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalCombatTools
    {
        public static CombatManager CM => CombatManager.Instance;
        public static CombatStats CS => CombatManager.Instance._stats;

        public static int CurrentPlayerTurn(this CombatStats stats) => stats.TurnsPassed + (stats.IsPlayerTurn ? 1 : 0);

        public static CombatSlot GetSlot(this TargetSlotInfo target)
        {
            if(target == null)
                return null;

            if(!CS.combatSlots.TryGetSlot(target.SlotID, target.IsTargetCharacterSlot, out var slot))
                return null;

            return slot;
        }
    }
}
