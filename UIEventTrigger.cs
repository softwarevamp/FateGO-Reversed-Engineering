using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
    public static UIEventTrigger current;
    public List<EventDelegate> onClick = new List<EventDelegate>();
    public List<EventDelegate> onDeselect = new List<EventDelegate>();
    public List<EventDelegate> onDoubleClick = new List<EventDelegate>();
    public List<EventDelegate> onDrag = new List<EventDelegate>();
    public List<EventDelegate> onDragEnd = new List<EventDelegate>();
    public List<EventDelegate> onDragOut = new List<EventDelegate>();
    public List<EventDelegate> onDragOver = new List<EventDelegate>();
    public List<EventDelegate> onDragStart = new List<EventDelegate>();
    public List<EventDelegate> onHoverOut = new List<EventDelegate>();
    public List<EventDelegate> onHoverOver = new List<EventDelegate>();
    public List<EventDelegate> onPress = new List<EventDelegate>();
    public List<EventDelegate> onRelease = new List<EventDelegate>();
    public List<EventDelegate> onSelect = new List<EventDelegate>();

    private void OnClick()
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onClick);
            current = null;
        }
    }

    private void OnDoubleClick()
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDoubleClick);
            current = null;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDrag);
            current = null;
        }
    }

    private void OnDragEnd()
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDragEnd);
            current = null;
        }
    }

    private void OnDragOut(GameObject go)
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDragOut);
            current = null;
        }
    }

    private void OnDragOver(GameObject go)
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDragOver);
            current = null;
        }
    }

    private void OnDragStart()
    {
        if (current == null)
        {
            current = this;
            EventDelegate.Execute(this.onDragStart);
            current = null;
        }
    }

    private void OnHover(bool isOver)
    {
        if (current == null)
        {
            current = this;
            if (isOver)
            {
                EventDelegate.Execute(this.onHoverOver);
            }
            else
            {
                EventDelegate.Execute(this.onHoverOut);
            }
            current = null;
        }
    }

    private void OnPress(bool pressed)
    {
        if (current == null)
        {
            current = this;
            if (pressed)
            {
                EventDelegate.Execute(this.onPress);
            }
            else
            {
                EventDelegate.Execute(this.onRelease);
            }
            current = null;
        }
    }

    private void OnSelect(bool selected)
    {
        if (current == null)
        {
            current = this;
            if (selected)
            {
                EventDelegate.Execute(this.onSelect);
            }
            else
            {
                EventDelegate.Execute(this.onDeselect);
            }
            current = null;
        }
    }
}

