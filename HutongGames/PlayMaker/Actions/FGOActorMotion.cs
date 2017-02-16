namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorMotion : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public MOTION motion;
        public float moveTime = 0.3f;
        public BattleActorControl.POS position;
        public FsmEvent sendEvent;
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            GameObject target = this.targetObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (this.motion != MOTION.NONE)
                {
                    if (this.motion == MOTION.JUMP)
                    {
                        component.motion_Jump(target, 3f, 0.3f, this.position, this.sendEvent.Name);
                    }
                    else if (this.motion == MOTION.STEP)
                    {
                        component.motion_Step(target, 0f, 0.15f, this.position, this.sendEvent.Name);
                    }
                    else if (this.motion == MOTION.BACKSTEP)
                    {
                        component.motion_BackStep(target, 0.2f, 0.15f, this.position, this.sendEvent.Name);
                    }
                    else if (this.motion == MOTION.STEP_WAIT)
                    {
                        component.motion_StepWait(target, 0f, 0.15f, this.position, this.sendEvent.Name);
                    }
                    else if (this.motion == MOTION.TREASURE_ARMS)
                    {
                        component.motion_TreasureArms(target, 0f, this.moveTime, this.position, this.sendEvent.Name);
                    }
                }
            }
            base.Finish();
        }

        public enum MOTION
        {
            NONE,
            JUMP,
            STEP,
            BACKSTEP,
            DASHATTACK,
            STEP_WAIT,
            TREASURE_ARMS
        }
    }
}

