namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Unparents all children from the Game Object.")]
    public class DetachChildren : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("GameObject to unparent children from.")]
        public FsmOwnerDefault gameObject;

        private static void DoDetachChildren(GameObject go)
        {
            if (go != null)
            {
                go.transform.DetachChildren();
            }
        }

        public override void OnEnter()
        {
            DoDetachChildren(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

