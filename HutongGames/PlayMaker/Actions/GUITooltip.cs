namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUI), HutongGames.PlayMaker.Tooltip("Gets the Tooltip of the control the mouse is currently over and store it in a String Variable.")]
    public class GUITooltip : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmString storeTooltip;

        public override void OnGUI()
        {
            this.storeTooltip.Value = GUI.tooltip;
        }

        public override void Reset()
        {
            this.storeTooltip = null;
        }
    }
}

