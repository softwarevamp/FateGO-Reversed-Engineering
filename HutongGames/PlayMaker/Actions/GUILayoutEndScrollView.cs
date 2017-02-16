namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Close a group started with GUILayout Begin ScrollView."), ActionCategory(ActionCategory.GUILayout)]
    public class GUILayoutEndScrollView : FsmStateAction
    {
        public override void OnGUI()
        {
            GUILayout.EndScrollView();
        }
    }
}

