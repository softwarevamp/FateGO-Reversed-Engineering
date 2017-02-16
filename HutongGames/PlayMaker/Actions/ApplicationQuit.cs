namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Quits the player application."), ActionCategory(ActionCategory.Application)]
    public class ApplicationQuit : FsmStateAction
    {
        public override void OnEnter()
        {
            Application.Quit();
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

