using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools
{
    public static class IDs
    {
        public static readonly string WreathDB = "Wreath_CH".Prefix();
        public static readonly string WreathEntity = "Wreath".Prefix();

        public static readonly string[] WreathAbilityA = "WreathA_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] WreathAbilityB = "WreathB_{0}_A".Prefix().GenerateLevels();
        public static readonly string[] WreathAbilityC = "WreathC_{0}_A".Prefix().GenerateLevels();
    }
}
