namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the material on a game object."), ActionCategory(ActionCategory.Material)]
    public class SetMaterial : ComponentAction<Renderer>
    {
        [CheckForComponent(typeof(Renderer)), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmMaterial material;
        public FsmInt materialIndex;

        private void DoSetMaterial()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                if (this.materialIndex.Value == 0)
                {
                    base.renderer.material = this.material.Value;
                }
                else if (base.renderer.materials.Length > this.materialIndex.Value)
                {
                    Material[] materials = base.renderer.materials;
                    materials[this.materialIndex.Value] = this.material.Value;
                    base.renderer.materials = materials;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetMaterial();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.material = null;
            this.materialIndex = 0;
        }
    }
}

