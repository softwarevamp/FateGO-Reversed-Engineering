﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Application), HutongGames.PlayMaker.Tooltip("Gets the Height of the Screen in pixels.")]
    public class GetScreenHeight : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmFloat storeScreenHeight;

        public override void OnEnter()
        {
            this.storeScreenHeight.Value = Screen.height;
            base.Finish();
        }

        public override void Reset()
        {
            this.storeScreenHeight = null;
        }
    }
}

