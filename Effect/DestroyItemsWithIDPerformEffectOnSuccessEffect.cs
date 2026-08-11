using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Effect
{
    public class DestroyItemsWithIDPerformEffectOnSuccessEffect : EffectSO
    {
        public List<string> itemIDs = [];
        public List<EffectInfo> effects = [];

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (itemIDs == null || itemIDs.Count == 0)
                return false;

            foreach(var t in targets)
            {
                if(!t.HasUnit)
                    continue;

                var u = t.Unit;
                if(!u.HasUsableItem || !itemIDs.Contains(u.HeldItem.name))
                    continue;

                if(!u.TryConsumeWearable())
                    continue;

                exitAmount++;
                if (effects != null)
                    CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction([.. effects], caster));
            }

            return exitAmount > 0;
        }
    }
}
