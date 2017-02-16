namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets a Game Object's Name."), ActionCategory(ActionCategory.GameObject)]
    public class SetName : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmString name;

        private void DoSetLayer()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                ownerDefaultTarget.name = this.name.Value;
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
            this.name = null;
        }
    }
}

