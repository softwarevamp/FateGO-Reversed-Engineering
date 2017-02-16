namespace WellFired
{
    using System;

    [USequencerEventHideDuration, USequencerFriendlyName("Pause uSequence"), USequencerEvent("Sequence/Pause uSequence")]
    public class USPauseSequenceEvent : USEventBase
    {
        public USSequencer sequence;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USPauseSequenceEvent : " + base.name, this);
            }
            if (this.sequence != null)
            {
                this.sequence.Pause();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

