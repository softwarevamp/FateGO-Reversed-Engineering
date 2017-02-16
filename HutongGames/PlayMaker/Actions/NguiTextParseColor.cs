namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Parse a color string into color variable"), ActionCategory("NGUI Tools")]
    public class NguiTextParseColor : FsmStateAction
    {
        private string _lastColor;
        [RequiredField, UIHint(UIHint.Variable), Tooltip("The Color result")]
        public FsmColor color;
        [RequiredField, Tooltip("The string representation of that color")]
        public FsmString colorString;
        public bool everyFrame;

        public override void OnEnter()
        {
            this._lastColor = this.colorString.Value;
            this.color.Value = NGUIText.ParseColor(this._lastColor, 0);
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            if (this._lastColor != this.colorString.Value)
            {
                this._lastColor = this.colorString.Value;
                this.color.Value = NGUIText.ParseColor(this._lastColor, 0);
            }
        }

        public override void Reset()
        {
            this.color = null;
            this.colorString = null;
            this.everyFrame = false;
        }
    }
}

