namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Input Common Confirm Dialog"), ActionCategory(ActionCategory.Input)]
    public class ActionCommonConfirmDialog : FsmStateAction
    {
        [Tooltip("Event to send when the cancel finishes.")]
        public FsmEvent cancelFinishedEvent;
        [Tooltip("Event to send when the decide finishes.")]
        public FsmEvent decideFinishedEvent;
        [Tooltip("Set the message name string.")]
        public FsmString messageName;
        [Tooltip("Set the title name string.")]
        public FsmString titleName;

        protected void EndDialog(bool isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
            base.Finish();
            if (isDecide)
            {
                if (this.decideFinishedEvent != null)
                {
                    base.Fsm.Event(this.decideFinishedEvent);
                }
            }
            else if (this.cancelFinishedEvent != null)
            {
                base.Fsm.Event(this.cancelFinishedEvent);
            }
        }

        public override void OnEnter()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(this.titleName.Value, this.messageName.Value, new CommonConfirmDialog.ClickDelegate(this.EndDialog));
        }

        public override void Reset()
        {
            this.decideFinishedEvent = null;
            this.cancelFinishedEvent = null;
        }
    }
}

