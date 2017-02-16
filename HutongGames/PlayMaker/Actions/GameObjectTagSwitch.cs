namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sends an Event based on a Game Object's Tag."), ActionCategory(ActionCategory.Logic)]
    public class GameObjectTagSwitch : FsmStateAction
    {
        [CompoundArray("Tag Switches", "Compare Tag", "Send Event"), UIHint(UIHint.Tag)]
        public FsmString[] compareTo;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("The GameObject to test."), UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject gameObject;
        public FsmEvent[] sendEvent;

        private void DoTagSwitch()
        {
            GameObject obj2 = this.gameObject.Value;
            if (obj2 != null)
            {
                for (int i = 0; i < this.compareTo.Length; i++)
                {
                    if (obj2.tag == this.compareTo[i].Value)
                    {
                        base.Fsm.Event(this.sendEvent[i]);
                        return;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoTagSwitch();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoTagSwitch();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.compareTo = new FsmString[1];
            this.sendEvent = new FsmEvent[1];
            this.everyFrame = false;
        }
    }
}

