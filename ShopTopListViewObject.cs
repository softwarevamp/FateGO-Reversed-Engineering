using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopTopListViewObject : ListViewObject
{
    protected ShopTopListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected ShopTopListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<ShopTopListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<ShopTopListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventEntered()
    {
        this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
    }

    private void EventEnterMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(0f, 800f, 0f);
        TweenPosition position = TweenPosition.Begin(this.dragObject, ListViewObject.BASE_MOVE_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventEnterMove2";
    }

    private void EventEnterMove2()
    {
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventEnterStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<ShopTopListViewObject>().Init(InitMode.VALID);
        base.Invoke("EventEnterMove", delay);
    }

    private void EventIntoMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position);
        TweenPosition position = TweenPosition.Begin(this.dragObject, ListViewObject.BASE_MOVE_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventIntoMove2";
    }

    private void EventIntoMove2()
    {
        base.SetVisible(true);
        this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventIntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<ShopTopListViewObject>().Init(InitMode.VALID);
        this.dragObject.transform.position = base.transform.TransformPoint(500f, 0f, 0f);
        base.Invoke("EventIntoMove", delay);
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

    private void EventRetryMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position);
        TweenPosition position = TweenPosition.Begin(this.dragObject, ListViewObject.BASE_MOVE_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventRetryMove2";
    }

    private void EventRetryMove2()
    {
        base.SetVisible(true);
        this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventRetryStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<ShopTopListViewObject>().Init(InitMode.VALID);
        this.dragObject.transform.position = base.transform.TransformPoint(0f, 800f, 0f);
        base.Invoke("EventRetryMove", delay);
    }

    public ShopTopListViewItem GetItem() => 
        (base.linkItem as ShopTopListViewItem);

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
        ShopTopListViewItem linkItem = base.linkItem as ShopTopListViewItem;
        ShopTopListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ShopTopListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.INTO:
                flag = true;
                this.dispMode = ShopTopListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.EventIntoStart(delay);
                return;

            case InitMode.ENTER:
                this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventEnterStart(delay);
                return;

            case InitMode.RETRY:
                this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventRetryStart(delay);
                return;

            case InitMode.ENTERED:
                this.dispMode = ShopTopListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                this.EventEntered();
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
        ShopTopListViewItem linkItem = base.linkItem as ShopTopListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput);
        }
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
        ShopTopListViewItem linkItem = base.linkItem as ShopTopListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != ShopTopListViewItemDraw.DispMode.INVISIBLE));
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
        INTO,
        ENTER,
        EXIT,
        RETRY,
        ENTERED
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

