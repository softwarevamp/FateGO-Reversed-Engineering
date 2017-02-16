using System;
using UnityEngine;

[AddComponentMenu("Sample/Test2ListView/SampleEventListViewItemDraw")]
public class SampleEventListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UITexture baseImageTexture;
    [SerializeField]
    protected UILabel eventTextLabel;
    protected int eventType = -1;
    protected static string[] eventTypeSpriteList = new string[] { "ai_quest_event", "ai_quest_free" };
    [SerializeField]
    protected UISprite iconImageSprite;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetItem(SampleEventListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            int eventType = item.EventType;
            if (this.eventType != eventType)
            {
                this.iconImageSprite.spriteName = eventTypeSpriteList[eventType];
                this.eventType = eventType;
            }
            if (item.EventText != null)
            {
                this.eventTextLabel.text = item.EventText;
            }
            TweenColor component = this.iconImageSprite.gameObject.GetComponent<TweenColor>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.baseImageTexture.color = (mode != DispMode.INVALID) ? Color.white : Color.gray;
        }
        else
        {
            this.eventType = -1;
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

