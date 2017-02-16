namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ExAction")]
    public class setTransformEx : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject gameObject;
        public FsmGameObject setTransform;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            GameObject obj3 = this.setTransform.Value;
            if (obj2 != null)
            {
                obj2.transform.position = obj3.transform.position;
                obj2.transform.rotation = obj3.transform.rotation;
                obj2.transform.localScale = obj3.transform.localScale;
            }
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

