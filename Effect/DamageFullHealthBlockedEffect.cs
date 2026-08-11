using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class DamageFullHealthBlockedEffect : CustomDamageEffectBase
    {
        public bool blockedByNotFullHealth;

        public override int BaseDamageAmount(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            if((unit.CurrentHealth < unit.MaximumHealth) == blockedByNotFullHealth)
                return 0;

            return entryVariable;
        }

        public static EffectSO Create(bool blockedByNotFullHealth = false, bool indirect = false, bool usePreviousExit = false, bool successOnKill = false, bool ignoreShield = false, string deathType = nameof(DeathType_GameIDs.Basic), string specialDamage = "")
        {
            var e = Create<DamageFullHealthBlockedEffect>(indirect, usePreviousExit, successOnKill, ignoreShield, deathType, specialDamage);
            e.blockedByNotFullHealth = blockedByNotFullHealth;

            return e;
        }
    }
}
