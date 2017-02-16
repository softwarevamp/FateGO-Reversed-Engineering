namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("Audio/Fade Audio"), USequencerFriendlyName("Fade Audio")]
    public class USFadeAudioEvent : USEventBase
    {
        public AnimationCurve fadeCurve;
        private float previousVolume = 1f;

        public USFadeAudioEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
        }

        public override void FireEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
            }
            else
            {
                this.previousVolume = component.volume;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
            }
            else
            {
                component.volume = this.fadeCurve.Evaluate(deltaTime);
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                Debug.LogWarning("Trying to fade audio on an object without an AudioSource");
            }
            else
            {
                component.volume = this.previousVolume;
            }
        }

        public void Update()
        {
            base.Duration = this.fadeCurve.length;
        }
    }
}

