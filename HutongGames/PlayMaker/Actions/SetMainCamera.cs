﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the main camera."), ActionCategory(ActionCategory.Camera)]
    public class SetMainCamera : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The GameObject to set as the main camera (should have a Camera component)."), RequiredField, CheckForComponent(typeof(Camera))]
        public FsmGameObject gameObject;

        public override void OnEnter()
        {
            if (this.gameObject.Value != null)
            {
                if (Camera.main != null)
                {
                    Camera.main.gameObject.tag = "Untagged";
                }
                this.gameObject.Value.tag = "MainCamera";
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

