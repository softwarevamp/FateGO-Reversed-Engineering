namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), HutongGames.PlayMaker.Tooltip("Gets the pressed state of a Key.")]
    public class GetKey : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if you're waiting for a key press/release.")]
        public bool everyFrame;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The key to test.")]
        public KeyCode key;
        [HutongGames.PlayMaker.Tooltip("Store if the key is down (True) or up (False)."), RequiredField, UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        private void DoGetKey()
        {
            this.storeResult.Value = Input.GetKey(this.key);
        }

        public override void OnEnter()
        {
            this.DoGetKey();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetKey();
        }

        public override void Reset()
        {
            this.key = KeyCode.None;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

