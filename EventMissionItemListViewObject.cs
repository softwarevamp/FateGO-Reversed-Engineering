using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventMissionItemListViewObject : ListViewObject
{
    protected EventMissionItemListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected EventMissionItemListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<EventMissionItemListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<EventMissionItemListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventExitMove()
    {
        if (this.dragObject == null)
        {
            Debug.Log("create error?");
            this.EventMoveEnd();
        }
        else
        {
            Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(1000f, 0f, 0f);
            TweenPosition position = TweenPosition.Begin(this.dragObject, ListViewObject.BASE_MOVE_TIME, pos);
            position.method = UITweener.Method.EaseInOut;
            position.eventReceiver = base.gameObject;
            position.callWhenFinished = "EventExitMove2";
        }
    }

    private void EventExitMove2()
    {
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventExitStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = EventMissionItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        EventMissionItemListViewObject component = this.dragObject.GetComponent<EventMissionItemListViewObject>();
        if (component == null)
        {
            Debug.Log("create error?");
            this.EventMoveEnd();
        }
        else
        {
            component.Init(InitMode.VALID);
            base.Invoke("EventExitMove", delay);
        }
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
        this.dispMode = EventMissionItemListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventIntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = EventMissionItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<EventMissionItemListViewObject>().Init(InitMode.VALID);
        this.dragObject.transform.position = base.transform.TransformPoint(1000f, 0f, 0f);
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

    public EventMissionItemListViewItem GetItem() => 
        (base.linkItem as EventMissionItemListViewItem);

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
        EventMissionItemListViewItem linkItem = base.linkItem as EventMissionItemListViewItem;
        EventMissionItemListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.INPUT;
                this.state = State.INPUT;
                break;

            case InitMode.INTO:
                flag = true;
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.EventIntoStart(delay);
                return;

            case InitMode.EXIT:
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventExitStart(delay);
                return;

            case InitMode.MODIFY:
                flag = true;
                this.dispMode = EventMissionItemListViewItemDraw.DispMode.VALID;
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

    public void OnClickSelect()
    {
        base.manager.SendMessage("OnClickListView", this);
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
        EventMissionItemListViewItem linkItem = base.linkItem as EventMissionItemListViewItem;
        base.SetInput(isInput);
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem);
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
        EventMissionItemListViewItem linkItem = base.linkItem as EventMissionItemListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != EventMissionItemListViewItemDraw.DispMode.INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    private void Update()
    {
        if (base.linkItem != null)
        {
            EventMissionItemListViewItem linkItem = base.linkItem as EventMissionItemListViewItem;
            if (linkItem != null)
            {
                this.itemDraw.UpdateItem(linkItem, this.dispMode);
            }
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

