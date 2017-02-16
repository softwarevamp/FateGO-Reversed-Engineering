using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FollowerRootComponent : SceneRootComponent
{
    protected BattleSetupInfo battleSetupInfo;
    [SerializeField]
    protected ServantClassIconComponent[] classIconList;
    [SerializeField]
    protected GameObject classInfoBase;
    [SerializeField]
    protected UILabel infomationLabel;
    [SerializeField]
    protected GameObject levelInfoBase;
    [SerializeField]
    protected UILabel levelInfoLabel;
    [SerializeField]
    protected FollowerSelectItemListViewManager operationItemListViewManager;
    protected string selectFriendCode;
    protected int selectItemNum;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected GameObject tutorialMaskBase;
    [SerializeField]
    protected GameObject tutorialMaskBase2;
    protected TutorialMode tutorialMode;
    protected TutorialState tutorialState;

    public override void beginFinish()
    {
        this.state = State.INIT;
        this.operationItemListViewManager.DestroyList();
        this.operationItemListViewManager.gameObject.SetActive(false);
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume(object data)
    {
        if (this.state == State.SUPPORT_SHOW_SCENE)
        {
            SupportInfoJump jump = data as SupportInfoJump;
            if (((jump != null) && (jump.SelectClassId >= 0)) && (this.selectItemNum >= 0))
            {
                FollowerSelectItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
                if (item != null)
                {
                    this.state = State.BATTLE_SETUP_SCENE;
                    this.battleSetupInfo.followerId = item.FollowerId;
                    this.battleSetupInfo.followerClassId = jump.SelectClassId;
                    if ((this.operationItemListViewManager.GetClassId() != this.battleSetupInfo.followerClassId) && this.operationItemListViewManager.ChangeClass(this.battleSetupInfo.followerClassId))
                    {
                        this.operationItemListViewManager.JumpItem(this.selectItemNum);
                    }
                    BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
                    SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.PartyOrganization, SceneManager.FadeType.BLACK, battleSetupInfo);
                    return;
                }
            }
        }
        else
        {
            this.battleSetupInfo = data as BattleSetupInfo;
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
            {
                if (this.battleSetupInfo.isChildFollower)
                {
                    this.ReturnBattleSetupMenu();
                }
                else
                {
                    this.ReturnQuestMenu();
                }
                return;
            }
        }
        switch (this.tutorialMode)
        {
            case TutorialMode.SELECT_NONE:
                if (!this.battleSetupInfo.isChildFollower)
                {
                    this.ReturnQuestMenu();
                }
                else
                {
                    this.ReturnBattleSetupMenu();
                }
                break;

            case TutorialMode.NPC:
                this.tutorialMode = TutorialMode.NPC;
                this.tutorialState = TutorialState.INIT;
                this.selectItemNum = -1;
                this.SearchDeckSvtEquipFriendPointUp();
                this.operationItemListViewManager.CreateList(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, this.friendPointUpVal, this.battleSetupInfo.eventUpValSetupInfo);
                this.state = State.INIT_SHOW_FOLLOWER;
                this.OnMoveEnd();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
                base.sendMessageResume();
                break;

            default:
                this.state = State.INPUT_SHOW_FOLLOWER;
                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
                base.sendMessageResume();
                break;
        }
        this.SearchDeckSvtEquipFriendPointUp();
        base.beginResume();
    }

    public override void beginStartUp(object data)
    {
        this.battleSetupInfo = data as BattleSetupInfo;
        if (this.battleSetupInfo == null)
        {
            this.battleSetupInfo = new BattleSetupInfo();
            this.battleSetupInfo.warId = 100;
            this.battleSetupInfo.questId = 0xf4241;
            this.battleSetupInfo.questPhase = 1;
            this.battleSetupInfo.deckId = 1L;
            this.battleSetupInfo.followerId = 0L;
            this.battleSetupInfo.followerClassId = 0;
            this.battleSetupInfo.isQuestNew = true;
        }
        this.titleInfo.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.BACK, TitleInfoControl.TitleKind.FOLLOWER);
        this.operationItemListViewManager.DestroyList();
        this.RefreshInfo();
        this.infomationLabel.text = LocalizationManager.Get("FOLLOWER_SELECT_EXPLANATION");
        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(this.battleSetupInfo.questId);
        if ((entity != null) && (entity.recommendLv > 0))
        {
            this.levelInfoBase.SetActive(true);
            this.levelInfoLabel.text = string.Empty + entity.recommendLv;
        }
        else
        {
            this.levelInfoBase.SetActive(false);
        }
        QuestPhaseEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase);
        if (((entity2 != null) && (entity2.classIds != null)) && (entity2.classIds.Length > 0))
        {
            this.classInfoBase.SetActive(true);
            for (int i = 0; i < this.classIconList.Length; i++)
            {
                ServantClassIconComponent component = this.classIconList[i];
                if ((entity2.classIds.Length > i) && (entity2.classIds[i] > 0))
                {
                    component.Set(entity2.classIds[i]);
                }
                else
                {
                    component.Clear();
                }
            }
        }
        else
        {
            this.classInfoBase.SetActive(false);
        }
        this.tutorialMode = TutorialMode.NONE;
        this.tutorialState = TutorialState.NONE;
        this.selectItemNum = -1;
        if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
            {
                this.StartBattleSetupMenu();
                return;
            }
            this.tutorialMode = TutorialMode.FRIEND;
            this.tutorialState = TutorialState.INIT;
            this.infomationLabel.text = string.Empty;
        }
        else
        {
            ConstantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ConstantMaster>(DataNameKind.Kind.CONSTANT);
            int num2 = master.GetValue("TUTORIAL_SUPPORT_QUEST_ID");
            if (this.battleSetupInfo.questId == num2)
            {
                int num3 = master.GetValue("TUTORIAL_SUPPORT_QUEST_PHASE");
                if (this.battleSetupInfo.questPhase == num3)
                {
                    this.tutorialMode = TutorialMode.NPC;
                    this.tutorialState = TutorialState.INIT;
                }
            }
        }
        if (((entity2 != null) && entity2.isNpcOnly) && (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcFollowerMaster>(DataNameKind.Kind.NPC_FOLLOWER).GetQuestFollowerList(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase).Length == 0))
        {
            this.tutorialMode = TutorialMode.SELECT_NONE;
            this.tutorialState = TutorialState.INIT;
            this.StartBattleSetupMenu();
        }
        else
        {
            this.SearchDeckSvtEquipFriendPointUp();
            base.beginStartUp();
        }
    }

    protected void CallbackFollowerList(string result)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected void EndCloseShowServant()
    {
        this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
    }

    protected void EndCloseTutorialArrow()
    {
        this.tutorialMaskBase2.SetActive(false);
        this.state = State.INPUT_SHOW_FOLLOWER;
        base.myFSM.SendEvent("MENU_SELECT_ITEM");
    }

    protected void EndOpenTutorialArrow()
    {
        this.state = State.INPUT_SHOW_FOLLOWER;
        this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.NPC_GUIDE_FIRST_SELECT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
    }

    protected void EndShowServant(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    protected void EndTutorialFollowerGuideNotification1()
    {
        this.state = State.INPUT_SHOW_FOLLOWER;
        this.titleInfo.setBackBtnEnable(false);
        this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
    }

    protected void EndTutorialFollowerGuideNotification2()
    {
        this.tutorialMaskBase2.SetActive(true);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(-20f, 100f), (float) 0f, new Rect(-500f, -14f, 1000f, 150f), new System.Action(this.EndOpenTutorialArrow));
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.operationItemListViewManager.gameObject.SetActive(true);
            this.operationItemListViewManager.CreateList(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, this.friendPointUpVal, this.battleSetupInfo.eventUpValSetupInfo);
            if (this.operationItemListViewManager.ItemSum > 0)
            {
                this.state = State.INIT_SHOW_FOLLOWER;
                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            }
            else
            {
                this.selectItemNum = -1;
                base.myFSM.SendEvent("MENU_SELECT_ITEM");
            }
        }
    }

    public void OnClickBack()
    {
        if ((this.state == State.INPUT_SHOW_FOLLOWER) && (this.tutorialState == TutorialState.NONE))
        {
            if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.state = State.QUIT_SHOW_FOLLOWER;
                if (this.battleSetupInfo.isChildFollower)
                {
                    base.myFSM.SendEvent("CLICK_BACK_BATTLE_SETUP");
                }
                else
                {
                    base.myFSM.SendEvent("CLICK_BACK");
                }
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    private void OnMoveEnd()
    {
        if (this.state == State.INIT_SHOW_FOLLOWER)
        {
            this.RefreshInfo();
            if (this.tutorialState != TutorialState.INIT)
            {
                this.state = State.INPUT_SHOW_FOLLOWER;
                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
            }
            else
            {
                this.state = State.INIT_SHOW_FOLLOWER;
                this.tutorialState = TutorialState.SELECT;
                switch (this.tutorialMode)
                {
                    case TutorialMode.FRIEND:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FOLLOWER1"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialFollowerGuideNotification1));
                        break;

                    case TutorialMode.NPC:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FOLLOWER2"), TutorialFlag.Id.NULL, new System.Action(this.EndTutorialFollowerGuideNotification2));
                        break;
                }
            }
        }
    }

    protected void OnSelectFollowerItem(FollowerSelectItemListViewManager.ResultKind kind, int n)
    {
        if (this.state == State.INPUT_SHOW_FOLLOWER)
        {
            this.selectItemNum = n;
            switch (kind)
            {
                case FollowerSelectItemListViewManager.ResultKind.SERVANT_STATUS:
                {
                    if (this.tutorialState != TutorialState.NONE)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        switch (this.tutorialMode)
                        {
                            case TutorialMode.FRIEND:
                                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                                break;

                            case TutorialMode.NPC:
                                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.NPC_GUIDE_FIRST_SELECT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                                break;
                        }
                        return;
                    }
                    FollowerSelectItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
                    if ((item.Type != Follower.Type.NPC) && (item.SvtId > 0))
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        this.SelectShowServant(item);
                        return;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                    return;
                }
                case FollowerSelectItemListViewManager.ResultKind.SERVANT_SKILL1_STATUS:
                case FollowerSelectItemListViewManager.ResultKind.SERVANT_SKILL2_STATUS:
                case FollowerSelectItemListViewManager.ResultKind.SERVANT_SKILL3_STATUS:
                {
                    int[] numArray;
                    int[] numArray2;
                    int[] numArray3;
                    string[] strArray;
                    string[] strArray2;
                    string str;
                    string str2;
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    FollowerSelectItemListViewItem item2 = this.operationItemListViewManager.GetItem(this.selectItemNum);
                    int index = 0;
                    if (kind != FollowerSelectItemListViewManager.ResultKind.SERVANT_SKILL2_STATUS)
                    {
                        if (kind == FollowerSelectItemListViewManager.ResultKind.SERVANT_SKILL3_STATUS)
                        {
                            index = 2;
                        }
                    }
                    else
                    {
                        index = 1;
                    }
                    item2.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                    SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(numArray[index]);
                    SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(numArray[index], numArray2[index]);
                    entity.getSkillMessageInfo(out str, out str2, numArray2[index]);
                    str = str + " " + string.Format(LocalizationManager.Get("MASTER_EQSKILL_LV_TXT"), numArray2[index]);
                    string info = string.Format(LocalizationManager.Get("BATTLE_SKILLCHARGETURN"), entity2.chargeTurn);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailLongInfoDialog(str, info, str2);
                    this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                    return;
                }
                case FollowerSelectItemListViewManager.ResultKind.SUPPORT_INFO:
                    if (this.tutorialState != TutorialState.NONE)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                        switch (this.tutorialMode)
                        {
                            case TutorialMode.FRIEND:
                                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                                break;

                            case TutorialMode.NPC:
                                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.NPC_GUIDE_FIRST_SELECT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                                return;
                        }
                        return;
                    }
                    if (this.operationItemListViewManager.GetItem(this.selectItemNum).Type != Follower.Type.NPC)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        base.myFSM.SendEvent("MENU_SHOW_SUPPORT");
                        return;
                    }
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
                    return;
            }
            if (this.operationItemListViewManager.GetItem(this.selectItemNum).SvtId <= 0)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.operationItemListViewManager.SetMode(FollowerSelectItemListViewManager.InitMode.INPUT, new FollowerSelectItemListViewManager.CallbackFunc(this.OnSelectFollowerItem));
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                if (this.tutorialState == TutorialState.NONE)
                {
                    base.myFSM.SendEvent("MENU_SELECT_ITEM");
                }
                else
                {
                    switch (this.tutorialMode)
                    {
                        case TutorialMode.FRIEND:
                            this.titleInfo.setBackBtnEnable(true);
                            base.myFSM.SendEvent("MENU_SELECT_ITEM");
                            break;

                        case TutorialMode.NPC:
                            this.state = State.QUIT_SHOW_FOLLOWER;
                            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.EndCloseTutorialArrow));
                            break;
                    }
                }
            }
        }
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
    }

    public void RequestFollowerList()
    {
        FollowerListRequest request = NetworkManager.getRequest<FollowerListRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackFollowerList));
        if (request.checkExpirationDate())
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            request.addActionField("followerlist");
            request.beginRequest();
        }
    }

    public bool ReturnBattleSetupMenu()
    {
        this.state = State.BATTLE_SETUP_SCENE;
        BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.PartyOrganization, SceneManager.FadeType.BLACK, battleSetupInfo);
        return true;
    }

    public bool ReturnQuestMenu()
    {
        BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
        this.battleSetupInfo = null;
        if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
        {
            this.state = State.INIT;
            SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, battleSetupInfo);
        }
        return true;
    }

    private void SearchDeckSvtEquipFriendPointUp()
    {
        UserDeckEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getEntityFromId(this.battleSetupInfo.deckId);
        this.friendPointUpVal = 0;
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            UserServantEntity userServant = entity.GetUserServant(i);
            if (userServant != null)
            {
                int num2 = userServant.getFriendPointUpVal(entity.GetEquipList(i));
                if (num2 > this.friendPointUpVal)
                {
                    this.friendPointUpVal = num2;
                }
            }
        }
    }

    public void SelectShowServant(FollowerSelectItemListViewItem item)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, item.ServantLeader, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
    }

    public bool StartBattleSetupMenu()
    {
        this.battleSetupInfo.followerId = 0L;
        if (this.selectItemNum >= 0)
        {
            FollowerSelectItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            if (item != null)
            {
                this.battleSetupInfo.followerId = item.FollowerId;
                this.battleSetupInfo.followerClassId = item.SelectClassId;
            }
        }
        this.state = State.BATTLE_SETUP_SCENE;
        BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.PartyOrganization, SceneManager.FadeType.BLACK, battleSetupInfo);
        return true;
    }

    public bool StartSupportInfoMenu()
    {
        if (this.selectItemNum >= 0)
        {
            FollowerSelectItemListViewItem item = this.operationItemListViewManager.GetItem(this.selectItemNum);
            if (item != null)
            {
                UserFollowerEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_FOLLOWER).getSingleEntity<UserFollowerEntity>();
                if (entity != null)
                {
                    FollowerInfo[] followerInfo = entity.followerInfo;
                    for (int i = 0; i < followerInfo.Length; i++)
                    {
                        if (followerInfo[i].userId == item.FollowerId)
                        {
                            this.state = State.SUPPORT_SHOW_SCENE;
                            SupportInfoJump data = new SupportInfoJump(followerInfo[i], FriendStatus.Kind.SEARCH, true);
                            SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.SupportSelect, SceneManager.FadeType.BLACK, data);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public int friendPointUpVal { get; private set; }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INIT_SHOW_FOLLOWER,
        INIT_SHOW_FOLLOWER2,
        INPUT_SHOW_FOLLOWER,
        QUIT_SHOW_FOLLOWER,
        QUIT_SHOW_FOLLOWER2,
        BATTLE_SETUP_SCENE,
        SUPPORT_SHOW_SCENE
    }

    protected enum TutorialMode
    {
        NONE,
        SELECT_NONE,
        FRIEND,
        NPC
    }

    protected enum TutorialState
    {
        NONE,
        INIT,
        SELECT
    }
}

