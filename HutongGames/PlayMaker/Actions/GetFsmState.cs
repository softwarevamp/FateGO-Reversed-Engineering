namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.StateMachine), HutongGames.PlayMaker.Tooltip("Gets the name of the specified FSMs current state. Either reference the fsm component directly, or find it on a game object.")]
    public class GetFsmState : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. E.g.,  useful if you're waiting for the state to change.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [HutongGames.PlayMaker.Tooltip("Drag a PlayMakerFSM component here.")]
        public PlayMakerFSM fsmComponent;
        [HutongGames.PlayMaker.Tooltip("Optional name of Fsm on Game Object. If left blank it will find the first PlayMakerFSM on the GameObject."), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [HutongGames.PlayMaker.Tooltip("If not specifyng the component above, specify the GameObject that owns the FSM")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), RequiredField, HutongGames.PlayMaker.Tooltip("Store the state name in a string variable.")]
        public FsmString storeResult;

        private void DoGetFsmState()
        {
            if (this.fsm == null)
            {
                if (this.fsmComponent != null)
                {
                    this.fsm = this.fsmComponent;
                }
                else
                {
                    GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                    if (ownerDefaultTarget != null)
                    {
                        this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                    }
                }
                if (this.fsm == null)
                {
                    this.storeResult.Value = string.Empty;
                    return;
                }
            }
            this.storeResult.Value = this.fsm.ActiveStateName;
        }

        public override void OnEnter()
        {
            this.DoGetFsmState();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetFsmState();
        }

        public override void Reset()
        {
            this.fsmComponent = null;
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

