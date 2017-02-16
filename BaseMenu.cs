using System;
using UnityEngine;

public class BaseMenu : BaseMonoBehaviour
{
    protected System.Action baseCallbackFunc;
    [SerializeField]
    protected UIPanel[] basePanelList;
    protected Vector3 basePosition;
    [SerializeField]
    protected GameObject baseWindow;
    protected static readonly float CLOSE_MOVE_TIME = 0.1f;
    protected static readonly float CLOSE_TIME = 0.2f;
    [SerializeField]
    protected Transform closeTransform;
    [SerializeField]
    protected Transform enterTransform;
    private bool isBasePosition;
    protected bool isInput;
    protected bool isOpen;
    protected bool isSelected;
    protected static readonly float OPEN_MOVE_TIME = 0.1f;
    protected static readonly float OPEN_TIME = 0.2f;

    public void Close(System.Action callback)
    {
        this.baseCallbackFunc = callback;
        this.isOpen = false;
        this.isInput = false;
        bool flag = true;
        if ((this.baseWindow != null) && (this.closeTransform != null))
        {
            Vector3 pos = this.baseWindow.transform.parent.InverseTransformPoint(this.closeTransform.position);
            TweenPosition position = TweenPosition.Begin(this.baseWindow, CLOSE_MOVE_TIME, pos);
            if (position != null)
            {
                position.method = UITweener.Method.EaseInOut;
                position.eventReceiver = base.gameObject;
                position.callWhenFinished = "EndCloseBaseDialog";
                flag = false;
            }
            else
            {
                this.baseWindow.transform.localPosition = this.basePosition;
            }
        }
        if (flag)
        {
            this.EndCloseBaseDialog();
        }
    }

    protected void EndCloseBaseDialog()
    {
        this.Init();
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    protected void EndOpenBaseDialog()
    {
        this.isInput = true;
        if (this.baseCallbackFunc != null)
        {
            System.Action baseCallbackFunc = this.baseCallbackFunc;
            this.baseCallbackFunc = null;
            baseCallbackFunc();
        }
    }

    public void Init()
    {
        if ((this.baseWindow != null) && !this.isBasePosition)
        {
            this.isBasePosition = true;
            this.basePosition = this.baseWindow.transform.localPosition;
        }
        base.gameObject.SetActive(false);
        this.isOpen = false;
        this.isInput = false;
        this.isSelected = false;
        if ((this.baseWindow != null) && (this.closeTransform != null))
        {
            this.baseWindow.transform.position = this.closeTransform.position;
        }
    }

    public void Open(System.Action callback)
    {
        base.gameObject.SetActive(true);
        this.baseCallbackFunc = callback;
        this.isOpen = true;
        this.isInput = false;
        this.isSelected = false;
        bool flag = true;
        if (this.baseWindow != null)
        {
            if (!this.isBasePosition)
            {
                this.isBasePosition = true;
                this.basePosition = this.baseWindow.transform.localPosition;
            }
            if (this.closeTransform != null)
            {
                this.baseWindow.transform.position = this.closeTransform.position;
                TweenPosition position = TweenPosition.Begin(this.baseWindow, OPEN_MOVE_TIME, this.basePosition);
                if (position != null)
                {
                    position.method = UITweener.Method.EaseInOut;
                    position.eventReceiver = base.gameObject;
                    position.callWhenFinished = "EndOpenBaseDialog";
                    flag = false;
                }
                else
                {
                    this.baseWindow.transform.localPosition = this.basePosition;
                }
            }
            else
            {
                this.baseWindow.transform.localPosition = this.basePosition;
            }
        }
        if (flag)
        {
            this.EndOpenBaseDialog();
        }
    }

    public bool IsBusy =>
        base.gameObject.activeSelf;

    public bool IsOpen =>
        this.isOpen;
}

