using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour
{
    private void OnClick()
    {
        UICenterOnChild child = NGUITools.FindInParents<UICenterOnChild>(base.gameObject);
        UIPanel panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        if (child != null)
        {
            if (child.enabled)
            {
                child.CenterOn(base.transform);
            }
        }
        else if ((panel != null) && (panel.clipping != UIDrawCall.Clipping.None))
        {
            UIScrollView component = panel.GetComponent<UIScrollView>();
            Vector3 pos = -panel.cachedTransform.InverseTransformPoint(base.transform.position);
            if (!component.canMoveHorizontally)
            {
                pos.x = panel.cachedTransform.localPosition.x;
            }
            if (!component.canMoveVertically)
            {
                pos.y = panel.cachedTransform.localPosition.y;
            }
            SpringPanel.Begin(panel.cachedGameObject, pos, 6f);
        }
    }
}

