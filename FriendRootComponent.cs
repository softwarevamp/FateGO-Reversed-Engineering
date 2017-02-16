using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendRootComponent : SceneRootComponent
{
    [SerializeField]
    protected ClassButtonControlComponent classButtonControl;
    [SerializeField]
    protected UILabel friendCountLabel;
    [SerializeField]
    protected FriendSearchMenu friendSearchMenu;
    [SerializeField]
    protected FriendSearchResultMenu friendSearchResultMenu;
    [SerializeField]
    protected FriendWarningDialog friendWarningDialog;
    [SerializeField]
    protected UILabel helpLabel;
    protected bool isReturnSupport;
    [SerializeField]
    protected NoticeNumberComponent noticeNumber;
    [SerializeField]
    protected FriendOperationConfirmMenu operationConfirmMenu;
    [SerializeField]
    protected FriendOperationItemListViewManager operationItemListViewManager;
    protected string selectFriendCode;
    protected OtherUserGameEntity selectFriendEntity;
    protected int selectItemNum;
    protected State state;
    [SerializeField]
    protected UICommonButton tabFriendButton;
    [SerializeField]
    protected UISprite tabFriendSprite;
    [SerializeField]
    protected UISprite tabFriendTitleSprite;
    protected static TabKind tabKindSave = TabKind.DEFAULT;
    [SerializeField]
    protected UICommonButton tabOfferedButton;
    [SerializeField]
    protected UISprite tabOfferedSprite;
    [SerializeField]
    protected UISprite tabOfferedTitleSprite;
    [SerializeField]
    protected UICommonButton tabSearchButton;
    [SerializeField]
    protected UISprite tabSearchSprite;
    [SerializeField]
    protected UISprite tabSearchTitleSprite;
    [SerializeField]
    protected TitleInfoControl titleInfo;

    public void BackFriendAcceptConfirm()
    {
        if (this.state == State.INPUT_FRIEND_ACCEPT_CONFIRM)
        {
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
            this.state = State.INPUT_SHOW_OFFERED;
        }
    }

    public void BackFriendAcceptConfirmRefreshShowOffered()
    {
        if (this.state == State.INPUT_FRIEND_ACCEPT_CONFIRM)
        {
            this.state = State.INPUT_SHOW_OFFERED;
            this.RefreshInfo();
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.DestroyList();
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFERED, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
        }
    }

    public void BackFriendCancelConfirm()
    {
        if (this.state == State.INPUT_FRIEND_CANCEL_CONFIRM)
        {
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
            this.state = State.INPUT_SHOW_OFFERED;
        }
    }

    public void BackFriendCancelConfirmRefreshShowOffer()
    {
        if (this.state == State.INPUT_FRIEND_CANCEL_CONFIRM)
        {
            this.state = State.INPUT_SHOW_OFFER;
            this.RefreshInfo();
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.DestroyList();
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFER, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
        }
    }

    public void BackFriendOfferConfirm()
    {
        if (this.state == State.INPUT_FRIEND_OFFER_CONFIRM)
        {
            this.state = State.INPUT_SEARCH_FRIEND;
            this.operationConfirmMenu.Close();
            this.StartSearchInput();
        }
        else if (this.state == State.INPUT_FRIEND_OFFER_CONFIRM_MAX_FRIEND)
        {
            this.state = State.INPUT_SEARCH_FRIEND;
            this.friendWarningDialog.Close();
            this.StartSearchInput();
        }
    }

    public void BackFriendOfferConfirmRefreshShowSearch()
    {
        if (this.state == State.INPUT_FRIEND_OFFER_CONFIRM)
        {
            this.state = State.INPUT_SEARCH_FRIEND;
            this.selectFriendCode = null;
            this.selectFriendEntity = null;
            this.operationConfirmMenu.Close();
            this.friendSearchResultMenu.Close();
            this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), true);
        }
    }

    public void BackFriendRejectConfirm()
    {
        if (this.state == State.INPUT_FRIEND_REJECT_CONFIRM)
        {
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
            this.state = State.INPUT_SHOW_OFFERED;
        }
    }

    public void BackFriendRejectConfirmRefreshShowOffered()
    {
        if (this.state == State.INPUT_FRIEND_REJECT_CONFIRM)
        {
            this.state = State.INPUT_SHOW_OFFERED;
            this.RefreshInfo();
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.DestroyList();
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFERED, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
        }
    }

    public void BackFriendRemoveConfirm()
    {
        if (this.state == State.INPUT_FRIEND_REMOVE_CONFIRM)
        {
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
            this.state = State.INPUT_SHOW_FRIEND;
        }
    }

    public void BackFriendRemoveConfirmRefreshShowFriend()
    {
        if (this.state == State.INPUT_FRIEND_REMOVE_CONFIRM)
        {
            this.state = State.INPUT_SHOW_FRIEND;
            this.RefreshInfo();
            this.operationConfirmMenu.Close();
            this.operationItemListViewManager.DestroyList();
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.FRIEND, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
        }
    }

    public void BackSearchFriend()
    {
        if (this.state == State.INPUT_SEARCH_FRIEND)
        {
            this.state = State.QUIT_SEARCH_FRIEND;
            this.RefreshInfo();
            this.friendSearchResultMenu.Close();
            this.friendSearchMenu.Close(new System.Action(this.OnMoveEnd));
        }
    }

    public void BackShowFriend()
    {
        if (this.state == State.INPUT_SHOW_FRIEND)
        {
            this.state = State.QUIT_SHOW_FRIEND;
            this.RefreshInfo();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
        }
    }

    public void BackShowOffer()
    {
        if (this.state == State.INPUT_SHOW_OFFER)
        {
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
            this.RefreshInfo();
            this.state = State.QUIT_SHOW_OFFER;
        }
    }

    public void BackShowOffered()
    {
        if (this.state == State.INPUT_SHOW_OFFERED)
        {
            this.state = State.QUIT_SHOW_OFFERED;
            this.RefreshInfo();
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
        }
    }

    public void BackShowSearch()
    {
        if (this.state == State.INPUT_SHOW_SEARCH)
        {
            this.state = State.QUIT_SHOW_SEARCH;
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
        }
    }

    public override void beginFinish()
    {
        this.operationItemListViewManager.DestroyList();
        this.operationConfirmMenu.Init();
        this.friendSearchMenu.Init();
        this.friendSearchResultMenu.Init();
        this.friendWarningDialog.Init();
        this.operationItemListViewManager.gameObject.SetActive(false);
        this.state = State.INIT;
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.FRIEND, 0x27);
        base.hideUserStatus();
        this.helpLabel.text = LocalizationManager.Get("FRIEND_HELP_TEXT");
        this.classButtonControl.init(new ClassButtonControlComponent.CallbackFunc(this.changeClass), false);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume(object data)
    {
        Debug.Log("Call beginResume");
        if (!(data is SupportInfoJump))
        {
            base.beginResume();
            this.isReturnSupport = false;
        }
        else
        {
            this.isReturnSupport = true;
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            this.beginInitialize();
            this.beginStartUp();
        }
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.FRIEND);
        this.titleInfo.setBackBtnSprite(true);
        this.tabFriendButton.isEnabled = true;
        this.tabOfferedButton.isEnabled = true;
        this.tabSearchButton.isEnabled = true;
        this.tabFriendButton.enabled = true;
        this.tabOfferedButton.enabled = true;
        this.tabSearchButton.enabled = true;
        this.tabFriendTitleSprite.spriteName = "btn_txt_friendlist_off";
        this.tabOfferedTitleSprite.spriteName = "btn_txt_friendrequest_off";
        this.tabSearchTitleSprite.spriteName = "btn_txt_friendsearch_off";
        this.tabFriendSprite.spriteName = "btn_bg_08";
        this.tabOfferedSprite.spriteName = "btn_bg_08";
        this.tabSearchSprite.spriteName = "btn_bg_08";
        this.tabFriendButton.SetState(UICommonButtonColor.State.Disabled, false);
        this.tabOfferedButton.SetState(UICommonButtonColor.State.Disabled, false);
        this.tabSearchButton.SetState(UICommonButtonColor.State.Disabled, false);
        TblFriendMaser maser = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND);
        OtherUserGameEntity[] list = maser.GetList(FriendStatus.Kind.OFFERED);
        if (this.tabKind == TabKind.DEFAULT)
        {
            if (list.Length > 0)
            {
                this.tabKind = TabKind.OFFERED;
            }
            else if (maser.GetList(FriendStatus.Kind.FRIEND).Length > 0)
            {
                this.tabKind = TabKind.FRIEND;
            }
            else
            {
                this.tabKind = TabKind.SEARCH;
            }
        }
        this.state = State.INIT;
        this.RefreshInfo();
        MainMenuBar.setMenuActive(true, null);
        base.beginStartUp();
    }

    public void changeClass(int classPos)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.RefreshInfo();
        this.operationItemListViewManager.DestroyList();
        State state = this.state;
        if (state != State.INPUT_SHOW_FRIEND)
        {
            if (state != State.INPUT_SHOW_OFFER)
            {
                if (state == State.INPUT_SHOW_OFFERED)
                {
                    this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFERED, classPos);
                }
                else if ((state == State.INPUT_SHOW_SEARCH) || (state == State.INPUT_SEARCH_FRIEND))
                {
                }
            }
        }
        else
        {
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.FRIEND, classPos);
        }
    }

    public bool CheckTabKind()
    {
        switch (this.tabKind)
        {
            case TabKind.FRIEND:
                base.myFSM.SendEvent("MENU_OPERATION_FRIEND");
                break;

            case TabKind.OFFERED:
                base.myFSM.SendEvent("MENU_OPERATION_OFFERED");
                break;

            case TabKind.SEARCH:
                base.myFSM.SendEvent("MENU_OPERATION_SEARCH");
                break;
        }
        return true;
    }

    protected void EndCloseShowServant()
    {
        this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
    }

    protected void EndMaxFriendWarning(bool isDecide)
    {
        this.friendWarningDialog.Close();
        this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
    }

    protected void EndMaxFriendWarningOfferConfirm(bool isDecide)
    {
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndNoSearchWarning(bool isDecide)
    {
        this.friendWarningDialog.Close();
        this.state = State.INPUT_SEARCH_FRIEND;
        this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), false);
        base.myFSM.SendEvent("REQUEST_NG");
    }

    private void EndRequestFriend(string result)
    {
        Debug.Log("EndRequestFriend");
        MainMenuBar.UpdateNoticeNumber();
        if (result != "ng")
        {
            Dictionary<string, object> dictionary = JsonManager.getDictionary(result);
            if (dictionary.ContainsKey("message"))
            {
                string str = dictionary["message"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, str, new NotificationDialog.ClickDelegate(this.OnEndRequestDialog), -1);
                    return;
                }
            }
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndRequestFriendProfile(string result)
    {
        Debug.Log("EndRequestFriendProfile");
        if (result == "ok")
        {
            OtherUserGameEntity[] friendCodeList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME).GetFriendCodeList(this.selectFriendCode);
            if (friendCodeList.Length > 0)
            {
                this.selectFriendEntity = friendCodeList[0];
                base.myFSM.SendEvent("REQUEST_OK");
                return;
            }
        }
        this.StartSearchInput();
        base.myFSM.SendEvent("REQUEST_NG");
    }

    protected void EndShowServant(bool isDecide)
    {
        this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.MODIFY, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.state = State.INIT_TOP;
        }
    }

    public void OnClickBack()
    {
        switch (this.state)
        {
            case State.INPUT_SHOW_FRIEND:
            case State.INPUT_SHOW_OFFER:
            case State.INPUT_SHOW_OFFERED:
            case State.INPUT_SHOW_SEARCH:
            case State.INPUT_SEARCH_FRIEND:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                base.myFSM.SendEvent("CLICK_BACK");
                break;
        }
    }

    public void OnClickTabFriend()
    {
        State state = this.state;
        switch (state)
        {
            case State.INPUT_SHOW_FRIEND:
            case State.INPUT_SHOW_OFFER:
            case State.INPUT_SHOW_OFFERED:
            case State.INPUT_SHOW_SEARCH:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.tabKind = TabKind.FRIEND;
                this.RefreshTab();
                base.myFSM.SendEvent("CLICK_TAB");
                break;

            default:
                if (state == State.INPUT_SEARCH_FRIEND)
                {
                    if (this.friendSearchResultMenu.getChangeCursorPos() != -1)
                    {
                        this.classButtonControl.setCursor(this.friendSearchResultMenu.GetCursorPos);
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.tabKind = TabKind.FRIEND;
                    this.RefreshTab();
                    base.myFSM.SendEvent("CLICK_TAB");
                }
                break;
        }
    }

    public void OnClickTabOffered()
    {
        State state = this.state;
        switch (state)
        {
            case State.INPUT_SHOW_FRIEND:
            case State.INPUT_SHOW_OFFER:
            case State.INPUT_SHOW_OFFERED:
            case State.INPUT_SHOW_SEARCH:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.tabKind = TabKind.OFFERED;
                this.RefreshTab();
                base.myFSM.SendEvent("CLICK_TAB");
                break;

            default:
                if (state == State.INPUT_SEARCH_FRIEND)
                {
                    if (this.friendSearchResultMenu.getChangeCursorPos() != -1)
                    {
                        this.classButtonControl.setCursor(this.friendSearchResultMenu.GetCursorPos);
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.tabKind = TabKind.OFFERED;
                    this.RefreshTab();
                    base.myFSM.SendEvent("CLICK_TAB");
                }
                break;
        }
    }

    public void OnClickTabSearch()
    {
        switch (this.state)
        {
            case State.INPUT_SHOW_FRIEND:
            case State.INPUT_SHOW_OFFER:
            case State.INPUT_SHOW_OFFERED:
            case State.INPUT_SHOW_SEARCH:
            case State.INPUT_SEARCH_FRIEND:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.tabKind = TabKind.SEARCH;
                this.RefreshTab();
                base.myFSM.SendEvent("CLICK_TAB");
                break;
        }
    }

    protected void OnEndRequestDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_SHOW_FRIEND:
            case State.INIT_SHOW_FRIEND2:
                this.state = State.INPUT_SHOW_FRIEND;
                this.tabKind = TabKind.FRIEND;
                this.RefreshTab();
                this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.QUIT_SHOW_FRIEND:
            case State.QUIT_SHOW_FRIEND2:
                this.state = State.INIT_TOP;
                this.operationItemListViewManager.DestroyList();
                this.operationItemListViewManager.gameObject.SetActive(false);
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.INIT_SHOW_OFFER:
            case State.INIT_SHOW_OFFER2:
                this.state = State.INPUT_SHOW_OFFER;
                this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.QUIT_SHOW_OFFER:
            case State.QUIT_SHOW_OFFER2:
                this.state = State.INIT_TOP;
                this.operationItemListViewManager.DestroyList();
                this.operationItemListViewManager.gameObject.SetActive(false);
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.INIT_SHOW_OFFERED:
            case State.INIT_SHOW_OFFERED2:
                this.state = State.INPUT_SHOW_OFFERED;
                this.tabKind = TabKind.OFFERED;
                this.RefreshTab();
                this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.QUIT_SHOW_OFFERED:
            case State.QUIT_SHOW_OFFERED2:
                this.state = State.INIT_TOP;
                this.operationItemListViewManager.DestroyList();
                this.operationItemListViewManager.gameObject.SetActive(false);
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.INIT_SHOW_SEARCH:
                this.state = State.INIT_SHOW_SEARCH2;
                break;

            case State.INIT_SHOW_SEARCH2:
                this.state = State.INPUT_SHOW_SEARCH;
                this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.QUIT_SHOW_SEARCH:
            case State.QUIT_SHOW_SEARCH2:
                this.state = State.INPUT_SEARCH_FRIEND;
                this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), false);
                this.operationItemListViewManager.DestroyList();
                this.operationItemListViewManager.gameObject.SetActive(false);
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.INIT_SEARCH_FRIEND:
            case State.INIT_SEARCH_FRIEND2:
                this.state = State.INPUT_SEARCH_FRIEND;
                this.tabKind = TabKind.SEARCH;
                this.RefreshTab();
                this.RefreshInfo();
                this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), true);
                base.myFSM.SendEvent("END_ACTION");
                break;

            case State.QUIT_SEARCH_FRIEND:
            case State.QUIT_SEARCH_FRIEND2:
                this.state = State.INIT_TOP;
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                break;
        }
    }

    private void OnSelectFriendItem(FriendOperationItemListViewManager.ResultKind kind, int n)
    {
        FriendOperationItemListViewItem item;
        int num;
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        string[] strArray;
        string[] strArray2;
        string str;
        string str2;
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.selectItemNum = n;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        TblFriendMaser maser = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND);
        switch (kind)
        {
            case FriendOperationItemListViewManager.ResultKind.SERVANT_SKILL1_STATUS:
            case FriendOperationItemListViewManager.ResultKind.SERVANT_SKILL2_STATUS:
            case FriendOperationItemListViewManager.ResultKind.SERVANT_SKILL3_STATUS:
                item = this.operationItemListViewManager.GetItem(this.selectItemNum);
                if (item.GameUser.getServantLeaderInfo(item.ClassPos) == null)
                {
                    goto Label_021D;
                }
                num = 0;
                if (kind != FriendOperationItemListViewManager.ResultKind.SERVANT_SKILL2_STATUS)
                {
                    if (kind == FriendOperationItemListViewManager.ResultKind.SERVANT_SKILL3_STATUS)
                    {
                        num = 2;
                    }
                    break;
                }
                num = 1;
                break;

            case FriendOperationItemListViewManager.ResultKind.OFFER:
                if (maser.GetList(FriendStatus.Kind.FRIEND).Length < entity.friendKeep)
                {
                    base.myFSM.SendEvent("MENU_SELECT_ITEM_OFFER");
                    return;
                }
                this.friendWarningDialog.Open(FriendWarningDialog.Kind.MAX_FRIEND, new FriendWarningDialog.CallbackFunc(this.EndMaxFriendWarning));
                return;

            case FriendOperationItemListViewManager.ResultKind.ACCEPT:
                if (maser.GetList(FriendStatus.Kind.FRIEND).Length < entity.friendKeep)
                {
                    base.myFSM.SendEvent("MENU_SELECT_ITEM_ACCEPT");
                    return;
                }
                this.friendWarningDialog.Open(FriendWarningDialog.Kind.MAX_FRIEND, new FriendWarningDialog.CallbackFunc(this.EndMaxFriendWarning));
                return;

            case FriendOperationItemListViewManager.ResultKind.REJECT:
                base.myFSM.SendEvent("MENU_SELECT_ITEM_REJECT");
                return;

            case FriendOperationItemListViewManager.ResultKind.CANCEL:
                base.myFSM.SendEvent("MENU_SELECT_ITEM_CANCEL");
                return;

            case FriendOperationItemListViewManager.ResultKind.REMOVE:
                base.myFSM.SendEvent("MENU_SELECT_ITEM_REMOVE");
                return;

            default:
            {
                FriendOperationItemListViewItem item2 = this.operationItemListViewManager.GetItem(this.selectItemNum);
                ServantLeaderInfo servantLeaderInfo = item2.GameUser.getServantLeaderInfo(item2.ClassPos);
                if (servantLeaderInfo != null)
                {
                    this.SelectShowServant(servantLeaderInfo);
                }
                else
                {
                    this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
                }
                return;
            }
        }
        item.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
        SkillEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(numArray[num]);
        SkillLvEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(numArray[num], numArray2[num]);
        entity2.getSkillMessageInfo(out str, out str2, numArray2[num]);
        str = str + " " + string.Format(LocalizationManager.Get("MASTER_EQSKILL_LV_TXT"), numArray2[num]);
        string info = string.Format(LocalizationManager.Get("BATTLE_SKILLCHARGETURN"), entity3.chargeTurn);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailLongInfoDialog(str, info, str2);
    Label_021D:
        this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INPUT, new FriendOperationItemListViewManager.CallbackFunc(this.OnSelectFriendItem));
    }

    private void OnSelectOffer(bool isDecide, int classPos)
    {
        this.classButtonControl.setCursor(classPos);
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_SELECT_ITEM_OFFER");
    }

    private void OnSelectSearchFriend(bool isDecide, string friendCode)
    {
        this.selectFriendCode = friendCode;
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    public void Quit()
    {
        this.operationItemListViewManager.DestroyList();
        this.operationItemListViewManager.gameObject.SetActive(false);
        this.RefreshInfo();
        this.state = State.INIT;
    }

    public void RefreshInfo()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        TblFriendMaser maser = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND);
        this.friendCountLabel.text = string.Format(LocalizationManager.Get("FRIEND_SHOW_INFOMATION"), maser.GetFriendSum(), entity.friendKeep);
        int sum = maser.GetSum(FriendStatus.Kind.OFFERED);
        this.noticeNumber.SetNumber(sum);
    }

    protected void RefreshTab()
    {
        switch (this.tabKind)
        {
            case TabKind.FRIEND:
                this.tabFriendButton.isEnabled = true;
                this.tabOfferedButton.isEnabled = true;
                this.tabSearchButton.isEnabled = true;
                this.tabFriendButton.enabled = false;
                this.tabOfferedButton.enabled = true;
                this.tabSearchButton.enabled = true;
                this.tabFriendTitleSprite.spriteName = "btn_txt_friendlist_on";
                this.tabOfferedTitleSprite.spriteName = "btn_txt_friendrequest_off";
                this.tabSearchTitleSprite.spriteName = "btn_txt_friendsearch_off";
                this.tabFriendSprite.spriteName = "btn_bg_on";
                this.tabOfferedSprite.spriteName = "btn_bg_09";
                this.tabSearchSprite.spriteName = "btn_bg_09";
                this.tabFriendButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabOfferedButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabSearchButton.SetState(UICommonButtonColor.State.Normal, true);
                break;

            case TabKind.OFFERED:
                this.tabFriendButton.isEnabled = true;
                this.tabOfferedButton.isEnabled = true;
                this.tabSearchButton.isEnabled = true;
                this.tabFriendButton.enabled = true;
                this.tabOfferedButton.enabled = false;
                this.tabSearchButton.enabled = true;
                this.tabFriendTitleSprite.spriteName = "btn_txt_friendlist_off";
                this.tabOfferedTitleSprite.spriteName = "btn_txt_friendrequest_on";
                this.tabSearchTitleSprite.spriteName = "btn_txt_friendsearch_off";
                this.tabFriendSprite.spriteName = "btn_bg_09";
                this.tabOfferedSprite.spriteName = "btn_bg_on";
                this.tabSearchSprite.spriteName = "btn_bg_09";
                this.tabFriendButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabOfferedButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabSearchButton.SetState(UICommonButtonColor.State.Normal, true);
                break;

            case TabKind.SEARCH:
                this.tabFriendButton.isEnabled = true;
                this.tabOfferedButton.isEnabled = true;
                this.tabSearchButton.isEnabled = true;
                this.tabFriendButton.enabled = true;
                this.tabOfferedButton.enabled = true;
                this.tabSearchButton.enabled = false;
                this.tabFriendTitleSprite.spriteName = "btn_txt_friendlist_off";
                this.tabOfferedTitleSprite.spriteName = "btn_txt_friendrequest_off";
                this.tabSearchTitleSprite.spriteName = "btn_txt_friendsearch_on";
                this.tabFriendSprite.spriteName = "btn_bg_09";
                this.tabOfferedSprite.spriteName = "btn_bg_09";
                this.tabSearchSprite.spriteName = "btn_bg_on";
                this.tabFriendButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabOfferedButton.SetState(UICommonButtonColor.State.Normal, true);
                this.tabSearchButton.SetState(UICommonButtonColor.State.Normal, true);
                break;
        }
    }

    public void RequestFriendAccept()
    {
        FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            OtherUserGameEntity gameUser = item.GameUser;
            if (gameUser != null)
            {
                NetworkManager.getRequest<FriendAcceptRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(gameUser.userId);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestFriendCancel()
    {
        FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            OtherUserGameEntity gameUser = item.GameUser;
            if (gameUser != null)
            {
                NetworkManager.getRequest<FriendCancelRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(gameUser.userId);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestFriendOffer()
    {
        if (this.selectFriendEntity != null)
        {
            NetworkManager.getRequest<FriendOfferRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(this.selectFriendEntity.userId);
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    public void RequestFriendProfile()
    {
        this.selectFriendEntity = null;
        if (this.selectFriendCode != null)
        {
            if (this.selectFriendCode.Replace(" ", string.Empty) != string.Empty)
            {
                NetworkManager.getRequest<ProfileTopRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriendProfile)).beginRequestFriendCode(this.selectFriendCode);
                return;
            }
            this.friendWarningDialog.Open(FriendWarningDialog.Kind.NO_STRING, new FriendWarningDialog.CallbackFunc(this.EndNoSearchWarning));
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestFriendReject()
    {
        FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            OtherUserGameEntity gameUser = item.GameUser;
            if (gameUser != null)
            {
                NetworkManager.getRequest<FriendRejectRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(gameUser.userId);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestFriendRemove()
    {
        FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            OtherUserGameEntity gameUser = item.GameUser;
            if (gameUser != null)
            {
                NetworkManager.getRequest<FriendRemoveRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(gameUser.userId);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    private void SelectedFriendAcceptConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    private void SelectedFriendCancelConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    private void SelectedFriendOfferConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    private void SelectedFriendRejectConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    private void SelectedFriendRemoveConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    public void SelectFriendAcceptConfirm()
    {
        if (this.state == State.INPUT_SHOW_OFFERED)
        {
            FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            this.state = State.INPUT_FRIEND_ACCEPT_CONFIRM;
            this.operationConfirmMenu.Open(FriendOperationConfirmMenu.Kind.ACCEPT, item.GameUser, new FriendOperationConfirmMenu.CallbackFunc(this.SelectedFriendAcceptConfirm));
        }
    }

    public void SelectFriendCancelConfirm()
    {
        if (this.state == State.INPUT_SHOW_OFFER)
        {
            FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            this.state = State.INPUT_FRIEND_CANCEL_CONFIRM;
            this.operationConfirmMenu.Open(FriendOperationConfirmMenu.Kind.CANCEL, item.GameUser, new FriendOperationConfirmMenu.CallbackFunc(this.SelectedFriendCancelConfirm));
        }
    }

    public void SelectFriendOfferConfirm()
    {
        if (this.state == State.INPUT_SEARCH_FRIEND)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetList(FriendStatus.Kind.FRIEND).Length >= entity.friendKeep)
            {
                this.state = State.INPUT_FRIEND_OFFER_CONFIRM_MAX_FRIEND;
                this.friendWarningDialog.Open(FriendWarningDialog.Kind.MAX_FRIEND, new FriendWarningDialog.CallbackFunc(this.EndMaxFriendWarningOfferConfirm));
            }
            else if (this.selectFriendEntity != null)
            {
                this.state = State.INPUT_FRIEND_OFFER_CONFIRM;
                this.operationConfirmMenu.Open(FriendOperationConfirmMenu.Kind.OFFER, this.selectFriendEntity, new FriendOperationConfirmMenu.CallbackFunc(this.SelectedFriendOfferConfirm));
            }
            else
            {
                this.StartSearchInput();
            }
        }
    }

    public void SelectFriendRejectConfirm()
    {
        if (this.state == State.INPUT_SHOW_OFFERED)
        {
            FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            this.state = State.INPUT_FRIEND_REJECT_CONFIRM;
            this.operationConfirmMenu.Open(FriendOperationConfirmMenu.Kind.REJECT, item.GameUser, new FriendOperationConfirmMenu.CallbackFunc(this.SelectedFriendRejectConfirm));
        }
    }

    public void SelectFriendRemoveConfirm()
    {
        if (this.state == State.INPUT_SHOW_FRIEND)
        {
            FriendOperationItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            this.state = State.INPUT_FRIEND_REMOVE_CONFIRM;
            this.operationConfirmMenu.Open(FriendOperationConfirmMenu.Kind.REMOVE, item.GameUser, new FriendOperationConfirmMenu.CallbackFunc(this.SelectedFriendRemoveConfirm));
        }
    }

    public void SelectSearchFriend()
    {
        if (this.state == State.INIT_TOP)
        {
            this.state = State.INIT_SEARCH_FRIEND;
            this.RefreshInfo();
            this.OnMoveEnd();
        }
    }

    public void SelectShowFriend()
    {
        if (this.state == State.INIT_TOP)
        {
            this.state = State.INIT_SHOW_FRIEND;
            this.RefreshInfo();
            this.operationItemListViewManager.gameObject.SetActive(true);
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.FRIEND, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectShowOffer()
    {
        if (this.state == State.INIT_TOP)
        {
            this.state = State.INIT_SHOW_OFFER;
            this.RefreshInfo();
            this.operationItemListViewManager.gameObject.SetActive(true);
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFER, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectShowOffered()
    {
        if (this.state == State.INIT_TOP)
        {
            this.state = State.INIT_SHOW_OFFERED;
            this.RefreshInfo();
            this.operationItemListViewManager.gameObject.SetActive(true);
            this.operationItemListViewManager.CreateList(FriendStatus.Kind.OFFERED, this.classButtonControl.GetCursorPos);
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectShowSearch()
    {
        if (this.state == State.INPUT_SEARCH_FRIEND)
        {
            this.operationItemListViewManager.gameObject.SetActive(true);
            this.operationItemListViewManager.CreateListFriendCode(this.selectFriendCode);
            this.state = State.INIT_SHOW_SEARCH;
            this.friendSearchMenu.Close(new System.Action(this.OnMoveEnd));
            this.operationItemListViewManager.SetMode(FriendOperationItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectShowServant(ServantLeaderInfo servantLeaderInfo)
    {
        if (((this.state == State.INPUT_SHOW_FRIEND) || (this.state == State.INPUT_SHOW_OFFER)) || ((this.state == State.INPUT_SHOW_OFFERED) || (this.state == State.INPUT_SHOW_SEARCH)))
        {
            ServantStatusDialog.Kind fRIEND;
            if (this.state == State.INPUT_SHOW_FRIEND)
            {
                fRIEND = ServantStatusDialog.Kind.FRIEND;
            }
            else
            {
                fRIEND = ServantStatusDialog.Kind.FOLLOWER;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(fRIEND, servantLeaderInfo, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
        }
    }

    public void ShowSearchResult()
    {
        if (this.state == State.INPUT_SEARCH_FRIEND)
        {
            if (this.friendSearchResultMenu.IsOpen)
            {
                this.friendSearchResultMenu.Close(new System.Action(this.ShowSearchResult));
            }
            else
            {
                this.friendSearchResultMenu.Open(this.selectFriendEntity, this.classButtonControl.GetCursorPos, new FriendSearchResultMenu.CallbackFunc(this.OnSelectOffer));
                this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), false);
                base.myFSM.SendEvent("END_ACTION");
            }
        }
    }

    protected void StartSearchInput()
    {
        if (this.friendSearchResultMenu.IsOpen)
        {
            this.friendSearchResultMenu.Open(this.selectFriendEntity, this.classButtonControl.GetCursorPos, new FriendSearchResultMenu.CallbackFunc(this.OnSelectOffer));
        }
        this.friendSearchMenu.Open(new FriendSearchMenu.CallbackFunc(this.OnSelectSearchFriend), false);
    }

    protected TabKind tabKind
    {
        get => 
            tabKindSave;
        set
        {
            tabKindSave = value;
        }
    }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INIT_SHOW_FRIEND,
        INIT_SHOW_FRIEND2,
        INPUT_SHOW_FRIEND,
        QUIT_SHOW_FRIEND,
        QUIT_SHOW_FRIEND2,
        INIT_SHOW_OFFER,
        INIT_SHOW_OFFER2,
        INPUT_SHOW_OFFER,
        QUIT_SHOW_OFFER,
        QUIT_SHOW_OFFER2,
        INIT_SHOW_OFFERED,
        INIT_SHOW_OFFERED2,
        INPUT_SHOW_OFFERED,
        QUIT_SHOW_OFFERED,
        QUIT_SHOW_OFFERED2,
        INIT_SHOW_SEARCH,
        INIT_SHOW_SEARCH2,
        INPUT_SHOW_SEARCH,
        QUIT_SHOW_SEARCH,
        QUIT_SHOW_SEARCH2,
        INIT_SEARCH_FRIEND,
        INIT_SEARCH_FRIEND2,
        INPUT_SEARCH_FRIEND,
        QUIT_SEARCH_FRIEND,
        QUIT_SEARCH_FRIEND2,
        INIT_FRIEND_OFFER_CONFIRM,
        INPUT_FRIEND_OFFER_CONFIRM,
        QUIT_FRIEND_OFFER_CONFIRM,
        INPUT_FRIEND_OFFER_CONFIRM_MAX_FRIEND,
        INIT_FRIEND_ACCEPT_CONFIRM,
        INPUT_FRIEND_ACCEPT_CONFIRM,
        QUIT_FRIEND_ACCEPT_CONFIRM,
        INIT_FRIEND_REJECT_CONFIRM,
        INPUT_FRIEND_REJECT_CONFIRM,
        QUIT_FRIEND_REJECT_CONFIRM,
        INIT_FRIEND_CANCEL_CONFIRM,
        INPUT_FRIEND_CANCEL_CONFIRM,
        QUIT_FRIEND_CANCEL_CONFIRM,
        INIT_FRIEND_REMOVE_CONFIRM,
        INPUT_FRIEND_REMOVE_CONFIRM,
        QUIT_FRIEND_REMOVE_CONFIRM
    }

    public enum TabKind
    {
        FRIEND,
        OFFERED,
        SEARCH,
        DEFAULT
    }
}

