namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Sequence/Stop And Skip"), USequencerFriendlyName("Stop and Skip sequencer")]
    public class USStopAndSkipToTimeSequenceEvent : USEventBase
    {
        [SerializeField]
        private USSequencer sequence;
        [SerializeField]
        private float timeToSkipTo;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USstopSequenceEvent : " + base.name, this);
            }
            if (this.sequence != null)
            {
                this.sequence.Stop();
                this.sequence.SkipTimelineTo(this.timeToSkipTo);
                this.sequence.UpdateSequencer(0f);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

