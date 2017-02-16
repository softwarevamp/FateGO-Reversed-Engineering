using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FollowerSelectItemListViewObject : ListViewObject
{
    protected FollowerSelectItemListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected FollowerSelectItemListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<FollowerSelectItemListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<FollowerSelectItemListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventEnterMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(0f, 1100f, 0f);
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
        this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<FollowerSelectItemListViewObject>().Init(InitMode.VALID);
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
            Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(1100f, 0f, 0f);
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
        this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        FollowerSelectItemListViewObject component = this.dragObject.GetComponent<FollowerSelectItemListViewObject>();
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
        this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventIntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<FollowerSelectItemListViewObject>().Init(InitMode.VALID);
        this.dragObject.transform.position = base.transform.TransformPoint(1100f, 0f, 0f);
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

    public FollowerSelectItemListViewItem GetItem() => 
        (base.linkItem as FollowerSelectItemListViewItem);

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
            FollowerSelectItemListViewItem linkItem = base.linkItem as FollowerSelectItemListViewItem;
            FollowerSelectItemListViewItemDraw.DispMode dispMode = this.dispMode;
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
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE;
                    this.state = State.IDLE;
                    break;

                case InitMode.INVALID:
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.VALID:
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.VALID;
                    this.state = State.IDLE;
                    break;

                case InitMode.INPUT:
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.VALID;
                    this.state = State.INPUT;
                    break;

                case InitMode.INTO:
                    flag = true;
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE;
                    this.state = State.MOVE;
                    this.EventIntoStart(delay);
                    return;

                case InitMode.TUTORIAL_INPUT:
                    this.SetInput(true);
                    this.dispMode = FollowerSelectItemListViewItemDraw.DispMode.VALID;
                    this.state = State.TUTORIAL_INPUT;
                    this.SetInputTutorial(true);
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

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSelect ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnClickSelectListView", this);
        }
    }

    public void OnClickSkill1()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSkill1 ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnClickSkill1ListView", this);
        }
    }

    public void OnClickSkill13()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSkill3 ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnClickSkill3ListView", this);
        }
    }

    public void OnClickSkill2()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSkill2 ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnClickSkill2ListView", this);
        }
    }

    public void OnClickSupport()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnClickSupport ListView " + base.linkItem.Index);
            base.manager.SendMessage("OnClickSupportListView", this);
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
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushListView", this);
        }
    }

    public void OnLongPushSkill1()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnLongPushSkill1 ListView " + base.linkItem.Index);
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushSkill1ListView", this);
        }
    }

    public void OnLongPushSkill2()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnLongPushSkill2 ListView " + base.linkItem.Index);
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushSkill2ListView", this);
        }
    }

    public void OnLongPushSkill3()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnLongPushSkill3 ListView " + base.linkItem.Index);
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushSkill3ListView", this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        FollowerSelectItemListViewItem linkItem = base.linkItem as FollowerSelectItemListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput, false);
        }
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
    }

    public void SetInputTutorial(bool isInput)
    {
        base.SetInput(isInput);
        FollowerSelectItemListViewItem linkItem = base.linkItem as FollowerSelectItemListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput, true);
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
        FollowerSelectItemListViewItem linkItem = base.linkItem as FollowerSelectItemListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != FollowerSelectItemListViewItemDraw.DispMode.INVISIBLE));
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
        INPUT,
        INTO,
        ENTER,
        EXIT,
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

