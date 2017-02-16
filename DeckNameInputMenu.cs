using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeckNameInputMenu : BaseDialog
{
    protected string baseName;
    [SerializeField]
    protected UILabel cancelLabel;
    protected string changeName;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel decideLabel;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UIInput inputTarget;
    [SerializeField]
    protected UILineInput nameInput;
    [SerializeField]
    protected UILabel nameText;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result, string name)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, name);
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
        this.inputTarget.GetComponent<Collider>().enabled = false;
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
            this.state = State.INPUT;
            this.inputTarget.GetComponent<Collider>().enabled = true;
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        this.inputTarget.GetComponent<Collider>().enabled = false;
        base.Init();
    }

    public void OnChangeInput()
    {
        this.changeName = this.nameInput.GetText();
        bool flag = (string.IsNullOrEmpty(this.changeName) || (this.changeName.Trim() == string.Empty)) || (this.changeName == this.baseName);
        if (!flag)
        {
            foreach (char ch in this.changeName)
            {
                if ((ch >= 0xd800) && (ch < 0xe000))
                {
                    flag = true;
                    break;
                }
            }
        }
        this.decideButton.isEnabled = !flag;
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(false, string.Empty);
        }
    }

    public void OnClickClear()
    {
        if (this.state == State.INPUT)
        {
            this.inputTarget.value = string.Empty;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.Callback(true, this.changeName);
        }
    }

    public void Open(string deckName, CallbackFunc callback)
    {
        base.gameObject.SetActive(true);
        this.baseName = this.changeName = deckName;
        this.callbackFunc = callback;
        this.titleLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_INPUT_DECK_NAME_TITLE");
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_INPUT_DECK_NAME_EXPLANATION");
        this.inputTarget.value = deckName;
        this.inputTarget.GetComponent<Collider>().enabled = false;
        this.decideLabel.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.cancelLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideButton.isEnabled = false;
        this.state = State.OPEN;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public delegate void CallbackFunc(bool result, string changeName);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        CLOSE
    }
}

