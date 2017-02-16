namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Manager Config System Check"), ActionCategory(ActionCategory.GameLogic)]
    public class ManagerConfigCheck : FsmStateAction
    {
        [RequiredField]
        public Kind kind;

        public override void OnEnter()
        {
            bool useDebugCommand = false;
            if (this.kind == Kind.USE_DEBUG_COMMAND)
            {
                useDebugCommand = ManagerConfig.UseDebugCommand;
            }
            base.Finish();
            base.Fsm.Event(!useDebugCommand ? "RESULT_FALSE" : "RESULT_TRUE");
        }

        public override void Reset()
        {
        }

        public enum Kind
        {
            USE_DEBUG_COMMAND
        }
    }
}

