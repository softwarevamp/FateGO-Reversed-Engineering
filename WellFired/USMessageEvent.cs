namespace WellFired
{
    using System;

    [USequencerEventHideDuration, USequencerEvent("Debug/Log Message"), USequencerFriendlyName("Debug Message")]
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

