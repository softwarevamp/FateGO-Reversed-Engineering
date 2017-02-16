namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Main Menu Bar set active state"), ActionCategory(ActionCategory.InputDevice)]
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

