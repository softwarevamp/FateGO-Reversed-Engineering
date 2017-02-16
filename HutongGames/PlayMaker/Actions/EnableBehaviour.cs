namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Enables/Disables a Behaviour on a GameObject. Optionally reset the Behaviour on exit - useful if you only want the Behaviour to be active while this state is active."), ActionCategory(ActionCategory.ScriptControl)]
    public class EnableBehaviour : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The name of the Behaviour to enable/disable."), UIHint(UIHint.Behaviour)]
        public FsmString behaviour;
        [HutongGames.PlayMaker.Tooltip("Optionally drag a component directly into this field (behavior name will be ignored).")]
        public Component component;
        private Behaviour componentTarget;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Set to True to enable, False to disable.")]
        public FsmBool enable;
        [HutongGames.PlayMaker.Tooltip("The GameObject that owns the Behaviour."), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmBool resetOnExit;

        private void DoEnableBehaviour(GameObject go)
        {
            if (go != null)
            {
                if (this.component != null)
                {
                    this.componentTarget = this.component as Behaviour;
                }
                else
                {
                    this.componentTarget = go.GetComponent(this.behaviour.Value) as Behaviour;
                }
                if (this.componentTarget == null)
                {
                    this.LogWarning(" " + go.name + " missing behaviour: " + this.behaviour.Value);
                }
                else
                {
                    this.componentTarget.enabled = this.enable.Value;
                }
            }
        }

        public override string ErrorCheck()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (((ownerDefaultTarget == null) || (this.component != null)) || (this.behaviour.IsNone || string.IsNullOrEmpty(this.behaviour.Value)))
            {
                return null;
            }
            Behaviour component = ownerDefaultTarget.GetComponent(this.behaviour.Value) as Behaviour;
            return ((component == null) ? "Behaviour missing" : null);
        }

        public override void OnEnter()
        {
            this.DoEnableBehaviour(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void OnExit()
        {
            if ((this.componentTarget != null) && this.resetOnExit.Value)
            {
                this.componentTarget.enabled = !this.enable.Value;
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.behaviour = null;
            this.component = null;
            this.enable = 1;
            this.resetOnExit = 1;
        }
    }
}

