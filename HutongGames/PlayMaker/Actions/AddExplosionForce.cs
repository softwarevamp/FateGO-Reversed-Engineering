﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Applies a force to a Game Object that simulates explosion effects. The explosion force will fall off linearly with distance. Hint: Use the Explosion Action instead to apply an explosion force to all objects in a blast radius.")]
    public class AddExplosionForce : ComponentAction<Rigidbody>
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("The center of the explosion. Hint: this is often the position returned from a GetCollisionInfo action.")]
        public FsmVector3 center;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("The strength of the explosion."), RequiredField]
        public FsmFloat force;
        [HutongGames.PlayMaker.Tooltip("The type of force to apply. See Unity Physics docs.")]
        public ForceMode forceMode;
        [RequiredField, CheckForComponent(typeof(Rigidbody)), HutongGames.PlayMaker.Tooltip("The GameObject to add the explosion force to.")]
        public FsmOwnerDefault gameObject;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The radius of the explosion. Force falls off linearly with distance.")]
        public FsmFloat radius;
        [HutongGames.PlayMaker.Tooltip("Applies the force as if it was applied from beneath the object. This is useful since explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
        public FsmFloat upwardsModifier;

        public override void Awake()
        {
            base.Fsm.HandleFixedUpdate = true;
        }

        private void DoAddExplosionForce()
        {
            GameObject go = (this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner;
            if ((this.center != null) && base.UpdateCache(go))
            {
                base.rigidbody.AddExplosionForce(this.force.Value, this.center.Value, this.radius.Value, this.upwardsModifier.Value, this.forceMode);
            }
        }

        public override void OnEnter()
        {
            this.DoAddExplosionForce();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            this.DoAddExplosionForce();
        }

        public override void Reset()
        {
            this.gameObject = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.center = vector;
            this.upwardsModifier = 0f;
            this.forceMode = ForceMode.Force;
            this.everyFrame = false;
        }
    }
}

