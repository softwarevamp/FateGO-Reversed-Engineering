namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets a named color value in a game object's material."), ActionCategory(ActionCategory.Material)]
    public class SetMaterialColor : ComponentAction<Renderer>
    {
        [HutongGames.PlayMaker.Tooltip("Set the parameter value."), RequiredField]
        public FsmColor color;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if the value is animated.")]
        public bool everyFrame;
        [CheckForComponent(typeof(Renderer)), HutongGames.PlayMaker.Tooltip("The GameObject that the material is applied to.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
        public FsmMaterial material;
        [HutongGames.PlayMaker.Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
        public FsmInt materialIndex;
        [HutongGames.PlayMaker.Tooltip("A named color parameter in the shader."), UIHint(UIHint.NamedColor)]
        public FsmString namedColor;

        private void DoSetMaterialColor()
        {
            if (!this.color.IsNone)
            {
                string propertyName = this.namedColor.Value;
                if (propertyName == string.Empty)
                {
                    propertyName = "_Color";
                }
                if (this.material.Value != null)
                {
                    this.material.Value.SetColor(propertyName, this.color.Value);
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
                            base.renderer.material.SetColor(propertyName, this.color.Value);
                        }
                        else if (base.renderer.materials.Length > this.materialIndex.Value)
                        {
                            Material[] materials = base.renderer.materials;
                            materials[this.materialIndex.Value].SetColor(propertyName, this.color.Value);
                            base.renderer.materials = materials;
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetMaterialColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetMaterialColor();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.material = null;
            this.namedColor = "_Color";
            this.color = Color.black;
            this.everyFrame = false;
        }
    }
}

