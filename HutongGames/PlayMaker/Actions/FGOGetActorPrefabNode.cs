namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOGetActorPrefabNode : FsmStateAction
    {
        [CheckForComponent(typeof(BattleFBXComponent)), RequiredField]
        public FsmGameObject actorObject;
        public FsmString prefabNodeName;
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleFBXComponent component = obj2.GetComponent<BattleFBXComponent>();
                if (!this.storeObject.IsNone)
                {
                    this.storeObject.Value = component.GetPrefabNode(this.prefabNodeName.Value);
                }
            }
            base.Finish();
        }
    }
}

