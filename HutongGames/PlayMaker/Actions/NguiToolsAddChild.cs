namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Add a child to a Ngui Element."), ActionCategory("NGUI Tools")]
    public class NguiToolsAddChild : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmGameObject childInstance;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to add")]
        public FsmGameObject childReference;
        [HutongGames.PlayMaker.Tooltip("The Parent"), RequiredField]
        public FsmOwnerDefault parent;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.parent);
            this.childInstance.Value = NGUITools.AddChild(ownerDefaultTarget, this.childReference.Value);
            UITable table = NGUITools.FindInParents<UITable>(this.childInstance.Value);
            if (table != null)
            {
                table.repositionNow = true;
            }
            UIGrid grid = NGUITools.FindInParents<UIGrid>(this.childInstance.Value);
            if (grid != null)
            {
                grid.repositionNow = true;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.parent = null;
            this.childReference = null;
            this.childInstance = null;
        }
    }
}

