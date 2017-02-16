namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Animation (Legacy)/Blend Animation No Scrub"), USequencerFriendlyName("Blend Animation No Scrub (Legacy)"), USequencerEventHideDuration]
    public class USBlendAnimNoScrubEvent : USEventBase
    {
        public AnimationClip blendedAnimation;

        public override void FireEvent()
        {
            Animation component = base.AffectedObject.GetComponent<Animation>();
            if (component == null)
            {
                Debug.Log("Attempting to play an animation on a GameObject without an Animation Component from USPlayAnimEvent.FireEvent");
            }
            else
            {
                component[this.blendedAnimation.name].wrapMode = WrapMode.Once;
                component[this.blendedAnimation.name].layer = 1;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            base.GetComponent<Animation>().CrossFade(this.blendedAnimation.name);
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
                base.Duration = this.blendedAnimation.length;
            }
        }
    }
}

