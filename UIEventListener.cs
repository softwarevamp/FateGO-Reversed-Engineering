using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public VectorDelegate onDrag;
    public VoidDelegate onDragEnd;
    public VoidDelegate onDragOut;
    public VoidDelegate onDragOver;
    public VoidDelegate onDragStart;
    public ObjectDelegate onDrop;
    public BoolDelegate onHover;
    public KeyCodeDelegate onKey;
    public BoolDelegate onPress;
    public FloatDelegate onScroll;
    public BoolDelegate onSelect;
    public VoidDelegate onSubmit;
    public BoolDelegate onTooltip;
    public object parameter;

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener component = go.GetComponent<UIEventListener>();
        if (component == null)
        {
            component = go.AddComponent<UIEventListener>();
        }
        return component;
    }

    private void OnClick()
    {
        if (this.onClick != null)
        {
            this.onClick(base.gameObject);
        }
    }

    private void OnDoubleClick()
    {
        if (this.onDoubleClick != null)
        {
            this.onDoubleClick(base.gameObject);
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (this.onDrag != null)
        {
            this.onDrag(base.gameObject, delta);
        }
    }

    private void OnDragEnd()
    {
        if (this.onDragEnd != null)
        {
            this.onDragEnd(base.gameObject);
        }
    }

    private void OnDragOut()
    {
        if (this.onDragOut != null)
        {
            this.onDragOut(base.gameObject);
        }
    }

    private void OnDragOver()
    {
        if (this.onDragOver != null)
        {
            this.onDragOver(base.gameObject);
        }
    }

    private void OnDragStart()
    {
        if (this.onDragStart != null)
        {
            this.onDragStart(base.gameObject);
        }
    }

    private void OnDrop(GameObject go)
    {
        if (this.onDrop != null)
        {
            this.onDrop(base.gameObject, go);
        }
    }

    private void OnHover(bool isOver)
    {
        if (this.onHover != null)
        {
            this.onHover(base.gameObject, isOver);
        }
    }

    private void OnKey(KeyCode key)
    {
        if (this.onKey != null)
        {
            this.onKey(base.gameObject, key);
        }
    }

    private void OnPress(bool isPressed)
    {
        if (this.onPress != null)
        {
            this.onPress(base.gameObject, isPressed);
        }
    }

    private void OnScroll(float delta)
    {
        if (this.onScroll != null)
        {
            this.onScroll(base.gameObject, delta);
        }
    }

    private void OnSelect(bool selected)
    {
        if (this.onSelect != null)
        {
            this.onSelect(base.gameObject, selected);
        }
    }

    private void OnSubmit()
    {
        if (this.onSubmit != null)
        {
            this.onSubmit(base.gameObject);
        }
    }

    private void OnTooltip(bool show)
    {
        if (this.onTooltip != null)
        {
            this.onTooltip(base.gameObject, show);
        }
    }

    public delegate void BoolDelegate(GameObject go, bool state);

    public delegate void FloatDelegate(GameObject go, float delta);

    public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

    public delegate void ObjectDelegate(GameObject go, GameObject obj);

    public delegate void VectorDelegate(GameObject go, Vector2 delta);

    public delegate void VoidDelegate(GameObject go);
}

