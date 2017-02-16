using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), AddComponentMenu("NGUI/Examples/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
    private bool mStarted;
    public int padBottom;
    public int padLeft;
    public int padRight;
    public int padTop;
    public Transform targetRoot;

    [ContextMenu("Execute")]
    public void Execute()
    {
        if (this.targetRoot == base.transform)
        {
            Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
        }
        else if (NGUITools.IsChild(this.targetRoot, base.transform))
        {
            Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
        }
        else
        {
            Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform.parent, this.targetRoot, false, true);
            float x = bounds.min.x + this.padLeft;
            float y = bounds.min.y + this.padBottom;
            float num3 = bounds.max.x + this.padRight;
            float num4 = bounds.max.y + this.padTop;
            base.GetComponent<UIWidget>().SetRect(x, y, num3 - x, num4 - y);
            base.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnEnable()
    {
        if (this.mStarted)
        {
            this.Execute();
        }
    }

    private void Start()
    {
        this.mStarted = true;
        this.Execute();
    }
}

