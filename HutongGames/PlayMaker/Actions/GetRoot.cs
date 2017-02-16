namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Gets the top most parent of the Game Object.\nIf the game object has no parent, returns itself.")]
    public class GetRoot : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject storeRoot;

        private void DoGetRoot()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this.storeRoot.Value = ownerDefaultTarget.transform.root.gameObject;
            }
        }

        public override void OnEnter()
        {
            this.DoGetRoot();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeRoot = null;
        }
    }
}

