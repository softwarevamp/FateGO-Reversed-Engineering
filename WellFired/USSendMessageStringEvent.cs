namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Send Message (String)"), USequencerEvent("Signal/Send Message (String)"), USequencerEventHideDuration]
    public class USSendMessageStringEvent : USEventBase
    {
        public string action = "OnSignal";
        public GameObject receiver;
        [SerializeField]
        private string valueToSend;

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

