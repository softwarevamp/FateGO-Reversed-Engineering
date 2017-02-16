using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyRoomControl : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache3C;
    [CompilerGenerated]
    private static System.Action <>f__am$cache3D;
    [CompilerGenerated]
    private static System.Action <>f__am$cache3E;
    [CompilerGenerated]
    private static CommonConfirmDialog.ClickDelegate <>f__am$cache3F;
    [CompilerGenerated]
    private static System.Action <>f__am$cache40;
    private string asstName;
    [SerializeField]
    protected ExUITexture backTexture;
    public static readonly string BGM_NAME = "BGM_MYROOM_1";
    public UISprite changeBGBtnImg;
    public UISprite changeBtnImg;
    public GameObject changeLimitBtn;
    public ContinueDeviceComponent continueDeviceComp;
    public static readonly int DEFAULT_BG_ID = 0x28a0;
    public static readonly Rect DISP_RECT = new Rect(0f, 0.41f, 1f, 0.5869f);
    private EventMaster eventMaster;
    [SerializeField]
    protected FavoriteChangeComponent favoriteChangeComp;
    private int figureSvtId;
    private int figureSvtLimitCnt;
    private int figureSvtLv;
    private long fvrUsrSvtId;
    private bool isExistVoiceData;
    private bool isVoicePlaying;
    public static MVSTATE m_state;
    public MaterialCollectionComponent materialCollectionComp;
    public Transform MaterialGachaBgRoot;
    [SerializeField]
    protected GameObject materialObj;
    public BattleSetupInfo mBattleSetupInfo;
    [SerializeField]
    private GameObject mBlocker;
    [SerializeField]
    private Camera mCamera2DUI;
    private CStateManager<MyRoomControl> mFSM;
    [SerializeField]
    private GameObject mHidePos;
    [SerializeField]
    private GameObject mMainObj;
    [SerializeField]
    private MaterialEventLogListViewManager mMaterialEventLogListViewManager;
    [SerializeField]
    protected GameObject moveBtnObj;
    [SerializeField]
    protected UILabel moveBtnTxt;
    private AssetData mScriptFileListAssetData;
    private string[] mScriptFileListStrs;
    [SerializeField]
    private GameObject mShowPos;
    public MVSTATE mState;
    private MyRoomStateMaterial mStateMaterial = new MyRoomStateMaterial();
    public MstProfileComponent mstPfComp;
    public GameObject mstpfObj;
    [SerializeField]
    private UIAtlas myRoomAtlas;
    public MyRoomData myRoomData;
    public PlayMakerFSM myRoomFsm;
    public MyRoomListControl myRoomListCtr;
    public NoticeInfoComponent noticeComp;
    [SerializeField]
    protected GameObject noticeObj;
    public SetGameOptionComponent optionComp;
    public UISprite playBtnImg;
    public SerialCodeComponent serialCodeComp;
    public StandFigureBack standFigureBack;
    public BoxCollider svtClickCollider;
    public MyRoomSvtControl svtCtr;
    private UIStandFigureR svtFigure;
    public GameObject svtObj;
    public BoxCollider titleBtnCollider;
    public TitleInfoControl titleInfo;
    private MENUTYPE type;
    private UserGameEntity usrData;
    public UserItemListViewManager usrItemListViewManager;
    public BoxCollider voiceClickCollider;
    private List<ServantVoiceData[]> voiceList;
    public GameObject voicePlayBtn;
    public GameObject voicePlayEffect;

    public void BlockTouch()
    {
        this.changeBGBtnImg.color = Color.gray;
        if (this.myRoomData.getUsrSvtData(this.fvrUsrSvtId).limitCount > 0)
        {
            this.changeBtnImg.color = Color.gray;
        }
        this.mBlocker.SetActive(true);
    }

    private void callbackChangeName(string result)
    {
        if (result.Equals("ok"))
        {
            this.myRoomData.setUserInfoData();
            this.myRoomFsm.SendEvent("REQUEST_OK");
        }
        else
        {
            this.mstPfComp.resetInput();
            this.myRoomFsm.SendEvent("REQUEST_NG");
        }
    }

    private void callbackVoicePlayed(string result)
    {
    }

    public void closeMaterialTop()
    {
        this.SetState(STATE.ETC);
    }

    private void destroySvtFigure()
    {
        if (this.svtFigure != null)
        {
            UnityEngine.Object.Destroy(this.svtFigure.gameObject);
            this.svtFigure = null;
        }
    }

    private void End_LoadScriptFileListAssetData(AssetData asset_data)
    {
        this.mScriptFileListAssetData = asset_data;
        string decryptObjectText = this.mScriptFileListAssetData.GetDecryptObjectText(null);
        char[] separator = new char[] { '\n' };
        this.mScriptFileListStrs = decryptObjectText.Replace("\r\n", "\n").Split(separator);
        this.mStateMaterial.Init(this);
        this.myRoomFsm.SendEvent("GO_NEXT");
    }

    private void EndLoad()
    {
        this.svtCtr.setSvtVoice(this.voiceList, this.asstName);
        if (<>f__am$cache3E == null)
        {
            <>f__am$cache3E = () => SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache3E);
        this.myRoomFsm.SendEvent("LOAD_END");
    }

    private string getBgImgName()
    {
        int num = this.eventMaster.getMyRoomBgImgId();
        int num2 = (num <= 0) ? DEFAULT_BG_ID : num;
        return $"Back/back{num2}";
    }

    public Camera GetCamera2DUI() => 
        this.mCamera2DUI;

    public MaterialEventLogListViewManager GetMaterialEventLogListViewManager() => 
        this.mMaterialEventLogListViewManager;

    public string getMyRoomBgm()
    {
        int id = this.eventMaster.getMyRoomBgmId();
        string fileName = BGM_NAME;
        if (id != 0)
        {
            BgmEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BGM).getEntityFromId<BgmEntity>(id);
            if (entity != null)
            {
                fileName = entity.fileName;
            }
        }
        return fileName;
    }

    public string[] GetScriptFileListStrs() => 
        this.mScriptFileListStrs;

    public static MVSTATE getState() => 
        m_state;

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    private ServantVoiceEntity getSvtVoiceEnt()
    {
        this.usrData = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.fvrUsrSvtId = this.usrData.favoriteUserSvtId;
        if (this.fvrUsrSvtId <= 0L)
        {
            return null;
        }
        UserServantEntity entity = this.myRoomData.getUsrSvtData(this.fvrUsrSvtId);
        if (entity != null)
        {
            this.figureSvtId = entity.svtId;
            this.figureSvtLimitCnt = entity.limitCount;
            if (this.figureSvtLimitCnt == 0)
            {
                this.changeBtnImg.color = Color.gray;
            }
            else
            {
                this.changeBtnImg.color = Color.white;
            }
            this.figureSvtLv = entity.lv;
        }
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(this.figureSvtId, this.figureSvtLimitCnt);
        int num2 = master.getVoicePrefix(this.figureSvtId, this.figureSvtLimitCnt);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num, num2, 1);
    }

    public int getVoiceSvtId()
    {
        if (this.fvrUsrSvtId > 0L)
        {
            UserServantEntity entity = this.myRoomData.getUsrSvtData(this.fvrUsrSvtId);
            if (entity != null)
            {
                return entity.svtId;
            }
        }
        return 0;
    }

    public void GoToTitle()
    {
        this.stopSvtVoice();
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        string title = string.Empty;
        string message = LocalizationManager.Get("MYROOM_TITLE_CONFIRM");
        if (<>f__am$cache3F == null)
        {
            <>f__am$cache3F = delegate (bool isDecide) {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
                if (isDecide)
                {
                    BSGameSdk.logout();
                    DataManager.isLogin = false;
                    if (<>f__am$cache40 == null)
                    {
                        <>f__am$cache40 = () => SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
                    }
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, <>f__am$cache40);
                }
            };
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(title, message, <>f__am$cache3F);
    }

    public void hideContinueDevice()
    {
        this.continueDeviceComp.hideMenu();
    }

    public void hideFavoriteSvt()
    {
        ServantVoiceEntity entity = this.getSvtVoiceEnt();
        if (entity != null)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
            this.ReleaseSvtVoiceData();
            this.voiceList = entity.getHomeVoiceList();
            this.asstName = entity.getVoiceAssetName();
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, delegate {
                this.svtCtr.setSvtVoice(this.voiceList, this.asstName);
                this.isExistVoiceData = true;
            }, SoundManager.CueType.ALL);
        }
        else
        {
            this.isExistVoiceData = false;
        }
        this.setMySvtFigure(delegate {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
            this.myRoomFsm.SendEvent("CLOSE_MENU");
        });
    }

    public void hideMaterialCollection()
    {
        this.materialCollectionComp.hideMenu();
    }

    public void hideOption()
    {
        this.optionComp.hideGameOption();
    }

    public void hideProfile()
    {
        this.mstPfComp.hideMstProfile();
        this.mstpfObj.SetActive(false);
    }

    public void hideSerialCode()
    {
        this.serialCodeComp.hideMenu();
    }

    public void hideSvtFigure()
    {
        this.playBtnImg.color = Color.gray;
        this.BlockTouch();
        this.standFigureBack.Fadeout(() => this.myRoomFsm.SendEvent("HIDE_END"));
    }

    public void hideUsrItemList()
    {
        this.usrItemListViewManager.DestroyList();
        this.usrItemListViewManager.gameObject.SetActive(false);
    }

    public void initMyRoom()
    {
        this.standFigureBack.Init();
        if (this.mBattleSetupInfo != null)
        {
            this.myRoomFsm.SendEvent("GO_NEXT");
        }
        else
        {
            if (this.mFSM == null)
            {
                this.mFSM = new CStateManager<MyRoomControl>(this, 2);
                this.mFSM.add(0, this.mStateMaterial);
                this.mFSM.add(1, new StateEtc());
            }
            this.SetState(STATE.ETC);
            SingletonTemplate<QuestTree>.Instance.Init();
            this.myRoomData.initMyRoomData();
            this.eventMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT);
            this.titleInfo.setTitleInfo(this.myRoomFsm, true, null, TitleInfoControl.TitleKind.MYROOM);
            this.titleInfo.setBackBtnDepth(30);
            this.myRoomListCtr.Setup();
            this.svtClickCollider.enabled = false;
            this.voiceClickCollider.enabled = false;
            this.isExistVoiceData = true;
            this.setUserStInfoView();
            this.mMainObj.SetLocalPosition(this.mShowPos.GetLocalPosition());
            if (<>f__am$cache3C == null)
            {
                <>f__am$cache3C = delegate {
                };
            }
            this.mMaterialEventLogListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.NONE, <>f__am$cache3C);
            SoundManager.playBgm(this.getMyRoomBgm());
            string assetName = this.getBgImgName();
            this.backTexture.uvRect = DISP_RECT;
            this.backTexture.SetAssetImage(assetName, new System.Action(this.LoadScriptFileListAssetData));
            this.mState = MVSTATE.INIT;
            m_state = this.mState;
            this.changeLimitBtn.SetActive(true);
            this.moveBtnObj.SetActive(true);
            this.moveBtnTxt.text = "INIT";
        }
    }

    private bool IsScrollviewInShowPosition()
    {
        if ((this.myRoomFsm.ActiveStateName != "Wait_Action") && (this.myRoomFsm.ActiveStateName != "Help"))
        {
            return false;
        }
        if (!(this.myRoomListCtr.transform.position == this.mShowPos.transform.position) && this.mMaterialEventLogListViewManager.IsDoing_Slide)
        {
            return false;
        }
        return true;
    }

    private void LoadScriptFileListAssetData()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
        this.ReleaseScriptFileListAssetData();
        AssetManager.loadAssetStorage(ScriptManager.textPath + "/ScriptFileList", new AssetLoader.LoadEndDataHandler(this.End_LoadScriptFileListAssetData));
    }

    [DllImport("__Internal")]
    public static extern void logout();
    private void moveControl(GameObject target, Vector3 movePos)
    {
        TweenPosition position = TweenPosition.Begin(target, 0.2f, movePos);
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "ReleaseTouch";
        position.method = UITweener.Method.EaseInOut;
    }

    public void OnClickBack()
    {
        if (!this.mMaterialEventLogListViewManager.IsDoing_Slide)
        {
            if (this.GetState() == STATE.MATERIAL)
            {
                if (this.mState != MVSTATE.INIT)
                {
                    this.setDefSvtPos();
                    return;
                }
                if (this.mStateMaterial.Back())
                {
                    return;
                }
            }
            string msg = "CLICK_BACK";
            bool flag = false;
            switch (this.type)
            {
                case MENUTYPE.MAIN:
                    msg = "CLICK_BACK";
                    break;

                case MENUTYPE.MATERIAL:
                    flag = true;
                    msg = "CLOSE_MATERIAL";
                    break;

                case MENUTYPE.MATERIAL_COLLECTION:
                    msg = "CLOSE_MATERIAL_COLLECTION";
                    break;

                case MENUTYPE.ITEM:
                    msg = "CLOSE_ITEMLIST";
                    break;

                case MENUTYPE.PROFILE:
                    msg = "CLOSE_CHANGE_PROFILE";
                    break;

                case MENUTYPE.OPTION:
                    msg = "CLOSE_GAMEOPTION";
                    break;

                case MENUTYPE.NOTICE:
                    flag = true;
                    msg = "CLOSE_NOTICE";
                    break;

                case MENUTYPE.SERIALCODE:
                    msg = "CLOSE_SERIAL_CODE";
                    break;

                case MENUTYPE.CONTINUEDEVICE:
                    msg = "CLOSE_CONTINUE_DEVICE";
                    break;

                case MENUTYPE.FAVORITE_CHANGE:
                    msg = "CLOSE_FAVORITE_CHANGE";
                    break;
            }
            Debug.Log("**** OnClickBack : " + msg);
            if ((this.mState != MVSTATE.INIT) && flag)
            {
                this.setDefSvtPos();
            }
            else
            {
                this.titleInfo.sendEvent(msg);
            }
            if ((this.myRoomFsm.ActiveStateName != "State 5") && (this.myRoomData.getUsrSvtData(this.fvrUsrSvtId).limitCount > 0))
            {
                this.changeBtnImg.color = Color.white;
            }
        }
    }

    public void OnClickChangeLimit()
    {
        if (this.IsScrollviewInShowPosition() && (this.myRoomData.getUsrSvtData(this.fvrUsrSvtId).limitCount > 0))
        {
            this.BlockTouch();
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.myRoomFsm.SendEvent("CHANGE_LIMIT");
        }
    }

    public void OnClickMoveBg()
    {
        if (this.IsScrollviewInShowPosition())
        {
            this.BlockTouch();
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            GameObject mMainObj = this.mMainObj;
            Vector3 localPosition = this.mShowPos.GetLocalPosition();
            Vector3 movePos = this.mHidePos.GetLocalPosition();
            if (this.type == MENUTYPE.MATERIAL)
            {
                mMainObj = this.materialObj;
                localPosition = new Vector3(250f, mMainObj.transform.localPosition.y, mMainObj.transform.localPosition.z);
                movePos = new Vector3(760f, mMainObj.transform.localPosition.y, mMainObj.transform.localPosition.z);
            }
            if (this.type == MENUTYPE.NOTICE)
            {
                mMainObj = this.noticeObj;
            }
            UITexture componentInChildren = this.svtFigure.GetComponentInChildren<UITexture>();
            Vector3 vector3 = new Vector3(-componentInChildren.transform.localPosition.x, this.svtObj.transform.localPosition.y, this.svtObj.transform.localPosition.z);
            switch (this.mState)
            {
                case MVSTATE.INIT:
                    this.moveControl(mMainObj, movePos);
                    this.moveControl(this.svtObj, vector3);
                    this.mState = MVSTATE.ONLY_SVT;
                    this.moveBtnTxt.text = "ON_SVT";
                    m_state = this.mState;
                    break;

                case MVSTATE.ONLY_SVT:
                    this.stopSvtVoice();
                    this.standFigureBack.Fadeout(() => this.ReleaseTouch());
                    this.mState = MVSTATE.ONLY_BG;
                    m_state = this.mState;
                    this.moveBtnTxt.text = "ON_BG";
                    this.voicePlayBtn.SetActive(false);
                    this.changeLimitBtn.SetActive(false);
                    break;

                case MVSTATE.ONLY_BG:
                    this.moveControl(mMainObj, localPosition);
                    this.standFigureBack.Fadein(delegate {
                        this.voicePlayBtn.SetActive(true);
                        this.changeLimitBtn.SetActive(true);
                        this.ReleaseTouch();
                    });
                    this.mState = MVSTATE.INIT;
                    m_state = this.mState;
                    this.moveBtnTxt.text = "INIT";
                    break;
            }
        }
    }

    public void OnClickSvt()
    {
        if (this.IsScrollviewInShowPosition())
        {
            if (!this.isVoicePlaying)
            {
                this.isVoicePlaying = true;
                this.myRoomFsm.SendEvent("PLAY_VOICE");
            }
            else
            {
                this.isVoicePlaying = false;
                this.svtCtr.stopVoice();
                this.setNormalFace();
            }
        }
    }

    private void OnDestroy()
    {
        this.ReleaseScriptFileListAssetData();
    }

    public void openFavoriteSvt()
    {
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.FAVORITE_CHANGE);
        this.favoriteChangeComp.showFavoriteChangeInfo();
    }

    public void PlayOpening(string assetPath, Color col, FullScreenMovieControlMode ctrlMode, FullScreenMovieScalingMode sclMode)
    {
        base.StartCoroutine(this.PlayOpeningCoroutine("file://" + assetPath, col, ctrlMode, sclMode));
    }

    [DebuggerHidden]
    private IEnumerator PlayOpeningCoroutine(string path, Color col, FullScreenMovieControlMode ctrlMode, FullScreenMovieScalingMode sclMode) => 
        new <PlayOpeningCoroutine>c__Iterator35 { 
            path = path,
            col = col,
            ctrlMode = ctrlMode,
            sclMode = sclMode,
            <$>path = path,
            <$>col = col,
            <$>ctrlMode = ctrlMode,
            <$>sclMode = sclMode
        };

    public void playSvtVoice()
    {
        int svtId = this.getVoiceSvtId();
        if (svtId > 0)
        {
            string str = this.svtCtr.playVoice(1);
            if (!string.IsNullOrEmpty(str))
            {
                int num2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<VoiceMaster>(DataNameKind.Kind.VOICE).getFlagRequestNumber(svtId, str, true);
                Debug.Log(string.Concat(new object[] { "play voice svtId ", svtId, " voice label ", str, " voicePlayed ", num2 }));
                if (num2 > 0)
                {
                    TopMyRoomRequest request = NetworkManager.getRequest<TopMyRoomRequest>(new NetworkManager.ResultCallbackFunc(this.callbackVoicePlayed));
                    int[][] voicePlayedList = new int[1][];
                    voicePlayedList[0] = new int[] { svtId, num2 };
                    request.beginRequest(voicePlayedList);
                }
            }
        }
    }

    public void quit()
    {
        this.ReleaseSvtVoiceData();
        this.destroySvtFigure();
        this.backTexture.ClearImage();
        this.mMaterialEventLogListViewManager.DestroyList();
    }

    public void reflectionGameOption()
    {
        this.optionComp.reflectionGameOption();
    }

    private void ReleaseScriptFileListAssetData()
    {
        if (this.mScriptFileListAssetData != null)
        {
            AssetManager.releaseAsset(this.mScriptFileListAssetData);
            this.mScriptFileListAssetData = null;
        }
    }

    private void ReleaseSvtVoiceData()
    {
        if (this.asstName != null)
        {
            SoundManager.releaseAudioAssetStorage(this.asstName);
            this.asstName = null;
            Debug.Log("!! ** !! ReleaseSvtVoiceData: " + this.asstName);
        }
    }

    public void ReleaseTouch()
    {
        if ((this.myRoomFsm.ActiveStateName != "State 5") && (this.myRoomData.getUsrSvtData(this.fvrUsrSvtId).limitCount > 0))
        {
            this.changeBtnImg.color = Color.white;
        }
        this.changeBGBtnImg.color = Color.white;
        this.mBlocker.SetActive(false);
    }

    public void requestChangeName()
    {
        int genderType = this.myRoomData.getUsrData().genderType;
        string changeName = this.myRoomFsm.Fsm.Variables.GetFsmString("ChangeUserName").Value;
        NetworkManager.getRequest<UserNameChangeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackChangeName)).beginRequest(changeName, genderType, 0);
    }

    public void resetProfileInfo()
    {
        this.mstPfComp.showMstProfile(this.myRoomData.getMstInfoData());
    }

    public void resetSvtVoiceData()
    {
        if (this.mBattleSetupInfo != null)
        {
            this.myRoomFsm.SendEvent("LOAD_END");
        }
        else
        {
            ServantVoiceEntity entity = this.resetSvtVoiceEnt();
            if (entity != null)
            {
                this.voiceList = entity.getHomeVoiceList();
                this.asstName = entity.getVoiceAssetName();
                SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, delegate {
                    this.svtCtr.setSvtVoice(this.voiceList, this.asstName);
                    this.myRoomFsm.SendEvent("LOAD_END");
                }, SoundManager.CueType.ALL);
            }
            else
            {
                this.isExistVoiceData = false;
                this.myRoomFsm.SendEvent("LOAD_END");
            }
        }
    }

    private ServantVoiceEntity resetSvtVoiceEnt()
    {
        this.usrData = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.fvrUsrSvtId = this.usrData.favoriteUserSvtId;
        if (this.fvrUsrSvtId <= 0L)
        {
            return null;
        }
        UserServantEntity entity = this.myRoomData.getUsrSvtData(this.fvrUsrSvtId);
        if (entity != null)
        {
            int imageLimitCount = ImageLimitCount.GetImageLimitCount(this.figureSvtId, this.figureSvtLimitCnt);
            for (int i = imageLimitCount; i == imageLimitCount; i = ImageLimitCount.GetImageLimitCount(this.figureSvtId, this.figureSvtLimitCnt))
            {
                this.figureSvtLimitCnt++;
                if (this.figureSvtLimitCnt > entity.limitCount)
                {
                    this.figureSvtLimitCnt = 0;
                }
            }
            this.changeBtnImg.color = Color.white;
            this.playBtnImg.color = Color.white;
            this.ReleaseTouch();
        }
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num3 = master.getVoiceId(this.figureSvtId, this.figureSvtLimitCnt);
        int num4 = master.getVoicePrefix(this.figureSvtId, this.figureSvtLimitCnt);
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num3, num4, 1);
    }

    public void setContinueDevice()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.CONTINUEDEVICE;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.CONTINUE_DEVICE);
        this.setSvtFigureActive(this.type);
        this.continueDeviceComp.showMenu();
    }

    private void setDefSvtPos()
    {
        this.voicePlayBtn.SetActive(true);
        this.changeLimitBtn.SetActive(true);
        if (this.mState != MVSTATE.INIT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            GameObject mMainObj = this.mMainObj;
            Vector3 localPosition = this.mShowPos.GetLocalPosition();
            if (this.type == MENUTYPE.MATERIAL)
            {
                mMainObj = this.materialObj;
                localPosition = new Vector3(250f, mMainObj.transform.localPosition.y, mMainObj.transform.localPosition.z);
            }
            if (this.type == MENUTYPE.NOTICE)
            {
                mMainObj = this.noticeObj;
            }
            if (this.mState == MVSTATE.ONLY_SVT)
            {
                Vector3 movePos = new Vector3(-512f, this.svtObj.transform.localPosition.y, this.svtObj.transform.localPosition.z);
                this.moveControl(this.svtObj, movePos);
            }
            if (this.mState == MVSTATE.ONLY_BG)
            {
                this.standFigureBack.Fadein(null);
                this.voicePlayBtn.SetActive(true);
            }
            this.moveControl(mMainObj, localPosition);
            this.mState = MVSTATE.INIT;
            m_state = this.mState;
        }
    }

    public void setEnalbeTitleBtn(bool isEnabled)
    {
        Debug.Log("**** setEnalbeTitleBtn : " + isEnabled);
        this.titleBtnCollider.enabled = isEnabled;
    }

    public void setFaceType()
    {
        if (this.playBtnImg != null)
        {
            this.voicePlayEffect.SetActive(true);
            this.voicePlayEffect.transform.localScale = Vector3.one;
            this.voicePlayEffect.GetComponent<TweenScale>().tweenFactor = 0f;
            this.voicePlayEffect.GetComponent<TweenAlpha>().tweenFactor = 0f;
        }
    }

    public void setFavoriteSvt()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.FAVORITE_CHANGE;
        this.setSvtFigureActive(this.type);
    }

    public void setGameOption()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.OPTION;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.GAMEOPTION);
        this.setSvtFigureActive(this.type);
        this.optionComp.showGameOption();
    }

    public void setMaterial()
    {
        this.stopSvtVoice();
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.MYROOM_MATERIAL);
        this.type = MENUTYPE.MATERIAL;
        this.setSvtFigureActive(this.type);
    }

    public void setMaterialCollection()
    {
        this.stopSvtVoice();
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.MYROOM_MATERIAL);
        this.type = MENUTYPE.MATERIAL_COLLECTION;
        this.setSvtFigureActive(this.type);
        this.materialCollectionComp.showMenu();
    }

    public void setMaterialTop()
    {
        this.SetState(STATE.MATERIAL);
    }

    public void setMySvtFigure()
    {
        this.setMySvtFigure(delegate {
            if (this.type == MENUTYPE.NOTICE)
            {
                this.myRoomFsm.SendEvent("LOAD_END_TO_HELP");
            }
            else
            {
                this.myRoomFsm.SendEvent("LOAD_END");
            }
            this.ReleaseTouch();
        });
    }

    public void setMySvtFigure(System.Action end_act)
    {
        <setMySvtFigure>c__AnonStorey8A storeya = new <setMySvtFigure>c__AnonStorey8A {
            end_act = end_act,
            <>f__this = this
        };
        this.standFigureBack.Set(this.figureSvtId, this.figureSvtLimitCnt, this.figureSvtLv, Face.Type.NORMAL, new System.Action(storeya.<>m__123));
    }

    public void setNormalFace()
    {
        this.svtFigure.SetFace(Face.Type.NORMAL);
        this.isVoicePlaying = false;
        if (this.playBtnImg != null)
        {
            this.voicePlayEffect.SetActive(false);
        }
    }

    public void setNoticeInfo()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.NOTICE;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.MYROOM_NOTICE);
        this.setSvtFigureActive(this.type);
        this.noticeComp.setNoticeInfo();
    }

    public void setProfileInfo()
    {
        this.mstpfObj.SetActive(true);
        this.stopSvtVoice();
        this.type = MENUTYPE.PROFILE;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.PROFILE);
        this.setSvtFigureActive(this.type);
        this.mstPfComp.showMstProfile(this.myRoomData.getMstInfoData());
    }

    public void setSerialCode()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.SERIALCODE;
        this.setSvtFigureActive(this.type);
        this.serialCodeComp.showMenu();
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    private void setSvtFigureActive(MENUTYPE type)
    {
        switch (type)
        {
            case MENUTYPE.MAIN:
            case MENUTYPE.MATERIAL:
                MainMenuBar.setMenuActive(true, null);
                this.standFigureBack.Fadein(null);
                this.voicePlayBtn.SetActive(true);
                this.moveBtnObj.SetActive(true);
                this.changeLimitBtn.SetActive(true);
                break;

            case MENUTYPE.MATERIAL_COLLECTION:
            case MENUTYPE.ITEM:
            case MENUTYPE.PROFILE:
            case MENUTYPE.OPTION:
            case MENUTYPE.CONTINUEDEVICE:
            case MENUTYPE.FAVORITE_CHANGE:
                MainMenuBar.setMenuActive(false, null);
                this.standFigureBack.Fadeout(null);
                this.voicePlayBtn.SetActive(false);
                this.moveBtnObj.SetActive(false);
                this.changeLimitBtn.SetActive(false);
                break;

            case MENUTYPE.NOTICE:
                MainMenuBar.setMenuActive(false, null);
                this.standFigureBack.Fadein(null);
                break;

            case MENUTYPE.SERIALCODE:
                MainMenuBar.setMenuActive(false, null);
                this.voicePlayBtn.SetActive(false);
                this.moveBtnObj.SetActive(false);
                this.changeLimitBtn.SetActive(false);
                break;
        }
    }

    private int setSvtFriendShipLv(int hSvtId) => 
        this.myRoomData.getSvtFriendshipLv(hSvtId);

    public void setSvtVoiceData()
    {
        if (this.mBattleSetupInfo != null)
        {
            this.myRoomFsm.SendEvent("LOAD_END");
        }
        else
        {
            ServantVoiceEntity entity = this.getSvtVoiceEnt();
            if (entity != null)
            {
                this.voiceList = entity.getHomeVoiceList();
                this.asstName = entity.getVoiceAssetName();
                SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, new System.Action(this.EndLoad), SoundManager.CueType.ALL);
            }
            else
            {
                this.isExistVoiceData = false;
                if (<>f__am$cache3D == null)
                {
                    <>f__am$cache3D = () => SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
                }
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache3D);
                this.myRoomFsm.SendEvent("LOAD_END");
                this.isVoicePlaying = false;
            }
        }
    }

    public void setUserStInfoView()
    {
        this.type = MENUTYPE.MAIN;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.MYROOM);
        this.setSvtFigureActive(this.type);
    }

    public void setUsrItemList()
    {
        this.stopSvtVoice();
        this.type = MENUTYPE.ITEM;
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.MYROOM_USERITEM);
        this.setSvtFigureActive(this.type);
        this.usrItemListViewManager.CreateList();
        this.usrItemListViewManager.SetMode(UserItemListViewManager.InitMode.INPUT);
    }

    private void showSvtFigure(int svtId, int limitCnt, int lv)
    {
        if (this.svtFigure != null)
        {
            this.destroySvtFigure();
            this.svtFigure = null;
        }
        if (svtId > 0)
        {
            this.svtFigure = StandFigureManager.CreateRenderPrefab(this.svtObj, svtId, limitCnt, lv, Face.Type.NORMAL, 10, null);
        }
    }

    public void stopSvtVoice()
    {
        this.svtCtr.stopVoice();
        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(null);
        this.setNormalFace();
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    [CompilerGenerated]
    private sealed class <PlayOpeningCoroutine>c__Iterator35 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Color <$>col;
        internal FullScreenMovieControlMode <$>ctrlMode;
        internal string <$>path;
        internal FullScreenMovieScalingMode <$>sclMode;
        internal Color col;
        internal FullScreenMovieControlMode ctrlMode;
        internal string path;
        internal FullScreenMovieScalingMode sclMode;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    Handheld.PlayFullScreenMovie(this.path, this.col, this.ctrlMode, this.sclMode);
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_0085;

                case 1:
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 2;
                    goto Label_0085;

                case 2:
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0085:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <setMySvtFigure>c__AnonStorey8A
    {
        internal MyRoomControl <>f__this;
        internal System.Action end_act;

        internal void <>m__123()
        {
            this.<>f__this.svtFigure = this.<>f__this.standFigureBack.getSvtStandFigure();
            this.<>f__this.svtCtr.setFigure(this.<>f__this.svtFigure);
            this.<>f__this.setSvtFigureActive(this.<>f__this.type);
            this.<>f__this.svtClickCollider.enabled = this.<>f__this.isExistVoiceData;
            this.<>f__this.voiceClickCollider.enabled = this.<>f__this.isExistVoiceData;
            if (!this.<>f__this.isExistVoiceData)
            {
                this.<>f__this.setFaceType();
            }
            else
            {
                this.<>f__this.setNormalFace();
            }
            this.<>f__this.favoriteChangeComp.hideFavoriteChangeInfo();
            MyRoomControl.MVSTATE mState = this.<>f__this.mState;
            if (mState == MyRoomControl.MVSTATE.INIT)
            {
                this.<>f__this.standFigureBack.Fadein(this.end_act);
            }
            else if (mState == MyRoomControl.MVSTATE.ONLY_SVT)
            {
                Vector3 movePos = new Vector3(-this.<>f__this.svtFigure.GetComponentInChildren<UITexture>().transform.localPosition.x, this.<>f__this.svtObj.transform.localPosition.y, this.<>f__this.svtObj.transform.localPosition.z);
                this.<>f__this.moveControl(this.<>f__this.svtObj, movePos);
                this.end_act.Call();
            }
            else
            {
                this.end_act.Call();
            }
        }
    }

    private enum MENUTYPE
    {
        MAIN,
        MATERIAL,
        MATERIAL_COLLECTION,
        ITEM,
        PROFILE,
        OPTION,
        NOTICE,
        SERIALCODE,
        CONTINUEDEVICE,
        FAVORITE_CHANGE
    }

    public enum MVSTATE
    {
        INIT,
        ONLY_SVT,
        ONLY_BG
    }

    public enum STATE
    {
        MATERIAL,
        ETC,
        SIZEOF
    }

    private class StateEtc : IState<MyRoomControl>
    {
        public void begin(MyRoomControl that)
        {
        }

        public void end(MyRoomControl that)
        {
        }

        public void update(MyRoomControl that)
        {
        }
    }
}

