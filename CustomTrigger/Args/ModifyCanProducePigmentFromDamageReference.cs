using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.CustomTrigger.Args
{
    public class ModifyCanProducePigmentFromDamageReference(bool curr, IUnit damagedUnit, IUnit possibleSourceUnit, int amount, string damageTypeID, bool directDamage, bool ignoreShield, int affectedStartSlot, int affectedEndSlot) : IBoolHolder, IIntHolder, IStringHolder, IUnitHolder
    {
        public bool canProducePigmentFromDamage = curr;
        public readonly IUnit damagedUnit = damagedUnit;
        public readonly IUnit sourceUnit = possibleSourceUnit;
        public readonly int amount = amount;
        public readonly string damageTypeID = damageTypeID;
        public readonly bool directDamage = directDamage;
        public readonly bool ignoreShield = ignoreShield;
        public readonly int affectedStartSlot = affectedStartSlot;
        public readonly int affectedEndSlot = affectedEndSlot;

        bool IBoolHolder.this[int index]
        {
            get => index switch
            {
                0 => canProducePigmentFromDamage,
                1 => directDamage,
                2 => ignoreShield,

                _ => false
            };
            set
            {
                if(index == 0)
                    canProducePigmentFromDamage = value;
                else
                    Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's second and third bool values are read-only.");
            }
        }
        bool IBoolHolder.Value
        {
            get => canProducePigmentFromDamage;
            set => canProducePigmentFromDamage = value;
        }

        int IIntHolder.this[int index]
        {
            get => index switch
            {
                0 => amount,
                1 => affectedStartSlot,
                2 => affectedEndSlot,

                _ => 0
            };
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's int values are read-only.");
        }
        int IIntHolder.Value
        {
            get => amount;
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's int values are read-only.");
        }

        IUnit IUnitHolder.this[int index]
        {
            get => index switch
            {
                0 => damagedUnit,
                1 => sourceUnit,

                _ => null
            };
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's Unit values are read-only.");
        }
        IUnit IUnitHolder.Value
        {
            get => damagedUnit;
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's Unit values are read-only.");
        }

        string IStringHolder.this[int index]
        {
            get => damageTypeID;
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's string value is read-only.");
        }
        string IStringHolder.Value
        {
            get => damageTypeID;
            set => Debug.LogWarning($"ModifyCanProducePigmentFromDamageReference's string value is read-only.");
        }
    }
}
