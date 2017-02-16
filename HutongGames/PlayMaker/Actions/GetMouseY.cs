namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), HutongGames.PlayMaker.Tooltip("Gets the Y Position of the mouse and stores it in a Float Variable.")]
    public class GetMouseY : FsmStateAction
    {
        public bool normalize;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmFloat storeResult;

        private void DoGetMouseY()
        {
            if (this.storeResult != null)
            {
                float y = Input.mousePosition.y;
                if (this.normalize)
                {
                    y /= (float) Screen.height;
                }
                this.storeResult.Value = y;
            }
        }

        public override void OnEnter()
        {
            this.DoGetMouseY();
        }

        public override void OnUpdate()
        {
            this.DoGetMouseY();
        }

        public override void Reset()
        {
            this.storeResult = null;
            this.normalize = true;
        }
    }
}

