using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("ScriptAction/ScriptSelect/ScriptSelectListViewObject")]
public class ScriptSelectListViewObject : ListViewObject
{
    protected ScriptSelectListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected ScriptSelectListViewItemDraw itemDraw;
    protected UIMessageButton messageButton;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<ScriptSelectListViewItemDraw>();
        this.messageButton = base.gameObject.GetComponent<UIMessageButton>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<ScriptSelectListViewObject>().Init(InitMode.VALID);
        return obj2;
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

    public ScriptSelectListViewItem GetItem() => 
        (base.linkItem as ScriptSelectListViewItem);

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
        ScriptSelectListViewItem linkItem = base.linkItem as ScriptSelectListViewItem;
        ScriptSelectListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INTO:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.EventMoveEnd();
                return;

            case InitMode.INPUT:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.NO_SELECT:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.NoSelectStart();
                return;

            case InitMode.SELECT:
                this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.MOVE;
                this.SelectStart();
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

    private void NoSelectMove()
    {
        if (this.itemDraw != null)
        {
            this.itemDraw.NoSelectDecide(new System.Action(this.NoSelectMove2));
        }
        else
        {
            this.NoSelectMove2();
        }
    }

    private void NoSelectMove2()
    {
        base.SetVisible(false);
        this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        this.EventMoveEnd();
    }

    private void NoSelectStart()
    {
        base.isBusy = true;
        base.Invoke("NoSelectMove", 0.1f);
    }

    private void OnDestroy()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    private void SelectMove()
    {
        if (this.itemDraw != null)
        {
            this.itemDraw.SelectDecide(new System.Action(this.SelectMove2));
        }
        else
        {
            this.SelectMove2();
        }
    }

    private void SelectMove2()
    {
        base.SetVisible(false);
        this.dispMode = ScriptSelectListViewItemDraw.DispMode.INVISIBLE;
        this.SetupDisp();
        this.EventMoveEnd();
    }

    private void SelectStart()
    {
        base.isBusy = true;
        base.Invoke("SelectMove", 0.1f);
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        this.messageButton.enabled = true;
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
        ScriptSelectListViewItem linkItem = base.linkItem as ScriptSelectListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != ScriptSelectListViewItemDraw.DispMode.INVISIBLE));
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
        INTO,
        INPUT,
        NO_SELECT,
        SELECT
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

