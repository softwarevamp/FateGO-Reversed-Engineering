﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("iTween"), HutongGames.PlayMaker.Tooltip("Multiplies a GameObject's scale over time.")]
    public class iTweenScaleBy : iTweenFsmAction
    {
        [HutongGames.PlayMaker.Tooltip("The time in seconds the animation will wait before beginning.")]
        public FsmFloat delay;
        [HutongGames.PlayMaker.Tooltip("The shape of the easing curve applied to the animation.")]
        public iTween.EaseType easeType = iTween.EaseType.linear;
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
        public FsmString id;
        [HutongGames.PlayMaker.Tooltip("The type of loop to apply once the animation has completed.")]
        public iTween.LoopType loopType;
        [HutongGames.PlayMaker.Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
        public FsmFloat speed;
        [HutongGames.PlayMaker.Tooltip("The time in seconds the animation will take to complete.")]
        public FsmFloat time;
        [HutongGames.PlayMaker.Tooltip("A vector that will multiply current GameObjects scale."), RequiredField]
        public FsmVector3 vector;

        private void DoiTween()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Vector3 zero = Vector3.zero;
                if (!this.vector.IsNone)
                {
                    zero = this.vector.Value;
                }
                base.itweenType = "scale";
                object[] args = new object[] { 
                    "amount", zero, "name", !this.id.IsNone ? this.id.Value : string.Empty, !this.speed.IsNone ? "speed" : "time", !this.speed.IsNone ? this.speed.Value : (!this.time.IsNone ? this.time.Value : 1f), "delay", !this.delay.IsNone ? this.delay.Value : 0f, "easetype", this.easeType, "looptype", this.loopType, "oncomplete", "iTweenOnComplete", "oncompleteparams", base.itweenID,
                    "onstart", "iTweenOnStart", "onstartparams", base.itweenID, "ignoretimescale", !base.realTime.IsNone ? ((object) base.realTime.Value) : ((object) 0)
                };
                iTween.ScaleBy(ownerDefaultTarget, iTween.Hash(args));
            }
        }

        public override void OnEnter()
        {
            base.OnEnteriTween(this.gameObject);
            if (this.loopType != iTween.LoopType.none)
            {
                base.IsLoop(true);
            }
            this.DoiTween();
        }

        public override void OnExit()
        {
            base.OnExitiTween(this.gameObject);
        }

        public override void Reset()
        {
            base.Reset();
            FsmString str = new FsmString {
                UseVariable = true
            };
            this.id = str;
            this.time = 1f;
            this.delay = 0f;
            this.loopType = iTween.LoopType.none;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.vector = vector;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.speed = num;
        }
    }
}

