namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorCheckScript : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmEvent FalseEvent;
        public FsmString scriptKey;
        public FsmInt scriptValue;
        public FsmEvent TrueEvent;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            string key = this.scriptKey.Value;
            int num = this.scriptValue.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    if (component.checkScriptValue(key, num))
                    {
                        base.Fsm.Event(this.TrueEvent);
                    }
                    else
                    {
                        base.Fsm.Event(this.FalseEvent);
                    }
                }
            }
            base.Finish();
        }
    }
}

