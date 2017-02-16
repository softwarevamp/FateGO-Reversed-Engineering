namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Animation), HutongGames.PlayMaker.Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
    public class StopAnimation : ComponentAction<Animation>
    {
        [UIHint(UIHint.Animation), HutongGames.PlayMaker.Tooltip("Leave empty to stop all playing animations.")]
        public FsmString animName;
        [RequiredField, CheckForComponent(typeof(Animation))]
        public FsmOwnerDefault gameObject;

        private void DoStopAnimation()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                if ((this.animName == null) || string.IsNullOrEmpty(this.animName.Value))
                {
                    base.animation.Stop();
                }
                else
                {
                    base.animation.Stop(this.animName.Value);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoStopAnimation();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
        }
    }
}

