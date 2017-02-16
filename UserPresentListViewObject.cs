using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserPresentListViewObject : ListViewObject
{
    protected UserPresentListViewItemDraw.DispMode dispMode;
    protected UserPresentListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<UserPresentListViewItemDraw>();
    }

    public UserPresentListViewItem GetItem() => 
        (base.linkItem as UserPresentListViewItem);

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
        UserPresentListViewItem linkItem = base.linkItem as UserPresentListViewItem;
        UserPresentListViewItemDraw.DispMode dispMode = this.dispMode;
        bool flag = this.state == State.INIT;
        if (linkItem == null)
        {
            initMode = InitMode.INVISIBLE;
        }
        Debug.Log("!!!** UserPresentListViewObject Init: " + initMode);
        base.SetVisible(initMode != InitMode.INVISIBLE);
        this.SetInput(initMode == InitMode.INPUT);
        base.transform.localPosition = base.basePosition;
        base.transform.localScale = base.baseScale;
        this.callbackFunc = callbackFunc;
        switch (initMode)
        {
            case InitMode.INVISIBLE:
                this.dispMode = UserPresentListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = UserPresentListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = UserPresentListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = UserPresentListViewItemDraw.DispMode.VALID;
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

    public void OnClickCheck()
    {
        if ((base.linkItem != null) && !((UserPresentListViewItem) base.linkItem).blocked)
        {
            Debug.Log("OnClickCheck");
            base.manager.SendMessage("OnClickListCheck", this);
        }
    }

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSelect");
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.manager.SendMessage("OnClickListView", this);
        }
    }

    private void OnDestroy()
    {
    }

    public void setBlocked(bool blocked)
    {
        this.itemDraw.SetBlocked(blocked);
    }

    public void setCheckBoxed(bool checkBoxed, int count)
    {
        this.itemDraw.SetCheck(checkBoxed);
        this.itemDraw.SetCheckCnt(count);
    }

    public override void SetInput(bool isInput)
    {
        Debug.Log("!!!** UserPresentListViewObject SetInput: " + isInput);
        base.SetInput(isInput);
        UserPresentListViewItem linkItem = base.linkItem as UserPresentListViewItem;
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
        UserPresentListViewItem linkItem = base.linkItem as UserPresentListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != UserPresentListViewItemDraw.DispMode.INVISIBLE));
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
        HOLD
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

