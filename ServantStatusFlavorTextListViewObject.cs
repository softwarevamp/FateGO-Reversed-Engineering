using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusFlavorTextListViewObject : MonoBehaviour
{
    protected Transform baseParent;
    protected Vector3 basePosition;
    protected Vector3 baseScale;
    protected ServantStatusFlavorTextListViewItemDraw.DispMode dispMode;
    [SerializeField]
    protected GameObject dispObject;
    protected int id;
    protected bool isBusy;
    protected bool isNew;
    protected bool isOpen;
    protected ServantStatusFlavorTextListViewItemDraw itemDraw;
    protected ServantStatusListViewItem mainInfo;
    protected ServantStatusFlavorTextListViewManager manager;
    protected UIDragDropListViewItem mDragDrop;
    protected string messageData;
    protected string messageData2;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        this.mDragDrop = base.GetComponent<UIDragDropListViewItem>();
        this.itemDraw = this.dispObject.GetComponent<ServantStatusFlavorTextListViewItemDraw>();
        this.SetBaseTransform();
        if (this.mDragDrop != null)
        {
            this.mDragDrop.SetEnable(false);
        }
    }

    public ServantStatusFlavorTextListViewItemDraw.Kind GetKind()
    {
        if (this.itemDraw != null)
        {
            return this.itemDraw.GetKind();
        }
        return ServantStatusFlavorTextListViewItemDraw.Kind.NONE;
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
        this.SetVisible(initMode != InitMode.INVISIBLE);
        this.SetInput(initMode == InitMode.INPUT);
        base.transform.localPosition = this.basePosition;
        base.transform.localScale = this.baseScale;
        this.callbackFunc = callbackFunc;
        switch (initMode)
        {
            case InitMode.INVISIBLE:
                this.dispMode = ServantStatusFlavorTextListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ServantStatusFlavorTextListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ServantStatusFlavorTextListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ServantStatusFlavorTextListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;
        }
        this.SetupDisp();
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

    public void OnClickVoice()
    {
        this.manager.SendMessage("OnClickListViewVoice", this);
    }

    private void OnDestroy()
    {
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

    public void SetItem(ServantStatusListViewItem item, int id, bool isOpen, bool isNew, string text, string text2)
    {
        this.mainInfo = item;
        this.id = id;
        this.isOpen = isOpen;
        this.isNew = isNew;
        this.messageData = text;
        this.messageData2 = text2;
        this.Init(InitMode.VALID);
    }

    public void SetManager(ServantStatusFlavorTextListViewManager manager)
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
        this.SetVisible(this.dispMode != ServantStatusFlavorTextListViewItemDraw.DispMode.INVISIBLE);
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(this.mainInfo, this.isOpen, this.isNew, this.messageData, this.messageData2, this.dispMode);
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (this.dispObject != null)
        {
            this.dispObject.SetActive(isVisible);
        }
    }

    public int Id =>
        this.id;

    public bool IsBusy =>
        this.isBusy;

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

