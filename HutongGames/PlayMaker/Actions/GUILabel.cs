namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("GUI Label."), ActionCategory(ActionCategory.GUI)]
    public class GUILabel : GUIContentAction
    {
        public override void OnGUI()
        {
            base.OnGUI();
            if (string.IsNullOrEmpty(base.style.Value))
            {
                GUI.Label(base.rect, base.content);
            }
            else
            {
                GUI.Label(base.rect, base.content, base.style.Value);
            }
        }
    }
}

