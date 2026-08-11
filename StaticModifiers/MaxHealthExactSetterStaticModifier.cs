using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.StaticModifiers
{
    public class MaxHealthExactSetterStaticModifier : WearableStaticModifierSetterSO
    {
        public int targetMaxHealth;

        public override void OnAttachedToCharacter(WearableStaticModifiers modifiers, CharacterSO character, int rank)
        {
            var chRank = character.ClampRank(rank);
            var origMaxHealth = character.GetMaxHealth(chRank);

            modifiers.MaximumHealthModifier = targetMaxHealth - origMaxHealth;
        }

        public override void OnDettachedFromCharacter(WearableStaticModifiers modifiers)
        {
            modifiers.MaximumHealthModifier = 0;
        }

        public static MaxHealthExactSetterStaticModifier Create(int targetMaxHealth)
        {
            var m = CreateScriptable<MaxHealthExactSetterStaticModifier>();
            m.targetMaxHealth = targetMaxHealth;

            return m;
        }
    }
}
