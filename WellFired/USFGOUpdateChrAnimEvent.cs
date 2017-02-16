namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Update Character Animation"), USequencerEvent("FGO/Character/Update Character Animation")]
    public class USFGOUpdateChrAnimEvent : USEventBase
    {
        private Animation anim;
        public string animationName = string.Empty;
        public float playbackSpeed = 1f;
        public string startEvent = string.Empty;
        public float startTime;
        public string stopEvent = string.Empty;
        public float stopTime;

        public override void FireEvent()
        {
            if (this.animationName == string.Empty)
            {
                Debug.Log("animationName is null from USFGOChrPlayAnimEvent::FireEvent");
            }
            else
            {
                BattleFBXComponent component = base.AffectedObject.GetComponent<BattleFBXComponent>();
                if (component == null)
                {
                    Debug.Log("BattleFBXComponent not found from USFGOChrPlayAnimEvent.FireEvent");
                }
                else
                {
                    string currentAnimName = component.CurrentAnimName;
                    float currentAnimTime = component.CurrentAnimTime;
                    bool flag = currentAnimName != null;
                    component.playAnimationTimeline(this.animationName, this.startEvent, this.stopEvent);
                    if ((this.startEvent != null) && (this.startEvent.Length > 0))
                    {
                        this.startTime = component.getTagTime(this.animationName, this.startEvent);
                    }
                    if ((this.stopEvent != null) && (this.stopEvent.Length > 0))
                    {
                        this.stopTime = component.getTagTime(this.animationName, this.stopEvent);
                    }
                    if (this.stopTime == 0f)
                    {
                        this.stopTime = component.getLength();
                    }
                    else if (this.stopTime > component.getLength())
                    {
                        this.stopTime = component.getLength();
                    }
                    Animation animation = base.AffectedObject.GetComponent<Animation>();
                    if (animation == null)
                    {
                        animation = component.RootTransform.GetComponent<Animation>();
                    }
                    int num2 = (int) (component.CurrentAnimTime / 0.03333334f);
                    animation[this.animationName].time = component.CurrentAnimTime;
                    AnimationState state = animation[this.animationName];
                    state.enabled = true;
                    animation.Sample();
                    state.enabled = false;
                    base.Duration = 0.1f;
                    if (flag)
                    {
                        component.playAnimationTimeline(currentAnimName, string.Empty, string.Empty);
                        component.AnimUpdate(currentAnimTime);
                        animation[currentAnimName].time = component.CurrentAnimTime;
                        state = animation[currentAnimName];
                        state.enabled = true;
                        animation.Sample();
                        state.enabled = false;
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
        }

        public void Update()
        {
        }
    }
}

