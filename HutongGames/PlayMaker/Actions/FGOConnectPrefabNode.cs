namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOConnectPrefabNode : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleFBXComponent))]
        public FsmGameObject actorObject;
        public FsmBool IsConnect;
        public FsmString prefabNodeName;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                obj2.GetComponent<BattleFBXComponent>().SetConnectPrefabNode(this.prefabNodeName.Value, this.IsConnect.Value);
            }
            base.Finish();
        }
    }
}

