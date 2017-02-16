using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyListMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected System.Action openCallbackFunc;
    [SerializeField]
    protected UICommonButton partyChangeButton;
    [SerializeField]
    protected UICommonButton partyEditButton;
    [SerializeField]
    protected PartyListViewManager partyListViewManager;
    [SerializeField]
    protected UICommonButton partyPointEventButton;
    [SerializeField]
    protected UICommonButton partyRemove2Button;
    [SerializeField]
    protected UICommonButton partyRemoveButton;
    [SerializeField]
    protected GameObject partySelectBase;
    [SerializeField]
    protected UICommonButton partyStartButton;
    [SerializeField]
    protected UICommonButton partySwap2Button;
    [SerializeField]
    protected UICommonButton partySwapButton;
    [SerializeField]
    protected GameObject questStartBase;
    protected State state;
    [SerializeField]
    protected GameObject tutorialMaskBase;
    protected TutorialMode tutorialMode;
    [SerializeField]
    protected UICommonButton tutorialPartyChangeButton;
    [SerializeField]
    protected PartyOrganizationChangeObject tutorialPartyOrganizationChangeEmptyObject;
    [SerializeField]
    protected UICommonButton tutorialPartyStartButton;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, this.partyListViewManager.GetCenterIndex(), -1);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID, new System.Action(this.EndCloseList));
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndBlockDecideDialog()
    {
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.INPUT, new PartyListViewManager.CallbackFunc(this.OnSelectItem));
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

    protected void EndCloseList()
    {
    }

    protected void EndCloseTutorialArrowChange()
    {
        this.tutorialMaskBase.SetActive(false);
        this.tutorialPartyChangeButton.gameObject.SetActive(false);
        this.state = State.SELECTED;
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
        this.Callback(ResultKind.CHANGE);
    }

    protected void EndCloseTutorialArrowEmptyItem()
    {
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
        this.tutorialPartyOrganizationChangeEmptyObject.gameObject.SetActive(false);
        this.tutorialMaskBase.SetActive(false);
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(ResultKind.TUTORIAL_CHANGE_SERVANT, 0, 1);
        }
    }

    protected void EndCloseTutorialArrowStart()
    {
        this.state = State.SELECTED;
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
        this.tutorialMaskBase.SetActive(false);
        this.tutorialPartyStartButton.gameObject.SetActive(false);
        this.Callback(ResultKind.START);
    }

    protected void EndEnter()
    {
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
        if (this.tutorialMode == TutorialMode.NONE)
        {
            if (this.callbackFunc != null)
            {
                this.state = State.INPUT;
                this.partyListViewManager.SetMode(PartyListViewManager.InitMode.INPUT, new PartyListViewManager.CallbackFunc(this.OnSelectItem));
            }
            else
            {
                this.state = State.SELECTED;
                this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            }
        }
        else
        {
            PartyListViewItem partyItem = this.GetItem(this.partyListViewManager.GetCenterIndex());
            this.state = State.TUTORIAL_OPEN;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.INPUT, new PartyListViewManager.CallbackFunc(this.OnSelectItem));
            switch (this.tutorialMode)
            {
                case TutorialMode.ORGANIZATION_GUIDE_CHANGE_SELECT:
                    this.tutorialMaskBase.SetActive(true);
                    this.tutorialPartyChangeButton.gameObject.SetActive(true);
                    this.tutorialPartyChangeButton.SetState(UICommonButtonColor.State.Normal, false);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(-120f, -255f), (float) 0f, new Rect(-200f, -295f, 160f, 80f), new System.Action(this.EndOpenTutorialArrow));
                    break;

                case TutorialMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT:
                    this.tutorialMaskBase.SetActive(true);
                    this.tutorialPartyOrganizationChangeEmptyObject.gameObject.SetActive(true);
                    this.tutorialPartyOrganizationChangeEmptyObject.SetItem(partyItem, 1, null);
                    this.partyListViewManager.SetMode(PartyListViewManager.InitMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_PARTY_ORGANIZATION1_2"), new Vector2(-245f, 70f), new Rect(-320f, -205f, 150f, 380f), (float) 0f, new Vector2(100f, -40f), -1, new System.Action(this.EndOpenTutorialArrow));
                    break;

                case TutorialMode.FOLLOWER_GUIDE_START_SELECT:
                    this.tutorialMaskBase.SetActive(true);
                    this.tutorialPartyStartButton.gameObject.SetActive(true);
                    this.tutorialPartyStartButton.SetState(UICommonButtonColor.State.Normal, false);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(440f, -235f), (float) 0f, new Rect(322f, -295f, 190f, 80f), new System.Action(this.EndOpenTutorialArrow));
                    break;
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
    }

    public void Enter()
    {
        this.Enter(null);
    }

    public void Enter(System.Action callback)
    {
        this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
        this.closeCallbackFunc = callback;
        this.state = State.ENTER;
        base.Close(new System.Action(this.EndEnter));
    }

    public PartyListViewItem GetCenterItem() => 
        (this.partyListViewManager.GetCenterItem() as PartyListViewItem);

    public PartyListViewItem GetItem(int index) => 
        this.partyListViewManager.GetItem(index);

    public void Init()
    {
        this.partyListViewManager.DestroyList();
        this.tutorialMaskBase.SetActive(false);
        this.tutorialPartyChangeButton.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.CANCEL);
        }
    }

    public void OnClickChange()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.CHANGE);
        }
        else if ((this.state == State.TUTORIAL) && (this.tutorialMode == TutorialMode.ORGANIZATION_GUIDE_CHANGE_SELECT))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.TUTORIAL_CLOSE;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrowChange));
        }
    }

    public void OnClickClassCompatibility()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.CLASS_COMPATIBILITY);
        }
    }

    public void OnClickCommandCard()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.COMMAND_CARD);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            switch (this.GetItem(this.partyListViewManager.GetCenterIndex()).GetDeckCondition())
            {
                case PartyListViewItem.DeckCondition.COST_OVER:
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_TITLE_COST_OVER"), LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_MESSAGE_COST_OVER"), new System.Action(this.EndBlockDecideDialog), -1);
                    return;
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.DECIDE);
        }
    }

    public void OnClickDeckName()
    {
        if ((this.state == State.INPUT) && (this.partyListViewManager.GetCenterIndex() >= 0))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.DECK_NAME);
        }
    }

    public void OnClickEdit()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.EDIT);
        }
    }

    public void OnClickMasterFormation()
    {
        if (this.state == State.INPUT)
        {
            Debug.Log("PartyListMenu:OnClickMasterEquip");
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.MASTER_FORMATION);
        }
    }

    public void OnClickPointEvent()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.EVENT_POINT);
        }
    }

    public void OnClickRemove()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.REMOVE);
        }
    }

    public void OnClickStart()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.START);
        }
        else if ((this.state == State.TUTORIAL) && (this.tutorialMode == TutorialMode.FOLLOWER_GUIDE_START_SELECT))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.state = State.TUTORIAL_CLOSE;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrowStart));
        }
    }

    public void OnClickSwap()
    {
        if (this.state == State.INPUT)
        {
            Debug.Log("PartyListMenu:OnClickSwap");
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.Callback(ResultKind.SWAP);
        }
    }

    public void OnClickTabParty1()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(0, true);
        }
    }

    public void OnClickTabParty10()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(9, true);
        }
    }

    public void OnClickTabParty2()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(1, true);
        }
    }

    public void OnClickTabParty3()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(2, true);
        }
    }

    public void OnClickTabParty4()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(3, true);
        }
    }

    public void OnClickTabParty5()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(4, true);
        }
    }

    public void OnClickTabParty6()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(5, true);
        }
    }

    public void OnClickTabParty7()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(6, true);
        }
    }

    public void OnClickTabParty8()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(7, true);
        }
    }

    public void OnClickTabParty9()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.partyListViewManager.MoveCenterItem(8, true);
        }
    }

    public void OnClickTutorialEmptyItem()
    {
        Debug.Log("OnClickTutorialEmptyItem: " + this.state);
        if (this.state == State.TUTORIAL)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.TUTORIAL_CLOSE;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(new System.Action(this.EndCloseTutorialArrowEmptyItem));
        }
    }

    protected void OnSelectItem(PartyListViewManager.ResultKind kind, int n, int m)
    {
        ResultKind cANCEL;
        if (this.state == State.INPUT)
        {
            cANCEL = ResultKind.CANCEL;
            switch (kind)
            {
                case PartyListViewManager.ResultKind.SELECT_SERVANT:
                    cANCEL = ResultKind.CHANGE_SERVANT;
                    goto Label_006D;

                case PartyListViewManager.ResultKind.SELECT_EQUIP:
                    cANCEL = ResultKind.CHANGE_EQUIP;
                    goto Label_006D;

                case PartyListViewManager.ResultKind.CHANGE_PARTY:
                    cANCEL = ResultKind.DETAIL_SERVANT;
                    goto Label_006D;

                case PartyListViewManager.ResultKind.MODIFY_STATUS:
                    cANCEL = ResultKind.MODIFY_STATUS;
                    goto Label_006D;
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.INPUT, new PartyListViewManager.CallbackFunc(this.OnSelectItem));
        }
        return;
    Label_006D:
        this.state = State.SELECTED;
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(cANCEL, n, m);
        }
    }

    public void Open(PartyListViewItem.MenuKind kind, TutorialMode tutorialMode, UserDeckEntity[] userDeckEntityList, int partyNum, FollowerInfo followerInfo, int followerClassId, EventUpValSetupInfo setupInfo, CallbackFunc callback, System.Action openCallback)
    {
        if (this.state == State.INIT)
        {
            Debug.Log("PartyListMenu Open: INIT");
            this.tutorialMode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            base.gameObject.SetActive(true);
            this.partyStartButton.SetState(UICommonButtonColor.State.Normal, false);
            this.partyEditButton.SetState(UICommonButtonColor.State.Normal, false);
            this.partyChangeButton.SetState(UICommonButtonColor.State.Normal, false);
            this.partySwapButton.SetState(UICommonButtonColor.State.Normal, false);
            this.partySwap2Button.SetState(UICommonButtonColor.State.Normal, false);
            this.partyRemoveButton.SetState(UICommonButtonColor.State.Normal, false);
            this.partyRemove2Button.SetState(UICommonButtonColor.State.Normal, false);
            this.questStartBase.SetActive(kind == PartyListViewItem.MenuKind.QUEST_START);
            this.partySelectBase.SetActive(kind == PartyListViewItem.MenuKind.SELECT_PARTY);
            this.partyPointEventButton.gameObject.SetActive(setupInfo != null);
            this.partyListViewManager.CreateList(kind, userDeckEntityList, partyNum, followerInfo, followerClassId, setupInfo);
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.ENTER)
        {
            this.tutorialMode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            base.gameObject.SetActive(true);
            this.partyListViewManager.CreateList(kind, userDeckEntityList, partyNum, followerInfo, followerClassId, setupInfo);
            this.partyListViewManager.SetMode(PartyListViewManager.InitMode.VALID);
            this.EndOpen();
        }
        else if (((this.state == State.INPUT) || (this.state == State.SELECTED)) || (this.state == State.TUTORIAL))
        {
            this.tutorialMode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.EndOpen();
        }
    }

    public delegate void CallbackFunc(PartyListMenu.ResultKind result, int n, int m);

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        CHANGE,
        SWAP,
        REMOVE,
        DECK_NAME,
        EDIT,
        START,
        MASTER_FORMATION,
        CLASS_COMPATIBILITY,
        COMMAND_CARD,
        EVENT_POINT,
        MODIFY_STATUS,
        CHANGE_SERVANT,
        CHANGE_EQUIP,
        DETAIL_SERVANT,
        TUTORIAL_CHANGE_SERVANT
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE,
        ENTER,
        TUTORIAL_OPEN,
        TUTORIAL,
        TUTORIAL_CLOSE
    }

    public enum TutorialMode
    {
        NONE,
        ORGANIZATION_GUIDE_CHANGE_SELECT,
        ORGANIZATION_GUIDE_DECK_EMPTY_SELECT,
        FOLLOWER_GUIDE_START_SELECT
    }
}

