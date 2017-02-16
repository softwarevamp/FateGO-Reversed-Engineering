namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Get GameObject Vector3"), ActionCategory(ActionCategory.Vector3)]
    public class GetGameObjectVector : FsmStateAction
    {
        public FsmGameObject gameObject;
        public bool isLocal;
        [RequiredField]
        public FsmVector3 storeVector3;

        private void DoVector3()
        {
            if ((this.gameObject != null) && (this.storeVector3 != null))
            {
                if (this.isLocal)
                {
                    this.storeVector3.Value = this.gameObject.Value.transform.localPosition;
                }
                else
                {
                    this.storeVector3.Value = this.gameObject.Value.transform.position;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoVector3();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeVector3 = null;
            this.isLocal = false;
        }
    }
}

