﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Destroys a Component of an Object.")]
    public class DestroyComponent : FsmStateAction
    {
        private Component aComponent;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The name of the Component to destroy."), UIHint(UIHint.ScriptComponent)]
        public FsmString component;
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the Component."), RequiredField]
        public FsmOwnerDefault gameObject;

        private void DoDestroyComponent(GameObject go)
        {
            this.aComponent = go.GetComponent(this.component.Value);
            if (this.aComponent == null)
            {
                this.LogError("No such component: " + this.component.Value);
            }
            else
            {
                UnityEngine.Object.Destroy(this.aComponent);
            }
        }

        public override void OnEnter()
        {
            this.DoDestroyComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
            base.Finish();
        }

        public override void Reset()
        {
            this.aComponent = null;
            this.gameObject = null;
            this.component = null;
        }
    }
}

