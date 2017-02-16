namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Signal/Send Message"), USequencerEventHideDuration, USequencerFriendlyName("Send Message")]
    public class USSendMessageEvent : USEventBase
    {
        public string action = "OnSignal";
        public GameObject receiver;

        public override void FireEvent()
        {
            if (Application.isPlaying)
            {
                if (this.receiver != null)
                {
                    this.receiver.SendMessage(this.action);
                }
                else
                {
                    Debug.LogWarning($"No receiver of signal "{this.action}" on object {this.receiver.name} ({this.receiver.GetType().Name})", this.receiver);
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

