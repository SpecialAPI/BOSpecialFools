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
    }
}
