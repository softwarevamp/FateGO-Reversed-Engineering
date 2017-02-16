﻿using System;
using UnityEngine;

[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alpha = 1f;
    private UIPanel mPanel;
    private UIWidget mWidget;

    private void LateUpdate()
    {
        if (this.mWidget != null)
        {
            this.mWidget.alpha = this.alpha;
        }
        if (this.mPanel != null)
        {
            this.mPanel.alpha = this.alpha;
        }
    }

    private void OnEnable()
    {
        this.mWidget = base.GetComponent<UIWidget>();
        this.mPanel = base.GetComponent<UIPanel>();
        this.LateUpdate();
    }
}

