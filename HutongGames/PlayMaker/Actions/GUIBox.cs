namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("GUI Box."), ActionCategory(ActionCategory.GUI)]
    public class GUIBox : GUIContentAction
    {
        public override void OnGUI()
        {
            base.OnGUI();
            if (string.IsNullOrEmpty(base.style.Value))
            {
                GUI.Box(base.rect, base.content);
            }
            else
            {
                GUI.Box(base.rect, base.content, base.style.Value);
            }
        }
    }
}

