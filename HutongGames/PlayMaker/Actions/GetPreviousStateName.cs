namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets the name of the previously active state and stores it in a String Variable."), ActionCategory(ActionCategory.StateMachine)]
    public class GetPreviousStateName : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmString storeName;

        public override void OnEnter()
        {
            this.storeName.Value = base.Fsm.PreviousActiveState?.Name;
            base.Finish();
        }

        public override void Reset()
        {
            this.storeName = null;
        }
    }
}

