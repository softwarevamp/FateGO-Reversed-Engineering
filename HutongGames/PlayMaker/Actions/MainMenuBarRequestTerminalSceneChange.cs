namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.InputDevice), Tooltip("Main Menu Bar request terminal scene change")]
    public class MainMenuBarRequestTerminalSceneChange : FsmStateAction
    {
        public override void OnEnter()
        {
            MainMenuBar.requestTerminalSceneChange();
            base.Finish();
        }
    }
}

