using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantFrameShortDlgComponent : BaseDialog
{
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UISprite confirmBtnImg;
    [SerializeField]
    protected UILabel confirmBtnLb;
    [SerializeField]
    protected UILabel confirmDetailLabel;
    [SerializeField]
    protected UILabel confirmTitleLabel;
    protected bool framePurchasable;
    [SerializeField]
    protected UILabel pwUpBtnLb;
    [SerializeField]
    protected UILabel sellBtnLb;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(resultClicked result)
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
        this.Callback(resultClicked.CANCEL);
    }

    public void OnClickDecide()
    {
        if (this.framePurchasable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(resultClicked.CONFIRM);
        }
    }

    public void OnClickPwUp()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.Callback(resultClicked.POWERUP);
    }

    public void OnClickSell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.Callback(resultClicked.SELL);
    }

    public void OpenShortSvt(int haveNum, int maxNum, bool is_equip, bool isQuest, CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        this.callbackFunc = callback;
        this.state = State.OPEN;
        string str = LocalizationManager.Get("SHORT_DLG_TITLE");
        string str2 = !is_equip ? LocalizationManager.Get("SHORT_SERVANT_SERVANT") : LocalizationManager.Get("SHORT_SERVANT_EQUIP");
        string str3 = !isQuest ? LocalizationManager.Get("SHORT_SERVANT_SUMMON") : LocalizationManager.Get("SHORT_SERVANT_QUEST");
        string str4 = string.Format(!isQuest ? LocalizationManager.Get("SHORT_SERVANT_FRAME") : LocalizationManager.Get("SHORT_SERVANT_FRAME_QUEST"), new object[] { str2, str3, haveNum, maxNum });
        if (this.confirmTitleLabel != null)
        {
            this.confirmTitleLabel.text = (str == null) ? string.Empty : str;
        }
        if (this.confirmDetailLabel != null)
        {
            this.confirmDetailLabel.text = (str4 == null) ? string.Empty : str4;
        }
        if (this.pwUpBtnLb != null)
        {
            this.pwUpBtnLb.text = !is_equip ? LocalizationManager.Get("SHORT_SERVANT_COMBINE") : LocalizationManager.Get("SHORT_SERVANT_EQ_COMBINE");
        }
        if (this.sellBtnLb != null)
        {
            this.sellBtnLb.text = !is_equip ? LocalizationManager.Get("SHORT_SERVANT_SELL") : LocalizationManager.Get("SHORT_SERVANT_EQ_SELL");
        }
        if (this.confirmBtnLb != null)
        {
            this.confirmBtnLb.text = !is_equip ? LocalizationManager.Get("SERVANT_FRAME_PURCHASE") : LocalizationManager.Get("SERVANT_EQUIP_FRAME_PURCHASE");
        }
        if (maxNum >= (!is_equip ? BalanceConfig.ServantFrameMax : BalanceConfig.ServantEquipFrameMax))
        {
            this.framePurchasable = false;
            this.confirmBtnImg.color = Color.gray;
        }
        else
        {
            this.framePurchasable = true;
        }
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void CallbackFunc(ServantFrameShortDlgComponent.resultClicked result);

    public enum resultClicked
    {
        CONFIRM,
        POWERUP,
        SELL,
        CANCEL
    }

    protected enum State
    {
        INIT,
        OPEN
    }
}

