namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Play Audio"), USequencerEvent("Audio/Play Audio"), USequencerEventHideDuration]
    public class USPlayAudioEvent : USEventBase
    {
        public AudioClip audioClip;
        public bool loop;
        private bool wasPlaying;

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                component = base.AffectedObject.AddComponent<AudioSource>();
                component.playOnAwake = false;
            }
            if (component.clip != this.audioClip)
            {
                component.clip = this.audioClip;
            }
            component.time = 0f;
            component.loop = this.loop;
            if (base.Sequence.IsPlaying)
            {
                component.Play();
            }
        }

        public override void ManuallySetTime(float deltaTime)
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component != null)
            {
                component.time = deltaTime;
            }
        }

        public override void PauseEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            this.wasPlaying = false;
            if ((component != null) && component.isPlaying)
            {
                this.wasPlaying = true;
            }
            if (component != null)
            {
                component.Pause();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                component = base.AffectedObject.AddComponent<AudioSource>();
                component.playOnAwake = false;
            }
            if (component.clip != this.audioClip)
            {
                component.clip = this.audioClip;
            }
            if (!component.isPlaying)
            {
                component.time = deltaTime;
                if (base.Sequence.IsPlaying && !component.isPlaying)
                {
                    component.Play();
                }
            }
        }

        public override void ResumeEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component != null)
            {
                component.time = base.Sequence.RunningTime - base.FireTime;
                if (this.wasPlaying)
                {
                    component.Play();
                }
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }

        public void Update()
        {
            if (!this.loop && (this.audioClip != null))
            {
                base.Duration = this.audioClip.length;
            }
            else
            {
                base.Duration = -1f;
            }
        }
    }
}

