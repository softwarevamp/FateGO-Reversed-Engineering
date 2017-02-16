namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOBattleCutIn : FsmStateAction
    {
        public FsmGameObject createObject;
        [RequiredField, CheckForComponent(typeof(BattlePerformance))]
        public FsmGameObject performanceObject;

        public override void OnEnter()
        {
            GameObject prefab = this.createObject.Value;
            GameObject obj3 = this.performanceObject.Value;
            if (obj3 != null)
            {
                obj3.GetComponent<BattlePerformance>().showCutIn(prefab);
            }
            base.Finish();
        }
    }
}

