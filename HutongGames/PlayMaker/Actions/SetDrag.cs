namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=4734.0"), ActionCategory(ActionCategory.Physics), HutongGames.PlayMaker.Tooltip("Sets the Drag of a Game Object's Rigid Body.")]
    public class SetDrag : ComponentAction<Rigidbody>
    {
        [RequiredField, HasFloatSlider(0f, 10f)]
        public FsmFloat drag;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Typically this would be set to True.")]
        public bool everyFrame;
        [CheckForComponent(typeof(Rigidbody)), RequiredField]
        public FsmOwnerDefault gameObject;

        private void DoSetDrag()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.rigidbody.drag = this.drag.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetDrag();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetDrag();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.drag = 1f;
        }
    }
}

