namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Tutorial Flag System Set Action"), ActionCategory(ActionCategory.GameLogic)]
    public class TutorialSetFlag : FsmStateAction
    {
        [Tooltip("Set tutorial flag name string.")]
        public FsmString flagName;

        public override void OnEnter()
        {
            TutorialFlag.Set(this.flagName.Value);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

