namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUI), HutongGames.PlayMaker.Tooltip("Sets the Tinting Color for all text rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
    public class SetGUIContentColor : FsmStateAction
    {
        public FsmBool applyGlobally;
        [RequiredField]
        public FsmColor contentColor;

        public override void OnGUI()
        {
            GUI.contentColor = this.contentColor.Value;
            if (this.applyGlobally.Value)
            {
                PlayMakerGUI.GUIContentColor = GUI.contentColor;
            }
        }

        public override void Reset()
        {
            this.contentColor = Color.white;
        }
    }
}

