using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardListViewDropObject")]
public class SampleCardListViewDropObject : ListViewDropObject
{
    protected System.Action callbackFunc;
    protected SampleCardListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected SampleCardListViewItemDraw itemDraw;
    protected State state;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<SampleCardListViewItemDraw>();
    }

    protected void CardDragDelete()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
        this.CardMoveEnd();
    }

    protected void CardDragMoveStart(Vector3 position)
    {
        base.isBusy = true;
        if (this.dragObject == null)
        {
            this.dispMode = SampleCardListViewItemDraw.DispMode.INVISIBLE;
            base.SetVisible(false);
            this.SetupDisp();
            this.dragObject = this.CreateDragObject();
            SampleCardListViewDropObject component = this.dragObject.GetComponent<SampleCardListViewDropObject>();
            if (component.itemDraw != null)
            {
                component.itemDraw.AddDepth(-1);
            }
        }
        TweenPosition position2 = TweenPosition.Begin(this.dragObject, 0.2f, this.dragObject.transform.parent.InverseTransformPoint(position));
        position2.method = UITweener.Method.EaseInOut;
        position2.eventReceiver = base.gameObject;
        position2.callWhenFinished = "CardMoveEnd";
    }

    private void CardIntoMove()
    {
        Vector3 pos = this.dragObject.transform.parent.InverseTransformPoint(base.transform.position);
        TweenPosition position = TweenPosition.Begin(this.dragObject, 1.5f, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "CardIntoMove2";
    }

    private void CardIntoMove2()
    {
        base.SetVisible(true);
        this.dispMode = SampleCardListViewItemDraw.DispMode.VALID;
        this.SetupDisp();
        NGUITools.Destroy(this.dragObject);
        this.dragObject = null;
        this.CardMoveEnd();
    }

    private void CardIntoStart(float delay)
    {
        base.isBusy = true;
        this.itemDraw.IsFront = false;
        this.dispMode = SampleCardListViewItemDraw.DispMode.INVISIBLE;
        base.SetVisible(false);
        this.SetupDisp();
        this.dragObject = this.CreateDragObject();
        this.dragObject.transform.position = base.transform.TransformPoint(-3000f, 0f, 0f);
        base.Invoke("CardIntoMove", delay);
    }

    protected void CardMoveEnd()
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

    private void CardTurnMove()
    {
        this.itemDraw.IsFront = !this.itemDraw.IsFront;
        this.SetupDisp();
        TweenScale scale = TweenScale.Begin(base.gameObject, 0.5f, base.baseScale);
        scale.method = UITweener.Method.EaseInOut;
        scale.eventReceiver = base.gameObject;
        scale.callWhenFinished = "CardTurnMove2";
    }

    private void CardTurnMove2()
    {
        this.CardMoveEnd();
    }

    private void CardTurnStart()
    {
        base.isBusy = true;
        this.SetupDisp();
        TweenScale scale = TweenScale.Begin(base.gameObject, 0.5f, new Vector3(0f, this.baseScale.y, this.baseScale.z));
        scale.method = UITweener.Method.EaseInOut;
        scale.eventReceiver = base.gameObject;
        scale.callWhenFinished = "CardTurnMove";
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        SampleCardListViewDropObject component = obj2.GetComponent<SampleCardListViewDropObject>();
        component.IsFront = this.IsFront;
        component.Init(InitMode.VALID);
        return obj2;
    }

    public SampleCardListViewItem GetItem() => 
        (base.linkItem as SampleCardListViewItem);

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
        SampleCardListViewItem linkItem = base.linkItem as SampleCardListViewItem;
        SampleCardListViewItemDraw.DispMode dispMode = this.dispMode;
        bool flag = this.state == State.INIT;
        if (linkItem != null)
        {
            if ((initMode == InitMode.INPUT) && !linkItem.IsDeck)
            {
                initMode = InitMode.INVALID;
            }
        }
        else
        {
            switch (initMode)
            {
                case InitMode.INTO:
                    this.itemDraw.IsFront = false;
                    break;

                case InitMode.TURN:
                    this.itemDraw.IsFront = !this.itemDraw.IsFront;
                    break;
            }
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
                this.dispMode = SampleCardListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = SampleCardListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = SampleCardListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INTO:
                this.dispMode = SampleCardListViewItemDraw.DispMode.VALID;
                this.state = State.MOVE;
                this.CardIntoStart(delay);
                return;

            case InitMode.TURN:
                this.state = State.MOVE;
                this.CardTurnStart();
                return;

            case InitMode.INPUT:
                this.dispMode = SampleCardListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.DRAG_MOVE:
                this.state = State.MOVE;
                this.CardDragMoveStart(position);
                return;

            case InitMode.DRAG_DELETE:
                this.CardDragDelete();
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
        SampleCardListViewItem linkItem = base.linkItem as SampleCardListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != SampleCardListViewItemDraw.DispMode.INVISIBLE));
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

    public bool IsFront
    {
        get => 
            ((this.itemDraw != null) && this.itemDraw.IsFront);
        set
        {
            if (this.itemDraw != null)
            {
                this.itemDraw.IsFront = value;
            }
        }
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        FADEIN,
        INTO,
        TURN,
        INPUT,
        DRAG_MOVE,
        DRAG_DELETE
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

