using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialEventLogListViewObject : ListViewObject
{
    protected MaterialEventLogListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected MaterialEventLogListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<MaterialEventLogListViewItemDraw>();
    }

    public MaterialEventLogListViewItem GetItem() => 
        (base.linkItem as MaterialEventLogListViewItem);

    public void Init(InitMode initMode)
    {
        this.Init(initMode, null);
    }

    public void Init(InitMode initMode, System.Action callbackFunc)
    {
        this.Init(initMode, callbackFunc, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay, Vector3 position)
    {
        MaterialEventLogListViewItem linkItem = base.linkItem as MaterialEventLogListViewItem;
        MaterialEventLogListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = MaterialEventLogListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = MaterialEventLogListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = MaterialEventLogListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = MaterialEventLogListViewItemDraw.DispMode.VALID;
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

    protected void InitItem()
    {
        this.state = State.INIT;
    }

    private void LateUpdate()
    {
        MaterialEventLogListViewItem linkItem = base.linkItem as MaterialEventLogListViewItem;
        if (linkItem != null)
        {
            this.itemDraw.LateUpdateItem(linkItem, this.dispMode);
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
        if (this.itemDraw != null)
        {
            MaterialEventLogListViewItem linkItem = base.linkItem as MaterialEventLogListViewItem;
            this.itemDraw.SetInput(linkItem, isInput);
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
        MaterialEventLogListViewItem linkItem = base.linkItem as MaterialEventLogListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != MaterialEventLogListViewItemDraw.DispMode.INVISIBLE));
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
        INPUT
    }
}

