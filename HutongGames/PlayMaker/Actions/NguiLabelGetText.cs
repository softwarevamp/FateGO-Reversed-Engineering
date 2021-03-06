﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("NGUI"), HutongGames.PlayMaker.Tooltip("Gets the text of a Ngui Label")]
    public class NguiLabelGetText : FsmStateAction
    {
        private UILabel _label;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active. Useful to get the text over time")]
        public bool everyFrame;
        [CheckForComponent(typeof(UILabel)), HutongGames.PlayMaker.Tooltip("The GameObject on which there is a UILabel"), RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The label"), RequiredField]
        public FsmString text;

        private void GetText()
        {
            this.text.Value = this._label.text;
        }

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
                    this.GetText();
                    if (!this.everyFrame)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.GetText();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.text = null;
        }
    }
}

