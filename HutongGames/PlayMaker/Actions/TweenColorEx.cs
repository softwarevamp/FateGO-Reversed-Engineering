namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ExAction")]
    public class TweenColorEx : FsmStateAction
    {
        public FsmColor endcolor = Color.white;
        [RequiredField]
        public FsmGameObject gameObject;
        public FsmFloat time = 0.15f;

        public override void OnEnter()
        {
            GameObject go = this.gameObject.Value;
            if (go != null)
            {
                TweenColor.Begin(go, this.time.Value, this.endcolor.Value);
            }
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

