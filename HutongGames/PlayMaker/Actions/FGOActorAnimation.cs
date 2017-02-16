namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorAnimation : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public ANIMATIONNAME animationname;
        public FsmBool endAnimationCall;
        public FsmEvent sendEvent;
        public FsmString starttag;

        public FGOActorAnimation()
        {
            FsmString str = new FsmString {
                UseVariable = true
            };
            this.starttag = str;
            this.endAnimationCall = 0;
        }

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                string endevent = null;
                if (this.sendEvent != null)
                {
                    endevent = this.sendEvent.Name;
                }
                if (this.endAnimationCall.Value)
                {
                    endevent = "END_ANIMATION";
                }
                string str2 = this.starttag.Value;
                if ((this.animationname == ANIMATIONNAME.spell) && !component.checkAnimation(this.animationname.ToString()))
                {
                    this.animationname = ANIMATIONNAME.attack_a;
                }
                if (0 < str2.Length)
                {
                    component.playCallAnimation(this.animationname.ToString(), endevent, this.starttag.Value);
                }
                else
                {
                    component.playCallAnimation(this.animationname.ToString(), endevent, null);
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public enum ANIMATIONNAME
        {
            NONE,
            attack_a,
            attack_b,
            attack_q,
            attack_gen,
            spell,
            damage_01,
            step_front,
            step_back,
            wait,
            treasure_arms,
            death_01,
            attack_ex,
            attack_a02,
            attack_b02,
            attack_q02,
            attack_gen02,
            attack_ex02,
            attack_a03,
            attack_b03,
            attack_q03,
            attack_gen03,
            attack_ex03,
            spell02,
            spell03
        }
    }
}

