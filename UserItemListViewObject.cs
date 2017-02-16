using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserItemListViewObject : ListViewObject
{
    protected UserItemListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected UserItemListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<UserItemListViewItemDraw>();
    }

    public UserItemListViewItem GetItem() => 
        (base.linkItem as UserItemListViewItem);

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
        UserItemListViewItem linkItem = base.linkItem as UserItemListViewItem;
        UserItemListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = UserItemListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = UserItemListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = UserItemListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = UserItemListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
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

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.manager.SendMessage("OnClickListView", this);
        }
    }

    private void OnDestroy()
    {
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        UserItemListViewItem linkItem = base.linkItem as UserItemListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput);
        }
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        this.state = State.INIT;
        base.SetItem(item, seed);
    }

    protected void SetupDisp()
    {
        UserItemListViewItem linkItem = base.linkItem as UserItemListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != UserItemListViewItemDraw.DispMode.INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    private void Start()
    {
        if (this.state == State.INIT)
        {
            this.Init(InitMode.VALID);
        }
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

