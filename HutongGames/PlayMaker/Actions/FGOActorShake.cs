namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorShake : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public FsmEvent ChangedEvent;
        private GameObject shakeTarget;
        [RequiredField]
        public FsmVector3 shakeValue;
        private float startTime;
        public FsmFloat time;
        private float timer;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    component.ShakePosition(this.shakeValue.Value, this.time.Value);
                    this.startTime = FsmTime.RealtimeSinceStartup;
                    this.timer = 0f;
                }
            }
            if ((this.time.Value <= 0f) || (this.ChangedEvent == null))
            {
                if (this.ChangedEvent != null)
                {
                    base.Fsm.Event(this.ChangedEvent);
                }
                this.ChangedEvent = null;
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.timer += Time.deltaTime;
            if (this.timer >= this.time.Value)
            {
                if (this.ChangedEvent != null)
                {
                    base.Fsm.Event(this.ChangedEvent);
                }
                base.Finish();
            }
        }
    }
}

