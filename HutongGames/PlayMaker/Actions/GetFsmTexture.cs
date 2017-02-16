namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.StateMachine), HutongGames.PlayMaker.Tooltip("Get the value of a Texture Variable from another FSM.")]
    public class GetFsmTexture : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        protected PlayMakerFSM fsm;
        [HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the FSM."), RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmTexture storeValue;
        [UIHint(UIHint.FsmTexture), RequiredField]
        public FsmString variableName;

        private void DoGetFsmVariable()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget != this.goLastFrame)
                {
                    this.goLastFrame = ownerDefaultTarget;
                    this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                }
                if ((this.fsm != null) && (this.storeValue != null))
                {
                    FsmTexture fsmTexture = this.fsm.FsmVariables.GetFsmTexture(this.variableName.Value);
                    if (fsmTexture != null)
                    {
                        this.storeValue.Value = fsmTexture.Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetFsmVariable();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetFsmVariable();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.variableName = string.Empty;
            this.storeValue = null;
            this.everyFrame = false;
        }
    }
}

