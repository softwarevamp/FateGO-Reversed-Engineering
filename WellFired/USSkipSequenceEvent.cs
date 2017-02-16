namespace WellFired
{
    using System;

    [USequencerFriendlyName("Skip uSequence"), USequencerEvent("Sequence/Skip uSequence"), USequencerEventHideDuration]
    public class USSkipSequenceEvent : USEventBase
    {
        public USSequencer sequence;
        public bool skipToEnd = true;
        public float skipToTime = -1f;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USSkipSequenceEvent : " + base.name, this);
            }
            else if ((!this.skipToEnd && (this.skipToTime < 0f)) && (this.skipToTime > this.sequence.Duration))
            {
                Debug.LogWarning("You haven't set the properties correctly on the Sequence for this USSkipSequenceEvent, either the skipToTime is invalid, or you haven't flagged it to skip to the end", this);
            }
            else if (this.skipToEnd)
            {
                this.sequence.SkipTimelineTo(this.sequence.Duration);
            }
            else
            {
                this.sequence.SkipTimelineTo(this.skipToTime);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

