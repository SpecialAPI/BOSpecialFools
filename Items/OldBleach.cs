using BOSpecialFools.StaticModifiers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Items
{
    public static class OldBleach
    {
        public static readonly string ID = "OldBleach_ExtraW".Prefix();

        public static void Init()
        {
            var name = "Old Bleach";
            var flav = "\"RIP 2023-2026\"";
            var desc = "This party member no longer has any passives.";

            NewItem<BasicWearable>(ID)
                .SetBasicInformation(name, flav, desc, "OldBleach")
                .SetStaticModifiers(ModdedDataModifier<BleachStaticModifier>(BleachStaticModifier.ID))
                .SetPrice(0)
                .AddWithoutItemPools()
                .AddItemTypes(ItemType_GameIDs.Magic.ToString());
        }
    }
}
