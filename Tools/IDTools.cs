using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class IDTools
    {
        public static string[] GenerateLevels(this string id, int numLevels = 4)
        {
            var output = new string[numLevels];

            for(var i = 0; i < numLevels; i++)
                output[i] = string.Format(id, i + 1);

            return output;
        }

        public static string Prefix(this string id)
        {
            return $"{MOD_PREFIX}_{id}";
        }

        public static string[] Prefix(this string[] ids)
        {
            var output = new string[ids.Length];

            for(var i = 0; i < ids.Length; i++)
                output[i] = ids[i].Prefix();

            return output;
        }
    }
}
