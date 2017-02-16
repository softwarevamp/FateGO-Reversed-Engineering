namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Close a group started with BeginVertical."), ActionCategory(ActionCategory.GUILayout)]
    public class GUILayoutEndVertical : FsmStateAction
    {
        public override void OnGUI()
        {
            GUILayout.EndVertical();
        }

        public override void Reset()
        {
        }
    }
}

