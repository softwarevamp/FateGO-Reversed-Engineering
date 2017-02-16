namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Particle System/Start Emitter"), USequencerFriendlyName("Start Emitter (Legacy)"), USequencerEventHideDuration]
    public class USParticleEmitterStartEvent : USEventBase
    {
        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
                if (component == null)
                {
                    Debug.Log("Attempting to emit particles, but the object has no particleSystem USParticleEmitterStartEvent::FireEvent");
                }
                else if (Application.isPlaying)
                {
                    component.Play();
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (!Application.isPlaying)
            {
                base.AffectedObject.GetComponent<ParticleSystem>().Simulate(deltaTime);
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
                ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }

        public void Update()
        {
            if (base.AffectedObject != null)
            {
                ParticleSystem component = base.AffectedObject.GetComponent<ParticleSystem>();
                if (component != null)
                {
                    base.Duration = component.duration + component.startLifetime;
                }
            }
        }
    }
}

