namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Restarts current level."), ActionCategory(ActionCategory.Level)]
    public class RestartLevel : FsmStateAction
    {
        public override void OnEnter()
        {
            Application.LoadLevel(Application.loadedLevelName);
            base.Finish();
        }
    }
}

