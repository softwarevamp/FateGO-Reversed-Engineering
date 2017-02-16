namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Converts a Bool value to a Color."), ActionCategory(ActionCategory.Convert)]
    public class ConvertBoolToColor : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The Bool variable to test.")]
        public FsmBool boolVariable;
        [HutongGames.PlayMaker.Tooltip("The Color variable to set based on the bool variable value."), UIHint(UIHint.Variable), RequiredField]
        public FsmColor colorVariable;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Color if Bool variable is false.")]
        public FsmColor falseColor;
        [HutongGames.PlayMaker.Tooltip("Color if Bool variable is true.")]
        public FsmColor trueColor;

        private void DoConvertBoolToColor()
        {
            this.colorVariable.Value = !this.boolVariable.Value ? this.falseColor.Value : this.trueColor.Value;
        }

        public override void OnEnter()
        {
            this.DoConvertBoolToColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoConvertBoolToColor();
        }

        public override void Reset()
        {
            this.boolVariable = null;
            this.colorVariable = null;
            this.falseColor = Color.black;
            this.trueColor = Color.white;
            this.everyFrame = false;
        }
    }
}

