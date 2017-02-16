namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Tests if a Game Object's Rigid Body is Kinematic.")]
    public class IsKinematic : ComponentAction<Rigidbody>
    {
        public bool everyFrame;
        public FsmEvent falseEvent;
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable)]
        public FsmBool store;
        public FsmEvent trueEvent;

        private void DoIsKinematic()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                bool isKinematic = base.rigidbody.isKinematic;
                this.store.Value = isKinematic;
                base.Fsm.Event(!isKinematic ? this.falseEvent : this.trueEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoIsKinematic();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoIsKinematic();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.store = null;
            this.everyFrame = false;
        }
    }
}

