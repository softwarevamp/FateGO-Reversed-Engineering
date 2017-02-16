namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class GetNodeFromName : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actorObject;
        public FsmString nodename;
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                Transform transform = obj2.transform.getNodeFromLvName(this.nodename.Value, -1);
                if (transform != null)
                {
                    this.storeObject.Value = transform.gameObject;
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.actorObject = null;
            this.nodename = string.Empty;
            this.storeObject = null;
        }
    }
}

