using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TitleInfoControl : MonoBehaviour
{
    private static readonly Dictionary<TitleKind, TutorialFlag.ImageId> _helpDispImageList;
    protected static readonly TitleKind[] _helpEnableKind;
    [CompilerGenerated]
    private static Comparison<int> <>f__am$cache4A;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4B;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4C;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4D;
    protected PlayMakerFSM activeSceneFSM;
    public static readonly float BACK_BTN_MV_SPD_TIME = 0.125f;
    public static readonly float BACK_BTN_SP_BASE_X = 15f;
    public static readonly float BACK_BTN_SP_TERMINAL_X = 2f;
    public GameObject backBtn;
    public UISprite backBtnBgSprite;
    public UISprite backBtnSprite;
    public static readonly float BASE_Y = 240f;
    [SerializeField]
    private UIAtlas commonAtlas;
    public static readonly float EVENT_ALPHA_ANIM_SPD_RATE = 0.5f;
    public static readonly float EVENT_ALPHA_ANIM_TIME_INTERVAL = 3f;
    private const float EVENT_BTN_NOTICE_NUMBER_SCALE = 0.75f;
    private const int EVENT_BTN_NOTICE_NUMBER_X = 0x3b;
    private const int EVENT_BTN_NOTICE_NUMBER_Y = 0x10;
    private static readonly string EVENT_BTN_REWARD_SPNAME = "btn_event_reward";
    private static readonly string EVENT_BTN_SHOP_SPNAME = "btn_event_trade";
    public static readonly float FRAME_IN_OUT_MV_SPD_TIME = 0.25f;
    public static readonly float FRAME_OUT_POS_Y = (BASE_Y + 100f);
    public UISprite headerBgImg;
    [SerializeField]
    private GameObject helpBtn;
    private System.Action mAdditionalCallbackOnBossAnim;
    private static readonly string MAP_HEADER_BG_DEFAULT_SPNAME = "img_titlebg_clear";
    private static readonly string MAP_HEADER_BG_SPNAME_PREFIX = "map_header_bg_";
    private GameObject mBlockMask;
    [SerializeField]
    private Transform mBlockMaskParent;
    [SerializeField]
    private GameObject mBlockMaskPrefab;
    [SerializeField]
    private UIGrid mBossStatusGrid;
    private List<EventSuperBossEntity> mCurrentBossesList;
    [SerializeField]
    private BoxCollider mDebugBtn;
    private System.Action mDebugBtnAct;
    [SerializeField]
    private UIWidget mEventAlphaAnimRoot;
    private float mEventAlphaAnimTgt;
    private float mEventAlphaAnimTimeOld;
    private EventBannerWindow mEventBannerWindow;
    [SerializeField]
    private GameObject mEventBannerWindowPrefab;
    [SerializeField]
    private UISprite mEventBgSp;
    private long mEventEndTime;
    private List<EventEndTimeInfo> mEventEndTimeInfs = new List<EventEndTimeInfo>();
    private int mEventInfoGroupIdx;
    private List<TitleInfoEventInfoComponent> mEventInfoGroups = new List<TitleInfoEventInfoComponent>();
    [SerializeField]
    private UISprite mEventItemEventBtn;
    private int mEventItemGroupIdx;
    private List<TitleInfoEventItemComponent> mEventItemGroups = new List<TitleInfoEventItemComponent>();
    [SerializeField]
    private UILabel mEventRestTimeLb;
    private long mEventRestTimeOld;
    [SerializeField]
    private GameObject mEventRoot;
    private bool mIsActiveEventItem;
    private bool mIsDoneSetEventItem;
    private bool mIsEventItemBgBlack;
    private bool mIsMovingFrameInOut;
    private bool mIsPauseEventAlphaAnim;
    private NoticeNumberComponent mNoticeNumber;
    private int mOnReleaseEventAlphaAnimFrameCount;
    private List<TitleInfoEventRaidStatusComponent> mRaidStatusList = new List<TitleInfoEventRaidStatusComponent>();
    [SerializeField]
    private TitleInfoEventRaidStatusComponent mRaidStatusPrefab;
    private int mShopEventId;
    private int mSuperBossAnimIdx;
    private List<TitleInfoSuperBossStatusComponent> mSuperBossStatusList = new List<TitleInfoSuperBossStatusComponent>();
    [SerializeField]
    private TitleInfoSuperBossStatusComponent mSuperBossStatusPrefab;
    [SerializeField]
    private GameObject mTitleInfoEventItemPrefab;
    [SerializeField]
    private GameObject mTitleInfoEventRaidBossPrefab;
    [SerializeField]
    private GameObject mTitleInfoEventSuperBossPrefab;
    private int mWarEventId;
    private int mWarId;
    [SerializeField]
    private NoticeNumberComponent noticeNumberPrefab;
    private TitleKind nowTitleKind;
    public static readonly float PARTICLE_Y = -236f;
    protected GameObject particleObj;
    [SerializeField]
    protected int particlePanelDepth = 0x73;
    [SerializeField]
    private GameObject particlePrefab;
    [SerializeField]
    private UIAtlas terminalAtlas;
    public static readonly int TIME_UPDATE_ITVL_SEC = 60;
    private static readonly Dictionary<TitleKind, int> titleDepthList;
    public UISprite titleImg;
    private static readonly Dictionary<TitleKind, string> titleNameList;

    static TitleInfoControl()
    {
        Dictionary<TitleKind, string> dictionary = new Dictionary<TitleKind, string> {
            { 
                TitleKind.NONE,
                null
            },
            { 
                TitleKind.TERMINAL,
                "img_txt_terminal"
            },
            { 
                TitleKind.HEROICTALE,
                "img_txt_heroictale"
            },
            { 
                TitleKind.STORY,
                "img_txt_story"
            },
            { 
                TitleKind.CALDEAGATE,
                "img_txt_cgate"
            },
            { 
                TitleKind.WAR100,
                "img_txt_cap00"
            },
            { 
                TitleKind.WAR101,
                "img_txt_cap01"
            },
            { 
                TitleKind.WAR102,
                "img_txt_cap02"
            },
            { 
                TitleKind.WAR103,
                "img_txt_cap03"
            },
            { 
                TitleKind.WAR104,
                "img_txt_cap04"
            },
            { 
                TitleKind.WAR105,
                "img_txt_cap05"
            },
            { 
                TitleKind.WAR106,
                "img_txt_cap06"
            },
            { 
                TitleKind.WAR107,
                "img_txt_cap07"
            },
            { 
                TitleKind.FORMATION,
                "img_txt_form_menu"
            },
            { 
                TitleKind.FORM_MASTER,
                "img_txt_form_master"
            },
            { 
                TitleKind.PARTY_CHECK,
                "img_txt_form_check"
            },
            { 
                TitleKind.FORM_SVT_LIST,
                "img_txt_form_saintgraf"
            },
            { 
                TitleKind.FORM_SVT_EQUIP_LIST,
                "img_txt_form_servantequip"
            },
            { 
                TitleKind.FORM_SVT_EQUIP_SELECT,
                "img_txt_form_servantequip_select"
            },
            { 
                TitleKind.COMBINE,
                "img_txt_combine"
            },
            { 
                TitleKind.SERVANT_COMBINE,
                "img_txt_sv"
            },
            { 
                TitleKind.SKILL_COMBINE,
                "img_txt_skill"
            },
            { 
                TitleKind.NP_COMBINE,
                "img_txt_np"
            },
            { 
                TitleKind.LIMIT_COMBINE,
                "img_txt_limit"
            },
            { 
                TitleKind.SVT_EQUIP_COMBINE,
                "img_txt_servantequip"
            },
            { 
                TitleKind.SUMMON,
                "img_title_summon"
            },
            { 
                TitleKind.SHOP,
                "img_txt_shop"
            },
            { 
                TitleKind.SHOP_SELL_SERVANT,
                "img_txt_sgsale"
            },
            { 
                TitleKind.FRIEND,
                "img_txt_friend"
            },
            { 
                TitleKind.MYROOM,
                "img_txt_myroom"
            },
            { 
                TitleKind.MYROOM_MATERIAL,
                "img_txt_material"
            },
            { 
                TitleKind.PRESENTBOX,
                "img_txt_presentbox"
            },
            { 
                TitleKind.MYROOM_USERITEM,
                "img_txt_itemscarried"
            },
            { 
                TitleKind.PROFILE,
                "img_txt_masterprofile"
            },
            { 
                TitleKind.GAMEOPTION,
                "img_txt_gameoption"
            },
            { 
                TitleKind.MYROOM_NOTICE,
                "img_txt_information"
            },
            { 
                TitleKind.SERIAL_CODE,
                "img_txt_serialcode"
            },
            { 
                TitleKind.CONTINUE_DEVICE,
                "img_txt_takeoverid"
            },
            { 
                TitleKind.FAVORITE_CHANGE,
                "img_txt_favorite"
            },
            { 
                TitleKind.SERVANT_PROFILE,
                "img_txt_sprofile"
            },
            { 
                TitleKind.BATTLE_CHARACTER,
                "img_txt_battlecharacter"
            },
            { 
                TitleKind.FOLLOWER,
                "img_txt_selectsupport"
            },
            { 
                TitleKind.BATTLE_SETUP_CONFIRM,
                "img_txt_form_check"
            },
            { 
                TitleKind.ORGANIZATION,
                "img_txt_form_menu"
            },
            { 
                TitleKind.MASTER_ORGANIZATION,
                "img_txt_form_master"
            },
            { 
                TitleKind.PARTY_ORGANIZATION,
                "img_txt_form_swap"
            },
            { 
                TitleKind.PARTY_ORGANIZATION_CHANGE,
                "img_txt_form_swap"
            },
            { 
                TitleKind.PARTY_ORGANIZATION_SWAP,
                "img_txt_form_change"
            },
            { 
                TitleKind.PARTY_ORGANIZATION_SERVANT,
                "img_txt_form_servant_select"
            },
            { 
                TitleKind.EVENT_REWARD,
                "img"
            },
            { 
                TitleKind.EVENT_BOX_GACHA,
                "img"
            },
            { 
                TitleKind.EVENT_MISSION,
                "img"
            },
            { 
                TitleKind.SUPPORT_SELECT,
                "img_txt_supportformation"
            },
            { 
                TitleKind.FRIEND_SUPPORT_INFO,
                "img_txt_supportformcheck"
            }
        };
        titleNameList = dictionary;
        Dictionary<TitleKind, int> dictionary2 = new Dictionary<TitleKind, int> {
            { 
                TitleKind.NONE,
                40
            },
            { 
                TitleKind.TERMINAL,
                14
            },
            { 
                TitleKind.HEROICTALE,
                14
            },
            { 
                TitleKind.STORY,
                14
            },
            { 
                TitleKind.CALDEAGATE,
                14
            },
            { 
                TitleKind.WAR100,
                40
            },
            { 
                TitleKind.WAR101,
                40
            },
            { 
                TitleKind.WAR102,
                40
            },
            { 
                TitleKind.WAR103,
                40
            },
            { 
                TitleKind.WAR104,
                40
            },
            { 
                TitleKind.WAR105,
                40
            },
            { 
                TitleKind.WAR106,
                40
            },
            { 
                TitleKind.WAR107,
                40
            },
            { 
                TitleKind.FORMATION,
                14
            },
            { 
                TitleKind.FORM_MASTER,
                40
            },
            { 
                TitleKind.PARTY_CHECK,
                40
            },
            { 
                TitleKind.FORM_SVT_LIST,
                10
            },
            { 
                TitleKind.FORM_SVT_EQUIP_LIST,
                10
            },
            { 
                TitleKind.FORM_SVT_EQUIP_SELECT,
                0x4a
            },
            { 
                TitleKind.COMBINE,
                40
            },
            { 
                TitleKind.SERVANT_COMBINE,
                40
            },
            { 
                TitleKind.SKILL_COMBINE,
                40
            },
            { 
                TitleKind.NP_COMBINE,
                40
            },
            { 
                TitleKind.LIMIT_COMBINE,
                40
            },
            { 
                TitleKind.SVT_EQUIP_COMBINE,
                40
            },
            { 
                TitleKind.SUMMON,
                40
            },
            { 
                TitleKind.SHOP,
                3
            },
            { 
                TitleKind.SHOP_SELL_SERVANT,
                40
            },
            { 
                TitleKind.FRIEND,
                40
            },
            { 
                TitleKind.MYROOM,
                14
            },
            { 
                TitleKind.MYROOM_MATERIAL,
                14
            },
            { 
                TitleKind.PRESENTBOX,
                40
            },
            { 
                TitleKind.MYROOM_USERITEM,
                14
            },
            { 
                TitleKind.PROFILE,
                14
            },
            { 
                TitleKind.GAMEOPTION,
                14
            },
            { 
                TitleKind.MYROOM_NOTICE,
                14
            },
            { 
                TitleKind.SERIAL_CODE,
                14
            },
            { 
                TitleKind.CONTINUE_DEVICE,
                14
            },
            { 
                TitleKind.FAVORITE_CHANGE,
                14
            },
            { 
                TitleKind.SERVANT_PROFILE,
                40
            },
            { 
                TitleKind.BATTLE_CHARACTER,
                40
            },
            { 
                TitleKind.FOLLOWER,
                40
            },
            { 
                TitleKind.BATTLE_SETUP_CONFIRM,
                40
            },
            { 
                TitleKind.ORGANIZATION,
                40
            },
            { 
                TitleKind.MASTER_ORGANIZATION,
                40
            },
            { 
                TitleKind.PARTY_ORGANIZATION,
                15
            },
            { 
                TitleKind.PARTY_ORGANIZATION_CHANGE,
                15
            },
            { 
                TitleKind.PARTY_ORGANIZATION_SWAP,
                15
            },
            { 
                TitleKind.PARTY_ORGANIZATION_SERVANT,
                0x23
            },
            { 
                TitleKind.EVENT_BOX_GACHA,
                0x23
            },
            { 
                TitleKind.EVENT_REWARD,
                0x23
            },
            { 
                TitleKind.EVENT_MISSION,
                0x23
            },
            { 
                TitleKind.SUPPORT_SELECT,
                40
            },
            { 
                TitleKind.FRIEND_SUPPORT_INFO,
                40
            }
        };
        titleDepthList = dictionary2;
        _helpEnableKind = new TitleKind[] { TitleKind.FORMATION, TitleKind.COMBINE, TitleKind.SHOP, TitleKind.EVENT_REWARD, TitleKind.EVENT_BOX_GACHA, TitleKind.EVENT_MISSION };
        Dictionary<TitleKind, TutorialFlag.ImageId> dictionary3 = new Dictionary<TitleKind, TutorialFlag.ImageId> {
            { 
                TitleKind.FORMATION,
                TutorialFlag.ImageId.FORMATION_TOP
            },
            { 
                TitleKind.COMBINE,
                TutorialFlag.ImageId.COMBINE_TOP
            },
            { 
                TitleKind.SHOP,
                TutorialFlag.ImageId.SHOP_TOP
            },
            { 
                TitleKind.EVENT_REWARD,
                TutorialFlag.ImageId.EVENT_REWARD
            },
            { 
                TitleKind.EVENT_BOX_GACHA,
                TutorialFlag.ImageId.EVENT_GACHA
            },
            { 
                TitleKind.EVENT_MISSION,
                TutorialFlag.ImageId.EVENT_MISSION
            }
        };
        _helpDispImageList = dictionary3;
    }

    private void Awake()
    {
        if (this.backBtnBgSprite == null)
        {
            this.backBtnBgSprite = this.backBtnSprite.gameObject.GetParent().GetComponent<UISprite>();
        }
        this.backBtnBgSprite.MakePixelPerfect();
        this.backBtnBgSprite.gameObject.SetLocalPosition(BACK_BTN_SP_BASE_X, 0f, 0f);
        this.backBtnBgSprite.MakePixelPerfect();
        this.headerBgImg.MakePixelPerfect();
        this.mDebugBtnAct = null;
        if (this.mDebugBtn != null)
        {
            this.mDebugBtn.gameObject.SetActive(true);
        }
        base.gameObject.SetLocalPositionY(BASE_Y);
        if ((this.particleObj == null) && (this.particlePrefab != null))
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.particlePrefab);
            if (self != null)
            {
                self.SafeSetParent(this);
                self.SetLocalPosition(new Vector3(0f, PARTICLE_Y, 0f));
                this.particleObj = self;
                this.particleObj.GetComponent<UIPanel>().depth = this.particlePanelDepth;
            }
        }
        this.SetActiveEventItem(false, false);
        this.SetEventRestTime(0L);
    }

    public void changeTitleInfo(bool isBack, TitleKind titleKind)
    {
        this.setBackBtn(true);
        this.setBackBtnSprite(isBack);
        this.setTitleImg(titleKind, true);
    }

    public void changeTitleInfo(BackKind backKind, TitleKind titleKind)
    {
        this.setBackBtnSprite(backKind);
        this.setTitleImg(titleKind, true);
    }

    public void CheckSuperBossHpAnim(System.Action callback)
    {
        <CheckSuperBossHpAnim>c__AnonStorey58 storey = new <CheckSuperBossHpAnim>c__AnonStorey58 {
            callback = callback,
            <>f__this = this
        };
        if (((this.mSuperBossAnimIdx > -1) && (this.mEventInfoGroups != null)) && (this.mEventInfoGroups.Count > this.mSuperBossAnimIdx))
        {
            this.SetTouchEnable(false);
            ((TitleInfoSuperBossComponent) this.mEventInfoGroups[this.mSuperBossAnimIdx]).StartDamageAnimation(TerminalSceneComponent.Instance.TerminalMap.GetAssetData(), new System.Action(storey.<>m__21));
        }
        else
        {
            storey.callback.Call();
            System.Action mAdditionalCallbackOnBossAnim = this.mAdditionalCallbackOnBossAnim;
            this.mAdditionalCallbackOnBossAnim = null;
            mAdditionalCallbackOnBossAnim.Call();
            this.ClearMssionNotifyPause();
        }
    }

    public void ClearMssionNotifyPause()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.CancelPause();
    }

    private void DestroyEventItem()
    {
        foreach (TitleInfoEventItemComponent component in this.mEventItemGroups)
        {
            UnityEngine.Object.Destroy(component.gameObject);
        }
        this.mEventItemGroups.Clear();
        this.mIsDoneSetEventItem = false;
    }

    private void DispEventItem(int event_id)
    {
        ShopMaster master = null;
        int[] eventItemList = null;
        if ((this.mEventBgSp != null) && (this.mEventRoot != null))
        {
            bool flag = event_id > 0;
            if (flag)
            {
                master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
                eventItemList = master.GetEventItemList(event_id);
                if (this.IsEventShopOnly() && (eventItemList != null))
                {
                    flag = eventItemList.Length > 0;
                }
            }
            this.mEventBgSp.gameObject.SetActive(flag);
            this.mEventRoot.gameObject.SetActive(flag);
            if (flag)
            {
                EventDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(event_id);
                bool flag2 = true;
                if ((!this.IsEventShopOnly() && (entity != null)) && entity.IsReward())
                {
                    flag2 = false;
                }
                if (flag2)
                {
                    bool flag3 = master.GetEnableEventEntitiyList(event_id).Length > 0;
                    this.mEventItemEventBtn.spriteName = EVENT_BTN_SHOP_SPNAME;
                    this.mEventItemEventBtn.color = !flag3 ? Color.gray : Color.white;
                    this.SetEventBtnCollider(flag3);
                }
                else
                {
                    this.mEventItemEventBtn.spriteName = EVENT_BTN_REWARD_SPNAME;
                    this.SetEventBtnCollider(true);
                    if ((entity != null) && entity.isMission)
                    {
                        int number = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getMissionProgressNum(event_id, MissionProgressType.Type.CLEAR);
                        if (this.mNoticeNumber == null)
                        {
                            this.mNoticeNumber = UnityEngine.Object.Instantiate<NoticeNumberComponent>(this.noticeNumberPrefab);
                            GameObject gameObject = this.mNoticeNumber.gameObject;
                            gameObject.SafeSetParent(this.mEventItemEventBtn);
                            gameObject.SetLocalPosition(new Vector2(59f, 16f));
                            gameObject.SetLocalScale((float) 0.75f);
                        }
                        this.mNoticeNumber.SetNumber(number);
                    }
                }
                if (!this.mIsDoneSetEventItem)
                {
                    long activeEventEndTime = this.GetActiveEventEndTime();
                    this.SetEventRestTime(activeEventEndTime);
                }
                if (!this.mIsDoneSetEventItem)
                {
                    if ((!this.IsEventShopOnly() && (entity != null)) && entity.isMission)
                    {
                        this.SetupEventMission(event_id);
                    }
                    if ((!this.IsEventShopOnly() && (entity != null)) && entity.isEventPoint)
                    {
                        this.SetupEventPoint(event_id);
                    }
                    if ((!this.IsEventShopOnly() && (entity != null)) && entity.isBoxGacha)
                    {
                        this.SetupEventBoxGacha(event_id);
                    }
                    List<List<int>> list = new List<List<int>>();
                    List<int> list2 = new List<int>();
                    List<ItemEntity> list3 = new List<ItemEntity>();
                    this.GroupingEventItem(list2, list3, eventItemList);
                    if (!this.IsEventShopOnly())
                    {
                        int[] numArray2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaMaster>(DataNameKind.Kind.BOX_GACHA).GetEventItemList(event_id);
                        this.GroupingEventItem(list2, list3, numArray2);
                    }
                    if (<>f__am$cache4A == null)
                    {
                        <>f__am$cache4A = (a, b) => b - a;
                    }
                    list2.Sort(<>f__am$cache4A);
                    foreach (int num3 in list2)
                    {
                        List<int> item = new List<int>();
                        foreach (ItemEntity entity2 in list3)
                        {
                            if (entity2.value == num3)
                            {
                                item.Add(entity2.id);
                                if (item.Count >= TitleInfoEventItemComponent.GROUP_DISP_OBJ_MAX)
                                {
                                    break;
                                }
                            }
                        }
                        list.Add(item);
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.SetupEventItem(list[i].ToArray(), TitleInfoEventItemComponent.DispType.ITEM);
                    }
                    this.InitEventAlphaAnim();
                }
                else
                {
                    foreach (TitleInfoEventItemComponent component in this.mEventItemGroups)
                    {
                        component.UpdateDisp();
                    }
                }
            }
        }
    }

    public bool FrameIn(bool is_force = false)
    {
        bool flag = this.FrameInOut(true, is_force);
        if (flag)
        {
            this.InitEventAlphaAnim();
        }
        return flag;
    }

    public void FrameIn_BackBtn(bool is_force = false)
    {
        this.FrameInOut_BackBtn(true, is_force);
    }

    private bool FrameInOut(bool is_framein, bool is_force = false)
    {
        <FrameInOut>c__AnonStorey59 storey = new <FrameInOut>c__AnonStorey59 {
            tgt_obj = base.gameObject
        };
        float num = BASE_Y;
        float num2 = FRAME_OUT_POS_Y;
        storey.tgt_y = !is_framein ? num2 : num;
        if (is_force || (storey.tgt_y == storey.tgt_obj.GetLocalPositionY()))
        {
            storey.tgt_obj.SetLocalPositionY(storey.tgt_y);
            return false;
        }
        float sec = TerminalPramsManager.GetIntpTime_AutoResume(FRAME_IN_OUT_MV_SPD_TIME);
        Vector3 localPosition = storey.tgt_obj.GetLocalPosition();
        localPosition.y = num;
        Vector3 vector2 = storey.tgt_obj.GetLocalPosition();
        vector2.y = num2;
        storey.mo = storey.tgt_obj.SafeGetComponent<MoveObject>();
        Vector3 from = !is_framein ? localPosition : vector2;
        Vector3 to = !is_framein ? vector2 : localPosition;
        storey.mo.Play(from, to, sec, new System.Action(storey.<>m__22), new System.Action(storey.<>m__23), 0f, Easing.TYPE.EXPONENTIAL_OUT);
        return true;
    }

    public void FrameInOut_BackBtn(bool is_framein, bool is_force = false)
    {
        GameObject gameObject = this.backBtnBgSprite.gameObject;
        int num = 0;
        int num2 = -this.backBtnBgSprite.width;
        int num3 = !is_framein ? num2 : num;
        if (is_force)
        {
            gameObject.SetLocalPositionX((float) num3);
        }
        else if (num3 != gameObject.GetLocalPositionX())
        {
            Vector3 localPosition = gameObject.GetLocalPosition();
            localPosition.x = num;
            Vector3 vector2 = gameObject.GetLocalPosition();
            vector2.x = num2;
            TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, BACK_BTN_MV_SPD_TIME);
            position.from = !is_framein ? localPosition : vector2;
            position.to = !is_framein ? vector2 : localPosition;
            position.method = UITweener.Method.EaseOut;
        }
    }

    public bool FrameOut(bool is_force = false) => 
        this.FrameInOut(false, is_force);

    public void FrameOut_BackBtn(bool is_force = false)
    {
        this.FrameInOut_BackBtn(false, is_force);
    }

    private long GetActiveEventEndTime()
    {
        if (this.IsEventShopOnly())
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId(this.mShopEventId).GetShopEndTime();
        }
        EventEndTimeInfo activeEventEndTimeInfo = this.GetActiveEventEndTimeInfo();
        if (activeEventEndTimeInfo == null)
        {
            return 0L;
        }
        return activeEventEndTimeInfo.end_time;
    }

    private EventEndTimeInfo GetActiveEventEndTimeInfo() => 
        ((this.mEventEndTimeInfs.Count <= 0) ? null : this.mEventEndTimeInfs[0]);

    private int GetActiveEventId()
    {
        if (this.IsEventShopOnly())
        {
            return this.mShopEventId;
        }
        EventEndTimeInfo activeEventEndTimeInfo = this.GetActiveEventEndTimeInfo();
        if (activeEventEndTimeInfo == null)
        {
            return 0;
        }
        return activeEventEndTimeInfo.event_id;
    }

    private static string getTitleImgName(TitleKind kind) => 
        titleNameList[kind];

    private void GroupingEventItem(List<int> out_group_ids, List<ItemEntity> out_item_ents, int[] item_ids)
    {
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        foreach (int num in item_ids)
        {
            ItemEntity item = master.getEntityFromId<ItemEntity>(num);
            if (item != null)
            {
                int num3 = item.value;
                if (!out_group_ids.Contains(num3))
                {
                    out_group_ids.Add(num3);
                }
                if (!out_item_ents.Contains(item))
                {
                    out_item_ents.Add(item);
                }
            }
        }
    }

    public void InitEventAlphaAnim()
    {
        if (this.IsEventItemGroups())
        {
            this.SetDispEventItemGroup(0);
            this.mEventAlphaAnimTgt = 1f;
            this.mEventAlphaAnimRoot.alpha = this.mEventAlphaAnimTgt;
            this.mEventAlphaAnimTimeOld = Time.realtimeSinceStartup;
        }
    }

    private bool IsEventItemGroups() => 
        (this.mEventItemGroups.Count > 1);

    private bool IsEventShopOnly() => 
        (this.mShopEventId > 0);

    private void LateUpdate()
    {
        this.backBtnSprite.color = this.backBtnBgSprite.color;
    }

    private void NextDispEventItemGroup()
    {
        this.SetDispEventItemGroup(-1);
    }

    public void OnClickEventAlphaAnimChangeBtn()
    {
        if (this.IsEventItemGroups() && (this.mEventAlphaAnimTgt > 0f))
        {
            this.mEventAlphaAnimTgt = 0f;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
    }

    public void OnClickEventBtn()
    {
        int activeEventId = this.GetActiveEventId();
        Debug.LogError("1   :  " + this.IsEventShopOnly());
        if (this.IsEventShopOnly())
        {
            OnClickShopBtn(activeEventId);
        }
        else
        {
            Debug.LogError("2   :  " + this.mEventBannerWindow.IsOpenPossible());
            if ((this.mEventBannerWindow != null) && this.mEventBannerWindow.IsOpenPossible())
            {
                if (<>f__am$cache4B == null)
                {
                    <>f__am$cache4B = delegate {
                    };
                }
                this.mEventBannerWindow.Open(<>f__am$cache4B);
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            else
            {
                OnClickEventBtn(activeEventId);
            }
        }
    }

    public static void OnClickEventBtn(int event_id)
    {
        EventDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(event_id);
        if ((entity != null) && entity.IsReward())
        {
            EventEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId(event_id);
            long num = NetworkManager.getTime();
            int condQuestId = entity.condQuestId;
            if (((condQuestId == 0) || SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(condQuestId, false)) && (num < entity2.getEventFinishedAt()))
            {
                SceneJumpInfo data = new SceneJumpInfo(string.Empty, event_id);
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.EventGacha, SceneManager.FadeType.BLACK, data);
            }
            else
            {
                string title = string.Empty;
                string condMessage = entity.condMessage;
                if (num >= entity2.getEventEndedAt())
                {
                    condMessage = string.Format(LocalizationManager.Get("EVENT_REWARD_END_MESSAGE"), entity2.getEventName());
                }
                if (<>f__am$cache4C == null)
                {
                    <>f__am$cache4C = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, condMessage, <>f__am$cache4C, -1);
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
        else
        {
            OnClickShopBtn(event_id);
        }
    }

    public void OnClickHelpBtn()
    {
        if (_helpDispImageList.ContainsKey(this.nowTitleKind))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            TutorialFlag.ImageId image = _helpDispImageList[this.nowTitleKind];
            if (<>f__am$cache4D == null)
            {
                <>f__am$cache4D = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(image, TutorialFlag.Id.NULL, <>f__am$cache4D);
        }
    }

    public static void OnClickShopBtn(int event_id)
    {
        EventItemListComponent.GoToShopEventItem(event_id);
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
    }

    public void OnDoubleClickDebugBtn()
    {
        this.mDebugBtnAct.Call();
    }

    private void OnEnable()
    {
        this.InitEventAlphaAnim();
    }

    public void sendEvent(string msg)
    {
        if ((msg != null) && (this.activeSceneFSM != null))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.activeSceneFSM.SendEvent(msg);
        }
    }

    public void SetActiveEventItem(WarEntity war_ent)
    {
        bool flag = war_ent.IsEvent();
        bool flag2 = flag;
        this.SetActiveEventItem(flag, flag2);
    }

    public void SetActiveEventItem(bool is_active, bool is_bg_black = false)
    {
        this.mIsEventItemBgBlack = is_bg_black;
        this.mIsActiveEventItem = is_active;
        if (this.mIsActiveEventItem)
        {
            this.SetEventItem();
            this.SetEventItemBg(is_bg_black);
        }
        else
        {
            this.DispEventItem(0);
        }
    }

    private void setBackBtn(bool isShow)
    {
        this.backBtn.SetActive(isShow);
        this.backBtnBgSprite.spriteName = "icon_return_bg";
        this.backBtnBgSprite.MakePixelPerfect();
        this.FrameIn_BackBtn(true);
    }

    public void setBackBtn_Terminal(bool isDispBack = false)
    {
        this.backBtnBgSprite.gameObject.SetActive(true);
        this.FrameIn_BackBtn(false);
        if (isDispBack)
        {
            this.setBackBtnSprite(isDispBack);
        }
        else
        {
            this.backBtnSprite.spriteName = "img_return_info";
            this.backBtnSprite.MakePixelPerfect();
            this.backBtnSprite.gameObject.SetLocalPositionX(BACK_BTN_SP_BASE_X);
            if (BalanceConfig.IsIOS_Examination)
            {
                this.backBtnBgSprite.gameObject.SetActive(false);
            }
        }
    }

    public void SetBackBtnAct(System.Action act)
    {
        <SetBackBtnAct>c__AnonStorey57 storey = new <SetBackBtnAct>c__AnonStorey57 {
            act = act
        };
        UIEventTrigger component = this.backBtnBgSprite.gameObject.GetComponent<UIEventTrigger>();
        EventDelegate item = new EventDelegate(new EventDelegate.Callback(storey.<>m__1C));
        component.onClick.Add(item);
    }

    public void setBackBtnColliderEnable(bool is_enable)
    {
        this.backBtnSprite.gameObject.GetParent().GetComponent<BoxCollider>().enabled = is_enable;
        UIButton componentInChildren = this.backBtn.GetComponentInChildren<UIButton>();
        if (componentInChildren != null)
        {
            componentInChildren.state = UIButtonColor.State.Normal;
        }
    }

    public void setBackBtnDepth(int depth)
    {
        UIPanel component = this.backBtn.GetComponent<UIPanel>();
        if (component != null)
        {
            component.depth = depth;
        }
    }

    public void setBackBtnEnable(bool isEnable)
    {
        UIButton componentInChildren = this.backBtn.GetComponentInChildren<UIButton>();
        if (componentInChildren != null)
        {
            componentInChildren.SetState(!isEnable ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
        }
        this.backBtnSprite.gameObject.GetParent().GetComponent<BoxCollider>().enabled = isEnable;
    }

    public void setBackBtnSprite(bool isDispBack)
    {
        string str = !isDispBack ? "img_return_terminal" : "img_return_modoru";
        this.backBtnSprite.spriteName = str;
        this.backBtnSprite.MakePixelPerfect();
        this.backBtnSprite.gameObject.SetLocalPositionX(!isDispBack ? BACK_BTN_SP_TERMINAL_X : BACK_BTN_SP_BASE_X);
        this.backBtnBgSprite.gameObject.GetComponent<UIButton>().enabled = true;
    }

    public void setBackBtnSprite(BackKind kind)
    {
        string str = null;
        float x = 0f;
        switch (kind)
        {
            case BackKind.TERMINAL:
                str = "img_return_terminal";
                x = BACK_BTN_SP_TERMINAL_X;
                break;

            case BackKind.CLOSE:
                str = "img_return_modoru";
                x = BACK_BTN_SP_BASE_X;
                break;

            case BackKind.BACK:
                str = "img_return_modoru2";
                x = BACK_BTN_SP_BASE_X;
                break;
        }
        if (str != null)
        {
            this.setBackBtn(true);
            this.backBtnBgSprite.gameObject.GetComponent<UIButton>().enabled = true;
            this.backBtnSprite.spriteName = str;
            this.backBtnSprite.MakePixelPerfect();
            this.backBtnSprite.gameObject.SetLocalPositionX(x);
        }
        else
        {
            this.setBackBtn(false);
            this.backBtnBgSprite.gameObject.GetComponent<UIButton>().enabled = false;
        }
    }

    public void SetDebugBtnAct(System.Action act)
    {
        this.mDebugBtnAct = act;
    }

    public void setDepth(int depth)
    {
        UIPanel component = base.gameObject.GetComponent<UIPanel>();
        if (component != null)
        {
            component.depth = depth;
        }
    }

    private void SetDispEventItemGroup(int group_id)
    {
        if (group_id >= 0)
        {
            this.mEventItemGroupIdx = group_id;
        }
        else
        {
            this.mEventItemGroupIdx++;
            if (this.mEventItemGroupIdx >= this.mEventItemGroups.Count)
            {
                this.mEventItemGroupIdx = 0;
            }
        }
        foreach (TitleInfoEventItemComponent component in this.mEventItemGroups)
        {
            component.gameObject.SetActive(false);
        }
        this.mEventItemGroups[this.mEventItemGroupIdx].gameObject.SetActive(true);
    }

    public void SetEventBtnCollider(bool is_active)
    {
        this.mEventItemEventBtn.gameObject.GetComponent<BoxCollider>().enabled = is_active;
    }

    private void SetEventItem()
    {
        bool flag = false;
        ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
        int[] openedEventIdList = master.GetOpenedEventIdList();
        if (!this.IsEventShopOnly())
        {
            this.mEventEndTimeInfs.Clear();
            foreach (EventEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<EventEntity>(DataNameKind.Kind.EVENT))
            {
                long num2 = entity.GetEndTime_ShopOrReward();
                if ((num2 > 0L) && (master.GetEnableEventEntitiyList(entity.id).Length > 0))
                {
                    EventEndTimeInfo item = new EventEndTimeInfo {
                        event_id = entity.getEventId(),
                        is_reward = entity.IsReward(),
                        end_time = num2
                    };
                    this.mEventEndTimeInfs.Add(item);
                    if (!flag && item.is_reward)
                    {
                        flag = true;
                    }
                }
            }
            this.mEventEndTimeInfs.Sort(new Comparison<EventEndTimeInfo>(EventEndTimeInfo.Sort));
            this.SetupEventBannerWindow(this.mEventEndTimeInfs);
        }
        this.DispEventItem(this.GetActiveEventId());
        this.mIsDoneSetEventItem = true;
        base.enabled = true;
    }

    private void SetEventItemBg(bool is_bg_black)
    {
        this.mEventBgSp.spriteName = !is_bg_black ? "img_event_titlebg" : "img_event_titlebg_black";
        this.mEventBgSp.MakePixelPerfect();
        this.titleImg.gameObject.SetActive(!is_bg_black);
    }

    public void SetEventRestTime(long end_time)
    {
        this.mEventEndTime = end_time;
        if (this.mEventRestTimeLb != null)
        {
            this.mEventRestTimeLb.gameObject.SetActive(end_time > 0L);
            this.mEventRestTimeOld = 0L;
        }
    }

    public void setHeaderBgImg(bool isMap)
    {
        string str = !isMap ? "img_titlebg" : "img_titlebg_clear";
        this.headerBgImg.spriteName = str;
        this.headerBgImg.MakePixelPerfect();
        this.particleObj.SetActive(!isMap);
    }

    public void SetHelpBtn(bool isShow)
    {
        if (this.helpBtn != null)
        {
            this.helpBtn.SetActive(isShow);
        }
    }

    public void SetHelpBtnColliderEnable(bool is_enable)
    {
        if (this.helpBtn != null)
        {
            this.helpBtn.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
        }
    }

    public void SetParent(Transform parent_transform)
    {
        base.transform.parent = parent_transform;
        base.transform.localPosition = Vector3.zero;
        base.transform.localRotation = Quaternion.identity;
        base.transform.localScale = Vector3.one;
        base.gameObject.SetLocalPositionY(BASE_Y);
    }

    public void setParticleDepth(int depth)
    {
        if (this.particleObj == null)
        {
            Debug.LogError("Can not Set TitlePrefab's Particle Depth!");
        }
        else
        {
            UIPanel component = this.particleObj.GetComponent<UIPanel>();
            if (component != null)
            {
                component.depth = depth;
            }
        }
    }

    public void SetShopEventItem(int event_id, bool is_init_req = false)
    {
        if (is_init_req)
        {
            this.DestroyEventItem();
        }
        this.mShopEventId = event_id;
        this.SetActiveEventItem(this.IsEventShopOnly(), false);
    }

    public void setTitleImg(TitleKind kind, bool setDefaultDepth = true)
    {
        string str = getTitleImgName(kind);
        if (str != null)
        {
            this.titleImg.gameObject.SetActive(true);
            this.titleImg.spriteName = str;
            this.titleImg.MakePixelPerfect();
            this.nowTitleKind = kind;
            Debug.Log(string.Concat(new object[] { "!!** setTitleImg imgName: ", str, " _nowTitleKind: ", this.nowTitleKind }));
            if (setDefaultDepth)
            {
                this.setParticleDepth(titleDepthList[kind]);
            }
            if (Array.IndexOf<TitleKind>(_helpEnableKind, kind) == -1)
            {
                this.SetHelpBtn(false);
            }
        }
    }

    public void setTitleInfo(PlayMakerFSM fsm, BackKind backKind, TitleKind kind)
    {
        this.activeSceneFSM = (fsm == null) ? null : fsm;
        this.setBackBtnSprite(backKind);
        this.setTitleImg(kind, true);
    }

    public void setTitleInfo(PlayMakerFSM fsm, bool isShow, string titleTxt, TitleKind kind)
    {
        this.activeSceneFSM = (fsm == null) ? null : fsm;
        this.setBackBtnSprite(false);
        this.setBackBtn(isShow);
        this.setTitleImg(kind, true);
    }

    public void SetTouchEnable(bool onOff)
    {
        if (onOff)
        {
            if (this.mBlockMask != null)
            {
                UnityEngine.Object.DestroyImmediate(this.mBlockMask);
                this.mBlockMask = null;
            }
        }
        else
        {
            this.mBlockMask = UnityEngine.Object.Instantiate<GameObject>(this.mBlockMaskPrefab);
            this.mBlockMask.SafeSetParent(this.mBlockMaskParent);
            this.mBlockMask.SetLocalPosition(Vector3.zero);
        }
    }

    private void SetupEventBannerWindow(List<EventEndTimeInfo> ev_end_time_infs)
    {
        if (this.mEventBannerWindow == null)
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mEventBannerWindowPrefab);
            if (self != null)
            {
                self.SafeSetParent(this);
                self.SetLocalPositionY(-BASE_Y);
                this.mEventBannerWindow = self.GetComponent<EventBannerWindow>();
            }
        }
        if (this.mEventBannerWindow.Setup(ev_end_time_infs))
        {
            this.mIsDoneSetEventItem = false;
        }
    }

    private void SetupEventBoxGacha(int event_id)
    {
        int[] eventItemList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaMaster>(DataNameKind.Kind.BOX_GACHA).GetEventItemList(event_id);
        this.SetupEventItem(eventItemList, TitleInfoEventItemComponent.DispType.ITEM);
    }

    private void SetupEventItem(int[] item_ids, TitleInfoEventItemComponent.DispType disp_type = 0)
    {
        GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mTitleInfoEventItemPrefab);
        self.SafeSetParent(this.mEventAlphaAnimRoot);
        TitleInfoEventItemComponent item = self.GetComponent<TitleInfoEventItemComponent>();
        item.Setup(item_ids, disp_type);
        this.mEventItemGroups.Add(item);
    }

    private void SetupEventMission(int event_id)
    {
        int[] numArray1 = new int[] { event_id };
        this.SetupEventItem(numArray1, TitleInfoEventItemComponent.DispType.MISSION);
    }

    private void SetupEventPoint(int event_id)
    {
        int[] numArray1 = new int[] { event_id };
        this.SetupEventItem(numArray1, TitleInfoEventItemComponent.DispType.POINT);
    }

    private void Update()
    {
        this.UpdateEventAlphaAnim();
        this.UpdateEventRestTime();
    }

    private void UpdateEventAlphaAnim()
    {
        if (this.IsEventItemGroups() && !SingletonMonoBehaviour<CommonUI>.Instance.IsActive_UserPresentBoxWindow())
        {
            float alpha = this.mEventAlphaAnimRoot.alpha;
            alpha += (this.mEventAlphaAnimTgt - alpha) * EVENT_ALPHA_ANIM_SPD_RATE;
            float num3 = Time.realtimeSinceStartup - this.mEventAlphaAnimTimeOld;
            if (num3 >= EVENT_ALPHA_ANIM_TIME_INTERVAL)
            {
                this.mEventAlphaAnimTgt = 0f;
                this.mEventAlphaAnimTimeOld = Time.realtimeSinceStartup;
            }
            if ((this.mEventAlphaAnimTgt <= 0f) && (alpha <= 0.01f))
            {
                alpha = 0f;
                this.mEventAlphaAnimTgt = 1f;
                this.mEventAlphaAnimTimeOld = Time.realtimeSinceStartup;
                this.NextDispEventItemGroup();
            }
            this.mEventAlphaAnimRoot.alpha = alpha;
        }
    }

    public void UpdateEventItem()
    {
        this.SetActiveEventItem(this.mIsActiveEventItem, this.mIsEventItemBgBlack);
    }

    private void UpdateEventRestTime()
    {
        if ((this.mEventRestTimeLb != null) && this.mEventRestTimeLb.gameObject.activeSelf)
        {
            long num = NetworkManager.getTime();
            long num2 = num - this.mEventRestTimeOld;
            if (num2 >= TIME_UPDATE_ITVL_SEC)
            {
                long mEventEndTime = this.mEventEndTime;
                string str = LocalizationManager.Get("TIME_REST_EVENT") + LocalizationManager.GetRestTime(mEventEndTime);
                this.mEventRestTimeLb.text = str;
                this.mEventRestTimeOld = num;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <CheckSuperBossHpAnim>c__AnonStorey58
    {
        internal TitleInfoControl <>f__this;
        internal System.Action callback;

        internal void <>m__21()
        {
            this.<>f__this.mIsPauseEventAlphaAnim = false;
            this.<>f__this.mEventAlphaAnimTimeOld = Time.realtimeSinceStartup;
            this.<>f__this.mSuperBossAnimIdx = -1;
            this.<>f__this.SetTouchEnable(true);
            this.callback.Call();
            System.Action mAdditionalCallbackOnBossAnim = this.<>f__this.mAdditionalCallbackOnBossAnim;
            this.<>f__this.mAdditionalCallbackOnBossAnim = null;
            mAdditionalCallbackOnBossAnim.Call();
            this.<>f__this.ClearMssionNotifyPause();
        }
    }

    [CompilerGenerated]
    private sealed class <FrameInOut>c__AnonStorey59
    {
        internal MoveObject mo;
        internal GameObject tgt_obj;
        internal float tgt_y;

        internal void <>m__22()
        {
            this.tgt_obj.SetLocalPositionY(this.mo.Now().y);
        }

        internal void <>m__23()
        {
            this.tgt_obj.SetLocalPositionY(this.tgt_y);
        }
    }

    [CompilerGenerated]
    private sealed class <SetBackBtnAct>c__AnonStorey57
    {
        internal System.Action act;

        internal void <>m__1C()
        {
            this.act.Call();
        }
    }

    public enum BackKind
    {
        NONE,
        TERMINAL,
        CLOSE,
        BACK
    }

    public class EventEndTimeInfo
    {
        public long end_time;
        public int event_id;
        public bool is_reward;
        public int noticeNumberCount;

        public static int Sort(TitleInfoControl.EventEndTimeInfo a, TitleInfoControl.EventEndTimeInfo b)
        {
            long num = b.end_time - a.end_time;
            if (num < 0L)
            {
                return -1;
            }
            if (num > 0L)
            {
                return 1;
            }
            return 0;
        }
    }

    public enum TitleKind
    {
        NONE,
        TERMINAL,
        HEROICTALE,
        STORY,
        CALDEAGATE,
        WAR100,
        WAR101,
        WAR102,
        WAR103,
        WAR104,
        WAR105,
        WAR106,
        WAR107,
        FORMATION,
        FORM_MASTER,
        PARTY_CHECK,
        FORM_SVT_LIST,
        FORM_SVT_EQUIP_LIST,
        FORM_SVT_EQUIP_SELECT,
        COMBINE,
        SERVANT_COMBINE,
        SKILL_COMBINE,
        NP_COMBINE,
        LIMIT_COMBINE,
        SVT_EQUIP_COMBINE,
        SUMMON,
        SHOP,
        SHOP_SELL_SERVANT,
        FRIEND,
        MYROOM,
        MYROOM_MATERIAL,
        PRESENTBOX,
        MYROOM_USERITEM,
        PROFILE,
        GAMEOPTION,
        MYROOM_NOTICE,
        SERIAL_CODE,
        CONTINUE_DEVICE,
        FAVORITE_CHANGE,
        SERVANT_PROFILE,
        BATTLE_CHARACTER,
        FOLLOWER,
        BATTLE_SETUP_CONFIRM,
        ORGANIZATION,
        MASTER_ORGANIZATION,
        PARTY_ORGANIZATION,
        PARTY_ORGANIZATION_CHANGE,
        PARTY_ORGANIZATION_SWAP,
        PARTY_ORGANIZATION_SERVANT,
        EVENT_REWARD,
        EVENT_BOX_GACHA,
        EVENT_MISSION,
        SUPPORT_SELECT,
        FRIEND_SUPPORT_INFO
    }
}

