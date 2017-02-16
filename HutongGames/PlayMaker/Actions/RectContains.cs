namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Rect), HutongGames.PlayMaker.Tooltip("Tests if a point is inside a rectangle.")]
    public class RectContains : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Event to send if the Point is outside the Rectangle.")]
        public FsmEvent falseEvent;
        [HutongGames.PlayMaker.Tooltip("Point to test.")]
        public FsmVector3 point;
        [HutongGames.PlayMaker.Tooltip("Rectangle to test."), RequiredField]
        public FsmRect rectangle;
        [HutongGames.PlayMaker.Tooltip("Store the result in a variable."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        [HutongGames.PlayMaker.Tooltip("Event to send if the Point is inside the Rectangle.")]
        public FsmEvent trueEvent;
        [HutongGames.PlayMaker.Tooltip("Specify/override X value.")]
        public FsmFloat x;
        [HutongGames.PlayMaker.Tooltip("Specify/override Y value.")]
        public FsmFloat y;

        private void DoRectContains()
        {
            if (!this.rectangle.IsNone)
            {
                Vector3 point = this.point.Value;
                if (!this.x.IsNone)
                {
                    point.x = this.x.Value;
                }
                if (!this.y.IsNone)
                {
                    point.y = this.y.Value;
                }
                bool flag = this.rectangle.Value.Contains(point);
                this.storeResult.Value = flag;
                base.Fsm.Event(!flag ? this.falseEvent : this.trueEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoRectContains();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoRectContains();
        }

        public override void Reset()
        {
            FsmRect rect = new FsmRect {
                UseVariable = true
            };
            this.rectangle = rect;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.point = vector;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.x = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.y = num;
            this.storeResult = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.everyFrame = false;
        }
    }
}

