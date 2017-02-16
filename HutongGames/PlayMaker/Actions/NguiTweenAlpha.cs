namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("NGUI")]
    public class NguiTweenAlpha : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject gameObject;
        public FsmFloat time = 0.15f;
        public FsmFloat toAlpha = 0f;

        public override void OnEnter()
        {
            GameObject go = this.gameObject.Value;
            if (go != null)
            {
                TweenAlpha.Begin(go, this.time.Value, this.toAlpha.Value);
            }
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

