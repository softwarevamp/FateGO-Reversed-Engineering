namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Main Menu Bar request selected scene change"), ActionCategory(ActionCategory.InputDevice)]
    public class MainMenuBarRequestSelectedSceneChange : FsmStateAction
    {
        public override void OnEnter()
        {
            MainMenuBar.requestSelectedSceneChange();
            base.Finish();
        }
    }
}

