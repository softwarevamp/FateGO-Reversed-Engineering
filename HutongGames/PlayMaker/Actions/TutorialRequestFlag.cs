namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.GameLogic), Tooltip("Tutorial Flag System Request Action")]
    public class TutorialRequestFlag : FsmStateAction
    {
        [Tooltip("Request tutorial flag name string.")]
        public FsmString flagName;

        protected void EndRequest(string result)
        {
            base.Finish();
            base.Fsm.Event((result != "ok") ? "REQUEST_NG" : "REQUEST_OK");
        }

        public override void OnEnter()
        {
            TutorialFlag.Id flagId = TutorialFlag.GetId(this.flagName.Value);
            NetworkManager.getRequest<TutorialSetRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequest)).beginRequest(flagId);
        }

        public override void Reset()
        {
        }
    }
}

