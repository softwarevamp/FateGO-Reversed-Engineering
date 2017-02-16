using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PresentBoxNotificationMenu : BaseDialog
{
    [SerializeField]
    protected UICommonButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected PresentBoxNotificationListViewManager ItemListViewManager;
    [SerializeField]
    protected UILabel message1Label;
    [SerializeField]
    protected UILabel message2Label;
    protected int selectItemNum;
    protected State state;

    protected event CallbackFunc callbackFunc;

    public void BackBuyBankItem()
    {
        if (this.state == State.INPUT_PRESENT_CHECK)
        {
            this.state = State.QUIT_PRESENT_CHECK;
            base.Invoke("OnMoveEnd", 0.1f);
        }
    }

    protected void Callback(Result result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(result);
        }
    }

    public void Close(System.Action callback)
    {
        if (this.state != State.INIT)
        {
            this.closeCallbackFunc = callback;
            this.state = State.QUIT_PRESENT_CHECK;
            base.Close(new System.Action(this.OnMoveEnd));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void Init()
    {
        this.message1Label.text = string.Empty;
        this.message2Label.text = string.Empty;
        this.closeLabel.text = string.Empty;
        this.ItemListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickClose()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.Callback(Result.CANCEL);
    }

    private void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_PRESENT_CHECK:
                this.state = State.INIT_PRESENT_CHECK2;
                break;

            case State.INIT_PRESENT_CHECK2:
                this.state = State.INPUT_PRESENT_CHECK;
                this.ItemListViewManager.SetMode(PresentBoxNotificationListViewManager.InitMode.INPUT, new PresentBoxNotificationListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_PRESENT_CHECK:
                this.state = State.QUIT_PRESENT_CHECK2;
                break;

            case State.QUIT_PRESENT_CHECK2:
                this.Init();
                if (this.closeCallbackFunc != null)
                {
                    System.Action closeCallbackFunc = this.closeCallbackFunc;
                    this.closeCallbackFunc = null;
                    closeCallbackFunc();
                }
                break;
        }
    }

    protected void OnSelectBuyItem(int n)
    {
        this.selectItemNum = n;
    }

    public void Open(UserPresentBoxEntity[] presentList, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.ItemListViewManager.IsInput = false;
            this.message1Label.text = LocalizationManager.Get("PRESENT_BOX_NOTIFICATION_MESSAGE1");
            this.message2Label.text = LocalizationManager.Get("PRESENT_BOX_NOTIFICATION_MESSAGE2");
            this.closeLabel.text = LocalizationManager.Get("PRESENT_BOX_NOTIFICATION_CLOSE");
            this.ItemListViewManager.CreateList(PresentBoxNotificationListViewManager.Kind.NORMAL, presentList);
            this.state = State.INIT_PRESENT_CHECK;
            base.Open(new System.Action(this.OnMoveEnd), true);
        }
    }

    public delegate void CallbackFunc(PresentBoxNotificationMenu.Result result);

    public enum Result
    {
        CANCEL
    }

    protected enum State
    {
        INIT,
        OPEN,
        INIT_PRESENT_CHECK,
        INIT_PRESENT_CHECK2,
        INPUT_PRESENT_CHECK,
        QUIT_PRESENT_CHECK,
        QUIT_PRESENT_CHECK2
    }
}

