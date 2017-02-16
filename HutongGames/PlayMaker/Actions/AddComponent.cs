namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Adds a Component to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Component on exiting the state.")]
    public class AddComponent : FsmStateAction
    {
        private Component addedComponent;
        [RequiredField, Title("Component Type"), UIHint(UIHint.ScriptComponent), HutongGames.PlayMaker.Tooltip("The type of Component to add to the Game Object.")]
        public FsmString component;
        [HutongGames.PlayMaker.Tooltip("The GameObject to add the Component to."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Remove the Component when this State is exited.")]
        public FsmBool removeOnExit;
        [UIHint(UIHint.Variable), ObjectType(typeof(Component)), HutongGames.PlayMaker.Tooltip("Store the component in an Object variable. E.g., to use with Set Property.")]
        public FsmObject storeComponent;

        private void DoAddComponent()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this.addedComponent = ownerDefaultTarget.AddComponent(GetType(this.component.Value));
                this.storeComponent.Value = this.addedComponent;
                if (this.addedComponent == null)
                {
                    this.LogError("Can't add component: " + this.component.Value);
                }
            }
        }

        private static System.Type GetType(string name)
        {
            System.Type globalType = ReflectionUtils.GetGlobalType(name);
            if (globalType != null)
            {
                return globalType;
            }
            globalType = ReflectionUtils.GetGlobalType("UnityEngine." + name);
            if (globalType != null)
            {
                return globalType;
            }
            return ReflectionUtils.GetGlobalType("HutongGames.PlayMaker." + name);
        }

        public override void OnEnter()
        {
            this.DoAddComponent();
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.removeOnExit.Value && (this.addedComponent != null))
            {
                UnityEngine.Object.Destroy(this.addedComponent);
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.component = null;
            this.storeComponent = null;
        }
    }
}

