namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Tutorial Flag System Check"), ActionCategory(ActionCategory.GameLogic)]
    public class TutorialCheckFlag : FsmStateAction
    {
        [Tooltip("Get tutorial flag name string.")]
        public FsmString flagName;

        public override void OnEnter()
        {
            bool flag = TutorialFlag.Get(this.flagName.Value);
            base.Finish();
            base.Fsm.Event(!flag ? "FLAG_OFF" : "FLAG_ON");
        }

        public override void Reset()
        {
        }
    }
}

