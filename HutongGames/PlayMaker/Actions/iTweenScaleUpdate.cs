﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory("iTween"), HutongGames.PlayMaker.Tooltip("CSimilar to ScaleTo but incredibly less expensive for usage inside the Update function or similar looping situations involving a 'live' set of changing values. Does not utilize an EaseType.")]
    public class iTweenScaleUpdate : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject go;
        private Hashtable hash;
        [HutongGames.PlayMaker.Tooltip("The time in seconds the animation will take to complete. If transformScale is set, this is used as an offset.")]
        public FsmFloat time;
        [HutongGames.PlayMaker.Tooltip("Scale To a transform scale.")]
        public FsmGameObject transformScale;
        [HutongGames.PlayMaker.Tooltip("A scale vector the GameObject will animate To.")]
        public FsmVector3 vectorScale;

        private void DoiTween()
        {
            iTween.ScaleUpdate(this.go, this.hash);
        }

        public override void OnEnter()
        {
            this.hash = new Hashtable();
            this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (this.go == null)
            {
                base.Finish();
            }
            else
            {
                if (this.transformScale.IsNone)
                {
                    this.hash.Add("scale", !this.vectorScale.IsNone ? this.vectorScale.Value : Vector3.zero);
                }
                else if (this.vectorScale.IsNone)
                {
                    this.hash.Add("scale", this.transformScale.Value.transform);
                }
                else
                {
                    this.hash.Add("scale", this.transformScale.Value.transform.localScale + this.vectorScale.Value);
                }
                this.hash.Add("time", !this.time.IsNone ? this.time.Value : 1f);
                this.DoiTween();
            }
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            this.hash.Remove("scale");
            if (this.transformScale.IsNone)
            {
                this.hash.Add("scale", !this.vectorScale.IsNone ? this.vectorScale.Value : Vector3.zero);
            }
            else if (this.vectorScale.IsNone)
            {
                this.hash.Add("scale", this.transformScale.Value.transform);
            }
            else
            {
                this.hash.Add("scale", this.transformScale.Value.transform.localScale + this.vectorScale.Value);
            }
            this.DoiTween();
        }

        public override void Reset()
        {
            FsmGameObject obj2 = new FsmGameObject {
                UseVariable = true
            };
            this.transformScale = obj2;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.vectorScale = vector;
            this.time = 1f;
        }
    }
}

