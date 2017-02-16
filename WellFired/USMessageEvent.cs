namespace WellFired
{
    using System;

    [USequencerFriendlyName("Debug Message"), USequencerEvent("Debug/Log Message"), USequencerEventHideDuration]
    public class USMessageEvent : USEventBase
    {
        public string message = "Default Message";

        public override void FireEvent()
        {
            Debug.Log(this.message);
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

