namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Character), HutongGames.PlayMaker.Tooltip("Moves a Game Object with a Character Controller. Velocity along the y-axis is ignored. Speed is in meters/s. Gravity is automatically applied.")]
    public class ControllerSimpleMove : FsmStateAction
    {
        private CharacterController controller;
        [CheckForComponent(typeof(CharacterController)), HutongGames.PlayMaker.Tooltip("The GameObject to move."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("The movement vector."), RequiredField]
        public FsmVector3 moveVector;
        private GameObject previousGo;
        [HutongGames.PlayMaker.Tooltip("Move in local or word space.")]
        public Space space;
        [HutongGames.PlayMaker.Tooltip("Multiply the movement vector by a speed factor.")]
        public FsmFloat speed;

        public override void OnUpdate()
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
                    Vector3 vector = (this.space != Space.World) ? ownerDefaultTarget.transform.TransformDirection(this.moveVector.Value) : this.moveVector.Value;
                    this.controller.SimpleMove((Vector3) (vector * this.speed.Value));
                }
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.moveVector = vector;
            this.speed = 1f;
            this.space = Space.World;
        }
    }
}

