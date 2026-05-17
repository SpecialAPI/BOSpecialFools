using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools
{
    public static class IDs
    {
        public static readonly string WreathDB = "Wreath_CH";
        public static readonly string WreathEntity = "Wreath";

        public static readonly string[] WreathAbilityA = "WreathA_{0}_A".GenerateLevels();
        public static readonly string[] WreathAbilityB = "WreathB_{0}_A".GenerateLevels();
        public static readonly string[] WreathAbilityC = "WreathC_{0}_A".GenerateLevels();

        public static class Pref
        {
            public static readonly string WreathDB = IDs.WreathDB.Prefix();
            public static readonly string WreathEntity = IDs.WreathEntity.Prefix();

            public static readonly string[] WreathAbilityA = IDs.WreathAbilityA.Prefix();
            public static readonly string[] WreathAbilityB = IDs.WreathAbilityB.Prefix();
            public static readonly string[] WreathAbilityC = IDs.WreathAbilityC.Prefix();
        }
    }
}
