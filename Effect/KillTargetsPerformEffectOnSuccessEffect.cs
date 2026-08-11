using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class KillTargetsPerformEffectOnSuccessEffect : EffectSO
    {
        public List<EffectInfo> effects = [];
        public bool startResultIsKilledRank = false;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            
            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;
                if(!u.DirectDeath(caster, false, out _))
                    continue;

                var startRes = 0;
                if (startResultIsKilledRank && u is CharacterCombat cc)
                    startRes = cc.ClampedRank;

                exitAmount++;
                if (effects != null)
                    CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction([.. effects], caster, startRes));
            }

            return exitAmount > 0;
        }
    }
}
