using BOSpecialFools.StaticModifiers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Items
{
    public static class Item19
    {
        public static readonly string ID = "Item19_ExtraW".Prefix();

        public static void Init()
        {
            var name = "Item 19";
            var flav = "\"The Original.\"";
            var desc = "Sets this party member's maximum health to 19.";

            var item = NewItem<BasicWearable>(ID)
                .SetBasicInformation(name, flav, desc, "Item19")
                .SetStaticModifiers(MaxHealthExactSetterStaticModifier.Create(19))
                .SetPrice(19)
                .AddWithoutItemPools();
        }
    }
}
