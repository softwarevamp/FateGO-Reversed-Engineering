namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Color), HutongGames.PlayMaker.Tooltip("Samples a Color on a continuous Colors gradient.")]
    public class ColorRamp : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("Array of colors to defining the gradient.")]
        public FsmColor[] colors;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Point on the gradient to sample. Should be between 0 and the number of colors in the gradient."), RequiredField]
        public FsmFloat sampleAt;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Store the sampled color in a Color variable."), UIHint(UIHint.Variable)]
        public FsmColor storeColor;

        private void DoColorRamp()
        {
            if ((((this.colors != null) && (this.colors.Length != 0)) && (this.sampleAt != null)) && (this.storeColor != null))
            {
                Color color;
                float f = Mathf.Clamp(this.sampleAt.Value, 0f, (float) (this.colors.Length - 1));
                if (f == 0f)
                {
                    color = this.colors[0].Value;
                }
                else if (f == this.colors.Length)
                {
                    color = this.colors[this.colors.Length - 1].Value;
                }
                else
                {
                    Color a = this.colors[Mathf.FloorToInt(f)].Value;
                    Color b = this.colors[Mathf.CeilToInt(f)].Value;
                    f -= Mathf.Floor(f);
                    color = Color.Lerp(a, b, f);
                }
                this.storeColor.Value = color;
            }
        }

        public override string ErrorCheck()
        {
            if (this.colors.Length < 2)
            {
                return "Define at least 2 colors to make a gradient.";
            }
            return null;
        }

        public override void OnEnter()
        {
            this.DoColorRamp();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoColorRamp();
        }

        public override void Reset()
        {
            this.colors = new FsmColor[3];
            this.sampleAt = 0f;
            this.storeColor = null;
            this.everyFrame = false;
        }
    }
}

