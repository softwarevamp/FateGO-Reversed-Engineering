namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets a Game Object's material randomly from an array of Materials."), ActionCategory(ActionCategory.Material)]
    public class SetRandomMaterial : ComponentAction<Renderer>
    {
        [RequiredField, CheckForComponent(typeof(Renderer))]
        public FsmOwnerDefault gameObject;
        public FsmInt materialIndex;
        public FsmMaterial[] materials;

        private void DoSetRandomMaterial()
        {
            if ((this.materials != null) && (this.materials.Length != 0))
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
                        base.renderer.material = this.materials[UnityEngine.Random.Range(0, this.materials.Length)].Value;
                    }
                    else if (base.renderer.materials.Length > this.materialIndex.Value)
                    {
                        Material[] materials = base.renderer.materials;
                        materials[this.materialIndex.Value] = this.materials[UnityEngine.Random.Range(0, this.materials.Length)].Value;
                        base.renderer.materials = materials;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetRandomMaterial();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.materials = new FsmMaterial[3];
        }
    }
}

