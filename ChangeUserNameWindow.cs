using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class ChangeUserNameWindow : BaseDialog
{
    public UILabel cancelTxtLb;
    private string changeName;
    protected System.Action closeCallbackFunc;
    public UIInput inputTarget;
    public UILineInput nameInput;
    public UILabel nameText;
    protected State state;
    public UIButton submitBtn;
    public UILabel submitTxtLb;
    public UILabel titleLb;
    public UILabel titleTxtLb;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result, string changeName)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, this.changeName);
        }
    }

    public void Close()
    {
        Debug.Log("!!** ExpInfoWindow Close");
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

    public void OnChangeInput()
    {
        this.changeName = this.nameInput.GetText();
        bool flag = string.IsNullOrEmpty(this.changeName) || (this.changeName.Trim() == string.Empty);
        Regex regex = new Regex(@"^[a-zA-Z\u4e00-\u9fa5]+$");
        int num = 0;
        if (!flag && regex.IsMatch(this.changeName))
        {
            foreach (char ch in this.changeName)
            {
                if ((ch.GetHashCode() >= 0) && (ch.GetHashCode() < 0xff))
                {
                    num++;
                }
                else
                {
                    num += 2;
                }
            }
            if (num > 12)
            {
                flag = true;
            }
        }
        this.submitBtn.isEnabled = !flag;
    }

    public void OnClickCancel()
    {
        this.Callback(false, string.Empty);
    }

    public void OnClickDecide()
    {
        this.Callback(true, this.changeName);
    }

    public void OpenChangeNameWindow(string usrName, CallbackFunc callback)
    {
        this.submitTxtLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.cancelTxtLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        base.gameObject.SetActive(true);
        this.resetInputVal();
        this.submitBtn.isEnabled = false;
        this.callbackFunc = callback;
        this.titleTxtLb.text = LocalizationManager.Get("CHANGE_TITLE");
        this.titleLb.text = LocalizationManager.Get("CHANGE_NAME_TITLE");
        this.nameText.text = usrName;
        this.state = State.OPEN;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void resetInputVal()
    {
        this.inputTarget.value = string.Empty;
    }

    public delegate void CallbackFunc(bool result, string changeName);

    protected enum State
    {
        INIT,
        OPEN,
        CLOSE
    }
}

