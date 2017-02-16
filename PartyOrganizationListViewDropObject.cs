using System;
using UnityEngine;

public class PartyOrganizationListViewDropObject : ListViewDropObject
{
    protected static float ACTION_TIME = 0.2f;
    protected System.Action callbackFunc;
    protected PartyOrganizationListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    [SerializeField]
    protected GameObject equipDispBase;
    [SerializeField]
    protected UIDragDropListViewItem equipUIDragDrop;
    protected PartyOrganizationListViewItemDraw itemDraw;
    protected State state;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<PartyOrganizationListViewItemDraw>();
    }

    public override bool ClearItem()
    {
        if (base.isBusy)
        {
            return false;
        }
        base.linkItem = null;
        this.callbackFunc = null;
        this.state = State.INIT;
        if (this.itemDraw != null)
        {
            this.itemDraw.ClearItem();
        }
        return true;
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<PartyOrganizationListViewDropObject>().Init(InitMode.VALID);
        return obj2;
    }

    protected void DragDelete()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    protected void DragMoveStart(Vector3 position)
    {
        base.isBusy = true;
        if (this.dragObject == null)
        {
            this.dispMode = PartyOrganizationListViewItemDraw.DispMode.INVISIBLE;
            this.SetVisible(false);
            this.SetupDisp();
            this.dragObject = this.CreateDragObject();
            if (this.dragObject.GetComponent<PartyOrganizationListViewDropObject>().itemDraw == null)
            {
            }
        }
        TweenPosition position2 = TweenPosition.Begin(this.dragObject, ACTION_TIME, this.dragObject.transform.parent.InverseTransformPoint(position));
        position2.method = UITweener.Method.EaseInOut;
        position2.eventReceiver = base.gameObject;
        position2.callWhenFinished = "MoveEnd";
    }

    public PartyOrganizationListViewItem GetItem() => 
        (base.linkItem as PartyOrganizationListViewItem);

    public void Init(InitMode initMode)
    {
        this.Init(initMode, null, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc)
    {
        this.Init(initMode, callbackFunc, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay)
    {
        this.Init(initMode, callbackFunc, delay, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay, Vector3 position)
    {
        if (initMode == InitMode.MODIFY)
        {
            this.SetupDisp();
        }
        else
        {
            PartyOrganizationListViewItem linkItem = base.linkItem as PartyOrganizationListViewItem;
            PartyOrganizationListViewItemDraw.DispMode dispMode = this.dispMode;
            bool flag = this.state == State.INIT;
            if (linkItem == null)
            {
                initMode = InitMode.INVISIBLE;
            }
            this.SetVisible((initMode != InitMode.INVISIBLE) && (initMode != InitMode.DRAG_INVISIBLE));
            this.SetInput((initMode == InitMode.INPUT) || (initMode == InitMode.DRAG_INPUT));
            base.transform.localPosition = base.basePosition;
            base.transform.localScale = base.baseScale;
            this.callbackFunc = callbackFunc;
            switch (initMode)
            {
                case InitMode.INVISIBLE:
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.INVISIBLE;
                    this.state = State.IDLE;
                    break;

                case InitMode.INVALID:
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.INVALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.VALID:
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.INTO:
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
                    this.state = State.MOVE;
                    this.IntoStart(delay);
                    return;

                case InitMode.INPUT:
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
                    this.state = State.INPUT;
                    break;

                case InitMode.DRAG_INVISIBLE:
                    flag = true;
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.DRAG_INVISIBLE;
                    this.state = State.MOVE;
                    break;

                case InitMode.DRAG_MOVE:
                    this.state = State.MOVE;
                    this.DragMoveStart(position);
                    return;

                case InitMode.DRAG_DELETE:
                    this.DragDelete();
                    return;

                case InitMode.DRAG_INPUT:
                    flag = true;
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
                    this.state = State.INPUT;
                    break;

                case InitMode.DRAG_VALID:
                    flag = true;
                    this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
                    this.state = State.IDLE;
                    break;
            }
            if (flag || (dispMode != this.dispMode))
            {
                this.SetupDisp();
            }
            if (this.callbackFunc != null)
            {
                System.Action action = this.callbackFunc;
                this.callbackFunc = null;
                action();
            }
        }
    }

    private void IntoMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position);
        TweenPosition position = TweenPosition.Begin(this.dragObject, 1.5f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "IntoMove2";
    }

    private void IntoMove2()
    {
        this.SetVisible(true);
        this.dispMode = PartyOrganizationListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.MoveEnd();
    }

    private void IntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = PartyOrganizationListViewItemDraw.DispMode.INVISIBLE;
        this.SetVisible(false);
        this.SetupDisp();
        this.dragObject = this.CreateDragObject();
        this.dragObject.transform.position = base.transform.TransformPoint(-3000f, 0f, 0f);
        base.Invoke("IntoMove", delay);
    }

    public override bool IsCanDrag()
    {
        PartyOrganizationListViewManager manager = base.manager as PartyOrganizationListViewManager;
        return manager.IsCanDrag();
    }

    protected void MoveEnd()
    {
        base.isBusy = false;
        this.state = State.IDLE;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public void OnClickItemEquip()
    {
        if ((this.state == State.INPUT) && (base.linkItem != null))
        {
            Debug.Log("Onclick ListViewDropEquip " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListDropEquip", this);
        }
    }

    public void OnClickListDrop()
    {
        if ((this.state == State.INPUT) && (base.linkItem != null))
        {
            Debug.Log("Onclick ListViewDropServant " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListDropServant", this);
        }
    }

    private void OnDestroy()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    public void OnLongPressItem()
    {
        if ((this.state == State.INPUT) && (base.linkItem != null))
        {
            Debug.Log("Onclick ListViewDropServantDetail " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListDropServantDetail", this);
        }
    }

    public void OnLongPressItemEquip()
    {
        if ((this.state == State.INPUT) && (base.linkItem != null))
        {
            Debug.Log("Onclick ListViewDropEquipDetail " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListDropEquipDetail", this);
        }
    }

    public void ReleaseItem()
    {
        if (base.linkItem != null)
        {
            base.linkItem = null;
            this.callbackFunc = null;
            this.state = State.INIT;
            if (this.itemDraw != null)
            {
                this.itemDraw.ClearItem();
            }
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
        if (this.equipUIDragDrop != null)
        {
            this.equipUIDragDrop.GetComponent<Collider>().enabled = isInput;
            this.equipUIDragDrop.SetEnable(isInput);
        }
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        this.state = State.INIT;
        if (this.equipUIDragDrop != null)
        {
            this.equipUIDragDrop.SetBaseTransform();
        }
        base.SetItem(item, seed);
    }

    protected void SetupDisp()
    {
        PartyOrganizationListViewItem linkItem = base.linkItem as PartyOrganizationListViewItem;
        this.SetVisible(((linkItem != null) && (this.dispMode != PartyOrganizationListViewItemDraw.DispMode.INVISIBLE)) && (this.dispMode != PartyOrganizationListViewItemDraw.DispMode.DRAG_INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    public void SetVisible(bool isVisible)
    {
        base.SetVisible(isVisible);
        if (this.equipDispBase != null)
        {
            this.equipDispBase.SetActive(isVisible);
        }
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INTO,
        INPUT,
        DRAG_INVISIBLE,
        DRAG_MOVE,
        DRAG_DELETE,
        DRAG_INPUT,
        DRAG_VALID,
        MODIFY
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

