namespace WellFired
{
    using System;

    [USequencerEventHideDuration, USequencerEvent("Object/Toggle Object"), USequencerFriendlyName("Toggle Object")]
    public class USEnableObjectEvent : USEventBase
    {
        public bool enable;
        private bool prevEnable;

        public override void FireEvent()
        {
            this.prevEnable = base.AffectedObject.activeSelf;
            base.AffectedObject.SetActive(this.enable);
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
            if (base.AffectedObject != null)
            {
                base.AffectedObject.SetActive(this.prevEnable);
            }
        }
    }
}

