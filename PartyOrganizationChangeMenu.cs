using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class PartyOrganizationChangeMenu : BaseMenu
{
    protected PartyListViewItem basePartyItem;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected GameObject explanationBase;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected GameObject mainDeckBase;
    protected Mode mode;
    protected System.Action openCallbackFunc;
    protected int openSelectMember;
    [SerializeField]
    protected UICommonButton partyDecideButton;
    protected PartyListViewItem partyItem;
    [SerializeField]
    protected PartyListViewIndicator partyListViewIndicator;
    [SerializeField]
    protected PartyOrganizationChangeObject[] partyOrganizationChangeObjectList = new PartyOrganizationChangeObject[BalanceConfig.DeckMemberMax];
    [SerializeField]
    protected UICommonButton partyPointEventButton;
    [SerializeField]
    protected UICommonButton partyRemoveButton;
    protected EventUpValSetupInfo setupInfo;
    protected State state;
    [SerializeField]
    protected GameObject tutorialMaskBase;
    [SerializeField]
    protected UICommonButton tutorialPartyDecideButton;
    [SerializeField]
    protected PartyOrganizationChangeObject tutorialPartyOrganizationChangeEmptyObject;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int n = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, n);
        }
    }

    protected void ClearItem()
    {
        this.mainDeckBase.SetActive(false);
        for (int i = 0; i < this.partyOrganizationChangeObjectList.Length; i++)
        {
            this.partyOrganizationChangeObjectList[i].ClearItem();
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
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndCloseTutorialArrowDecide()
    {
        this.tutorialMaskBase.SetActive(false);
        this.tutorialPartyDecideButton.gameObject.SetActive(false);
        this.state = State.SELECTED;
        this.Callback(ResultKind.DECIDE, -1);
    }

    protected void EndCloseTutorialArrowEmptyItem()
    {
        this.tutorialMaskBase.SetActive(false);
        this.tutorialPartyOrganizationChangeEmptyObject.gameObject.SetActive(false);
        this.state = State.SELECTED;
        this.Callback(ResultKind.SELECT_SERVANT, 1);
    }

    protected void EndOpen()
    {
        if (this.openSelectMember >= 0)
        {
            this.state = State.SELECTED;
            if (this.openCallbackFunc != null)
            {
                System.Action openCallbackFunc = this.openCallbackFunc;
                this.openCallbackFunc = null;
                openCallbackFunc();
            }
            if (this.callbackFunc != null)
            {
                PartyOrganizationListViewItem member = this.partyItem.GetMember(this.openSelectMember);
                Debug.Log(string.Concat(new object[] { "PartyOrganizationChangeMenu:EndOpen ", this.state, " menber ", this.openSelectMember, " ", member.IsFollower }));
                if (member.IsFollower)
                {
                    this.openSelectMember = -1;
                    this.state = State.INPUT;
                    this.SetInput(true);
                }
                else if (this.mode == Mode.SELECT_SERVANT)
                {
                    this.state = State.SELECTED;
                    this.Callback(ResultKind.SELECT_SERVANT, this.openSelectMember);
                }
                else if (this.mode == Mode.SELECT_EQUIP)
                {
                    this.state = State.SELECTED;
                    this.Callback(ResultKind.SELECT_EQUIP, this.openSelectMember);
                }
                else if (this.mode == Mode.DETAIL_SERVANT)
                {
                    this.state = State.INPUT;
                    this.SetInput(true);
                    this.partyOrganizationChangeObjectList[this.openSelectMember].SendMessage("OpenServantDetail");
                }
                else
                {
                    this.openSelectMember = -1;
                    this.state = State.INPUT;
                    this.SetInput(true);
                }
            }
        }
        else
        {
            if (this.mode == Mode.NONE)
            {
                if (this.callbackFunc != null)
                {
                    this.state = State.INPUT;
                    this.SetInput(true);
                }
                else
                {
                    this.state = State.SELECTED;
                }
            }
            else
            {
                this.state = State.TUTORIAL_OPEN;
                switch (this.mode)
                {
                    case Mode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT:
                        this.tutorialMaskBase.SetActive(true);
                        this.tutorialPartyOrganizationChangeEmptyObject.gameObject.SetActive(true);
                        this.tutorialPartyOrganizationChangeEmptyObject.SetItem(this.partyItem, 1, new PartyOrganizationChangeObject.CallbackFunc(this.OnClickEmptyItem));
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(-245f, 70f), (float) 0f, new Rect(-320f, -205f, 150f, 380f), new System.Action(this.EndOpenTutorialArrow));
                        break;

                    case Mode.ORGANIZATION_GUIDE_DECIDE_SELECT:
                        this.tutorialMaskBase.SetActive(true);
                        this.tutorialPartyDecideButton.gameObject.SetActive(true);
                        this.tutorialPartyDecideButton.SetState(UICommonButtonColor.State.Normal, false);
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_PARTY_ORGANIZATION4"), new Vector2(440f, -235f), new Rect(322f, -295f, 190f, 80f), (float) 0f, new Vector2(0f, 0f), -1, new System.Action(this.EndOpenTutorialArrow));
                        break;
                }
            }
            if (this.openCallbackFunc != null)
            {
                System.Action action2 = this.openCallbackFunc;
                this.openCallbackFunc = null;
                action2();
            }
        }
    }

    protected void EndOpenTutorialArrow()
    {
        Debug.Log("EndOpenTutorialArrow");
        this.state = State.TUTORIAL;
        this.tutorialPartyOrganizationChangeEmptyObject.SetInput(true);
    }

    public PartyListViewItem GetBaseItem() => 
        this.basePartyItem;

    public PartyListViewItem GetItem() => 
        this.partyItem;

    public void Init()
    {
        this.ClearItem();
        this.basePartyItem = null;
        this.partyItem = null;
        this.state = State.INIT;
        base.Init();
    }

    public bool IsThroughSelect() => 
        (this.openSelectMember >= 0);

    public void ModifyItem()
    {
        this.basePartyItem.Modify();
        this.partyItem.Modify();
        for (int i = 0; i < this.partyOrganizationChangeObjectList.Length; i++)
        {
            this.partyOrganizationChangeObjectList[i].ModifyItem();
        }
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickClassCompatibility()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.Callback(ResultKind.CLASS_COMPATIBILITY, -1);
        }
    }

    public void OnClickCommandCard()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.Callback(ResultKind.COMMAND_CARD, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.DECIDE, -1);
        }
        else if ((this.state == State.TUTORIAL) && (this.mode == Mode.ORGANIZATION_GUIDE_DECIDE_SELECT))
        {
            this.state = State.TUTORIAL_CLOSE;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(new System.Action(this.EndCloseTutorialArrowDecide));
        }
    }

    protected void OnClickEmptyItem(PartyOrganizationChangeObject.ResultKind result, int n)
    {
        Debug.Log("OnClickEmptyItem");
        if ((this.state == State.TUTORIAL) && (this.mode == Mode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT))
        {
            this.state = State.TUTORIAL_CLOSE;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrowEmptyItem));
        }
    }

    protected void OnClickItem(PartyOrganizationChangeObject.ResultKind result, int n)
    {
        if (this.state == State.INPUT)
        {
            Debug.Log("PartyOrganizationChangeMenu : OnClickItem " + n);
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (result == PartyOrganizationChangeObject.ResultKind.SELECT_EQUIP)
            {
                this.Callback(ResultKind.SELECT_EQUIP, n);
            }
            else
            {
                this.Callback(ResultKind.SELECT_SERVANT, n);
            }
        }
    }

    public void OnClickPointEvent()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.state = State.SELECTED;
            this.Callback(ResultKind.EVENT_POINT, -1);
        }
    }

    public void OnClickRemove()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.REMOVE, -1);
        }
    }

    public void Open(PartyListViewItem.SetupKind kind, Mode tutorialMode, PartyListViewItem basePartyItem, PartyListViewItem partyItem, int selectMember, EventUpValSetupInfo setupInfo, CallbackFunc callback, System.Action openCallback)
    {
        Debug.Log("PartyOrganizationChangeMenu:Open " + this.state);
        if (this.state == State.INIT)
        {
            this.mode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.openSelectMember = selectMember;
            this.setupInfo = setupInfo;
            base.gameObject.SetActive(true);
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(false);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if (this.state == State.ENTER)
        {
            this.mode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.openSelectMember = selectMember;
            base.gameObject.SetActive(true);
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(true);
            this.EndOpen();
        }
        else if (((this.state == State.INPUT) || (this.state == State.SELECTED)) || (this.state == State.TUTORIAL))
        {
            this.mode = tutorialMode;
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.basePartyItem = basePartyItem;
            this.partyItem = partyItem;
            this.openSelectMember = selectMember;
            this.SetItem();
            this.SetInput(false);
            this.SetupButton(true);
            this.EndOpen();
        }
    }

    protected void SetInput(bool isInput)
    {
        for (int i = 0; i < this.partyOrganizationChangeObjectList.Length; i++)
        {
            this.partyOrganizationChangeObjectList[i].SetInput(isInput);
        }
        this.explanationBase.SetActive(true);
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_SERVANT_CHANGE_EXPLANATION");
    }

    protected void SetItem()
    {
        this.partyListViewIndicator.DrawPartyInfo(this.partyItem);
        for (int i = 0; i < this.partyOrganizationChangeObjectList.Length; i++)
        {
            this.partyOrganizationChangeObjectList[i].SetItem(this.partyItem, i, new PartyOrganizationChangeObject.CallbackFunc(this.OnClickItem));
        }
    }

    protected void SetupButton(bool isMove)
    {
        this.partyPointEventButton.gameObject.SetActive(this.setupInfo != null);
        this.partyPointEventButton.SetState(UICommonButtonColor.State.Normal, isMove);
    }

    public delegate void CallbackFunc(PartyOrganizationChangeMenu.ResultKind result, int n);

    public enum Mode
    {
        NONE,
        SELECT_SERVANT,
        SELECT_EQUIP,
        DETAIL_SERVANT,
        ORGANIZATION_GUIDE_DECK_EMPTY_SELECT,
        ORGANIZATION_GUIDE_DECIDE_SELECT
    }

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        REMOVE,
        CLASS_COMPATIBILITY,
        COMMAND_CARD,
        EVENT_POINT,
        SELECT_SERVANT,
        SELECT_EQUIP
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
}

