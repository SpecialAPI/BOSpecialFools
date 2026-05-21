using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class DamageByMissingHealthUpToEntryEffect : CustomDamageEffectBase
    {
        public override int BaseDamageAmount(IUnit unit, TargetSlotInfo target, CombatStats stats, IUnit caster, bool areTargetSlots, int entryVariable, bool indirect)
        {
            return Mathf.Clamp(unit.MaximumHealth - unit.CurrentHealth, 0, entryVariable);
        }

        public static EffectSO Create(bool indirect = false, bool usePreviousExit = false, bool successOnKill = false, bool ignoreShield = false, string deathType = nameof(DeathType_GameIDs.Basic), string specialDamage = "")
        {
            var e = Create<DamageByMissingHealthUpToEntryEffect>(indirect, usePreviousExit, successOnKill, ignoreShield, deathType, specialDamage);

            return e;
        }
    }
}
