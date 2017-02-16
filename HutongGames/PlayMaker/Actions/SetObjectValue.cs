namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.UnityObject), Tooltip("Sets the value of an Object Variable.")]
    public class SetObjectValue : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmObject objectValue;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmObject objectVariable;

        public override void OnEnter()
        {
            this.objectVariable.Value = this.objectValue.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.objectVariable.Value = this.objectValue.Value;
        }

        public override void Reset()
        {
            this.objectVariable = null;
            this.objectValue = null;
            this.everyFrame = false;
        }
    }
}

