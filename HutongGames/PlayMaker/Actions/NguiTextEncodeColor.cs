namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("NGUI Tools"), HutongGames.PlayMaker.Tooltip("Encode a color into a string for label coloring tags")]
    public class NguiTextEncodeColor : FsmStateAction
    {
        private Color _lastColor;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The Color")]
        public FsmColor color;
        [UIHint(UIHint.Variable), RequiredField, HutongGames.PlayMaker.Tooltip("The string representation  result of that color")]
        public FsmString colorString;
        public bool everyFrame;

        public override void OnEnter()
        {
            this._lastColor = this.color.Value;
            this.colorString.Value = NGUIText.EncodeColor(this._lastColor);
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            if (this._lastColor != this.color.Value)
            {
                this._lastColor = this.color.Value;
                this.colorString.Value = NGUIText.EncodeColor(this._lastColor);
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

