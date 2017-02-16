namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets a Float Variable to a random value between Min/Max."), ActionCategory(ActionCategory.Math)]
    public class RandomFloat : FsmStateAction
    {
        [RequiredField]
        public FsmFloat max;
        [RequiredField]
        public FsmFloat min;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmFloat storeResult;

        public override void OnEnter()
        {
            this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.min = 0f;
            this.max = 1f;
            this.storeResult = null;
        }
    }
}

