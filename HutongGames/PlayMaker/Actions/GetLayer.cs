namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.GameObject), Tooltip("Gets a Game Object's Layer and stores it in an Int Variable.")]
    public class GetLayer : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmGameObject gameObject;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmInt storeResult;

        private void DoGetLayer()
        {
            if (this.gameObject.Value != null)
            {
                this.storeResult.Value = this.gameObject.Value.layer;
            }
        }

        public override void OnEnter()
        {
            this.DoGetLayer();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetLayer();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

