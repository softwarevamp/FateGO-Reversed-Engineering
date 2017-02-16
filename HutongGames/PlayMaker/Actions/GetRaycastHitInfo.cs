namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets info on the last Raycast and store in variables."), ActionCategory(ActionCategory.Physics)]
    public class GetRaycastHitInfo : FsmStateAction
    {
        [Tooltip("Get the distance along the ray to the hit point and store it in a variable."), UIHint(UIHint.Variable)]
        public FsmFloat distance;
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
        public FsmGameObject gameObjectHit;
        [UIHint(UIHint.Variable), Tooltip("Get the normal at the hit point and store it in a variable.")]
        public FsmVector3 normal;
        [UIHint(UIHint.Variable), Title("Hit Point"), Tooltip("Get the world position of the ray hit point and store it in a variable.")]
        public FsmVector3 point;

        public override void OnEnter()
        {
            this.StoreRaycastInfo();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.StoreRaycastInfo();
        }

        public override void Reset()
        {
            this.gameObjectHit = null;
            this.point = null;
            this.normal = null;
            this.distance = null;
            this.everyFrame = false;
        }

        private void StoreRaycastInfo()
        {
            if (base.Fsm.RaycastHitInfo.collider != null)
            {
                this.gameObjectHit.Value = base.Fsm.RaycastHitInfo.collider.gameObject;
                this.point.Value = base.Fsm.RaycastHitInfo.point;
                this.normal.Value = base.Fsm.RaycastHitInfo.normal;
                this.distance.Value = base.Fsm.RaycastHitInfo.distance;
            }
        }
    }
}

