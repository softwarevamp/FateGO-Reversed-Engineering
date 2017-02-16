namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Play Animation"), USequencerFriendlyName("FGO Play Animation"), USequencerEventHideDuration]
    public class USFGOPlayAnimationEvent : USEventBase
    {
        private BattleActorControl actor;
        public bool AddBattleSide;
        public bool AddLevelNumber;
        public string AnimationName = string.Empty;
        private Animation[] animations;
        private readonly string BattleSideEnemy = "_enemy";
        private readonly string BattleSidePlayer = "_player";
        private changeVColor[] changeVColors;
        private bool originalActive;
        private ParticleSystem[] particles;
        private UVScroll[] uvScrolls;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
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
                            string animationName = this.GetAnimationName(animation.clip.name);
                            animation.Play(animationName);
                        }
                    }
                    foreach (Animation animation2 in this.animations)
                    {
                        string name = this.GetAnimationName(animation2.clip.name);
                        AnimationState state = animation2[name];
                        if (state != null)
                        {
                            if (!animation2.IsPlaying(name))
                            {
                                animation2.Play(name);
                            }
                            state.time = 0f;
                            state.enabled = true;
                            animation2.Sample();
                            state.enabled = false;
                        }
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

        private string GetAnimationName(string defaultName)
        {
            string animationName = this.AnimationName;
            if ((animationName == null) || (animationName.Length == 0))
            {
                animationName = defaultName;
            }
            if ((this.AddLevelNumber || this.AddBattleSide) && ((SingletonMonoBehaviour<BattleSequenceManager>.Instance != null) && (this.actor == null)))
            {
                GameObject actor = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor;
                if (actor != null)
                {
                    this.actor = actor.GetComponent<BattleActorControl>();
                }
            }
            if (this.AddLevelNumber)
            {
                if (SingletonMonoBehaviour<BattleSequenceManager>.Instance != null)
                {
                    if (this.actor != null)
                    {
                        animationName = animationName + "_" + this.actor.BattleSvtData.getLimitImageIndex();
                    }
                }
                else
                {
                    animationName = animationName + "_0";
                }
            }
            if (!this.AddBattleSide)
            {
                return animationName;
            }
            if (SingletonMonoBehaviour<BattleSequenceManager>.Instance != null)
            {
                if (this.actor != null)
                {
                    animationName = animationName + (!this.actor.IsEnemy ? this.BattleSidePlayer : this.BattleSideEnemy);
                }
                return animationName;
            }
            return (animationName + this.BattleSidePlayer);
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    string animationName = this.GetAnimationName(animation.clip.name);
                    AnimationState state = animation[animationName];
                    if (state != null)
                    {
                        if (!animation.IsPlaying(animationName))
                        {
                            animation.Play(animationName);
                        }
                        state.time = deltaTime;
                        state.enabled = true;
                        animation.Sample();
                        state.enabled = false;
                    }
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

