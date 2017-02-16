namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Gets the Width of the Screen in pixels."), ActionCategory(ActionCategory.Application)]
    public class GetScreenWidth : FsmStateAction
    {
        [UIHint(UIHint.Variable), RequiredField]
        public FsmFloat storeScreenWidth;

        public override void OnEnter()
        {
            this.storeScreenWidth.Value = Screen.width;
            base.Finish();
        }

        public override void Reset()
        {
            this.storeScreenWidth = null;
        }
    }
}

