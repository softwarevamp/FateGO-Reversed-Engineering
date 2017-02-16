namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Level), HutongGames.PlayMaker.Tooltip("Makes the Game Object not be destroyed automatically when loading a new scene.")]
    public class DontDestroyOnLoad : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("GameObject to mark as DontDestroyOnLoad."), RequiredField]
        public FsmOwnerDefault gameObject;

        public override void OnEnter()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.Owner.transform.root.gameObject);
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

