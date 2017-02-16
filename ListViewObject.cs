using System;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewObject")]
public class ListViewObject : MonoBehaviour
{
    protected static float BASE_MOVE_TIME = 0.2f;
    protected Transform baseParent;
    protected Vector3 basePosition;
    protected Vector3 baseScale;
    [SerializeField]
    protected GameObject dispObject;
    protected GameObject dragObjectPrefab;
    protected bool isBusy;
    protected ListViewItem linkItem;
    protected ListViewManager manager;
    protected UIDragDropListViewItem mDragDrop;
    protected Coroutine mPressCountine;
    protected PressState mPressState;
    protected static float PRESS_TIME = 0.3f;

    protected void Awake()
    {
        this.mDragDrop = base.GetComponent<UIDragDropListViewItem>();
        this.SetBaseTransform();
        if (this.mDragDrop != null)
        {
            this.mDragDrop.SetEnable(false);
        }
    }

    public virtual bool ClearItem()
    {
        if (((this.linkItem != null) && (this.linkItem.ViewObject == this)) && !this.isBusy)
        {
            this.linkItem.ViewObject = null;
            this.linkItem = null;
            this.SetVisible(false);
            this.SetInput(false);
            return true;
        }
        return false;
    }

    public virtual GameObject CreateDragObject()
    {
        GameObject dragRoot = this.GetDragRoot();
        GameObject obj3 = NGUITools.AddChild(dragRoot, this.dragObjectPrefab);
        ListViewObject component = obj3.GetComponent<ListViewObject>();
        component.linkItem = this.linkItem;
        component.transform.position = base.transform.position;
        component.transform.eulerAngles = base.transform.eulerAngles;
        component.transform.localScale = Vector3.one;
        Vector3 position = base.transform.TransformPoint(1f, 1f, 0f);
        Vector3 vector2 = component.transform.InverseTransformPoint(position);
        vector2.z = 1f;
        component.transform.localScale = vector2;
        component.gameObject.layer = dragRoot.layer;
        Vector3 localPosition = component.transform.localPosition;
        localPosition.z = 0f;
        component.transform.localPosition = localPosition;
        component.SetBaseTransform();
        component.SetVisible(true);
        component.SetInput(false);
        return obj3;
    }

    public void DragMaskEnd()
    {
        this.manager.DragMaskEnd();
    }

    public void DragMaskStart()
    {
        this.manager.DragMaskStart();
    }

    protected void EndMoveCenter()
    {
        this.isBusy = false;
        this.manager.EndScrollAnim();
    }

    public GameObject GetDragRoot() => 
        this.manager.GetDragRoot();

    public ListViewItem GetItem() => 
        this.linkItem;

    public virtual bool IsCanDrag() => 
        false;

    public bool MoveCenter(bool isAnimation = true)
    {
        UIPanel panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        if ((panel == null) || (panel.clipping == UIDrawCall.Clipping.None))
        {
            return false;
        }
        UIScrollView component = panel.GetComponent<UIScrollView>();
        Transform cachedTransform = panel.cachedTransform;
        Vector3[] worldCorners = panel.worldCorners;
        Vector3 position = (Vector3) ((worldCorners[2] + worldCorners[0]) * 0.5f);
        Vector3 vector2 = cachedTransform.InverseTransformPoint(base.transform.position);
        Vector3 vector3 = cachedTransform.InverseTransformPoint(position);
        Vector3 vector4 = vector2 - vector3;
        if (!component.canMoveHorizontally)
        {
            vector4.x = 0f;
        }
        if (!component.canMoveVertically)
        {
            vector4.y = 0f;
        }
        vector4.z = 0f;
        Vector3 pos = cachedTransform.localPosition - vector4;
        if (isAnimation)
        {
            SpringPanel panel2 = SpringPanel.Begin(panel.cachedGameObject, pos, 6f);
            if (panel2 != null)
            {
                this.isBusy = true;
                panel2.onFinished = (SpringPanel.OnFinished) Delegate.Combine(panel2.onFinished, new SpringPanel.OnFinished(this.EndMoveCenter));
            }
        }
        else
        {
            Vector3 localPosition = component.transform.localPosition;
            component.transform.localPosition = pos;
            Vector3 vector7 = pos - localPosition;
            Vector2 clipOffset = panel.clipOffset;
            clipOffset.x -= vector7.x;
            clipOffset.y -= vector7.y;
            panel.clipOffset = clipOffset;
            component.UpdateScrollbars(false);
        }
        return true;
    }

    public bool MoveTop(bool isAnimation = true)
    {
        UIPanel panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        if ((panel == null) || (panel.clipping == UIDrawCall.Clipping.None))
        {
            return false;
        }
        UIScrollView component = panel.GetComponent<UIScrollView>();
        Transform cachedTransform = panel.cachedTransform;
        Vector2 vector = (Vector2) (this.manager.getPitch() / 2f);
        Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
        Vector3[] worldCorners = panel.worldCorners;
        Vector3 vector3 = (Vector3) ((worldCorners[1] + worldCorners[2]) * 0.5f);
        Vector3 vector4 = cachedTransform.InverseTransformPoint(base.transform.position);
        Vector3 vector5 = cachedTransform.InverseTransformPoint(panel.worldCorners[1]);
        Vector3 vector6 = (vector4 - vector5) - vector2;
        if (!component.canMoveHorizontally)
        {
            vector6.x = 0f;
        }
        if (!component.canMoveVertically)
        {
            vector6.y = 0f;
        }
        vector6.z = 0f;
        Vector3 pos = cachedTransform.localPosition - vector6;
        if (isAnimation)
        {
            SpringPanel panel2 = SpringPanel.Begin(panel.cachedGameObject, pos, 6f);
            if (panel2 != null)
            {
                this.isBusy = true;
                panel2.onFinished = (SpringPanel.OnFinished) Delegate.Combine(panel2.onFinished, new SpringPanel.OnFinished(this.EndMoveCenter));
            }
        }
        else
        {
            Vector3 localPosition = component.transform.localPosition;
            component.transform.localPosition = pos;
            Vector3 vector9 = pos - localPosition;
            Vector2 clipOffset = panel.clipOffset;
            clipOffset.x -= vector9.x;
            clipOffset.y -= vector9.y;
            panel.clipOffset = clipOffset;
            component.UpdateScrollbars(false);
        }
        return true;
    }

    public void OnClick()
    {
        if ((this.linkItem != null) && (this.mPressState != PressState.STATE_STOP))
        {
            this.manager.SendMessage("OnClickListView", this);
        }
    }

    protected void OnDragEnd()
    {
        this.isBusy = false;
    }

    protected void OnDragStart()
    {
        this.isBusy = true;
    }

    public void ReleaseItem()
    {
        if (this.linkItem != null)
        {
            this.linkItem.ViewObject = null;
            this.linkItem = null;
            this.SetVisible(false);
            this.SetInput(false);
        }
    }

    public void SetBaseTransform()
    {
        this.baseParent = base.transform.parent;
        this.basePosition = base.transform.localPosition;
        this.baseScale = base.transform.localScale;
    }

    public void SetDragPrefab(GameObject prefab)
    {
        this.dragObjectPrefab = prefab;
    }

    public virtual void SetInput(bool isInput)
    {
        if (base.GetComponent<Collider>() != null)
        {
            base.GetComponent<Collider>().enabled = isInput;
        }
    }

    public virtual void SetItem(ListViewItem item)
    {
        this.linkItem = item;
        this.SetVisible(false);
        this.SetInput(false);
    }

    public virtual void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        item.ViewObject = this;
        this.linkItem = item;
        base.transform.parent = seed.Parent.transform;
        base.transform.localPosition = item.BasePosition;
        base.transform.localRotation = seed.transform.localRotation;
        base.transform.localScale = seed.transform.localScale;
        base.gameObject.layer = seed.Parent.layer;
        this.SetVisible(true);
        this.SetInput(false);
        base.gameObject.SendMessage("SetBaseTransform");
    }

    public void SetManager(ListViewManager manager)
    {
        this.manager = manager;
    }

    public void SetTransform(Vector3 position)
    {
        base.transform.localPosition = position;
        this.basePosition = position;
    }

    public void SetVisible(bool isVisible)
    {
        if (this.dispObject != null)
        {
            this.dispObject.SetActive(isVisible);
        }
    }

    public int Index
    {
        get
        {
            if (this.linkItem != null)
            {
                return this.linkItem.Index;
            }
            return -1;
        }
    }

    public bool IsBusy =>
        this.isBusy;

    public ListViewManager Manager =>
        this.manager;

    public enum PressState
    {
        STATE_BREAK = 3,
        STATE_PRESS = 1,
        STATE_STOP = 2
    }
}

