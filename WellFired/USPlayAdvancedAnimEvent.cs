namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Play Advanced Animation (Legacy)"), USequencerEvent("Animation (Legacy)/Play Animation Advanced")]
    public class USPlayAdvancedAnimEvent : USEventBase
    {
        public AnimationClip animationClip;
        public int animationLayer = 1;
        public float animationWeight = 1f;
        public AnimationBlendMode blendMode;
        public bool crossFadeAnimation;
        public float playbackSpeed = 1f;
        public WrapMode wrapMode;

        public override void EndEvent()
        {
            this.StopEvent();
        }

        public override void FireEvent()
        {
            if (this.animationClip == null)
            {
                Debug.Log("Attempting to play an animation on a GameObject but you haven't given the event an animation clip from USPlayAnimEvent::FireEvent");
            }
            else
            {
                Animation component = base.AffectedObject.GetComponent<Animation>();
                if (component == null)
                {
                    Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
                }
                else
                {
                    component.wrapMode = this.wrapMode;
                    if (this.crossFadeAnimation)
                    {
                        component.CrossFade(this.animationClip.name);
                    }
                    else
                    {
                        component.Play(this.animationClip.name);
                    }
                    AnimationState state = component[this.animationClip.name];
                    if (state != null)
                    {
                        state.enabled = true;
                        state.weight = this.animationWeight;
                        state.blendMode = this.blendMode;
                        state.layer = this.animationLayer;
                        state.speed = this.playbackSpeed;
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            Animation component = base.AffectedObject.GetComponent<Animation>();
            if (component == null)
            {
                Debug.LogError(string.Concat(new object[] { "Trying to play an animation : ", this.animationClip.name, " but : ", base.AffectedObject, " doesn't have an animation component, we will add one, this time, though you should add it manually" }));
                component = base.AffectedObject.AddComponent<Animation>();
            }
            if (component[this.animationClip.name] == null)
            {
                Debug.LogError("Trying to play an animation : " + this.animationClip.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
                component.AddClip(this.animationClip, this.animationClip.name);
            }
            AnimationState state = component[this.animationClip.name];
            if (!component.IsPlaying(this.animationClip.name))
            {
                component.wrapMode = this.wrapMode;
                if (this.crossFadeAnimation)
                {
                    component.CrossFade(this.animationClip.name);
                }
                else
                {
                    component.Play(this.animationClip.name);
                }
            }
            state.weight = this.animationWeight;
            state.blendMode = this.blendMode;
            state.layer = this.animationLayer;
            state.speed = this.playbackSpeed;
            state.time = deltaTime * this.playbackSpeed;
            state.enabled = true;
            component.Sample();
        }

        public override void StopEvent()
        {
            if (base.AffectedObject != null)
            {
                Animation component = base.AffectedObject.GetComponent<Animation>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }

        public void Update()
        {
            if ((this.wrapMode != WrapMode.Loop) && (this.animationClip != null))
            {
                base.Duration = this.animationClip.length / this.playbackSpeed;
            }
        }
    }
}

