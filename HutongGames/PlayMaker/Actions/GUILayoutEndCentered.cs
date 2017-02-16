﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUILayout), HutongGames.PlayMaker.Tooltip("End a centered GUILayout block started with GUILayoutBeginCentered.")]
    public class GUILayoutEndCentered : FsmStateAction
    {
        public override void OnGUI()
        {
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        public override void Reset()
        {
        }
    }
}

