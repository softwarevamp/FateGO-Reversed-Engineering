namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("GUILayout Box."), ActionCategory(ActionCategory.GUILayout)]
    public class GUILayoutBox : GUILayoutAction
    {
        [HutongGames.PlayMaker.Tooltip("Image to display in the Box.")]
        public FsmTexture image;
        [HutongGames.PlayMaker.Tooltip("Optional GUIStyle in the active GUISkin.")]
        public FsmString style;
        [HutongGames.PlayMaker.Tooltip("Text to display in the Box.")]
        public FsmString text;
        [HutongGames.PlayMaker.Tooltip("Optional Tooltip string.")]
        public FsmString tooltip;

        public override void OnGUI()
        {
            if (string.IsNullOrEmpty(this.style.Value))
            {
                GUILayout.Box(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), base.LayoutOptions);
            }
            else
            {
                GUILayout.Box(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.text = string.Empty;
            this.image = null;
            this.tooltip = string.Empty;
            this.style = string.Empty;
        }
    }
}

