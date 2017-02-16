namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Play Effect"), USequencerEventHideDuration, USequencerEvent("FGO/Play Effect")]
    public class USFGOPlayEffectEvent : USEventBase
    {
        private Animation[] animations;
        private changeVColor[] changeVColors;
        private bool originalActive;
        private ParticleSystem[] particles;
        private UVScroll[] uvScrolls;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                string effectAudio = this.GetEffectAudio(base.AffectedObject.name);
                if (effectAudio != null)
                {
                    SoundManager.playSe("Battle", effectAudio, 1f, null);
                }
                this.originalActive = base.AffectedObject.activeSelf;
                base.AffectedObject.SetActive(true);
                this.animations = base.AffectedObject.GetComponentsInChildren<Animation>(true);
                if (!Application.isPlaying)
                {
                    this.particles = base.AffectedObject.GetComponentsInChildren<ParticleSystem>(true);
                    this.uvScrolls = base.AffectedObject.GetComponentsInChildren<UVScroll>(true);
                    this.changeVColors = base.AffectedObject.GetComponentsInChildren<changeVColor>(true);
                }
                if (this.animations != null)
                {
                    foreach (Animation animation in this.animations)
                    {
                        if (animation != null)
                        {
                            animation.Play(animation.clip.name);
                        }
                    }
                    foreach (Animation animation2 in this.animations)
                    {
                        AnimationState state = animation2[animation2.clip.name];
                        if (!animation2.IsPlaying(animation2.clip.name))
                        {
                            animation2.Play(animation2.clip.name);
                        }
                        state.time = 0f;
                        state.enabled = true;
                        animation2.Sample();
                        state.enabled = false;
                    }
                }
                this.uvScrolls = base.AffectedObject.GetComponentsInChildren<UVScroll>(true);
                foreach (UVScroll scroll in this.uvScrolls)
                {
                    scroll.UpdateUV();
                }
                if (Application.isPlaying && (this.particles != null))
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (system != null)
                        {
                            system.Play();
                        }
                    }
                }
            }
        }

        protected string GetEffectAudio(string name)
        {
            if ((name != null) && name.StartsWith("Start_nf"))
            {
                return "NP_START_1";
            }
            return null;
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    AnimationState state = animation[animation.clip.name];
                    if (!animation.IsPlaying(animation.clip.name))
                    {
                        animation.Play(animation.clip.name);
                    }
                    state.time = deltaTime;
                    state.enabled = true;
                    animation.Sample();
                    state.enabled = false;
                }
            }
            if (!Application.isPlaying)
            {
                FlipEffectUpdater component = base.AffectedObject.transform.parent.GetComponent<FlipEffectUpdater>();
                if (component != null)
                {
                    component.OnLateUpdate();
                }
                if (this.particles != null)
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        system.Simulate(deltaTime);
                    }
                }
                if (this.uvScrolls != null)
                {
                    foreach (UVScroll scroll in this.uvScrolls)
                    {
                        scroll.UpdateUV();
                    }
                }
                if (this.changeVColors != null)
                {
                    foreach (changeVColor color in this.changeVColors)
                    {
                        color.UpdateVColor();
                    }
                }
            }
        }

        public override void StopEvent()
        {
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    if (animation != null)
                    {
                        animation.Stop();
                    }
                }
            }
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                base.AffectedObject.SetActive(this.originalActive);
                if (this.particles != null)
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (system != null)
                        {
                            system.Stop();
                        }
                    }
                }
            }
        }

        public void Update()
        {
            if (base.AffectedObject != null)
            {
                float length = 0f;
                if (this.animations != null)
                {
                    foreach (Animation animation in this.animations)
                    {
                        if (((animation != null) && (animation.clip != null)) && (length < animation.clip.length))
                        {
                            length = animation.clip.length;
                        }
                    }
                }
                if (this.particles != null)
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (system != null)
                        {
                            float num4 = system.duration + system.startLifetime;
                            if (length < num4)
                            {
                                length = num4;
                            }
                        }
                    }
                }
                base.Duration = length;
            }
        }
    }
}

