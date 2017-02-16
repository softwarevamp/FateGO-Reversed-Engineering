namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorSetBillboard : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmBool isEnabled;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BillBoard component = obj2.GetComponent<BillBoard>();
                if (component != null)
                {
                    component.enabled = this.isEnabled.Value;
                    obj2.transform.rotation = Quaternion.Euler(obj2.transform.rotation.x, 180f, obj2.transform.rotation.z);
                }
            }
            base.Finish();
        }
    }
}

