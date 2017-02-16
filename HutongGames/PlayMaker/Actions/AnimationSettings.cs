namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set the Wrap Mode, Blend Mode, Layer and Speed of an Animation.\nNOTE: Settings are applied once, on entering the state, NOT continuously. To dynamically control an animation's settings, use Set Animation Speede etc."), ActionCategory(ActionCategory.Animation)]
    public class AnimationSettings : FsmStateAction
    {
        [UIHint(UIHint.Animation), HutongGames.PlayMaker.Tooltip("The name of the animation."), RequiredField]
        public FsmString animName;
        [HutongGames.PlayMaker.Tooltip("How the animation is blended with other animations on the Game Object.")]
        public AnimationBlendMode blendMode;
        [HutongGames.PlayMaker.Tooltip("A GameObject with an Animation Component."), RequiredField, CheckForComponent(typeof(Animation))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("The animation layer")]
        public FsmInt layer;
        [HutongGames.PlayMaker.Tooltip("The speed of the animation. 1 = normal; 2 = double speed..."), HasFloatSlider(0f, 5f)]
        public FsmFloat speed;
        [HutongGames.PlayMaker.Tooltip("The behavior of the animation when it wraps.")]
        public WrapMode wrapMode;

        private void DoAnimationSettings()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && !string.IsNullOrEmpty(this.animName.Value))
            {
                Animation component = ownerDefaultTarget.GetComponent<Animation>();
                if (component == null)
                {
                    this.LogWarning("Missing animation component: " + ownerDefaultTarget.name);
                }
                else
                {
                    AnimationState state = component[this.animName.Value];
                    if (state == null)
                    {
                        this.LogWarning("Missing animation: " + this.animName.Value);
                    }
                    else
                    {
                        state.wrapMode = this.wrapMode;
                        state.blendMode = this.blendMode;
                        if (!this.layer.IsNone)
                        {
                            state.layer = this.layer.Value;
                        }
                        if (!this.speed.IsNone)
                        {
                            state.speed = this.speed.Value;
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoAnimationSettings();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
            this.wrapMode = WrapMode.Loop;
            this.blendMode = AnimationBlendMode.Blend;
            this.speed = 1f;
            this.layer = 0;
        }
    }
}

