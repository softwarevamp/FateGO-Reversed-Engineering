namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Inserts a space in the current layout group."), ActionCategory(ActionCategory.GUILayout)]
    public class GUILayoutSpace : FsmStateAction
    {
        public FsmFloat space;

        public override void OnGUI()
        {
            GUILayout.Space(this.space.Value);
        }

        public override void Reset()
        {
            this.space = 10f;
        }
    }
}

