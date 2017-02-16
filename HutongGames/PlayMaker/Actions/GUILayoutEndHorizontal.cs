namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Close a group started with BeginHorizontal."), ActionCategory(ActionCategory.GUILayout)]
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

