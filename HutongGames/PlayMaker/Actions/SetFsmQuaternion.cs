namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set the value of a Quaternion Variable in another FSM."), ActionCategory(ActionCategory.StateMachine)]
    public class SetFsmQuaternion : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [HutongGames.PlayMaker.Tooltip("Set the value of the variable."), RequiredField]
        public FsmQuaternion setValue;
        [RequiredField, UIHint(UIHint.FsmQuaternion), HutongGames.PlayMaker.Tooltip("The name of the FSM variable.")]
        public FsmString variableName;

        private void DoSetFsmQuaternion()
        {
            if (this.setValue != null)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    if (ownerDefaultTarget != this.goLastFrame)
                    {
                        this.goLastFrame = ownerDefaultTarget;
                        this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                    }
                    if (this.fsm == null)
                    {
                        this.LogWarning("Could not find FSM: " + this.fsmName.Value);
                    }
                    else
                    {
                        FsmQuaternion fsmQuaternion = this.fsm.FsmVariables.GetFsmQuaternion(this.variableName.Value);
                        if (fsmQuaternion != null)
                        {
                            fsmQuaternion.Value = this.setValue.Value;
                        }
                        else
                        {
                            this.LogWarning("Could not find variable: " + this.variableName.Value);
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetFsmQuaternion();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetFsmQuaternion();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.variableName = string.Empty;
            this.setValue = null;
            this.everyFrame = false;
        }
    }
}

