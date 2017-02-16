namespace WellFired
{
    using System;

    [USequencerEvent("Sequence/Set Playback Rate"), USequencerEventHideDuration, USequencerFriendlyName("Set uSequence Playback Rate")]
    public class USSetPlaybackRateEvent : USEventBase
    {
        public float playbackRate = 1f;
        private float prevPlaybackRate = 1f;
        public USSequencer sequence;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USSetPlaybackRate : " + base.name, this);
            }
            if (this.sequence != null)
            {
                this.prevPlaybackRate = this.sequence.PlaybackRate;
                this.sequence.PlaybackRate = this.playbackRate;
            }
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
            if (this.sequence != null)
            {
                this.sequence.PlaybackRate = this.prevPlaybackRate;
            }
        }
    }
}

