﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Enables/Disables Fog in the scene."), ActionCategory(ActionCategory.RenderSettings)]
    public class EnableFog : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Set to True to enable, False to disable.")]
        public FsmBool enableFog;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the Enable Fog setting is changing.")]
        public bool everyFrame;

        public override void OnEnter()
        {
            RenderSettings.fog = this.enableFog.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            RenderSettings.fog = this.enableFog.Value;
        }

        public override void Reset()
        {
            this.enableFog = 1;
            this.everyFrame = false;
        }
    }
}

