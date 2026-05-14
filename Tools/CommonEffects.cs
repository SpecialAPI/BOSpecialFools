using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class CommonEffects
    {
        public static EffectSO Damage = CreateScriptable<DamageEffect>();
        public static EffectSO IndirecDamage = CreateScriptable<DamageEffect>(x => x._indirect = true);
        public static EffectSO SwapLeft = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false);
        public static EffectSO SwapRight = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = true);
        public static EffectSO SwapSides = CreateScriptable<SwapToSidesEffect>();

        public static EffectSO ApplyFocused = ApplyStatus(StatusField.Focused);
        public static EffectSO ApplyRuptured = ApplyStatus(StatusField.Ruptured);
        public static EffectSO ApplyFrail = ApplyStatus(StatusField.Frail);
        public static EffectSO ApplyOilSlicked = ApplyStatus(StatusField.OilSlicked);
        public static EffectSO ApplySpotlight = ApplyStatus(StatusField.Spotlight);
        public static EffectSO ApplyCursed = ApplyStatus(StatusField.Cursed);
        public static EffectSO ApplyLinked = ApplyStatus(StatusField.Linked);
        public static EffectSO ApplyDivineProtection = ApplyStatus(StatusField.Focused);
        public static EffectSO ApplyScars = ApplyStatus(StatusField.Scars);
        public static EffectSO ApplyGutted = ApplyStatus(StatusField.Gutted);
        public static EffectSO ApplyStunned = ApplyStatus(StatusField.Stunned);

        public static EffectSO ApplyConstricted = ApplyField(StatusField.Constricted);
        public static EffectSO ApplyFire = ApplyField(StatusField.OnFire);
        public static EffectSO ApplyShield = ApplyField(StatusField.Shield);

        public static EffectSO ApplyStatus(StatusEffect_SO status) => CreateScriptable<StatusEffect_Apply_Effect>(x => x._Status = status);
        public static EffectSO ApplyField(FieldEffect_SO field) => CreateScriptable<FieldEffect_Apply_Effect>(x => x._Field = field);
    }
}
