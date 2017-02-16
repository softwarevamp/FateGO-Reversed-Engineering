namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [USequencerFriendlyName("FGO Play Target Animation"), USequencerEvent("FGO/Character/Play Target Animation")]
    public class USFGOPlayTargetAnimationEvent : USEventBase
    {
        public string animationName = string.Empty;
        public float playbackSpeed = 1f;
        public string startEvent = string.Empty;
        public float startTime;
        public string stopEvent = string.Empty;
        public float stopTime;
        public USFGOCreateEffectEvent.EffectTarget target = USFGOCreateEffectEvent.EffectTarget.Target;
        protected List<GameObject> Targets;
        public WrapMode wrapMode = WrapMode.Once;

        public override void FireEvent()
        {
            if (this.animationName == string.Empty)
            {
                Debug.Log("animationName is null from USFGOChrPlayAnimEvent::FireEvent");
            }
            else
            {
                if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                {
                    BattleActorControl control = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                    if ((control != null) && control.IsEnemy)
                    {
                        switch (this.target)
                        {
                            case USFGOCreateEffectEvent.EffectTarget.PlayerParty:
                                this.target = USFGOCreateEffectEvent.EffectTarget.EnemyParty;
                                break;

                            case USFGOCreateEffectEvent.EffectTarget.EnemyParty:
                                this.target = USFGOCreateEffectEvent.EffectTarget.PlayerParty;
                                break;
                        }
                    }
                }
                this.Targets = USFGOCreateEffectEvent.getTargets(this.target, -1);
                foreach (GameObject obj2 in this.Targets)
                {
                    BattleFBXComponent component = obj2.GetComponent<BattleFBXComponent>();
                    if (component != null)
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
                            base.Duration = component.getLength();
                        }
                        component.SetWrapMode(this.animationName, this.wrapMode);
                    }
                }
                if ((this.wrapMode != WrapMode.Loop) && (this.Targets != null))
                {
                    float num = 0f;
                    foreach (GameObject obj3 in this.Targets)
                    {
                        BattleFBXComponent component2 = obj3.GetComponent<BattleFBXComponent>();
                        if ((component2 != null) && ((component2 != null) && (num < (this.stopTime - this.startTime))))
                        {
                            num = this.stopTime - this.startTime;
                        }
                    }
                    base.Duration = num;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (this.animationName != string.Empty)
            {
                foreach (GameObject obj2 in this.Targets)
                {
                    BattleFBXComponent component = obj2.GetComponent<BattleFBXComponent>();
                    if (component != null)
                    {
                        Animation animation = obj2.GetComponent<Animation>();
                        if (animation == null)
                        {
                            animation = component.RootTransform.GetComponent<Animation>();
                        }
                        if ((component == null) || (animation == null))
                        {
                            Debug.LogError(string.Concat(new object[] { "Trying to play an animation : ", this.animationName, " but : ", obj2, " doesn't have an BattleFBXComponent, we will add one, this time, though you should add it manually" }));
                            break;
                        }
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
        }

        public override void StopEvent()
        {
        }

        public void Update()
        {
        }
    }
}

