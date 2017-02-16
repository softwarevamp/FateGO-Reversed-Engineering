namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOFieldLogic : FsmStateAction
    {
        [CheckForComponent(typeof(BattleFieldMotionComponent)), RequiredField]
        public FsmOwnerDefault gameObject;
        public PROC proc;
        public FsmEvent sendEvent;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            string endEvent = string.Empty;
            if (this.sendEvent != null)
            {
                endEvent = this.sendEvent.Name;
            }
            if (ownerDefaultTarget != null)
            {
                BattleFieldMotionComponent component = ownerDefaultTarget.GetComponent<BattleFieldMotionComponent>();
                if (this.proc == PROC.START_REPLACE)
                {
                    component.startReplaceActor(endEvent);
                }
                else if (this.proc == PROC.LOAD_TARGETACTOR)
                {
                    component.loadReplaceActor(endEvent);
                }
                else if (this.proc == PROC.LOAD_REPLACE)
                {
                    component.loadReplace(endEvent);
                }
            }
            base.Finish();
        }

        public enum PROC
        {
            START_REPLACE,
            LOAD_TARGETACTOR,
            LOAD_REPLACE
        }
    }
}

