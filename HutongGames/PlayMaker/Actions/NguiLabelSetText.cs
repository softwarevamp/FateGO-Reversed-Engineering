namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the text of a Ngui Label"), ActionCategory("NGUI")]
    public class NguiLabelSetText : FsmStateAction
    {
        private UILabel _label;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active. Useful to change the text over time")]
        public bool everyFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject on which there is a UILabel"), CheckForComponent(typeof(UILabel))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("The label")]
        public FsmString text;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget == null)
            {
                this.LogWarning("no gameObject");
                base.Finish();
            }
            else
            {
                this._label = ownerDefaultTarget.GetComponent<UILabel>();
                if (this._label == null)
                {
                    this.LogWarning("no UILabel");
                    base.Finish();
                }
                else
                {
                    this.SetText();
                    if (!this.everyFrame)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.SetText();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.text = null;
        }

        private void SetText()
        {
            this._label.text = this.text.Value;
        }
    }
}

