namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Animation), HutongGames.PlayMaker.Tooltip("Rewinds the named animation.")]
    public class RewindAnimation : ComponentAction<Animation>
    {
        [UIHint(UIHint.Animation)]
        public FsmString animName;
        [RequiredField, CheckForComponent(typeof(Animation))]
        public FsmOwnerDefault gameObject;

        private void DoRewindAnimation()
        {
            if (!string.IsNullOrEmpty(this.animName.Value))
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (base.UpdateCache(ownerDefaultTarget))
                {
                    base.animation.Rewind(this.animName.Value);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoRewindAnimation();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
        }
    }
}

