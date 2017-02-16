namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Moves a Game Object with a Character Controller. See also Controller Simple Move. NOTE: It is recommended that you make only one call to Move or SimpleMove per frame."), ActionCategory(ActionCategory.Character)]
    public class ControllerMove : FsmStateAction
    {
        private CharacterController controller;
        [RequiredField, CheckForComponent(typeof(CharacterController)), HutongGames.PlayMaker.Tooltip("The GameObject to move.")]
        public FsmOwnerDefault gameObject;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The movement vector.")]
        public FsmVector3 moveVector;
        [HutongGames.PlayMaker.Tooltip("Movement vector is defined in units per second. Makes movement frame rate independent.")]
        public FsmBool perSecond;
        private GameObject previousGo;
        [HutongGames.PlayMaker.Tooltip("Move in local or word space.")]
        public Space space;

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
                    Vector3 motion = (this.space != Space.World) ? ownerDefaultTarget.transform.TransformDirection(this.moveVector.Value) : this.moveVector.Value;
                    if (this.perSecond.Value)
                    {
                        this.controller.Move((Vector3) (motion * Time.deltaTime));
                    }
                    else
                    {
                        this.controller.Move(motion);
                    }
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
            this.space = Space.World;
            this.perSecond = 1;
        }
    }
}

