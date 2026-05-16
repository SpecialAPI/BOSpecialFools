using BOSpecialFools.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalTargetingTools
    {
        public static BaseCombatTargettingSO Join(this BaseCombatTargettingSO a, BaseCombatTargettingSO b, bool? areTargetAllies = null, bool? areTargetSlots = null)
        {
            JoinTargeting j;

            if (a is JoinTargeting aj)
            {
                if (b is JoinTargeting bj)
                    aj.targeting.AddRange(bj.targeting);
                else
                    aj.targeting.Add(b);

                j = aj;
            }
            else if (b is JoinTargeting bj)
            {
                bj.targeting.Insert(0, a);

                j = bj;
            }
            else
            {
                j = CreateScriptable<JoinTargeting>();

                j.targeting.Add(a);
                j.areTargetAllies = a.AreTargetAllies;
                j.areTargetSlots = a.AreTargetSlots;

                j.targeting.Add(b);
            }

            if (areTargetAllies is bool ata)
                j.areTargetAllies = ata;
            if (areTargetSlots is bool ats)
                j.areTargetSlots = ats;

            return j;
        }

        public static BaseCombatTargettingSO FilterUnitByDelegate(this BaseCombatTargettingSO orig, Func<IUnit, SlotsCombat, int, bool, bool> filter)
        {
            var f = CreateScriptable<UnitFilterByDelegateTargeting>();
            f.orig = orig;
            f.filter = filter;

            return f;
        }

        public static BaseCombatTargettingSO FilterUnitByDamagedThisTurn(this BaseCombatTargettingSO orig, bool needsToBeDamaged)
        {
            var f = CreateScriptable<UnitFilterByDamagedThisTurnTargeting>();
            f.orig = orig;
            f.needsToBeDamaged = needsToBeDamaged;

            return f;
        }

        public static BaseCombatTargettingSO MinMaxByPosition(this BaseCombatTargettingSO orig, bool getRightmost)
        {
            var m = CreateScriptable<UnitMinMaxByPositionTargeting>();
            m.orig = orig;
            m.isMax = getRightmost;

            return m;
        }
    }
}
