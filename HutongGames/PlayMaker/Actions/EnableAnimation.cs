﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Animation), HutongGames.PlayMaker.Tooltip("Enables/Disables an Animation on a GameObject.\nAnimation time is paused while disabled. Animation must also have a non zero weight to play.")]
    public class EnableAnimation : FsmStateAction
    {
        private AnimationState anim;
        [UIHint(UIHint.Animation), HutongGames.PlayMaker.Tooltip("The name of the animation to enable/disable."), RequiredField]
        public FsmString animName;
        [HutongGames.PlayMaker.Tooltip("Set to True to enable, False to disable."), RequiredField]
        public FsmBool enable;
        [RequiredField, CheckForComponent(typeof(Animation)), HutongGames.PlayMaker.Tooltip("The GameObject playing the animation.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Reset the initial enabled state when exiting the state.")]
        public FsmBool resetOnExit;

        private void DoEnableAnimation(GameObject go)
        {
            if (go != null)
            {
                Animation component = go.GetComponent<Animation>();
                if (component == null)
                {
                    this.LogError("Missing animation component!");
                }
                else
                {
                    this.anim = component[this.animName.Value];
                    if (this.anim != null)
                    {
                        this.anim.enabled = this.enable.Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoEnableAnimation(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.resetOnExit.Value && (this.anim != null))
            {
                this.anim.enabled = !this.enable.Value;
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
            this.enable = 1;
            this.resetOnExit = 0;
        }
    }
}

