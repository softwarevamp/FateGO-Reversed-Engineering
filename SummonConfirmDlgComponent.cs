using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonConfirmDlgComponent : BaseDialog
{
    [SerializeField]
    protected UIButton closeBtn;
    [SerializeField]
    protected UILabel closeBtnLb;
    [SerializeField]
    protected GameObject closeBtnObject;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel confirmBtnLb;
    [SerializeField]
    protected GameObject confirmBtnObject;
    [SerializeField]
    protected UILabel confirmDetailLabel;
    [SerializeField]
    protected UILabel confirmTitleLabel;
    protected SeManager.SystemSeKind seKind;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
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
    }

    public void Init()
    {
        this.confirmTitleLabel.text = string.Empty;
        this.confirmDetailLabel.text = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        this.seKind = SeManager.SystemSeKind.DECIDE;
        base.Init();
    }

    public void OnClickCancel()
    {
        this.Callback(false);
    }

    public void OnClickDecide()
    {
        this.Callback(true);
    }

    public void Open(string title, string msg, string decideTxt, CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        this.callbackFunc = callback;
        if (this.confirmTitleLabel != null)
        {
            this.confirmTitleLabel.text = (title == null) ? string.Empty : title;
        }
        if (this.confirmDetailLabel != null)
        {
            this.confirmDetailLabel.text = (msg == null) ? string.Empty : msg;
        }
        if (this.confirmBtnLb != null)
        {
            this.confirmBtnLb.text = (decideTxt == null) ? LocalizationManager.Get("COMMON_CONFIRM_DECIDE") : decideTxt;
        }
        this.setBtnInfoActive();
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void OpenConfirmFree(CallbackFunc callback)
    {
        int dailyFreeGachaResetTime = BalanceConfig.DailyFreeGachaResetTime;
        string str = (dailyFreeGachaResetTime >= 10) ? dailyFreeGachaResetTime.ToString() : ("0" + dailyFreeGachaResetTime);
        string msg = string.Format(LocalizationManager.Get("CONFIRM_FREESUMMON_MSG"), str);
        this.state = State.CONFIRM_FREE;
        this.Open(null, msg, null, callback);
    }

    public void OpenConfirmPoint(int havePointNum, int needPointNum, int afterPointNum, CallbackFunc callback)
    {
        string msg = string.Format(LocalizationManager.Get("CONFIRM_POINTSUMMON_MSG"), needPointNum, havePointNum, afterPointNum);
        this.state = State.CONFIRM_POINT;
        this.Open(null, msg, null, callback);
    }

    public void OpenConfirmStone(PayType.Type type, int price, int haveStoneNum, int haveFreeStoneNum, int haveChargeStoneNum, int afterStoneNum, int afterFreeStoneNum, int afterChargeStoneNum, CallbackFunc callback)
    {
        string format = (type != PayType.Type.CHARGE_STONE) ? LocalizationManager.Get("CONFIRM_PAYSUMMON_MSG") : LocalizationManager.Get("CONFIRM_CHARGESUMMON_MSG");
        string msg = string.Format(format, new object[] { price, haveStoneNum, haveFreeStoneNum, haveChargeStoneNum, afterStoneNum, afterFreeStoneNum, afterChargeStoneNum });
        this.state = State.CONFIRM_STONE;
        this.Open(null, msg, null, callback);
    }

    public void OpenConfirmTicket(int haveTicketNum, int afterTicketNum, CallbackFunc callback)
    {
        string msg = string.Format(LocalizationManager.Get("CONFIRM_TICKETSUMMON_MSG"), haveTicketNum, afterTicketNum);
        this.state = State.CONFIRM_TICKET;
        this.Open(null, msg, null, callback);
    }

    public void OpenShortChargeStone(int price, int haveChargeStoneNum, int haveFreeStoneNum, CallbackFunc callback)
    {
        string title = string.Empty;
        string msg = string.Format(LocalizationManager.Get("SHORT_HAVE_CHARGE_STONE"), price, haveChargeStoneNum, haveFreeStoneNum.ToString("#,0"));
        string decideTxt = LocalizationManager.Get("STONE_PURCHASE");
        this.state = State.SHORT_STONE;
        this.Open(title, msg, decideTxt, callback);
    }

    public void OpenShortPoint(int havePoint, CallbackFunc callback)
    {
        string title = LocalizationManager.Get("SHORT_DLG_TITLE");
        string msg = string.Format(LocalizationManager.Get("SHORT_HAVE_POINT"), havePoint);
        this.state = State.SHORT_POINT;
        this.Open(title, msg, null, callback);
    }

    public void OpenShortStone(int haveNum, CallbackFunc callback)
    {
        string title = LocalizationManager.Get("SHORT_DLG_TITLE");
        string msg = string.Format(LocalizationManager.Get("SHORT_HAVE_STONE"), haveNum);
        string decideTxt = LocalizationManager.Get("STONE_PURCHASE");
        this.state = State.SHORT_STONE;
        this.Open(title, msg, decideTxt, callback);
    }

    private void setBtnInfoActive()
    {
        switch (this.state)
        {
            case State.SHORT_SERVANT:
            case State.SHORT_STONE:
                if (this.closeBtnObject.activeSelf)
                {
                    this.closeBtnObject.SetActive(false);
                }
                this.confirmBtnObject.SetActive(true);
                break;

            case State.SHORT_POINT:
                if (this.confirmBtnObject.activeSelf)
                {
                    this.confirmBtnObject.SetActive(false);
                }
                this.closeBtnObject.SetActive(true);
                break;

            case State.CONFIRM_STONE:
            case State.CONFIRM_TICKET:
            case State.CONFIRM_POINT:
            case State.CONFIRM_FREE:
                if (this.closeBtnObject.activeSelf)
                {
                    this.closeBtnObject.SetActive(false);
                }
                this.confirmBtnObject.SetActive(true);
                break;
        }
    }

    public void setTutorial(bool isTutorial)
    {
        this.closeBtn.isEnabled = isTutorial;
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        SHORT_SERVANT,
        SHORT_STONE,
        SHORT_POINT,
        CONFIRM_STONE,
        CONFIRM_TICKET,
        CONFIRM_POINT,
        CONFIRM_FREE
    }
}

