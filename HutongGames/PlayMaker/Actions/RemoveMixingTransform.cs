namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Removes a mixing transform previously added with Add Mixing Transform. If transform has been added as recursive, then it will be removed as recursive. Once you remove all mixing transforms added to animation state all curves become animated again."), ActionCategory(ActionCategory.Animation)]
    public class RemoveMixingTransform : ComponentAction<Animation>
    {
        [HutongGames.PlayMaker.Tooltip("The name of the animation."), RequiredField]
        public FsmString animationName;
        [RequiredField, CheckForComponent(typeof(Animation)), HutongGames.PlayMaker.Tooltip("The GameObject playing the animation.")]
        public FsmOwnerDefault gameObject;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The mixing transform to remove. E.g., root/upper_body/left_shoulder")]
        public FsmString transfrom;

        private void DoRemoveMixingTransform()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                AnimationState state = base.animation[this.animationName.Value];
                if (state != null)
                {
                    Transform mix = ownerDefaultTarget.transform.Find(this.transfrom.Value);
                    state.AddMixingTransform(mix);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoRemoveMixingTransform();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animationName = string.Empty;
        }
    }
}

