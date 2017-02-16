namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Multiplies a Vector3 variable by a Float."), ActionCategory(ActionCategory.Vector3)]
    public class Vector3Multiply : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmFloat multiplyBy;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmVector3 vector3Variable;

        public override void OnEnter()
        {
            this.vector3Variable.Value = (Vector3) (this.vector3Variable.Value * this.multiplyBy.Value);
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.vector3Variable.Value = (Vector3) (this.vector3Variable.Value * this.multiplyBy.Value);
        }

        public override void Reset()
        {
            this.vector3Variable = null;
            this.multiplyBy = 1f;
            this.everyFrame = false;
        }
    }
}

