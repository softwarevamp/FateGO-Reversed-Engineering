namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorCheck : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public CHECK check;
        public FsmEvent FalseEvent;
        public FsmEvent TrueEvent;

        public override void OnEnter()
        {
            bool isEnemy = true;
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (this.check == CHECK.PREVATTACK_ME)
                {
                    isEnemy = component.checkPrevAttackMe();
                }
                else if (this.check == CHECK.NEXTATTACK_ME)
                {
                    isEnemy = component.checkNextAttackMe();
                }
                else if (this.check == CHECK.ISENEMY)
                {
                    isEnemy = component.IsEnemy;
                }
                else if (this.check == CHECK.ISPLAYER)
                {
                    isEnemy = !component.IsEnemy;
                }
                else if (this.check == CHECK.STEP_IN)
                {
                    isEnemy = component.checkStepIn();
                }
                else if (this.check == CHECK.ISSHADOW)
                {
                    isEnemy = component.isShadowServant();
                }
                else if (this.check == CHECK.ISMONSTER)
                {
                    isEnemy = component.isMonsterServant();
                }
            }
            if (isEnemy)
            {
                base.Fsm.Event(this.TrueEvent);
            }
            else
            {
                base.Fsm.Event(this.FalseEvent);
            }
            base.Finish();
        }

        public enum CHECK
        {
            NONE,
            PREVATTACK_ME,
            NEXTATTACK_ME,
            ISPLAYER,
            ISENEMY,
            STEP_IN,
            ISSHADOW,
            ISMONSTER
        }
    }
}

