using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FriendSearchResultMenu : BaseMenu
{
    [SerializeField]
    protected ClassButtonControlComponent classButtonControl;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected FriendIconComponent friendIcon;
    private OtherUserGameEntity otherUserGameEntity;
    protected string searchId = string.Empty;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, this.classButtonControl.GetCursorPos);
        }
    }

    public void changeClass(int classPos)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (this.otherUserGameEntity != null)
        {
            this.friendIcon.Set(this.otherUserGameEntity, true, this.classButtonControl.GetCursorPos);
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

    public int getChangeCursorPos() => 
        this.classButtonControl.getChangeCursorPos();

    public void Init()
    {
        this.titleLabel.text = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
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

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(true);
        }
    }

    public void Open(OtherUserGameEntity entity, int classPos, CallbackFunc callback)
    {
        this.classButtonControl.init(new ClassButtonControlComponent.CallbackFunc(this.changeClass), false);
        this.classButtonControl.setCursor(classPos);
        this.otherUserGameEntity = entity;
        if (this.state == State.CLOSE)
        {
            this.EndClose();
        }
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.titleLabel.text = LocalizationManager.Get("FRIEND_SEARCH_RESULT_TITLE");
            if (entity != null)
            {
                this.friendIcon.Set(entity, true, this.classButtonControl.GetCursorPos);
            }
            this.decideButton.enabled = true;
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.state = State.INPUT;
        }
    }

    public int GetCursorPos =>
        this.classButtonControl.GetCursorPos;

    public delegate void CallbackFunc(bool result, int classPos);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

