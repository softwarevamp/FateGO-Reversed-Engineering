namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Tests if a GameObject is a Child of another GameObject."), ActionCategory(ActionCategory.Logic)]
    public class GameObjectIsChildOf : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Event to send if GameObject is NOT a child.")]
        public FsmEvent falseEvent;
        [RequiredField, HutongGames.PlayMaker.Tooltip("GameObject to test.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Is it a child of this GameObject?"), RequiredField]
        public FsmGameObject isChildOf;
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store result in a bool variable")]
        public FsmBool storeResult;
        [HutongGames.PlayMaker.Tooltip("Event to send if GameObject is a child.")]
        public FsmEvent trueEvent;

        private void DoIsChildOf(GameObject go)
        {
            if ((go != null) && (this.isChildOf != null))
            {
                bool flag = go.transform.IsChildOf(this.isChildOf.Value.transform);
                this.storeResult.Value = flag;
                base.Fsm.Event(!flag ? this.falseEvent : this.trueEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoIsChildOf(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.isChildOf = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.storeResult = null;
        }
    }
}

