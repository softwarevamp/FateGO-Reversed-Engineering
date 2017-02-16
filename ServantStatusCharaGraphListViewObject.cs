using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusCharaGraphListViewObject : ListViewObject
{
    protected ServantStatusCharaGraphListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected ServantStatusCharaGraphListViewItemDraw itemDraw;
    protected static readonly float MAXIM_IN_SPEED = 0.2f;
    protected static readonly float MAXIM_OUT_SPEED = 0.2f;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<ServantStatusCharaGraphListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<ServantStatusCharaGraphListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventMaximMove()
    {
        TweenRotation.Begin(this.dragObject, MAXIM_IN_SPEED, Quaternion.Euler(0f, 0f, 90f));
        TweenScale.Begin(this.dragObject, MAXIM_IN_SPEED, new Vector3(1.7391f, 1.7391f, 1f));
        TweenPosition position = TweenPosition.Begin(this.dragObject, MAXIM_IN_SPEED, Vector3.zero);
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventMaximMove2";
    }

    private void EventMaximMove2()
    {
        this.EventMoveEnd();
    }

    private void EventMaximStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<ServantStatusCharaGraphListViewObject>().Init(InitMode.VALID);
        base.Invoke("EventMaximMove", delay);
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

    private void EventUsuallyMove()
    {
        TweenRotation.Begin(this.dragObject, MAXIM_OUT_SPEED, Quaternion.Euler(0f, 0f, 0f));
        TweenScale.Begin(this.dragObject, MAXIM_OUT_SPEED, new Vector3(1f, 1f, 1f));
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position);
        TweenPosition position = TweenPosition.Begin(this.dragObject, MAXIM_OUT_SPEED, pos);
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventUsuallyMove2";
    }

    private void EventUsuallyMove2()
    {
        base.SetVisible(true);
        this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventUsuallyStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        base.Invoke("EventUsuallyMove", delay);
    }

    public ServantStatusCharaGraphListViewItem GetItem() => 
        (base.linkItem as ServantStatusCharaGraphListViewItem);

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
        ServantStatusCharaGraphListViewItem linkItem = base.linkItem as ServantStatusCharaGraphListViewItem;
        ServantStatusCharaGraphListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.MAXIM:
                this.dispMode = ServantStatusCharaGraphListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventMaximStart(delay);
                return;

            case InitMode.USUALLY:
                this.state = State.MOVE;
                this.EventUsuallyStart(delay);
                return;
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

    private void OnDestroy()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        this.state = State.INIT;
        base.SetItem(item, seed);
    }

    protected void SetupDisp()
    {
        ServantStatusCharaGraphListViewItem linkItem = base.linkItem as ServantStatusCharaGraphListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != ServantStatusCharaGraphListViewItemDraw.DispMode.INVISIBLE));
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
        MAXIM,
        USUALLY
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

