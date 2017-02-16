using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusListViewObject : MonoBehaviour
{
    protected Transform baseParent;
    protected Vector3 basePosition;
    protected Vector3 baseScale;
    protected ServantStatusListViewItemDraw.DispMode dispMode;
    [SerializeField]
    protected GameObject dispObject;
    protected int index;
    protected bool isBusy;
    protected ServantStatusListViewItemDraw itemDraw;
    protected ServantStatusListViewItem mainInfo;
    protected ServantStatusListViewManager manager;
    protected UIDragDropListViewItem mDragDrop;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        this.mDragDrop = base.GetComponent<UIDragDropListViewItem>();
        this.itemDraw = this.dispObject.GetComponent<ServantStatusListViewItemDraw>();
        this.SetBaseTransform();
        if (this.mDragDrop != null)
        {
            this.mDragDrop.SetEnable(false);
        }
    }

    private void EventBattleStart()
    {
        ServantStatusListViewItemDraw.Kind kind = this.GetKind();
        if ((((kind == ServantStatusListViewItemDraw.Kind.NP) || (kind == ServantStatusListViewItemDraw.Kind.COMMAND)) || (kind == ServantStatusListViewItemDraw.Kind.MAIN)) && (this.itemDraw != null))
        {
            this.itemDraw.PlayBattle(this.mainInfo);
        }
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    private void EventCommandStart()
    {
        if (this.itemDraw != null)
        {
            this.itemDraw.ModifyCommandCard(this.mainInfo);
        }
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    private void EventFaceStart()
    {
        if (this.itemDraw != null)
        {
            this.itemDraw.ModifyFace(this.mainInfo);
        }
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    protected void EventMoveEnd()
    {
        this.isBusy = false;
        this.state = State.IDLE;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public ServantStatusListViewItemDraw.Kind GetKind()
    {
        if (this.itemDraw != null)
        {
            return this.itemDraw.GetKind();
        }
        return ServantStatusListViewItemDraw.Kind.NONE;
    }

    public int GetSize()
    {
        BoxCollider component = base.GetComponent<Collider>() as BoxCollider;
        if (component != null)
        {
            return (int) component.size.y;
        }
        return 0;
    }

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
        ServantStatusListViewItemDraw.DispMode dispMode = this.dispMode;
        bool flag = this.state == State.INIT;
        this.SetVisible(initMode != InitMode.INVISIBLE);
        this.SetInput(initMode == InitMode.INPUT);
        base.transform.localPosition = this.basePosition;
        base.transform.localScale = this.baseScale;
        this.callbackFunc = callbackFunc;
        switch (initMode)
        {
            case InitMode.INVISIBLE:
                this.dispMode = ServantStatusListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ServantStatusListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ServantStatusListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ServantStatusListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                flag = true;
                break;

            case InitMode.BATTLE:
                this.EventBattleStart();
                return;

            case InitMode.COMMAND:
                this.EventCommandStart();
                return;

            case InitMode.FACE:
                this.EventFaceStart();
                return;

            case InitMode.MODIFY:
                this.dispMode = ServantStatusListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                flag = true;
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

    protected void OnClick()
    {
        this.manager.SendMessage("OnClickListView", this);
    }

    public void OnClickCommandCharaLevel1()
    {
        this.manager.SendMessage("OnClickCommandCharaLevel1", this);
    }

    public void OnClickCommandCharaLevel2()
    {
        this.manager.SendMessage("OnClickCommandCharaLevel2", this);
    }

    public void OnClickCommandCharaLevel3()
    {
        this.manager.SendMessage("OnClickCommandCharaLevel3", this);
    }

    public void OnClickEquip1()
    {
        this.manager.SendMessage("OnClickListViewEquip1", this);
    }

    public void OnClickEquipExp()
    {
        this.manager.SendMessage("OnClickListViewEquipExp", this);
    }

    public void OnClickExp()
    {
        this.manager.SendMessage("OnClickListViewExp", this);
    }

    public void OnClickFaceCharaLevel1()
    {
        this.manager.SendMessage("OnClickFaceCharaLevel1", this);
    }

    public void OnClickFaceCharaLevel2()
    {
        this.manager.SendMessage("OnClickFaceCharaLevel2", this);
    }

    public void OnClickFaceCharaLevel3()
    {
        this.manager.SendMessage("OnClickFaceCharaLevel3", this);
    }

    public void OnClickFaceCharaLevel4()
    {
        this.manager.SendMessage("OnClickFaceCharaLevel4", this);
    }

    public void OnClickFriendship()
    {
        this.manager.SendMessage("OnClickListViewFriendship", this);
    }

    private void OnDestroy()
    {
    }

    public void OnLongPushEquip1()
    {
        this.manager.SendMessage("OnLongPushListViewEquip1", this);
    }

    public void SetBaseTransform()
    {
        this.baseParent = base.transform.parent;
        this.basePosition = base.transform.localPosition;
        this.baseScale = base.transform.localScale;
    }

    public void SetInput(bool isInput)
    {
        if (base.GetComponent<Collider>() != null)
        {
            base.GetComponent<Collider>().enabled = isInput;
        }
        if (this.mDragDrop != null)
        {
            this.mDragDrop.SetEnable(isInput);
        }
    }

    public void SetItem(ServantStatusListViewItem item)
    {
        this.mainInfo = item;
        this.Init(InitMode.VALID);
    }

    public void SetManager(ServantStatusListViewManager manager)
    {
        this.manager = manager;
        if (this.state == State.INIT)
        {
            this.Init(InitMode.VALID);
        }
    }

    public void SetTransform(Vector3 position)
    {
        base.transform.position = position;
        this.basePosition = position;
    }

    protected void SetupDisp()
    {
        this.SetVisible(this.dispMode != ServantStatusListViewItemDraw.DispMode.INVISIBLE);
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(this.mainInfo, this.dispMode);
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (this.dispObject != null)
        {
            this.dispObject.SetActive(isVisible);
        }
    }

    public int Index =>
        this.index;

    public bool IsBusy =>
        this.isBusy;

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        BATTLE,
        COMMAND,
        FACE,
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

