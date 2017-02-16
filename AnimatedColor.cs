using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), ExecuteInEditMode]
public class AnimatedColor : MonoBehaviour
{
    public Color color = Color.white;
    private UIWidget mWidget;

    private void LateUpdate()
    {
        this.mWidget.color = this.color;
    }

    private void OnEnable()
    {
        this.mWidget = base.GetComponent<UIWidget>();
        this.LateUpdate();
    }
}

