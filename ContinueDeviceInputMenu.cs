using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ContinueDeviceInputMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILineInput passward1Input;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(string result)
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
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        this.decideLabel.text = string.Empty;
        this.passward1Input.GetComponent<UIInput>().value = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnChangeInput()
    {
        string text = this.passward1Input.GetText();
        bool flag = true;
        if (text.Length > 0)
        {
            if (text.Length < 4)
            {
                flag = false;
            }
            if (flag)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char ch = text[i];
                    if ((((ch < 'A') || (ch > 'Z')) && ((ch < 'a') || (ch > 'z'))) && ((ch < '0') || (ch > '9')))
                    {
                        flag = false;
                        break;
                    }
                }
            }
        }
        else
        {
            flag = false;
        }
        this.decideButton.SetState(!flag ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(null);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            string text = this.passward1Input.GetText();
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(text);
        }
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.passward1Input.SetInputEnable(true);
            this.decideButton.SetState(UICommonButtonColor.State.Disabled, false);
            this.titleLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_INPUT_TITLE");
            this.decideLabel.text = LocalizationManager.Get("CONTINUE_DEVICE_INPUT_DECIDE");
            UIInput component = this.passward1Input.GetComponent<UIInput>();
            component.value = string.Empty;
            string str = LocalizationManager.Get("CONTINUE_DEVICE_INPUT_EXPLANATIOIN3");
            component.defaultText = str;
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INPUT;
        }
    }

    public void RepeatInputCode(CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.state = State.INPUT;
    }

    public delegate void CallbackFunc(string result);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

