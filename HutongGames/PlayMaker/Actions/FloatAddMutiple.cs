namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Math), Tooltip("Adds multipe float variables to float variable.")]
    public class FloatAddMutiple : FsmStateAction
    {
        [RequiredField, Tooltip("Add to this variable."), UIHint(UIHint.Variable)]
        public FsmFloat addTo;
        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), Tooltip("The float variables to add.")]
        public FsmFloat[] floatVariables;

        private void DoFloatAdd()
        {
            for (int i = 0; i < this.floatVariables.Length; i++)
            {
                this.addTo.Value += this.floatVariables[i].Value;
            }
        }

        public override void OnEnter()
        {
            this.DoFloatAdd();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFloatAdd();
        }

        public override void Reset()
        {
            this.floatVariables = null;
            this.addTo = null;
            this.everyFrame = false;
        }
    }
}

