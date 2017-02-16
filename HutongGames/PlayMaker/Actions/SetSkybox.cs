namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the global Skybox."), ActionCategory(ActionCategory.RenderSettings)]
    public class SetSkybox : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the Skybox is changing.")]
        public bool everyFrame;
        public FsmMaterial skybox;

        public override void OnEnter()
        {
            RenderSettings.skybox = this.skybox.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            RenderSettings.skybox = this.skybox.Value;
        }

        public override void Reset()
        {
            this.skybox = null;
        }
    }
}

