namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorSetStepFlg : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmBool stepFlg;

        public override void OnEnter()
        {
            GameObject obj2 = this.actObject.Value;
            bool flg = this.stepFlg.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    component.setStepFlg(flg);
                }
            }
            base.Finish();
        }
    }
}

