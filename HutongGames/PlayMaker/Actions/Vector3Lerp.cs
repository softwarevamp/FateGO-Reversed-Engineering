namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Vector3), HutongGames.PlayMaker.Tooltip("Linearly interpolates between 2 vectors.")]
    public class Vector3Lerp : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
        public FsmFloat amount;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if any of the values are changing.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("First Vector."), RequiredField]
        public FsmVector3 fromVector;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the result in this vector variable."), RequiredField]
        public FsmVector3 storeResult;
        [HutongGames.PlayMaker.Tooltip("Second Vector."), RequiredField]
        public FsmVector3 toVector;

        private void DoVector3Lerp()
        {
            this.storeResult.Value = Vector3.Lerp(this.fromVector.Value, this.toVector.Value, this.amount.Value);
        }

        public override void OnEnter()
        {
            this.DoVector3Lerp();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoVector3Lerp();
        }

        public override void Reset()
        {
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.fromVector = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.toVector = vector;
            this.storeResult = null;
            this.everyFrame = true;
        }
    }
}

