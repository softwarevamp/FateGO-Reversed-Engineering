namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Gets the number of children that a GameObject has."), ActionCategory(ActionCategory.GameObject)]
    public class GetChildCount : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to test.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Store the number of children in an int variable."), UIHint(UIHint.Variable), RequiredField]
        public FsmInt storeResult;

        private void DoGetChildCount()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this.storeResult.Value = ownerDefaultTarget.transform.childCount;
            }
        }

        public override void OnEnter()
        {
            this.DoGetChildCount();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeResult = null;
        }
    }
}

