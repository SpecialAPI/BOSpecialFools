using BOSpecialFools.CustomTargeting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalTargetingTools
    {
        public static BaseCombatTargettingSO FilterUnit(this BaseCombatTargettingSO orig, Func<IUnit, bool> filter)
        {
            var f = CreateScriptable<UnitFilterTargeting>();
            f.orig = orig;
            f.filter = filter;

            return f;
        }

        public static BaseCombatTargettingSO FilterByHealth(this BaseCombatTargettingSO orig, bool getWeakest, bool ignoreDead = true)
        {
            var f = CreateScriptable<FilterByHealthTargeting>();
            f.orig = orig;
            f.getWeakest = getWeakest;
            f.ignoreDead = ignoreDead;

            return f;
        }
    }
}
