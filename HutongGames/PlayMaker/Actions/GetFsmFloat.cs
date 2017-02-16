﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the value of a Float Variable from another FSM."), ActionCategory(ActionCategory.StateMachine)]
    public class GetFsmFloat : FsmStateAction
    {
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [UIHint(UIHint.FsmName), HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
        [RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmFloat storeValue;
        [RequiredField, UIHint(UIHint.FsmFloat)]
        public FsmString variableName;

        private void DoGetFsmFloat()
        {
            if (!this.storeValue.IsNone)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    if (ownerDefaultTarget != this.goLastFrame)
                    {
                        this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                        this.goLastFrame = ownerDefaultTarget;
                    }
                    if (this.fsm != null)
                    {
                        FsmFloat fsmFloat = this.fsm.FsmVariables.GetFsmFloat(this.variableName.Value);
                        if (fsmFloat != null)
                        {
                            this.storeValue.Value = fsmFloat.Value;
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetFsmFloat();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetFsmFloat();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.storeValue = null;
        }
    }
}

