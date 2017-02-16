using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendOperationItemListViewObject : ListViewObject
{
    protected FriendOperationItemListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected FriendOperationItemListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<FriendOperationItemListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<FriendOperationItemListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    private void EventEnterMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(0f, 980f, 0f);
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
        this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<FriendOperationItemListViewObject>().Init(InitMode.VALID);
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
            Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position) + new Vector3(980f, 0f, 0f);
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
        this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        FriendOperationItemListViewObject component = this.dragObject.GetComponent<FriendOperationItemListViewObject>();
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
        this.dispMode = FriendOperationItemListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.EventMoveEnd();
    }

    private void EventIntoStart(float delay)
    {
        base.isBusy = true;
        this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        base.SetVisible(false);
        this.dragObject = this.CreateDragObject();
        this.dragObject.GetComponent<FriendOperationItemListViewObject>().Init(InitMode.VALID);
        this.dragObject.transform.position = base.transform.TransformPoint(980f, 0f, 0f);
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

    public FriendOperationItemListViewItem GetItem() => 
        (base.linkItem as FriendOperationItemListViewItem);

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
        FriendOperationItemListViewItem linkItem = base.linkItem as FriendOperationItemListViewItem;
        FriendOperationItemListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.INTO:
                flag = true;
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.EventIntoStart(delay);
                return;

            case InitMode.EXIT:
                this.dispMode = FriendOperationItemListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.EventExitStart(delay);
                return;

            case InitMode.MODIFY:
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

    public void OnClickAccept()
    {
        if (base.linkItem != null)
        {
            Debug.Log("Onclick ListViewAccept " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListViewAccept", this);
        }
    }

    public void OnClickCancel()
    {
        if (base.linkItem != null)
        {
            Debug.Log("Onclick ListViewReject " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListViewCancel", this);
        }
    }

    public void OnClickOffer()
    {
        if (base.linkItem != null)
        {
            Debug.Log("Onclick ListViewOffer " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListViewOffer", this);
        }
    }

    public void OnClickReject()
    {
        if (base.linkItem != null)
        {
            Debug.Log("Onclick ListViewReject " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListViewReject", this);
        }
    }

    public void OnClickRemove()
    {
        if (base.linkItem != null)
        {
            Debug.Log("Onclick ListViewRemove " + base.linkItem.Index);
            base.manager.SendMessage("OnClickListViewRemove", this);
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

    public void OnClickSupportInfo()
    {
        if (base.linkItem != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            FriendOperationItemListViewItem linkItem = base.linkItem as FriendOperationItemListViewItem;
            SupportInfoJump data = new SupportInfoJump(linkItem.GameUser, linkItem.Kind, false);
            data.SetReturnNowScene();
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.SupportSelect, SceneManager.FadeType.BLACK, data);
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
        FriendOperationItemListViewItem linkItem = base.linkItem as FriendOperationItemListViewItem;
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
        FriendOperationItemListViewItem linkItem = base.linkItem as FriendOperationItemListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != FriendOperationItemListViewItemDraw.DispMode.INVISIBLE));
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

