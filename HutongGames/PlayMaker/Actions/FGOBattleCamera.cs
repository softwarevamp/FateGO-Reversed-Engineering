namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOBattleCamera : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject cameraObject;
        public FsmString eventname;
        public FsmString side;

        public override void OnEnter()
        {
            GameObject obj2 = this.cameraObject.Value;
            if (obj2 != null)
            {
                PlayMakerFSM component = obj2.GetComponent<PlayMakerFSM>();
                if (!this.side.IsNone)
                {
                    component.SendEvent(this.eventname.Value + this.side.Value);
                }
                else
                {
                    component.SendEvent(this.eventname.Value);
                }
            }
            base.Finish();
        }
    }
}

