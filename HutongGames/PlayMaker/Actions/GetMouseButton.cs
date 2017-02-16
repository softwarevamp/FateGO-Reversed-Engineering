namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), HutongGames.PlayMaker.Tooltip("Gets the pressed state of the specified Mouse Button and stores it in a Bool Variable. See Unity Input Manager doc.")]
    public class GetMouseButton : FsmStateAction
    {
        [RequiredField]
        public MouseButton button;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        public override void OnUpdate()
        {
            this.storeResult.Value = Input.GetMouseButton((int) this.button);
        }

        public override void Reset()
        {
            this.button = MouseButton.Left;
            this.storeResult = null;
        }
    }
}

