namespace WellFired
{
    using System;

    [USequencerEvent("FGO/Character/Change Limit Count 2"), USequencerFriendlyName("FGO Actor Change Limit Count 2")]
    public class USFGOChangeLimitCount2Event : USEventBase
    {
        public int[] limitCountList = new int[] { 0, 1, 2, 3, 4 };

        public override void FireEvent()
        {
            base.Duration = 0.5f;
            if (base.AffectedObject != null)
            {
                BattleActorControl control = base.AffectedObject.GetComponent<BattleActorControl>();
                BattleFBXComponent component = base.AffectedObject.GetComponent<BattleFBXComponent>();
                if ((control != null) && (component != null))
                {
                    int index = control.getLimitCount();
                    if (this.limitCountList.Length > index)
                    {
                        component.SetEvolutionLevel(control.getServantId(), this.limitCountList[index]);
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

