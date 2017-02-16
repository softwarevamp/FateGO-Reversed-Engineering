namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Animation (Legacy)/Blend Animation"), USequencerFriendlyName("Blend Animation (Legacy)")]
    public class USBlendAnimEvent : USEventBase
    {
        public AnimationClip animationClipDest;
        public AnimationClip animationClipSource;
        public float blendPoint = 1f;

        public override void FireEvent()
        {
            Animation component = base.AffectedObject.GetComponent<Animation>();
            if (component == null)
            {
                Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
            }
            else
            {
                component.wrapMode = WrapMode.Loop;
                component.Play(this.animationClipSource.name);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            Animation component = base.AffectedObject.GetComponent<Animation>();
            if (component == null)
            {
                Debug.LogError(string.Concat(new object[] { "Trying to play an animation : ", this.animationClipSource.name, " but : ", base.AffectedObject, " doesn't have an animation component, we will add one, this time, though you should add it manually" }));
                component = base.AffectedObject.AddComponent<Animation>();
            }
            if (component[this.animationClipSource.name] == null)
            {
                Debug.LogError("Trying to play an animation : " + this.animationClipSource.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
                component.AddClip(this.animationClipSource, this.animationClipSource.name);
            }
            if (component[this.animationClipDest.name] == null)
            {
                Debug.LogError("Trying to play an animation : " + this.animationClipDest.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
                component.AddClip(this.animationClipDest, this.animationClipDest.name);
            }
            if (deltaTime < this.blendPoint)
            {
                component.CrossFade(this.animationClipSource.name);
            }
            else
            {
                component.CrossFade(this.animationClipDest.name);
            }
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
            if (base.Duration < 0f)
            {
                base.Duration = 2f;
            }
        }
    }
}

