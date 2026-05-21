using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class CommonEffects
    {
        public static readonly EffectSO Damage = CreateScriptable<DamageEffect>();
        public static readonly EffectSO IndirectDamage = CreateScriptable<DamageEffect>(x => x._indirect = true);

        public static readonly EffectSO Heal = CreateScriptable<HealEffect>();

        public static readonly EffectSO SwapLeft = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false);
        public static readonly EffectSO SwapRight = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = true);
        public static readonly EffectSO SwapSides = CreateScriptable<SwapToSidesEffect>();

        public static readonly EffectSO GenRed = GenPigment(Pigments.Red);
        public static readonly EffectSO GenBlue = GenPigment(Pigments.Blue);
        public static readonly EffectSO GenYellow = GenPigment(Pigments.Yellow);
        public static readonly EffectSO GenPurple = GenPigment(Pigments.Purple);
        public static readonly EffectSO GenRandomBasicPigment = GenRandomPigment(Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple);
        public static readonly EffectSO RandomizeAllPigment = RandomizeAllPigmntInto(Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple);

        public static readonly EffectSO Refresh = CreateScriptable<RefreshAbilityUseEffect>();
        public static readonly EffectSO RestoreSwap = CreateScriptable<RestoreSwapUseEffect>();

        public static readonly EffectSO UnitCheck = CreateScriptable<CheckHasUnitEffect>();

        public static readonly EffectSO ApplyFocused = ApplyStatus(StatusField.Focused);
        public static readonly EffectSO ApplyRuptured = ApplyStatus(StatusField.Ruptured);
        public static readonly EffectSO ApplyFrail = ApplyStatus(StatusField.Frail);
        public static readonly EffectSO ApplyOilSlicked = ApplyStatus(StatusField.OilSlicked);
        public static readonly EffectSO ApplySpotlight = ApplyStatus(StatusField.Spotlight);
        public static readonly EffectSO ApplyCursed = ApplyStatus(StatusField.Cursed);
        public static readonly EffectSO ApplyLinked = ApplyStatus(StatusField.Linked);
        public static readonly EffectSO ApplyDivineProtection = ApplyStatus(StatusField.Focused);
        public static readonly EffectSO ApplyScars = ApplyStatus(StatusField.Scars);
        public static readonly EffectSO ApplyGutted = ApplyStatus(StatusField.Gutted);
        public static readonly EffectSO ApplyStunned = ApplyStatus(StatusField.Stunned);

        public static readonly EffectSO ApplyConstricted = ApplyField(StatusField.Constricted);
        public static readonly EffectSO ApplyFire = ApplyField(StatusField.OnFire);
        public static readonly EffectSO ApplyShield = ApplyField(StatusField.Shield);

        public static EffectSO ApplyStatus(StatusEffect_SO status, bool oneRandomTarget = false)
        {
            var e = CreateScriptable<StatusEffect_Apply_Effect>();
            e._Status = status;
            e._JustOneRandomTarget = oneRandomTarget;

            return e;
        }

        public static EffectSO ApplyField(FieldEffect_SO field)
        {
            var e = CreateScriptable<FieldEffect_Apply_Effect>();
            e._Field = field;

            return e;
        }

        public static EffectSO Animation(AttackVisualsSO anim)
        {
            var e = CreateScriptable<AnimationVisualsOnEffectTargetsEffect>();
            e.visuals = anim;

            return e;
        }

        public static EffectSO AddPassive(BasePassiveAbilitySO passive)
        {
            var e = CreateScriptable<AddPassiveEffect>();
            e._passiveToAdd = passive;

            return e;
        }

        public static EffectSO GenPigment(ManaColorSO mana)
        {
            var e = CreateScriptable<GenerateColorManaEffect>();
            e.mana = mana;

            return e;
        }

        public static EffectSO GenRandomPigment(params ManaColorSO[] pool)
        {
            var e = CreateScriptable<GenerateRandomManaBetweenEffect>();
            e.possibleMana = pool;

            return e;
        }

        public static EffectSO RandomizeAllPigmntInto(params ManaColorSO[] pool)
        {
            var e = CreateScriptable<RandomizeAllManaEffect>();
            e.manaRandomOptions = pool;

            return e;
        }

        public static EffectSO SetCasterStoredValue(string storedValue)
        {
            var e = CreateScriptable<CasterStoreValueSetterEffect>();
            e.m_unitStoredDataID = storedValue;

            return e;
        }
    }
}
