namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Play an animation on a subset of the hierarchy. E.g., A waving animation on the upper body."), ActionCategory(ActionCategory.Animation)]
    public class AddMixingTransform : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The name of the animation to mix. NOTE: The animation should already be added to the Animation Component on the GameObject.")]
        public FsmString animationName;
        [HutongGames.PlayMaker.Tooltip("The GameObject playing the animation."), CheckForComponent(typeof(Animation)), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("If recursive is true all children of the mix transform will also be animated.")]
        public FsmBool recursive;
        [HutongGames.PlayMaker.Tooltip("The mixing transform. E.g., root/upper_body/left_shoulder"), RequiredField]
        public FsmString transform;

        private void DoAddMixingTransform()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Animation component = ownerDefaultTarget.GetComponent<Animation>();
                if (component != null)
                {
                    AnimationState state = component[this.animationName.Value];
                    if (state != null)
                    {
                        Transform mix = ownerDefaultTarget.transform.Find(this.transform.Value);
                        state.AddMixingTransform(mix, this.recursive.Value);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoAddMixingTransform();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animationName = string.Empty;
            this.transform = string.Empty;
            this.recursive = 1;
        }
    }
}

