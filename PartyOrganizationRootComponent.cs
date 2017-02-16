using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyOrganizationRootComponent : SceneRootComponent
{
    protected long activeDeckId;
    protected PartyListViewItem basePartyItem;
    protected BattleSetupInfo battleSetupInfo;
    protected string[] cacheAssetNameList;
    protected PartyOrganizationChangeMenu.Mode changePartyMode;
    [SerializeField]
    protected DeckNameInputMenu deckNameInputMenu;
    protected static float EDIT_SCENE_FADE_TIME = 0.5f;
    protected string editDeckName;
    protected int followerClassId;
    protected FollowerInfo followerInfo;
    [SerializeField]
    protected UILabel indexLabel;
    protected MenuMode menuMode;
    [SerializeField]
    protected PartyOrganizationCommandCardMenu operationCommandCardMenu;
    [SerializeField]
    protected PartyOrganizationConfirmDeck2Menu operationConfirmDeck2Menu;
    [SerializeField]
    protected PartyOrganizationConfirmDeckMenu operationConfirmDeckMenu;
    [SerializeField]
    protected PartyOrganizationConfirmMenu operationConfirmMenu;
    [SerializeField]
    protected PartyOrganizationEventPointMenu operationEventPointMenu;
    protected PartyListViewItem partyItem;
    [SerializeField]
    protected PartyListMenu partyListMenu;
    [SerializeField]
    protected PartyOrganizationChangeMenu partyOrganizationChangeMenu;
    [SerializeField]
    protected PartyOrganizationMenu partyOrganizationMenu;
    [SerializeField]
    protected PartyServantSelectMenu partyServantSelectMenu;
    protected int selectPartyMemberNum;
    protected int selectPartyNum;
    protected EventUpValSetupInfo setupInfo;
    protected PartyListViewItem.SetupKind setupKind;
    protected State showBackupState;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected TitleInfoControl titleInfo2;
    [SerializeField]
    protected GameObject tutorialMaskBase;
    protected TutorialMode tutorialMode;
    protected TutorialState tutorialState;
    protected UserDeckEntity[] userDeckEntityList;

    public void BackOrganizationChange()
    {
        if (this.state == State.INPUT_ORGANIZATION_CHANGE)
        {
            this.state = State.QUIT_ORGANIZATION_CHANGE;
            this.RefreshInfo();
            this.SetCacheAssetNameList(this.partyOrganizationChangeMenu.GetItem().GetAssetNameList());
            this.partyOrganizationChangeMenu.Close(new System.Action(this.OnMoveEnd));
        }
    }

    public void BackOrganizationSwap()
    {
        if (this.state == State.INPUT_ORGANIZATION_SWAP)
        {
            this.state = State.QUIT_ORGANIZATION_SWAP;
            this.RefreshInfo();
            this.SetCacheAssetNameList(this.partyOrganizationMenu.GetItem().GetAssetNameList());
            this.partyOrganizationMenu.Close(new System.Action(this.OnMoveEnd));
        }
    }

    public void BackParty()
    {
        if (this.state == State.INPUT_SELECT_PARTY)
        {
            this.state = State.QUIT_SELECT_PARTY;
            if (this.menuMode == MenuMode.QUEST_START)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, EDIT_SCENE_FADE_TIME, new System.Action(this.EndFadeBackParty));
            }
            else
            {
                this.EndFadeBackParty();
            }
        }
    }

    public void BackQuest()
    {
        if (this.state == State.INPUT_QUEST_START)
        {
            this.state = State.QUIT_QUEST_START;
            this.EndFadeBackQuest();
        }
    }

    public bool BackScene()
    {
        this.state = State.QUIT_SCENE;
        BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
        this.battleSetupInfo = null;
        if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
        {
            if (battleSetupInfo != null)
            {
                PartyListViewItem centerItem = this.partyListMenu.GetCenterItem();
                if (centerItem != null)
                {
                    battleSetupInfo.deckId = centerItem.DeckId;
                }
                battleSetupInfo.isChildFollower = false;
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, battleSetupInfo);
            }
            else
            {
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
            }
        }
        else
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Formation, SceneManager.FadeType.BLACK, null);
        }
        return true;
    }

    public override void beginFinish()
    {
        this.partyListMenu.Init();
        this.partyOrganizationChangeMenu.Init();
        this.partyOrganizationMenu.Init();
        this.partyServantSelectMenu.Init();
        this.operationConfirmMenu.Init();
        this.operationConfirmDeckMenu.Init();
        this.operationConfirmDeck2Menu.Init();
        this.operationCommandCardMenu.Init();
        this.operationEventPointMenu.Init();
        this.state = State.INIT;
        this.userDeckEntityList = null;
        this.followerInfo = null;
        this.basePartyItem = null;
        this.partyItem = null;
        this.SetCacheAssetNameList(null);
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        this.menuMode = MenuMode.SELECT_PARTY;
        this.selectPartyNum = -1;
        this.changePartyMode = PartyOrganizationChangeMenu.Mode.NONE;
        this.selectPartyMemberNum = -1;
        this.state = State.INIT;
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        object data = (this.state != State.MASTER_FORMATION_SCENE) ? null : this.battleSetupInfo;
        this.beginFinish();
        this.beginStartUp(data);
        base.resumeMainMenuBar();
    }

    public override void beginStartUp(object data)
    {
        this.battleSetupInfo = data as BattleSetupInfo;
        this.setupInfo = null;
        this.followerInfo = null;
        this.selectPartyNum = -1;
        this.changePartyMode = PartyOrganizationChangeMenu.Mode.NONE;
        this.selectPartyMemberNum = -1;
        this.tutorialMode = TutorialMode.NONE;
        this.tutorialState = TutorialState.NONE;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.activeDeckId = (entity == null) ? 0L : entity.activeDeckId;
        if (this.battleSetupInfo != null)
        {
            this.setupKind = PartyListViewItem.SetupKind.BATTLE_SETUP;
            this.menuMode = MenuMode.QUEST_START;
            this.setupInfo = this.battleSetupInfo.eventUpValSetupInfo;
            if (this.battleSetupInfo.deckId > 0L)
            {
                this.activeDeckId = this.battleSetupInfo.deckId;
            }
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
            {
                if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
                {
                    base.sendMessage("SKIP_SETUP");
                    return;
                }
                this.tutorialMode = TutorialMode.FRIEND;
                this.tutorialState = TutorialState.INIT;
            }
            else if (this.battleSetupInfo.questId == 0xf4242)
            {
                this.tutorialMode = TutorialMode.NPC;
                this.tutorialState = TutorialState.INIT;
            }
            this.selectPartyNum = 0;
            long followerId = this.battleSetupInfo.followerId;
            if (followerId > 0L)
            {
                this.followerInfo = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcFollowerMaster>(DataNameKind.Kind.NPC_FOLLOWER).GetFollower(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, followerId);
                this.followerClassId = this.battleSetupInfo.followerClassId;
                if (this.followerInfo != null)
                {
                    Debug.Log("FollowerInfo is NPC " + followerId);
                }
                else if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_FOLLOWER).isSingleEntityExists())
                {
                    this.followerInfo = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_FOLLOWER).getSingleEntity<UserFollowerEntity>().getFollowerInfo(followerId);
                    if (this.followerInfo != null)
                    {
                        Debug.Log("FollowerInfo is USER " + followerId);
                    }
                }
            }
            this.titleInfo.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.BACK, TitleInfoControl.TitleKind.BATTLE_SETUP_CONFIRM);
        }
        else
        {
            this.setupKind = PartyListViewItem.SetupKind.PARTY_ORGANIZATION;
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END) && !TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
            {
                this.tutorialMode = TutorialMode.ORGANIZATION_GUIDE;
                this.tutorialState = TutorialState.INIT;
            }
            this.selectPartyNum = 0;
            SoundManager.playBgm("BGM_CHALDEA_1");
            this.titleInfo.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION);
        }
        this.RefreshInfo();
        base.beginStartUp();
    }

    public bool CheckTabKind()
    {
        switch (this.menuMode)
        {
            case MenuMode.QUEST_START:
                base.myFSM.SendEvent("MENU_QUEST_START");
                break;

            case MenuMode.SELECT_PARTY:
                base.myFSM.SendEvent("MENU_PARTY_SELECT");
                break;

            case MenuMode.ORGANIZATION_CHANGE:
                base.myFSM.SendEvent("MENU_PARTY_OGRANIZATION_CHANGE");
                break;

            case MenuMode.ORGANIZATION_SWAP:
                base.myFSM.SendEvent("MENU_PARTY_OGRANIZATION_SWAP");
                break;
        }
        return true;
    }

    public bool ConfirmPartyOrganizationChangeCancel()
    {
        PartyListViewItem item = this.partyOrganizationChangeMenu.GetItem();
        if (item != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            if (item.CompMember(this.basePartyItem))
            {
                if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
                {
                    this.menuMode = MenuMode.QUEST_START;
                }
                else
                {
                    this.menuMode = MenuMode.SELECT_PARTY;
                }
                base.myFSM.SendEvent("MENU_BACK");
            }
            else
            {
                this.operationConfirmDeck2Menu.Open(PartyOrganizationConfirmDeck2Menu.Kind.CANCEL, this.partyOrganizationChangeMenu.GetBaseItem(), this.partyOrganizationChangeMenu.GetItem(), new PartyOrganizationConfirmDeck2Menu.CallbackFunc(this.EndConfirmPartyOrganizationChangeCancel));
            }
            return true;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool ConfirmPartyOrganizationChangeDecide()
    {
        PartyListViewItem item = this.partyOrganizationChangeMenu.GetItem();
        if (item == null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
            return false;
        }
        if (this.tutorialState != TutorialState.SELECT_CHANGE_DECIDE)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        }
        switch (item.GetDeckCondition())
        {
            case PartyListViewItem.DeckCondition.OK:
                base.myFSM.SendEvent("MENU_DECIDE");
                break;

            case PartyListViewItem.DeckCondition.EMPTY_DECK_MEMBER:
            case PartyListViewItem.DeckCondition.LEADER_ONLY_DECK_MEMBER:
            case PartyListViewItem.DeckCondition.SHORTAGE_DECK_MEMBER:
                this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.EMPTY_DECK_MEMBER, this.partyOrganizationChangeMenu.GetBaseItem(), new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndConfirmPartyOrganizationChangeDecide));
                break;

            case PartyListViewItem.DeckCondition.COST_OVER:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_TITLE_COST_OVER"), LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_MESSAGE_COST_OVER"), new System.Action(this.EndConfirmPartyOrganizationChangeDecideCancel), -1);
                break;

            default:
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.state = State.INIT_TOP;
                base.myFSM.SendEvent("MENU_CANCEL");
                return false;
        }
        return true;
    }

    public bool ConfirmPartyOrganizationChangeRemove()
    {
        PartyListViewItem partyItem = this.partyOrganizationChangeMenu.GetItem();
        if ((partyItem != null) && (partyItem.GetDeckCondition() != PartyListViewItem.DeckCondition.EMPTY_DECK_MEMBER))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.REMOVE, partyItem, new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndConfirmPartyOrganizationChangeRemove));
            return true;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool ConfirmPartyOrganizationSwapCancel()
    {
        PartyListViewItem item = this.partyOrganizationMenu.GetItem();
        if (item != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            if (item.CompMember(this.basePartyItem))
            {
                if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
                {
                    this.menuMode = MenuMode.QUEST_START;
                }
                else
                {
                    this.menuMode = MenuMode.SELECT_PARTY;
                }
                base.myFSM.SendEvent("MENU_BACK");
            }
            else
            {
                this.operationConfirmDeck2Menu.Open(PartyOrganizationConfirmDeck2Menu.Kind.CANCEL, this.partyOrganizationMenu.GetBaseItem(), this.partyOrganizationMenu.GetItem(), new PartyOrganizationConfirmDeck2Menu.CallbackFunc(this.EndConfirmPartyOrganizationSwapCancel));
            }
            return true;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool ConfirmPartyOrganizationSwapDecide()
    {
        PartyListViewItem item = this.partyOrganizationMenu.GetItem();
        if (item == null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
            return false;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        switch (item.GetDeckCondition())
        {
            case PartyListViewItem.DeckCondition.OK:
                base.myFSM.SendEvent("MENU_DECIDE");
                break;

            case PartyListViewItem.DeckCondition.EMPTY_DECK_MEMBER:
            case PartyListViewItem.DeckCondition.LEADER_ONLY_DECK_MEMBER:
            case PartyListViewItem.DeckCondition.SHORTAGE_DECK_MEMBER:
                this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.SHORTAGE_DECK_MEMBER, this.partyOrganizationMenu.GetItem(), new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndConfirmPartyOrganizationSwapDecide));
                break;

            case PartyListViewItem.DeckCondition.COST_OVER:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_TITLE_COST_OVER"), LocalizationManager.Get("PARTY_ORGANIZATION_DECIDE_WARNING_MESSAGE_COST_OVER"), new System.Action(this.EndConfirmPartyOrganizationSwapDecideCancel), -1);
                break;

            default:
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.state = State.INIT_TOP;
                base.myFSM.SendEvent("MENU_CANCEL");
                return false;
        }
        return true;
    }

    public bool ConfirmPartyOrganizationSwapRemove()
    {
        PartyListViewItem partyItem = this.partyOrganizationMenu.GetItem();
        if ((partyItem != null) && !partyItem.IsDeckEmpty())
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.REMOVE, partyItem, new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndConfirmPartyOrganizationSwapRemove));
            return true;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    protected void EndClassCompatibilityMenu()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseClassCompatibilityMenu(null);
    }

    protected void EndCloseOrganizationChangeServantEquipListCancel()
    {
        this.EndSelectedOrganizationChange();
    }

    protected void EndCloseOrganizationChangeServantEquipListDecide()
    {
        this.state = State.INPUT_ORGANIZATION_CHANGE;
        this.RecoverInputShowMenu();
    }

    protected void EndCloseShowClassCompatibilityMenu()
    {
        this.state = this.showBackupState;
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndCloseShowCommandCard()
    {
        this.state = this.showBackupState;
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndCloseShowEventPoint()
    {
        this.state = this.showBackupState;
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndCloseSwapServantEquipListCancel()
    {
        this.state = State.INPUT_ORGANIZATION_SWAP;
        this.RecoverInputShowMenu();
    }

    protected void EndCloseSwapServantEquipListDecide()
    {
        this.state = State.INPUT_ORGANIZATION_SWAP;
        this.RecoverInputShowMenu();
    }

    protected void EndCloseTutorialArrowChange()
    {
        this.tutorialMaskBase.SetActive(false);
        base.myFSM.SendEvent("CLICK_BACK");
    }

    protected void EndConfirmPartyOrganizationChangeCancel(bool isDecide)
    {
        this.operationConfirmDeck2Menu.Close();
        if (isDecide)
        {
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            base.myFSM.SendEvent("MENU_BACK");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyOrganizationChangeDecide(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        if (isDecide)
        {
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            base.myFSM.SendEvent("MENU_BACK");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyOrganizationChangeDecideCancel()
    {
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndConfirmPartyOrganizationChangeRemove(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        if (isDecide)
        {
            this.partyOrganizationChangeMenu.GetItem().ClearMember();
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_DECIDE");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyOrganizationSwapCancel(bool isDecide)
    {
        this.operationConfirmDeck2Menu.Close();
        if (isDecide)
        {
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            base.myFSM.SendEvent("MENU_BACK");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyOrganizationSwapDecide(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        if (isDecide)
        {
            base.myFSM.SendEvent("MENU_DECIDE");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyOrganizationSwapDecideCancel()
    {
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("MENU_CANCEL");
    }

    protected void EndConfirmPartyOrganizationSwapRemove(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        if (isDecide)
        {
            this.partyOrganizationMenu.GetItem().ClearMember();
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_DECIDE");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("MENU_CANCEL");
        }
    }

    protected void EndConfirmPartyRemove(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        if (isDecide)
        {
            this.partyItem.ClearMember();
            this.partyOrganizationChangeMenu.Init();
            this.menuMode = MenuMode.ORGANIZATION_CHANGE;
            base.myFSM.SendEvent("CLICK_REMOVE");
        }
        else
        {
            this.RecoverInputShowMenu();
        }
    }

    protected void EndFadeBackParty()
    {
        this.RefreshInfo();
        this.partyListMenu.Close(new System.Action(this.OnMoveEnd));
    }

    protected void EndFadeBackQuest()
    {
        this.RefreshInfo();
        this.partyListMenu.Close(new System.Action(this.OnMoveEnd));
    }

    protected void EndFadeSelectParty()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(EDIT_SCENE_FADE_TIME, new System.Action(this.OnMoveEnd));
    }

    protected void EndFadeSelectQuest()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(EDIT_SCENE_FADE_TIME, new System.Action(this.OnMoveEnd));
    }

    protected void EndInputDeckNameMenu(bool isDecide, string changeName)
    {
        <EndInputDeckNameMenu>c__AnonStoreyA4 ya = new <EndInputDeckNameMenu>c__AnonStoreyA4 {
            isDecide = isDecide,
            <>f__this = this
        };
        if (ya.isDecide)
        {
            this.editDeckName = changeName;
        }
        this.deckNameInputMenu.Close(new System.Action(ya.<>m__175));
    }

    protected void EndOpenOrganizationChangeServant()
    {
        if (this.tutorialState == TutorialState.SELECT_PARTY)
        {
            this.tutorialState = TutorialState.SELECT_SERVANT;
            if (this.tutorialMode == TutorialMode.ORGANIZATION_GUIDE)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_PARTY_ORGANIZATION3"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialOrganizationGuideNotification3));
            }
        }
    }

    protected void EndOpenTutorialArrow()
    {
        this.state = State.INPUT_SELECT_PARTY;
    }

    protected void EndQuestStartScriptAction()
    {
        ScriptManager.PlayBattleStart(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, new ScriptManager.CallbackFunc(this.EndScriptAction), false);
    }

    protected void EndRequestBattleSetup(string result)
    {
        Debug.Log("endBattleRequest:" + result);
        if (result.Equals("ng"))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
            SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
        }
        else
        {
            BattleEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.BATTLE).getSingleEntity<BattleEntity>();
            if (entity != null)
            {
                BattleData.setResumeBattleId(entity.id, entity.questId, entity.questPhase);
            }
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    protected void EndRequestDeckName(string result)
    {
        Debug.Log("EndRequestDeckName");
        if (result != "ng")
        {
            this.userDeckEntityList = null;
            this.partyListMenu.Init();
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndRequestOrganizationChange(string result)
    {
        Debug.Log("EndRequestOrganizationChange");
        if (result != "ng")
        {
            this.userDeckEntityList = null;
            this.partyListMenu.Init();
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            if (this.tutorialMode == TutorialMode.ORGANIZATION_GUIDE)
            {
                TutorialFlag.SetProgress(TutorialFlag.Progress._3);
            }
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndRequestOrganizationSelect(string result)
    {
        Debug.Log("EndRequestOrganizationSelect");
        if (result != "ng")
        {
            this.userDeckEntityList = null;
            this.partyListMenu.Init();
            this.menuMode = MenuMode.QUEST_START;
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            this.state = State.INIT_TOP;
            this.activeDeckId = 0L;
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndRequestOrganizationSwap(string result)
    {
        Debug.Log("EndRequestOrganizationSwap");
        if (result != "ng")
        {
            this.userDeckEntityList = null;
            this.partyListMenu.Init();
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            this.state = State.INIT_TOP;
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndScriptAction(bool isExit)
    {
        base.myFSM.SendEvent("SCRIPT_FINISHED");
    }

    protected void EndSelectedOrganizationChange()
    {
        this.state = State.INPUT_ORGANIZATION_CHANGE;
        if (this.partyItem.CompMember(this.basePartyItem))
        {
            if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
            {
                this.menuMode = MenuMode.QUEST_START;
            }
            else
            {
                this.menuMode = MenuMode.SELECT_PARTY;
            }
            this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.NONE, this.basePartyItem, this.partyItem, -1, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
            this.partyServantSelectMenu.Close();
            base.myFSM.SendEvent("NO_MODIFY");
        }
        else
        {
            this.state = State.INPUT_ORGANIZATION_CHANGE;
            this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.NONE, this.basePartyItem, this.partyItem, -1, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
            this.partyServantSelectMenu.Close();
        }
    }

    protected void EndSelectedOrganizationChangeServant(PartyServantSelectMenu.ResultKind result, PartyServantListViewItem item)
    {
        this.partyOrganizationChangeMenu.ModifyItem();
        if (result != PartyServantSelectMenu.ResultKind.CANCEL)
        {
            if (item.IsBase)
            {
                this.partyItem.ClearMember(this.selectPartyMemberNum);
            }
            else if (item.IsParty)
            {
                this.partyItem.SwapMember(this.selectPartyMemberNum, item.PartyIndex);
            }
            else
            {
                this.partyItem.SetMember(this.selectPartyMemberNum, item);
            }
        }
        this.selectPartyMemberNum = -1;
        this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_CHANGE);
        if (this.tutorialState == TutorialState.SELECT_SERVANT)
        {
            this.tutorialState = TutorialState.SELECT_CHANGE_DECIDE;
            this.state = State.INPUT_ORGANIZATION_CHANGE;
            this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.NONE, this.basePartyItem, this.partyItem, -1, this.setupInfo, null, null);
            this.partyServantSelectMenu.Close(new System.Action(this.EndTutorialOrganizationGuideNotification4));
        }
        else
        {
            this.EndSelectedOrganizationChange();
        }
    }

    protected void EndSelectedOrganizationSwapServant(PartyServantSelectMenu.ResultKind result, PartyServantListViewItem item)
    {
        this.partyOrganizationMenu.ModifyItem();
        if (result != PartyServantSelectMenu.ResultKind.CANCEL)
        {
            if (item.IsBase)
            {
                this.partyItem.ClearMember(this.selectPartyMemberNum);
            }
            else if (item.IsParty)
            {
                this.partyItem.SwapMember(this.selectPartyMemberNum, item.PartyIndex);
            }
            else
            {
                this.partyItem.SetMember(this.selectPartyMemberNum, item);
            }
        }
        this.selectPartyMemberNum = -1;
        this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SWAP);
        this.partyServantSelectMenu.Close();
        this.state = State.INPUT_ORGANIZATION_SWAP;
        this.RecoverInputShowMenu();
    }

    protected void EndSelectOrganizationChangeServantEquip(EquipGraphListMenu.ResultKind result, EquipGraphListViewItem equipItem)
    {
        if (result == EquipGraphListMenu.ResultKind.DECIDE)
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.selectPartyMemberNum);
            long userSvtId = (equipItem == null) ? 0L : equipItem.UserServant.id;
            if (member.EquipUserSvtId != userSvtId)
            {
                this.partyItem.SetEquip(this.selectPartyMemberNum, userSvtId);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(new System.Action(this.EndCloseOrganizationChangeServantEquipListDecide));
                return;
            }
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(new System.Action(this.EndCloseOrganizationChangeServantEquipListCancel));
    }

    protected void EndSelectOrganizationSwapServantEquip(EquipGraphListMenu.ResultKind result, EquipGraphListViewItem equipItem)
    {
        if (result == EquipGraphListMenu.ResultKind.DECIDE)
        {
            PartyOrganizationListViewItem member = this.partyItem.GetMember(this.selectPartyMemberNum);
            long userSvtId = (equipItem == null) ? 0L : equipItem.UserServant.id;
            if (member.EquipUserSvtId != userSvtId)
            {
                this.partyItem.SetEquip(this.selectPartyMemberNum, userSvtId);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(new System.Action(this.EndCloseSwapServantEquipListDecide));
                return;
            }
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(new System.Action(this.EndCloseSwapServantEquipListCancel));
    }

    protected void EndShowClassCompatibilityMenu()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseClassCompatibilityMenu(new System.Action(this.EndCloseShowClassCompatibilityMenu));
    }

    protected void EndShowCommandCard(bool isDecide)
    {
        this.operationCommandCardMenu.Close(new System.Action(this.EndCloseShowCommandCard));
    }

    protected void EndShowEventPoint(bool isDecide)
    {
        this.operationEventPointMenu.Close(new System.Action(this.EndCloseShowEventPoint));
    }

    protected void EndTutorialFollowerGuideNotification1()
    {
        this.state = State.INPUT_SELECT_PARTY;
        this.tutorialState = TutorialState.SELECT_START;
        this.partyListMenu.Open(PartyListViewItem.MenuKind.QUEST_START, PartyListMenu.TutorialMode.FOLLOWER_GUIDE_START_SELECT, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
    }

    protected void EndTutorialOrganizationGuideNotification1()
    {
        this.state = State.INPUT_SELECT_PARTY;
        this.tutorialState = TutorialState.SELECT_PARTY;
        this.partyListMenu.Open(PartyListViewItem.MenuKind.SELECT_PARTY, PartyListMenu.TutorialMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
    }

    protected void EndTutorialOrganizationGuideNotification2()
    {
        this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT, this.basePartyItem, this.partyItem, -1, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
    }

    protected void EndTutorialOrganizationGuideNotification3()
    {
        this.titleInfo2.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SERVANT);
        this.partyServantSelectMenu.Open(PartyServantSelectMenu.TutorialMode.ORGANIZATION_GUIDE_FIRST_SELECT, this.partyItem, this.selectPartyMemberNum, new PartyServantSelectMenu.CallbackFunc(this.EndSelectedOrganizationChangeServant), null);
    }

    protected void EndTutorialOrganizationGuideNotification4()
    {
        this.EndTutorialOrganizationGuideNotification5();
    }

    protected void EndTutorialOrganizationGuideNotification5()
    {
        this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.ORGANIZATION_GUIDE_DECIDE_SELECT, this.basePartyItem, this.partyItem, -1, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
    }

    protected void EndTutorialOrganizationGuideNotification6()
    {
        this.tutorialMaskBase.SetActive(true);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(-430f, 250f), (float) 180f, new Rect(-510f, 225f, 160f, 60f), new System.Action(this.EndOpenTutorialArrow));
    }

    protected void EndWarningQuestStart(bool isDecide)
    {
        this.operationConfirmDeckMenu.Close();
        this.RecoverInputShowMenu();
    }

    public bool FollowerScene()
    {
        this.state = State.QUIT_SCENE;
        BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
        this.battleSetupInfo = null;
        if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
        {
            if (battleSetupInfo != null)
            {
                PartyListViewItem centerItem = this.partyListMenu.GetCenterItem();
                if (centerItem != null)
                {
                    battleSetupInfo.deckId = centerItem.DeckId;
                }
                battleSetupInfo.isChildFollower = true;
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, battleSetupInfo);
            }
            else
            {
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
            }
        }
        else
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Formation, SceneManager.FadeType.BLACK, null);
        }
        return true;
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.state = State.INIT_TOP;
        }
    }

    public bool InputDeckName()
    {
        if (this.basePartyItem != null)
        {
            this.showBackupState = this.state;
            this.state = State.INPUT_DECK_NAME;
            this.deckNameInputMenu.Open(this.basePartyItem.DeckName, new DeckNameInputMenu.CallbackFunc(this.EndInputDeckNameMenu));
            return true;
        }
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool MasterFormationScene()
    {
        this.state = State.MASTER_FORMATION_SCENE;
        if (this.battleSetupInfo != null)
        {
            PartyListViewItem centerItem = this.partyListMenu.GetCenterItem();
            if (centerItem != null)
            {
                this.battleSetupInfo.deckId = centerItem.DeckId;
            }
        }
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.MasterFormation, SceneManager.FadeType.BLACK, null);
        return true;
    }

    public void OnClickBack()
    {
        Debug.Log("OnClickBack: state " + this.state);
        switch (this.state)
        {
            case State.INPUT_ORGANIZATION_CHANGE:
                this.partyOrganizationChangeMenu.OnClickCancel();
                return;

            case State.INPUT_ORGANIZATION_SWAP:
                this.partyOrganizationMenu.OnClickCancel();
                return;

            case State.INPUT_ORGANIZATION_CHANGE_SERVANT_SELECT:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.partyServantSelectMenu.Close();
                this.state = State.INPUT_ORGANIZATION_CHANGE;
                if (!this.partyItem.CompMember(this.basePartyItem))
                {
                    this.RecoverInputShowMenu();
                    return;
                }
                this.menuMode = MenuMode.SELECT_PARTY;
                base.myFSM.SendEvent("NO_MODIFY");
                return;

            case State.INPUT_ORGANIZATION_SWAP_SERVANT_SELECT:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.partyServantSelectMenu.Close();
                this.state = State.INPUT_ORGANIZATION_SWAP;
                this.RecoverInputShowMenu();
                return;

            case State.INPUT_ORGANIZATION_CHANGE_EQUIP_SELECT:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(null);
                this.state = State.INPUT_ORGANIZATION_CHANGE;
                if (!this.partyItem.CompMember(this.basePartyItem))
                {
                    this.RecoverInputShowMenu();
                    return;
                }
                this.menuMode = MenuMode.SELECT_PARTY;
                base.myFSM.SendEvent("NO_MODIFY");
                return;

            case State.INPUT_ORGANIZATION_SWAP_EQUIP_SELECT:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(null);
                this.state = State.INPUT_ORGANIZATION_SWAP;
                this.RecoverInputShowMenu();
                return;

            case State.INPUT_QUEST_START:
                Debug.Log("OnClickBack: INPUT_QUEST_START");
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                base.myFSM.SendEvent("CLICK_BACK");
                break;

            case State.INPUT_SELECT_PARTY:
                Debug.Log("OnClickBack: INPUT_SELECT_PARTY " + this.setupKind);
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                if (this.tutorialState == TutorialState.SELECT_CHANGE_DECIDE)
                {
                    this.state = State.QUIT_SELECT_PARTY;
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrowChange));
                }
                else if (this.setupKind == PartyListViewItem.SetupKind.BATTLE_SETUP)
                {
                    this.menuMode = MenuMode.QUEST_START;
                    base.myFSM.SendEvent("CLICK_BACK_QUEST");
                }
                else
                {
                    base.myFSM.SendEvent("CLICK_BACK");
                }
                return;
        }
    }

    protected void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_QUEST_START:
            case State.INIT_QUEST_START2:
                this.state = State.INPUT_QUEST_START;
                this.menuMode = MenuMode.QUEST_START;
                this.RefreshInfo();
                if (this.tutorialState == TutorialState.INIT)
                {
                    this.state = State.INIT_QUEST_START;
                    switch (this.tutorialMode)
                    {
                        case TutorialMode.FRIEND:
                            this.EndTutorialFollowerGuideNotification1();
                            goto Label_00D6;

                        case TutorialMode.NPC:
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FOLLOWER_ORGANIZATION2"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialFollowerGuideNotification1));
                            goto Label_00D6;
                    }
                }
                break;

            case State.INPUT_QUEST_START:
            case State.INPUT_SELECT_PARTY:
            case State.INPUT_ORGANIZATION_CHANGE:
            case State.INPUT_ORGANIZATION_SWAP:
                return;

            case State.QUIT_QUEST_START:
            case State.QUIT_QUEST_START2:
                this.state = State.INIT_TOP;
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.INIT_SELECT_PARTY:
            case State.INIT_SELECT_PARTY2:
                this.state = State.INPUT_SELECT_PARTY;
                this.menuMode = MenuMode.SELECT_PARTY;
                this.RefreshInfo();
                if (this.tutorialState != TutorialState.INIT)
                {
                    if (this.tutorialState == TutorialState.SELECT_CHANGE_DECIDE)
                    {
                        this.state = State.INIT_SELECT_PARTY;
                        this.tutorialState = TutorialState.SELECT_RETURN;
                        this.EndTutorialOrganizationGuideNotification6();
                    }
                    else
                    {
                        this.partyListMenu.Open(PartyListViewItem.MenuKind.SELECT_PARTY, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
                    }
                }
                else
                {
                    this.state = State.INIT_SELECT_PARTY;
                    switch (this.tutorialMode)
                    {
                        case TutorialMode.ORGANIZATION_GUIDE:
                            this.EndTutorialOrganizationGuideNotification1();
                            break;

                        case TutorialMode.FRIEND:
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FOLLOWER_ORGANIZATION1"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialFollowerGuideNotification1));
                            break;

                        case TutorialMode.NPC:
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FOLLOWER_ORGANIZATION2"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialFollowerGuideNotification1));
                            break;
                    }
                }
                this.SetCacheAssetNameList(null);
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.QUIT_SELECT_PARTY:
            case State.QUIT_SELECT_PARTY2:
                this.state = State.INIT_TOP;
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.INIT_ORGANIZATION_CHANGE:
            case State.INIT_ORGANIZATION_CHANGE2:
                this.state = State.INPUT_ORGANIZATION_CHANGE;
                this.menuMode = MenuMode.ORGANIZATION_CHANGE;
                this.RefreshInfo();
                Debug.Log("INPUT_ORGANIZATION_CHANGE: " + this.selectPartyMemberNum);
                this.partyOrganizationChangeMenu.Open(this.setupKind, this.changePartyMode, this.basePartyItem, this.partyItem, this.selectPartyMemberNum, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.QUIT_ORGANIZATION_CHANGE:
            case State.QUIT_ORGANIZATION_CHANGE2:
                this.state = State.INIT_TOP;
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.INIT_ORGANIZATION_SWAP:
            case State.INIT_ORGANIZATION_SWAP2:
                this.state = State.INPUT_ORGANIZATION_SWAP;
                this.menuMode = MenuMode.ORGANIZATION_SWAP;
                this.RefreshInfo();
                this.partyOrganizationMenu.Open(this.setupKind, this.basePartyItem, this.partyItem, this.setupInfo, new PartyOrganizationMenu.CallbackFunc(this.OnSelectedOrganizationSwap), null);
                base.myFSM.SendEvent("END_ACTION");
                return;

            case State.QUIT_ORGANIZATION_SWAP:
            case State.QUIT_ORGANIZATION_SWAP2:
                this.state = State.INIT_TOP;
                this.RefreshInfo();
                base.myFSM.SendEvent("END_ACTION");
                return;

            default:
                return;
        }
    Label_00D6:
        this.partyListMenu.Open(PartyListViewItem.MenuKind.QUEST_START, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
        this.SetCacheAssetNameList(null);
        base.myFSM.SendEvent("END_ACTION");
    }

    protected void OnSelectedOrganizationChange(PartyOrganizationChangeMenu.ResultKind result, int m)
    {
        this.changePartyMode = PartyOrganizationChangeMenu.Mode.NONE;
        this.selectPartyMemberNum = m;
        switch (result)
        {
            case PartyOrganizationChangeMenu.ResultKind.CANCEL:
                base.myFSM.SendEvent("CLICK_BACK");
                break;

            case PartyOrganizationChangeMenu.ResultKind.DECIDE:
                base.myFSM.SendEvent("CLICK_DECIDE");
                break;

            case PartyOrganizationChangeMenu.ResultKind.REMOVE:
                base.myFSM.SendEvent("CLICK_REMOVE");
                break;

            case PartyOrganizationChangeMenu.ResultKind.CLASS_COMPATIBILITY:
                base.myFSM.SendEvent("CLICK_CLASS_COMPATIBILITY");
                break;

            case PartyOrganizationChangeMenu.ResultKind.COMMAND_CARD:
                base.myFSM.SendEvent("CLICK_COMMAND_CARD");
                break;

            case PartyOrganizationChangeMenu.ResultKind.EVENT_POINT:
                base.myFSM.SendEvent("CLICK_EVENT_POINT");
                break;

            case PartyOrganizationChangeMenu.ResultKind.SELECT_SERVANT:
                this.state = State.INPUT_ORGANIZATION_CHANGE_SERVANT_SELECT;
                if (this.tutorialState != TutorialState.SELECT_PARTY)
                {
                    this.partyServantSelectMenu.Open(PartyServantSelectMenu.TutorialMode.NONE, this.partyItem, this.selectPartyMemberNum, new PartyServantSelectMenu.CallbackFunc(this.EndSelectedOrganizationChangeServant), new System.Action(this.EndOpenOrganizationChangeServant));
                    break;
                }
                this.partyServantSelectMenu.Open(PartyServantSelectMenu.TutorialMode.ORGANIZATION_GUIDE_FIRST_DIALOG, this.partyItem, this.selectPartyMemberNum, new PartyServantSelectMenu.CallbackFunc(this.EndSelectedOrganizationChangeServant), new System.Action(this.EndOpenOrganizationChangeServant));
                break;

            case PartyOrganizationChangeMenu.ResultKind.SELECT_EQUIP:
                this.state = State.INPUT_ORGANIZATION_CHANGE_EQUIP_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.OpenEquipGraphListMenu(this.partyItem, this.selectPartyMemberNum, new EquipGraphListMenu.CallbackFunc(this.EndSelectOrganizationChangeServantEquip));
                break;
        }
    }

    protected void OnSelectedOrganizationSwap(PartyOrganizationMenu.ResultKind result, int m)
    {
        this.selectPartyMemberNum = m;
        switch (result)
        {
            case PartyOrganizationMenu.ResultKind.CANCEL:
                base.myFSM.SendEvent("CLICK_BACK");
                return;

            case PartyOrganizationMenu.ResultKind.DECIDE:
                if (!this.partyItem.CompMember(this.basePartyItem))
                {
                    base.myFSM.SendEvent("CLICK_DECIDE");
                    return;
                }
                if (this.setupKind != PartyListViewItem.SetupKind.BATTLE_SETUP)
                {
                    this.menuMode = MenuMode.SELECT_PARTY;
                    break;
                }
                this.menuMode = MenuMode.QUEST_START;
                break;

            case PartyOrganizationMenu.ResultKind.REMOVE:
                base.myFSM.SendEvent("CLICK_REMOVE");
                return;

            case PartyOrganizationMenu.ResultKind.CLASS_COMPATIBILITY:
                base.myFSM.SendEvent("CLICK_CLASS_COMPATIBILITY");
                return;

            case PartyOrganizationMenu.ResultKind.COMMAND_CARD:
                base.myFSM.SendEvent("CLICK_COMMAND_CARD");
                return;

            case PartyOrganizationMenu.ResultKind.EVENT_POINT:
                base.myFSM.SendEvent("CLICK_EVENT_POINT");
                return;

            case PartyOrganizationMenu.ResultKind.SELECT_SERVANT:
                this.state = State.INPUT_ORGANIZATION_SWAP_SERVANT_SELECT;
                this.titleInfo2.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SERVANT);
                this.partyServantSelectMenu.Open(PartyServantSelectMenu.TutorialMode.NONE, this.partyItem, this.selectPartyMemberNum, new PartyServantSelectMenu.CallbackFunc(this.EndSelectedOrganizationSwapServant), null);
                return;

            case PartyOrganizationMenu.ResultKind.SELECT_EQUIP:
                this.state = State.INPUT_ORGANIZATION_SWAP_EQUIP_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.OpenEquipGraphListMenu(this.partyItem, this.selectPartyMemberNum, new EquipGraphListMenu.CallbackFunc(this.EndSelectOrganizationSwapServantEquip));
                return;

            default:
                return;
        }
        base.myFSM.SendEvent("NO_MODIFY");
    }

    protected void OnSelectPartyList(PartyListMenu.ResultKind result, int n, int m)
    {
        Debug.Log(string.Concat(new object[] { "OnSelectPartyList ", n, " ", m, " state ", this.state }));
        if ((this.state == State.INPUT_QUEST_START) || (this.state == State.INPUT_SELECT_PARTY))
        {
            Debug.Log("OnSelectPartyList result " + result);
            this.selectPartyNum = n;
            this.changePartyMode = PartyOrganizationChangeMenu.Mode.NONE;
            this.selectPartyMemberNum = m;
            this.basePartyItem = this.partyListMenu.GetItem(this.selectPartyNum);
            this.partyItem = this.basePartyItem.Clone();
            this.indexLabel.text = string.Empty + (this.selectPartyNum + 1);
            switch (result)
            {
                case PartyListMenu.ResultKind.DECIDE:
                    if (this.setupKind != PartyListViewItem.SetupKind.BATTLE_SETUP)
                    {
                        base.myFSM.SendEvent("CLICK_DECIDE");
                        return;
                    }
                    this.menuMode = MenuMode.QUEST_START;
                    this.SetCacheAssetNameList(this.basePartyItem.GetAssetNameList());
                    base.myFSM.SendEvent("CLICK_QUEST");
                    return;

                case PartyListMenu.ResultKind.CHANGE:
                    this.partyOrganizationChangeMenu.Init();
                    this.menuMode = MenuMode.ORGANIZATION_CHANGE;
                    base.myFSM.SendEvent("CLICK_CHANGE");
                    return;

                case PartyListMenu.ResultKind.SWAP:
                    this.partyOrganizationMenu.Init();
                    this.menuMode = MenuMode.ORGANIZATION_SWAP;
                    base.myFSM.SendEvent("CLICK_SWAP");
                    return;

                case PartyListMenu.ResultKind.REMOVE:
                    if (this.partyItem.GetDeckCondition() == PartyListViewItem.DeckCondition.EMPTY_DECK_MEMBER)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        this.RecoverInputShowMenu();
                        return;
                    }
                    this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.REMOVE, this.partyItem, new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndConfirmPartyRemove));
                    return;

                case PartyListMenu.ResultKind.DECK_NAME:
                    base.myFSM.SendEvent("CLICK_DECK_NAME");
                    return;

                case PartyListMenu.ResultKind.EDIT:
                    this.menuMode = MenuMode.SELECT_PARTY;
                    this.SetCacheAssetNameList(this.basePartyItem.GetAssetNameList());
                    base.myFSM.SendEvent("CLICK_EDIT");
                    return;

                case PartyListMenu.ResultKind.START:
                    switch (this.partyItem.GetDeckCondition())
                    {
                        case PartyListViewItem.DeckCondition.OK:
                            if (this.tutorialMode != TutorialMode.FRIEND)
                            {
                                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                            }
                            this.battleSetupInfo.deckId = this.basePartyItem.DeckId;
                            base.myFSM.SendEvent("CLICK_START");
                            return;

                        case PartyListViewItem.DeckCondition.EMPTY_DECK_MEMBER:
                        case PartyListViewItem.DeckCondition.LEADER_ONLY_DECK_MEMBER:
                        case PartyListViewItem.DeckCondition.SHORTAGE_DECK_MEMBER:
                            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                            this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.START_SHORTAGE_DECK_MEMBER, this.partyItem, new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndWarningQuestStart));
                            return;

                        case PartyListViewItem.DeckCondition.COST_OVER:
                            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                            this.operationConfirmDeckMenu.Open(PartyOrganizationConfirmDeckMenu.Kind.START_COST_OVER, this.partyItem, new PartyOrganizationConfirmDeckMenu.CallbackFunc(this.EndWarningQuestStart));
                            return;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.RecoverInputShowMenu();
                    return;

                case PartyListMenu.ResultKind.MASTER_FORMATION:
                    base.myFSM.SendEvent("CLICK_MASTER_FORMATION");
                    return;

                case PartyListMenu.ResultKind.CLASS_COMPATIBILITY:
                    base.myFSM.SendEvent("CLICK_CLASS_COMPATIBILITY");
                    return;

                case PartyListMenu.ResultKind.COMMAND_CARD:
                    base.myFSM.SendEvent("CLICK_COMMAND_CARD");
                    return;

                case PartyListMenu.ResultKind.EVENT_POINT:
                    base.myFSM.SendEvent("CLICK_EVENT_POINT");
                    return;

                case PartyListMenu.ResultKind.MODIFY_STATUS:
                    this.SetCacheAssetNameList(this.basePartyItem.GetAssetNameList());
                    this.partyListMenu.Init();
                    this.RecoverInputShowMenu();
                    this.SetCacheAssetNameList(null);
                    return;

                case PartyListMenu.ResultKind.CHANGE_SERVANT:
                case PartyListMenu.ResultKind.TUTORIAL_CHANGE_SERVANT:
                {
                    PartyOrganizationListViewItem member = this.partyItem.GetMember(m);
                    this.changePartyMode = PartyOrganizationChangeMenu.Mode.SELECT_SERVANT;
                    if (!member.IsFollower)
                    {
                        if (result == PartyListMenu.ResultKind.CHANGE_SERVANT)
                        {
                            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        }
                        this.partyOrganizationChangeMenu.Init();
                        this.menuMode = MenuMode.ORGANIZATION_CHANGE;
                        base.myFSM.SendEvent("CLICK_CHANGE");
                        return;
                    }
                    if (this.setupKind != PartyListViewItem.SetupKind.BATTLE_SETUP)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        this.RecoverInputShowMenu();
                        break;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    base.myFSM.SendEvent("CLICK_FOLLOWER");
                    break;
                }
                case PartyListMenu.ResultKind.CHANGE_EQUIP:
                {
                    PartyOrganizationListViewItem item2 = this.partyItem.GetMember(m);
                    this.changePartyMode = PartyOrganizationChangeMenu.Mode.SELECT_EQUIP;
                    if (!item2.IsFollower)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        this.partyOrganizationChangeMenu.Init();
                        this.menuMode = MenuMode.ORGANIZATION_CHANGE;
                        base.myFSM.SendEvent("CLICK_CHANGE");
                        return;
                    }
                    if (this.setupKind != PartyListViewItem.SetupKind.BATTLE_SETUP)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        this.RecoverInputShowMenu();
                        return;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    base.myFSM.SendEvent("CLICK_FOLLOWER");
                    return;
                }
                case PartyListMenu.ResultKind.DETAIL_SERVANT:
                {
                    PartyOrganizationListViewItem item3 = this.partyItem.GetMember(m);
                    this.changePartyMode = PartyOrganizationChangeMenu.Mode.DETAIL_SERVANT;
                    if (!item3.IsFollower)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        this.partyOrganizationChangeMenu.Init();
                        this.menuMode = MenuMode.ORGANIZATION_CHANGE;
                        base.myFSM.SendEvent("CLICK_CHANGE");
                        return;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.RecoverInputShowMenu();
                    return;
                }
                default:
                    return;
            }
        }
    }

    public void Quit()
    {
        this.RefreshInfo();
        this.state = State.INIT;
    }

    protected void RecoverInputShowMenu()
    {
        switch (this.state)
        {
            case State.INPUT_QUEST_START:
                this.partyListMenu.Open(PartyListViewItem.MenuKind.QUEST_START, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
                break;

            case State.INPUT_SELECT_PARTY:
                this.partyListMenu.Open(PartyListViewItem.MenuKind.SELECT_PARTY, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, new PartyListMenu.CallbackFunc(this.OnSelectPartyList), null);
                break;

            case State.INPUT_ORGANIZATION_CHANGE:
                this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.NONE, this.basePartyItem, this.partyItem, -1, this.setupInfo, new PartyOrganizationChangeMenu.CallbackFunc(this.OnSelectedOrganizationChange), null);
                break;

            case State.INPUT_ORGANIZATION_SWAP:
                this.partyOrganizationMenu.Open(this.setupKind, this.basePartyItem, this.partyItem, this.setupInfo, new PartyOrganizationMenu.CallbackFunc(this.OnSelectedOrganizationSwap), null);
                break;
        }
    }

    public void RefreshInfo()
    {
    }

    public bool RequestBattleSetup()
    {
        ScriptManager.LoadBattleStartGameDemo(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, false, delegate (string demoInfo) {
            if (demoInfo != null)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                this.battleSetupInfo.battleBefore = true;
                this.battleSetupInfo.isBefore = false;
                this.battleSetupInfo.demoInfo = demoInfo;
                Debug.Log(string.Concat(new object[] { "RequestBattleSetup: exists BattleDemo:", this.battleSetupInfo.questId, ":", this.battleSetupInfo.questPhase, ":after" }));
                base.myFSM.SendEvent("GO_BATTLEDEMO");
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_TIP);
                BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
                this.battleSetupInfo = null;
                NetworkManager.getRequest<BattleSetupRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestBattleSetup)).beginRequest(battleSetupInfo.questId, battleSetupInfo.questPhase, battleSetupInfo.deckId, battleSetupInfo.followerId, battleSetupInfo.followerClassId, 1);
            }
        });
        return true;
    }

    public void RequestDeckName()
    {
        PartyListViewItem basePartyItem = this.basePartyItem;
        if ((basePartyItem != null) && !string.IsNullOrEmpty(this.editDeckName))
        {
            NetworkManager.getRequest<DeckEditNameRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestDeckName)).beginRequest(basePartyItem.DeckId, this.editDeckName);
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    public void RequestOrganizationChange()
    {
        PartyListViewItem item = this.partyOrganizationChangeMenu.GetItem();
        if (item != null)
        {
            UserDeckEntity userDeck = item.GetUserDeck();
            if (userDeck != null)
            {
                NetworkManager.getRequest<DeckSetupRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestOrganizationChange)).beginRequest(item.MainDeckId, item.DeckId, userDeck);
                return;
            }
        }
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestOrganizationSelect()
    {
        PartyListViewItem basePartyItem = this.basePartyItem;
        if (basePartyItem != null)
        {
            UserDeckEntity userDeck = basePartyItem.GetUserDeck();
            if (userDeck != null)
            {
                UserGameEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                this.activeDeckId = (entity2 == null) ? 0L : entity2.activeDeckId;
                if (basePartyItem.DeckId != this.activeDeckId)
                {
                    this.activeDeckId = basePartyItem.DeckId;
                    NetworkManager.getRequest<DeckSetupRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestOrganizationSelect)).beginRequest(basePartyItem.MainDeckId, basePartyItem.DeckId, userDeck);
                    return;
                }
            }
        }
        base.myFSM.SendEvent("REQUEST_OK");
    }

    public void RequestOrganizationSwap()
    {
        PartyListViewItem item = this.partyOrganizationMenu.GetItem();
        if (item != null)
        {
            UserDeckEntity userDeck = item.GetUserDeck();
            if (userDeck != null)
            {
                NetworkManager.getRequest<DeckSetupRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestOrganizationSwap)).beginRequest(item.MainDeckId, item.DeckId, userDeck);
                return;
            }
        }
        this.state = State.INIT_TOP;
        base.myFSM.SendEvent("REQUEST_NG");
    }

    protected void SelectedFriendOfferConfirm(bool isDecide)
    {
        base.myFSM.SendEvent(!isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    public void SelectOrganizationChange()
    {
        if (this.state == State.INIT_TOP)
        {
            this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_CHANGE);
            this.RefreshInfo();
            this.state = State.INIT_ORGANIZATION_CHANGE;
            this.partyOrganizationChangeMenu.Open(this.setupKind, PartyOrganizationChangeMenu.Mode.NONE, this.basePartyItem, this.partyItem, -1, this.setupInfo, null, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectOrganizationSwap()
    {
        if (this.state == State.INIT_TOP)
        {
            this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SWAP);
            this.RefreshInfo();
            this.state = State.INIT_ORGANIZATION_SWAP;
            this.partyOrganizationMenu.Open(this.setupKind, this.basePartyItem, this.partyItem, this.setupInfo, null, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectParty()
    {
        if (this.state == State.INIT_TOP)
        {
            this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.PARTY_ORGANIZATION);
            if (this.userDeckEntityList == null)
            {
                this.userDeckEntityList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getDeckList(NetworkManager.UserId);
                if (this.activeDeckId > 0L)
                {
                    this.selectPartyNum = 0;
                    for (int i = 0; i < this.userDeckEntityList.Length; i++)
                    {
                        if (this.userDeckEntityList[i].id == this.activeDeckId)
                        {
                            this.selectPartyNum = i;
                            break;
                        }
                    }
                    this.activeDeckId = 0L;
                }
            }
            this.state = State.INIT_SELECT_PARTY;
            this.RefreshInfo();
            this.partyListMenu.Open(PartyListViewItem.MenuKind.SELECT_PARTY, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, null, new System.Action(this.EndFadeSelectParty));
        }
    }

    public void SelectQuest()
    {
        if (this.state == State.INIT_TOP)
        {
            this.titleInfo.changeTitleInfo(TitleInfoControl.BackKind.BACK, TitleInfoControl.TitleKind.BATTLE_SETUP_CONFIRM);
            if (this.userDeckEntityList == null)
            {
                this.userDeckEntityList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getDeckList(NetworkManager.UserId);
                if (this.activeDeckId > 0L)
                {
                    this.selectPartyNum = 0;
                    for (int i = 0; i < this.userDeckEntityList.Length; i++)
                    {
                        if (this.userDeckEntityList[i].id == this.activeDeckId)
                        {
                            this.selectPartyNum = i;
                            break;
                        }
                    }
                    this.activeDeckId = 0L;
                }
            }
            this.state = State.INIT_QUEST_START;
            this.RefreshInfo();
            this.partyListMenu.Open(PartyListViewItem.MenuKind.QUEST_START, PartyListMenu.TutorialMode.NONE, this.userDeckEntityList, this.selectPartyNum, this.followerInfo, this.followerClassId, this.setupInfo, null, new System.Action(this.EndFadeSelectQuest));
        }
    }

    protected void SetCacheAssetNameList(string[] list)
    {
        string[] cacheAssetNameList = this.cacheAssetNameList;
        if (list != null)
        {
            AssetManager.loadAssetStorage(list, null);
        }
        this.cacheAssetNameList = list;
        if (cacheAssetNameList != null)
        {
            AssetManager.releaseAssetStorage(cacheAssetNameList);
        }
    }

    public bool ShowClassCompatibility()
    {
        if (this.basePartyItem != null)
        {
            this.showBackupState = this.state;
            this.state = State.INPUT_CLASS_COMPATIBILITY;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenClassCompatibilityMenu(new System.Action(this.EndShowClassCompatibilityMenu));
            return true;
        }
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool ShowCommandCard()
    {
        if (this.basePartyItem != null)
        {
            this.showBackupState = this.state;
            this.state = State.INPUT_COMMAD_CARD;
            this.operationCommandCardMenu.Open(PartyOrganizationCommandCardMenu.Kind.NORMAL, this.partyItem, new PartyOrganizationCommandCardMenu.CallbackFunc(this.EndShowCommandCard));
            return true;
        }
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool ShowEventPoint()
    {
        if (this.basePartyItem != null)
        {
            this.showBackupState = this.state;
            this.state = State.INPUT_EVENT_POINT;
            this.operationEventPointMenu.Open(this.setupInfo, this.partyItem, new PartyOrganizationEventPointMenu.CallbackFunc(this.EndShowEventPoint));
            return true;
        }
        this.RecoverInputShowMenu();
        base.myFSM.SendEvent("MENU_CANCEL");
        return false;
    }

    public bool StartBattle()
    {
        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Battle, SceneManager.FadeType.BLACK, null);
        return true;
    }

    public bool StartBattleDemo()
    {
        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.BattleDemoScene, SceneManager.FadeType.BLACK, this.battleSetupInfo);
        return true;
    }

    public bool StartScriptAction()
    {
        Debug.Log(string.Concat(new object[] { "StartScriptAction warId ", this.battleSetupInfo.warId, " questId ", this.battleSetupInfo.questId, " questPhase ", this.battleSetupInfo.questPhase, " deckId ", this.battleSetupInfo.deckId, " followerId ", this.battleSetupInfo.followerId, " isQuestNew ", this.battleSetupInfo.isQuestNew }));
        if (this.battleSetupInfo.isQuestNew)
        {
            SingletonTemplate<clsQuestCheck>.Instance.PlayQuestStartAction(delegate {
                this.EndQuestStartScriptAction();
            });
        }
        else
        {
            this.EndQuestStartScriptAction();
        }
        return true;
    }

    [CompilerGenerated]
    private sealed class <EndInputDeckNameMenu>c__AnonStoreyA4
    {
        internal PartyOrganizationRootComponent <>f__this;
        internal bool isDecide;

        internal void <>m__175()
        {
            this.<>f__this.state = this.<>f__this.showBackupState;
            this.<>f__this.RecoverInputShowMenu();
            this.<>f__this.myFSM.SendEvent(!this.isDecide ? "MENU_CANCEL" : "MENU_DECIDE");
        }
    }

    protected enum MenuMode
    {
        QUEST_START,
        SELECT_PARTY,
        ORGANIZATION_CHANGE,
        ORGANIZATION_SWAP
    }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INIT_QUEST_START,
        INIT_QUEST_START2,
        INPUT_QUEST_START,
        QUIT_QUEST_START,
        QUIT_QUEST_START2,
        INIT_SELECT_PARTY,
        INIT_SELECT_PARTY2,
        INPUT_SELECT_PARTY,
        QUIT_SELECT_PARTY,
        QUIT_SELECT_PARTY2,
        INIT_ORGANIZATION_CHANGE,
        INIT_ORGANIZATION_CHANGE2,
        INPUT_ORGANIZATION_CHANGE,
        QUIT_ORGANIZATION_CHANGE,
        QUIT_ORGANIZATION_CHANGE2,
        INIT_ORGANIZATION_SWAP,
        INIT_ORGANIZATION_SWAP2,
        INPUT_ORGANIZATION_SWAP,
        QUIT_ORGANIZATION_SWAP,
        QUIT_ORGANIZATION_SWAP2,
        INPUT_ORGANIZATION_CHANGE_SERVANT_SELECT,
        INPUT_ORGANIZATION_SWAP_SERVANT_SELECT,
        INPUT_ORGANIZATION_CHANGE_EQUIP_SELECT,
        INPUT_ORGANIZATION_SWAP_EQUIP_SELECT,
        INPUT_CLASS_COMPATIBILITY,
        INPUT_COMMAD_CARD,
        INPUT_EVENT_POINT,
        INPUT_CHARA_GRAPH,
        INPUT_DECK_NAME,
        QUIT_SCENE,
        MASTER_FORMATION_SCENE
    }

    protected enum TutorialMode
    {
        NONE,
        ORGANIZATION_GUIDE,
        FRIEND,
        NPC
    }

    protected enum TutorialState
    {
        NONE,
        INIT,
        SELECT_PARTY,
        SELECT_MEMBER,
        SELECT_SERVANT,
        SELECT_CHANGE_DECIDE,
        SELECT_RETURN,
        SELECT_START
    }
}

