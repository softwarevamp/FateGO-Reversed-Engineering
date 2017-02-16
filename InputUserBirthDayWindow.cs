using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputUserBirthDayWindow : BaseDialog
{
    [SerializeField]
    private UILineInput birthDayInput;
    [SerializeField]
    private UILineInput birthMonthInput;
    [SerializeField]
    private UIButton cancelBtn;
    [SerializeField]
    private UILabel cancelTxt;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    private UIButton confirmBtn;
    [SerializeField]
    private UISprite confirmBtnBg;
    [SerializeField]
    private UILabel confirmTxt;
    [SerializeField]
    private UILabel inputInfoLb;
    private bool isInput;
    [SerializeField]
    private UILabel noticeLb;
    private int[] paramList;
    protected State state;
    [SerializeField]
    private UILabel titleLb;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result, int[] param)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, this.paramList);
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

    protected void closeNotification()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
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
            this.state = State.CLOSE;
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        Input.imeCompositionMode = IMECompositionMode.Auto;
        this.state = State.INIT;
        base.Init();
    }

    public void OnChangeInput()
    {
        string text = this.birthMonthInput.GetText();
        string str2 = this.birthDayInput.GetText();
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(str2))
        {
            this.isInput = false;
        }
        else
        {
            this.isInput = true;
        }
        this.setExeBtnState();
    }

    public void OnClickCancel()
    {
        this.Callback(false, this.paramList);
    }

    public void OnClickDecide()
    {
        if (this.isInput)
        {
            try
            {
                int month = int.Parse(this.birthMonthInput.GetText());
                int day = int.Parse(this.birthDayInput.GetText());
                Debug.Log(string.Empty + new DateTime(0x7d0, month, day).ToString());
                if ((month == 2) && (day == 0x1d))
                {
                    throw new Exception();
                }
                this.paramList[0] = month;
                this.paramList[1] = day;
                this.Callback(true, this.paramList);
            }
            catch
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("CHECK_BIRTHDAY_TITLE_MESSAGE"), LocalizationManager.Get("CHECK_BIRTHDAY_MESSAGE"), new System.Action(this.closeNotification), -1);
            }
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void OpenInputBirthDayWindow(CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        Input.imeCompositionMode = IMECompositionMode.On;
        this.isInput = false;
        this.setExeBtnState();
        this.callbackFunc = callback;
        this.titleLb.text = LocalizationManager.Get("INPUT_BIRTHDAY_TITLE");
        this.inputInfoLb.text = LocalizationManager.Get("INPUT_BIRTHDAY_INFO");
        this.noticeLb.text = LocalizationManager.Get("INPUT_BIRTHDAY_NOTICE");
        this.state = State.OPEN;
        this.paramList = new int[2];
        base.Open(new System.Action(this.EndOpen), true);
    }

    private void setExeBtnState()
    {
        UIWidget component = this.confirmBtnBg.GetComponent<UIWidget>();
        Debug.Log("!!** setExeBtnState BgWidget : " + component);
        if (this.isInput)
        {
            this.confirmTxt.color = Color.black;
            component.color = Color.white;
        }
        else
        {
            this.confirmTxt.color = Color.black;
            component.color = Color.gray;
        }
    }

    public delegate void CallbackFunc(bool result, int[] param);

    protected enum State
    {
        INIT,
        OPEN,
        CLOSE
    }
}

