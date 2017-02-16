using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantListViewObject : ListViewObject
{
    protected ServantListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected ServantListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<ServantListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<ServantListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    public ServantListViewItem GetItem() => 
        (base.linkItem as ServantListViewItem);

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
        ServantListViewItem linkItem = base.linkItem as ServantListViewItem;
        ServantListViewItemDraw.DispMode dispMode = this.dispMode;
        bool flag = this.state == State.INIT;
        if (linkItem == null)
        {
            initMode = InitMode.INVISIBLE;
        }
        base.SetVisible(initMode != InitMode.INVISIBLE);
        this.SetInput(initMode == InitMode.INPUT);
        base.transform.localPosition = base.basePosition;
        base.transform.localScale = base.baseScale;
        this.callbackFunc = callbackFunc;
        switch (initMode)
        {
            case InitMode.INVISIBLE:
                this.dispMode = ServantListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ServantListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ServantListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ServantListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.MODIFY:
                this.dispMode = ServantListViewItemDraw.DispMode.VALID;
                flag = true;
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

    protected void InitItem()
    {
        this.state = State.INIT;
    }

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            ServantListViewItem linkItem = base.linkItem as ServantListViewItem;
            if (!linkItem.IsCanNotSelect)
            {
                Debug.Log("OnClickSelect ListView " + base.linkItem.Index);
                base.manager.SendMessage("OnClickSelectListView", this);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
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

    public void OnLongPush()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnLongPush ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnLongPushListView", this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        ServantListViewItem linkItem = base.linkItem as ServantListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput);
        }
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
    }

    public override void SetItem(ListViewItem item)
    {
        base.SetItem(item);
        this.InitItem();
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        base.SetItem(item, seed);
        this.InitItem();
    }

    protected void SetupDisp()
    {
        ServantListViewItem linkItem = base.linkItem as ServantListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != ServantListViewItemDraw.DispMode.INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    public override string ToString() => 
        (this.dispMode + " " + base.basePosition);

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
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

