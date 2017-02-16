using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/WebConnectMenu")]
public class WebConnectMenu : MonoBehaviour
{
    public UIButton cancelButton;
    public UIButton decideButton;
    public GameObject menuRootObject;
    protected string selectConnectPath;
    protected string settingConnectPath = string.Empty;
    protected State state;
    public UILineInput wwwPathInput;

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
        this.EndInput();
        this.state = State.INIT;
        this.menuRootObject.SetActive(false);
    }

    protected void EndInput()
    {
        if (this.state != State.INIT)
        {
            UIInput component = this.wwwPathInput.GetComponent<UIInput>();
            this.settingConnectPath = component.value;
            component.value = string.Empty;
            this.wwwPathInput.SetInputEnable(false);
            this.decideButton.enabled = false;
            this.cancelButton.enabled = false;
            Input.imeCompositionMode = IMECompositionMode.Auto;
        }
    }

    public void OnChangeServerInput()
    {
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.EndInput();
            this.state = State.SELECTED;
            this.Callback(false);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.selectConnectPath = this.wwwPathInput.GetText();
            this.state = State.SELECTED;
            WebViewManager.OpenView(string.Empty, this.selectConnectPath, new System.Action(this.OnEndWebView));
        }
    }

    protected void OnEndWebView()
    {
        this.Callback(true);
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.menuRootObject.SetActive(true);
            this.wwwPathInput.SetInputEnable(true);
            this.decideButton.enabled = true;
            this.cancelButton.enabled = true;
            this.wwwPathInput.GetComponent<UIInput>().value = this.settingConnectPath;
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INPUT,
        SELECTED,
        CLOSE
    }
}

