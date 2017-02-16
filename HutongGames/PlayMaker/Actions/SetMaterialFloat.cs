namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets a named float in a game object's material."), ActionCategory(ActionCategory.Material)]
    public class SetMaterialFloat : ComponentAction<Renderer>
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is animated.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Set the parameter value."), RequiredField]
        public FsmFloat floatValue;
        [CheckForComponent(typeof(Renderer)), HutongGames.PlayMaker.Tooltip("The GameObject that the material is applied to.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
        public FsmMaterial material;
        [HutongGames.PlayMaker.Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
        public FsmInt materialIndex;
        [RequiredField, HutongGames.PlayMaker.Tooltip("A named float parameter in the shader.")]
        public FsmString namedFloat;

        private void DoSetMaterialFloat()
        {
            if (this.material.Value != null)
            {
                this.material.Value.SetFloat(this.namedFloat.Value, this.floatValue.Value);
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
                        base.renderer.material.SetFloat(this.namedFloat.Value, this.floatValue.Value);
                    }
                    else if (base.renderer.materials.Length > this.materialIndex.Value)
                    {
                        Material[] materials = base.renderer.materials;
                        materials[this.materialIndex.Value].SetFloat(this.namedFloat.Value, this.floatValue.Value);
                        base.renderer.materials = materials;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetMaterialFloat();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetMaterialFloat();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.material = null;
            this.namedFloat = string.Empty;
            this.floatValue = 0f;
            this.everyFrame = false;
        }
    }
}

