namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Destroys a Game Object.")]
    public class DestroyObject : FsmStateAction
    {
        [HasFloatSlider(0f, 5f), HutongGames.PlayMaker.Tooltip("Optional delay before destroying the Game Object.")]
        public FsmFloat delay;
        [HutongGames.PlayMaker.Tooltip("Detach children before destroying the Game Object.")]
        public FsmBool detachChildren;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to destroy.")]
        public FsmGameObject gameObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            if (obj2 != null)
            {
                if (this.delay.Value <= 0f)
                {
                    UnityEngine.Object.Destroy(obj2);
                }
                else
                {
                    UnityEngine.Object.Destroy(obj2, this.delay.Value);
                }
                if (this.detachChildren.Value)
                {
                    obj2.transform.DetachChildren();
                }
            }
            base.Finish();
        }

        public override void OnUpdate()
        {
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.delay = 0f;
        }
    }
}

