using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger
{
    public static class LocalCustomTriggers
    {
        public static readonly string OnAnyoneFleetingEnd = $"{MOD_PREFIX}_OnAnyoneFleetingEnd";
        public static readonly string OnAnyoneDamaged = $"{MOD_PREFIX}_OnAnyoneDamaged";
        public static readonly string OnAnyoneDirectDamaged = $"{MOD_PREFIX}_OnAnyoneDirectDamaged";
        public static readonly string OnAnyonesMaxHealthChanged = $"{MOD_PREFIX}_OnAnyonesMaxHealthChanged";
        public static readonly string OnAnyoneHealed = $"{MOD_PREFIX}_OnAnyoneHealed";
        public static readonly string OnAnyoneBeingDamaged = $"{MOD_PREFIX}_OnAnyoneBeingDamaged";
        public static readonly string ModifyCanProducePigmentFromDamage_Anyone = $"{MOD_PREFIX}_ModifyCanProducePigmentFromDamage_Anyone";
        public static readonly string OnBeforeAbilityAnimation = $"{MOD_PREFIX}_OnBeforeAbilityAnimation";
        public static readonly string OnPassivePopup = $"{MOD_PREFIX}_OnPassivePopup";
    }
}
