namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Character), HutongGames.PlayMaker.Tooltip("Gets the Collision Flags from a Character Controller on a Game Object. Collision flags give you a broad overview of where the character collided with any other object.")]
    public class GetControllerCollisionFlags : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("True if the Character Controller capsule was hit from above."), UIHint(UIHint.Variable)]
        public FsmBool above;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("True if the Character Controller capsule was hit from below.")]
        public FsmBool below;
        private CharacterController controller;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject with a Character Controller component."), CheckForComponent(typeof(CharacterController))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("True if the Character Controller capsule is on the ground"), UIHint(UIHint.Variable)]
        public FsmBool isGrounded;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("True if no collisions in last move.")]
        public FsmBool none;
        private GameObject previousGo;
        [HutongGames.PlayMaker.Tooltip("True if the Character Controller capsule was hit on the sides."), UIHint(UIHint.Variable)]
        public FsmBool sides;

        public override void OnUpdate()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget != this.previousGo)
                {
                    this.controller = ownerDefaultTarget.GetComponent<CharacterController>();
                    this.previousGo = ownerDefaultTarget;
                }
                if (this.controller != null)
                {
                    this.isGrounded.Value = this.controller.isGrounded;
                    CollisionFlags collisionFlags = this.controller.collisionFlags;
                    this.none.Value = false;
                    this.sides.Value = (this.controller.collisionFlags & CollisionFlags.Sides) != CollisionFlags.None;
                    this.above.Value = (this.controller.collisionFlags & CollisionFlags.Above) != CollisionFlags.None;
                    this.below.Value = (this.controller.collisionFlags & CollisionFlags.Below) != CollisionFlags.None;
                }
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.isGrounded = null;
            this.none = null;
            this.sides = null;
            this.above = null;
            this.below = null;
        }
    }
}

