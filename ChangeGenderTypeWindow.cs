using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChangeGenderTypeWindow : BaseDialog
{
    private int changeGenderType;
    protected System.Action closeCallbackFunc;
    private int currentGenderType;
    [SerializeField]
    private UIButton femaleBtn;
    [SerializeField]
    private UILabel femaleTxt;
    [SerializeField]
    private UILabel inputInfoLb;
    [SerializeField]
    private UIButton maleBtn;
    [SerializeField]
    private UILabel maleTxt;
    protected State state;
    [SerializeField]
    private UILabel titleLb;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result, int changeType)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, this.changeGenderType);
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
            this.state = State.CLOSE;
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickFemale()
    {
        this.changeGenderType = 2;
        if (this.currentGenderType.Equals(this.changeGenderType))
        {
            this.Callback(false, this.currentGenderType);
        }
        else
        {
            this.Callback(true, this.changeGenderType);
        }
    }

    public void OnClickMale()
    {
        this.changeGenderType = 1;
        if (this.currentGenderType.Equals(this.changeGenderType))
        {
            this.Callback(false, this.currentGenderType);
        }
        else
        {
            this.Callback(true, this.changeGenderType);
        }
    }

    public void OpenChangeGenderWindow(int currentType, CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        this.currentGenderType = currentType;
        this.callbackFunc = callback;
        this.titleLb.text = LocalizationManager.Get("CHANGE_GENDER_TITLE");
        this.inputInfoLb.text = LocalizationManager.Get("CHANGE_GENDER_INFO");
        this.femaleTxt.text = LocalizationManager.Get("FEMALE_TEXT");
        this.maleTxt.text = LocalizationManager.Get("MALE_TEXT");
        this.state = State.OPEN;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void CallbackFunc(bool result, int changeType);

    protected enum State
    {
        INIT,
        OPEN,
        CLOSE
    }
}

