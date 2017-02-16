﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Gets a Game Object's Transform and stores it in an Object Variable."), ActionCategory(ActionCategory.GameObject)]
    public class GetTransform : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmGameObject gameObject;
        [UIHint(UIHint.Variable), RequiredField, ObjectType(typeof(Transform))]
        public FsmObject storeTransform;

        private void DoGetGameObjectName()
        {
            GameObject obj2 = this.gameObject.Value;
            this.storeTransform.Value = obj2?.transform;
        }

        public override void OnEnter()
        {
            this.DoGetGameObjectName();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetGameObjectName();
        }

        public override void Reset()
        {
            FsmGameObject obj2 = new FsmGameObject {
                UseVariable = true
            };
            this.gameObject = obj2;
            this.storeTransform = null;
            this.everyFrame = false;
        }
    }
}

