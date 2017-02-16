﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Tests if a GameObject Variable has a null value. E.g., If the FindGameObject action failed to find an object."), ActionCategory(ActionCategory.Logic)]
    public class GameObjectIsNull : FsmStateAction
    {
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [Tooltip("The GameObject variable to test."), RequiredField, UIHint(UIHint.Variable)]
        public FsmGameObject gameObject;
        [Tooltip("Event to send if the GamObject is NOT null.")]
        public FsmEvent isNotNull;
        [Tooltip("Event to send if the GamObject is null.")]
        public FsmEvent isNull;
        [Tooltip("Store the result in a bool variable."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        private void DoIsGameObjectNull()
        {
            bool flag = this.gameObject.Value == null;
            if (this.storeResult != null)
            {
                this.storeResult.Value = flag;
            }
            base.Fsm.Event(!flag ? this.isNotNull : this.isNull);
        }

        public override void OnEnter()
        {
            this.DoIsGameObjectNull();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoIsGameObjectNull();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.isNull = null;
            this.isNotNull = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

