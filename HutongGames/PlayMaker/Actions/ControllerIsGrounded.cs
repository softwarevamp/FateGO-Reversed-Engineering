namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Character), HutongGames.PlayMaker.Tooltip("Tests if a Character Controller on a Game Object was touching the ground during the last move.")]
    public class ControllerIsGrounded : FsmStateAction
    {
        private CharacterController controller;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Event to send if not touching the ground.")]
        public FsmEvent falseEvent;
        [HutongGames.PlayMaker.Tooltip("The GameObject to check."), RequiredField, CheckForComponent(typeof(CharacterController))]
        public FsmOwnerDefault gameObject;
        private GameObject previousGo;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Sore the result in a bool variable.")]
        public FsmBool storeResult;
        [HutongGames.PlayMaker.Tooltip("Event to send if touching the ground.")]
        public FsmEvent trueEvent;

        private void DoControllerIsGrounded()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget != this.previousGo)
                {
                    this.controller = ownerDefaultTarget.GetComponent<CharacterController>();
                    this.previousGo = ownerDefaultTarget;
                }
                if (this.controller != null)
                {
                    bool isGrounded = this.controller.isGrounded;
                    this.storeResult.Value = isGrounded;
                    base.Fsm.Event(!isGrounded ? this.falseEvent : this.trueEvent);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoControllerIsGrounded();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoControllerIsGrounded();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

