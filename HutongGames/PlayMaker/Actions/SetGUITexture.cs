namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUIElement), HutongGames.PlayMaker.Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
    public class SetGUITexture : ComponentAction<GUITexture>
    {
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the GUITexture."), CheckForComponent(typeof(GUITexture)), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Texture to apply.")]
        public FsmTexture texture;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.guiTexture.texture = this.texture.Value;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.texture = null;
        }
    }
}

