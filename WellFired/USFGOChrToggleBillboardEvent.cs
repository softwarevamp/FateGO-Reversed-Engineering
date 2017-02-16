namespace WellFired
{
    using System;

    [USequencerFriendlyName("FGO Toggle Character Billboard"), USequencerEvent("FGO/Character/Toggle Billboard"), USequencerEventHideDuration]
    public class USFGOChrToggleBillboardEvent : USEventBase
    {
        public bool isEnabled;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                BillBoard component = base.AffectedObject.GetComponent<BillBoard>();
                if (component != null)
                {
                    component.enabled = this.isEnabled;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

