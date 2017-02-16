using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class PartyServantSelectMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected System.Action openCallbackFunc;
    [SerializeField]
    protected PartyServantListViewManager partyServantListViewManager;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected GameObject tutorialMaskBase;
    protected TutorialMode tutorialMode;
    protected int tutorialSelect;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int n = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, (n < 0) ? null : this.partyServantListViewManager.GetItem(n));
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.VALID);
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
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

    protected void EndCloseTutorialArrow()
    {
        this.tutorialMaskBase.SetActive(false);
        this.state = State.SELECTED;
        this.Callback(ResultKind.DECIDE, this.tutorialSelect);
    }

    protected void EndOpen()
    {
        if (this.tutorialMode == TutorialMode.NONE)
        {
            this.state = State.INPUT;
            this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.INPUT, new PartyServantListViewManager.CallbackFunc(this.OnSelectItem));
        }
        else
        {
            this.state = State.TUTORIAL_OPEN;
            this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.VALID);
            if (this.tutorialMode == TutorialMode.ORGANIZATION_GUIDE_FIRST_SELECT)
            {
                this.tutorialMaskBase.SetActive(true);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(-225f, 100f), (float) 0f, new Rect(-300f, -30f, 150f, 180f), new System.Action(this.EndOpenTutorialArrow));
            }
        }
        if (this.openCallbackFunc != null)
        {
            System.Action openCallbackFunc = this.openCallbackFunc;
            this.openCallbackFunc = null;
            openCallbackFunc();
        }
    }

    protected void EndOpenTutorialArrow()
    {
        this.state = State.TUTORIAL;
        this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.ORGANIZATION_GUIDE_FIRST_SELECT, new PartyServantListViewManager.CallbackFunc(this.OnSelectItem));
    }

    public void Init()
    {
        this.partyServantListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(ResultKind.DECIDE, -1);
        }
    }

    protected void OnSelectItem(PartyServantListViewManager.ResultKind kind, int n)
    {
        if (this.state == State.INPUT)
        {
            PartyServantListViewItem item = (n < 0) ? null : this.partyServantListViewManager.GetItem(n);
            if (kind == PartyServantListViewManager.ResultKind.DECIDE)
            {
                if (item.IsActionEventJoinLeader || item.IsSame)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.INPUT, new PartyServantListViewManager.CallbackFunc(this.OnSelectItem));
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.state = State.SELECTED;
                    this.Callback(ResultKind.DECIDE, n);
                }
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.state = State.SELECTED;
                this.Callback(ResultKind.CANCEL, -1);
            }
        }
        else if (this.state == State.TUTORIAL)
        {
            this.tutorialSelect = n;
            if (this.tutorialMode == TutorialMode.ORGANIZATION_GUIDE_FIRST_SELECT)
            {
                this.state = State.TUTORIAL_CLOSE;
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrow));
            }
        }
    }

    public void Open(TutorialMode tutorialMode, PartyListViewItem partyItem, int num, CallbackFunc callback, System.Action openCallback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.tutorialMode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            base.gameObject.SetActive(true);
            this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SERVANT);
            this.titleInfo.setBackBtnSprite(true);
            this.partyServantListViewManager.CreateList(partyItem, num);
            this.partyServantListViewManager.SetMode(PartyServantListViewManager.InitMode.VALID);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (((this.state == State.INPUT) || (this.state == State.SELECTED)) || ((this.state == State.TUTORIAL) || (this.state == State.TUTORIAL_OPEN)))
        {
            this.tutorialMode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.EndOpen();
        }
    }

    public delegate void CallbackFunc(PartyServantSelectMenu.ResultKind result, PartyServantListViewItem item);

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        DETAIL
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE,
        TUTORIAL_OPEN,
        TUTORIAL,
        TUTORIAL_CLOSE
    }

    public enum TutorialMode
    {
        NONE,
        ORGANIZATION_GUIDE_FIRST_DIALOG,
        ORGANIZATION_GUIDE_FIRST_SELECT
    }
}

