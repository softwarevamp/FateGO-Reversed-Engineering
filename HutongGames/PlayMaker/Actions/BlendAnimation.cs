namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Blends an Animation towards a Target Weight over a specified Time.\nOptionally sends an Event when finished."), ActionCategory(ActionCategory.Animation)]
    public class BlendAnimation : FsmStateAction
    {
        [UIHint(UIHint.Animation), HutongGames.PlayMaker.Tooltip("The name of the animation to blend."), RequiredField]
        public FsmString animName;
        private DelayedEvent delayedFinishEvent;
        [HutongGames.PlayMaker.Tooltip("Event to send when the blend has finished.")]
        public FsmEvent finishEvent;
        [RequiredField, CheckForComponent(typeof(Animation)), HutongGames.PlayMaker.Tooltip("The GameObject to animate.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Target weight to blend to."), RequiredField, HasFloatSlider(0f, 1f)]
        public FsmFloat targetWeight;
        [RequiredField, HutongGames.PlayMaker.Tooltip("How long should the blend take."), HasFloatSlider(0f, 5f)]
        public FsmFloat time;

        private void DoBlendAnimation(GameObject go)
        {
            if (go != null)
            {
                Animation component = go.GetComponent<Animation>();
                if (component == null)
                {
                    this.LogWarning("Missing Animation component on GameObject: " + go.name);
                    base.Finish();
                }
                else
                {
                    AnimationState state = component[this.animName.Value];
                    if (state == null)
                    {
                        this.LogWarning("Missing animation: " + this.animName.Value);
                        base.Finish();
                    }
                    else
                    {
                        float fadeLength = this.time.Value;
                        component.Blend(this.animName.Value, this.targetWeight.Value, fadeLength);
                        if (this.finishEvent != null)
                        {
                            this.delayedFinishEvent = base.Fsm.DelayedEvent(this.finishEvent, state.length);
                        }
                        else
                        {
                            base.Finish();
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoBlendAnimation((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
        }

        public override void OnUpdate()
        {
            if (DelayedEvent.WasSent(this.delayedFinishEvent))
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
            this.targetWeight = 1f;
            this.time = 0.3f;
            this.finishEvent = null;
        }
    }
}

