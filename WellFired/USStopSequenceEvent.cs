namespace WellFired
{
    using System;

    [USequencerFriendlyName("stop uSequence"), USequencerEventHideDuration, USequencerEvent("Sequence/Stop uSequence")]
    public class USStopSequenceEvent : USEventBase
    {
        public USSequencer sequence;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USstopSequenceEvent : " + base.name, this);
            }
            if (this.sequence != null)
            {
                this.sequence.Stop();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

