namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets a Game Object's Tag and stores it in a String Variable."), ActionCategory(ActionCategory.GameObject)]
    public class GetTag : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmGameObject gameObject;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmString storeResult;

        private void DoGetTag()
        {
            if (this.gameObject.Value != null)
            {
                this.storeResult.Value = this.gameObject.Value.tag;
            }
        }

        public override void OnEnter()
        {
            this.DoGetTag();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetTag();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

