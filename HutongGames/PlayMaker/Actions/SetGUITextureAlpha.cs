namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUIElement), HutongGames.PlayMaker.Tooltip("Sets the Alpha of the GUITexture attached to a Game Object. Useful for fading GUI elements in/out.")]
    public class SetGUITextureAlpha : ComponentAction<GUITexture>
    {
        [RequiredField]
        public FsmFloat alpha;
        public bool everyFrame;
        [CheckForComponent(typeof(GUITexture)), RequiredField]
        public FsmOwnerDefault gameObject;

        private void DoGUITextureAlpha()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                Color color = base.guiTexture.color;
                base.guiTexture.color = new Color(color.r, color.g, color.b, this.alpha.Value);
            }
        }

        public override void OnEnter()
        {
            this.DoGUITextureAlpha();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGUITextureAlpha();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.alpha = 1f;
            this.everyFrame = false;
        }
    }
}

