namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorCutIn : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmString filename;
        [CheckForComponent(typeof(BattlePerformance)), RequiredField]
        public FsmGameObject performanceObject;
        public FsmVector3 poppos;
        public TYPE type = TYPE.RTLD;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            GameObject obj3 = this.performanceObject.Value;
            if ((obj2 != null) && (obj3 != null))
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                BattlePerformance performance = obj3.GetComponent<BattlePerformance>();
                if (this.type == TYPE.INPUT_NAME)
                {
                    Debug.Log("------INPUT_NAME:" + this.filename.Value);
                    performance.playActorBigCutIn(this.filename.Value, this.poppos.Value, true);
                }
                else
                {
                    performance.playBigCutIn(component.uniqueID, (int) this.type, this.poppos.Value);
                }
            }
            base.Finish();
        }

        public enum TYPE
        {
            CENTER = 3,
            INPUT_NAME = 4,
            LTRD = 2,
            RTLD = 1
        }
    }
}

