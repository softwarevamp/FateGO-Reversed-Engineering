using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FriendSearchMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UICommonButton copyButton;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UILabel myAddressLabel;
    [SerializeField]
    protected UILineInput searchDataInput;
    protected string searchId;
    protected State state;
    [SerializeField]
    protected UILabel title1Label;
    [SerializeField]
    protected UILabel title2Label;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            string text = this.searchDataInput.GetText();
            this.callbackFunc = null;
            callbackFunc(result, text);
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

    public void EndCopyDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.title1Label.text = string.Empty;
        this.title2Label.text = string.Empty;
        this.myAddressLabel.text = string.Empty;
        UIInput component = this.searchDataInput.GetComponent<UIInput>();
        component.value = string.Empty;
        component.defaultText = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnChangeServerInput()
    {
        string text = this.searchDataInput.GetText();
        string str2 = string.Empty;
        for (int i = 0; i < text.Length; i++)
        {
            char ch = text[i];
            if ((ch >= '0') && (ch <= '9'))
            {
                str2 = str2 + ch;
            }
        }
        if (text != str2)
        {
            this.searchDataInput.GetComponent<UIInput>().value = str2;
            text = str2;
        }
        if (text.Length >= 12)
        {
            this.decideButton.isEnabled = true;
            this.decideButton.SetState(UICommonButtonColor.State.Normal, true);
        }
        else
        {
            this.decideButton.isEnabled = false;
            this.decideButton.SetState(UICommonButtonColor.State.Disabled, true);
        }
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(false);
        }
    }

    public void OnClickCopy()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            string text = this.myAddressLabel.text;
            if (!string.IsNullOrEmpty(text))
            {
                CommonServicePluginScript.SetClipBoardText(text);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, LocalizationManager.Get("COPY_MY_ID"), new NotificationDialog.ClickDelegate(this.EndCopyDialog), -1);
            }
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.searchId = this.searchDataInput.GetText();
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(CallbackFunc callback, bool isCodeClear = false)
    {
        if (isCodeClear || (this.searchId == null))
        {
            this.searchId = string.Empty;
        }
        UIInput component = this.searchDataInput.GetComponent<UIInput>();
        component.value = this.searchId;
        component.defaultText = LocalizationManager.Get("FRIEND_SEARCH_EXPLANATION");
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.title1Label.text = LocalizationManager.Get("FRIEND_SEARCH_TITLE1");
            this.title2Label.text = LocalizationManager.Get("FRIEND_SEARCH_TITLE2");
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            this.myAddressLabel.text = LocalizationManager.GetNumberFormat(entity.friendCode);
            this.searchDataInput.SetInputEnable(true);
            this.decideButton.isEnabled = false;
            this.decideButton.SetState(UICommonButtonColor.State.Disabled, false);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc(bool result, string friendCode);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

