using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Misc
{
    public class AddObserverAction(Action<object, object> handler, string trigger, object sender) : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            CombatManager.Instance.AddObserver(handler, trigger, sender);
            yield break;
        }
    }
}
