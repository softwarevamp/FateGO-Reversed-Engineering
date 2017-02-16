namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorNoblePhantasmc : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmGameObject battlePerformance;
        public FsmInt tagValue;
        public bool whiteInFlg = true;

        private void onEndNoblePhantasm()
        {
            base.Finish();
        }

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 == null)
            {
                obj2 = this.battlePerformance.Value;
                BattlePerformance component = obj2.GetComponent<BattlePerformance>();
                if (component != null)
                {
                    if (this.tagValue.Value != 0)
                    {
                        obj2 = component.EnemyActorList[0];
                    }
                    else
                    {
                        obj2 = component.PlayerActorList[0];
                    }
                }
            }
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().startNoblePhantasm(new System.Action(this.onEndNoblePhantasm), this.whiteInFlg);
            }
        }
    }
}

