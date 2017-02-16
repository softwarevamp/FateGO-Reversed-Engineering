namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Transform), HutongGames.PlayMaker.Tooltip("Rotates a Game Object so its forward vector points at a Target. The Target can be specified as a GameObject or a world Position. If you specify both, then Position specifies a local offset from the target object's Position.")]
    public class LookAt : FsmStateAction
    {
        [Title("Draw Debug Line"), HutongGames.PlayMaker.Tooltip("Draw a debug line from the GameObject to the Target.")]
        public FsmBool debug;
        [HutongGames.PlayMaker.Tooltip("Color to use for the debug line.")]
        public FsmColor debugLineColor;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame = true;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;
        private GameObject go;
        private GameObject goTarget;
        [HutongGames.PlayMaker.Tooltip("Don't rotate vertically.")]
        public FsmBool keepVertical;
        private Vector3 lookAtPos;
        private Vector3 lookAtPosWithVertical;
        [HutongGames.PlayMaker.Tooltip("The GameObject to Look At.")]
        public FsmGameObject targetObject;
        [HutongGames.PlayMaker.Tooltip("World position to look at, or local offset from Target Object if specified.")]
        public FsmVector3 targetPosition;
        [HutongGames.PlayMaker.Tooltip("Rotate the GameObject to point its up direction vector in the direction hinted at by the Up Vector. See Unity Look At docs for more details.")]
        public FsmVector3 upVector;

        private void DoLookAt()
        {
            if (this.UpdateLookAtPosition())
            {
                this.go.transform.LookAt(this.lookAtPos, !this.upVector.IsNone ? this.upVector.Value : Vector3.up);
                if (this.debug.Value)
                {
                    Debug.DrawLine(this.go.transform.position, this.lookAtPos, this.debugLineColor.Value);
                }
            }
        }

        public Vector3 GetLookAtPosition() => 
            this.lookAtPos;

        public Vector3 GetLookAtPositionWithVertical() => 
            this.lookAtPosWithVertical;

        public override void OnEnter()
        {
            this.DoLookAt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnLateUpdate()
        {
            this.DoLookAt();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.targetObject = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.targetPosition = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.upVector = vector;
            this.keepVertical = 1;
            this.debug = 0;
            this.debugLineColor = Color.yellow;
            this.everyFrame = true;
        }

        public bool UpdateLookAtPosition()
        {
            this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (this.go == null)
            {
                return false;
            }
            this.goTarget = this.targetObject.Value;
            if ((this.goTarget == null) && this.targetPosition.IsNone)
            {
                return false;
            }
            if (this.goTarget != null)
            {
                this.lookAtPos = this.targetPosition.IsNone ? this.goTarget.transform.position : this.goTarget.transform.TransformPoint(this.targetPosition.Value);
            }
            else
            {
                this.lookAtPos = this.targetPosition.Value;
            }
            this.lookAtPosWithVertical = this.lookAtPos;
            if (this.keepVertical.Value)
            {
                this.lookAtPos.y = this.go.transform.position.y;
            }
            return true;
        }
    }
}

