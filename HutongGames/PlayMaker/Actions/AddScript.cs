namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.ScriptControl), HutongGames.PlayMaker.Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
    public class AddScript : FsmStateAction
    {
        private Component addedComponent;
        [HutongGames.PlayMaker.Tooltip("The GameObject to add the script to."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Remove the script from the GameObject when this State is exited.")]
        public FsmBool removeOnExit;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The Script to add to the GameObject."), UIHint(UIHint.ScriptComponent)]
        public FsmString script;

        private void DoAddComponent(GameObject go)
        {
            if (go != null)
            {
                this.addedComponent = go.AddComponent(GetType(this.script.Value));
                if (this.addedComponent == null)
                {
                    this.LogError("Can't add script: " + this.script.Value);
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
            this.DoAddComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
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
            this.script = null;
        }
    }
}

