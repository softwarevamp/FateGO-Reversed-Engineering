using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AgeVerificationMenu : BaseDialog
{
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel messageLabel;
    protected static readonly string SAVE_KEY_CUMULATIVE_AMOUNT = "AgeVerificationCumulativeAmount";
    protected static readonly string SAVE_KEY_EXPIRATION_DATE = "AgeVerificationExpirationDate";
    protected static readonly string SAVE_KEY_TYPE = "AgeVerificationType";
    protected int selectType;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;
    [SerializeField]
    protected UILabel type1Label;
    [SerializeField]
    protected UILabel type2Label;
    [SerializeField]
    protected UILabel type3Label;
    protected static string[] typeTextList = new string[] { "AGE_VEIFICATION_NONE", "AGE_VEIFICATION_TYPE1", "AGE_VEIFICATION_TYPE2", "AGE_VEIFICATION_TYPE3" };

    protected event CallbackFunc callbackFunc;

    protected void Callback(int result)
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
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    public static void Concent(int type)
    {
        if (!ManagerConfig.UseMock && !IsConcent())
        {
            long num = NetworkManager.getNextMonthTime();
            PlayerPrefs.SetInt(SAVE_KEY_TYPE, type);
            PlayerPrefs.SetString(SAVE_KEY_EXPIRATION_DATE, string.Empty + num);
            PlayerPrefs.SetInt(SAVE_KEY_CUMULATIVE_AMOUNT, 0);
            PlayerPrefs.Save();
        }
    }

    protected void ConfirmType(int type)
    {
        this.state = State.CONFIRM;
        this.selectType = type;
        SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("AGE_VEIFICATION_CONFIRM_TITLE"), string.Format(LocalizationManager.Get("AGE_VEIFICATION_CONFIRM_MESSAGE"), LocalizationManager.Get(typeTextList[this.selectType])), LocalizationManager.Get("AGE_VEIFICATION_CONFIRM_DECIDE"), LocalizationManager.Get("AGE_VEIFICATION_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.OnEndConfirm));
    }

    protected void EndClose()
    {
        this.Init();
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public static int GetAgeType() => 
        PlayerPrefs.GetInt(SAVE_KEY_TYPE, 0);

    public static int GetCumulativeAmount() => 
        PlayerPrefs.GetInt(SAVE_KEY_CUMULATIVE_AMOUNT, 0);

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.type1Label.text = string.Empty;
        this.type2Label.text = string.Empty;
        this.type3Label.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public static bool IsConcent()
    {
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        int @int = PlayerPrefs.GetInt(SAVE_KEY_TYPE, 0);
        if (@int == 0)
        {
            return false;
        }
        if (@int >= 3)
        {
            return true;
        }
        DateTime time = NetworkManager.getLocalDateTime();
        DateTime time2 = NetworkManager.getDateTime(long.Parse(PlayerPrefs.GetString(SAVE_KEY_EXPIRATION_DATE, "0")));
        return ((time2.Year > time.Year) || ((time2.Year == time.Year) && (time2.Month > time.Month)));
    }

    public static bool IsConcentFirst() => 
        (ManagerConfig.UseMock || (PlayerPrefs.GetInt(SAVE_KEY_TYPE, 0) == 0));

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(-1);
        }
    }

    public void OnClickType1()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ConfirmType(1);
        }
    }

    public void OnClickType2()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ConfirmType(2);
        }
    }

    public void OnClickType3()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ConfirmType(3);
        }
    }

    protected void OnEndConfirm(bool result)
    {
        if (result)
        {
            Concent(this.selectType);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        if (result)
        {
            this.Callback(this.selectType);
        }
        else
        {
            this.state = State.INPUT;
        }
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.titleLabel.text = LocalizationManager.Get("AGE_VEIFICATION_TITLE");
            this.messageLabel.text = LocalizationManager.Get("AGE_VEIFICATION_MESSAGE");
            this.type1Label.text = LocalizationManager.Get(typeTextList[1]);
            this.type2Label.text = LocalizationManager.Get(typeTextList[2]);
            this.type3Label.text = LocalizationManager.Get(typeTextList[3]);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INIT;
        }
    }

    public static void SaveCumulativeAmount(int count)
    {
        PlayerPrefs.SetInt(SAVE_KEY_CUMULATIVE_AMOUNT, count);
    }

    public delegate void CallbackFunc(int result);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        CONFIRM,
        SELECTED,
        CLOSE
    }
}

