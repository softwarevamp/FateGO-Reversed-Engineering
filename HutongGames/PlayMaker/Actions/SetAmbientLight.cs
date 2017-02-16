﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.RenderSettings), HutongGames.PlayMaker.Tooltip("Sets the Ambient Light Color for the scene.")]
    public class SetAmbientLight : FsmStateAction
    {
        [RequiredField]
        public FsmColor ambientColor;
        public bool everyFrame;

        private void DoSetAmbientColor()
        {
            RenderSettings.ambientLight = this.ambientColor.Value;
        }

        public override void OnEnter()
        {
            this.DoSetAmbientColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetAmbientColor();
        }

        public override void Reset()
        {
            this.ambientColor = Color.gray;
            this.everyFrame = false;
        }
    }
}

