namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Color of the GUITexture attached to a Game Object."), ActionCategory(ActionCategory.GUIElement)]
    public class SetGUITextureColor : ComponentAction<GUITexture>
    {
        [RequiredField]
        public FsmColor color;
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(GUITexture))]
        public FsmOwnerDefault gameObject;

        private void DoSetGUITextureColor()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.guiTexture.color = this.color.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetGUITextureColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetGUITextureColor();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.color = Color.white;
            this.everyFrame = false;
        }
    }
}

