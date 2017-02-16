namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Sets a Game Object's Layer.")]
    public class SetLayer : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Layer)]
        public int layer;

        private void DoSetLayer()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                ownerDefaultTarget.layer = this.layer;
            }
        }

        public override void OnEnter()
        {
            this.DoSetLayer();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.layer = 0;
        }
    }
}

