using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class QuestBoardListViewItemDraw : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cache78;
    [CompilerGenerated]
    private static System.Action <>f__am$cache79;
    [CompilerGenerated]
    private static System.Action <>f__am$cache7A;
    public static readonly string BANNER_POINT_SPNAME_OFF = "img_slider_off";
    public static readonly string BANNER_POINT_SPNAME_ON = "img_slider_on";
    public static readonly string CALDEAGATE_SPNAME_PREFIX = "caldeagate_";
    public static readonly Color CAMPAIGN_AP_COLOR = new Color(1f, 0.7568628f, 0.2352941f);
    public static readonly string CHAPTER_SP_BASE_NAME = "img_main_{0:D2}";
    public static readonly Vector3 CLEAR_POS_AREA = new Vector3(-155f, -47f, 0f);
    public static readonly Vector3 CLEAR_POS_QUEST = new Vector3(-183f, -55f, 0f);
    public static readonly int CLIP_RANGE_MARGIN = 200;
    public static readonly int CLIP_W_AREA = 480;
    public static readonly int CLIP_W_DEFAULT = 600;
    public static readonly string EVENT_WAR_SPNAME_PREFIX = "event_war_";
    public static readonly float FLICK_THRESHOLD = 2f;
    public static readonly int FOCUS_IDX_OFFSET = (SCRL_OBJ_DUMMY_NUM / 2);
    public static readonly Vector3 LINE_ST_POS_AREA = new Vector3(-215f, -5f, 0f);
    public static readonly Vector3 LINE_ST_POS_SHORTCUT = new Vector3(-240f, -5f, 0f);
    [SerializeField]
    private UILabel mApCostLb;
    [SerializeField]
    private GameObject mApObj;
    [SerializeField]
    private GameObject mBannerArrowObj;
    [SerializeField]
    private ScrollArrowComponent[] mBannerArrows;
    private float mBannerAutoMoveTimeInterval;
    private float mBannerAutoMoveTimeOld;
    private int mBannerCount;
    [SerializeField]
    private UIGrid mBannerPointGrid;
    [SerializeField]
    private UISprite[] mBannerPointSps;
    [SerializeField]
    private GameObject mBannerRoot;
    private int mCampaignStrIdx;
    private List<string> mCampaignStrs;
    [SerializeField]
    private UISprite mcBaseP;
    [SerializeField]
    private GameObject mClassIconRoot;
    [SerializeField]
    private ServantClassIconComponent[] mClassIcons;
    [SerializeField]
    private UISprite mClassStrSp;
    [SerializeField]
    private UIAtlas mCommonAtlas;
    private QuestBoardListViewEarthLine mEarthLine;
    private GameObject mEarthLineObj;
    [SerializeField]
    private GameObject mEarthLinePrefab;
    private bool mIsEnableBannerAutoMoveOld;
    private bool mIsEnableDragX;
    private bool mIsPressed;
    private bool mIsPressedForDragX;
    [SerializeField]
    private UISprite mLabelChapterSp;
    [SerializeField]
    private ItemIconComponent mLabelFaceIcon;
    [SerializeField]
    private UISprite mLabelFaceMaskQuestSp;
    [SerializeField]
    private UISprite mLabelFaceMaskShortcutSp;
    [SerializeField]
    private GameObject mLabelFaceObj;
    [SerializeField]
    private UISprite mLabelNameSp;
    [SerializeField]
    private GameObject mLabelObj;
    [SerializeField]
    private UILabel mLabelSubSectionNumLb;
    private QuestBoardListViewItem mListViewItem;
    private QuestBoardListViewObject mListViewObject;
    [SerializeField]
    private NewIconComponent mNewIcon;
    [SerializeField]
    private UISprite mNextSp;
    private NoticeNumberComponent mNoticeNumber;
    [SerializeField]
    private GameObject mNoticeNumberPrefab;
    [SerializeField]
    private GameObject mNoticeNumberRoot;
    private long mOldTime;
    [SerializeField]
    private UILabel mOptionInfoLb;
    [SerializeField]
    private GameObject mOptionObj;
    [SerializeField]
    private UILabel mOptionTimeLb;
    [SerializeField]
    private GameObject mPhaseObj;
    [SerializeField]
    private UISprite[] mPhaseSp = new UISprite[PHASE_MAX];
    [SerializeField]
    private UISprite mPhaseStrSp;
    private Vector2 mPressPos;
    [SerializeField]
    private UISprite mRewardFirstSP;
    [SerializeField]
    private UISprite mRewardGetSP;
    [SerializeField]
    private ItemIconComponent mRewardIcon;
    private int mRewardIconIdx;
    private GiftEntity[] mRewardIconInfs;
    [SerializeField]
    private GameObject mRewardObj;
    public static readonly string[] msLabelMainSprNames = new string[] { string.Empty, string.Empty, "img_questtxt_free", string.Empty, string.Empty, "img_questtxt_event", string.Empty };
    public static readonly string msPhaseSprName_ArrowOff = "img_questarrow_off";
    public static readonly string msPhaseSprName_ArrowOn = "img_questarrow_on";
    public static readonly string msPhaseSprName_LoopFirstOff = "img_loop_off";
    public static readonly string msPhaseSprName_LoopOff = "img_questloop_off";
    public static readonly string msPhaseSprName_LoopOn = "img_questloop_on";
    public static readonly string[] msQBoardL1Names = new string[] { "img_questboard_event{0:D3}", "img_questboard_cap{0:D3}", "img_questboard_gate", "img_questboard_story01", "img_questboard_hero01" };
    public static readonly string msQBoardL2Name_Hero = "img_questboard_hero02";
    public static readonly string msQBoardL2Name_Story = "img_questboard_story02";
    public static readonly string[] msQBoardL3Names = new string[] { string.Empty, "img_questboard_main", "img_questboard_free", "img_questboard_story03", string.Empty, "img_questboard_free", "img_questboard_hero03" };
    [SerializeField]
    private GameObject mStatusObj;
    [SerializeField]
    private GameObject[] mStatusSp = new GameObject[3];
    [SerializeField]
    private UIAtlas mTerminalAtlas;
    [SerializeField]
    private TerminalBannerComponent[] mTerminalBanners;
    [SerializeField]
    private UILabel mTitleLevelLb;
    [SerializeField]
    private UISprite mTitleLevelSp;
    [SerializeField]
    private UISprite mTitleLevelStrSp;
    [SerializeField]
    private UILabel mTitleNameLb;
    [SerializeField]
    private GameObject mTitleObj;
    [SerializeField]
    private UILabel mTitleShortcutLb;
    private float mTouchPosDif;
    private Vector2 mTouchPosNow;
    private Vector2 mTouchPosOld;
    public static readonly int NEW_H_AREA = 0x12;
    public static readonly int NEW_H_DEFAULT = 0x19;
    public static readonly Vector3 NEW_POS_AREA = new Vector3(-194f, 27f, 0f);
    public static readonly Vector3 NEW_POS_QUEST = new Vector3(-211f, 47f, 0f);
    public static readonly Vector3 NEW_POS_QUEST_NEXT = new Vector3(-105f, 57f, 0f);
    public static readonly Vector3 NEW_POS_SHORTCUT = new Vector3(-211f, 47f, 0f);
    public static readonly int NEW_W_AREA = 0x4f;
    public static readonly int NEW_W_DEFAULT = 110;
    public static readonly float NEXT_POS_X_AREA_L = -130f;
    public static readonly float NEXT_POS_X_AREA_R = 137f;
    public static readonly float NEXT_POS_X_QUEST = -185f;
    public static readonly Color OVER_AP_COLOR = new Color(0.9019608f, 0f, 0f);
    public static readonly int PHASE_MAX = 5;
    public static readonly int POS_Y_ITVL_AREA = 0x87;
    public static readonly int POS_Y_ITVL_QUEST = 150;
    public static readonly int POS_Y_ITVL_SHORTCUT = 150;
    public static readonly string QBOARD_CAP_CLOSED = "img_questboard_cap_closed";
    public static readonly string QUEST_BOARD_ICON_SPNAME_PREFIX = "quest_board_icon_";
    public static readonly string QUEST_BOARD_MASK_SPNAME_PREFIX = "quest_board_mask_";
    public static readonly string QUEST_BOARD_SPNAME_PREFIX = "quest_board_";
    public static readonly string QUEST_EVENT_FACE_MASK_SPNAME = "img_questboard_story05mask";
    public static readonly int QUEST_FACE_MASK_SP_W = 0x7c;
    public static readonly string QUEST_MAIN_FACE_MASK_SPNAME = "img_questboard_story04mask";
    public static readonly string QUEST_STORY_FACE_MASK_SPNAME = "img_questboard_story03mask";
    public static readonly int SCRL_OBJ_DUMMY_NUM = 4;
    public static readonly int SHORTCUT_FACE_MASK_SP_W = 0x88;
    public static readonly string SHORTCUT_FACE_MASK_SPNAME = "img_questboard_story02mask";
    public static readonly int TIME_UPDATE_ITVL_SEC = 60;
    public static readonly int TITLE_LEVEL_BASE_X = 0xaf;

    private void Awake()
    {
        if (this.mEarthLineObj == null)
        {
            this.mEarthLineObj = UnityEngine.Object.Instantiate<GameObject>(this.mEarthLinePrefab);
            this.mEarthLineObj.SafeSetParent(this);
            this.mEarthLine = this.mEarthLineObj.SafeGetComponent<QuestBoardListViewEarthLine>();
            this.mEarthLine.SetupFirst(this.mEarthLineObj.GetComponent<LineRenderer>());
        }
    }

    private void callbackSweepRequest(string result)
    {
        this.mListViewItem.qbvm_info.MTerminalList.mPlayerStatus.mfInitUserData();
        this.mListViewItem.qbvm_info.callbackRequest(result);
    }

    private void ChangeNextCampaignStr()
    {
        if (this.IsCampaignStrs())
        {
            this.mCampaignStrIdx++;
            if (this.mCampaignStrIdx >= this.mCampaignStrs.Count)
            {
                this.mCampaignStrIdx = 0;
            }
            this.mOptionInfoLb.text = this.mCampaignStrs[this.mCampaignStrIdx];
        }
    }

    private void ChangeNextRewardIcon()
    {
        if (this.IsRewardIcons())
        {
            this.mRewardIconIdx++;
            if (this.mRewardIconIdx >= this.mRewardIconInfs.Length)
            {
                this.mRewardIconIdx = 0;
            }
            GiftEntity entity = this.mRewardIconInfs[this.mRewardIconIdx];
            this.mRewardIcon.SetGift((Gift.Type) entity.type, entity.objectId, -1);
        }
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
                if (<>f__am$cache79 == null)
                {
                    <>f__am$cache79 = (System.Action) (() => SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null));
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, <>f__am$cache79, -1);
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
            if (<>f__am$cache7A == null)
            {
                <>f__am$cache7A = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(str3, str4, <>f__am$cache7A, -1);
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
                    <Click_Quest>c__AnonStoreyBD ybd = new <Click_Quest>c__AnonStoreyBD {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(ybd.<>m__1BD));
                });
            }
            else if (master.CheckEquipAdd(1))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num11, entity.svtEquipKeep, true, true, delegate (ServantFrameShortDlgComponent.resultClicked result) {
                    <Click_Quest>c__AnonStoreyBE ybe = new <Click_Quest>c__AnonStoreyBE {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(ybe.<>m__1BE));
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
                int questPhase = this.mListViewItem.quest_info.mfGetQuestPhase() + 1;
                if (questPhase > this.mListViewItem.quest_info.mfGetPhaseMax())
                {
                    questPhase = this.mListViewItem.quest_info.mfGetPhaseMax();
                }
                NetworkManager.getRequest<BattleSweepRequest>(new NetworkManager.ResultCallbackFunc(this.callbackSweepRequest)).beginRequest(this.mListViewItem.quest_info.mfGetQuestID(), questPhase, entity.activeDeckId, 0L, 2);
            }
        }
    }

    public void ClickDropBtn()
    {
        if (this.mListViewItem != null)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            string message = (entity.sweepNum > 0L) ? string.Format(LocalizationManager.Get("BATTLE_DROP_TEXT"), entity.sweepNum) : LocalizationManager.Get("BATTLE_DROP_NO_COUNT");
            if (entity.sweepNum <= 0L)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog(LocalizationManager.Get("BATTLE_DROP_TITLE"), message, null, false);
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("BATTLE_DROP_TITLE"), message, LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_DECIDE"), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.EndErrorDialog));
            }
        }
    }

    private void EndErrorDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        if (isDecide)
        {
            this.Click_Quest(this.mListViewItem.quest_info);
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
            this.mListViewItem.qbvm_info.MTerminalList.mPlayerStatus.mfInitUserData();
            this.mListViewItem.qbvm_info.SetupDisp();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseApRecoverItemListDialog();
    }

    private List<string> GetCampaignStr(int quest_id, int phase, ref int ap_calc_val)
    {
        List<string> list = new List<string>();
        EventMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
        EventQuestMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventQuestMaster>(DataNameKind.Kind.EVENT_QUEST);
        EventCampaignMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventCampaignMaster>(DataNameKind.Kind.EVENT_CAMPAIGN);
        foreach (EventEntity entity in master.GetEnableEntitiyList(GameEventType.TYPE.QUEST_CAMPAIGN, true))
        {
            EventCampaignEntity entity3;
            string str;
            int num2 = entity.getEventId();
            EventQuestEntity entity2 = master2.getEntityFromId<EventQuestEntity>(num2, quest_id, phase);
            if (entity2 == null)
            {
                entity2 = master2.getEntityFromId<EventQuestEntity>(num2, quest_id, 0);
            }
            if (entity2 != null)
            {
                entity3 = master3.getData(num2);
                if (entity3 != null)
                {
                    CombineAdjustTarget.TYPE type = (CombineAdjustTarget.TYPE) entity3.getTarget();
                    switch (type)
                    {
                        case CombineAdjustTarget.TYPE.QUEST_AP:
                        case CombineAdjustTarget.TYPE.QUEST_EXP:
                        case CombineAdjustTarget.TYPE.QUEST_QP:
                        case CombineAdjustTarget.TYPE.QUEST_DROP:
                        case CombineAdjustTarget.TYPE.QUEST_EVENT_POINT:
                        case CombineAdjustTarget.TYPE.ENEMY_SVT_CALSS_PICK_UP:
                            if (type != CombineAdjustTarget.TYPE.QUEST_AP)
                            {
                                goto Label_00FC;
                            }
                            if (entity3.getCalcType() == 2)
                            {
                                goto Label_00F3;
                            }
                            break;
                    }
                }
            }
            continue;
        Label_00F3:
            ap_calc_val = entity3.getValue();
        Label_00FC:
            str = entity.getEventName();
            list.Add(str);
        }
        return list;
    }

    private static string GetRestTimeText(long end_time) => 
        (LocalizationManager.Get("TIME_REST_QUEST") + LocalizationManager.GetRestTime(end_time));

    private bool IsBanners(QuestBoardListViewItem item)
    {
        AreaBoardInfo mAreaBoardInfo = item.quest_info.mAreaBoardInfo;
        if (mAreaBoardInfo == null)
        {
            return false;
        }
        if (mAreaBoardInfo.qb_type != enQBoardL1Type.enBanner)
        {
            return false;
        }
        if (this.mBannerCount <= 1)
        {
            return false;
        }
        return true;
    }

    private bool IsCampaignStrs() => 
        (this.mCampaignStrs?.Count > 1);

    public bool IsFlickL() => 
        (this.TouchPosDif <= -FLICK_THRESHOLD);

    public bool IsFlickR() => 
        (this.TouchPosDif >= FLICK_THRESHOLD);

    private bool IsRewardIcons() => 
        (this.mRewardIconInfs?.Length > 1);

    public void LateUpdateItem(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        if ((item != null) && (item.quest_info != null))
        {
            clsMapCtrl_QuestInfo info = item.quest_info;
            if (mode != DispMode.INVISIBLE)
            {
                this.MoveBanner(item);
            }
        }
    }

    private void MoveBanner(QuestBoardListViewItem item)
    {
        if (this.IsBanners(item))
        {
            AreaBoardInfo mAreaBoardInfo = item.quest_info.mAreaBoardInfo;
            bool flag = (!this.IsPressed && !SingletonMonoBehaviour<WebViewManager>.Instance.IsBusy) && !SingletonMonoBehaviour<CommonUI>.Instance.IsActive_UserPresentBoxWindow();
            if (flag && !this.mIsEnableBannerAutoMoveOld)
            {
                this.ResetBannerAutoMoveTime();
            }
            if (flag)
            {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                float num2 = realtimeSinceStartup - this.mBannerAutoMoveTimeOld;
                if (num2 >= this.mBannerAutoMoveTimeInterval)
                {
                    for (int j = 0; j < this.mBannerCount; j++)
                    {
                        this.mTerminalBanners[j].StartAutoMoveL();
                    }
                    this.mBannerAutoMoveTimeOld = realtimeSinceStartup;
                    this.mBannerAutoMoveTimeInterval = TerminalBannerComponent.BANNER_AUTO_MOVE_TIME_INTERVAL;
                }
            }
            this.mIsEnableBannerAutoMoveOld = flag;
            int num4 = mAreaBoardInfo.banner_focus_idx;
            for (int i = 0; i < this.mBannerCount; i++)
            {
                TerminalBannerComponent component = this.mTerminalBanners[i];
                component.Move(this);
                if (component.IsFocus())
                {
                    mAreaBoardInfo.banner_focus_idx = component.Idx;
                }
            }
            if (mAreaBoardInfo.banner_focus_idx != num4)
            {
                this.UpdateDispBannerPoint(mAreaBoardInfo.banner_focus_idx);
            }
        }
    }

    public void OnChangeAlphaAnim(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        this.ChangeNextCampaignStr();
        this.ChangeNextRewardIcon();
    }

    public void OnDragStartItem(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        if (((item.quest_info != null) && this.mIsPressedForDragX) && this.IsBanners(item))
        {
            Vector2 vector = CTouch.getScreenPosition(qmanager.GetCamera());
            Vector2 vector2 = this.mPressPos - vector;
            float introduced3 = Mathf.Abs(vector2.x);
            this.mIsEnableDragX = introduced3 > Mathf.Abs(vector2.y);
            qmanager.GetScrollView().enabled = !this.mIsEnableDragX;
            this.SetDispBannerArrows(this.mIsEnableDragX, false);
        }
    }

    private void OnPressBanner(QuestBoardListViewItem item)
    {
        if (this.IsBanners(item))
        {
            for (int i = 0; i < this.mBannerCount; i++)
            {
                this.mTerminalBanners[i].OnPress(this);
            }
            this.SetDispBannerArrows(true, false);
        }
    }

    public void OnPressItem(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        if ((item != null) && (item.quest_info != null))
        {
            clsMapCtrl_QuestInfo info = item.quest_info;
            if (mode != DispMode.INVISIBLE)
            {
                this.mIsPressed = true;
                if (!qmanager.GetScrollView().IsLimitOverPosition())
                {
                    this.mPressPos = CTouch.getScreenPosition(qmanager.GetCamera());
                    this.mIsPressedForDragX = true;
                    this.OnPressBanner(item);
                }
            }
        }
    }

    private void OnPullBanner(QuestBoardListViewItem item)
    {
        if (this.IsBanners(item))
        {
            for (int i = 0; i < this.mBannerCount; i++)
            {
                this.mTerminalBanners[i].OnPull(this);
            }
            this.ResetBannerAutoMoveTime();
            this.SetDispBannerArrows(false, false);
        }
    }

    public void OnPullItem(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        if ((item != null) && (item.quest_info != null))
        {
            clsMapCtrl_QuestInfo info = item.quest_info;
            if (mode != DispMode.INVISIBLE)
            {
                this.OnPullBanner(item);
                this.mIsPressed = false;
                this.mIsPressedForDragX = false;
                this.mIsEnableDragX = false;
                qmanager.GetScrollView().enabled = true;
                this.mTouchPosDif = 0f;
            }
        }
    }

    private void ResetBannerAutoMoveTime()
    {
        this.mBannerAutoMoveTimeOld = Time.realtimeSinceStartup;
        this.mBannerAutoMoveTimeInterval = TerminalBannerComponent.BANNER_AUTO_MOVE_TIME_START;
    }

    private void SetCaldeaGate(QuestBoardListViewItem item)
    {
        if (item.quest_info.mAreaBoardInfo.qb_type == enQBoardL1Type.enCaldeaGate)
        {
            EventEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).GetEnableEntitiyList(GameEventType.TYPE.EVENT_QUEST, false);
            if (enableEntitiyList.Length > 0)
            {
                List<EventEntity> list = new List<EventEntity>();
                list.AddRange(enableEntitiyList);
                WarMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    EventEntity entity = list[i];
                    if ((master2.getByEventId(entity.getEventId()) != null) || (entity.getEventBannerID() <= 0))
                    {
                        list.RemoveAt(i);
                    }
                }
                if (list.Count > 0)
                {
                    if (<>f__am$cache78 == null)
                    {
                        <>f__am$cache78 = (a, b) => b.getEventBannerPriority() - a.getEventBannerPriority();
                    }
                    list.Sort(<>f__am$cache78);
                    string bannerName = CALDEAGATE_SPNAME_PREFIX + list[0].getEventBannerID();
                    this.mcBaseP.enabled = AtlasManager.SetBanner(this.mcBaseP, bannerName);
                }
            }
        }
    }

    private void SetCampaignStrAlpha(float alpha)
    {
        if (this.IsCampaignStrs())
        {
            this.mOptionInfoLb.alpha = alpha;
        }
    }

    private void SetDispBannerArrows(bool is_disp, bool is_force = false)
    {
        for (int i = 0; i < this.mBannerArrows.Length; i++)
        {
            this.mBannerArrows[i].SetDisp(is_disp, is_force);
        }
    }

    private bool SetFaceImage(QuestEntity qdat, UISprite mask_sp, int mask_w, string mask_sp_name, Color base_col)
    {
        bool flag = qdat.getCharaIconId() > 0;
        this.mLabelFaceObj.SetActive(flag);
        if (!flag)
        {
            return false;
        }
        this.mLabelFaceIcon.SetFaceImage(qdat.getServantId(), qdat.getLimitCount(), -1);
        this.mLabelFaceIcon.SetColor(base_col);
        mask_sp.enabled = true;
        mask_sp.color = base_col;
        if (qdat.bannerId > 0)
        {
            string bannerName = QUEST_BOARD_MASK_SPNAME_PREFIX + qdat.bannerId;
            AtlasManager.SetBanner(mask_sp, bannerName);
            mask_sp_name = bannerName;
        }
        else
        {
            mask_sp.atlas = this.mTerminalAtlas;
            mask_sp.spriteName = mask_sp_name;
        }
        UISpriteData sprite = mask_sp.atlas.GetSprite(mask_sp_name);
        mask_sp.enabled = sprite != null;
        if (mask_sp.enabled)
        {
            sprite.width = mask_w - 1;
        }
        return true;
    }

    public void SetInput(QuestBoardListViewItem item, bool isInput)
    {
    }

    public void SetItem(QuestBoardListViewItem item, DispMode disp_mode)
    {
        if (item != null)
        {
            this.mListViewItem = item;
            QuestBoardListViewItem.InfoKind kind = item.info_kind;
            clsMapCtrl_QuestInfo info = item.quest_info;
            if (info == null)
            {
                base.gameObject.SetLocalScale(Vector3.zero);
            }
            else if ((kind != QuestBoardListViewItem.InfoKind.AREA) && (info.mfGetMine() == null))
            {
                base.gameObject.SetLocalScale(Vector3.zero);
            }
            else if (disp_mode != DispMode.INVISIBLE)
            {
                base.gameObject.SetLocalScale(Vector3.one);
                this.mEarthLine.Hide();
                this.mcBaseP.gameObject.SetActive(true);
                this.mcBaseP.enabled = true;
                this.mStatusObj.SetActive(true);
                foreach (GameObject obj2 in this.mStatusSp)
                {
                    obj2.SetActive(false);
                }
                this.mBannerRoot.SetActive(false);
                this.mNoticeNumberRoot.SetActive(false);
                this.mNextSp.gameObject.SetActive(false);
                this.mClassIconRoot.SetActive(false);
                switch (kind)
                {
                    case QuestBoardListViewItem.InfoKind.AREA:
                        this.SetItem_Area(item);
                        break;

                    case QuestBoardListViewItem.InfoKind.MAP:
                    case QuestBoardListViewItem.InfoKind.CALDEA:
                        this.SetItem_Quest(item);
                        break;

                    case QuestBoardListViewItem.InfoKind.STORY:
                    case QuestBoardListViewItem.InfoKind.HERO:
                        this.SetItem_Shortcut(item);
                        break;
                }
            }
        }
    }

    private void SetItem_Area(QuestBoardListViewItem item)
    {
        clsMapCtrl_QuestInfo info = item.quest_info;
        AreaBoardInfo mAreaBoardInfo = info.mAreaBoardInfo;
        int num = mAreaBoardInfo.etc_id;
        EventEntity entity = mAreaBoardInfo.ev_dat;
        bool flag = (mAreaBoardInfo.qb_type == enQBoardL1Type.enCapter) && (entity != null);
        bool flag2 = false;
        long num2 = NetworkManager.getTime();
        bool flag3 = mAreaBoardInfo.qb_type == enQBoardL1Type.enBanner;
        this.mLabelObj.SetActive(false);
        this.mTitleObj.SetActive(false);
        this.mOptionObj.SetActive(true);
        this.mOptionInfoLb.gameObject.SetActive(false);
        this.mRewardObj.SetActive(false);
        this.mPhaseObj.SetActive(false);
        this.mApObj.SetActive(false);
        this.mcBaseP.gameObject.SetActive(!flag3);
        if (flag3)
        {
            this.SetupBanner(item);
        }
        else
        {
            string bannerName = string.Empty;
            if (flag)
            {
                int noticeBannerId = entity.noticeBannerId;
                flag2 = SingletonTemplate<QuestTree>.Instance.IsActiveEventWar(num);
                if (flag2)
                {
                    noticeBannerId = entity.bannerId;
                }
                bannerName = EVENT_WAR_SPNAME_PREFIX + noticeBannerId;
                AtlasManager.SetBanner(this.mcBaseP, bannerName);
            }
            else
            {
                this.mcBaseP.atlas = this.mTerminalAtlas;
                if ((mAreaBoardInfo.qb_type == enQBoardL1Type.enCapter) && (num == 0))
                {
                    bannerName = QBOARD_CAP_CLOSED;
                }
                else if (mAreaBoardInfo.status_id == 1)
                {
                    bannerName = string.Format(msQBoardL1Names[((int) mAreaBoardInfo.qb_type) - 1], num + "b");
                }
                else
                {
                    bannerName = string.Format(msQBoardL1Names[((int) mAreaBoardInfo.qb_type) - 1], num);
                }
            }
            this.mcBaseP.spriteName = bannerName;
            this.SetCaldeaGate(item);
            if (mAreaBoardInfo.status_id != 1)
            {
                this.mNoticeNumberRoot.SetActive(true);
            }
            else
            {
                this.mNoticeNumberRoot.SetActive(false);
            }
            if (this.mNoticeNumber == null)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
                obj2.SafeSetParent(this.mNoticeNumberRoot);
                this.mNoticeNumber = obj2.GetComponent<NoticeNumberComponent>();
            }
            int number = mAreaBoardInfo.quest_count;
            if (flag && !flag2)
            {
                number = 0;
            }
            this.mNoticeNumber.SetNumber(number);
        }
        this.mcBaseP.MakePixelPerfect();
        this.mcBaseP.gameObject.GetComponent<UIWidget>().color = Color.white;
        if (mAreaBoardInfo.status != enStatus.enNone)
        {
            int index = ((int) mAreaBoardInfo.status) - 1;
            bool flag4 = true;
            if (((mAreaBoardInfo.status == enStatus.enNew) && flag) && !flag2)
            {
                flag4 = false;
            }
            this.mStatusSp[index].SetActive(flag4);
        }
        if (this.mStatusSp[0].activeSelf && (mAreaBoardInfo.status_id != 1))
        {
            this.mNewIcon.Set();
            this.mNewIcon.gameObject.SetLocalPosition(NEW_POS_AREA);
            UIWidget component = this.mNewIcon.gameObject.GetComponent<UIWidget>();
            component.width = NEW_W_AREA;
            component.height = NEW_H_AREA;
        }
        else
        {
            this.mNewIcon.Clear();
        }
        GameObject self = this.mStatusSp[1];
        if (self.activeSelf)
        {
            self.SetLocalPosition(CLEAR_POS_AREA);
            if (!flag && ((num & 1) == 0))
            {
                self.SetLocalPositionX(-CLEAR_POS_AREA.x);
            }
        }
        this.mNextSp.gameObject.SetActive(mAreaBoardInfo.is_next && (mAreaBoardInfo.status_id != 1));
        if (mAreaBoardInfo.is_next)
        {
            TweenPosition position = this.mNextSp.gameObject.GetComponent<TweenPosition>();
            if ((num & 1) == 0)
            {
                position.from.x = NEXT_POS_X_AREA_R;
            }
            else
            {
                position.from.x = NEXT_POS_X_AREA_L;
            }
            position.to.x = position.from.x;
        }
        bool flag5 = (flag && (num2 >= entity.getEventStartedAt())) && (num2 < entity.getEventEndedAt());
        if (flag5)
        {
            long endTime = info.GetEndTime();
            this.mOptionTimeLb.text = GetRestTimeText(endTime);
            this.mOldTime = 0L;
        }
        this.mOptionTimeLb.transform.parent.gameObject.SetActive(flag5);
        this.mEarthLine.SetupSecond(LINE_ST_POS_AREA, this.mListViewObject, num);
    }

    private void SetItem_Quest(QuestBoardListViewItem item)
    {
        clsMapCtrl_QuestInfo info = item.quest_info;
        QuestEntity qdat = info.mfGetMine();
        int index = (int) info.mfGetQuestType();
        bool flag = info.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Closed;
        Color color = !flag ? Color.white : Color.gray;
        this.mLabelObj.SetActive(true);
        this.mLabelNameSp.gameObject.SetActive((index == 2) || (index == 5));
        this.mLabelFaceMaskShortcutSp.gameObject.SetActive(false);
        this.mLabelFaceMaskQuestSp.gameObject.SetActive(true);
        this.mTitleObj.SetActive(true);
        this.mTitleLevelStrSp.gameObject.SetActive(true);
        this.mTitleLevelSp.gameObject.SetActive(true);
        this.mTitleLevelLb.gameObject.SetActive(true);
        this.mTitleShortcutLb.gameObject.SetActive(false);
        this.mOptionObj.SetActive(true);
        this.mRewardObj.SetActive(true);
        this.mPhaseObj.SetActive(true);
        this.mApObj.SetActive(true);
        if (qdat.bannerId > 0)
        {
            string bannerName = QUEST_BOARD_SPNAME_PREFIX + qdat.bannerId;
            this.mcBaseP.enabled = AtlasManager.SetBanner(this.mcBaseP, bannerName);
        }
        else
        {
            this.mcBaseP.atlas = this.mTerminalAtlas;
            this.mcBaseP.spriteName = msQBoardL3Names[index];
        }
        this.mcBaseP.MakePixelPerfect();
        this.mcBaseP.gameObject.GetComponent<UIWidget>().color = color;
        if (((info.mfGetQuestType() == QuestEntity.enType.MAIN) && (info.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Normal)) && !SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false))
        {
            this.mNextSp.gameObject.SetActive(true);
            this.mNextSp.gameObject.SetLocalPositionX(NEXT_POS_X_QUEST);
            TweenPosition position = this.mNextSp.gameObject.GetComponent<TweenPosition>();
            position.from.x = NEXT_POS_X_QUEST;
            position.to.x = position.from.x;
        }
        if (!flag)
        {
            int num2 = -1;
            if (info.mfIsNew())
            {
                num2 = 0;
            }
            else if (SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false))
            {
                num2 = 1;
            }
            if (num2 >= 0)
            {
                this.mStatusSp[num2].gameObject.SetActive(true);
            }
        }
        if (this.mStatusSp[0].activeSelf)
        {
            this.mNewIcon.Set();
            Vector3 v = !this.mNextSp.gameObject.activeSelf ? NEW_POS_QUEST : NEW_POS_QUEST_NEXT;
            this.mNewIcon.gameObject.SetLocalPosition(v);
            UIWidget widget = this.mNewIcon.gameObject.GetComponent<UIWidget>();
            widget.width = NEW_W_DEFAULT;
            widget.height = NEW_H_DEFAULT;
        }
        else
        {
            this.mNewIcon.Clear();
        }
        GameObject self = this.mStatusSp[1];
        if (self.activeSelf)
        {
            self.SetLocalPosition(CLEAR_POS_QUEST);
        }
        this.mTitleNameLb.text = qdat.getQuestName();
        if (flag && (info.QuestReleaseClosedID > 0))
        {
            ClosedMessageEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ClosedMessageMaster>(DataNameKind.Kind.CLOSED_MESSAGE).getEntityFromId<ClosedMessageEntity>(info.QuestReleaseClosedID);
            string format = string.Empty;
            if (entity2 != null)
            {
                format = entity2.message;
            }
            string str3 = string.Empty;
            CondType.Kind questReleaseType = info.QuestReleaseType;
            switch (questReleaseType)
            {
                case CondType.Kind.SVT_LEVEL:
                case CondType.Kind.SVT_LIMIT:
                case CondType.Kind.SVT_FRIENDSHIP:
                    str3 = string.Format(format, info.QuestReleaseValue);
                    break;

                case CondType.Kind.DATE:
                {
                    DateTime time = NetworkManager.getLocalDateTime(qdat.getOpenedAt());
                    str3 = string.Format(format, new object[] { time.Month, time.Day, time.Hour, time.Minute });
                    break;
                }
                default:
                    if (questReleaseType == CondType.Kind.QUEST_CLEAR)
                    {
                        int id = SingletonTemplate<QuestTree>.Instance.GetWarID_ByQuestID(info.QuestReleaseTargetID);
                        WarEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(id);
                        if (entity3 != null)
                        {
                            str3 = string.Format(format, entity3.name);
                        }
                    }
                    else
                    {
                        str3 = format;
                    }
                    break;
            }
            this.mTitleNameLb.text = str3;
        }
        this.mTitleNameLb.color = color;
        this.mTitleLevelLb.text = string.Empty + qdat.getRecommendLv();
        this.mTitleLevelLb.color = color;
        this.mTitleLevelSp.color = color;
        this.mTitleLevelStrSp.color = color;
        this.mTitleLevelSp.gameObject.SetLocalPositionX((float) TITLE_LEVEL_BASE_X);
        this.mTitleLevelSp.gameObject.AddLocalPositionX((float) (-this.mTitleLevelLb.width / 2));
        bool flag2 = false;
        string str4 = string.Empty;
        switch (((QuestEntity.enType) index))
        {
            case QuestEntity.enType.MAIN:
                str4 = QUEST_MAIN_FACE_MASK_SPNAME;
                break;

            case QuestEntity.enType.FREE:
            case QuestEntity.enType.EVENT:
                str4 = QUEST_EVENT_FACE_MASK_SPNAME;
                break;

            case QuestEntity.enType.FRIENDSHIP:
                str4 = QUEST_STORY_FACE_MASK_SPNAME;
                break;
        }
        flag2 = this.SetFaceImage(qdat, this.mLabelFaceMaskQuestSp, QUEST_FACE_MASK_SP_W, str4, color);
        this.mLabelNameSp.gameObject.SetActive(!flag2 && (index != 1));
        if (this.mLabelNameSp.gameObject.activeSelf)
        {
            this.mLabelNameSp.enabled = true;
            if (qdat.iconId > 0)
            {
                string str5 = QUEST_BOARD_ICON_SPNAME_PREFIX + qdat.iconId;
                this.mLabelNameSp.enabled = AtlasManager.SetBanner(this.mLabelNameSp, str5);
            }
            else
            {
                this.mLabelNameSp.atlas = this.mTerminalAtlas;
                this.mLabelNameSp.spriteName = msLabelMainSprNames[index];
            }
            this.mLabelNameSp.MakePixelPerfect();
            this.mLabelNameSp.gameObject.GetComponent<UIWidget>().color = color;
        }
        this.mLabelChapterSp.gameObject.SetActive((!flag2 && !flag) && (index == 1));
        if (this.mLabelChapterSp.gameObject.activeSelf)
        {
            this.mLabelChapterSp.enabled = true;
            int num4 = qdat.getChapterId();
            int num5 = qdat.getChapterSubId();
            bool flag3 = qdat.iconId <= 0;
            if (flag3)
            {
                this.mLabelChapterSp.atlas = this.mTerminalAtlas;
                this.mLabelChapterSp.spriteName = string.Format(CHAPTER_SP_BASE_NAME, num4);
                this.mLabelSubSectionNumLb.text = string.Empty + num5;
            }
            else
            {
                string str6 = QUEST_BOARD_ICON_SPNAME_PREFIX + qdat.iconId;
                this.mLabelChapterSp.enabled = AtlasManager.SetBanner(this.mLabelChapterSp, str6);
            }
            this.mLabelChapterSp.MakePixelPerfect();
            this.mLabelSubSectionNumLb.gameObject.SetActive(flag3);
        }
        this.mRewardIconInfs = null;
        this.mRewardIconIdx = 0;
        this.mRewardIcon.gameObject.SetActive(false);
        if (qdat.getGiftIconId() > 0)
        {
            this.mRewardIcon.SetItemImage((ImageItem.Id) qdat.getGiftIconId());
            this.mRewardIcon.gameObject.SetActive(true);
        }
        else
        {
            GiftEntity[] giftListById = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT).GetGiftListById(qdat.giftId);
            if (giftListById.Length > 0)
            {
                GiftEntity entity4 = giftListById[0];
                this.mRewardIcon.SetGift((Gift.Type) entity4.type, entity4.objectId, -1);
                this.mRewardIcon.gameObject.SetActive(true);
            }
            this.mRewardIconInfs = giftListById;
        }
        this.mRewardObj.GetComponent<UIWidget>().color = color;
        this.mRewardIcon.SetColor(color);
        bool flag4 = (qdat.getAfterClear() == 3) && !SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false);
        this.mRewardFirstSP.gameObject.SetActive(flag4);
        this.mRewardFirstSP.color = color;
        bool flag5 = SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(info.mfGetQuestID(), false);
        this.mRewardGetSP.gameObject.SetActive(flag5);
        this.mRewardGetSP.color = color;
        bool flag6 = !flag && (qdat.getAfterClear() == 2);
        for (int i = 0; i < PHASE_MAX; i++)
        {
            bool flag7 = i < info.mfGetPhaseMax();
            UISprite sprite = this.mPhaseSp[i];
            sprite.gameObject.SetActive(flag7);
            if (flag7)
            {
                bool flag8 = i < info.mfGetQuestPhase();
                bool flag9 = (i == (info.mfGetPhaseMax() - 1)) && ((qdat.getAfterClear() == 3) || (qdat.getAfterClear() == 4));
                if (flag8)
                {
                    sprite.spriteName = !flag9 ? msPhaseSprName_ArrowOn : msPhaseSprName_LoopOn;
                }
                else
                {
                    sprite.spriteName = !flag9 ? msPhaseSprName_ArrowOff : msPhaseSprName_LoopOff;
                }
                if ((i == (info.mfGetPhaseMax() - 1)) && flag6)
                {
                    sprite.spriteName = msPhaseSprName_LoopFirstOff;
                }
                sprite.MakePixelPerfect();
            }
        }
        int num7 = 0;
        bool flag10 = false;
        if (!flag)
        {
            int phase = Mathf.Min(info.mfGetQuestPhase() + 1, info.mfGetPhaseMax());
            this.mCampaignStrs = this.GetCampaignStr(qdat.getQuestId(), phase, ref num7);
            if (this.mCampaignStrs.Count > 0)
            {
                this.mCampaignStrIdx = 0;
                this.mOptionInfoLb.text = this.mCampaignStrs[this.mCampaignStrIdx];
                this.mOptionInfoLb.alpha = 1f;
                info.SetApCalcVal(num7);
                flag10 = true;
            }
        }
        this.mOptionInfoLb.transform.parent.gameObject.SetActive(flag10);
        int num9 = qdat.getActConsume(num7);
        this.mApCostLb.text = string.Empty + num9;
        this.mApCostLb.color = (num7 <= 0) ? color : CAMPAIGN_AP_COLOR;
        UserGameEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int num10 = entity5.getActMax();
        int num11 = entity5.getAct();
        if ((num9 > num10) || (num9 > num11))
        {
            Color color2 = OVER_AP_COLOR;
            if (flag)
            {
                color2 = (Color) (color2 * 0.5f);
            }
            this.mApCostLb.color = color2;
        }
        long endTime = info.GetEndTime();
        bool flag11 = endTime > 0L;
        this.mOptionTimeLb.transform.parent.gameObject.SetActive(flag11);
        this.mOptionTimeLb.text = GetRestTimeText(endTime);
        this.mOptionTimeLb.color = color;
        this.mOldTime = 0L;
        int num13 = info.mfGetQuestPhase() + 1;
        if (num13 > info.mfGetPhaseMax())
        {
            num13 = info.mfGetPhaseMax();
        }
        QuestPhaseEntity entity6 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestPhaseMaster>(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(qdat.getQuestId(), num13);
        bool flag12 = ((entity6 != null) && (entity6.classIds != null)) && (entity6.classIds.Length > 0);
        this.mClassIconRoot.SetActive(flag12);
        this.mClassStrSp.color = color;
        if (flag12)
        {
            int[] classIds = entity6.classIds;
            for (int j = 0; j < this.mClassIcons.Length; j++)
            {
                ServantClassIconComponent component = this.mClassIcons[j];
                component.gameObject.SetActive(j < classIds.Length);
                if (component.gameObject.activeSelf)
                {
                    component.Set(classIds[j]);
                    component.SetColor(color);
                }
            }
        }
    }

    private void SetItem_Shortcut(QuestBoardListViewItem item)
    {
        QuestBoardListViewItem.InfoKind kind = item.info_kind;
        clsMapCtrl_QuestInfo info = item.quest_info;
        QuestEntity qdat = info.mfGetMine();
        int num = (int) info.mfGetQuestType();
        Color white = Color.white;
        this.mLabelObj.SetActive(true);
        this.mLabelNameSp.gameObject.SetActive(false);
        this.mLabelChapterSp.gameObject.SetActive(false);
        this.mLabelFaceMaskShortcutSp.gameObject.SetActive(true);
        this.mLabelFaceMaskQuestSp.gameObject.SetActive(false);
        this.mTitleObj.SetActive(true);
        this.mTitleLevelStrSp.gameObject.SetActive(false);
        this.mTitleLevelSp.gameObject.SetActive(false);
        this.mTitleLevelLb.gameObject.SetActive(false);
        this.mTitleShortcutLb.gameObject.SetActive(true);
        this.mOptionObj.SetActive(false);
        this.mRewardObj.SetActive(false);
        this.mPhaseObj.SetActive(false);
        this.mApObj.SetActive(false);
        this.mcBaseP.atlas = this.mTerminalAtlas;
        this.mcBaseP.spriteName = (kind != QuestBoardListViewItem.InfoKind.STORY) ? msQBoardL2Name_Hero : msQBoardL2Name_Story;
        this.mcBaseP.MakePixelPerfect();
        this.mcBaseP.gameObject.GetComponent<UIWidget>().color = white;
        if (info.mfIsNew())
        {
            int index = 0;
            this.mStatusSp[index].SetActive(true);
            this.mNewIcon.Set();
            this.mNewIcon.gameObject.SetLocalPosition(NEW_POS_SHORTCUT);
            UIWidget component = this.mNewIcon.gameObject.GetComponent<UIWidget>();
            component.width = NEW_W_DEFAULT;
            component.height = NEW_H_DEFAULT;
        }
        else
        {
            this.mNewIcon.Clear();
        }
        GameObject self = this.mStatusSp[1];
        if (self.activeSelf)
        {
            self.SetLocalPosition(CLEAR_POS_QUEST);
        }
        this.mTitleNameLb.text = qdat.getQuestName();
        this.mTitleNameLb.color = white;
        int id = qdat.getServantId();
        ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(id);
        if (entity2 != null)
        {
            this.mTitleShortcutLb.text = entity2.name;
        }
        else
        {
            this.mTitleShortcutLb.text = string.Empty;
        }
        this.SetFaceImage(qdat, this.mLabelFaceMaskShortcutSp, SHORTCUT_FACE_MASK_SP_W, SHORTCUT_FACE_MASK_SPNAME, white);
        this.mEarthLine.SetupSecond(LINE_ST_POS_SHORTCUT, this.mListViewObject, info.mfGetWarID());
    }

    public void SetListViewObject(QuestBoardListViewObject lvo)
    {
        this.mListViewObject = lvo;
    }

    private void SetRewardIconAlpha(float alpha)
    {
        if (this.IsRewardIcons())
        {
            this.mRewardIcon.SetAlpha(alpha);
        }
    }

    private void SetupBanner(QuestBoardListViewItem item)
    {
        AreaBoardInfo mAreaBoardInfo = item.quest_info.mAreaBoardInfo;
        BannerEntity[] entityArray = mAreaBoardInfo.banner_ents;
        this.mBannerCount = entityArray.Length;
        if (this.mBannerCount > this.mTerminalBanners.Length)
        {
            this.mBannerCount = this.mTerminalBanners.Length;
        }
        this.mBannerRoot.SetActive(true);
        for (int i = 0; i < this.mTerminalBanners.Length; i++)
        {
            TerminalBannerComponent component = this.mTerminalBanners[i];
            TerminalBannerComponent component2 = null;
            if ((i + 1) < this.mBannerCount)
            {
                component2 = this.mTerminalBanners[i + 1];
            }
            component.gameObject.SetActive(i < this.mBannerCount);
            if (component.gameObject.activeSelf)
            {
                component.Setup(i, entityArray[i].bannerId, this.mBannerCount, component2);
                component.StartLoadAndDisp();
            }
        }
        this.ResetBannerAutoMoveTime();
        mAreaBoardInfo.banner_focus_idx = 0;
        this.mIsEnableBannerAutoMoveOld = false;
        bool flag = this.mBannerCount >= 2;
        this.mBannerArrowObj.SetActive(flag);
        if (flag)
        {
            this.SetDispBannerArrows(false, true);
        }
        this.mBannerPointGrid.gameObject.SetActive(flag);
        if (flag)
        {
            for (int j = 0; j < this.mBannerPointSps.Length; j++)
            {
                this.mBannerPointSps[j].gameObject.SetActive(j < this.mBannerCount);
            }
            this.mBannerPointGrid.Reposition();
        }
        this.UpdateDispBannerPoint(0);
    }

    private void UpdateDispBannerPoint(int pos_idx = 0)
    {
        for (int i = 0; i < this.mBannerCount; i++)
        {
            this.mBannerPointSps[i].spriteName = (i != pos_idx) ? BANNER_POINT_SPNAME_OFF : BANNER_POINT_SPNAME_ON;
        }
    }

    public void UpdateItem(QuestBoardListViewItem item, DispMode mode, QuestBoardListViewManager qmanager)
    {
        if ((item != null) && (item.quest_info != null))
        {
            clsMapCtrl_QuestInfo info = item.quest_info;
            if (mode != DispMode.INVISIBLE)
            {
                if (this.mOptionObj.activeSelf && this.mOptionTimeLb.transform.parent.gameObject.activeSelf)
                {
                    long num = NetworkManager.getTime();
                    long num2 = num - this.mOldTime;
                    if (num2 >= TIME_UPDATE_ITVL_SEC)
                    {
                        long endTime = info.GetEndTime();
                        this.mOptionTimeLb.text = GetRestTimeText(endTime);
                        this.mOldTime = num;
                    }
                }
                this.SetCampaignStrAlpha(qmanager.AlphaAnimNow);
                this.SetRewardIconAlpha(qmanager.AlphaAnimNow);
                this.UpdateTouch(qmanager);
            }
        }
    }

    private void UpdateTouch(QuestBoardListViewManager qmanager)
    {
        if (this.mIsPressedForDragX)
        {
            this.mTouchPosOld = this.mTouchPosNow;
            this.mTouchPosNow = CTouch.getScreenPosition(qmanager.GetCamera());
            if (this.mIsEnableDragX)
            {
                this.mTouchPosDif = this.mTouchPosNow.x - this.mTouchPosOld.x;
            }
        }
    }

    public bool IsEnableDragX =>
        this.mIsEnableDragX;

    public bool IsPressed =>
        this.mIsPressed;

    public bool IsPressedForDragX =>
        this.mIsPressedForDragX;

    public float TouchPosDif =>
        this.mTouchPosDif;

    [CompilerGenerated]
    private sealed class <Click_Quest>c__AnonStoreyBD
    {
        internal QuestBoardListViewItemDraw <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1BD()
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
    private sealed class <Click_Quest>c__AnonStoreyBE
    {
        internal QuestBoardListViewItemDraw <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1BE()
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

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID
    }

    public enum enQBoardL1Type
    {
        enNone,
        enBanner,
        enCapter,
        enCaldeaGate,
        enStory,
        enHeroBallad,
        enMAX
    }

    public enum enStatus
    {
        enNone,
        enNew,
        enClear,
        enComplete,
        enMAX
    }
}

