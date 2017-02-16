namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Plays the Script data."), ActionCategory(ActionCategory.Movie)]
    public class PlayScript : FsmStateAction
    {
        [Tooltip("Set menu button active.")]
        public FsmBool activeMenu;
        [Tooltip("Set skip button active.")]
        public FsmBool activeSkip;
        [Tooltip("Set the asset name string.")]
        public FsmString assetName;
        [Tooltip("Event to send when the se finishes playing.")]
        public FsmEvent finishedEvent;
        [Tooltip("Set the label name string.")]
        public FsmString labelName;

        protected void EndPlayScript(bool isExit)
        {
            if (this.finishedEvent != null)
            {
                base.Fsm.Event(this.finishedEvent);
            }
        }

        protected void EndPlayScriptDebug(bool isExit)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
            if (this.finishedEvent != null)
            {
                base.Fsm.Event(this.finishedEvent);
            }
        }

        public override void OnEnter()
        {
            if (string.IsNullOrEmpty(this.assetName.Value) && string.IsNullOrEmpty(this.labelName.Value))
            {
                this.EndPlayScript(false);
            }
            else
            {
                ScriptManager.Play(this.assetName.Value, this.labelName.Value, this.activeMenu.Value, this.activeSkip.Value, new ScriptManager.CallbackFunc(this.EndPlayScript));
            }
            base.Finish();
        }
    }
}

