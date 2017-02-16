namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Set Playback Speed"), USequencerEvent("Animation (Mecanim)/Animator/Set Playback Speed"), USequencerEventHideDuration]
    public class USSetAnimatorPlaybackSpeed : USEventBase
    {
        public float playbackSpeed = 1f;
        private float prevPlaybackSpeed;

        public override void FireEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component == null)
            {
                Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
            }
            else
            {
                this.prevPlaybackSpeed = component.speed;
                component.speed = this.playbackSpeed;
            }
        }

        public override void ProcessEvent(float runningTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            Animator component = base.AffectedObject.GetComponent<Animator>();
            if (component != null)
            {
                component.speed = this.prevPlaybackSpeed;
            }
        }
    }
}

