namespace BOSpecialFools.TriggerEffects
{
    public class PermaStatusToAllUnitsTriggerEffect(StatusEffect_SO status, string unitListSV, bool applyToAllies, bool applyToEnemies) : TriggerEffect
    {
        public override void DoEffect(IUnit sender, object args, TriggerEffectInfo triggerInfo, TriggerEffectActivationExtraInfo extraInfo)
        {
            if(extraInfo.activation == TriggerEffectActivation.Connection)
                OnConnection(sender, args, triggerInfo, extraInfo);
            else if(extraInfo.activation == TriggerEffectActivation.Disconnection)
                OnDisconnection(sender, args, triggerInfo, extraInfo);
            else
                OnTrigger(sender, args, triggerInfo, extraInfo);
        }

        public void OnConnection(IUnit sender, object args, TriggerEffectInfo triggerInfo, TriggerEffectActivationExtraInfo extraInfo)
        {
            if (!sender.IsAlive || !CombatManager.Instance._stats.TryGetUnitOnField(sender.ID, sender.IsUnitCharacter, out _))
                return;
            
            if (triggerInfo.doesPopup && extraInfo.TryGetPopupUIAction(sender.ID, sender.IsUnitCharacter, false, out var action))
                CombatManager.Instance.AddUIAction(action);
            
            ApplyStatus(sender);
        }

        public void OnTrigger(IUnit sender, object args, TriggerEffectInfo triggerInfo, TriggerEffectActivationExtraInfo extraInfo)
        {
            ApplyStatus(sender);
        }

        public void ApplyStatus(IUnit sender)
        {
            var stats = CombatManager.Instance._stats;
            var targets = new List<IUnit>();
            if(applyToAllies)
                targets.AddRange(sender.IsUnitCharacter ? stats.CharactersOnField.Values.OrderBy(x => x.SlotID) : stats.EnemiesOnField.Values.OrderBy(x => x.SlotID));
            if(applyToEnemies)
                targets.AddRange(sender.IsUnitCharacter ? stats.EnemiesOnField.Values.OrderBy(x => x.SlotID) : stats.CharactersOnField.Values.OrderBy(x => x.SlotID));

            sender.TryGetStoredData(unitListSV, out var svHolder);
            if (svHolder.m_ObjectData is not List<IUnit> uList)
                svHolder.m_ObjectData = uList = new();
            
            foreach (var u in targets)
            {
                if (uList.Contains(u))
                    continue;
                
                uList.Add(u);
                u.ApplyStatusEffect(status, 0, 1);
            }

            for (var i = 0; i < uList.Count; i++)
            {
                var u = uList[i];
                if (u == null)
                    continue;
                if (targets.Contains(u))
                    continue;
                
                uList.RemoveAt(i--);
                u.DettachStatusRestrictor(status.StatusID);
            }
        }

        public void OnDisconnection(IUnit sender, object args, TriggerEffectInfo triggerInfo, TriggerEffectActivationExtraInfo extraInfo)
        {
            sender.TryGetStoredData(unitListSV, out var svHolder);
            if (svHolder.m_ObjectData is not List<IUnit> uList)
                return;

            for (var i = 0; i < uList.Count; i++)
            {
                var u = uList[i];
                if (u == null)
                    continue;
                
                uList.RemoveAt(i--);
                u.DettachStatusRestrictor(status.StatusID);
            }
        }

        public override bool ManuallyHandlePopup => true;
    }
}