using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventGachaRootComponent : SceneRootComponent
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache76;
    [CompilerGenerated]
    private static System.Action <>f__am$cache77;
    [CompilerGenerated]
    private static System.Action <>f__am$cache78;
    [CompilerGenerated]
    private static System.Action <>f__am$cache79;
    [CompilerGenerated]
    private static Predicate<BoxGachaTalkEntity> <>f__am$cache7A;
    [CompilerGenerated]
    private static Predicate<BoxGachaTalkEntity> <>f__am$cache7B;
    [SerializeField]
    protected GameObject arrowInfo;
    [SerializeField]
    protected ExUITexture backTexture;
    protected static AssetData bannerData;
    private int beforeBoxGachaId;
    private int beforeMoveIdx;
    private int[] beforeResultNumbers;
    private Vector3 befScrollPos;
    public static readonly string BG_ROOT = "Back/back{0}";
    [SerializeField]
    protected GameObject bgImgObj;
    private string bgmName = "BGM_CHALDEA_1";
    [SerializeField]
    protected BoxGachaItemListViewManager boxGachaListViewManager;
    [SerializeField]
    protected BoxGachaResultEffectComponent boxGachaResultComp;
    private Vector3 center;
    [SerializeField]
    protected UICenterOnChild centerChild;
    [SerializeField]
    protected GameObject changeBtnInfoObj;
    [SerializeField]
    protected GameObject changeTypeInfoObj;
    private int compMissionNum;
    private int currentBoxGachaBaseId;
    private BoxGachaEntity currentBoxGachaEnt;
    private int currentBoxGachaId;
    private int currentEventId = DEFAULT_EVENT_ID;
    [SerializeField]
    protected GameObject currentEventInfoObj;
    private int currentEventSvtId;
    private int currentEventSvtLimitCnt;
    private int currentIdx;
    private int currentMoveIdx;
    private Vector3 currentScrollPos;
    private MultiSvtInfoComponent currentSvtInfoComp;
    private List<VoiceData> currentVoiceDataList;
    public static readonly int DEFAULT_BG_ID = 0x2846;
    public static readonly int DEFAULT_EVENT_ID = 0x3b9ac9f6;
    public static readonly int DEFAULT_SVT_ID = 0x7a4a4;
    public static readonly Rect DISP_RECT = new Rect(0f, 0.41f, 1f, 0.5869f);
    private int downLoadCnt;
    private int drawGachaTime;
    private BoxGachaEntity[] eventBoxGachaEntList;
    private EventDetailEntity eventDetailEnt;
    [SerializeField]
    protected UILabel eventGachaGuidNumLb;
    [SerializeField]
    protected GameObject eventGachaInfo;
    [SerializeField]
    protected UILabel eventGachaNumLb;
    [SerializeField]
    protected GameObject eventGachaTxtInfo;
    [SerializeField]
    protected UISprite eventLogoSprite;
    private EventMissionEntity[] eventMissionEntList;
    [SerializeField]
    protected GameObject eventMissionInfo;
    [SerializeField]
    protected EventMissionItemListViewManager eventMissionListViewManager;
    [SerializeField]
    protected UILabel eventMissionPrgTxt;
    [SerializeField]
    protected GameObject eventMissionTxtInfo;
    [SerializeField]
    protected UICommonButton eventPointBtn;
    [SerializeField]
    protected UISprite eventPointBtnImg;
    [SerializeField]
    protected GameObject eventPointInfoObj;
    [SerializeField]
    protected EventPointItemListViewManager eventPointListViewManager;
    [SerializeField]
    protected GameObject eventPointRewardInfo;
    [SerializeField]
    protected UILabel eventPointTxt;
    private EventRewardEntity[] eventRewardEntList;
    [SerializeField]
    protected GameObject eventRewardTxtInfo;
    [SerializeField]
    protected UISprite gachaBtnTxtSprite;
    [SerializeField]
    protected GameObject gachaMainInfo;
    [SerializeField]
    protected UISprite havePointInfoTxtSprite;
    [SerializeField]
    protected UIPanel indexPanel;
    private bool isEventGacha;
    private bool isEventReward;
    private bool isExchangeShop;
    private bool isMission;
    private bool isMultiEvent;
    private bool isVoicePlaying;
    protected SceneJumpInfo jumpInfo;
    [SerializeField]
    protected UIButton leftArrowBtn;
    [SerializeField]
    protected UISprite lineupInfoSprite;
    [SerializeField]
    protected UIWrapContent loopCtr;
    private static readonly int MaxDrawCount = 10;
    protected System.Action modifyCallbackFunc;
    [SerializeField]
    protected UIScrollView mScroll;
    [SerializeField]
    protected UILabel multiChangeInfoLabel;
    [SerializeField]
    protected UISprite multiGachaTimeSprite;
    [SerializeField]
    protected UISprite multiInfoTxtSprite;
    [SerializeField]
    protected UISprite multiLineUpSprite;
    [SerializeField]
    protected UISprite multiPointSprite;
    private List<MultiSvtInfoComponent> multiSvtInfoList;
    [SerializeField]
    protected GameObject multiSvtObj;
    [SerializeField]
    protected UISprite oneGachaTimeSprite;
    [SerializeField]
    protected UISprite oneInfoTxtSprite;
    [SerializeField]
    protected UISprite playBtnImg;
    [SerializeField]
    protected UISprite pointBtnTxtSprite;
    [SerializeField]
    protected UILabel pointInfoLabel;
    [SerializeField]
    protected UISprite pointInfoTxtSprite;
    [SerializeField]
    protected UICommonButton presentGachaBtn;
    [SerializeField]
    protected UISprite presentGachaBtnImg;
    [SerializeField]
    protected GameObject presentGachaInfoObj;
    protected static string requestBannaData;
    protected string requestVoiceData;
    [SerializeField]
    protected UISprite resetTxtSprite;
    private int[] resNoList;
    private int resultBoxGachaId;
    private int[] resultIdList;
    private int[] resultNumbers;
    private int[] resultRareIdxs;
    private static string REWARDIMG_ATLAS_PATH = "EventReward/EventRewardAtlas";
    private static AssetData rewardImgData;
    [SerializeField]
    protected UIButton rightArrowBtn;
    [SerializeField]
    protected SetBoxGachaResourceControl setGachaResourceCtr;
    [SerializeField]
    protected UISprite singleGachaTimeSprite;
    [SerializeField]
    protected UISprite singleInfoTxtSprite;
    [SerializeField]
    protected GameObject slideIndexPrefab;
    [SerializeField]
    protected UIGrid sliderGrid;
    [SerializeField]
    protected GameObject sliderInfo;
    [SerializeField]
    protected StandFigureBack standFigureBack;
    [SerializeField]
    protected GameObject standFigureObj;
    private StateType stateType;
    private UIStandFigureR svtFigure;
    [SerializeField]
    protected UIGrid svtFigureListGrid;
    [SerializeField]
    protected MyRoomSvtControl svtVoiceCtr;
    private readonly int[] testNumbers = new int[] { 1, 1, 1 };
    private readonly int[] testResult = new int[] { 0xc355, 0x2711, 0x2b19, 0xc369 };
    [SerializeField]
    protected TitleInfoControl titleInfo;
    private int totalMissionNum;
    private UserBoxGachaEntity usrBoxGachaEnt;
    protected List<ServantVoiceData[]> voiceList;
    [SerializeField]
    protected GameObject voicePlayEffect;

    public override void beginFinish()
    {
        this.quit();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.NONE, 30);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        base.beginResume();
    }

    public override void beginStartUp(object data)
    {
        this.jumpInfo = null;
        if ((data != null) && (data is SceneJumpInfo))
        {
            this.jumpInfo = data as SceneJumpInfo;
            if (this.jumpInfo != null)
            {
                this.currentEventId = this.jumpInfo.Id;
            }
        }
        this.titleInfo.setBackBtnDepth(20);
        MainMenuBar.setMenuActive(true, null);
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
        requestBannaData = "Banner/DownloadBanner";
        if (!AssetManager.loadAssetStorage(requestBannaData, new AssetLoader.LoadEndDataHandler(this.EndLoadEventBanner)))
        {
            requestBannaData = null;
            this.EndLoadEventBanner(null);
        }
    }

    private void callbackBoxGachaDraw(string result)
    {
        if (result.Equals("ng"))
        {
            SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
            base.myFSM.SendEvent("REQUEST_NG");
        }
        else if (!result.Equals("ng"))
        {
            resData[] dataArray = JsonManager.DeserializeArray<resData>("[" + result + "]");
            this.resultIdList = dataArray[0].giftIds;
            this.resNoList = dataArray[0].resultNumbers;
            List<int> list = new List<int>(this.resNoList.Length);
            foreach (int num in this.resNoList)
            {
                if (!list.Contains(num))
                {
                    list.Add(num);
                }
            }
            this.resultNumbers = list.ToArray();
            this.resultRareIdxs = dataArray[0].rareIndex;
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    private void callbackBoxGachaReset(string result)
    {
        if (result.Equals("ng"))
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    private void callBackNoticeDlg(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog(() => base.myFSM.SendEvent("END_NOTICE"));
    }

    private void checkEventExist()
    {
        this.currentIdx = 0;
        this.currentMoveIdx = 0;
        if ((this.eventDetailEnt != null) && this.eventDetailEnt.isEventPoint)
        {
            this.eventRewardEntList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventRewardMaster>(DataNameKind.Kind.EVENT_REWARD).GetEventRewardEntitiyList(this.currentEventId);
            if ((this.eventRewardEntList != null) && (this.eventRewardEntList.Length > 0))
            {
                this.isEventReward = true;
                int index = UnityEngine.Random.Range(0, this.eventDetailEnt.guideImageIds.Length);
                this.currentEventSvtId = this.eventDetailEnt.guideImageIds[index];
                this.currentEventSvtLimitCnt = this.eventDetailEnt.guideLimitCounts[index];
            }
        }
        if ((this.eventDetailEnt != null) && this.eventDetailEnt.isBoxGacha)
        {
            this.standFigureObj.SetActive(false);
            EventRewardSaveData.LoadBoxGachaData(this.currentEventId);
            this.currentIdx = EventRewardSaveData.GachaIdx;
            this.currentMoveIdx = EventRewardSaveData.GachaMoveIdx;
            this.eventBoxGachaEntList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaMaster>(DataNameKind.Kind.BOX_GACHA).getBoxGachaDataByEventId(this.currentEventId);
            if ((this.eventBoxGachaEntList != null) && (this.eventBoxGachaEntList.Length > 0))
            {
                this.isEventGacha = true;
                this.currentEventSvtId = this.eventBoxGachaEntList[this.currentIdx].guideImageId;
                this.currentEventSvtLimitCnt = this.eventBoxGachaEntList[this.currentIdx].guideLimitCount;
            }
        }
        if ((this.eventDetailEnt != null) && this.eventDetailEnt.isMission)
        {
            EventRewardSaveData.LoadMissionData(this.currentEventId);
            this.eventMissionEntList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getEventMissionList(this.currentEventId);
            if ((this.eventMissionEntList != null) && (this.eventMissionEntList.Length > 0))
            {
                this.isMission = true;
                int num2 = UnityEngine.Random.Range(0, this.eventDetailEnt.guideImageIds.Length);
                this.currentEventSvtId = this.eventDetailEnt.guideImageIds[num2];
                this.currentEventSvtLimitCnt = this.eventDetailEnt.guideLimitCounts[num2];
            }
        }
    }

    public void checkPresentNum()
    {
        this.stopSvtVoice();
        string eventName = ((SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(NetworkManager.UserId).Length + this.drawGachaTime) < BalanceConfig.PresentBoxMax) ? "START_GACHA" : "SHOW_MSG";
        base.myFSM.SendEvent(eventName);
    }

    public void clearSvtList()
    {
        int childCount = this.loopCtr.transform.childCount;
        int num2 = this.sliderGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(this.loopCtr.transform.GetChild(i).gameObject);
            }
        }
        if (num2 > 0)
        {
            for (int j = num2 - 1; j >= 0; j--)
            {
                UnityEngine.Object.Destroy(this.sliderGrid.transform.GetChild(j).gameObject);
            }
        }
        this.mScroll.onDragStarted = (UIScrollView.OnDragNotification) Delegate.Remove(this.mScroll.onDragStarted, new UIScrollView.OnDragNotification(this.OnDragStarted));
        this.centerChild.onFinished = (SpringPanel.OnFinished) Delegate.Remove(this.centerChild.onFinished, new SpringPanel.OnFinished(this.OnCenterOnChildFinished));
    }

    private void closeReseEndDlg(bool isRes)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog(() => base.myFSM.SendEvent("END_NOTICE"));
    }

    private void destroySvtFigure()
    {
        if (this.svtFigure != null)
        {
            UnityEngine.Object.Destroy(this.svtFigure.gameObject);
            this.svtFigure = null;
        }
    }

    public void endGachaExe()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }

    protected void EndLoadEventBanner(AssetData data)
    {
        if (requestBannaData != null)
        {
            if (bannerData != null)
            {
                AssetManager.releaseAsset(bannerData);
            }
            requestBannaData = null;
            bannerData = data;
        }
        this.SetLoadRewardImgData();
    }

    private void EndLoadRewardImg(AssetData data)
    {
        if (rewardImgData != null)
        {
            AssetManager.releaseAsset(rewardImgData);
        }
        rewardImgData = data;
        this.eventDetailEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId<EventDetailEntity>(this.currentEventId);
        int id = (this.eventDetailEnt == null) ? 0 : this.eventDetailEnt.getBgmId();
        string name = "BGM_CHALDEA_1";
        if (id != 0)
        {
            BgmEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BGM).getEntityFromId<BgmEntity>(id);
            if (entity != null)
            {
                name = entity.fileName;
            }
        }
        SoundManager.playBgm(name);
        this.isEventReward = false;
        this.isEventGacha = false;
        this.isMission = false;
        this.checkEventExist();
        this.setGuideSvtVoice();
    }

    private void EndLoadVoice()
    {
        this.svtVoiceCtr.setSvtVoice(this.voiceList, this.requestVoiceData);
        string assetName = this.getBgImgName();
        this.backTexture.uvRect = DISP_RECT;
        this.backTexture.SetAssetImage(assetName, new System.Action(this.EndSetBackImg));
    }

    private void EndLoadVoiceData()
    {
        this.downLoadCnt--;
        if (this.downLoadCnt == 0)
        {
            base.beginStartUp();
        }
    }

    private void EndSetBackImg()
    {
        this.downLoadCnt = this.currentVoiceDataList.Count;
        foreach (VoiceData data in this.currentVoiceDataList)
        {
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(data.DataName, new System.Action(this.EndLoadVoiceData), SoundManager.CueType.ALL);
        }
    }

    public void exeBoxGacha(int gachaTime)
    {
        this.drawGachaTime = gachaTime;
        base.myFSM.SendEvent("EXE_BOXGACHA");
    }

    private string getBgImgName()
    {
        int rewardPageBgId = this.eventDetailEnt.rewardPageBgId;
        int num2 = (rewardPageBgId <= 0) ? DEFAULT_BG_ID : rewardPageBgId;
        return string.Format(BG_ROOT, num2);
    }

    private BoxGachaTalkEntity GetBoxGachaTalk(int talkId, int[] rareIndex)
    {
        BoxGachaTalkEntity[] array = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaTalkMaster>(DataNameKind.Kind.BOX_GACHA_TALK).getTalkData(talkId);
        if (<>f__am$cache7A == null)
        {
            <>f__am$cache7A = talk => talk.isRare;
        }
        BoxGachaTalkEntity[] entityArray2 = Array.FindAll<BoxGachaTalkEntity>(array, <>f__am$cache7A);
        if (<>f__am$cache7B == null)
        {
            <>f__am$cache7B = talk => !talk.isRare;
        }
        BoxGachaTalkEntity[] entityArray3 = Array.FindAll<BoxGachaTalkEntity>(array, <>f__am$cache7B);
        if (((rareIndex != null) && (rareIndex.Length > 0)) && (entityArray2.Length > 0))
        {
            return entityArray2[UnityEngine.Random.Range(0, entityArray2.Length)];
        }
        return entityArray3[UnityEngine.Random.Range(0, entityArray3.Length)];
    }

    public void Init()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
        this.standFigureBack.Init();
        if ((SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(this.currentEventId) != null) && this.eventDetailEnt.isExchangeShop)
        {
            this.titleInfo.SetShopEventItem(this.currentEventId, true);
        }
        this.changeBtnInfoObj.SetActive(false);
        this.currentEventInfoObj.SetActive(false);
        this.eventPointRewardInfo.SetActive(false);
        this.eventGachaInfo.SetActive(false);
        this.eventMissionInfo.SetActive(false);
        this.resultNumbers = null;
        this.resNoList = null;
        this.arrowInfo.SetActive(false);
        this.sliderInfo.SetActive(false);
        this.beforeBoxGachaId = 0;
        this.beforeResultNumbers = null;
        this.setSvtFigure();
        this.setCurrentSvtVoice();
        this.isMultiEvent = false;
        this.setDispBtnInfo();
        SetBanner(this.eventLogoSprite, "event_logo_" + this.currentEventId);
        base.myFSM.SendEvent("SET_END");
    }

    private void OnCenterOnChildFinished()
    {
        if (this.centerChild != null)
        {
            Debug.Log("!!** OnCenterOnChildFinished");
            this.leftArrowBtn.enabled = true;
            this.rightArrowBtn.enabled = true;
            this.beforeMoveIdx = this.currentMoveIdx;
            this.currentScrollPos = this.mScroll.transform.localPosition;
            this.currentSvtInfoComp = this.centerChild.centeredObject.GetComponent<MultiSvtInfoComponent>();
            this.currentIdx = this.currentSvtInfoComp.getBannerIdx();
            this.currentMoveIdx = this.currentSvtInfoComp.getMoveBannerIdx();
            this.currentEventSvtId = this.currentSvtInfoComp.getGuideSvtInfo();
            this.svtFigure = this.currentSvtInfoComp.getSvtFigureR();
            this.setSliderIcon(this.currentIdx);
            this.setCtrSlideOn(true);
            EventRewardSaveData.GachaIdx = this.currentIdx;
            EventRewardSaveData.GachaMoveIdx = this.currentMoveIdx;
            EventRewardSaveData.SaveBoxGachaData(this.currentEventId);
            base.myFSM.SendEvent("CHANGE_SVT");
        }
    }

    public void OnClickBack()
    {
        this.stopSvtVoice();
        this.titleInfo.sendEvent("GOTO_TERMINAL");
    }

    public void OnClickDetail()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        string detailUrl = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BOX_GACHA_BASE_DETAIL).getEntityFromId<BoxGachaBaseDetailEntity>(this.currentBoxGachaBaseId).detailUrl;
        NoticeInfoComponent.TermID = "7";
        if (<>f__am$cache78 == null)
        {
            <>f__am$cache78 = delegate {
            };
        }
        WebViewManager.OpenWebView(string.Empty, <>f__am$cache78);
        if (<>f__am$cache79 == null)
        {
            <>f__am$cache79 = delegate {
            };
        }
        WebViewManager.OpenUniWebView(string.Empty, detailUrl, <>f__am$cache79);
    }

    public void OnClickGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.setTabInfo(StateType.EVENT_GACHA);
        this.setEventDisp(this.stateType);
    }

    public void onClickLeftArrow()
    {
        this.stopSvtVoice();
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        this.setCtrSlideOn(false);
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        int childCount = this.loopCtr.transform.childCount;
        int index = this.currentMoveIdx - 1;
        if (index < 0)
        {
            index = childCount - 1;
        }
        this.centerChild.CenterOn(this.loopCtr.transform.GetChild(index));
    }

    public void OnClickReward()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.setTabInfo(StateType.EVENT_REWARD);
        this.setEventDisp(this.stateType);
    }

    public void onClickRightArrow()
    {
        this.stopSvtVoice();
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        this.setCtrSlideOn(false);
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        int childCount = this.loopCtr.transform.childCount;
        int index = this.currentMoveIdx + 1;
        if (index >= childCount)
        {
            index = 0;
        }
        this.centerChild.CenterOn(this.loopCtr.transform.GetChild(index));
    }

    public void OnClickSvt()
    {
        if (!this.isVoicePlaying)
        {
            this.playSvtVoice();
        }
        else
        {
            this.svtVoiceCtr.stopVoice();
            this.setNormalFace();
        }
    }

    private void OnDragStarted()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.setCtrSlideOn(false);
        this.stopSvtVoice();
    }

    public void playEventMissionSvtVoice(string voiceID)
    {
        this.svtVoiceCtr.playVoice(voiceID);
    }

    private void playSvtVoice()
    {
        if (this.playBtnImg != null)
        {
            this.voicePlayEffect.SetActive(true);
            this.voicePlayEffect.transform.localScale = Vector3.one;
            this.voicePlayEffect.GetComponent<TweenScale>().tweenFactor = 0f;
            this.voicePlayEffect.GetComponent<TweenAlpha>().tweenFactor = 0f;
        }
        this.isVoicePlaying = true;
        this.svtVoiceCtr.playVoice(new System.Action(this.setNormalFace));
    }

    public void quit()
    {
        this.ReleaseSvtVoiceData();
        this.svtVoiceCtr.stopVoice();
        this.destroySvtFigure();
        this.backTexture.ClearImage();
        this.eventPointListViewManager.DestroyList();
        this.boxGachaListViewManager.DestroyList();
        EventRewardSaveData.SaveMissionData(this.currentEventId);
        this.eventMissionListViewManager.DestroyList();
        this.clearSvtList();
    }

    private void ReleaseSvtVoiceData()
    {
        if (this.currentVoiceDataList != null)
        {
            foreach (VoiceData data in this.currentVoiceDataList)
            {
                SoundManager.releaseAudioAssetStorage(data.DataName);
            }
            this.currentVoiceDataList.Clear();
        }
        if (this.requestVoiceData != null)
        {
            SoundManager.releaseAudioAssetStorage(this.requestVoiceData);
            this.requestVoiceData = null;
        }
    }

    public void requestBoxGachaDraw()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
        NetworkManager.getRequest<BoxGachaDrawRequest>(new NetworkManager.ResultCallbackFunc(this.callbackBoxGachaDraw)).beginRequest(this.currentBoxGachaId, this.drawGachaTime);
    }

    public void requestGachaReset()
    {
        this.stopSvtVoice();
        NetworkManager.getRequest<BoxGachaResetRequest>(new NetworkManager.ResultCallbackFunc(this.callbackBoxGachaReset)).beginRequest(this.currentBoxGachaId);
    }

    public void resetDisp()
    {
        this.boxGachaResultComp.clearResultList((System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.beforeBoxGachaId = this.currentBoxGachaEnt.id;
            this.beforeResultNumbers = this.resultNumbers;
            this.titleInfo.SetShopEventItem(this.currentEventId, false);
            this.setDisp(true);
            this.setEventBoxGachaList();
            SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
            base.myFSM.SendEvent("END_FADE");
        })));
    }

    public void resetGachaList()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.boxGachaListViewManager.DestroyList();
            this.resultNumbers = null;
            this.setEventBoxGachaList();
            base.myFSM.SendEvent("END_FADE");
        });
    }

    public static bool SetBanner(UISprite sprite, string bannerName)
    {
        if (requestBannaData == null)
        {
            if (sprite == null)
            {
                return true;
            }
            if (bannerData != null)
            {
                sprite.atlas = bannerData.GetObject<GameObject>().GetComponent<UIAtlas>();
                if (sprite.atlas.GetSprite(bannerName) != null)
                {
                    sprite.spriteName = bannerName;
                    return true;
                }
            }
        }
        return false;
    }

    private void setBoxGachaInfoImg()
    {
        string imgName = "img_gachatxt_" + this.currentEventId;
        string str2 = "img_txt_getpresent_" + this.currentEventId;
        setRewardInfoImg(this.singleGachaTimeSprite, imgName);
        setRewardInfoImg(this.singleInfoTxtSprite, str2);
        setRewardInfoImg(this.oneGachaTimeSprite, imgName);
        setRewardInfoImg(this.oneInfoTxtSprite, str2);
        setRewardInfoImg(this.multiGachaTimeSprite, imgName);
        setRewardInfoImg(this.multiInfoTxtSprite, str2);
        if (this.isMultiEvent)
        {
            setRewardInfoImg(this.multiLineUpSprite, "img_txt_lineup_" + this.currentEventId);
        }
        else
        {
            setRewardInfoImg(this.lineupInfoSprite, "img_txt_lineup_" + this.currentEventId);
        }
        setRewardInfoImg(this.resetTxtSprite, "img_resettxt_" + this.currentEventId);
    }

    private void setCenter()
    {
        Vector3[] worldCorners = this.indexPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.indexPanel.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
    }

    private void setChangeSvtDispCtr(bool isDisp)
    {
        this.gachaMainInfo.SetActive(isDisp);
        this.presentGachaBtn.GetComponent<BoxCollider>().enabled = isDisp;
        this.eventPointBtn.GetComponent<BoxCollider>().enabled = isDisp;
        this.titleInfo.SetEventBtnCollider(isDisp);
    }

    private void setCtrSlideOn(bool isDisp)
    {
        this.titleInfo.setBackBtnColliderEnable(isDisp);
        this.setChangeSvtDispCtr(isDisp);
        this.boxGachaListViewManager.itemColliderCtr(isDisp);
    }

    public void setCurrentBoxGachaInfo()
    {
        this.currentBoxGachaEnt = this.eventBoxGachaEntList[this.currentIdx];
        long[] args = new long[] { NetworkManager.UserId, (long) this.currentBoxGachaEnt.id };
        this.usrBoxGachaEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_BOX_GACHA).getEntityFromId<UserBoxGachaEntity>(args);
        if (this.usrBoxGachaEnt != null)
        {
            this.setGachaBase(this.currentBoxGachaEnt.baseIds[this.usrBoxGachaEnt.boxIndex], this.usrBoxGachaEnt.isReset);
        }
        else
        {
            this.setGachaBase(this.currentBoxGachaEnt.baseIds[0], false);
        }
        this.setGachaExeInfo();
    }

    private void setCurrentSvtVoice()
    {
        this.svtVoiceCtr.setSvtVoice(this.currentVoiceDataList[this.currentIdx].DataList, this.currentVoiceDataList[this.currentIdx].DataName);
        this.svtVoiceCtr.setFigure(this.svtFigure);
        this.playSvtVoice();
    }

    private void setDisp(bool isDisp)
    {
        this.titleInfo.gameObject.SetActive(isDisp);
        this.eventGachaInfo.SetActive(isDisp);
        this.changeTypeInfoObj.SetActive(isDisp);
        this.bgImgObj.SetActive(isDisp);
        MainMenuBar.setMenuActive(isDisp, null);
        this.setMultiSvtBannerEnable(!isDisp);
    }

    private void setDispBtnInfo()
    {
        if (this.isEventReward && this.isEventGacha)
        {
            this.isMultiEvent = true;
            this.multiChangeInfoLabel.text = LocalizationManager.Get("EVENT_REWARD_CHANGE_INFO");
            this.changeBtnInfoObj.SetActive(true);
            this.currentEventInfoObj.SetActive(false);
            this.eventMissionTxtInfo.SetActive(false);
            this.stateType = StateType.EVENT_GACHA;
            this.setEventPointInfoImg();
            this.setBoxGachaInfoImg();
            this.setTabInfo(this.stateType);
        }
        else if (this.isMission)
        {
            this.changeBtnInfoObj.SetActive(false);
            this.currentEventInfoObj.SetActive(false);
            this.stateType = StateType.EVENT_MISSION;
            this.setDispCurrentTxtInfo();
        }
        else
        {
            this.changeBtnInfoObj.SetActive(false);
            this.currentEventInfoObj.SetActive(true);
            this.stateType = !this.isEventReward ? StateType.EVENT_GACHA : StateType.EVENT_REWARD;
            this.setDispCurrentTxtInfo();
        }
        this.setEventDisp(this.stateType);
    }

    private void setDispCurrentTxtInfo()
    {
        switch (this.stateType)
        {
            case StateType.EVENT_REWARD:
                this.eventRewardTxtInfo.SetActive(true);
                this.eventGachaTxtInfo.SetActive(false);
                this.eventMissionTxtInfo.SetActive(false);
                this.setEventPointInfoImg();
                break;

            case StateType.EVENT_GACHA:
                this.eventRewardTxtInfo.SetActive(false);
                this.eventGachaTxtInfo.SetActive(true);
                this.eventMissionTxtInfo.SetActive(false);
                this.setBoxGachaInfoImg();
                break;

            case StateType.EVENT_MISSION:
                this.eventRewardTxtInfo.SetActive(false);
                this.eventGachaTxtInfo.SetActive(false);
                this.eventMissionTxtInfo.SetActive(true);
                break;
        }
    }

    public void setDispRePosition(int idx)
    {
        this.loopCtr.setScrollPos(idx);
        if (this.sliderInfo.activeSelf)
        {
            this.setSliderIcon(idx);
        }
        this.currentIdx = idx;
        this.currentMoveIdx = idx;
        this.befScrollPos = this.mScroll.transform.localPosition;
    }

    private void setEventBoxGachaList()
    {
        if (this.stateType == StateType.EVENT_GACHA)
        {
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_EVENT_GACHA))
            {
                if (<>f__am$cache77 == null)
                {
                    <>f__am$cache77 = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.EVENT_GACHA, TutorialFlag.Id.TUTORIAL_LABEL_EVENT_GACHA, <>f__am$cache77);
            }
            this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.EVENT_BOX_GACHA);
            this.titleInfo.setBackBtnSprite(true);
            this.titleInfo.SetHelpBtn(true);
            this.eventPointRewardInfo.SetActive(false);
            this.eventGachaInfo.SetActive(true);
            this.eventMissionInfo.SetActive(false);
            this.setCurrentBoxGachaInfo();
        }
    }

    private void setEventDisp(StateType type)
    {
        switch (type)
        {
            case StateType.EVENT_REWARD:
                this.eventPointRewardInfo.SetActive(true);
                this.eventGachaInfo.SetActive(false);
                this.eventMissionInfo.SetActive(false);
                this.setEventPointRewardList();
                break;

            case StateType.EVENT_GACHA:
                this.eventPointRewardInfo.SetActive(false);
                this.eventGachaInfo.SetActive(true);
                this.eventMissionInfo.SetActive(false);
                this.setEventBoxGachaList();
                break;

            case StateType.EVENT_MISSION:
                this.eventPointRewardInfo.SetActive(false);
                this.eventGachaInfo.SetActive(false);
                this.eventMissionInfo.SetActive(true);
                this.setEventMissionList();
                break;
        }
    }

    private void setEventMissionList()
    {
        if (this.stateType == StateType.EVENT_MISSION)
        {
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_EVENT_MISSION))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.EVENT_MISSION, TutorialFlag.Id.TUTORIAL_LABEL_EVENT_MISSION, delegate {
                    if (this.eventDetailEnt.tutorialImageIds[0] != string.Empty)
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenImageDialogWithAssets(this.eventDetailEnt.tutorialImageIds, null);
                    }
                });
            }
            this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.EVENT_MISSION);
            this.titleInfo.setBackBtnSprite(true);
            this.titleInfo.SetHelpBtn(true);
            this.setMissionCompleteNum();
            this.eventPointRewardInfo.SetActive(false);
            this.eventGachaInfo.SetActive(false);
            this.eventMissionInfo.SetActive(true);
            this.eventMissionListViewManager.CreateList(this.eventMissionEntList, this.currentEventId);
            this.eventMissionListViewManager.setMissionListIdx();
        }
    }

    private void setEventPointInfoImg()
    {
        if (this.isMultiEvent)
        {
            setRewardInfoImg(this.multiPointSprite, "img_pointtxt02_" + this.currentEventId);
        }
        else
        {
            setRewardInfoImg(this.pointInfoTxtSprite, "img_txt_pointreward_" + this.currentEventId);
            setRewardInfoImg(this.havePointInfoTxtSprite, "img_pointtxt01_" + this.currentEventId);
        }
    }

    private void setEventPointRewardList()
    {
        if (this.stateType == StateType.EVENT_REWARD)
        {
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_EVENT_REWARD))
            {
                if (<>f__am$cache76 == null)
                {
                    <>f__am$cache76 = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.EVENT_REWARD, TutorialFlag.Id.TUTORIAL_LABEL_EVENT_REWARD, <>f__am$cache76);
            }
            this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.EVENT_REWARD);
            this.titleInfo.setBackBtnSprite(true);
            this.titleInfo.SetHelpBtn(true);
            int num = 0;
            long[] args = new long[] { NetworkManager.UserId, (long) this.currentEventId };
            UserEventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT).getEntityFromId<UserEventEntity>(args);
            if (entity != null)
            {
                num = entity.value;
            }
            this.pointInfoLabel.text = $"{num:N0}";
            this.eventPointTxt.text = $"{num:N0}";
            this.eventPointRewardInfo.SetActive(true);
            this.eventGachaInfo.SetActive(false);
            this.eventMissionInfo.SetActive(false);
            this.eventPointListViewManager.CreateList(this.eventRewardEntList, num);
            this.eventPointListViewManager.setNextRewardInfo();
        }
    }

    private void setGachaBase(int baseId, bool isReset)
    {
        this.currentBoxGachaId = this.currentBoxGachaEnt.id;
        this.currentBoxGachaBaseId = baseId;
        BoxGachaBaseEntity[] baseData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BoxGachaBaseMaster>(DataNameKind.Kind.BOX_GACHA_BASE).getGachaBaseList(baseId);
        if (baseData != null)
        {
            this.beforeResultNumbers = (this.currentBoxGachaId == this.beforeBoxGachaId) ? this.resultNumbers : null;
            this.boxGachaListViewManager.CreateList(baseData, this.currentEventId, this.currentBoxGachaId, this.currentBoxGachaBaseId, this.beforeResultNumbers, isReset);
        }
        string str = "1";
        if (this.usrBoxGachaEnt != null)
        {
            str = this.usrBoxGachaEnt.resetNum.ToString();
        }
        this.eventGachaGuidNumLb.text = str;
        this.eventGachaNumLb.text = str;
    }

    private void setGachaExeInfo()
    {
        this.setGachaResourceCtr.init(this.currentBoxGachaEnt);
        int gachaItemMaxNum = this.boxGachaListViewManager.GetGachaItemMaxNum();
        int gachaItemCurrentNum = this.boxGachaListViewManager.GetGachaItemCurrentNum();
        int num3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.currentBoxGachaEnt.payTargetId).num / this.currentBoxGachaEnt.payValue;
        int num4 = (gachaItemCurrentNum >= gachaItemMaxNum) ? gachaItemMaxNum : gachaItemCurrentNum;
        int canDrawNum = (num4 >= MaxDrawCount) ? MaxDrawCount : num4;
        canDrawNum = (num3 >= canDrawNum) ? canDrawNum : num3;
        this.setGachaResourceCtr.setBoxGachaItemInfo(canDrawNum, new SetBoxGachaResourceControl.ClickDelegate(this.exeBoxGacha));
    }

    private void setGuideSvtVoice()
    {
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        ServantVoiceMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE);
        ServantVoiceEntity entity = null;
        this.currentVoiceDataList = new List<VoiceData>();
        this.downLoadCnt = 0;
        if (this.isEventGacha)
        {
            if (this.eventBoxGachaEntList != null)
            {
                foreach (BoxGachaEntity entity2 in this.eventBoxGachaEntList)
                {
                    int num2 = master.getVoiceId(entity2.guideImageId, entity2.guideLimitCount);
                    int num3 = master.getVoicePrefix(entity2.guideImageId, entity2.guideLimitCount);
                    entity = master2.getEntityFromId<ServantVoiceEntity>(num2, num3, 5);
                    if (entity != null)
                    {
                        this.requestVoiceData = entity.getVoiceAssetName();
                        this.voiceList = entity.getEventRewardVoiceList();
                        this.currentVoiceDataList.Add(new VoiceData(this.requestVoiceData, this.voiceList));
                    }
                }
            }
        }
        else
        {
            int num4 = master.getVoiceId(this.currentEventSvtId, this.currentEventSvtLimitCnt);
            int num5 = master.getVoicePrefix(this.currentEventSvtId, this.currentEventSvtLimitCnt);
            entity = master2.getEntityFromId<ServantVoiceEntity>(num4, num5, 5);
            if (entity != null)
            {
                this.requestVoiceData = entity.getVoiceAssetName();
                this.voiceList = entity.getEventRewardVoiceList();
                this.currentVoiceDataList.Add(new VoiceData(this.requestVoiceData, this.voiceList));
            }
        }
        string assetName = this.getBgImgName();
        this.backTexture.uvRect = DISP_RECT;
        this.backTexture.SetAssetImage(assetName, new System.Action(this.EndSetBackImg));
    }

    private void SetLoadRewardImgData()
    {
        if (!AssetManager.loadAssetStorage(REWARDIMG_ATLAS_PATH, new AssetLoader.LoadEndDataHandler(this.EndLoadRewardImg)))
        {
            this.EndLoadRewardImg(null);
        }
    }

    public void setMissionCompleteNum()
    {
        this.compMissionNum = 0;
        this.totalMissionNum = 0;
        this.compMissionNum = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getAchiveMissionNum(this.currentEventId);
        if (this.eventMissionEntList.Length > 0)
        {
            this.totalMissionNum = this.eventMissionEntList.Length;
        }
        this.eventMissionPrgTxt.text = $"{this.compMissionNum}/{this.totalMissionNum}";
    }

    private void setMultiSvtBannerEnable(bool isBoxGacha)
    {
        if ((this.multiSvtInfoList != null) && (this.multiSvtInfoList.Count > 0))
        {
            foreach (MultiSvtInfoComponent component in this.multiSvtInfoList)
            {
                component.setEnabledCollider(!isBoxGacha);
            }
            this.sliderInfo.SetActive(!isBoxGacha);
            this.arrowInfo.SetActive(!isBoxGacha);
        }
    }

    public void setMySvtFigure()
    {
        if (this.eventDetailEnt != null)
        {
            this.standFigureBack.Set(this.currentEventSvtId, 0, Face.Type.NORMAL, null);
        }
        else
        {
            this.standFigureBack.Set(DEFAULT_SVT_ID, 0, Face.Type.NORMAL, null);
        }
        this.svtFigure = this.standFigureBack.getSvtStandFigure();
        this.svtVoiceCtr.setFigure(this.svtFigure);
        this.svtVoiceCtr.playVoice(new System.Action(this.setNormalFace));
    }

    public void setNormalFace()
    {
        this.isVoicePlaying = false;
        if (this.playBtnImg != null)
        {
            this.voicePlayEffect.SetActive(false);
        }
        if (this.svtFigure != null)
        {
            this.svtFigure.SetFace(Face.Type.NORMAL);
        }
    }

    public void setPlaySvtVoice()
    {
        if (!this.befScrollPos.Equals(this.currentScrollPos))
        {
            this.setCurrentSvtVoice();
            this.befScrollPos = this.mScroll.transform.localPosition;
        }
    }

    public void setRecieveModifyItem()
    {
        this.eventMissionListViewManager.ModifyItem();
        this.eventMissionListViewManager.SetMode(EventMissionItemListViewManager.InitMode.INPUT, delegate {
            this.setMissionCompleteNum();
            base.myFSM.SendEvent("END_DISP");
        });
    }

    public void setResultList()
    {
        this.boxGachaListViewManager.DestroyList();
        int[] talkIds = this.currentBoxGachaEnt.talkIds;
        int index = Array.IndexOf<int>(this.currentBoxGachaEnt.baseIds, this.currentBoxGachaBaseId);
        int talkId = talkIds[index];
        this.boxGachaResultComp.init(this.currentEventSvtId, this.GetBoxGachaTalk(talkId, this.resultRareIdxs), this.resultIdList, this.resultRareIdxs, this.resNoList, this.svtFigure, this.currentBoxGachaBaseId, this.currentEventId);
    }

    public static void setRewardInfoImg(UISprite sprite, string imgName)
    {
        if ((sprite != null) && (rewardImgData != null))
        {
            sprite.atlas = rewardImgData.GetObject<GameObject>().GetComponent<UIAtlas>();
            if (sprite.atlas.GetSprite(imgName) != null)
            {
                sprite.spriteName = imgName;
            }
        }
    }

    private void setSliderIcon(int idx)
    {
        int childCount = this.sliderGrid.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            SelectBannerSliderIcon componentInChildren = this.sliderGrid.transform.GetChild(i).GetComponentInChildren<SelectBannerSliderIcon>();
            componentInChildren.setEnableOnImg(false);
            if (i == idx)
            {
                componentInChildren.setEnableOnImg(true);
            }
        }
    }

    private void setSvtFigure()
    {
        if (this.isEventGacha)
        {
            int length = this.eventBoxGachaEntList.Length;
            bool flag = false;
            if (length > 1)
            {
                length *= 2;
                flag = true;
            }
            if (flag)
            {
                this.multiSvtInfoList = new List<MultiSvtInfoComponent>();
                if (this.centerChild == null)
                {
                    this.centerChild = this.loopCtr.gameObject.AddComponent<UICenterOnChild>();
                }
                this.mScroll.onDragStarted = (UIScrollView.OnDragNotification) Delegate.Combine(this.mScroll.onDragStarted, new UIScrollView.OnDragNotification(this.OnDragStarted));
                this.centerChild.onFinished = (SpringPanel.OnFinished) Delegate.Combine(this.centerChild.onFinished, new SpringPanel.OnFinished(this.OnCenterOnChildFinished));
                this.loopCtr.itemSize = 860;
                int num2 = this.eventBoxGachaEntList.Length;
                int idx = 0;
                for (int i = 0; i < length; i++)
                {
                    GameObject obj2 = base.createObject(this.multiSvtObj, this.loopCtr.transform, null);
                    obj2.transform.localPosition = this.loopCtr.transform.localPosition;
                    obj2.transform.localScale = Vector3.one;
                    obj2.transform.localRotation = Quaternion.identity;
                    int num5 = i + 1;
                    obj2.name = "0" + num5;
                    if (num5 > 9)
                    {
                        obj2.name = "1" + num5;
                    }
                    MultiSvtInfoComponent item = obj2.GetComponent<MultiSvtInfoComponent>();
                    if (flag)
                    {
                        idx = ((length / 2) - num2) + i;
                        if (idx > (num2 - 1))
                        {
                            idx -= num2;
                        }
                    }
                    item.setCurrentBoxGachaInfo(this.eventBoxGachaEntList[idx], idx, i);
                    this.multiSvtInfoList.Add(item);
                }
                float num6 = this.sliderGrid.cellWidth * 0.5f;
                for (int j = 0; j < num2; j++)
                {
                    base.createObject(this.slideIndexPrefab, this.sliderGrid.transform, null).transform.localScale = Vector3.one;
                }
                this.loopCtr.SortAlphabetically();
                this.loopCtr.resetScroll();
                this.loopCtr.WrapContent();
                if (flag)
                {
                    this.arrowInfo.SetActive(true);
                    this.sliderInfo.SetActive(true);
                    this.sliderGrid.transform.localPosition = new Vector3(-(num6 * (num2 - 1)), this.center.y, this.center.z);
                    this.sliderGrid.repositionNow = true;
                    this.setSliderIcon(this.currentIdx);
                }
                this.setDispRePosition(this.currentIdx);
                this.svtFigure = this.multiSvtInfoList[this.currentIdx].getSvtFigureR();
            }
            else
            {
                this.standFigureBack.Set(this.currentEventSvtId, 0, Face.Type.NORMAL, null);
                this.svtFigure = this.standFigureBack.getSvtStandFigure();
            }
        }
        else
        {
            this.standFigureBack.Set(this.currentEventSvtId, 0, Face.Type.NORMAL, null);
            this.svtFigure = this.standFigureBack.getSvtStandFigure();
        }
    }

    private void setTabInfo(StateType type)
    {
        this.stateType = type;
        string str = "btn_bg_off";
        string str2 = "btn_bg_on";
        this.presentGachaBtn.isEnabled = true;
        this.presentGachaBtn.enabled = type != StateType.EVENT_GACHA;
        this.presentGachaBtnImg.spriteName = (type == StateType.EVENT_GACHA) ? str2 : str;
        string imgName = ((type == StateType.EVENT_GACHA) ? "btn_gacha_on_" : "btn_gacha_off_") + this.currentEventId;
        setRewardInfoImg(this.gachaBtnTxtSprite, imgName);
        this.presentGachaBtn.SetState(UICommonButtonColor.State.Normal, false);
        this.presentGachaInfoObj.SetActive(type == StateType.EVENT_GACHA);
        this.eventPointBtn.isEnabled = true;
        this.eventPointBtn.enabled = type != StateType.EVENT_REWARD;
        this.eventPointBtnImg.spriteName = (type == StateType.EVENT_REWARD) ? str2 : str;
        string str4 = ((type == StateType.EVENT_REWARD) ? "btn_reward_on_" : "btn_reward_off_") + this.currentEventId;
        setRewardInfoImg(this.pointBtnTxtSprite, str4);
        this.eventPointBtn.SetState(UICommonButtonColor.State.Normal, false);
        this.eventPointInfoObj.SetActive(type == StateType.EVENT_REWARD);
        this.setMultiSvtBannerEnable(type == StateType.EVENT_REWARD);
    }

    public void showBoxGachaResult()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.setDisp(false);
            base.myFSM.SendEvent("END_FADE");
        });
    }

    public void showNoticeMsg()
    {
        string message = string.Format(LocalizationManager.Get("BOX_GACHA_CHECK_PRESEN_NUM_TXT"), BalanceConfig.PresentBoxMax);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, message, new NotificationDialog.ClickDelegate(this.callBackNoticeDlg), -1);
    }

    public void showResetEndDlg()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, LocalizationManager.Get("BOX_GACHA_RESET_RESULT_MSG"), new NotificationDialog.ClickDelegate(this.closeReseEndDlg), -1);
    }

    public void Start()
    {
        base.Start();
    }

    public void stopSvtVoice()
    {
        this.svtVoiceCtr.stopVoice();
        this.svtFigure.SetFace(Face.Type.NORMAL);
    }

    public EventMissionItemListViewManager missionItemListViewManager
    {
        get
        {
            if (this.eventMissionListViewManager.isActiveAndEnabled)
            {
                return this.eventMissionListViewManager;
            }
            return null;
        }
    }

    public class resData
    {
        public int[] giftIds;
        public int[] rareIndex;
        public int[] resultNumbers;
    }

    public enum StateType
    {
        EVENT_REWARD,
        EVENT_GACHA,
        EXE_GACHA,
        EVENT_MISSION
    }

    public class VoiceData
    {
        public List<ServantVoiceData[]> DataList;
        public string DataName;

        public VoiceData(string voiceDataName, List<ServantVoiceData[]> voiceDataList)
        {
            this.DataName = voiceDataName;
            this.DataList = voiceDataList;
        }
    }
}

