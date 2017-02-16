namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Device), Tooltip("Push the Test Notiffication data.")]
    public class PushTestNotiffication : FsmStateAction
    {
        protected void EndPlaySe()
        {
            base.Finish();
        }

        public override void OnEnter()
        {
        }

        public override void Reset()
        {
        }
    }
}

