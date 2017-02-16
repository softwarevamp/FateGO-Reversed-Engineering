namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUILayout), HutongGames.PlayMaker.Tooltip("GUILayout Text Field to edit an Int Variable. Optionally send an event if the text has been edited.")]
    public class GUILayoutIntField : GUILayoutAction
    {
        [HutongGames.PlayMaker.Tooltip("Optional event to send when the value changes.")]
        public FsmEvent changedEvent;
        [HutongGames.PlayMaker.Tooltip("Int Variable to show in the edit field."), UIHint(UIHint.Variable)]
        public FsmInt intVariable;
        [HutongGames.PlayMaker.Tooltip("Optional GUIStyle in the active GUISKin.")]
        public FsmString style;

        public override void OnGUI()
        {
            bool changed = GUI.changed;
            GUI.changed = false;
            if (!string.IsNullOrEmpty(this.style.Value))
            {
                this.intVariable.Value = int.Parse(GUILayout.TextField(this.intVariable.Value.ToString(), this.style.Value, base.LayoutOptions));
            }
            else
            {
                this.intVariable.Value = int.Parse(GUILayout.TextField(this.intVariable.Value.ToString(), base.LayoutOptions));
            }
            if (GUI.changed)
            {
                base.Fsm.Event(this.changedEvent);
                GUIUtility.ExitGUI();
            }
            else
            {
                GUI.changed = changed;
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.intVariable = null;
            this.style = string.Empty;
            this.changedEvent = null;
        }
    }
}

