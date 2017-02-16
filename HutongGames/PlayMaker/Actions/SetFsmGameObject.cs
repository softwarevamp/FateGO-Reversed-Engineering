namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference"), ActionCategory(ActionCategory.StateMachine)]
    public class SetFsmGameObject : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [HutongGames.PlayMaker.Tooltip("Set the value of the variable.")]
        public FsmGameObject setValue;
        [RequiredField, UIHint(UIHint.FsmGameObject), HutongGames.PlayMaker.Tooltip("The name of the FSM variable.")]
        public FsmString variableName;

        private void DoSetFsmGameObject()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget != this.goLastFrame)
                {
                    this.goLastFrame = ownerDefaultTarget;
                    this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                }
                if (this.fsm != null)
                {
                    FsmGameObject obj3 = this.fsm.FsmVariables.FindFsmGameObject(this.variableName.Value);
                    if (obj3 != null)
                    {
                        obj3.Value = this.setValue?.Value;
                    }
                    else
                    {
                        this.LogWarning("Could not find variable: " + this.variableName.Value);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetFsmGameObject();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetFsmGameObject();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.setValue = null;
            this.everyFrame = false;
        }
    }
}

