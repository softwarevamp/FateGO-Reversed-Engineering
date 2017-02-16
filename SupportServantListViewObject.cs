using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupportServantListViewObject : ListViewObject
{
    protected SupportServantListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected SupportServantListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<SupportServantListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<SupportServantListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    protected void EventMoveEnd()
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

    public SupportServantListViewItem GetItem() => 
        (base.linkItem as SupportServantListViewItem);

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
            SupportServantListViewItem linkItem = base.linkItem as SupportServantListViewItem;
            SupportServantListViewItemDraw.DispMode dispMode = this.dispMode;
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
                    this.dispMode = SupportServantListViewItemDraw.DispMode.INVISIBLE;
                    this.state = State.IDLE;
                    break;

                case InitMode.INVALID:
                    this.dispMode = SupportServantListViewItemDraw.DispMode.INVALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.VALID:
                    this.dispMode = SupportServantListViewItemDraw.DispMode.VALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.INPUT:
                    this.dispMode = SupportServantListViewItemDraw.DispMode.VALID;
                    this.state = State.INPUT;
                    break;

                case InitMode.MODIFY:
                    this.dispMode = SupportServantListViewItemDraw.DispMode.VALID;
                    flag = true;
                    this.state = State.IDLE;
                    break;

                case InitMode.TUTORIAL_INPUT:
                    base.SetInput(true);
                    this.dispMode = SupportServantListViewItemDraw.DispMode.VALID;
                    this.state = State.TUTORIAL_INPUT;
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

    protected void InitItem()
    {
        this.state = State.INIT;
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
        if ((this.state != State.TUTORIAL_INPUT) && (base.linkItem != null))
        {
            Debug.Log("OnLongPush ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnLongPushListView", this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        SupportServantListViewItem linkItem = base.linkItem as SupportServantListViewItem;
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
        SupportServantListViewItem linkItem = base.linkItem as SupportServantListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != SupportServantListViewItemDraw.DispMode.INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        MODIFY,
        TUTORIAL_INPUT
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT,
        TUTORIAL_INPUT
    }
}

