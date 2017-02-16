namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Animation), HutongGames.PlayMaker.Tooltip("Sets the Speed of an Animation. Check Every Frame to update the animation time continuosly, e.g., if you're manipulating a variable that controls animation speed.")]
    public class SetAnimationSpeed : ComponentAction<Animation>
    {
        [UIHint(UIHint.Animation), RequiredField]
        public FsmString animName;
        public bool everyFrame;
        [CheckForComponent(typeof(Animation)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmFloat speed = 1f;

        private void DoSetAnimationSpeed(GameObject go)
        {
            if (base.UpdateCache(go))
            {
                AnimationState state = base.animation[this.animName.Value];
                if (state == null)
                {
                    this.LogWarning("Missing animation: " + this.animName.Value);
                }
                else
                {
                    state.speed = this.speed.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetAnimationSpeed((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetAnimationSpeed((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
            this.speed = 1f;
            this.everyFrame = false;
        }
    }
}

