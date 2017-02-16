namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Push the Test Notiffication data."), ActionCategory(ActionCategory.Device)]
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

