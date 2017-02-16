namespace WellFired
{
    using System;

    [USequencerFriendlyName("FGO Actor Change Limit Count"), USequencerEvent("FGO/Character/Change Limit Count")]
    public class USFGOChangeLimitCountEvent : USEventBase
    {
        public int limitCount;

        public override void FireEvent()
        {
            base.Duration = 0.5f;
            if (base.AffectedObject != null)
            {
                BattleActorControl control = base.AffectedObject.GetComponent<BattleActorControl>();
                BattleFBXComponent component = base.AffectedObject.GetComponent<BattleFBXComponent>();
                if ((control != null) && (component != null))
                {
                    if (this.limitCount != -1)
                    {
                        component.SetEvolutionLevel(control.getServantId(), this.limitCount);
                    }
                    else
                    {
                        component.ChangeActorLimitCount();
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
        }

        public void Update()
        {
        }
    }
}

