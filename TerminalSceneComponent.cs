using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TerminalSceneComponent : SceneRootComponent
{
    public const int LOGIN_BONUS_WAITE_BG_PANEL_DEPTH = 0xc9;
    [SerializeField]
    private GameObject mDebugWindowPrefab;
    [SerializeField]
    private EarthCore mEarthCore;
    private CStateManager<TerminalSceneComponent> mFSM;
    protected static TerminalSceneComponent mInstance;
    private bool mIsStart_LoginBonusWhiteBg;
    [SerializeField]
    private ScrPlayerStatus mPlayerStatus;
    private UserServantEntity[] mStandSvtDatas;
    private int mStandSvtIdx;
    [SerializeField]
    private UIAtlas mTerminalAtlas;
    [SerializeField]
    private ScrTerminalListTop mTerminalList;
    [SerializeField]
    private ScrTerminalMap mTerminalMap;
    [SerializeField]
    private StandFigureSlideComponent mTerminalServant;
    [SerializeField]
    private TitleInfoControl mTitleInfo;
    private TUTORIAL_KIND mTutorialKind;
    [SerializeField]
    private Camera mUICamera;
    public readonly Vector2 TUTORIAL_GACHA_ARROW_POS = new Vector2(-80f, -160f);
    public readonly Rect TUTORIAL_GACHA_ARROW_RECT = new Rect(-155f, -310f, 150f, 150f);
    public const float TUTORIAL_GACHA_ARROW_WAY = 0f;
    public readonly Vector2 TUTORIAL_MENU_ARROW_POS = new Vector2(450f, -230f);
    public readonly Rect TUTORIAL_MENU_ARROW_RECT = new Rect(320f, -315f, 200f, 100f);
    public const float TUTORIAL_MENU_ARROW_WAY = 0f;
    public readonly Vector2 TUTORIAL_MENU_MESSAGE_POS = new Vector2(0f, 20f);
    public readonly Vector2 TUTORIAL_QUEST_ARROW_POS = new Vector2(0f, 140f);
    public readonly Rect TUTORIAL_QUEST_ARROW_RECT = new Rect(-30f, 65f, 550f, 150f);
    public const float TUTORIAL_QUEST_ARROW_WAY = 90f;
    public readonly Vector2 TUTORIAL_QUEST_MESSAGE_POS = new Vector2(0f, -40f);
    public readonly Vector2 TUTORIAL_SPOT_ARROW_POS = new Vector2(-70f, 0f);
    public readonly Rect TUTORIAL_SPOT_ARROW_RECT = new Rect(-100f, -140f, 200f, 300f);
    public const float TUTORIAL_SPOT_ARROW_WAY = 90f;
    public readonly Vector2 TUTORIAL_SPOT_MESSAGE_POS = new Vector2(0f, 180f);

    private void Awake()
    {
        mInstance = this;
    }

    public override void beginFinish()
    {
        TerminalPramsManager.Save_SaveData();
        Input.multiTouchEnabled = false;
    }

    public override void beginInitialize()
    {
        TerminalPramsManager.Load_SaveData();
        TerminalPramsManager.mfSetSceneStatus(TerminalPramsManager.enSceneStatus.enInitialize);
        base.beginInitialize();
        this.IsReq_InitEarthRotateY = true;
        CTouch.init();
        CTouch.setScreenCamera(this.mUICamera);
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<TerminalSceneComponent>(this, 9);
            this.mFSM.add(0, new StateNormal());
            this.mFSM.add(1, new StateTutorial1_SpotArrow());
            this.mFSM.add(2, new StateTutorial1_QuestArrow());
            this.mFSM.add(3, new StateTutorial2_SpotArrow());
            this.mFSM.add(4, new StateTutorial2_QuestArrow());
            this.mFSM.add(5, new StateTutorial3_MenuArrow());
            this.mFSM.add(6, new StateTutorial3_GachaArrow());
            this.mFSM.add(7, new StateTutorial4_SpotArrow());
            this.mFSM.add(8, new StateTutorial4_QuestArrow());
            this.SetState(STATE.NORMAL);
        }
        this.mTitleInfo.setTitleInfo(null, true, string.Empty, TitleInfoControl.TitleKind.TERMINAL);
        this.mTitleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.TERMINAL);
        this.mTitleInfo.setBackBtnDepth(0x1d);
        this.mTerminalMap.mMapCamera.Init();
        this.mPlayerStatus.SetCloseGiftAct(delegate {
            this.mTitleInfo.InitEventAlphaAnim();
            this.mTitleInfo.UpdateEventItem();
        });
        this.mPlayerStatus.SetApRecoverAct(delegate {
            this.mPlayerStatus.mfInitUserData();
            this.mTerminalList.GetQuestBoardListViewManager().SetupDisp();
        });
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginPause()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
        TerminalPramsManager.Save_SaveData();
        Input.multiTouchEnabled = false;
    }

    public override void beginResume()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        this.IsReq_InitEarthRotateY = false;
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        TerminalPramsManager.mfSetSceneStatus(TerminalPramsManager.enSceneStatus.enResume);
        base.beginResume();
    }

    public override void beginStartUp()
    {
        this.beginStartUp(null);
    }

    public override void beginStartUp(object data)
    {
        this.TransitionInfo = data as TerminalTransitionInfo;
        RenderSettings.ambientLight = new Color(0.2f, 0.2588235f, 0.3176471f, 1f);
        base.setMainMenuBar(MainMenuBar.Kind.TERMINAL, 30);
        MainMenuBar.setMenuActive(true, this.mUICamera);
        AccountingManager.SetEnableStore(true);
        if (!TerminalPramsManager.IsDispDone_UIStandFigure)
        {
            TerminalPramsManager.IsDispDone_UIStandFigure = true;
            TerminalPramsManager.IsDispUIStandFigure = true;
        }
        this.mStandSvtDatas = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).GetUserServantListFromDeck();
        UserServantEntity usd = this.mStandSvtDatas[this.mStandSvtIdx];
        if (usd == null)
        {
            usd = this.GetNextStandServant();
        }
        this.SetupStandServant(usd, false, () => this.loadBanner());
    }

    private void callbackTopHome(string result)
    {
        MainMenuBar.UpdateNoticeNumber();
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected void callbackTopLogin(string result)
    {
        TopHomeRequest request = NetworkManager.getRequest<TopHomeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopHome));
        if (request.checkExpirationDate())
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            request.addDefaultField("home");
            request.beginRequest();
        }
    }

    public void CheckRewardPopupChain(System.Action friendPointClose)
    {
        <CheckRewardPopupChain>c__AnonStoreyDD ydd = new <CheckRewardPopupChain>c__AnonStoreyDD {
            friendPointClose = friendPointClose,
            <>f__this = this
        };
        this.mTerminalList.StartShowWarClearReward(new System.Action(ydd.<>m__224));
    }

    public bool CheckTutorial()
    {
        bool flag = false;
        this.mTutorialKind = TUTORIAL_KIND.NONE;
        if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            this.mIsStart_LoginBonusWhiteBg = !TerminalPramsManager.IsAutoResume;
            TerminalPramsManager.IsAutoResume = true;
            TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Map;
            TerminalPramsManager.WarId = ConstantMaster.getValue("FIRST_WAR_ID");
            TerminalPramsManager.SpotId = -1;
            this.mTitleInfo.setBackBtnColliderEnable(false);
            MainMenuBar.SetMenuBtnColliderEnable(false);
            this.mPlayerStatus.SetGiftBtnColliderEnable(false);
            this.mPlayerStatus.SetApRecoverBtnEnable(false);
            clsQuestCheck instance = SingletonTemplate<clsQuestCheck>.Instance;
            this.mTutorialKind = TUTORIAL_KIND._4;
            if (!instance.IsQuestClear(ConstantMaster.getValue("TUTORIAL_QUEST_ID1"), false))
            {
                flag = true;
                TerminalPramsManager.QuestId = ConstantMaster.getValue("TUTORIAL_QUEST_ID1");
                this.mTerminalList.StartFollower();
                return flag;
            }
            if (!instance.IsQuestClear(ConstantMaster.getValue("TUTORIAL_QUEST_ID2"), false))
            {
                this.mTutorialKind = TUTORIAL_KIND._1;
                return flag;
            }
            if (!instance.IsQuestClear(ConstantMaster.getValue("TUTORIAL_QUEST_ID3"), false))
            {
                this.mTutorialKind = TUTORIAL_KIND._2;
                return flag;
            }
            if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._1))
            {
                this.mTutorialKind = TUTORIAL_KIND._3;
                return flag;
            }
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA))
            {
                flag = true;
                TerminalPramsManager.SummonType = 1;
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Summon, SceneManager.FadeType.BLACK, null);
                return flag;
            }
            if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._2))
            {
                flag = true;
                TerminalPramsManager.SummonType = 1;
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Summon, SceneManager.FadeType.BLACK, null);
                return flag;
            }
            if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
            {
                flag = true;
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.PartyOrganization, SceneManager.FadeType.BLACK, null);
            }
        }
        return flag;
    }

    public void Fadein_MapDisp(float fade_time, System.Action end_act = null)
    {
        <Fadein_MapDisp>c__AnonStoreyDA yda = new <Fadein_MapDisp>c__AnonStoreyDA {
            end_act = end_act,
            <>f__this = this
        };
        int warId = TerminalPramsManager.WarId;
        WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(warId);
        this.mTerminalMap.SetDisp(true);
        this.mTitleInfo.SetActiveEventItem(entity);
        this.mTitleInfo.FrameIn(false);
        MainMenuBar.FrameIn(false);
        this.mPlayerStatus.FrameIn();
        int id = int.Parse(entity.bgmId);
        SoundManager.playBgm(SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BGM).getEntityFromId<BgmEntity>(id).fileName);
        System.Action action = new System.Action(yda.<>m__21F);
        if (this.mIsStart_LoginBonusWhiteBg)
        {
            this.mIsStart_LoginBonusWhiteBg = false;
            SingletonMonoBehaviour<CommonUI>.Instance.SetupLoginResultData();
            SingletonMonoBehaviour<CommonUI>.Instance.StartLoginAndCampaignBonus(new System.Action(yda.<>m__220), null, 0xc9);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(yda.<>m__221));
        }
    }

    public void Fadein_WorldDisp(System.Action end_act = null)
    {
        this.Fadein_WorldDisp(SceneManager.DEFAULT_FADE_TIME, end_act);
    }

    public void Fadein_WorldDisp(float fade_time, System.Action end_act = null)
    {
        this.mTerminalMap.SetDisp(false);
        if (this.IsReq_InitEarthRotateY)
        {
            this.IsReq_InitEarthRotateY = false;
            this.mEarthCore.SetRotateY_PointInitAngle();
        }
        this.mTitleInfo.SetActiveEventItem(true, false);
        this.mTerminalMap.ReleaseMap();
        SoundManager.playBgm("BGM_CHALDEA_1");
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(fade_time, end_act);
    }

    private UserServantEntity GetNextStandServant()
    {
        UserServantEntity entity = null;
        for (int i = 0; i < this.mStandSvtDatas.Length; i++)
        {
            this.mStandSvtIdx++;
            if (this.mStandSvtIdx >= this.mStandSvtDatas.Length)
            {
                this.mStandSvtIdx = 0;
            }
            entity = this.mStandSvtDatas[this.mStandSvtIdx];
            if (entity != null)
            {
                return entity;
            }
        }
        return entity;
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    protected void loadBanner()
    {
        AtlasManager.LoadBanner(() => base.sendMessageStartUp());
    }

    public void mcbfCheckSceneStatus()
    {
        if (TerminalPramsManager.mfGetSceneStatus() != TerminalPramsManager.enSceneStatus.enResume)
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_INIT");
        }
        else
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_RESUME");
        }
    }

    private void mfCallFsmEvent(string sEventStr)
    {
        base.myFSM.SendEvent(sEventStr);
    }

    private void mfsmfInitTable()
    {
        SingletonTemplate<QuestTree>.Instance.Init();
    }

    public void PlayChapterStart(System.Action end_act)
    {
        <PlayChapterStart>c__AnonStoreyDB ydb = new <PlayChapterStart>c__AnonStoreyDB {
            end_act = end_act,
            <>f__this = this
        };
        if (this.mTutorialKind == TUTORIAL_KIND._1)
        {
            this.IsWarStartAnim = true;
        }
        if (TerminalPramsManager.Debug_IsWarStartActionSkip)
        {
            this.IsWarStartAnim = false;
        }
        if (this.IsWarStartAnim && !TerminalPramsManager.IsWarStartedId(TerminalPramsManager.WarId))
        {
            ScriptManager.PlayChapterStart(TerminalPramsManager.WarId, new ScriptManager.CallbackFunc(ydb.<>m__222), false);
        }
        else
        {
            ydb.end_act.Call();
        }
    }

    public void PlayTutorial()
    {
        switch (this.mTutorialKind)
        {
            case TUTORIAL_KIND._1:
                this.SetState(STATE.TUTORIAL1_SPOT_ARROW);
                break;

            case TUTORIAL_KIND._2:
                this.SetState(STATE.TUTORIAL2_SPOT_ARROW);
                break;

            case TUTORIAL_KIND._3:
                this.SetState(STATE.TUTORIAL3_MENU_ARROW);
                break;

            case TUTORIAL_KIND._4:
                this.SetState(STATE.TUTORIAL4_SPOT_ARROW);
                break;
        }
    }

    public void requestTopHome()
    {
        if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            this.callbackTopLogin(string.Empty);
        }
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    private void SetupStandServant(UserServantEntity usd, bool is_slide_in, System.Action end_act = null)
    {
        <SetupStandServant>c__AnonStoreyD8 yd = new <SetupStandServant>c__AnonStoreyD8 {
            is_slide_in = is_slide_in,
            end_act = end_act,
            <>f__this = this
        };
        this.mTerminalServant.Setup(usd, 0, new System.Action(yd.<>m__21D));
        this.mTerminalServant.SetBtnAct(new System.Action(yd.<>m__21E));
    }

    private void StartMashuPowerUpAction(System.Action end_act)
    {
        <StartMashuPowerUpAction>c__AnonStoreyDC ydc = new <StartMashuPowerUpAction>c__AnonStoreyDC {
            end_act = end_act
        };
        if (TerminalPramsManager.WarId != ConstantMaster.getValue("MASHU_CHANGE_WAR_ID"))
        {
            ydc.end_act.Call();
        }
        else
        {
            ydc.tutorial_flag_id = TutorialFlag.Id.TUTORIAL_LABEL_MASHU_CHANGE;
            if (TutorialFlag.Get(ydc.tutorial_flag_id))
            {
                ydc.end_act.Call();
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenPowerUp(new CombineResultEffectComponent.ClickDelegate(ydc.<>m__223));
            }
        }
    }

    private void Update()
    {
        CTouch.process();
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
        string activeStateName = this.mTerminalMap.mfGetMyFsmP().ActiveStateName;
        bool flag = false;
        if (((!this.IsTutorialActive && (activeStateName.IndexOf("Map Touch Enable") >= 0)) && (!MainMenuBar.IsEnableOutSideCollider && !SingletonMonoBehaviour<CommonUI>.Instance.IsActive_UserPresentBoxWindow())) && ((!SingletonMonoBehaviour<CommonUI>.Instance.IsActive_ApRecvDlgComp() && !SingletonMonoBehaviour<QuestAfterAction>.Instance.IsPlaying()) && !SingletonTemplate<TerminalDebugWindow>.Instance.IsActive()))
        {
            flag = true;
        }
        this.mTerminalMap.mMapCamera.Process(flag);
    }

    public EarthCore EarthCore =>
        this.mEarthCore;

    public static TerminalSceneComponent Instance =>
        mInstance;

    public bool IsReq_InitEarthRotateY { get; set; }

    public bool IsTutorialActive
    {
        get => 
            (this.mTutorialKind != TUTORIAL_KIND.NONE);
        private set
        {
        }
    }

    public bool IsWarStartAnim { get; set; }

    public ScrPlayerStatus PlayerStatus =>
        this.mPlayerStatus;

    public ScrTerminalMap TerminalMap =>
        this.mTerminalMap;

    public StandFigureSlideComponent TerminalServant =>
        this.mTerminalServant;

    public TerminalTransitionInfo TransitionInfo { get; private set; }

    [CompilerGenerated]
    private sealed class <CheckRewardPopupChain>c__AnonStoreyDD
    {
        internal TerminalSceneComponent <>f__this;
        internal System.Action friendPointClose;

        internal void <>m__224()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetupLoginResultData();
            SingletonMonoBehaviour<CommonUI>.Instance.StartServantEventJoinLeaveNotification(() => SingletonMonoBehaviour<CommonUI>.Instance.StartLoginAndCampaignBonus(delegate {
                SingletonMonoBehaviour<CommonUI>.Instance.StartFriendPointNotification(this.friendPointClose);
                SingletonMonoBehaviour<CommonUI>.Instance.ClearLoginResultData();
            }, () => this.<>f__this.PlayerStatus.mfInitUserData(), -1));
        }

        internal void <>m__22A()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.StartLoginAndCampaignBonus(delegate {
                SingletonMonoBehaviour<CommonUI>.Instance.StartFriendPointNotification(this.friendPointClose);
                SingletonMonoBehaviour<CommonUI>.Instance.ClearLoginResultData();
            }, () => this.<>f__this.PlayerStatus.mfInitUserData(), -1);
        }

        internal void <>m__22B()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.StartFriendPointNotification(this.friendPointClose);
            SingletonMonoBehaviour<CommonUI>.Instance.ClearLoginResultData();
        }

        internal void <>m__22C()
        {
            this.<>f__this.PlayerStatus.mfInitUserData();
        }
    }

    [CompilerGenerated]
    private sealed class <Fadein_MapDisp>c__AnonStoreyDA
    {
        internal TerminalSceneComponent <>f__this;
        internal System.Action end_act;

        internal void <>m__21F()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, this.end_act);
        }

        internal void <>m__220()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => this.end_act.Call());
            SingletonMonoBehaviour<CommonUI>.Instance.ClearLoginResultData();
        }

        internal void <>m__221()
        {
            this.<>f__this.mTitleInfo.CheckSuperBossHpAnim(delegate {
                if (TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
                {
                    this.<>f__this.CheckRewardPopupChain(this.end_act);
                }
                else
                {
                    this.end_act.Call();
                }
            });
        }

        internal void <>m__226()
        {
            this.end_act.Call();
        }

        internal void <>m__227()
        {
            if (TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
            {
                this.<>f__this.CheckRewardPopupChain(this.end_act);
            }
            else
            {
                this.end_act.Call();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayChapterStart>c__AnonStoreyDB
    {
        internal TerminalSceneComponent <>f__this;
        internal System.Action end_act;

        internal void <>m__222(bool isExit)
        {
            this.<>f__this.StartMashuPowerUpAction(delegate {
                TerminalPramsManager.SetWarStartedId(TerminalPramsManager.WarId);
                this.end_act.Call();
            });
        }

        internal void <>m__228()
        {
            TerminalPramsManager.SetWarStartedId(TerminalPramsManager.WarId);
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <SetupStandServant>c__AnonStoreyD8
    {
        internal TerminalSceneComponent <>f__this;
        internal System.Action end_act;
        internal bool is_slide_in;

        internal void <>m__21D()
        {
            if (this.is_slide_in)
            {
                this.<>f__this.mTerminalServant.SlideIn(null);
            }
            this.end_act.Call();
        }

        internal void <>m__21E()
        {
            if (((!this.<>f__this.mTerminalServant.IsLoding() && !this.<>f__this.mTerminalServant.IsMoving()) && !this.<>f__this.mEarthCore.IsFocusMoving) && (Mathf.Abs(this.<>f__this.mEarthCore.GetStateMain().InertialSpdOld) <= 0f))
            {
                if (this.<>f__this.mTerminalServant.IsFrameIn() && !this.<>f__this.mEarthCore.IsFocusIn)
                {
                    <SetupStandServant>c__AnonStoreyD9 yd = new <SetupStandServant>c__AnonStoreyD9 {
                        <>f__ref$216 = this,
                        _usd = this.<>f__this.GetNextStandServant()
                    };
                    TerminalPramsManager.IsDispUIStandFigure = this.<>f__this.mStandSvtIdx != 0;
                    this.<>f__this.mEarthCore.FocusInOut(!TerminalPramsManager.IsDispUIStandFigure, false);
                    this.<>f__this.mTerminalServant.SlideOut(new System.Action(yd.<>m__225), false);
                    TerminalPramsManager.PlaySE_Decide();
                }
                else if (!this.<>f__this.mTerminalServant.IsFrameIn() && this.<>f__this.mEarthCore.IsFocusIn)
                {
                    TerminalPramsManager.IsDispUIStandFigure = true;
                    this.<>f__this.mEarthCore.FocusInOut(!TerminalPramsManager.IsDispUIStandFigure, false);
                    this.<>f__this.mTerminalServant.SlideIn(null);
                    TerminalPramsManager.PlaySE_Decide();
                }
            }
        }

        private sealed class <SetupStandServant>c__AnonStoreyD9
        {
            internal UserServantEntity _usd;
            internal TerminalSceneComponent.<SetupStandServant>c__AnonStoreyD8 <>f__ref$216;

            internal void <>m__225()
            {
                this.<>f__ref$216.<>f__this.SetupStandServant(this._usd, TerminalPramsManager.IsDispUIStandFigure, null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <StartMashuPowerUpAction>c__AnonStoreyDC
    {
        internal System.Action end_act;
        internal TutorialFlag.Id tutorial_flag_id;

        internal void <>m__223(bool is_decide)
        {
            NetworkManager.getRequest<TutorialSetRequest>(result => this.end_act.Call()).beginRequest(this.tutorial_flag_id);
        }

        internal void <>m__229(string result)
        {
            this.end_act.Call();
        }
    }

    public enum STATE
    {
        NORMAL,
        TUTORIAL1_SPOT_ARROW,
        TUTORIAL1_QUEST_ARROW,
        TUTORIAL2_SPOT_ARROW,
        TUTORIAL2_QUEST_ARROW,
        TUTORIAL3_MENU_ARROW,
        TUTORIAL3_GACHA_ARROW,
        TUTORIAL4_SPOT_ARROW,
        TUTORIAL4_QUEST_ARROW,
        SIZEOF
    }

    private class StateNormal : IState<TerminalSceneComponent>
    {
        public void begin(TerminalSceneComponent that)
        {
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }
    }

    private class StateTutorial1_QuestArrow : IState<TerminalSceneComponent>
    {
        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyDF ydf = new <begin>c__AnonStoreyDF {
                that = that
            };
            ydf.focus_quest = ydf.that.mTerminalList.GetFocusQuest();
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_QUEST1");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, ydf.that.TUTORIAL_QUEST_ARROW_POS, ydf.that.TUTORIAL_QUEST_ARROW_RECT, (float) 90f, ydf.that.TUTORIAL_QUEST_MESSAGE_POS, -1, new System.Action(ydf.<>m__230));
            ydf.that.mTerminalList.SetQuestClickAct(new System.Action(ydf.<>m__231));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyDF
        {
            internal QuestBoardListViewObject focus_quest;
            internal TerminalSceneComponent that;

            internal void <>m__230()
            {
                this.that.mTerminalList.GetQuestBoardListViewManager().GetScrollView().enabled = false;
                this.focus_quest.SetInput(true);
                this.focus_quest.gameObject.GetComponent<UIDragScrollView>().enabled = false;
            }

            internal void <>m__231()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(() => this.that.SetState(TerminalSceneComponent.STATE.NORMAL));
            }

            internal void <>m__232()
            {
                this.that.SetState(TerminalSceneComponent.STATE.NORMAL);
            }
        }
    }

    private class StateTutorial1_SpotArrow : IState<TerminalSceneComponent>
    {
        private bool mIsGoNext;

        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyDE yde = new <begin>c__AnonStoreyDE {
                <>f__this = this
            };
            this.mIsGoNext = false;
            yde.focus_spot = that.mTerminalMap.GetFocusSpot();
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_SPOT1");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, that.TUTORIAL_SPOT_ARROW_POS, that.TUTORIAL_SPOT_ARROW_RECT, (float) 90f, that.TUTORIAL_SPOT_MESSAGE_POS, -1, new System.Action(yde.<>m__22D));
            that.mTerminalMap.SetMapCamera_FocusSpot(yde.focus_spot, 0.4f, null);
            that.mTerminalMap.SetSpotClickAct(new System.Action(yde.<>m__22E));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
            if (this.mIsGoNext && (that.mTerminalList.GetQuestBoardListViewManager().GetInitMode() == QuestBoardListViewManager.InitMode.VALID))
            {
                that.SetState(TerminalSceneComponent.STATE.TUTORIAL1_QUEST_ARROW);
            }
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyDE
        {
            internal TerminalSceneComponent.StateTutorial1_SpotArrow <>f__this;
            internal SrcSpotBasePrefab focus_spot;

            internal void <>m__22D()
            {
                this.focus_spot.SetBtnColliderEnable(true);
            }

            internal void <>m__22E()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow((System.Action) (() => (this.<>f__this.mIsGoNext = true)));
            }

            internal void <>m__22F()
            {
                this.<>f__this.mIsGoNext = true;
            }
        }
    }

    private class StateTutorial2_QuestArrow : IState<TerminalSceneComponent>
    {
        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE1 ye = new <begin>c__AnonStoreyE1 {
                that = that
            };
            ye.focus_quest = ye.that.mTerminalList.GetFocusQuest();
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_QUEST2");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, ye.that.TUTORIAL_QUEST_ARROW_POS, ye.that.TUTORIAL_QUEST_ARROW_RECT, (float) 90f, ye.that.TUTORIAL_QUEST_MESSAGE_POS, -1, new System.Action(ye.<>m__236));
            ye.that.mTerminalList.SetQuestClickAct(new System.Action(ye.<>m__237));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE1
        {
            internal QuestBoardListViewObject focus_quest;
            internal TerminalSceneComponent that;

            internal void <>m__236()
            {
                this.that.mTerminalList.GetQuestBoardListViewManager().GetScrollView().enabled = false;
                this.focus_quest.SetInput(true);
                this.focus_quest.gameObject.GetComponent<UIDragScrollView>().enabled = false;
            }

            internal void <>m__237()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(() => this.that.SetState(TerminalSceneComponent.STATE.NORMAL));
            }

            internal void <>m__238()
            {
                this.that.SetState(TerminalSceneComponent.STATE.NORMAL);
            }
        }
    }

    private class StateTutorial2_SpotArrow : IState<TerminalSceneComponent>
    {
        private bool mIsGoNext;

        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE0 ye = new <begin>c__AnonStoreyE0 {
                <>f__this = this
            };
            this.mIsGoNext = false;
            ye.focus_spot = that.mTerminalMap.GetFocusSpot();
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_SPOT2");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, that.TUTORIAL_SPOT_ARROW_POS, that.TUTORIAL_SPOT_ARROW_RECT, (float) 90f, that.TUTORIAL_SPOT_MESSAGE_POS, -1, new System.Action(ye.<>m__233));
            that.mTerminalMap.SetMapCamera_FocusSpot(ye.focus_spot, 0.4f, null);
            that.mTerminalMap.SetSpotClickAct(new System.Action(ye.<>m__234));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
            if (this.mIsGoNext && (that.mTerminalList.GetQuestBoardListViewManager().GetInitMode() == QuestBoardListViewManager.InitMode.VALID))
            {
                that.SetState(TerminalSceneComponent.STATE.TUTORIAL2_QUEST_ARROW);
            }
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE0
        {
            internal TerminalSceneComponent.StateTutorial2_SpotArrow <>f__this;
            internal SrcSpotBasePrefab focus_spot;

            internal void <>m__233()
            {
                this.focus_spot.SetBtnColliderEnable(true);
            }

            internal void <>m__234()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow((System.Action) (() => (this.<>f__this.mIsGoNext = true)));
            }

            internal void <>m__235()
            {
                this.<>f__this.mIsGoNext = true;
            }
        }
    }

    private class StateTutorial3_GachaArrow : IState<TerminalSceneComponent>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache0;

        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE3 ye = new <begin>c__AnonStoreyE3 {
                that = that
            };
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_GACHA");
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = () => MainMenuBar.SetDispBtnColliderEnable(true, MainMenuBarButton.Kind.SUMMON);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, ye.that.TUTORIAL_GACHA_ARROW_POS, ye.that.TUTORIAL_GACHA_ARROW_RECT, (float) 0f, ye.that.TUTORIAL_MENU_MESSAGE_POS, -1, <>f__am$cache0);
            MainMenuBar.SetDispBtnAct(MainMenuBarButton.Kind.SUMMON, new System.Action(ye.<>m__23D));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE3
        {
            internal TerminalSceneComponent that;

            internal void <>m__23D()
            {
                MainMenuBar.SetDispBtnColliderEnable(false, MainMenuBarButton.Kind.SIZEOF);
                TutorialFlag.SetProgress(TutorialFlag.Progress._1);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(() => this.that.SetState(TerminalSceneComponent.STATE.NORMAL));
            }

            internal void <>m__23E()
            {
                this.that.SetState(TerminalSceneComponent.STATE.NORMAL);
            }
        }
    }

    private class StateTutorial3_MenuArrow : IState<TerminalSceneComponent>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache0;

        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE2 ye = new <begin>c__AnonStoreyE2 {
                that = that
            };
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_MENU");
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = () => MainMenuBar.SetMenuBtnColliderEnable(true);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, ye.that.TUTORIAL_MENU_ARROW_POS, ye.that.TUTORIAL_MENU_ARROW_RECT, (float) 0f, ye.that.TUTORIAL_MENU_MESSAGE_POS, -1, <>f__am$cache0);
            MainMenuBar.SetMenuBtnAct(new System.Action(ye.<>m__23A));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE2
        {
            internal TerminalSceneComponent that;

            internal void <>m__23A()
            {
                MainMenuBar.SetMenuBtnColliderEnable(false);
                MainMenuBar.SetDispBtnColliderEnable(false, MainMenuBarButton.Kind.SIZEOF);
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(() => this.that.SetState(TerminalSceneComponent.STATE.TUTORIAL3_GACHA_ARROW));
            }

            internal void <>m__23B()
            {
                this.that.SetState(TerminalSceneComponent.STATE.TUTORIAL3_GACHA_ARROW);
            }
        }
    }

    private class StateTutorial4_QuestArrow : IState<TerminalSceneComponent>
    {
        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE5 ye = new <begin>c__AnonStoreyE5 {
                that = that
            };
            ye.focus_quest = ye.that.mTerminalList.GetFocusQuest();
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_QUEST3");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, ye.that.TUTORIAL_QUEST_ARROW_POS, ye.that.TUTORIAL_QUEST_ARROW_RECT, (float) 90f, ye.that.TUTORIAL_QUEST_MESSAGE_POS, -1, new System.Action(ye.<>m__242));
            ye.that.mTerminalList.SetQuestClickAct(new System.Action(ye.<>m__243));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE5
        {
            internal QuestBoardListViewObject focus_quest;
            internal TerminalSceneComponent that;

            internal void <>m__242()
            {
                this.that.mTerminalList.GetQuestBoardListViewManager().GetScrollView().enabled = false;
                this.focus_quest.SetInput(true);
                this.focus_quest.gameObject.GetComponent<UIDragScrollView>().enabled = false;
            }

            internal void <>m__243()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(() => this.that.SetState(TerminalSceneComponent.STATE.NORMAL));
            }

            internal void <>m__244()
            {
                this.that.SetState(TerminalSceneComponent.STATE.NORMAL);
            }
        }
    }

    private class StateTutorial4_SpotArrow : IState<TerminalSceneComponent>
    {
        private bool mIsGoNext;

        public void begin(TerminalSceneComponent that)
        {
            <begin>c__AnonStoreyE4 ye = new <begin>c__AnonStoreyE4 {
                <>f__this = this
            };
            this.mIsGoNext = false;
            ye.focus_spot = that.mTerminalMap.GetFocusSpot();
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(that.TUTORIAL_SPOT_ARROW_POS, (float) 90f, that.TUTORIAL_SPOT_ARROW_RECT, new System.Action(ye.<>m__23F));
            that.mTerminalMap.SetMapCamera_FocusSpot(ye.focus_spot, 0.4f, null);
            that.mTerminalMap.SetSpotClickAct(new System.Action(ye.<>m__240));
        }

        public void end(TerminalSceneComponent that)
        {
        }

        public void update(TerminalSceneComponent that)
        {
            if (this.mIsGoNext && (that.mTerminalList.GetQuestBoardListViewManager().GetInitMode() == QuestBoardListViewManager.InitMode.VALID))
            {
                that.SetState(TerminalSceneComponent.STATE.TUTORIAL4_QUEST_ARROW);
            }
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyE4
        {
            internal TerminalSceneComponent.StateTutorial4_SpotArrow <>f__this;
            internal SrcSpotBasePrefab focus_spot;

            internal void <>m__23F()
            {
                this.focus_spot.SetBtnColliderEnable(true);
            }

            internal void <>m__240()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark((System.Action) (() => (this.<>f__this.mIsGoNext = true)));
            }

            internal void <>m__241()
            {
                this.<>f__this.mIsGoNext = true;
            }
        }
    }

    public enum TUTORIAL_KIND
    {
        NONE,
        _1,
        _2,
        _3,
        _4
    }
}

