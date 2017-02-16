namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Material), HutongGames.PlayMaker.Tooltip("Get a texture from a material on a GameObject")]
    public class GetMaterialTexture : ComponentAction<Renderer>
    {
        [HutongGames.PlayMaker.Tooltip("The GameObject the Material is applied to."), CheckForComponent(typeof(Renderer)), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Get the shared version of the texture.")]
        public bool getFromSharedMaterial;
        [HutongGames.PlayMaker.Tooltip("The index of the Material in the Materials array.")]
        public FsmInt materialIndex;
        [UIHint(UIHint.NamedTexture), HutongGames.PlayMaker.Tooltip("The texture to get. See Unity Shader docs for names.")]
        public FsmString namedTexture;
        [HutongGames.PlayMaker.Tooltip("Store the texture in a variable."), RequiredField, UIHint(UIHint.Variable), Title("StoreTexture")]
        public FsmTexture storedTexture;

        private void DoGetMaterialTexture()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                string propertyName = this.namedTexture.Value;
                if (propertyName == string.Empty)
                {
                    propertyName = "_MainTex";
                }
                if ((this.materialIndex.Value == 0) && !this.getFromSharedMaterial)
                {
                    this.storedTexture.Value = base.renderer.material.GetTexture(propertyName);
                }
                else if ((this.materialIndex.Value == 0) && this.getFromSharedMaterial)
                {
                    this.storedTexture.Value = base.renderer.sharedMaterial.GetTexture(propertyName);
                }
                else if ((base.renderer.materials.Length > this.materialIndex.Value) && !this.getFromSharedMaterial)
                {
                    Material[] materials = base.renderer.materials;
                    this.storedTexture.Value = base.renderer.materials[this.materialIndex.Value].GetTexture(propertyName);
                    base.renderer.materials = materials;
                }
                else if ((base.renderer.materials.Length > this.materialIndex.Value) && this.getFromSharedMaterial)
                {
                    Material[] sharedMaterials = base.renderer.sharedMaterials;
                    this.storedTexture.Value = base.renderer.sharedMaterials[this.materialIndex.Value].GetTexture(propertyName);
                    base.renderer.materials = sharedMaterials;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetMaterialTexture();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.namedTexture = "_MainTex";
            this.storedTexture = null;
            this.getFromSharedMaterial = false;
        }
    }
}

