namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Tutorial Progress Flag System Check"), ActionCategory(ActionCategory.GameLogic)]
    public class TutorialCheckProgressFlag : FsmStateAction
    {
        [Tooltip("compare tutorial progress flag data int.")]
        public FsmInt cmpData;

        public override void OnEnter()
        {
            int progress = TutorialFlag.GetProgress();
            base.Finish();
            if (progress > this.cmpData.Value)
            {
                base.Fsm.Event("FLAG_OVER");
            }
            else if (progress == this.cmpData.Value)
            {
                base.Fsm.Event("FLAG_EQUAL");
            }
            else
            {
                base.Fsm.Event("FLAG_UNDER");
            }
        }

        public override void Reset()
        {
        }
    }
}

