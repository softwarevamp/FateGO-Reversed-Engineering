using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestBoardListViewObject : ListViewObject
{
    protected QuestBoardListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected QuestBoardListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<QuestBoardListViewItemDraw>();
        this.itemDraw.SetListViewObject(this);
    }

    public QuestBoardListViewItem GetItem() => 
        (base.linkItem as QuestBoardListViewItem);

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
        QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
        QuestBoardListViewItemDraw.DispMode dispMode = this.dispMode;
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
                this.dispMode = QuestBoardListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = QuestBoardListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = QuestBoardListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = QuestBoardListViewItemDraw.DispMode.VALID;
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

    public bool IsStateInput() => 
        (this.state == State.INPUT);

    private void LateUpdate()
    {
        if (base.linkItem != null)
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
            if (linkItem != null)
            {
                QuestBoardListViewManager qmanager = base.manager as QuestBoardListViewManager;
                this.itemDraw.LateUpdateItem(linkItem, this.dispMode, qmanager);
            }
        }
    }

    public void OnChangeAlphaAnim()
    {
        if (base.linkItem != null)
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
            if (linkItem != null)
            {
                QuestBoardListViewManager qmanager = base.manager as QuestBoardListViewManager;
                this.itemDraw.OnChangeAlphaAnim(linkItem, this.dispMode, qmanager);
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

    private void OnDragStart()
    {
        if (base.linkItem != null)
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
            if (linkItem != null)
            {
                QuestBoardListViewManager qmanager = base.manager as QuestBoardListViewManager;
                base.OnDragStart();
                this.itemDraw.OnDragStartItem(linkItem, this.dispMode, qmanager);
            }
        }
    }

    private void OnPress(bool is_press)
    {
        if ((Input.touchCount < 2) && (base.linkItem != null))
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
            if (linkItem != null)
            {
                QuestBoardListViewManager qmanager = base.manager as QuestBoardListViewManager;
                if (is_press)
                {
                    this.itemDraw.OnPressItem(linkItem, this.dispMode, qmanager);
                }
                else
                {
                    this.itemDraw.OnPullItem(linkItem, this.dispMode, qmanager);
                }
            }
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        if (this.itemDraw != null)
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
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

    public void SetupDisp()
    {
        QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != QuestBoardListViewItemDraw.DispMode.INVISIBLE));
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

    private void Update()
    {
        if (base.linkItem != null)
        {
            QuestBoardListViewItem linkItem = base.linkItem as QuestBoardListViewItem;
            if (linkItem != null)
            {
                QuestBoardListViewManager qmanager = base.manager as QuestBoardListViewManager;
                this.itemDraw.UpdateItem(linkItem, this.dispMode, qmanager);
            }
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

