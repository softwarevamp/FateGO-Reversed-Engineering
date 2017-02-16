namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    public class FGOActorNodeEffect : FsmStateAction
    {
        public FsmString nodename;
        [RequiredField]
        public FsmGameObject popObject;
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject original = this.popObject.Value;
            GameObject obj3 = this.targetObject.Value;
            if (original != null)
            {
                Vector3 zero = Vector3.zero;
                Vector3 up = Vector3.up;
                GameObject obj4 = (GameObject) UnityEngine.Object.Instantiate(original, zero, Quaternion.Euler(up));
                Transform transform = obj3.transform.getNodeFromLvName(this.nodename.Value, -1);
                obj4.transform.parent = transform;
                obj4.transform.localPosition = zero;
                obj4.transform.localEulerAngles = up;
                obj4.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.popObject = null;
            this.targetObject = null;
            this.nodename = null;
        }
    }
}

