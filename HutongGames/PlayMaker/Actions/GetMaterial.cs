namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get a material at index on a gameObject and store it in a variable"), ActionCategory(ActionCategory.Material)]
    public class GetMaterial : ComponentAction<Renderer>
    {
        [HutongGames.PlayMaker.Tooltip("The GameObject the Material is applied to."), RequiredField, CheckForComponent(typeof(Renderer))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
        public bool getSharedMaterial;
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the material in a variable.")]
        public FsmMaterial material;
        [HutongGames.PlayMaker.Tooltip("The index of the Material in the Materials array.")]
        public FsmInt materialIndex;

        private void DoGetMaterial()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                if ((this.materialIndex.Value == 0) && !this.getSharedMaterial)
                {
                    this.material.Value = base.renderer.material;
                }
                else if ((this.materialIndex.Value == 0) && this.getSharedMaterial)
                {
                    this.material.Value = base.renderer.sharedMaterial;
                }
                else if ((base.renderer.materials.Length > this.materialIndex.Value) && !this.getSharedMaterial)
                {
                    Material[] materials = base.renderer.materials;
                    this.material.Value = materials[this.materialIndex.Value];
                    base.renderer.materials = materials;
                }
                else if ((base.renderer.materials.Length > this.materialIndex.Value) && this.getSharedMaterial)
                {
                    Material[] sharedMaterials = base.renderer.sharedMaterials;
                    this.material.Value = sharedMaterials[this.materialIndex.Value];
                    base.renderer.sharedMaterials = sharedMaterials;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetMaterial();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.material = null;
            this.materialIndex = 0;
            this.getSharedMaterial = false;
        }
    }
}

