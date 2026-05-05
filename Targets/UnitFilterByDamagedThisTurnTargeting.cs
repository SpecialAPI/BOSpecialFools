using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Targets
{
    public class UnitFilterByDamagedThisTurnTargeting : UnitFilterTargetingBase
    {
        public bool needsToBeDamaged;

        protected override bool FilterUnit(IUnit unit, SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var history = unit.DamageHistory();
            var currentTurn = CS.CurrentPlayerTurn();

            var damaged = history.Count > 0 && history[history.Count - 1].turn == currentTurn;
            return damaged == needsToBeDamaged;
        }
    }
}
