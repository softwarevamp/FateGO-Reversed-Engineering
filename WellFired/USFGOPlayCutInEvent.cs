namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/CutIn/Play CutIn(Obsolete?)"), USequencerFriendlyName("FGO Play CutIn")]
    public class USFGOPlayCutInEvent : USEventBase
    {
        private Animation[] animations;
        private changeVColor[] changeVColors;
        public string cutInName;
        public GameObject cutInObject;
        private bool originalActive;
        private ParticleSystem[] particles;
        public float playbackSpeed = 1f;
        private UVScroll[] uvScrolls;

        public override void FireEvent()
        {
            if (this.cutInObject != null)
            {
                this.originalActive = this.cutInObject.activeSelf;
                this.cutInObject.SetActive(true);
                this.animations = this.cutInObject.GetComponentsInChildren<Animation>(true);
                if (!Application.isPlaying)
                {
                    this.particles = this.cutInObject.GetComponentsInChildren<ParticleSystem>(true);
                    this.uvScrolls = this.cutInObject.GetComponentsInChildren<UVScroll>(true);
                    this.changeVColors = this.cutInObject.GetComponentsInChildren<changeVColor>(true);
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
            if (this.cutInObject != null)
            {
                this.cutInObject.SetActive(this.originalActive);
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
            if (this.cutInObject != null)
            {
                float length = 0f;
                if (this.animations != null)
                {
                    foreach (Animation animation in this.animations)
                    {
                        if ((animation != null) && (length < animation.clip.length))
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

