using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptTextListViewObject")]
public class ScriptTextListViewObject : ListViewObject
{
    protected ScriptTextListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected ScriptTextListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<ScriptTextListViewItemDraw>();
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<ScriptTextListViewObject>().Init(InitMode.VALID);
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

    public ScriptTextListViewItem GetItem() => 
        (base.linkItem as ScriptTextListViewItem);

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
        ScriptTextListViewItem linkItem = base.linkItem as ScriptTextListViewItem;
        ScriptTextListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = ScriptTextListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = ScriptTextListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = ScriptTextListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = ScriptTextListViewItemDraw.DispMode.VALID;
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

    public void OnClickSingle()
    {
        if (base.linkItem != null)
        {
            base.manager.SendMessage("OnClickSingleListView", this);
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
            base.manager.SendMessage("OnLongPushListView", this);
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
        ScriptTextListViewItem linkItem = base.linkItem as ScriptTextListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != ScriptTextListViewItemDraw.DispMode.INVISIBLE));
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

