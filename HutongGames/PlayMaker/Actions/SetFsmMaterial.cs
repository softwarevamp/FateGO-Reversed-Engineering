﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set the value of a Material Variable in another FSM."), ActionCategory(ActionCategory.StateMachine)]
    public class SetFsmMaterial : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM."), RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Set the value of the variable.")]
        public FsmMaterial setValue;
        [RequiredField, UIHint(UIHint.FsmMaterial), HutongGames.PlayMaker.Tooltip("The name of the FSM variable.")]
        public FsmString variableName;

        private void DoSetFsmBool()
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
                        FsmMaterial fsmMaterial = this.fsm.FsmVariables.GetFsmMaterial(this.variableName.Value);
                        if (fsmMaterial != null)
                        {
                            fsmMaterial.Value = this.setValue.Value;
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
            this.DoSetFsmBool();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetFsmBool();
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

