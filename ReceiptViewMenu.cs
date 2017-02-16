using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ReceiptViewMenu")]
public class ReceiptViewMenu : MonoBehaviour
{
    public UIButton cancelButton;
    public GameObject menuRootObject;
    protected State state;
    public UITextList textList;

    protected event CallbackFunc callbackFunc;

    protected void Callback()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc();
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
            this.menuRootObject.SetActive(false);
        }
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.EndInput();
            this.state = State.SELECTED;
            this.Callback();
        }
    }

    public void Open(string data, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.textList.Clear();
            this.textList.Add(data);
            this.menuRootObject.SetActive(true);
            this.cancelButton.enabled = true;
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc();

    protected enum State
    {
        INIT,
        INPUT,
        SELECTED,
        CLOSE
    }
}

