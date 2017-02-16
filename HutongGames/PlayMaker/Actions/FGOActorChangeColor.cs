namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorChangeColor : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public FsmColor addColor;
        public FsmEvent ChangedEvent;
        public iTween.EaseType easeType;
        public FsmColor mulColor;
        public bool realTime;
        private float startTime;
        public FsmFloat time;
        private float timer;

        public override void OnEnter()
        {
            GameObject target = this.actorObject.Value;
            if (target != null)
            {
                BattleActorControl component = target.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    Color mainColor = component.GetMainColor();
                    Color addColor = component.GetAddColor();
                    object[] args = new object[] { "from", mainColor, "to", this.mulColor.Value, "easetype", this.easeType, "onupdate", "SetMainColor", "time", this.time.Value };
                    iTween.ValueTo(target, iTween.Hash(args));
                    object[] objArray2 = new object[] { "from", addColor, "to", this.addColor.Value, "easetype", this.easeType, "onupdate", "SetAddColor", "time", this.time.Value };
                    iTween.ValueTo(target, iTween.Hash(objArray2));
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
            if (this.realTime)
            {
                this.timer = FsmTime.RealtimeSinceStartup - this.startTime;
            }
            else
            {
                this.timer += Time.deltaTime;
            }
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

