using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalAbilityTools
    {
        public static AdvancedAbilitySO NewRankedAbility(string[] ids) => NewAbility(RankedValue(ids), Profile);

        public static T NewRankedAbility<T>(string[] ids) where T : AbilitySO => NewAbility<T>(RankedValue(ids), Profile);
    }
}
