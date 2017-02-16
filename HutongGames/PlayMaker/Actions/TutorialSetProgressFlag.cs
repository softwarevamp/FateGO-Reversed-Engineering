namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.GameLogic), Tooltip("Tutorial Progress Flag System Set Action")]
    public class TutorialSetProgressFlag : FsmStateAction
    {
        [Tooltip("Set tutorial progress flag data int.")]
        public FsmInt flagData;

        public override void OnEnter()
        {
            TutorialFlag.SetProgress(this.flagData.Value);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

