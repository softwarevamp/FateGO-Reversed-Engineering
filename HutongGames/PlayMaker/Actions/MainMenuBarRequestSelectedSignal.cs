namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Main Menu Bar request selected signal"), ActionCategory(ActionCategory.InputDevice)]
    public class MainMenuBarRequestSelectedSignal : FsmStateAction
    {
        public override void OnEnter()
        {
            MainMenuBar.requestSelectedSignal();
            base.Finish();
        }
    }
}

