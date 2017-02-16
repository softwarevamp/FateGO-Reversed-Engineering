namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    public class FGOGetNodeObject : FsmStateAction
    {
        public FsmString nodename;
        public FsmGameObject storeObject;
        [RequiredField]
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.targetObject.Value;
            if (obj2 != null)
            {
                Transform transform = obj2.transform.getNodeFromLvName(this.nodename.Value, -1);
                this.storeObject.Value = transform.gameObject;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.targetObject = null;
            this.nodename = null;
            this.storeObject = null;
        }
    }
}

