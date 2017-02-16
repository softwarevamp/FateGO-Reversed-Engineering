using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TripleButtonDlgComponent : BaseDialog
{
    [SerializeField]
    protected UILabel closeBtnLb;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel confirmDetailLabel;
    [SerializeField]
    protected UILabel confirmTitleLabel;
    protected readonly Vector3 messagePosNormal = new Vector3(0f, 10f, 0f);
    protected readonly Vector3 messagePosNoTitle = new Vector3(0f, 30f, 0f);
    [SerializeField]
    protected UILabel middleBtnLb;
    [SerializeField]
    protected UILabel rightBtnLb;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultClicked result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        if (this.state == State.OPEN)
        {
            this.state = State.INIT;
        }
    }

    public void Init()
    {
        this.confirmTitleLabel.text = string.Empty;
        this.confirmDetailLabel.text = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.Callback(ResultClicked.CANCEL);
    }

    public void OnClickMiddle()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.Callback(ResultClicked.MIDDLE);
    }

    public void OnClickRight()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.Callback(ResultClicked.RIGHT);
    }

    public void Open(string title, string msg, string closeBtnText, string middleBtnText, string rightBtnText, CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        this.callbackFunc = callback;
        this.state = State.OPEN;
        if (this.confirmTitleLabel != null)
        {
            this.confirmTitleLabel.text = (title == null) ? string.Empty : title;
            if (this.confirmTitleLabel.text.Length == 0)
            {
                this.confirmDetailLabel.transform.localPosition = this.messagePosNoTitle;
            }
            else
            {
                this.confirmDetailLabel.transform.localPosition = this.messagePosNormal;
            }
        }
        if (this.confirmDetailLabel != null)
        {
            this.confirmDetailLabel.text = (msg == null) ? string.Empty : msg;
        }
        if (this.closeBtnLb != null)
        {
            this.closeBtnLb.text = closeBtnText;
        }
        if (this.middleBtnLb != null)
        {
            this.middleBtnLb.text = middleBtnText;
        }
        if (this.rightBtnLb != null)
        {
            this.rightBtnLb.text = rightBtnText;
        }
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void CallbackFunc(TripleButtonDlgComponent.ResultClicked result);

    public enum ResultClicked
    {
        RIGHT,
        MIDDLE,
        CANCEL
    }

    protected enum State
    {
        INIT,
        OPEN
    }
}

