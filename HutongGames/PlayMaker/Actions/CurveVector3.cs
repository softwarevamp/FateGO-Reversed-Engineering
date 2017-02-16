﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Animates the value of a Vector3 Variable FROM-TO with assistance of Deformation Curves."), ActionCategory(ActionCategory.AnimateVariables)]
    public class CurveVector3 : CurveFsmAction
    {
        [HutongGames.PlayMaker.Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.x and toValue.x.")]
        public CurveFsmAction.Calculation calculationX;
        [HutongGames.PlayMaker.Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.y and toValue.y.")]
        public CurveFsmAction.Calculation calculationY;
        [HutongGames.PlayMaker.Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.z and toValue.z.")]
        public CurveFsmAction.Calculation calculationZ;
        [RequiredField]
        public FsmAnimationCurve curveX;
        [RequiredField]
        public FsmAnimationCurve curveY;
        [RequiredField]
        public FsmAnimationCurve curveZ;
        private bool finishInNextStep;
        [RequiredField]
        public FsmVector3 fromValue;
        [RequiredField]
        public FsmVector3 toValue;
        private Vector3 vct;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmVector3 vectorVariable;

        public override void OnEnter()
        {
            base.OnEnter();
            this.finishInNextStep = false;
            base.resultFloats = new float[3];
            base.fromFloats = new float[] { !this.fromValue.IsNone ? this.fromValue.Value.x : 0f, !this.fromValue.IsNone ? this.fromValue.Value.y : 0f, !this.fromValue.IsNone ? this.fromValue.Value.z : 0f };
            base.toFloats = new float[] { !this.toValue.IsNone ? this.toValue.Value.x : 0f, !this.toValue.IsNone ? this.toValue.Value.y : 0f, !this.toValue.IsNone ? this.toValue.Value.z : 0f };
            base.curves = new AnimationCurve[] { this.curveX.curve, this.curveY.curve, this.curveZ.curve };
            base.calculations = new CurveFsmAction.Calculation[] { this.calculationX, this.calculationY, this.calculationZ };
            base.Init();
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!this.vectorVariable.IsNone && base.isRunning)
            {
                this.vct = new Vector3(base.resultFloats[0], base.resultFloats[1], base.resultFloats[2]);
                this.vectorVariable.Value = this.vct;
            }
            if (this.finishInNextStep && !base.looping)
            {
                base.Finish();
                if (base.finishEvent != null)
                {
                    base.Fsm.Event(base.finishEvent);
                }
            }
            if (base.finishAction && !this.finishInNextStep)
            {
                if (!this.vectorVariable.IsNone)
                {
                    this.vct = new Vector3(base.resultFloats[0], base.resultFloats[1], base.resultFloats[2]);
                    this.vectorVariable.Value = this.vct;
                }
                this.finishInNextStep = true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.vectorVariable = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.toValue = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.fromValue = vector;
        }
    }
}

