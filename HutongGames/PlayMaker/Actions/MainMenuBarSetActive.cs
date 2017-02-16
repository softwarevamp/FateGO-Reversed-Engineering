namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.InputDevice), Tooltip("Main Menu Bar set active state")]
    public class MainMenuBarSetActive : FsmStateAction
    {
        [Tooltip("Set active type.")]
        public FsmBool activeType;

        public override void OnEnter()
        {
            MainMenuBar.setButtonActive(this.activeType.Value);
            base.Finish();
        }
    }
}

