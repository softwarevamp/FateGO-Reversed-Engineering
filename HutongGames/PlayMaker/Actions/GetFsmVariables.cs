﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.StateMachine), HutongGames.PlayMaker.Tooltip("Get the values of multiple variables in another FSM and store in variables of the same name in this FSM.")]
    public class GetFsmVariables : FsmStateAction
    {
        private GameObject cachedGO;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM"), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField, HideTypeFilter, UIHint(UIHint.Variable)]
        public FsmVar[] getVariables;
        private PlayMakerFSM sourceFsm;
        private INamedVariable[] sourceVariables;
        private NamedVariable[] targetVariables;

        private void DoGetFsmVariables()
        {
            this.InitFsmVars();
            for (int i = 0; i < this.getVariables.Length; i++)
            {
                this.getVariables[i].GetValueFrom(this.sourceVariables[i]);
                this.getVariables[i].ApplyValueTo(this.targetVariables[i]);
            }
        }

        private void InitFsmVars()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && (ownerDefaultTarget != this.cachedGO))
            {
                this.sourceVariables = new INamedVariable[this.getVariables.Length];
                this.targetVariables = new NamedVariable[this.getVariables.Length];
                for (int i = 0; i < this.getVariables.Length; i++)
                {
                    string variableName = this.getVariables[i].variableName;
                    this.sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                    this.sourceVariables[i] = this.sourceFsm.FsmVariables.GetVariable(variableName);
                    this.targetVariables[i] = base.Fsm.Variables.GetVariable(variableName);
                    this.getVariables[i].Type = FsmUtility.GetVariableType(this.targetVariables[i]);
                    if (!string.IsNullOrEmpty(variableName) && (this.sourceVariables[i] == null))
                    {
                        this.LogWarning("Missing Variable: " + variableName);
                    }
                    this.cachedGO = ownerDefaultTarget;
                }
            }
        }

        public override void OnEnter()
        {
            this.InitFsmVars();
            this.DoGetFsmVariables();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetFsmVariables();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.getVariables = null;
        }
    }
}

