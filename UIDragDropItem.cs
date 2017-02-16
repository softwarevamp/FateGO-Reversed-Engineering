using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
    public bool cloneOnDrag;
    public bool interactable = true;
    [NonSerialized]
    protected UIButton mButton;
    [NonSerialized]
    protected Collider mCollider;
    [NonSerialized]
    protected Collider2D mCollider2D;
    [NonSerialized]
    protected bool mDragging;
    [NonSerialized]
    protected UIDragScrollView mDragScrollView;
    [NonSerialized]
    protected float mDragStartTime;
    [NonSerialized]
    protected UIGrid mGrid;
    [NonSerialized]
    protected Transform mParent;
    [NonSerialized]
    protected bool mPressed;
    [NonSerialized]
    protected UIRoot mRoot;
    [NonSerialized]
    protected UITable mTable;
    [NonSerialized]
    protected UICamera.MouseOrTouch mTouch;
    [NonSerialized]
    protected Transform mTrans;
    [HideInInspector]
    public float pressAndHoldDelay = 1f;
    public Restriction restriction;

    [DebuggerHidden]
    protected IEnumerator EnableDragScrollView() => 
        new <EnableDragScrollView>c__Iterator3D { <>f__this = this };

    protected virtual void OnDrag(Vector2 delta)
    {
        if (this.interactable && ((this.mDragging && base.enabled) && (this.mTouch == UICamera.currentTouch)))
        {
            this.OnDragDropMove((Vector2) (delta * this.mRoot.pixelSizeAdjustment));
        }
    }

    protected virtual void OnDragDropEnd()
    {
    }

    protected virtual void OnDragDropMove(Vector2 delta)
    {
        this.mTrans.localPosition += delta;
    }

    protected virtual void OnDragDropRelease(GameObject surface)
    {
        if (!this.cloneOnDrag)
        {
            if (this.mButton != null)
            {
                this.mButton.isEnabled = true;
            }
            else if (this.mCollider != null)
            {
                this.mCollider.enabled = true;
            }
            else if (this.mCollider2D != null)
            {
                this.mCollider2D.enabled = true;
            }
            UIDragDropContainer container = (surface == null) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
            if (container != null)
            {
                this.mTrans.parent = (container.reparentTarget == null) ? container.transform : container.reparentTarget;
                Vector3 localPosition = this.mTrans.localPosition;
                localPosition.z = 0f;
                this.mTrans.localPosition = localPosition;
            }
            else
            {
                this.mTrans.parent = this.mParent;
            }
            this.mParent = this.mTrans.parent;
            this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
            this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
            if (this.mDragScrollView != null)
            {
                base.StartCoroutine(this.EnableDragScrollView());
            }
            NGUITools.MarkParentAsChanged(base.gameObject);
            if (this.mTable != null)
            {
                this.mTable.repositionNow = true;
            }
            if (this.mGrid != null)
            {
                this.mGrid.repositionNow = true;
            }
            this.OnDragDropEnd();
        }
        else
        {
            NGUITools.Destroy(base.gameObject);
        }
    }

    protected virtual void OnDragDropStart()
    {
        if (this.mDragScrollView != null)
        {
            this.mDragScrollView.enabled = false;
        }
        if (this.mButton != null)
        {
            this.mButton.isEnabled = false;
        }
        else if (this.mCollider != null)
        {
            this.mCollider.enabled = false;
        }
        else if (this.mCollider2D != null)
        {
            this.mCollider2D.enabled = false;
        }
        this.mParent = this.mTrans.parent;
        this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
        this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
        this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
        if (UIDragDropRoot.root != null)
        {
            this.mTrans.parent = UIDragDropRoot.root;
        }
        Vector3 localPosition = this.mTrans.localPosition;
        localPosition.z = 0f;
        this.mTrans.localPosition = localPosition;
        TweenPosition component = base.GetComponent<TweenPosition>();
        if (component != null)
        {
            component.enabled = false;
        }
        SpringPosition position2 = base.GetComponent<SpringPosition>();
        if (position2 != null)
        {
            position2.enabled = false;
        }
        NGUITools.MarkParentAsChanged(base.gameObject);
        if (this.mTable != null)
        {
            this.mTable.repositionNow = true;
        }
        if (this.mGrid != null)
        {
            this.mGrid.repositionNow = true;
        }
    }

    protected virtual void OnDragEnd()
    {
        if (this.interactable && (base.enabled && (this.mTouch == UICamera.currentTouch)))
        {
            this.StopDragging(UICamera.hoveredObject);
        }
    }

    protected virtual void OnDragStart()
    {
        if (this.interactable && (base.enabled && (this.mTouch == UICamera.currentTouch)))
        {
            if (this.restriction != Restriction.None)
            {
                if (this.restriction == Restriction.Horizontal)
                {
                    Vector2 totalDelta = this.mTouch.totalDelta;
                    float introduced2 = Mathf.Abs(totalDelta.x);
                    if (introduced2 < Mathf.Abs(totalDelta.y))
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.Vertical)
                {
                    Vector2 vector2 = this.mTouch.totalDelta;
                    float introduced3 = Mathf.Abs(vector2.x);
                    if (introduced3 > Mathf.Abs(vector2.y))
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.PressAndHold)
                {
                    return;
                }
            }
            this.StartDragging();
        }
    }

    protected virtual void OnPress(bool isPressed)
    {
        if (this.interactable)
        {
            if (isPressed)
            {
                this.mTouch = UICamera.currentTouch;
                this.mDragStartTime = RealTime.time + this.pressAndHoldDelay;
                this.mPressed = true;
            }
            else
            {
                this.mPressed = false;
                this.mTouch = null;
            }
        }
    }

    protected virtual void Start()
    {
        this.mTrans = base.transform;
        this.mCollider = base.gameObject.GetComponent<Collider>();
        this.mCollider2D = base.gameObject.GetComponent<Collider2D>();
        this.mButton = base.GetComponent<UIButton>();
        this.mDragScrollView = base.GetComponent<UIDragScrollView>();
    }

    protected virtual void StartDragging()
    {
        if (this.interactable && !this.mDragging)
        {
            if (this.cloneOnDrag)
            {
                this.mPressed = false;
                GameObject obj2 = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
                obj2.transform.localPosition = base.transform.localPosition;
                obj2.transform.localRotation = base.transform.localRotation;
                obj2.transform.localScale = base.transform.localScale;
                UIButtonColor component = obj2.GetComponent<UIButtonColor>();
                if (component != null)
                {
                    component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
                }
                if ((this.mTouch != null) && (this.mTouch.pressed == base.gameObject))
                {
                    this.mTouch.current = obj2;
                    this.mTouch.pressed = obj2;
                    this.mTouch.dragged = obj2;
                    this.mTouch.last = obj2;
                }
                UIDragDropItem item = obj2.GetComponent<UIDragDropItem>();
                item.mTouch = this.mTouch;
                item.mPressed = true;
                item.mDragging = true;
                item.Start();
                item.OnDragDropStart();
                if (UICamera.currentTouch == null)
                {
                    UICamera.currentTouch = this.mTouch;
                }
                this.mTouch = null;
                UICamera.Notify(base.gameObject, "OnPress", false);
                UICamera.Notify(base.gameObject, "OnHover", false);
            }
            else
            {
                this.mDragging = true;
                this.OnDragDropStart();
            }
        }
    }

    public void StopDragging(GameObject go)
    {
        if (this.mDragging)
        {
            this.mDragging = false;
            this.OnDragDropRelease(go);
        }
    }

    protected virtual void Update()
    {
        if (((this.restriction == Restriction.PressAndHold) && this.mPressed) && (!this.mDragging && (this.mDragStartTime < RealTime.time)))
        {
            this.StartDragging();
        }
    }

    [CompilerGenerated]
    private sealed class <EnableDragScrollView>c__Iterator3D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UIDragDropItem <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.<>f__this.mDragScrollView != null)
                    {
                        this.<>f__this.mDragScrollView.enabled = true;
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold
    }
}

