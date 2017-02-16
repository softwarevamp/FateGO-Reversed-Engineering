namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOGetBattleResultDispFlag : FsmStateAction
    {
        public BattleResultComponent.resultData.ResultDispFlagEnum check;
        public FsmEvent FalseEvent;
        [CheckForComponent(typeof(BattleResultComponent)), RequiredField]
        public FsmGameObject performanceObject;
        public FsmEvent TrueEvent;

        public override void OnEnter()
        {
            bool flag = false;
            GameObject obj2 = this.performanceObject.Value;
            if (obj2 != null)
            {
                BattlePerformance component = obj2.GetComponent<BattlePerformance>();
                if (component.resultwindow != null)
                {
                    BattleResultComponent.resultData data = component.resultwindow.getBattleResult();
                    if (data != null)
                    {
                        flag = data.checkResultDispFlag(this.check);
                    }
                }
            }
            if (flag)
            {
                base.Fsm.Event(this.TrueEvent);
            }
            else
            {
                base.Fsm.Event(this.FalseEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.performanceObject = null;
        }
    }
}

