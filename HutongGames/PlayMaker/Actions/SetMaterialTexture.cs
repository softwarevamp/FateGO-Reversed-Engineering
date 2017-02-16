﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Material), HutongGames.PlayMaker.Tooltip("Sets a named texture in a game object's material.")]
    public class SetMaterialTexture : ComponentAction<Renderer>
    {
        [CheckForComponent(typeof(Renderer)), HutongGames.PlayMaker.Tooltip("The GameObject that the material is applied to.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
        public FsmMaterial material;
        [HutongGames.PlayMaker.Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
        public FsmInt materialIndex;
        [HutongGames.PlayMaker.Tooltip("A named parameter in the shader."), UIHint(UIHint.NamedTexture)]
        public FsmString namedTexture;
        public FsmTexture texture;

        private void DoSetMaterialTexture()
        {
            string propertyName = this.namedTexture.Value;
            if (propertyName == string.Empty)
            {
                propertyName = "_MainTex";
            }
            if (this.material.Value != null)
            {
                this.material.Value.SetTexture(propertyName, this.texture.Value);
            }
            else
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (base.UpdateCache(ownerDefaultTarget))
                {
                    if (base.renderer.material == null)
                    {
                        this.LogError("Missing Material!");
                    }
                    else if (this.materialIndex.Value == 0)
                    {
                        base.renderer.material.SetTexture(propertyName, this.texture.Value);
                    }
                    else if (base.renderer.materials.Length > this.materialIndex.Value)
                    {
                        Material[] materials = base.renderer.materials;
                        materials[this.materialIndex.Value].SetTexture(propertyName, this.texture.Value);
                        base.renderer.materials = materials;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetMaterialTexture();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.material = null;
            this.namedTexture = "_MainTex";
            this.texture = null;
        }
    }
}

