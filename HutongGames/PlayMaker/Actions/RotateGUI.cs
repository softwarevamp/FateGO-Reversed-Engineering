﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUI), HutongGames.PlayMaker.Tooltip("Rotates the GUI around a pivot point. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
    public class RotateGUI : FsmStateAction
    {
        [RequiredField]
        public FsmFloat angle;
        private bool applied;
        public bool applyGlobally;
        public bool normalized;
        [RequiredField]
        public FsmFloat pivotX;
        [RequiredField]
        public FsmFloat pivotY;

        public override void OnGUI()
        {
            if (!this.applied)
            {
                Vector2 pivotPoint = new Vector2(this.pivotX.Value, this.pivotY.Value);
                if (this.normalized)
                {
                    pivotPoint.x *= Screen.width;
                    pivotPoint.y *= Screen.height;
                }
                GUIUtility.RotateAroundPivot(this.angle.Value, pivotPoint);
                if (this.applyGlobally)
                {
                    PlayMakerGUI.GUIMatrix = GUI.matrix;
                    this.applied = true;
                }
            }
        }

        public override void OnUpdate()
        {
            this.applied = false;
        }

        public override void Reset()
        {
            this.angle = 0f;
            this.pivotX = 0.5f;
            this.pivotY = 0.5f;
            this.normalized = true;
            this.applyGlobally = false;
        }
    }
}

