﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SvtEqCombineListViewObject : ListViewObject
{
    protected SvtEqCombineListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected SvtEqCombineListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<SvtEqCombineListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<SvtEqCombineListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventEnterMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(0f, 1000f, 0f);
        TweenPosition position = TweenPosition.Begin(this.dragObject, 1f, pos);
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
        this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<SvtEqCombineListViewObject>().Init(InitMode.VALID);
        base.Invoke("EventEnterMove", delay);
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
            TweenPosition position = TweenPosition.Begin(this.dragObject, 1f, pos);
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
        this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        SvtEqCombineListViewObject component = this.dragObject.GetComponent<SvtEqCombineListViewObject>();
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
        TweenPosition position = TweenPosition.Begin(this.dragObject, 1.5f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "EventIntoMove2";
    }

    private void EventIntoMove2()
    {
        base.SetVisible(true);
        this.dispMode = SvtEqCombineListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventIntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<SvtEqCombineListViewObject>().Init(InitMode.VALID);
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

    public SvtEqCombineListViewItem GetItem() => 
        (base.linkItem as SvtEqCombineListViewItem);

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
        SvtEqCombineListViewItem linkItem = base.linkItem as SvtEqCombineListViewItem;
        SvtEqCombineListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.INTO:
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.EventIntoStart(delay);
                return;

            case InitMode.EXIT:
                this.dispMode = SvtEqCombineListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventExitStart(delay);
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

    protected void InitItem()
    {
        this.state = State.INIT;
    }

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            SvtEqCombineListViewItem linkItem = base.linkItem as SvtEqCombineListViewItem;
            if (linkItem.ListType == SvtEqCombineListViewItem.Type.SVTEQ_BASE)
            {
                if (!linkItem.IsCanNotBaseSelect)
                {
                    Debug.Log("OnClickSelect ListView " + base.linkItem.Index);
                    base.manager.SendMessage("OnClickSelectBase", this);
                }
                else if (linkItem.IsCanNotBaseSelect)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
            }
            if (linkItem.ListType == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL)
            {
                if (!linkItem.IsCanNotSelect)
                {
                    Debug.Log("OnClickSelect ListView " + base.linkItem.Index);
                    if (linkItem.IsMtSelect)
                    {
                        linkItem.IsMtSelect = false;
                        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                    }
                    else if (!linkItem.IsMtSelect)
                    {
                        if (linkItem.IsSelectMax)
                        {
                            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        }
                        else if (!linkItem.IsSelectMax)
                        {
                            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        }
                    }
                    base.manager.SendMessage("OnClickSelectMaterial", this);
                }
                else if (linkItem.IsCanNotSelect)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
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
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            Debug.Log("OnLongPush ListView " + base.linkItem.Index);
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushListView", this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        SvtEqCombineListViewItem linkItem = base.linkItem as SvtEqCombineListViewItem;
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
        SvtEqCombineListViewItem linkItem = base.linkItem as SvtEqCombineListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != SvtEqCombineListViewItemDraw.DispMode.INVISIBLE));
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

    public override string ToString() => 
        (this.dispMode + " " + base.basePosition);

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        FADEIN,
        INPUT,
        INTO,
        ENTER,
        EXIT
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

