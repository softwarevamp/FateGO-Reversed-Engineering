namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Text used by the GUIText Component attached to a Game Object."), ActionCategory(ActionCategory.GUIElement)]
    public class SetGUIText : ComponentAction<GUIText>
    {
        public bool everyFrame;
        [CheckForComponent(typeof(GUIText)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmString text;

        private void DoSetGUIText()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.guiText.text = this.text.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetGUIText();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetGUIText();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.text = string.Empty;
        }
    }
}

