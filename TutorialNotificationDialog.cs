using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TutorialNotificationDialog : BaseDialog
{
    protected System.Action closeCallbackFunc;
    protected TutorialFlag.Id flagId;
    protected bool isButtonEnable;
    [SerializeField]
    protected UILabel messageLabel;

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.isButtonEnable = false;
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
        this.isButtonEnable = true;
    }

    protected void EndTurorialRequest(string result)
    {
        this.Close(this.closeCallbackFunc);
    }

    public void Init()
    {
        if (this.messageLabel != null)
        {
            this.messageLabel.text = string.Empty;
        }
        this.isButtonEnable = false;
        base.gameObject.SetActive(false);
        base.Init();
    }

    public void OnClickClose()
    {
        if (this.isButtonEnable)
        {
            this.isButtonEnable = false;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            if (this.flagId == TutorialFlag.Id.NULL)
            {
                this.Close(this.closeCallbackFunc);
            }
            else
            {
                NetworkManager.getRequest<TutorialSetRequest>(new NetworkManager.ResultCallbackFunc(this.EndTurorialRequest)).beginRequest(this.flagId);
            }
        }
    }

    public void Open(string message, TutorialFlag.Id flagId = -1, System.Action func = null)
    {
        this.closeCallbackFunc = func;
        this.flagId = flagId;
        if (this.messageLabel != null)
        {
            this.messageLabel.text = (message == null) ? string.Empty : message;
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }
}

