namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("A Vertical Slider linked to a Float Variable."), ActionCategory(ActionCategory.GUILayout)]
    public class GUILayoutVerticalSlider : GUILayoutAction
    {
        [RequiredField]
        public FsmFloat bottomValue;
        public FsmEvent changedEvent;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmFloat floatVariable;
        [RequiredField]
        public FsmFloat topValue;

        public override void OnGUI()
        {
            bool changed = GUI.changed;
            GUI.changed = false;
            if (this.floatVariable != null)
            {
                this.floatVariable.Value = GUILayout.VerticalSlider(this.floatVariable.Value, this.topValue.Value, this.bottomValue.Value, base.LayoutOptions);
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
            this.floatVariable = null;
            this.topValue = 100f;
            this.bottomValue = 0f;
            this.changedEvent = null;
        }
    }
}

