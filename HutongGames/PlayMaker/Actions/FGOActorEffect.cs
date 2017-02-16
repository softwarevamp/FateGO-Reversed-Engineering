namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorEffect : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmString actorside;
        public FsmString effectname;
        public FsmVector3 localpos;
        public PROC proctype;
        public FsmBool sideflip;
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            GameObject obj3 = null;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (((this.proctype == PROC.NAME_INPUT) && (this.effectname.Value != null)) && !this.effectname.Value.Equals(string.Empty))
                {
                    obj3 = component.playSideEffect(this.effectname.Value, this.localpos.Value, this.sideflip.Value);
                }
            }
            if (this.storeObject != null)
            {
                this.storeObject.Value = obj3;
            }
            base.Finish();
        }

        public enum PROC
        {
            NAME_INPUT,
            DAMAGE_1,
            DAMAGE_2,
            DAMAGE_3,
            DAMAGE_4,
            DAMAGE_5,
            DAMAGE_6
        }
    }
}

