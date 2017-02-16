namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorShowMessage : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public TYPE showType;

        public override void OnEnter()
        {
            GameObject obj2 = this.actObject.Value;
            if (obj2 != null)
            {
                BattlePerformance performance = null;
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    performance = component.performance;
                }
                else
                {
                    performance = obj2.GetComponent<BattlePerformance>();
                }
                if (performance != null)
                {
                    if (this.showType == TYPE.COMMON)
                    {
                        performance.showActionMessage();
                    }
                    else if (this.showType == TYPE.NOBLE_NAME)
                    {
                        performance.showActionNobleTitle();
                    }
                }
            }
            base.Finish();
        }

        public enum TYPE
        {
            NOBLE_NAME,
            COMMON
        }
    }
}

