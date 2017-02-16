using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TitleRootComponent : SceneRootComponent
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache1F;
    [CompilerGenerated]
    private static System.Action <>f__am$cache21;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map34;
    public UIButton allocMemButton;
    public UILabel appVerLb;
    public UIButton assetLoadButton;
    public UILabel assetLoadLabel;
    public UILabel cpRghtLb;
    public UIButton debugButton;
    public GameObject loginPrefab;
    public LogoMain logoMain;
    public GameObject registerPrefab;
    public UILineInput registerUserIdInput;
    public UILineInput registerUserMailInput;
    public UILineInput registerUserPwInput;
    public GameObject SetOffBtn;
    public GameObject SetOnBtn;
    public GameObject startImg;
    public UIButton takeoverBtn;
    public UIButton takeoverCancel;
    public UIButton takeoverDecide;
    public TakeoverDecideMenu takeoverDecideMenu;
    public UILabel takeOverDetailLb;
    public UILineInput takeoverKeyInput;
    public GameObject takeoverObj;
    public UILineInput takeoverPasswardInput;
    public UILabel takeOverPwLb;
    public UILabel takteOverIdLb;
    public TermsOfUseMenu termsOfUseMenu;
    public GameObject titleRootObjet;
    public UILineInput userIdInput;
    public UILineInput userPwInput;
    public UILabel usrFriendCodeLb;
    public GameObject YYBLoginObj;

    protected void battleContinueRequest()
    {
        switch (BattleData.getContinueBattleFlg())
        {
            case 1:
            {
                BattleUseContinueRequest request = NetworkManager.getRequest<BattleUseContinueRequest>(new NetworkManager.ResultCallbackFunc(this.endRequestBattleContinueRequest));
                if (!request.beginRetryRequest(false))
                {
                    long battleId = BattleData.getResumeBattleId();
                    if (battleId > 0L)
                    {
                        request.beginRequest(battleId);
                    }
                    else
                    {
                        BattleData.setContinueBattleFlg(0, true);
                        base.myFSM.SendEvent("GO_TERMINAL");
                    }
                }
                break;
            }
            case 2:
            {
                BattleCommandSpellRequest request2 = NetworkManager.getRequest<BattleCommandSpellRequest>(new NetworkManager.ResultCallbackFunc(this.endRequestBattleContinueRequest));
                if (!request2.beginRetryRequest(false))
                {
                    long num3 = BattleData.getResumeBattleId();
                    if (num3 > 0L)
                    {
                        int commandSpellId = ConstantMaster.getValue("GAME_OVER_COMMAND_SPELL_ID");
                        request2.beginRequest(num3, commandSpellId, true);
                    }
                    else
                    {
                        BattleData.setContinueBattleFlg(0, true);
                        base.myFSM.SendEvent("GO_TERMINAL");
                    }
                }
                break;
            }
        }
    }

    public override void beginFinish()
    {
        this.titleRootObjet.SetActive(false);
    }

    public override void beginInitialize()
    {
        this.titleRootObjet.SetActive(false);
        this.titleRootObjet.GetComponent<UIPanel>().alpha = !LogoMain.IsPLayLogo() ? 1f : 0f;
        base.beginInitialize();
        this.debugButton.gameObject.SetActive(ManagerConfig.UseDebugCommand);
        this.allocMemButton.gameObject.SetActive(ManagerConfig.UseDebugCommand);
        this.assetLoadButton.gameObject.SetActive(ManagerConfig.UseDebugCommand);
        this.assetLoadLabel.text = !SingletonMonoBehaviour<AssetManager>.Instance.GetDebugStatusOut() ? "Asset OFF" : "Asset ON";
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        MonoBehaviour.print("android");
        this.loginPrefab.SetActive(false);
        this.startImg.SetActive(true);
    }

    public override void beginStartUp()
    {
        Time.timeScale = 1f;
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0f, null);
        base.sendMessageStartUp();
    }

    protected void callbackInputDecide(string result)
    {
        if (result != "ng")
        {
            Dictionary<string, object> dictionary = JsonManager.getDictionary(result);
            if ((dictionary.ContainsKey("userId") && dictionary.ContainsKey("authKey")) && dictionary.ContainsKey("secretKey"))
            {
                UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                if (entity != null)
                {
                    string userId = dictionary["userId"].ToString();
                    string authKey = dictionary["authKey"].ToString();
                    string secretKey = dictionary["secretKey"].ToString();
                    UserSaveData.DeleteContinueData();
                    SingletonMonoBehaviour<NetworkManager>.Instance.SetAuth(userId, authKey, secretKey);
                    SingletonMonoBehaviour<NetworkManager>.Instance.WriteAuth();
                    DateTime time = NetworkManager.getDateTime(entity.birthDay);
                    SingletonMonoBehaviour<NetworkManager>.Instance.SetSignup(entity.name, entity.genderType, time.Month, time.Day);
                    SingletonMonoBehaviour<NetworkManager>.Instance.WriteSignup();
                    UserServantNewManager.CreateContinueDeviceSaveData();
                    UserServantCollectionManager.CreateContinueDeviceSaveData();
                    ServantCommentManager.CreateContinueDeviceSaveData();
                    UserEquipNewManager.CreateContinueDeviceSaveData();
                    OtherUserNewManager.CreateContinueDeviceSaveData();
                    SingletonMonoBehaviour<NetworkManager>.Instance.SetFriendCode(entity.friendCode);
                    SingletonMonoBehaviour<NetworkManager>.Instance.WriteFriendCode();
                    string dispFriendCode = SingletonMonoBehaviour<NetworkManager>.Instance.GetDispFriendCode();
                    if (dispFriendCode != null)
                    {
                        this.usrFriendCodeLb.text = string.Format(LocalizationManager.Get("USER_FRIEND_CODE"), dispFriendCode);
                    }
                    else
                    {
                        this.usrFriendCodeLb.text = string.Empty;
                    }
                    TermsOfUseMenu.Save();
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, string.Format(LocalizationManager.Get("CONTINUE_DEVICE_TAKEOVER_DECIDE_MESSAGE"), entity.name), new NotificationDialog.ClickDelegate(this.onSelectTakeoverDecide), -1);
                    return;
                }
            }
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, LocalizationManager.Get("CONTINUE_DEVICE_TAKEOVER_FAIL_MESSAGE"), new NotificationDialog.ClickDelegate(this.onRequestTakeoverFail2), -1);
    }

    private void callbackLgoinAction(string result)
    {
        string key = result;
        if (key != null)
        {
            int num;
            if (<>f__switch$map34 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(1) {
                    { 
                        "88",
                        0
                    }
                };
                <>f__switch$map34 = dictionary;
            }
            if (<>f__switch$map34.TryGetValue(key, out num) && (num == 0))
            {
                base.myFSM.SendEvent("REQUEST_NG");
                return;
            }
        }
        SingletonMonoBehaviour<NetworkManager>.Instance.LoginResponseResult = result;
        UserServantLockManager.InitServantLockStatus();
        base.myFSM.SendEvent("REQUEST_OK");
    }

    private void callbackLogin(string result)
    {
        if (result == "signup")
        {
            base.myFSM.SendEvent("REQUEST_DO_SIGNUP");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    public void callbackTopGameData(string res)
    {
        if (res == "ok")
        {
            SingletonMonoBehaviour<ManagementManager>.Instance.callbackTopGameData(string.Empty);
            this.loginPrefab.SetActive(false);
            this.registerPrefab.SetActive(false);
            this.startImg.SetActive(true);
        }
    }

    private void callbackTopSignup(string result)
    {
        if (result == "ok")
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected bool CheckAndTransitionBattleAfterTalk()
    {
        int num;
        int num2;
        if (BattleData.GetBattleAfterTalkResumeInfo(out num, out num2))
        {
            BattleData.FinishBattleInfo data = new BattleData.FinishBattleInfo {
                questId = num,
                questPhase = num2,
                winLoseInfo = 1
            };
            SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.FinishBattle, SceneManager.FadeType.WHITE, data);
            return true;
        }
        return false;
    }

    public void checkConcentTermsOfUse()
    {
        if (DataManager.isLogin)
        {
            if (BalanceConfig.IsIOS_Examination)
            {
                base.myFSM.SendEvent("CONCENT_OK");
            }
            else if (this.termsOfUseMenu.IsConcent())
            {
                base.myFSM.SendEvent("CONCENT_NG");
            }
            else
            {
                base.myFSM.SendEvent("CONCENT_OK");
            }
        }
        else
        {
            base.myFSM.SendEvent("CONCENT_RE");
            BSGameSdk.login();
        }
    }

    public void checkPlayMode()
    {
        if (LogoMain.IsPLayLogo())
        {
            base.myFSM.SendEvent("PLAY_LOGO_MODE");
        }
        else
        {
            base.myFSM.SendEvent("PLAY_LOGO_MODE");
        }
    }

    public bool checkSignup()
    {
        if (ManagerConfig.UseMock)
        {
            base.myFSM.SendEvent("SIGNUP_ENTRY_OK");
            return true;
        }
        if (SingletonMonoBehaviour<NetworkManager>.Instance.ReadSignup())
        {
            base.myFSM.SendEvent("SIGNUP_ENTRY_OK");
            return true;
        }
        base.myFSM.SendEvent("SIGNUP_ENTRY_NONE");
        return false;
    }

    private bool CheckUserName(string name, string pw, string type, string mail, ref string msg)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
        {
            msg = "用户名或密码不能为空";
            return true;
        }
        if ((type == "register") && string.IsNullOrEmpty(mail))
        {
            msg = "QQ号不能为空";
            return true;
        }
        return false;
    }

    public bool CheckValidBattleSaveData()
    {
        long battleId = BattleData.getResumeBattleId();
        if (battleId > 0L)
        {
            BattleResumeRequest request = NetworkManager.getRequest<BattleResumeRequest>(new NetworkManager.ResultCallbackFunc(this.endRequestBattleResumeRequest));
            int questId = BattleData.getSavedQuestId();
            int questPhase = BattleData.getSavedQuestPhase();
            request.beginRequest(battleId, questId, questPhase);
        }
        else if (!this.CheckAndTransitionBattleAfterTalk())
        {
            if (BattleData.isReserveResumeBattle())
            {
                int num4 = BattleData.getSavedQuestId();
                int num5 = BattleData.getSavedQuestPhase();
                NetworkManager.getRequest<BattleResumeRequest>(new NetworkManager.ResultCallbackFunc(this.endRequestBattleResumeRequest)).beginRequest(0L, num4, num5);
            }
            else
            {
                BattleData.setContinueBattleFlg(0, true);
                base.myFSM.SendEvent("GO_TERMINAL");
            }
        }
        return true;
    }

    protected bool closeTakeoverInput()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.takeoverKeyInput.SetInputEnable(false);
            this.takeoverPasswardInput.SetInputEnable(false);
            this.takeoverObj.SetActive(false);
            if (<>f__am$cache21 == null)
            {
                <>f__am$cache21 = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache21);
        });
        return true;
    }

    public bool ConfirmBattleContinueResume()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("BATTLECONTINUERESUME_CONFIRM_TITLE"), LocalizationManager.Get("BATTLECONTINUERESUME_CONFIRM_MESSAGE"), LocalizationManager.Get("BATTLECONTINUERESUME_CONFIRM_DECIDE"), LocalizationManager.Get("BATTLECONTINUERESUME_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.OnConfirmBattleContinueResume));
        return true;
    }

    public bool ConfirmBattleResume()
    {
        if (this.IsTutorialBattleNow())
        {
            this.OnConfirmBattleResume(true);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("BATTLERESUME_CONFIRM_TITLE"), LocalizationManager.Get("BATTLERESUME_CONFIRM_MESSAGE"), LocalizationManager.Get("BATTLERESUME_CONFIRM_DECIDE"), LocalizationManager.Get("BATTLERESUME_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.OnConfirmBattleResume));
        }
        return true;
    }

    protected void EndCloseSelectTutorialCancelConfirmCancel()
    {
        base.myFSM.SendEvent("CLICK_CANCEL");
    }

    protected void EndCloseSelectTutorialCancelConfirmDecide()
    {
        base.myFSM.SendEvent("CLICK_DECIDE");
    }

    protected void EndDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
    }

    public void endLogo()
    {
        this.logoMain.Quit();
    }

    public void endRequestBattleContinueRequest(string result)
    {
        if (result == "ok")
        {
            BattleData.setContinueBattleFlg(0, true);
            base.myFSM.SendEvent("GO_BATTLE");
        }
        else
        {
            this.ConfirmBattleContinueResume();
        }
    }

    protected void endRequestBattleResumeRequest(string result)
    {
        Debug.LogWarning("endRequestBattleResumeRequest:" + result);
        if (result == "ng")
        {
            BattleData.setContinueBattleFlg(0, true);
            if (!this.CheckAndTransitionBattleAfterTalk())
            {
                base.myFSM.SendEvent("GO_TERMINAL");
            }
        }
        else if (BattleData.getContinueBattleFlg() > 0)
        {
            this.battleContinueRequest();
        }
        else
        {
            base.myFSM.SendEvent("GO_BATTLE");
        }
    }

    protected void endRequestTutorialCancelRequest(string result)
    {
        if (result == "ok")
        {
            TutorialFlag.CompleteProgress();
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void endTermsOfUseInput(bool result)
    {
        if (result)
        {
            this.termsOfUseMenu.Close(new System.Action(this.endTermsOfUseOk));
        }
        else
        {
            this.termsOfUseMenu.Close();
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("TERMS_OF_USE_DENIAL_TITLE"), LocalizationManager.Get("TERMS_OF_USE_DENIAL_MESSAGE"), new System.Action(this.endTermsOfUseNg), -1);
        }
    }

    protected void endTermsOfUseNg()
    {
        base.myFSM.SendEvent("CONCENT_NG");
    }

    protected void endTermsOfUseOk()
    {
        base.myFSM.SendEvent("CONCENT_OK");
    }

    protected void endTitleFade()
    {
        base.myFSM.SendEvent("END_FADE");
    }

    public void IsActive()
    {
        if (UIToggle.current.value)
        {
            SingletonMonoBehaviour<DataManager>.Instance.serverId = int.Parse(UIToggle.current.name);
            Debug.LogError(SingletonMonoBehaviour<DataManager>.Instance.serverId);
        }
    }

    protected bool IsTutorialBattleNow()
    {
        long userId = NetworkManager.UserId;
        long[] args = new long[] { userId, (long) ConstantMaster.getValue("TUTORIAL_QUEST_ID1") };
        UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(args);
        if ((entity != null) && (entity.getClearNum() == 0))
        {
            return true;
        }
        long[] numArray2 = new long[] { userId, (long) ConstantMaster.getValue("TUTORIAL_QUEST_ID2") };
        entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(numArray2);
        if ((entity != null) && (entity.getClearNum() == 0))
        {
            return true;
        }
        long[] numArray3 = new long[] { userId, (long) ConstantMaster.getValue("TUTORIAL_QUEST_ID3") };
        entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(numArray3);
        return (((entity != null) && (entity.getClearNum() == 0)) || !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END));
    }

    public void onChangeTakeoverInput()
    {
        string text = this.takeoverKeyInput.GetText();
        string str2 = this.takeoverPasswardInput.GetText();
        bool flag = (text.Length >= 10) && (str2.Length >= 4);
        this.takeoverDecide.enabled = flag;
        this.takeoverDecide.isEnabled = flag;
        this.takeoverDecide.defaultColor = !flag ? Color.gray : Color.white;
    }

    public void OnClickLoginBtn()
    {
        string text = this.userIdInput.GetText();
        string pw = this.userPwInput.GetText();
        this.StartRegisterOrLogin(text, pw, "login", string.Empty);
    }

    public void OnClickRegisterBtn()
    {
        string text = this.registerUserIdInput.GetText();
        string pw = this.registerUserPwInput.GetText();
        string mail = this.registerUserMailInput.GetText();
        this.StartRegisterOrLogin(text, pw, "register", mail);
    }

    public void onClickRegisterHideBtn()
    {
        this.registerPrefab.SetActive(false);
        this.loginPrefab.SetActive(true);
    }

    public void onClickRegisterShowBtn()
    {
        this.registerPrefab.SetActive(true);
        this.loginPrefab.SetActive(false);
    }

    public void onClickSetOffBtn()
    {
        this.SetOnBtn.SetActive(true);
        this.SetOffBtn.SetActive(false);
    }

    public void onClickSetOnBtn()
    {
        this.SetOnBtn.SetActive(false);
        this.SetOffBtn.SetActive(true);
    }

    public void OnClickSwitchingAllocMem()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.switchingAllocMem();
    }

    public void OnClickSwitchingAssetLoad()
    {
        SingletonMonoBehaviour<AssetManager>.Instance.SwitchingDebugStatusOut();
        this.assetLoadLabel.text = !SingletonMonoBehaviour<AssetManager>.Instance.GetDebugStatusOut() ? "Asset OFF" : "Asset ON";
    }

    public void onClickTakeoverCancel()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.closeTakeoverInput();
        Input.imeCompositionMode = IMECompositionMode.Auto;
        base.myFSM.SendEvent("TAKEOVER_CANCEL");
    }

    public void onClickTakeoverInput()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        Input.imeCompositionMode = IMECompositionMode.Auto;
        base.myFSM.SendEvent("TAKEOVER_INPUT_OK");
    }

    protected void onCloseRequestTakeoverFail2()
    {
        base.myFSM.SendEvent("REQUEST_NG");
    }

    protected void onCloseTakeoverDecide()
    {
        this.closeTakeoverInput();
        UIInput component = this.takeoverKeyInput.GetComponent<UIInput>();
        UIInput input2 = this.takeoverPasswardInput.GetComponent<UIInput>();
        component.value = string.Empty;
        input2.value = string.Empty;
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected void OnConfirmBattleContinueResume(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        if (isDecide)
        {
            BattleData.deleteSaveData();
            base.myFSM.SendEvent("GO_TERMINAL");
        }
        else
        {
            this.battleContinueRequest();
        }
    }

    protected void OnConfirmBattleResume(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_TIP);
            base.myFSM.SendEvent("CLICK_DECIDE");
        }
        else
        {
            BattleData.deleteSaveData();
            base.myFSM.SendEvent("CLICK_CANCEL");
        }
    }

    private void onEndWebView()
    {
        base.myFSM.SendEvent("CLOSE_NOTICE");
    }

    protected void onRequestTakeoverFail2(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog(new System.Action(this.onCloseRequestTakeoverFail2));
    }

    protected void onSelectTakeoverDecide(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog(new System.Action(this.onCloseTakeoverDecide));
    }

    protected void OnSelectTutorialCancelConfirm(bool isDecide)
    {
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.EndCloseSelectTutorialCancelConfirmDecide));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.EndCloseSelectTutorialCancelConfirmCancel));
        }
    }

    public void openBannerWebVew()
    {
        this.onEndWebView();
    }

    protected bool openTakeoverInput()
    {
        this.takeoverObj.SetActive(true);
        this.takeoverKeyInput.SetInputEnable(true);
        this.takeoverPasswardInput.SetInputEnable(true);
        UIInput component = this.takeoverKeyInput.GetComponent<UIInput>();
        UIInput input2 = this.takeoverPasswardInput.GetComponent<UIInput>();
        input2.value = string.Empty;
        component.defaultText = LocalizationManager.Get("CONTINUE_DEVICE_TAKEOVER_EXPLANATIOIN1");
        input2.defaultText = LocalizationManager.Get("CONTINUE_DEVICE_TAKEOVER_EXPLANATIOIN2");
        this.onChangeTakeoverInput();
        if (<>f__am$cache1F == null)
        {
            <>f__am$cache1F = delegate {
            };
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache1F);
        return true;
    }

    public void QQLoginBtnClick()
    {
        BSGameSdk.YYBLogin(1);
        this.SetYYBLoginStatus(false);
    }

    public void RepeatStartTitle()
    {
        MonoBehaviour.print("android");
        DataManager.isLogin = false;
        this.loginPrefab.SetActive(false);
        this.startImg.SetActive(true);
        this.startTitle();
    }

    public void requestLogin()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.RequestLogin(new NetworkManager.LoginCallbackFunc(this.callbackLogin), true);
        Debug.Log("!!** requestLogin");
    }

    public void requestLoginAction()
    {
        NetworkManager.getRequest<TopLoginActionRequest>(new NetworkManager.ResultCallbackFunc(this.callbackLgoinAction)).beginRequest("toplogin");
    }

    public void requestSignup()
    {
        TopSignupRequest request = NetworkManager.getRequest<TopSignupRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopSignup));
        Debug.Log("!!** requestSignup");
        request.beginRequest("signuptop");
    }

    public void requestTakeoverDecide()
    {
        ContinueDecideRequest request = NetworkManager.getRequest<ContinueDecideRequest>(new NetworkManager.ResultCallbackFunc(this.callbackInputDecide));
        string text = this.takeoverKeyInput.GetText();
        string continuePass = this.takeoverPasswardInput.GetText();
        request.beginRequestCode(text, continuePass);
    }

    public bool requestTutorialCancel()
    {
        TutorialClearRequest request = NetworkManager.getRequest<TutorialClearRequest>(new NetworkManager.ResultCallbackFunc(this.endRequestTutorialCancelRequest));
        request.addActionField("tutorialclear");
        request.beginRequest();
        return true;
    }

    private void RestoreUserName()
    {
        string str = PlayerPrefs.GetString("userName");
        string str2 = PlayerPrefs.GetString("userPw");
        if (!string.IsNullOrEmpty(str))
        {
            this.userIdInput.GetComponent<UIInput>().value = str;
            this.userPwInput.GetComponent<UIInput>().value = str2;
        }
    }

    public void SetBoxCLient(bool isEnabled)
    {
        this.titleRootObjet.gameObject.GetComponent<BoxCollider>().enabled = isEnabled;
    }

    public void SetYYBLoginStatus(bool isShow)
    {
        this.YYBLoginObj.SetActive(isShow);
    }

    public bool startDownLoad() => 
        true;

    public void startLogo()
    {
        this.logoMain.Init(base.myFSM);
    }

    public void StartRegisterOrLogin(string name, string pw, string type, string mail)
    {
        string msg = string.Empty;
        if (this.CheckUserName(name, pw, type, mail, ref msg))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailInfoDialog(string.Empty, string.Empty, msg);
        }
        else
        {
            this.StorageUserMess(name, pw);
            LoginToMemberCenterRequest request = NetworkManager.getRequest<LoginToMemberCenterRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopGameData));
            request.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
            request.addField("t", "22360");
            request.addField("v", "1.0.1");
            request.addField("s", "1");
            request.addField("mac", "00000000000000E0");
            request.addField("os", SystemInfo.operatingSystem);
            request.addField("ptype", SystemInfo.deviceModel);
            request.addField("imei", "aaaaa");
            request.addField("username", name);
            request.addField("type", type);
            request.addField("qq", mail);
            request.addField("password", pw);
            request.addField("rksdkid", "1");
            request.addField("rkchannel", "24");
            request.beginRequest();
        }
    }

    public bool startSignupInput()
    {
        base.myFSM.SendEvent("SIGNUP_INPUT_OK");
        return true;
    }

    public bool startTakeoverInput()
    {
        this.takeOverDetailLb.text = LocalizationManager.Get("TAKEOVER_DETAIL_TXT");
        this.takteOverIdLb.text = LocalizationManager.Get("TAKEOVER_ID_TXT");
        this.takeOverPwLb.text = LocalizationManager.Get("TAKEOVER_PW_TXT");
        Input.imeCompositionMode = IMECompositionMode.On;
        if (this.takeoverObj.activeSelf)
        {
            this.openTakeoverInput();
            return true;
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.openTakeoverInput();
        });
        return true;
    }

    public bool startTermsOfUseInput()
    {
        this.termsOfUseMenu.Open(new TermsOfUseMenu.CallbackFunc(this.endTermsOfUseInput));
        return true;
    }

    public void startTitle()
    {
        this.SetYYBLoginStatus(false);
        ManagementManager.CompletionStartup();
        this.titleRootObjet.SetActive(true);
        MonoBehaviour.print("android1");
        BSGameSdk.init(true, "1", "112", "248", "a4e39619a09d49e9aead9b820980013a");
        if (ManagerConfig.UseMock)
        {
            this.takeoverBtn.isEnabled = false;
        }
        this.appVerLb.text = string.Format(LocalizationManager.Get("APP_VERSION"), ManagerConfig.AppVer);
        this.cpRghtLb.text = LocalizationManager.Get("TITLE_COPYRIGHT");
        this.usrFriendCodeLb.text = string.Empty;
        if (SingletonMonoBehaviour<NetworkManager>.Instance.ReadFriendCode())
        {
            string dispFriendCode = SingletonMonoBehaviour<NetworkManager>.Instance.GetDispFriendCode();
            if (dispFriendCode != null)
            {
                this.usrFriendCodeLb.text = string.Format(LocalizationManager.Get("USER_FRIEND_CODE"), dispFriendCode);
            }
        }
        UIPanel component = this.titleRootObjet.GetComponent<UIPanel>();
        if (component.alpha < 1f)
        {
            component.alpha = 1f;
            base.Invoke("startTitleFade", 0.1f);
        }
        else
        {
            this.endTitleFade();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        this.RestoreUserName();
    }

    protected void startTitleFade()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(1.5f, new System.Action(this.endTitleFade));
    }

    public bool startTutorialCancelConfirm()
    {
        if (ManagerConfig.UseDebugCommand)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("TUTORIAL_CANCEL_CONFIRM_TITLE"), LocalizationManager.Get("TUTORIAL_CANCEL_CONFIRM_MESSAGE"), new CommonConfirmDialog.ClickDelegate(this.OnSelectTutorialCancelConfirm));
        }
        else
        {
            base.myFSM.SendEvent("CLICK_CANCEL");
        }
        return true;
    }

    private void StorageUserMess(string name, string pw)
    {
        PlayerPrefs.SetString("userName", name);
        PlayerPrefs.SetString("userPw", pw);
    }

    public void WXLoginBtnClick()
    {
        BSGameSdk.YYBLogin(2);
        this.SetYYBLoginStatus(false);
    }
}

