namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUILayout), HutongGames.PlayMaker.Tooltip("Close a group started with BeginHorizontal.")]
    public class GUILayoutEndHorizontal : FsmStateAction
    {
        public override void OnGUI()
        {
            GUILayout.EndHorizontal();
        }

        public override void Reset()
        {
        }
    }
}

