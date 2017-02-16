namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Character/Play Character Animation"), USequencerFriendlyName("FGO Play Character Animation")]
    public class USFGOChrPlayAnimEvent : USEventBase
    {
        public string animationName = string.Empty;
        public float playbackSpeed = 1f;
        public string startEvent = string.Empty;
        public float startTime;
        public string stopEvent = string.Empty;
        public float stopTime;
        public WrapMode wrapMode = WrapMode.Once;

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
                    else if ((this.wrapMode != WrapMode.Loop) && (this.stopTime > component.getLength()))
                    {
                        this.stopTime = component.getLength();
                    }
                    if (this.wrapMode != WrapMode.Loop)
                    {
                        base.Duration = this.stopTime - this.startTime;
                    }
                    component.SetWrapMode(this.animationName, this.wrapMode);
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if ((this.wrapMode != WrapMode.Loop) && (deltaTime > base.Duration))
            {
                deltaTime = base.Duration;
            }
            if (this.animationName != string.Empty)
            {
                BattleFBXComponent component = base.AffectedObject.GetComponent<BattleFBXComponent>();
                Animation animation = base.AffectedObject.GetComponent<Animation>();
                if (animation == null)
                {
                    animation = component.RootTransform.GetComponent<Animation>();
                }
                if ((component == null) || (animation == null))
                {
                    Debug.LogError(string.Concat(new object[] { "Trying to play an animation : ", this.animationName, " but : ", base.AffectedObject, " doesn't have an BattleFBXComponent, we will add one, this time, though you should add it manually" }));
                }
                else
                {
                    component.playAnimationTimeline(this.animationName, string.Empty, string.Empty);
                    component.AnimUpdate(this.startTime + deltaTime);
                    int num = (int) (component.CurrentAnimTime / 0.03333334f);
                    animation[this.animationName].time = ((float) num) / 30f;
                    AnimationState state = animation[this.animationName];
                    state.enabled = true;
                    animation.Sample();
                    state.enabled = false;
                }
            }
        }

        public override void StopEvent()
        {
            if (base.AffectedObject == null)
            {
            }
        }

        public void Update()
        {
            if (((this.wrapMode != WrapMode.Loop) && (base.AffectedObject != null)) && (base.AffectedObject.GetComponent<BattleFBXComponent>() != null))
            {
                base.Duration = this.stopTime - this.startTime;
            }
        }
    }
}

