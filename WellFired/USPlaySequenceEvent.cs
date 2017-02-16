namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Play uSequence"), USequencerEvent("Sequence/Play uSequence"), USequencerEventHideDuration]
    public class USPlaySequenceEvent : USEventBase
    {
        public bool restartSequencer;
        public USSequencer sequence;

        public override void FireEvent()
        {
            if (this.sequence == null)
            {
                Debug.LogWarning("No sequence for USPlaySequenceEvent : " + base.name, this);
            }
            else if (!Application.isPlaying)
            {
                Debug.LogWarning("Sequence playback controls are not supported in the editor, but will work in game, just fine.");
            }
            else if (!this.restartSequencer)
            {
                this.sequence.Play();
            }
            else
            {
                this.sequence.RunningTime = 0f;
                this.sequence.Play();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

