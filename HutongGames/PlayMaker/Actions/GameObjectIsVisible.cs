﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Tests if a Game Object is visible."), ActionCategory(ActionCategory.Logic)]
    public class GameObjectIsVisible : ComponentAction<Renderer>
    {
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Event to send if the GameObject is NOT visible.")]
        public FsmEvent falseEvent;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to test."), CheckForComponent(typeof(Renderer))]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Store the result in a bool variable."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        [HutongGames.PlayMaker.Tooltip("Event to send if the GameObject is visible.")]
        public FsmEvent trueEvent;

        private void DoIsVisible()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                bool isVisible = base.renderer.isVisible;
                this.storeResult.Value = isVisible;
                base.Fsm.Event(!isVisible ? this.falseEvent : this.trueEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoIsVisible();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoIsVisible();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

