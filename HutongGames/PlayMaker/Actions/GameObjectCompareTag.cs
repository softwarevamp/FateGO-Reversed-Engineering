﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Logic), Tooltip("Tests if a Game Object has a tag.")]
    public class GameObjectCompareTag : FsmStateAction
    {
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [Tooltip("Event to send if the GameObject does not have the Tag.")]
        public FsmEvent falseEvent;
        [RequiredField, Tooltip("The GameObject to test.")]
        public FsmGameObject gameObject;
        [Tooltip("Store the result in a Bool variable."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        [UIHint(UIHint.Tag), Tooltip("The Tag to check for."), RequiredField]
        public FsmString tag;
        [Tooltip("Event to send if the GameObject has the Tag.")]
        public FsmEvent trueEvent;

        private void DoCompareTag()
        {
            bool flag = false;
            if (this.gameObject.Value != null)
            {
                flag = this.gameObject.Value.CompareTag(this.tag.Value);
            }
            this.storeResult.Value = flag;
            base.Fsm.Event(!flag ? this.falseEvent : this.trueEvent);
        }

        public override void OnEnter()
        {
            this.DoCompareTag();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoCompareTag();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.tag = "Untagged";
            this.trueEvent = null;
            this.falseEvent = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

