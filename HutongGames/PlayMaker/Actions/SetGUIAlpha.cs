﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the global Alpha for the GUI. Useful for fading GUI up/down. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls."), ActionCategory(ActionCategory.GUI)]
    public class SetGUIAlpha : FsmStateAction
    {
        [RequiredField]
        public FsmFloat alpha;
        public FsmBool applyGlobally;

        public override void OnGUI()
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha.Value);
            if (this.applyGlobally.Value)
            {
                PlayMakerGUI.GUIColor = GUI.color;
            }
        }

        public override void Reset()
        {
            this.alpha = 1f;
        }
    }
}

