using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ScrTerminalListTop : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache19;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1A;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1B;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1C;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1D;
    [CompilerGenerated]
    private static CommonConfirmDialog.ClickDelegate <>f__am$cache1E;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1F;
    [CompilerGenerated]
    private static System.Action <>f__am$cache20;
    [CompilerGenerated]
    private static System.Action <>f__am$cache21;
    [CompilerGenerated]
    private static System.Action <>f__am$cache22;
    [CompilerGenerated]
    private static System.Action <>f__am$cache23;
    public const int EVENT_BANNER_MAX = 1;
    private int keep_oldFriendShipRank;
    public UISprite mActionBgSp;
    public GameObject mActionPanel;
    private int mCaldeaQuestCount;
    public EarthCore mEarthCore;
    public ScrPlayerStatus mPlayerStatus;
    public GameObject mQcraBox;
    public GameObject mQcraItem;
    public GameObject mQcraSvt;
    private List<clsMapCtrl_QuestInfo> mQestInfs_Area = new List<clsMapCtrl_QuestInfo>();
    private List<clsMapCtrl_QuestInfo> mQestInfs_Caldea = new List<clsMapCtrl_QuestInfo>();
    private List<clsMapCtrl_QuestInfo> mQestInfs_Map = new List<clsMapCtrl_QuestInfo>();
    private List<clsMapCtrl_QuestInfo> mQestInfs_Story = new List<clsMapCtrl_QuestInfo>();
    private QuestRewardBoxAction mQrba;
    private QuestRewardItemAction mQria;
    private ServantRewardAction mQrsa;
    [SerializeField]
    private QuestBoardListViewManager mQuestBoardListViewManager;
    private System.Action mQuestClickAct;
    private PlayMakerFSM mTargetFsm;
    [SerializeField]
    private ScrTerminalMap mTerminalMap;
    [SerializeField]
    private TerminalSceneComponent mTerminalScene;
    public StandFigureSlideComponent mTerminalServant;
    public TitleInfoControl mTitleInfo;
    private List<GiftEntity> mWarClearRewardList;
    [SerializeField]
    private QuestAfterActionDialog questAfterDialog;

    public void cbfCapter_Create()
    {
        int warId = TerminalPramsManager.WarId;
        int spotId = TerminalPramsManager.SpotId;
        int questId = TerminalPramsManager.QuestId;
        this.mf04Map_CreateEventList(warId, spotId, questId);
        this.mfCallFsmEvent("evCAPTER_CREATE_FINISH");
    }

    public void cbfCapter_Select(int iPrevList)
    {
        int warId = TerminalPramsManager.WarId;
        if ((iPrevList != 0) && (iPrevList == 2))
        {
            TerminalPramsManager.IsAutoShortcut = true;
        }
        this.mEarthCore.SetFocusEarthPoint(warId);
        this.mfCallFsmEvent("evCAPTER_SELECT_FINISH");
    }

    public void cbfEvent_CaldeaGate()
    {
        this.StartFollower();
    }

    public void cbfEvent_Map()
    {
        this.StartFollower();
    }

    public void cbfInit_List()
    {
        this.mfInit_05CaldeaGate();
        this.mfInit_04Map();
        this.mfInit_03Story();
        this.mfInit_01Area();
        this.mEarthCore.Setup(this.mQuestTree.GetWarInfoAll_OrderReverse(), !TerminalPramsManager.IsDispUIStandFigure);
        this.mfCallFsmEvent("EVENT_INIT");
    }

    public void cbfPushBanner(int iSelect)
    {
        this.mfCallFsmEvent("evBANNER_FINISH");
    }

    public void cbfTitleInfoBtnBack_Click()
    {
        <cbfTitleInfoBtnBack_Click>c__AnonStoreyC3 yc = new <cbfTitleInfoBtnBack_Click>c__AnonStoreyC3 {
            <>f__this = this
        };
        Debug.Log(string.Concat(new object[] { "BackBtn:Input.touchCount=", Input.touchCount, " : frm=", Time.frameCount }));
        string activeStateName = this.mfGetMyFsmP().ActiveStateName;
        yc.end_act = new System.Action(yc.<>m__1CC);
        if (activeStateName.IndexOf("AREA WAIT") >= 0)
        {
            if (this.mQuestBoardListViewManager.IsInput)
            {
                TerminalPramsManager.PlaySE_Decide();
                NoticeInfoComponent.TermID = "6";
                if (!ManagerConfig.UseWebViewAuthoring)
                {
                    if (<>f__am$cache19 == null)
                    {
                        <>f__am$cache19 = delegate {
                        };
                    }
                    WebViewManager.OpenWebView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), <>f__am$cache19);
                }
                else
                {
                    if (<>f__am$cache1A == null)
                    {
                        <>f__am$cache1A = delegate {
                        };
                    }
                    WebViewManager.OpenView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), "index.html", <>f__am$cache1A);
                }
                this.mTitleInfo.setBackBtn_Terminal(false);
            }
        }
        else if ((activeStateName.IndexOf("CALDEAGATE WAIT") >= 0) || (activeStateName.IndexOf("STORY WAIT") >= 0))
        {
            if (this.mQuestBoardListViewManager.IsInput)
            {
                TerminalPramsManager.PlaySE_Cancel();
                this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.EXIT, new System.Action(yc.<>m__1CF));
            }
        }
        else if (activeStateName.IndexOf("SPOT SELECT WAIT") >= 0)
        {
            if (!this.mTerminalMap.GetSpotMaskObj().activeSelf)
            {
                TerminalPramsManager.PlaySE_Cancel();
                this.mTitleInfo.FrameOut(false);
                MainMenuBar.FrameOut(false);
                this.mPlayerStatus.FrameOut();
                this.mTerminalMap.mfCallFsmEvent("evGoBack_Earth");
                MainMenuBar.SetMenuBtnColliderEnable(false);
                yc.end_act.Call();
            }
        }
        else if ((activeStateName.IndexOf("CAPTER WAIT") >= 0) && this.mQuestBoardListViewManager.IsInput)
        {
            TerminalPramsManager.IsDoneShortcut = false;
            TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Map;
            TerminalPramsManager.PlaySE_Cancel();
            if (<>f__am$cache1B == null)
            {
                <>f__am$cache1B = delegate {
                };
            }
            this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.EXIT, <>f__am$cache1B);
            this.mTerminalMap.mfCallFsmEvent("evGoBack_Earth");
            this.mTitleInfo.setBackBtnSprite(false);
            yc.end_act.Call();
        }
    }

    public bool Click_Area(AreaBoardInfo ainf)
    {
        <Click_Area>c__AnonStoreyCE yce = new <Click_Area>c__AnonStoreyCE {
            <>f__this = this
        };
        QuestBoardListViewItemDraw.enQBoardL1Type type = ainf.qb_type;
        int num = ainf.etc_id;
        QuestBoardListViewItemDraw.enStatus status = ainf.status;
        EventEntity entity = ainf.ev_dat;
        if (num < 0)
        {
            return false;
        }
        if (type == QuestBoardListViewItemDraw.enQBoardL1Type.enBanner)
        {
            BannerEntity entity2 = ainf.banner_ents[ainf.banner_focus_idx];
            this.setInfomationWebViewInfo("活动详情", entity2.linkBody);
            return false;
        }
        this.mfSetFsmValueInt("eQBoardL1Type", (int) type);
        yce.fsm_ev_str = "evGO_SELECT";
        switch (type)
        {
            case QuestBoardListViewItemDraw.enQBoardL1Type.enCapter:
            {
                int num2 = num;
                if (ainf.status_id != 1)
                {
                    if (num2 == 0)
                    {
                        this.OpenNotificationDialog_QuestNone("QUEST_CHAPTER_CLOSED", null);
                        return false;
                    }
                    if ((entity != null) && !this.IsPlayEventWar(num2, entity))
                    {
                        return false;
                    }
                    this.mTerminalScene.IsWarStartAnim = status == QuestBoardListViewItemDraw.enStatus.enNew;
                    TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Map;
                    TerminalPramsManager.WarId = num2;
                    TerminalPramsManager.SummonType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getVaildPayType();
                    this.mTitleInfo.FrameOut(false);
                    MainMenuBar.FrameOut(false);
                    this.mPlayerStatus.FrameOut();
                    this.mTerminalServant.SlideOut(null, false);
                    this.mTerminalServant.SetEnable(false);
                    MainMenuBar.SetMenuBtnColliderEnable(false);
                    this.mTitleInfo.SetEventBtnCollider(false);
                    yce.fsm_ev_str = "GO_MAP";
                    break;
                }
                this.OpenNotificationDialog_QuestNone("QUEST_CHAPTER_CLOSED", null);
                return false;
            }
            case QuestBoardListViewItemDraw.enQBoardL1Type.enCaldeaGate:
                if (this.mQestInfs_Caldea.Count > 0)
                {
                    TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Caldea;
                    break;
                }
                this.OpenNotificationDialog_QuestNone("QUEST_CALDEA_NONE", null);
                return false;

            case QuestBoardListViewItemDraw.enQBoardL1Type.enStory:
                if (this.mQestInfs_Story.Count > 0)
                {
                    TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Story;
                    break;
                }
                this.OpenNotificationDialog_QuestNone("QUEST_SHORTCUT_NONE", null);
                return false;
        }
        if (type == QuestBoardListViewItemDraw.enQBoardL1Type.enCapter)
        {
            this.mTitleInfo.setBackBtnSprite(false);
            this.mTitleInfo.backBtnBgSprite.gameObject.SetActive(true);
        }
        else
        {
            this.mTitleInfo.setBackBtn_Terminal(true);
        }
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.EXIT, new System.Action(yce.<>m__1ED));
        return true;
    }

    public void Click_Quest(clsMapCtrl_QuestInfo qinf)
    {
        int num = qinf.mfGetWarID();
        int num2 = qinf.mfGetSpotID();
        int num3 = qinf.mfGetQuestID();
        int num4 = qinf.mfGetQuestPhase();
        int needAp = qinf.mfGetMine().getActConsume(qinf.GetApCalcVal());
        QuestEntity.enForceOperation operation = (QuestEntity.enForceOperation) qinf.mfGetMine().getForceOperation();
        if (TerminalPramsManager.Debug_IsQuestReleaseAll)
        {
            operation = QuestEntity.enForceOperation.FORCE_OPEN;
        }
        if (operation != QuestEntity.enForceOperation.FORCE_OPEN)
        {
            long endTime = qinf.GetEndTime();
            long num7 = endTime - NetworkManager.getTime();
            if ((endTime > 0L) && (num7 <= 0L))
            {
                string title = string.Empty;
                string message = LocalizationManager.Get("QUEST_TIME_OVER");
                if (<>f__am$cache21 == null)
                {
                    <>f__am$cache21 = (System.Action) (() => SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null));
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, <>f__am$cache21, -1);
                return;
            }
        }
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int num8 = entity.getActMax();
        int num9 = entity.getAct();
        if (needAp > num8)
        {
            string str3 = LocalizationManager.Get("SHORT_DLG_TITLE");
            string str4 = LocalizationManager.Get("QUEST_AP_MAX_OVER");
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(str3, str4, <>f__am$cache22, -1);
        }
        else
        {
            int num10;
            int num11;
            UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
            master.getCount(out num10, out num11);
            if (master.CheckServantAdd(1))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num10, entity.svtKeep, false, true, delegate (ServantFrameShortDlgComponent.resultClicked result) {
                    <Click_Quest>c__AnonStoreyD0 yd = new <Click_Quest>c__AnonStoreyD0 {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(yd.<>m__1F7));
                });
            }
            else if (master.CheckEquipAdd(1))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num11, entity.svtEquipKeep, true, true, delegate (ServantFrameShortDlgComponent.resultClicked result) {
                    <Click_Quest>c__AnonStoreyD1 yd = new <Click_Quest>c__AnonStoreyD1 {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(yd.<>m__1F8));
                });
            }
            else if (needAp > num9)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenApRecoverItemListDialog(needAp, new ApRecoverDlgComponent.CallbackFunc(this.EndRecoverUserGameRecover));
            }
            else
            {
                if (num == WarEntity.CALDEAGATE_ID)
                {
                    TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Caldea;
                }
                else
                {
                    TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Map;
                }
                TerminalPramsManager.IsDoneShortcut = false;
                TerminalPramsManager.PhaseCnt = num4;
                TerminalPramsManager.QuestId = num3;
                TerminalPramsManager.SpotId = num2;
                TerminalPramsManager.WarId = num;
                TerminalPramsManager.SummonType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getVaildPayType();
                this.mQuestClickAct.Call();
                this.mQuestClickAct = null;
                this.mfCallFsmEvent("evGO_SELECT");
            }
        }
    }

    public void Click_Shortcut(int iWarID, int iQuestID)
    {
        <Click_Shortcut>c__AnonStoreyCF ycf = new <Click_Shortcut>c__AnonStoreyCF {
            iWarID = iWarID,
            <>f__this = this
        };
        TerminalPramsManager.QuestId = iQuestID;
        TerminalPramsManager.WarId = ycf.iWarID;
        TerminalPramsManager.SummonType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getVaildPayType();
        TerminalPramsManager.IsDoneShortcut = true;
        TerminalPramsManager.PlaySE_Decide();
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.EXIT, new System.Action(ycf.<>m__1F1));
        if (WarEntity.CALDEAGATE_ID != ycf.iWarID)
        {
            this.mTitleInfo.FrameOut(false);
            MainMenuBar.FrameOut(false);
            this.mPlayerStatus.FrameOut();
            this.mTerminalServant.SlideOut(null, false);
            this.mTerminalServant.SetEnable(false);
            MainMenuBar.SetMenuBtnColliderEnable(false);
            this.mTitleInfo.SetEventBtnCollider(false);
        }
    }

    private void EndPurchaseSvtEquipFrame(ServantEquipFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipFramePurchaseMenu();
    }

    private void EndPurchaseSvtFrame(ServantFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFramePurchaseMenu();
    }

    private void EndRecoverUserGameRecover(ApRecoverDlgComponent.Result result)
    {
        if (result == ApRecoverDlgComponent.Result.RECOVER)
        {
            this.mPlayerStatus.mfInitUserData();
            this.mQuestBoardListViewManager.SetupDisp();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseApRecoverItemListDialog();
    }

    private string GetEventWarTimeStr(DateTime date_time) => 
        string.Format(LocalizationManager.Get("QUEST_EVENT_WAR_TIME"), new object[] { date_time.Month, date_time.Day, date_time.Hour, date_time.Minute });

    public QuestBoardListViewObject GetFocusQuest()
    {
        if (this.mQuestBoardListViewManager.ObjectList.Count > 0)
        {
            return this.mQuestBoardListViewManager.ObjectList[0];
        }
        return null;
    }

    public QuestBoardListViewManager GetQuestBoardListViewManager() => 
        this.mQuestBoardListViewManager;

    private bool IsPlayEventWar(int war_id, EventEntity ev_dat)
    {
        if (TerminalPramsManager.Debug_IsQuestReleaseAll)
        {
            return true;
        }
        long num = NetworkManager.getTime();
        string title = string.Empty;
        DateTime time = NetworkManager.getLocalDateTime(ev_dat.startedAt);
        DateTime time2 = NetworkManager.getLocalDateTime(ev_dat.getEventEndedAt());
        if (num > ev_dat.getEventEndedAt())
        {
            string str3 = string.Format(LocalizationManager.Get("QUEST_EVENT_WAR_END"), ev_dat.getEventName(), this.GetEventWarTimeStr(time2));
            ShopEntity[] enableEventEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEnableEventEntitiyList(ev_dat.getEventId());
            if ((enableEventEntitiyList != null) && (enableEventEntitiyList.Length > 0))
            {
                string decideTxt = LocalizationManager.Get("QUEST_EVENT_WAR_SHOP_BTN");
                string cancleTxt = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
                DateTime time3 = NetworkManager.getLocalDateTime(enableEventEntitiyList[0].closedAt);
                str3 = str3 + string.Format(LocalizationManager.Get("QUEST_EVENT_WAR_SHOP"), this.GetEventWarTimeStr(time3));
                if (<>f__am$cache1E == null)
                {
                    <>f__am$cache1E = delegate (bool is_decide) {
                        if (<>f__am$cache23 == null)
                        {
                            <>f__am$cache23 = delegate {
                            };
                        }
                        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(<>f__am$cache23);
                        if (is_decide)
                        {
                            EventItemListComponent.GoToShopEventItem(0);
                        }
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(title, str3, decideTxt, cancleTxt, <>f__am$cache1E);
            }
            else
            {
                if (<>f__am$cache1F == null)
                {
                    <>f__am$cache1F = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, str3, <>f__am$cache1F, -1);
            }
            return false;
        }
        if ((num >= ev_dat.getEventStartedAt()) && SingletonTemplate<QuestTree>.Instance.IsWarOpen(war_id))
        {
            return true;
        }
        string message = string.Format(LocalizationManager.Get("QUEST_EVENT_WAR_NOTICE"), ev_dat.getEventName(), this.GetEventWarTimeStr(time), this.GetEventWarTimeStr(time2));
        string key = "QUEST_EVENT_WAR_COND_" + war_id;
        if (LocalizationManager.ContainsKey(key))
        {
            message = message + LocalizationManager.Get(key);
        }
        if (<>f__am$cache20 == null)
        {
            <>f__am$cache20 = delegate {
            };
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, <>f__am$cache20, -1);
        return false;
    }

    public void mcbfAreaInit()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("Battle", () => this.StartWarClearAction(), SoundManager.CueType.ALL);
    }

    public void mcbfAreaWait()
    {
        this.mTitleInfo.setBackBtn_Terminal(false);
        if (!TerminalPramsManager.IsAutoResume)
        {
            TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Top;
            TerminalPramsManager.IsDoneShortcut = false;
            TerminalPramsManager.SpotId = -1;
            TerminalPramsManager.SummonType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getVaildPayType();
            if (!TerminalPramsManager.IsDispDone_AutoWebView && !BalanceConfig.IsIOS_Examination)
            {
                NoticeInfoComponent.TermID = "6";
                TerminalPramsManager.IsDispDone_AutoWebView = true;
                if (!ManagerConfig.UseWebViewAuthoring)
                {
                    WebViewManager.OpenWebView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), () => this.mcbfAreaWaitNext());
                }
                else
                {
                    WebViewManager.OpenView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), "index.html", () => this.mcbfAreaWaitNext());
                }
            }
            else
            {
                this.mcbfAreaWaitNext();
            }
        }
        else
        {
            foreach (clsMapCtrl_QuestInfo info in this.mQestInfs_Area)
            {
                AreaBoardInfo mAreaBoardInfo = info.mAreaBoardInfo;
                QuestBoardListViewItemDraw.enQBoardL1Type enNone = QuestBoardListViewItemDraw.enQBoardL1Type.enNone;
                switch (TerminalPramsManager.DispState)
                {
                    case TerminalPramsManager.eDispState.Map:
                    {
                        if (TerminalPramsManager.WarId == mAreaBoardInfo.etc_id)
                        {
                            break;
                        }
                        continue;
                    }
                    case TerminalPramsManager.eDispState.Caldea:
                        enNone = QuestBoardListViewItemDraw.enQBoardL1Type.enCaldeaGate;
                        goto Label_0089;

                    case TerminalPramsManager.eDispState.Story:
                        enNone = QuestBoardListViewItemDraw.enQBoardL1Type.enStory;
                        goto Label_0089;

                    default:
                        goto Label_0089;
                }
                enNone = QuestBoardListViewItemDraw.enQBoardL1Type.enCapter;
            Label_0089:
                if (enNone == mAreaBoardInfo.qb_type)
                {
                    if (!this.Click_Area(mAreaBoardInfo))
                    {
                        break;
                    }
                    return;
                }
            }
            TerminalPramsManager.IsAutoResume = false;
            this.StartQuestBoardListIn_Area();
            this.mTerminalScene.Fadein_WorldDisp(null);
            TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Top;
        }
    }

    private void mcbfAreaWaitNext()
    {
        this.mTerminalScene.CheckRewardPopupChain(new System.Action(this.StartFavoriteTutorial));
    }

    public void mcbfCaldeaGateInit()
    {
        this.mTitleInfo.FrameIn(false);
        MainMenuBar.FrameIn(false);
        this.mPlayerStatus.FrameIn();
        this.SlideIn_TerminalServant();
        this.mQuestBoardListViewManager.CreateList(QuestBoardListViewItem.InfoKind.CALDEA, this.mQestInfs_Caldea, (float) QuestBoardListViewItemDraw.POS_Y_ITVL_QUEST, (float) QuestBoardListViewItemDraw.CLIP_W_DEFAULT);
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.INTO, () => this.mfCallFsmEvent("evGO_EVENT"));
        this.mTitleInfo.setTitleImg(TitleInfoControl.TitleKind.CALDEAGATE, true);
        if (TerminalPramsManager.QuestId > 0)
        {
            int num = 0;
            foreach (clsMapCtrl_QuestInfo info in this.mQestInfs_Caldea)
            {
                if (info.mfGetQuestID() == TerminalPramsManager.QuestId)
                {
                    this.mQuestBoardListViewManager.SetFocusItem(num);
                    break;
                }
                num++;
            }
        }
    }

    public void mcbfCaldeaGateWait()
    {
        if (TerminalPramsManager.IsAutoResume)
        {
            TerminalPramsManager.IsAutoResume = false;
            this.mTerminalScene.Fadein_WorldDisp(null);
        }
    }

    public void mcbfCapterInit()
    {
        this.mQuestBoardListViewManager.CreateList(QuestBoardListViewItem.InfoKind.MAP, this.mQestInfs_Map, (float) QuestBoardListViewItemDraw.POS_Y_ITVL_QUEST, (float) QuestBoardListViewItemDraw.CLIP_W_DEFAULT);
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.INTO, () => this.mfCallFsmEvent("evGO_EVENT"));
        if (TerminalPramsManager.QuestId > 0)
        {
            int num = 0;
            foreach (clsMapCtrl_QuestInfo info in this.mQestInfs_Map)
            {
                if (info.mfGetQuestID() == TerminalPramsManager.QuestId)
                {
                    this.mQuestBoardListViewManager.SetFocusItem(num);
                    break;
                }
                num++;
            }
        }
    }

    public void mcbfCapterWait()
    {
    }

    public void mcbfCheckSceneStatus()
    {
        if (TerminalPramsManager.mfGetSceneStatus() != TerminalPramsManager.enSceneStatus.enResume)
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_INIT");
        }
        else if (TerminalPramsManager.SpotId == SpotEntity.CALDEAGATE_ID)
        {
            this.mfCallFsmEvent("evGO_CALDEA");
        }
        else
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_RESUME");
        }
    }

    public void mcbfSpotSelectWait()
    {
    }

    public void mcbfStoryInit()
    {
        this.mTitleInfo.FrameIn(false);
        MainMenuBar.FrameIn(false);
        this.mPlayerStatus.FrameIn();
        this.SlideIn_TerminalServant();
        this.mQuestBoardListViewManager.CreateList(QuestBoardListViewItem.InfoKind.STORY, this.mQestInfs_Story, (float) QuestBoardListViewItemDraw.POS_Y_ITVL_SHORTCUT, (float) QuestBoardListViewItemDraw.CLIP_W_DEFAULT);
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.INTO, () => this.mfCallFsmEvent("evGO_EVENT"));
        this.mTitleInfo.setTitleImg(TitleInfoControl.TitleKind.STORY, true);
    }

    public void mcbfStoryWait()
    {
        if (TerminalPramsManager.IsAutoResume)
        {
            if (TerminalPramsManager.DispState == TerminalPramsManager.eDispState.Story)
            {
                if (TerminalPramsManager.IsDoneShortcut)
                {
                    foreach (clsMapCtrl_QuestInfo info in this.mQestInfs_Story)
                    {
                        if (info.mfGetQuestID() == TerminalPramsManager.QuestId)
                        {
                            int iWarID = info.mfGetWarID();
                            int iQuestID = info.mfGetQuestID();
                            this.Click_Shortcut(iWarID, iQuestID);
                            return;
                        }
                    }
                }
                TerminalPramsManager.IsAutoResume = false;
                this.mTerminalScene.Fadein_WorldDisp(null);
            }
        }
        else
        {
            TerminalPramsManager.IsDoneShortcut = false;
        }
    }

    public void mf04Map_CreateEventList(int iWarID, int iSpotID, int iQuestID)
    {
        List<clsMapCtrl_QuestInfo> list = this.mQuestTree.mfGetQuestInfoListP();
        List<clsMapCtrl_QuestInfo> list2 = this.mQestInfs_Map;
        list2.Clear();
        foreach (clsMapCtrl_QuestInfo info in list)
        {
            if (((iWarID == info.mfGetWarID()) && (iSpotID == info.mfGetSpotID())) && (info.mfGetDispType() != clsMapCtrl_QuestInfo.enDispType.None))
            {
                list2.Add(info);
            }
        }
    }

    private void mfCallFsmEvent(string sEventStr)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Event(sEventStr);
        }
    }

    private int mfGetFsmValueInt(string sValueStr)
    {
        int num = 0;
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            num = rfsm.Fsm.Variables.GetFsmInt(sValueStr).Value;
        }
        return num;
    }

    public PlayMakerFSM mfGetMyFsmP()
    {
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
        return this.mTargetFsm;
    }

    public void mfInit_01Area()
    {
        BannerEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BannerMaster>(DataNameKind.Kind.BANNER).GetEnableEntitiyList();
        int length = enableEntitiyList.Length;
        if (length > 1)
        {
            length = 1;
        }
        List<clsMapCtrl_WarInfo> chapterWarList = this.mQuestTree.mfGetMapCtrlP().GetChapterWarList();
        List<clsMapCtrl_WarInfo> list2 = new List<clsMapCtrl_WarInfo>();
        if (SingletonTemplate<clsQuestCheck>.Instance.IsLastWarClear())
        {
            list2.Add(new clsMapCtrl_WarInfo());
        }
        foreach (clsMapCtrl_WarInfo info in chapterWarList)
        {
            list2.Add(info);
        }
        int count = list2.Count;
        List<clsMapCtrl_WarInfo> list3 = this.mQuestTree.mfGetMapCtrlP().GetEventWarList_BeforeEnd();
        List<clsMapCtrl_WarInfo> list4 = this.mQuestTree.mfGetMapCtrlP().GetEventWarList_AfterEnd();
        int num3 = list3.Count;
        int num4 = list4.Count;
        int num5 = 1;
        int num6 = 1;
        int num7 = 0;
        int num8 = num7 + length;
        int num9 = num8 + num3;
        int num10 = num9 + num5;
        int num11 = num10 + count;
        int num12 = num11 + num6;
        int num13 = num12 + num4;
        int num14 = num13;
        List<clsMapCtrl_QuestInfo> list5 = this.mQestInfs_Area;
        list5.Clear();
        for (int i = 0; i < num14; i++)
        {
            clsMapCtrl_QuestInfo item = new clsMapCtrl_QuestInfo();
            AreaBoardInfo ainf = new AreaBoardInfo {
                ev_dat = null
            };
            if (i < num8)
            {
                int num16 = i;
                ainf.qb_type = QuestBoardListViewItemDraw.enQBoardL1Type.enBanner;
                ainf.status = QuestBoardListViewItemDraw.enStatus.enNone;
                ainf.banner_ents = enableEntitiyList;
            }
            else if (i < num9)
            {
                int num17 = i - num8;
                this.SetAreaBoard_WarInf(ainf, list3[num17], false);
                long val = ainf.ev_dat.getEventEndedAt();
                item.SetEndTime(val);
            }
            else if (i < num10)
            {
                ainf.qb_type = QuestBoardListViewItemDraw.enQBoardL1Type.enCaldeaGate;
                ainf.etc_id = 0;
                ainf.status = QuestBoardListViewItemDraw.enStatus.enNone;
                foreach (clsMapCtrl_QuestInfo info4 in this.mQestInfs_Caldea)
                {
                    if (info4.mfIsNew())
                    {
                        ainf.status = QuestBoardListViewItemDraw.enStatus.enNew;
                        break;
                    }
                }
                ainf.quest_count = this.mCaldeaQuestCount;
            }
            else if (i < num11)
            {
                int num19 = i - num10;
                bool flag = (num19 == 0) && !SingletonTemplate<clsQuestCheck>.Instance.IsLastWarClear();
                this.SetAreaBoard_WarInf(ainf, list2[num19], flag);
            }
            else if (i < num12)
            {
                ainf.qb_type = QuestBoardListViewItemDraw.enQBoardL1Type.enStory;
                ainf.etc_id = 0;
                ainf.status = QuestBoardListViewItemDraw.enStatus.enNone;
                foreach (clsMapCtrl_QuestInfo info5 in this.mQestInfs_Story)
                {
                    if (info5.mfIsNew())
                    {
                        ainf.status = QuestBoardListViewItemDraw.enStatus.enNew;
                        break;
                    }
                }
                ainf.quest_count = this.mQestInfs_Story.Count;
            }
            else if (i < num13)
            {
                int num20 = i - num12;
                this.SetAreaBoard_WarInf(ainf, list4[num20], false);
            }
            item.mAreaBoardInfo = ainf;
            list5.Add(item);
        }
    }

    public void mfInit_03Story()
    {
        List<clsMapCtrl_QuestInfo> list = this.mQuestTree.mfGetQuestInfoListP();
        List<clsMapCtrl_QuestInfo> list2 = this.mQestInfs_Story;
        list2.Clear();
        foreach (clsMapCtrl_QuestInfo info in list)
        {
            if ((((info.mfGetQuestType() == QuestEntity.enType.FRIENDSHIP) && (info.mfGetDispType() != clsMapCtrl_QuestInfo.enDispType.None)) && (info.mfGetDispType() != clsMapCtrl_QuestInfo.enDispType.Closed)) && ((info.mfGetSpotID() == SpotEntity.CALDEAGATE_ID) || ((info.mMapCtrl_WarInfo.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.None) && (info.mMapCtrl_SpotInfo.mfGetDispType() == clsMapCtrl_SpotInfo.enDispType.Normal))))
            {
                list2.Add(info);
            }
        }
    }

    public void mfInit_04Map()
    {
    }

    public void mfInit_05CaldeaGate()
    {
        List<clsMapCtrl_QuestInfo> list = this.mQuestTree.mfGetQuestInfoListP();
        List<clsMapCtrl_QuestInfo> list2 = this.mQestInfs_Caldea;
        list2.Clear();
        this.mCaldeaQuestCount = 0;
        foreach (clsMapCtrl_QuestInfo info in list)
        {
            if ((WarEntity.CALDEAGATE_ID == info.mfGetWarID()) && (info.mfGetDispType() != clsMapCtrl_QuestInfo.enDispType.None))
            {
                if (!SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false))
                {
                    this.mCaldeaQuestCount++;
                }
                list2.Add(info);
            }
        }
    }

    private void mfSetFsmValueInt(string sValueStr, int iValueInt)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Variables.GetFsmInt(sValueStr).Value = iValueInt;
        }
    }

    private void onEndWebView()
    {
        Debug.Log("close web view");
    }

    public void OpenigMovieAfter()
    {
        this.StartHeroineLimitAction();
    }

    private void OpenNotificationDialog_QuestNone(string msg_str_key, System.Action end_act = null)
    {
        string title = string.Empty;
        string message = LocalizationManager.Get(msg_str_key);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, end_act, -1);
    }

    public void openWebView(string title, string path, string dynamicPath, string url)
    {
        if (string.IsNullOrEmpty(dynamicPath))
        {
            WebViewManager.OpenView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), path, new System.Action(this.onEndWebView));
        }
        else
        {
            WebViewManager.OpenViewDynamic(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), dynamicPath, new System.Action(this.onEndWebView));
        }
        if (<>f__am$cache1D == null)
        {
            <>f__am$cache1D = delegate {
            };
        }
        WebViewManager.OpenUniWebView(title, url, <>f__am$cache1D);
    }

    private void SetAreaBoard_WarInf(AreaBoardInfo ainf, clsMapCtrl_WarInfo war_inf, bool is_disp_next = false)
    {
        QuestBoardListViewItemDraw.enStatus enNone = QuestBoardListViewItemDraw.enStatus.enNone;
        switch (war_inf.mfGetStatus())
        {
            case clsMapCtrl_WarInfo.enStatus.New:
                enNone = QuestBoardListViewItemDraw.enStatus.enNew;
                break;

            case clsMapCtrl_WarInfo.enStatus.Clear:
                enNone = QuestBoardListViewItemDraw.enStatus.enClear;
                break;

            case clsMapCtrl_WarInfo.enStatus.Complete:
                enNone = QuestBoardListViewItemDraw.enStatus.enClear;
                break;
        }
        ainf.qb_type = QuestBoardListViewItemDraw.enQBoardL1Type.enCapter;
        ainf.status = enNone;
        ainf.is_next = is_disp_next;
        ainf.ev_dat = null;
        WarEntity entity = war_inf.mfGetMine();
        if (entity != null)
        {
            int num = entity.getWarId();
            int num2 = entity.getStatus();
            ainf.etc_id = num;
            ainf.status_id = num2;
            ainf.quest_count = this.mQuestTree.GetQuestCount(num);
            ainf.ev_dat = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(war_inf.GetEventId());
        }
    }

    private void SetFocusCaldeaGateQuest()
    {
        if (TerminalPramsManager.IsQuestClear)
        {
            QuestMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST);
            int questId = TerminalPramsManager.QuestId;
            QuestEntity entity = master.getEntityFromId<QuestEntity>(questId);
            if (entity.IsCaldeaGate())
            {
                int[] numArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).GetQuestIdList(CondType.Kind.QUEST_CLEAR, questId, -1);
                if (numArray.Length > 0)
                {
                    clsMapCtrl_QuestInfo info = null;
                    foreach (int num2 in numArray)
                    {
                        clsMapCtrl_QuestInfo questInfo = SingletonTemplate<QuestTree>.Instance.GetQuestInfo(num2);
                        if (((questInfo != null) && (questInfo.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Normal)) && ((questInfo.mfGetMine().getQuestType() == entity.getQuestType()) && questInfo.mfGetMine().IsCaldeaGate()))
                        {
                            info = questInfo;
                            break;
                        }
                    }
                    if (info != null)
                    {
                        TerminalPramsManager.QuestId = info.mfGetQuestID();
                    }
                }
            }
        }
    }

    public void setInfomationWebViewInfo(string title, string url)
    {
        string path = string.Empty;
        string dynamicPath = string.Empty;
        if (!ManagerConfig.UseWebViewAuthoring)
        {
            dynamicPath = "InformationTop";
        }
        else
        {
            path = "index.html";
            dynamicPath = string.Empty;
        }
        NoticeInfoComponent.TermID = "6";
        this.openWebView(title, path, dynamicPath, url);
    }

    public void SetQuestClickAct(System.Action act)
    {
        this.mQuestClickAct = act;
    }

    private void ShowWarClearReward(System.Action endAct)
    {
        string str;
        string str2;
        <ShowWarClearReward>c__AnonStoreyCD ycd = new <ShowWarClearReward>c__AnonStoreyCD {
            endAct = endAct,
            <>f__this = this
        };
        this.mWarClearRewardList[0].GetInfo(out str, out str2);
        str2 = string.Empty;
        if (this.mWarClearRewardList[0].type == 1)
        {
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.mWarClearRewardList[0].objectId);
            if (entity.IsServant || entity.IsServantEquip)
            {
                str2 = entity.getClassName() + "\n";
            }
        }
        string message = string.Format(LocalizationManager.Get("PRESENT_BOX_NOTIFICATION_WAR_CLEAR"), str2, str);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, message, new System.Action(ycd.<>m__1E4), -1);
    }

    private void SlideIn_TerminalServant()
    {
        this.mTerminalScene.TerminalServant.SetEnable(true);
        if (TerminalPramsManager.IsDispUIStandFigure)
        {
            this.mTerminalServant.SlideIn(null);
        }
    }

    private void StartFavoriteTutorial()
    {
        if (TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE1) && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2))
        {
            string message = LocalizationManager.Get("TUTORIAL_MESSAGE_TERMINAL_FAVORITE");
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(message, TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2, () => this.StartQuestBoardListIn_Area());
        }
        else
        {
            this.StartQuestBoardListIn_Area();
        }
    }

    public void StartFollower()
    {
        int warId = TerminalPramsManager.WarId;
        int questId = TerminalPramsManager.QuestId;
        int phaseCnt = TerminalPramsManager.PhaseCnt;
        WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(warId);
        QuestPhaseEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(questId, phaseCnt + 1);
        UserGameEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        BattleSetupInfo data = new BattleSetupInfo {
            warId = warId,
            questId = questId,
            questPhase = (entity2 == null) ? phaseCnt : (phaseCnt + 1),
            isQuestNew = SingletonTemplate<clsQuestCheck>.Instance.mfCheck_IsQuestNew(questId),
            deckId = (entity3 == null) ? 0L : entity3.activeDeckId,
            followerId = 0L,
            followerClassId = 0
        };
        data.eventUpValSetupInfo = new EventUpValSetupInfo(data.questId, data.questPhase);
        if (!data.eventUpValSetupInfo.IsBonusSkill())
        {
            data.eventUpValSetupInfo = null;
        }
        data.isChildFollower = false;
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.Follower, SceneManager.FadeType.BLACK, data);
    }

    private void StartFriendPointNotification()
    {
    }

    private void StartHeroineFriendshipAction()
    {
        if (SingletonTemplate<TerminalDebugWindow>.Instance.HeroineFriendshipWin.GetValSafe(0) != 0)
        {
            foreach (UserServantEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList())
            {
                if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.getSvtId()).checkIsHeroineSvt())
                {
                    UserServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(entity.id);
                    QuestClearHeroineInfo info = new QuestClearHeroineInfo {
                        oldUsrSvtData = entity3
                    };
                    this.keep_oldFriendShipRank = 0;
                    TerminalPramsManager.mQuestClearHeroineInfo = info;
                    break;
                }
            }
        }
        bool flag2 = false;
        QuestClearHeroineInfo mQuestClearHeroineInfo = TerminalPramsManager.mQuestClearHeroineInfo;
        if (mQuestClearHeroineInfo != null)
        {
            mQuestClearHeroineInfo.isChangeLimitcnt = false;
            mQuestClearHeroineInfo.isChangeTreasureDvc = false;
            mQuestClearHeroineInfo.oldFriendShipRank = this.keep_oldFriendShipRank;
            flag2 = mQuestClearHeroineInfo.IsUpFriendShipRank();
        }
        if (flag2)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenFriendshipUp(mQuestClearHeroineInfo.oldUsrSvtData, mQuestClearHeroineInfo.oldFriendShipRank, is_decide => this.StartQuestClearItemAction());
        }
        else
        {
            this.StartQuestClearItemAction();
        }
    }

    private void StartHeroineLimitAction()
    {
        bool isChangeLimitcnt = false;
        QuestClearHeroineInfo mQuestClearHeroineInfo = TerminalPramsManager.mQuestClearHeroineInfo;
        this.keep_oldFriendShipRank = -1;
        if (SingletonTemplate<TerminalDebugWindow>.Instance.HeroineLimitCountWin.GetValSafe(0) != 0)
        {
            foreach (UserServantEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList())
            {
                if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.getSvtId()).checkIsHeroineSvt())
                {
                    UserServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(entity.id);
                    mQuestClearHeroineInfo = new QuestClearHeroineInfo {
                        oldUsrSvtData = entity3,
                        isChangeLimitcnt = true
                    };
                    break;
                }
            }
        }
        if (mQuestClearHeroineInfo != null)
        {
            this.keep_oldFriendShipRank = mQuestClearHeroineInfo.oldFriendShipRank;
            mQuestClearHeroineInfo.isChangeTreasureDvc = false;
            mQuestClearHeroineInfo.oldFriendShipRank = -1;
            isChangeLimitcnt = mQuestClearHeroineInfo.isChangeLimitcnt;
        }
        if (isChangeLimitcnt)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenLimitUpCombineResult(CombineResultEffectComponent.Kind.LIMITUP, mQuestClearHeroineInfo.oldUsrSvtData, delegate (bool isDecide) {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
                this.StartHeroineTreasureAction();
            });
        }
        else
        {
            this.StartHeroineTreasureAction();
        }
    }

    private void StartHeroineTreasureAction()
    {
        DebugWindow heroineTreasureWin = SingletonTemplate<TerminalDebugWindow>.Instance.HeroineTreasureWin;
        bool flag = heroineTreasureWin.GetValSafe(2) > 0;
        if (flag)
        {
            foreach (UserServantEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList())
            {
                if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.getSvtId()).checkIsHeroineSvt())
                {
                    UserServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(entity.id);
                    TerminalPramsManager.mQuestClearHeroineInfo = new QuestClearHeroineInfo();
                    TerminalPramsManager.mQuestClearHeroineInfo.oldUsrSvtData = entity3;
                    break;
                }
            }
        }
        QuestClearHeroineInfo mQuestClearHeroineInfo = TerminalPramsManager.mQuestClearHeroineInfo;
        if (mQuestClearHeroineInfo != null)
        {
            int[] numArray;
            int[] numArray2;
            int[] numArray3;
            int[] numArray4;
            int[] numArray5;
            int[] numArray6;
            int[] numArray7;
            int[] numArray8;
            int[] numArray9;
            int[] numArray10;
            string[] strArray;
            string[] strArray2;
            string[] strArray3;
            string[] strArray4;
            int[] numArray11;
            int[] numArray12;
            int[] numArray13;
            int[] numArray14;
            bool[] flagArray;
            bool[] flagArray2;
            UserServantEntity oldUsrSvtData = mQuestClearHeroineInfo.oldUsrSvtData;
            UserServantEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(oldUsrSvtData.id);
            oldUsrSvtData.getTreasureDeviceInfo(out numArray, out numArray3, out numArray5, out numArray7, out numArray9, out strArray, out strArray3, out numArray11, out numArray13, out flagArray, TerminalPramsManager.QuestId);
            entity5.getTreasureDeviceInfo(out numArray2, out numArray4, out numArray6, out numArray8, out numArray10, out strArray2, out strArray4, out numArray12, out numArray14, out flagArray2, -1);
            if (flag)
            {
                bool flag2 = heroineTreasureWin.GetValSafe(1) != 0;
                int index = 0;
                flagArray2[index] = true;
                numArray2[index] = heroineTreasureWin.GetValSafe(2);
                numArray4[index] = heroineTreasureWin.GetValSafe(3);
                if (!flag2)
                {
                    flagArray[index] = false;
                    numArray[index] = numArray2[index];
                }
                else
                {
                    numArray[index] = numArray2[index] - 1;
                    flagArray[index] = true;
                }
            }
            this.StartHeroineTreasureAction_sub(0, numArray, numArray2, numArray3, numArray4, flagArray, flagArray2, () => this.StartHeroineFriendshipAction());
        }
        else
        {
            this.StartHeroineFriendshipAction();
        }
    }

    private bool StartHeroineTreasureAction_sub(int idx, int[] tdIdList_old, int[] tdIdList_now, int[] tdLvList_old, int[] tdLvList_now, bool[] tdIsUseList_old, bool[] tdIsUseList_now, System.Action end_act)
    {
        <StartHeroineTreasureAction_sub>c__AnonStoreyC4 yc = new <StartHeroineTreasureAction_sub>c__AnonStoreyC4 {
            idx = idx,
            tdIdList_old = tdIdList_old,
            tdIdList_now = tdIdList_now,
            tdLvList_old = tdLvList_old,
            tdLvList_now = tdLvList_now,
            tdIsUseList_old = tdIsUseList_old,
            tdIsUseList_now = tdIsUseList_now,
            end_act = end_act,
            <>f__this = this
        };
        if (yc.idx >= 3)
        {
            yc.end_act.Call();
            return false;
        }
        if (yc.tdIsUseList_now[yc.idx])
        {
            CombineResultEffectComponent.Kind tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
            QuestClearHeroineInfo mQuestClearHeroineInfo = TerminalPramsManager.mQuestClearHeroineInfo;
            mQuestClearHeroineInfo.isChangeLimitcnt = false;
            mQuestClearHeroineInfo.oldFriendShipRank = -1;
            if (!yc.tdIsUseList_old[yc.idx])
            {
                mQuestClearHeroineInfo.isChangeTreasureDvc = true;
                tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
                mQuestClearHeroineInfo.treasureDvcId = yc.tdIdList_now[yc.idx];
                mQuestClearHeroineInfo.treasureDvcLv = yc.tdLvList_now[yc.idx];
            }
            else if (yc.tdIdList_old[yc.idx] != yc.tdIdList_now[yc.idx])
            {
                mQuestClearHeroineInfo.isChangeTreasureDvc = true;
                tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVC_RANKUP;
                mQuestClearHeroineInfo.treasureDvcId = yc.tdIdList_now[yc.idx];
                mQuestClearHeroineInfo.treasureDvcLv = yc.tdLvList_now[yc.idx];
                if ((yc.tdIdList_old[yc.idx] % 10) == 0)
                {
                    tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
                }
            }
            if (mQuestClearHeroineInfo.isChangeTreasureDvc)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNobleCombineResult(tREASUREDVCOPEN, mQuestClearHeroineInfo.oldUsrSvtData, mQuestClearHeroineInfo.treasureDvcId, mQuestClearHeroineInfo.treasureDvcLv, new CombineResultEffectComponent.ClickDelegate(yc.<>m__1D5), yc.tdIdList_old[yc.idx], yc.tdLvList_old[yc.idx]);
                return true;
            }
        }
        return this.StartHeroineTreasureAction_sub(yc.idx + 1, yc.tdIdList_old, yc.tdIdList_now, yc.tdLvList_old, yc.tdLvList_now, yc.tdIsUseList_old, yc.tdIsUseList_now, yc.end_act);
    }

    private void StartLoginAndCampaignBonus()
    {
    }

    private void StartQuestBoardListIn_Area()
    {
        this.mTitleInfo.FrameIn(false);
        MainMenuBar.FrameIn(false);
        this.mPlayerStatus.FrameIn();
        this.SlideIn_TerminalServant();
        this.mTitleInfo.setBackBtn_Terminal(false);
        this.mQuestBoardListViewManager.CreateList(QuestBoardListViewItem.InfoKind.AREA, this.mQestInfs_Area, (float) QuestBoardListViewItemDraw.POS_Y_ITVL_AREA, (float) QuestBoardListViewItemDraw.CLIP_W_AREA);
        if (<>f__am$cache1C == null)
        {
            <>f__am$cache1C = () => MainMenuBar.SetMenuBtnColliderEnable(true);
        }
        this.mQuestBoardListViewManager.SetMode(QuestBoardListViewManager.InitMode.INTO, <>f__am$cache1C);
        this.mTitleInfo.setTitleImg(TitleInfoControl.TitleKind.TERMINAL, true);
    }

    private void StartQuestClearChangeAction()
    {
        <StartQuestClearChangeAction>c__AnonStoreyCC ycc = new <StartQuestClearChangeAction>c__AnonStoreyCC {
            <>f__this = this
        };
        ycc.sb = new StringBuilder();
        if (TerminalPramsManager.IsPhaseClear)
        {
            int questId = TerminalPramsManager.QuestId;
            long[] args = new long[] { NetworkManager.UserId, (long) questId };
            UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(args);
            if (TerminalPramsManager.IsQuestClear && (entity.getClearNum() == 1))
            {
                QuestReleaseEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getEntitys<QuestReleaseEntity>(DataNameKind.Kind.QUEST_RELEASE);
                List<QuestEntity> list = new List<QuestEntity>();
                foreach (QuestReleaseEntity entity2 in entityArray)
                {
                    if ((entity2.getType() == 1) && (entity2.getTargetId() == questId))
                    {
                        QuestEntity item = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(entity2.getQuestId());
                        if ((item.getQuestType() == 3) && SingletonTemplate<clsQuestCheck>.Instance.IsQuestRelease(item.getQuestId(), -1, CondType.Kind.NONE))
                        {
                            list.Add(item);
                        }
                    }
                }
                if (list.Count > 0)
                {
                    if (ycc.sb.Length > 0)
                    {
                        ycc.sb.Append("\n\n");
                    }
                    ycc.sb.Append(LocalizationManager.Get("QUEST_START_ACTION_TITLE_STORY"));
                    foreach (QuestEntity entity4 in list)
                    {
                        ServantEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity4.getServantId());
                        ycc.sb.AppendFormat(LocalizationManager.Get("QUEST_CLEAR_CHANGE_ACTION_STORY_FORMAT"), entity5.name, entity4.getQuestName());
                    }
                    ycc.sb.Append("\n");
                    ycc.sb.Append(LocalizationManager.Get("QUEST_CLEAR_CHANGE_ACTION_STORY"));
                }
            }
            if (TerminalPramsManager.IsQuestClear)
            {
                foreach (ServantCommentEntity entity6 in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<ServantCommentEntity>(DataNameKind.Kind.SERVANT_COMMENT))
                {
                    if (((entity6.condType == 1) && (entity6.condValue == questId)) && CondType.IsOpen(CondType.Kind.SVT_GET, entity6.svtId, 0))
                    {
                        if (ycc.sb.Length > 0)
                        {
                            ycc.sb.Append("\n\n");
                        }
                        ServantEntity entity7 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity6.svtId);
                        ycc.sb.AppendFormat(LocalizationManager.Get("QUEST_CLEAR_CHANGE_ACTION_COLLECTION"), entity7.name);
                    }
                }
            }
        }
        if (ycc.sb.Length > 0)
        {
            this.mActionBgSp.gameObject.SetActive(true);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(ycc.<>m__1E2));
        }
        else
        {
            this.StartSceneFadeIn();
        }
    }

    private void StartQuestClearItemAction()
    {
        QuestRewardInfo[] mQuestRewardInfos = TerminalPramsManager.mQuestRewardInfos;
        int num = -1;
        DebugWindow questRewardWin = SingletonTemplate<TerminalDebugWindow>.Instance.QuestRewardWin;
        int valSafe = questRewardWin.GetValSafe(1);
        int num3 = questRewardWin.GetValSafe(2);
        int num4 = questRewardWin.GetValSafe(3);
        int num5 = questRewardWin.GetValSafe(5);
        int num6 = questRewardWin.GetValSafe(6);
        if (((valSafe > 0) || (num3 > 0)) || (num4 > 0))
        {
            QuestRewardInfo info = new QuestRewardInfo {
                type = (num3 < 0x186a0) ? 2 : 1,
                objectId = num3,
                num = num4,
                isNew = questRewardWin.IsEnableSafe(4),
                userSvtId = num5,
                limitCount = num6
            };
            TerminalPramsManager.QuestId = 0xf4241;
            num = valSafe;
            mQuestRewardInfos = new QuestRewardInfo[] { info };
        }
        if (mQuestRewardInfos != null)
        {
            this.StartQuestClearItemAction_sub(0, mQuestRewardInfos, num, false, () => this.StartQuestClearSkillAction());
        }
        else
        {
            this.StartQuestClearSkillAction();
        }
    }

    private void StartQuestClearItemAction_PlayItemOrSvt(bool is_from_treasure_box, Action<bool> play_end_act, float fade_in_time = 0)
    {
        <StartQuestClearItemAction_PlayItemOrSvt>c__AnonStoreyC7 yc = new <StartQuestClearItemAction_PlayItemOrSvt>c__AnonStoreyC7 {
            play_end_act = play_end_act,
            is_from_treasure_box = is_from_treasure_box,
            <>f__this = this
        };
        if (this.mQria != null)
        {
            this.mQria.Play(yc.is_from_treasure_box, new System.Action(yc.<>m__1DA), fade_in_time);
        }
        else
        {
            this.mQrsa.Play(yc.is_from_treasure_box, new System.Action(yc.<>m__1DB), fade_in_time);
        }
    }

    private void StartQuestClearItemAction_sub(int idx, QuestRewardInfo[] qris, int item_icon_id, bool is_from_treasure_box, System.Action sub_end_act)
    {
        <StartQuestClearItemAction_sub>c__AnonStoreyC5 yc = new <StartQuestClearItemAction_sub>c__AnonStoreyC5 {
            idx = idx,
            qris = qris,
            item_icon_id = item_icon_id,
            sub_end_act = sub_end_act,
            <>f__this = this
        };
        if (yc.idx >= yc.qris.Length)
        {
            yc.sub_end_act.Call();
        }
        else
        {
            QuestRewardInfo qri = yc.qris[yc.idx];
            bool flag = false;
            if (qri != null)
            {
                flag = qri.num > 0;
            }
            if (!flag)
            {
                this.StartQuestClearItemAction_sub(yc.idx + 1, yc.qris, yc.item_icon_id, is_from_treasure_box, yc.sub_end_act);
            }
            else
            {
                <StartQuestClearItemAction_sub>c__AnonStoreyC6 yc2 = new <StartQuestClearItemAction_sub>c__AnonStoreyC6 {
                    <>f__ref$197 = yc,
                    <>f__this = this
                };
                int questId = TerminalPramsManager.QuestId;
                QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(questId);
                if (yc.item_icon_id < 0)
                {
                    yc.item_icon_id = entity.getGiftIconId();
                }
                this.mQrba = null;
                if (ImageItem.IsTreasure(yc.item_icon_id))
                {
                    GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mQcraBox);
                    self.transform.parent = this.mActionPanel.transform;
                    self.transform.localPosition = Vector3.zero;
                    self.transform.localRotation = Quaternion.identity;
                    self.transform.localScale = Vector3.one;
                    this.mQrba = self.SafeGetComponent<QuestRewardBoxAction>();
                    QuestRewardBoxAction.BOX_TYPE nORMAL = QuestRewardBoxAction.BOX_TYPE.NORMAL;
                    switch (yc.item_icon_id)
                    {
                        case 1:
                            nORMAL = QuestRewardBoxAction.BOX_TYPE.NORMAL;
                            break;

                        case 2:
                            nORMAL = QuestRewardBoxAction.BOX_TYPE.SILVER;
                            break;

                        case 3:
                            nORMAL = QuestRewardBoxAction.BOX_TYPE.GOLD;
                            break;
                    }
                    this.mQrba.Setup(nORMAL);
                }
                yc.item_icon_id = 0;
                this.mQria = null;
                this.mQrsa = null;
                if (Gift.IsServant(qri.type))
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.mQcraSvt);
                    obj3.transform.parent = this.mActionPanel.transform;
                    obj3.transform.localPosition = Vector3.zero;
                    obj3.transform.localRotation = Quaternion.identity;
                    obj3.transform.localScale = Vector3.one;
                    this.mQrsa = obj3.SafeGetComponent<ServantRewardAction>();
                    ServantRewardAction.PLAY_FLAG play_flag = ServantRewardAction.PLAY_FLAG.FADE_OUT | ServantRewardAction.PLAY_FLAG.FADE_IN;
                    if (qri.type == 7)
                    {
                        play_flag |= ServantRewardAction.PLAY_FLAG.EVENT_SVT_GET;
                    }
                    this.mQrsa.Setup(qri, play_flag);
                }
                else
                {
                    GameObject obj4 = UnityEngine.Object.Instantiate<GameObject>(this.mQcraItem);
                    obj4.transform.parent = this.mActionPanel.transform;
                    obj4.transform.localPosition = Vector3.zero;
                    obj4.transform.localRotation = Quaternion.identity;
                    obj4.transform.localScale = Vector3.one;
                    this.mQria = obj4.SafeGetComponent<QuestRewardItemAction>();
                    this.mQria.Setup(qri);
                }
                this.mActionBgSp.gameObject.SetActive(true);
                yc2.play_end_act = new Action<bool>(yc2.<>m__1D8);
                if (this.mQrba != null)
                {
                    this.mQrba.Play(new System.Action(yc2.<>m__1D9));
                }
                else
                {
                    this.StartQuestClearItemAction_PlayItemOrSvt(is_from_treasure_box, yc2.play_end_act, 0f);
                }
            }
        }
    }

    private void StartQuestClearSkillAction()
    {
        DebugWindow questClearSkillWin = SingletonTemplate<TerminalDebugWindow>.Instance.QuestClearSkillWin;
        int valSafe = questClearSkillWin.GetValSafe(2);
        if (valSafe > 0)
        {
            int num = questClearSkillWin.GetValSafe(3);
            int priority = questClearSkillWin.GetValSafe(4);
            ServantSkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantSkillMaster>(DataNameKind.Kind.SERVANT_SKILL).getEntityFromId(valSafe, num, priority);
            if (entity != null)
            {
                List<ServantSkillEntity> list = new List<ServantSkillEntity> {
                    entity
                };
                TerminalPramsManager.mQuestClearReward_Skill = list;
            }
        }
        List<ServantSkillEntity> list2 = TerminalPramsManager.mQuestClearReward_Skill;
        if (list2 != null)
        {
            UserServantCollectionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION);
            List<UserServantCollectionEntity> list3 = new List<UserServantCollectionEntity>();
            foreach (ServantSkillEntity entity2 in list2)
            {
                UserServantCollectionEntity item = master.getEntityFromId(NetworkManager.UserId, entity2.svtId);
                if ((item != null) && item.IsGet())
                {
                    list3.Add(item);
                }
            }
            this.StartQuestClearSkillAction_usd(0, list3, () => this.StartQuestClearTreasureAction());
        }
        else
        {
            this.StartQuestClearTreasureAction();
        }
    }

    private bool StartQuestClearSkillAction_slot(UserServantCollectionEntity usd, int idx, int[] tdIdList_old, int[] tdIdList_now, int[] tdLvList_old, int[] tdLvList_now, bool[] tdIsUseList_old, bool[] tdIsUseList_now, System.Action end_act)
    {
        <StartQuestClearSkillAction_slot>c__AnonStoreyC9 yc = new <StartQuestClearSkillAction_slot>c__AnonStoreyC9 {
            usd = usd,
            idx = idx,
            tdIdList_old = tdIdList_old,
            tdIdList_now = tdIdList_now,
            tdLvList_old = tdLvList_old,
            tdLvList_now = tdLvList_now,
            tdIsUseList_old = tdIsUseList_old,
            tdIsUseList_now = tdIsUseList_now,
            end_act = end_act,
            <>f__this = this
        };
        if (yc.idx >= BalanceConfig.SvtSkillListMax)
        {
            yc.end_act.Call();
            return false;
        }
        if (yc.tdIsUseList_now[yc.idx])
        {
            bool flag = false;
            CombineResultEffectComponent.Kind sKILLOPEN = CombineResultEffectComponent.Kind.SKILLOPEN;
            if (!yc.tdIsUseList_old[yc.idx])
            {
                flag = true;
            }
            else if (yc.tdIdList_old[yc.idx] != yc.tdIdList_now[yc.idx])
            {
                flag = true;
                sKILLOPEN = CombineResultEffectComponent.Kind.SKILL_RANKUP;
            }
            if (flag)
            {
                UserServantEntity usrSvtData = new UserServantEntity(yc.usd);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSkillCombineResult(sKILLOPEN, usrSvtData, yc.tdIdList_now[yc.idx], yc.tdLvList_now[yc.idx], new CombineResultEffectComponent.ClickDelegate(yc.<>m__1DE), yc.tdIdList_old[yc.idx], yc.tdLvList_old[yc.idx]);
                return true;
            }
        }
        return this.StartQuestClearSkillAction_slot(yc.usd, yc.idx + 1, yc.tdIdList_old, yc.tdIdList_now, yc.tdLvList_old, yc.tdLvList_now, yc.tdIsUseList_old, yc.tdIsUseList_now, yc.end_act);
    }

    private bool StartQuestClearSkillAction_usd(int usd_idx, List<UserServantCollectionEntity> usd_list, System.Action end_act)
    {
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        int[] numArray4;
        int[] numArray5;
        int[] numArray6;
        string[] strArray;
        string[] strArray2;
        string[] strArray3;
        string[] strArray4;
        bool[] flagArray;
        bool[] flagArray2;
        <StartQuestClearSkillAction_usd>c__AnonStoreyC8 yc = new <StartQuestClearSkillAction_usd>c__AnonStoreyC8 {
            usd_idx = usd_idx,
            usd_list = usd_list,
            end_act = end_act,
            <>f__this = this
        };
        if (yc.usd_idx >= yc.usd_list.Count)
        {
            yc.end_act.Call();
            return false;
        }
        UserServantCollectionEntity usd = yc.usd_list[yc.usd_idx];
        usd.getSkillInfo(out numArray, out numArray3, out numArray5, out strArray, out strArray3, out flagArray, TerminalPramsManager.QuestId);
        usd.getSkillInfo(out numArray2, out numArray4, out numArray6, out strArray2, out strArray4, out flagArray2, -1);
        DebugWindow questClearSkillWin = SingletonTemplate<TerminalDebugWindow>.Instance.QuestClearSkillWin;
        if (questClearSkillWin.GetValSafe(2) > 0)
        {
            bool flag2 = questClearSkillWin.GetValSafe(1) != 0;
            int index = 0;
            flagArray2[index] = true;
            if (!flag2)
            {
                flagArray[index] = false;
                numArray[index] = numArray2[index];
            }
            else
            {
                numArray[index] = numArray2[index] - 1;
                flagArray[index] = true;
            }
        }
        return (this.StartQuestClearSkillAction_slot(usd, 0, numArray, numArray2, numArray3, numArray4, flagArray, flagArray2, new System.Action(yc.<>m__1DD)) || this.StartQuestClearSkillAction_usd(yc.usd_idx + 1, yc.usd_list, yc.end_act));
    }

    private void StartQuestClearTreasureAction()
    {
        int valSafe = SingletonTemplate<TerminalDebugWindow>.Instance.QuestClearTreasureWin.GetValSafe(2);
        if (valSafe > 0)
        {
            foreach (ServantTreasureDvcEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<ServantTreasureDvcEntity>(DataNameKind.Kind.SERVANT_TREASUREDEVICE))
            {
                if (entity.treasureDeviceId == valSafe)
                {
                    List<ServantTreasureDvcEntity> list = new List<ServantTreasureDvcEntity> {
                        entity
                    };
                    TerminalPramsManager.mQuestClearReward_Treasure = list;
                    break;
                }
            }
        }
        List<ServantTreasureDvcEntity> list2 = TerminalPramsManager.mQuestClearReward_Treasure;
        QuestClearHeroineInfo mQuestClearHeroineInfo = TerminalPramsManager.mQuestClearHeroineInfo;
        if (list2 != null)
        {
            UserServantEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList();
            List<UserServantEntity> list3 = new List<UserServantEntity>();
            List<int> list4 = new List<int>();
            foreach (UserServantEntity entity2 in entityArray3)
            {
                foreach (ServantTreasureDvcEntity entity3 in list2)
                {
                    int item = entity2.getSvtId();
                    if (((item == entity3.svtId) && ((mQuestClearHeroineInfo == null) || (entity3.svtId != mQuestClearHeroineInfo.oldUsrSvtData.getSvtId()))) && !list4.Contains(item))
                    {
                        list3.Add(entity2);
                        list4.Add(item);
                        break;
                    }
                }
            }
            this.StartQuestClearTreasureAction_usd(0, list3, () => this.StartQuestClearChangeAction());
        }
        else
        {
            this.StartQuestClearChangeAction();
        }
    }

    private bool StartQuestClearTreasureAction_slot(UserServantEntity usd, int idx, int[] tdIdList_old, int[] tdIdList_now, int[] tdLvList_old, int[] tdLvList_now, bool[] tdIsUseList_old, bool[] tdIsUseList_now, System.Action end_act)
    {
        <StartQuestClearTreasureAction_slot>c__AnonStoreyCB ycb = new <StartQuestClearTreasureAction_slot>c__AnonStoreyCB {
            usd = usd,
            idx = idx,
            tdIdList_old = tdIdList_old,
            tdIdList_now = tdIdList_now,
            tdLvList_old = tdLvList_old,
            tdLvList_now = tdLvList_now,
            tdIsUseList_old = tdIsUseList_old,
            tdIsUseList_now = tdIsUseList_now,
            end_act = end_act,
            <>f__this = this
        };
        if (ycb.idx >= 3)
        {
            ycb.end_act.Call();
            return false;
        }
        if (ycb.tdIsUseList_now[ycb.idx])
        {
            bool flag = false;
            CombineResultEffectComponent.Kind tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
            if (!ycb.tdIsUseList_old[ycb.idx])
            {
                flag = true;
            }
            else if (ycb.tdIdList_old[ycb.idx] != ycb.tdIdList_now[ycb.idx])
            {
                flag = true;
                tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVC_RANKUP;
                if ((ycb.tdIdList_old[ycb.idx] % 10) == 0)
                {
                    tREASUREDVCOPEN = CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
                }
            }
            if (flag)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNobleCombineResult(tREASUREDVCOPEN, ycb.usd, ycb.tdIdList_now[ycb.idx], ycb.tdLvList_now[ycb.idx], new CombineResultEffectComponent.ClickDelegate(ycb.<>m__1E1), ycb.tdIdList_old[ycb.idx], ycb.tdLvList_old[ycb.idx]);
                return true;
            }
        }
        return this.StartQuestClearTreasureAction_slot(ycb.usd, ycb.idx + 1, ycb.tdIdList_old, ycb.tdIdList_now, ycb.tdLvList_old, ycb.tdLvList_now, ycb.tdIsUseList_old, ycb.tdIsUseList_now, ycb.end_act);
    }

    private bool StartQuestClearTreasureAction_usd(int usd_idx, List<UserServantEntity> usd_list, System.Action end_act)
    {
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        int[] numArray4;
        int[] numArray5;
        int[] numArray6;
        int[] numArray7;
        int[] numArray8;
        int[] numArray9;
        int[] numArray10;
        string[] strArray;
        string[] strArray2;
        string[] strArray3;
        string[] strArray4;
        int[] numArray11;
        int[] numArray12;
        int[] numArray13;
        int[] numArray14;
        bool[] flagArray;
        bool[] flagArray2;
        <StartQuestClearTreasureAction_usd>c__AnonStoreyCA yca = new <StartQuestClearTreasureAction_usd>c__AnonStoreyCA {
            usd_idx = usd_idx,
            usd_list = usd_list,
            end_act = end_act,
            <>f__this = this
        };
        if (yca.usd_idx >= yca.usd_list.Count)
        {
            yca.end_act.Call();
            return false;
        }
        UserServantEntity usd = yca.usd_list[yca.usd_idx];
        usd.getTreasureDeviceInfo(out numArray, out numArray3, out numArray5, out numArray7, out numArray9, out strArray, out strArray3, out numArray13, out numArray11, out flagArray, TerminalPramsManager.QuestId);
        usd.getTreasureDeviceInfo(out numArray2, out numArray4, out numArray6, out numArray8, out numArray10, out strArray2, out strArray4, out numArray14, out numArray12, out flagArray2, -1);
        DebugWindow questClearTreasureWin = SingletonTemplate<TerminalDebugWindow>.Instance.QuestClearTreasureWin;
        if (questClearTreasureWin.GetValSafe(2) > 0)
        {
            bool flag2 = questClearTreasureWin.GetValSafe(1) != 0;
            int index = 0;
            flagArray2[index] = true;
            numArray2[index] = questClearTreasureWin.GetValSafe(2);
            if (!flag2)
            {
                flagArray[index] = false;
                numArray[index] = numArray2[index];
            }
            else
            {
                numArray[index] = numArray2[index] - 1;
                flagArray[index] = true;
            }
        }
        return (this.StartQuestClearTreasureAction_slot(usd, 0, numArray, numArray2, numArray3, numArray4, flagArray, flagArray2, new System.Action(yca.<>m__1E0)) || this.StartQuestClearTreasureAction_usd(yca.usd_idx + 1, yca.usd_list, yca.end_act));
    }

    private void StartSceneFadeIn()
    {
        this.SetFocusCaldeaGateQuest();
        TerminalPramsManager.mQuestClearHeroineInfo = null;
        TerminalPramsManager.mQuestClearReward_Skill = null;
        TerminalPramsManager.mQuestClearReward_Treasure = null;
        TerminalPramsManager.mQuestRewardInfos = null;
        TerminalPramsManager.IsQuestClear = false;
        TerminalPramsManager.IsPhaseClear = false;
        SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage("Battle");
        SingletonTemplate<MissionNotifyManager>.Instance.CancelPause();
        if (!this.mTerminalScene.CheckTutorial())
        {
            if (!TerminalPramsManager.IsAutoResume)
            {
                this.mTerminalScene.Fadein_WorldDisp(() => this.StartSceneFadeIn_End());
            }
            else
            {
                this.StartSceneFadeIn_End();
            }
        }
    }

    private void StartSceneFadeIn_End()
    {
        this.mfCallFsmEvent("evGO_EVENT");
    }

    private void StartServantEventJoinLeaveNotification()
    {
    }

    public void StartShowWarClearReward(System.Action endAct)
    {
        if ((this.mWarClearRewardList != null) && (this.mWarClearRewardList.Count > 0))
        {
            this.ShowWarClearReward(endAct);
        }
        else
        {
            endAct.Call();
        }
    }

    private void StartWarClearAction()
    {
        if (TerminalPramsManager.IsWarClear)
        {
            TerminalPramsManager.IsWarClear = false;
            TerminalPramsManager.IsAutoResume = false;
            ScriptManager.PlayChapterClear(TerminalPramsManager.WarId, delegate (bool isExit) {
                if (TerminalPramsManager.WarId == ConstantMaster.getValue("FIRST_WAR_ID"))
                {
                    this.mfCallFsmEvent("PLAY_OPENING_MOVIE");
                }
                else
                {
                    this.OpenigMovieAfter();
                }
            }, false);
        }
        else
        {
            this.OpenigMovieAfter();
        }
    }

    private QuestTree mQuestTree =>
        SingletonTemplate<QuestTree>.Instance;

    [CompilerGenerated]
    private sealed class <cbfTitleInfoBtnBack_Click>c__AnonStoreyC3
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;

        internal void <>m__1CC()
        {
            this.<>f__this.mfCallFsmEvent("gevINFOBAR_BACK");
        }

        internal void <>m__1CF()
        {
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <Click_Area>c__AnonStoreyCE
    {
        internal ScrTerminalListTop <>f__this;
        internal string fsm_ev_str;

        internal void <>m__1ED()
        {
            this.<>f__this.mfCallFsmEvent(this.fsm_ev_str);
        }
    }

    [CompilerGenerated]
    private sealed class <Click_Quest>c__AnonStoreyD0
    {
        internal ScrTerminalListTop <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1F7()
        {
            switch (this.result)
            {
                case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFramePurchaseMenu(new ServantFramePurchaseMenu.CallbackFunc(this.<>f__this.EndPurchaseSvtFrame), null);
                    break;

                case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantCombine"));
                    break;

                case ServantFrameShortDlgComponent.resultClicked.SELL:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 0));
                    break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Click_Quest>c__AnonStoreyD1
    {
        internal ScrTerminalListTop <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1F8()
        {
            switch (this.result)
            {
                case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipFramePurchaseMenu(new ServantEquipFramePurchaseMenu.CallbackFunc(this.<>f__this.EndPurchaseSvtEquipFrame), null);
                    break;

                case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantEQCombine"));
                    break;

                case ServantFrameShortDlgComponent.resultClicked.SELL:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 1));
                    break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Click_Shortcut>c__AnonStoreyCF
    {
        internal ScrTerminalListTop <>f__this;
        internal int iWarID;

        internal void <>m__1F1()
        {
            if (WarEntity.CALDEAGATE_ID != this.iWarID)
            {
                this.<>f__this.mfCallFsmEvent("evGO_SELECT");
            }
            else
            {
                this.<>f__this.mfCallFsmEvent("evGO_CALDEA");
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowWarClearReward>c__AnonStoreyCD
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action endAct;

        internal void <>m__1E4()
        {
            this.<>f__this.mWarClearRewardList.RemoveAt(0);
            if (this.<>f__this.mWarClearRewardList.Count > 0)
            {
                this.<>f__this.ShowWarClearReward(this.endAct);
            }
            else
            {
                this.<>f__this.mWarClearRewardList = null;
                this.endAct.Call();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <StartHeroineTreasureAction_sub>c__AnonStoreyC4
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;
        internal int idx;
        internal int[] tdIdList_now;
        internal int[] tdIdList_old;
        internal bool[] tdIsUseList_now;
        internal bool[] tdIsUseList_old;
        internal int[] tdLvList_now;
        internal int[] tdLvList_old;

        internal void <>m__1D5(bool is_decide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
            this.<>f__this.StartHeroineTreasureAction_sub(this.idx + 1, this.tdIdList_old, this.tdIdList_now, this.tdLvList_old, this.tdLvList_now, this.tdIsUseList_old, this.tdIsUseList_now, this.end_act);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearChangeAction>c__AnonStoreyCC
    {
        internal ScrTerminalListTop <>f__this;
        internal StringBuilder sb;

        internal void <>m__1E2()
        {
            this.<>f__this.questAfterDialog.Open(string.Empty, this.sb.ToString(), (System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.<>f__this.mActionBgSp.gameObject.SetActive(false);
                this.<>f__this.StartSceneFadeIn();
            })));
        }

        internal void <>m__1F9()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.<>f__this.mActionBgSp.gameObject.SetActive(false);
                this.<>f__this.StartSceneFadeIn();
            });
        }

        internal void <>m__1FA()
        {
            this.<>f__this.mActionBgSp.gameObject.SetActive(false);
            this.<>f__this.StartSceneFadeIn();
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearItemAction_PlayItemOrSvt>c__AnonStoreyC7
    {
        internal ScrTerminalListTop <>f__this;
        internal bool is_from_treasure_box;
        internal Action<bool> play_end_act;

        internal void <>m__1DA()
        {
            UnityEngine.Object.Destroy(this.<>f__this.mQria.gameObject);
            this.play_end_act.Call<bool>(this.is_from_treasure_box);
        }

        internal void <>m__1DB()
        {
            UnityEngine.Object.Destroy(this.<>f__this.mQrsa.gameObject);
            this.play_end_act.Call<bool>(this.is_from_treasure_box);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearItemAction_sub>c__AnonStoreyC5
    {
        internal ScrTerminalListTop <>f__this;
        internal int idx;
        internal int item_icon_id;
        internal QuestRewardInfo[] qris;
        internal System.Action sub_end_act;
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearItemAction_sub>c__AnonStoreyC6
    {
        internal ScrTerminalListTop.<StartQuestClearItemAction_sub>c__AnonStoreyC5 <>f__ref$197;
        internal ScrTerminalListTop <>f__this;
        internal Action<bool> play_end_act;

        internal void <>m__1D8(bool _is_from_treasure_box)
        {
            this.<>f__this.mActionBgSp.gameObject.SetActive(false);
            this.<>f__this.StartQuestClearItemAction_sub(this.<>f__ref$197.idx + 1, this.<>f__ref$197.qris, this.<>f__ref$197.item_icon_id, _is_from_treasure_box, this.<>f__ref$197.sub_end_act);
        }

        internal void <>m__1D9()
        {
            this.<>f__this.StartQuestClearItemAction_PlayItemOrSvt(true, this.play_end_act, 1f);
            UnityEngine.Object.Destroy(this.<>f__this.mQrba.gameObject);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearSkillAction_slot>c__AnonStoreyC9
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;
        internal int idx;
        internal int[] tdIdList_now;
        internal int[] tdIdList_old;
        internal bool[] tdIsUseList_now;
        internal bool[] tdIsUseList_old;
        internal int[] tdLvList_now;
        internal int[] tdLvList_old;
        internal UserServantCollectionEntity usd;

        internal void <>m__1DE(bool is_decide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
            this.<>f__this.StartQuestClearSkillAction_slot(this.usd, this.idx + 1, this.tdIdList_old, this.tdIdList_now, this.tdLvList_old, this.tdLvList_now, this.tdIsUseList_old, this.tdIsUseList_now, this.end_act);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearSkillAction_usd>c__AnonStoreyC8
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;
        internal int usd_idx;
        internal List<UserServantCollectionEntity> usd_list;

        internal void <>m__1DD()
        {
            this.<>f__this.StartQuestClearSkillAction_usd(this.usd_idx + 1, this.usd_list, this.end_act);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearTreasureAction_slot>c__AnonStoreyCB
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;
        internal int idx;
        internal int[] tdIdList_now;
        internal int[] tdIdList_old;
        internal bool[] tdIsUseList_now;
        internal bool[] tdIsUseList_old;
        internal int[] tdLvList_now;
        internal int[] tdLvList_old;
        internal UserServantEntity usd;

        internal void <>m__1E1(bool is_decide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
            this.<>f__this.StartQuestClearTreasureAction_slot(this.usd, this.idx + 1, this.tdIdList_old, this.tdIdList_now, this.tdLvList_old, this.tdLvList_now, this.tdIsUseList_old, this.tdIsUseList_now, this.end_act);
        }
    }

    [CompilerGenerated]
    private sealed class <StartQuestClearTreasureAction_usd>c__AnonStoreyCA
    {
        internal ScrTerminalListTop <>f__this;
        internal System.Action end_act;
        internal int usd_idx;
        internal List<UserServantEntity> usd_list;

        internal void <>m__1E0()
        {
            this.<>f__this.StartQuestClearTreasureAction_usd(this.usd_idx + 1, this.usd_list, this.end_act);
        }
    }

    public enum enPrevList
    {
        TOPMENU,
        HEROBALLAD,
        STORY
    }
}

