using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/UIDragDropListViewItem")]
public class UIDragDropListViewItem : MonoBehaviour
{
    protected GameObject dragObject;
    [SerializeField]
    protected ListViewObject listViewObject;
    protected Collider mCollider;
    protected UIDragScrollView mDragScrollView;
    protected ListViewObject mListViewObject;
    protected Transform mParent;
    protected float mPressTime;
    protected UIRoot mRoot;
    protected Vector3 mTarget;
    protected int mTouchID = -2147483648;
    protected Transform mTrans;
    public Restriction restriction;

    private void OnDrag(Vector2 delta)
    {
        if (base.enabled && (this.mTouchID == UICamera.currentTouchID))
        {
            this.OnDragDropMove((Vector3) (delta * this.mRoot.pixelSizeAdjustment));
        }
    }

    protected virtual void OnDragDropMove(Vector3 delta)
    {
        if (this.dragObject != null)
        {
            Transform transform = this.dragObject.transform;
            transform.localPosition += delta;
        }
    }

    protected virtual void OnDragDropRelease(GameObject surface)
    {
        if (this.dragObject != null)
        {
            Debug.Log("OnDragDropRelease " + this.dragObject.transform.position.ToString());
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
            this.mTouchID = -2147483648;
            if (this.mCollider != null)
            {
                this.mCollider.enabled = true;
            }
            this.mParent = this.mTrans.parent;
            if (this.mDragScrollView != null)
            {
                this.mDragScrollView.enabled = true;
            }
            NGUITools.MarkParentAsChanged(base.gameObject);
            this.mListViewObject.DragMaskEnd();
        }
    }

    protected virtual void OnDragDropStart()
    {
        if (this.dragObject != null)
        {
            if (this.mDragScrollView != null)
            {
                this.mDragScrollView.enabled = false;
            }
            if (this.mCollider != null)
            {
                this.mCollider.enabled = false;
            }
            this.mTouchID = UICamera.currentTouchID;
            this.mParent = this.mTrans.parent;
            this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
            this.mListViewObject.DragMaskStart();
        }
    }

    private void OnDragEnd()
    {
        if (base.enabled && (this.mTouchID == UICamera.currentTouchID))
        {
            Debug.Log("OnDragEnd");
            this.OnDragDropRelease(UICamera.hoveredObject);
        }
    }

    private void OnDragStart()
    {
        if (base.enabled && (this.mTouchID == -2147483648))
        {
            if (this.restriction != Restriction.None)
            {
                if (this.restriction == Restriction.Horizontal)
                {
                    Vector2 totalDelta = UICamera.currentTouch.totalDelta;
                    float introduced2 = Mathf.Abs(totalDelta.x);
                    if (introduced2 < Mathf.Abs(totalDelta.y))
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.Vertical)
                {
                    Vector2 vector2 = UICamera.currentTouch.totalDelta;
                    float introduced3 = Mathf.Abs(vector2.x);
                    if (introduced3 > Mathf.Abs(vector2.y))
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.PressAndHold)
                {
                    if ((this.mPressTime + 1f) > RealTime.time)
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.Press)
                {
                }
            }
            if (this.mListViewObject.IsCanDrag())
            {
                this.mTarget = base.transform.position;
                this.dragObject = this.mListViewObject.CreateDragObject();
                this.OnDragDropStart();
            }
        }
    }

    private void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            this.mPressTime = RealTime.time;
        }
    }

    public void SetBaseTransform()
    {
        this.mListViewObject = (this.listViewObject == null) ? base.GetComponent<ListViewObject>() : this.listViewObject;
        this.mDragScrollView = this.mListViewObject.GetComponent<UIDragScrollView>();
        this.mTrans = this.mListViewObject.transform;
        this.mTrans = base.transform;
        this.mCollider = base.GetComponent<Collider>();
    }

    public void SetEnable(bool flag)
    {
        if (this.mCollider != null)
        {
            this.mCollider.enabled = flag;
        }
        if (this.mDragScrollView != null)
        {
            this.mDragScrollView.enabled = flag;
        }
    }

    protected virtual void Start()
    {
        this.SetBaseTransform();
    }

    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold,
        Press
    }
}

