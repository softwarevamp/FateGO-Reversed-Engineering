namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Signal/Send Message (Bool)"), USequencerFriendlyName("Send Message (Bool)")]
    public class USSendMessageBoolEvent : USEventBase
    {
        public string action = "OnSignal";
        public GameObject receiver;
        [SerializeField]
        private bool valueToSend;

        public override void FireEvent()
        {
            if (Application.isPlaying)
            {
                if (this.receiver != null)
                {
                    this.receiver.SendMessage(this.action, this.valueToSend);
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

