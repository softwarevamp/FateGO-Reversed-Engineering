namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.StateMachine), Tooltip("Set the value of a Game Object Variable in another All FSM. Accept null reference")]
    public class SetAllFsmGameObject : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmOwnerDefault gameObject;

        private void DoSetFsmGameObject()
        {
        }

        public override void OnEnter()
        {
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
        }
    }
}

