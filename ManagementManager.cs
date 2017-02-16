using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ManagementManager : SingletonMonoBehaviour<ManagementManager>
{
    protected bool _IsInitialized;
    [CompilerGenerated]
    private static ErrorDialog.ClickDelegate <>f__am$cache1B;
    public UILabel bootModeLabel;
    public UILabel buildVersionLabel;
    public GameObject debugInfoRootObject;
    protected static bool isDuringStartup = true;
    public bool isErrorDialog;
    protected bool isGetCDNMarkOver;
    public static bool IsNetorking = true;
    protected bool isPlayLogo;
    protected bool IsQuitDialogOpen;
    protected bool IsQuitFlag;
    protected bool isReadAuth;
    protected bool isReadGameData;
    protected CDN mCdn;
    public static int mCdnIndex;
    public static int mEndCdnIndex;
    public static int mEndNJsonSerIndex = -1;
    public static int mEndNSerIndex;
    protected string[] mNetworkJsonServer = new string[] { "https://line1.s1.bili.fate.biligame.net", "https://line2.s1.bili.fate.biligame.net", "https://line4.s1.bili.fate.biligame.net" };
    protected string[] mNetworkServer;
    public static int mNJsonSerIndex;
    public static int mNSerIndex;
    protected static readonly string PushStateSendedKey = "IsNotificationStatusSend";
    public SceneManager scenemanager;
    public SceneList.Type scenetype;
    protected string tResultURL;
    public static string tryNewJsonLineErrorCode = string.Empty;

    protected void Awake()
    {
        if (!base.CheckInstance())
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    protected void callbackAccountRegist(string result)
    {
        this.isReadAuth = true;
    }

    protected void callbackPlayLogo()
    {
        this.isPlayLogo = true;
    }

    protected void callbackTerminalTopHome(string result)
    {
        isDuringStartup = false;
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
    }

    protected void callbackTerminalTopLogin(string result)
    {
        if (!ManagerConfig.UseMock && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            this.callbackTerminalTopHome(null);
        }
        else
        {
            NetworkManager.getRequest<TopHomeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTerminalTopHome)).beginRequest();
        }
    }

    public void callbackTopGameData(string result)
    {
        this.isReadGameData = true;
    }

    protected void callbackTopHome(string result)
    {
        isDuringStartup = false;
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.scenetype, SceneManager.FadeType.NONE, null);
    }

    protected void callbackTopLogin(string result)
    {
        if (!ManagerConfig.UseMock && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            this.callbackTopHome(null);
        }
        else
        {
            NetworkManager.getRequest<TopHomeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopHome)).beginRequest();
        }
    }

    public string CheckErrorCode(int index, string nowError, string preError)
    {
        int num = index * 2;
        int num2 = 0;
        char[] chArray = preError.ToCharArray();
        if (((chArray == null) || (chArray.Length == 0)) || ((chArray.Length < num) || (chArray.Length < (num + 2))))
        {
            return string.Empty;
        }
        for (int i = num; i < (num + 2); i++)
        {
            chArray[i] = nowError[num2];
            num2++;
        }
        string str = string.Empty;
        for (int j = 0; j < chArray.Length; j++)
        {
            str = str + chArray[j];
        }
        return str;
    }

    public static void CompletionStartup()
    {
        isDuringStartup = false;
    }

    protected void EndDifferentCreateUserServerDialog(bool isDecide)
    {
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.DebugTest, SceneManager.FadeType.BLACK, null);
    }

    protected void EndLogDialog(bool isDecide)
    {
        this.isErrorDialog = false;
    }

    protected void EndQuitDialog(bool isDecide)
    {
        Application.Quit();
    }

    protected void EndRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            this.isErrorDialog = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public CDN GetmCdn() => 
        this.mCdn;

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            string str = DateTime.Now.ToString();
            string str2 = "unknow error ";
            char[] separator = new char[] { '\n' };
            string[] strArray = stackTrace.Split(separator);
            if ((strArray != null) && (strArray.Length > 0))
            {
                str2 = string.Empty;
                for (int i = 0; i < strArray.Length; i++)
                {
                    string str3 = strArray[i];
                    if (str3.Contains("Game/Scripts"))
                    {
                        char[] chArray2 = new char[] { '/' };
                        string[] strArray2 = str3.Split(chArray2);
                        str3 = strArray2[strArray2.Length - 1];
                        str2 = str2 + " # " + str3;
                        break;
                    }
                }
            }
            else
            {
                str2 = stackTrace;
            }
            if (<>f__am$cache1B == null)
            {
                <>f__am$cache1B = isDecide => Application.Quit();
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog("发生错误", "网络连接异常，请确认在网络情况良好的环境下进行游戏。", <>f__am$cache1B, false);
        }
    }

    protected bool IsInitialized() => 
        !IsDuringStartup;

    protected bool IsLoading() => 
        ((NetworkManager.CommunicationIsBusy() || AssetManager.LoadIsBusy()) || SingletonMonoBehaviour<WebViewManager>.Instance.IsBusy);

    public void reboot(bool isLogin = false)
    {
        isDuringStartup = true;
        SingletonMonoBehaviour<ScriptManager>.Instance.reboot();
        SingletonMonoBehaviour<SceneManager>.Instance.reboot();
        SingletonTemplate<MissionNotifyManager>.Instance.Reboot();
        SingletonMonoBehaviour<CommonUI>.Instance.Reboot();
        AtlasManager.Reboot();
        FlashingIconManager.Reboot();
        SoundManager.reboot();
        GC.Collect();
        base.StartCoroutine(this.startCheckAll(true, isLogin));
    }

    protected void requestLogin()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.RequestLogin(new NetworkManager.LoginCallbackFunc(this.callbackTopLogin), true);
    }

    protected void requestTerminalLogin()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.RequestLogin(new NetworkManager.LoginCallbackFunc(this.callbackTerminalTopLogin), true);
    }

    private void Start()
    {
        Application.targetFrameRate = 0x18;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
        UnityEngine.Object.DontDestroyOnLoad(this);
        base.StartCoroutine(this.startCheckAll(false, false));
    }

    [DebuggerHidden]
    private IEnumerator startCheckAll(bool isReboot, bool isLogin) => 
        new <startCheckAll>c__IteratorC { 
            isReboot = isReboot,
            isLogin = isLogin,
            <$>isReboot = isReboot,
            <$>isLogin = isLogin,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator TryNewJsonUrl(int index, string errorCode) => 
        new <TryNewJsonUrl>c__IteratorB { 
            errorCode = errorCode,
            index = index,
            <$>errorCode = errorCode,
            <$>index = index,
            <>f__this = this
        };

    public void TryNewNetwork(string errorCode)
    {
        mNSerIndex++;
        if (errorCode != "00")
        {
            string nowError = errorCode;
            NetworkManager.tryNewNetworkLineErrorCode = this.CheckErrorCode(mNSerIndex - 1, nowError, NetworkManager.tryNewNetworkLineErrorCode);
        }
        if (mNSerIndex >= this.mNetworkServer.Length)
        {
            mNSerIndex = 0;
        }
        if (mNSerIndex == mEndNSerIndex)
        {
            this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE") + "(" + NetworkManager.tryNewNetworkLineErrorCode + ")", new ErrorDialog.ClickDelegate(this.EndRetryDialog), false);
        }
        this.tResultURL = this.mNetworkServer[mNSerIndex];
        NetworkManager.gameServerAddress = this.tResultURL + "/rongame_beta/";
        ManagerConfig.DevelopGameServerAddress = this.tResultURL + "/rongame_beta/";
    }

    public void TryNewNetworkForNoAuto(string errorCode)
    {
        mNSerIndex++;
        if (errorCode != "00")
        {
            string nowError = errorCode;
            NetworkManager.tryNewNetworkLineErrorCode = this.CheckErrorCode(mNSerIndex - 1, nowError, NetworkManager.tryNewNetworkLineErrorCode);
        }
        if (mNSerIndex >= this.mNetworkServer.Length)
        {
            mNSerIndex = 0;
        }
        this.tResultURL = this.mNetworkServer[mNSerIndex];
        NetworkManager.gameServerAddress = this.tResultURL + "/rongame_beta/";
        ManagerConfig.DevelopGameServerAddress = this.tResultURL + "/rongame_beta/";
    }

    public void TryNewTextCDNUrl(string errorCode)
    {
        mCdnIndex++;
        if (errorCode != "00")
        {
            string nowError = errorCode;
            AssetLoader.tryNewLineErrorCode = this.CheckErrorCode(mCdnIndex - 1, nowError, AssetLoader.tryNewLineErrorCode);
        }
        if (mCdnIndex >= this.mCdn.list[0].cdn.Length)
        {
            mCdnIndex = 0;
        }
        if (mCdnIndex == mEndCdnIndex)
        {
            this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, "下载资源文件失败(" + AssetLoader.tryNewLineErrorCode + ")", "重试", "退出", new ErrorDialog.ClickDelegate(this.EndRetryDialog), false);
            Debug.Log("下载配置文件失败");
        }
        string str3 = this.mCdn.list[0].cdn[mCdnIndex];
        ManagerConfig.DevelopDataServerAddress = str3;
        SingletonMonoBehaviour<NetworkManager>.Instance.SetServerSetting();
    }

    private void Update()
    {
        if ((this.IsQuitFlag && this.IsInitialized()) && !this.IsLoading())
        {
            Application.Quit();
        }
        if ((((Application.platform == RuntimePlatform.Android) && Input.GetKeyDown(KeyCode.Escape)) && (!this.IsQuitDialogOpen && this.IsInitialized())) && !this.IsLoading())
        {
            this.IsQuitDialogOpen = true;
            DialogManager.Instance.SetLabel(LocalizationManager.Get("BACK_BUTTON_CONFIRM_YES"), LocalizationManager.Get("BACK_BUTTON_CONFIRM_NO"), LocalizationManager.Get("BACK_BUTTON_CONFIRM_NO"));
            DialogManager.Instance.ShowSelectDialog(LocalizationManager.Get("BACK_BUTTON_CONFIRM"), delegate (bool result) {
                if (result)
                {
                    this.IsQuitFlag = true;
                }
                else
                {
                    this.IsQuitDialogOpen = false;
                }
            });
        }
    }

    public static bool IsDuringStartup =>
        isDuringStartup;

    [CompilerGenerated]
    private sealed class <startCheckAll>c__IteratorC : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isLogin;
        internal bool <$>isReboot;
        internal ManagementManager <>f__this;
        internal int <accountingResult>__19;
        internal long <allSize>__16;
        internal long <allSize>__18;
        internal int <c>__6;
        internal bool <enableRemotePush>__22;
        internal long <freeSize>__10;
        internal long <freeSize1>__8;
        internal long <freeSize2>__9;
        internal int <i>__0;
        internal int <i>__2;
        internal int <i>__21;
        internal int <i>__3;
        internal bool <isDebugMenu>__4;
        internal bool <isLoad>__15;
        internal string[] <productList>__20;
        internal DataManager.ReadMasterDataResult <readMasterResult>__11;
        internal TopGameDataRequest <request>__12;
        internal long <size>__14;
        internal long <size>__17;
        internal float <startTime>__5;
        internal float <startTime>__7;
        internal DataManager.UpdateMasterDataResult <updateResult>__13;
        internal WWW <www>__1;
        internal bool isLogin;
        internal bool isReboot;

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
                    if (!this.isReboot)
                    {
                        break;
                    }
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_10C8;

                case 1:
                    break;

                case 2:
                case 3:
                    if (this.<>f__this.isErrorDialog)
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 3;
                    }
                    else
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                        this.<www>__1 = new WWW(ManagerConfig.DevelopCDNAddress);
                        this.$current = this.<www>__1;
                        this.$PC = 4;
                    }
                    goto Label_10C8;

                case 4:
                    if (string.IsNullOrEmpty(this.<www>__1.error))
                    {
                        goto Label_0235;
                    }
                    ManagementManager.IsNetorking = false;
                    goto Label_01DF;

                case 5:
                    goto Label_01DF;

                case 6:
                    goto Label_0235;

                case 7:
                    this.<isDebugMenu>__4 = false;
                    if (Input.touchCount <= 0)
                    {
                        this.<startTime>__7 = Time.time;
                        goto Label_051F;
                    }
                    this.<startTime>__5 = Time.time;
                    goto Label_0498;

                case 8:
                    this.<c>__6 = Input.touchCount;
                    if (this.<c>__6 < 4)
                    {
                        if (this.<c>__6 >= 1)
                        {
                            if ((Time.time - this.<startTime>__5) <= 3f)
                            {
                                goto Label_0498;
                            }
                            this.<isDebugMenu>__4 = true;
                        }
                    }
                    else
                    {
                        this.<isDebugMenu>__4 = true;
                    }
                    goto Label_05A1;

                case 9:
                    if (!Input.GetKey(KeyCode.D))
                    {
                        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) || Input.GetMouseButton(2))
                        {
                            if ((Time.time - this.<startTime>__7) <= 5f)
                            {
                                goto Label_051F;
                            }
                            this.<isDebugMenu>__4 = true;
                        }
                    }
                    else
                    {
                        this.<isDebugMenu>__4 = true;
                    }
                    goto Label_05A1;

                case 10:
                    goto Label_0739;

                case 11:
                    this.<readMasterResult>__11 = SingletonMonoBehaviour<DataManager>.Instance.getReadMasterDataResult();
                    if (this.<readMasterResult>__11 != DataManager.ReadMasterDataResult.OK)
                    {
                        DataManager.ClearCacheAll();
                    }
                    this.<request>__12 = NetworkManager.getRequest<TopGameDataRequest>(new NetworkManager.ResultCallbackFunc(this.<>f__this.callbackTopGameData));
                    this.<request>__12.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
                    this.<request>__12.addField("t", "22360");
                    this.<request>__12.addField("v", "1.0.1");
                    this.<request>__12.addField("version", SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion());
                    this.<request>__12.addField("s", "1");
                    this.<request>__12.addField("mac", "00000000000000E0");
                    this.<request>__12.addField("os", SystemInfo.operatingSystem);
                    this.<request>__12.addField("ptype", SystemInfo.deviceModel);
                    this.<request>__12.addField("imei", "aaaaa");
                    this.<request>__12.addField("username", "lv9999");
                    this.<request>__12.addField("type", "login");
                    this.<request>__12.addField("password", "111111");
                    this.<request>__12.addField("rksdkid", "1");
                    this.<request>__12.addField("rkchannel", "24");
                    this.<request>__12.beginRequest();
                    SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                    goto Label_092C;

                case 12:
                    goto Label_092C;

                case 13:
                    this.<updateResult>__13 = SingletonMonoBehaviour<DataManager>.Instance.getUpdateMasterDataResult();
                    if (this.<updateResult>__13 == DataManager.UpdateMasterDataResult.OK)
                    {
                        SingletonMonoBehaviour<NetworkManager>.Instance.SetServerSetting();
                        this.$current = 0;
                        this.$PC = 0x10;
                        goto Label_10C8;
                    }
                    DataManager.ClearCacheAll();
                    SingletonMonoBehaviour<DataManager>.Instance.Initialize();
                    if (!ManagerConfig.UseDebugCommand)
                    {
                        goto Label_0A12;
                    }
                    this.<>f__this.isErrorDialog = true;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]MasterDataUpdate error for debug", "InitializeResult (" + this.<updateResult>__13 + ")", new ErrorDialog.ClickDelegate(this.<>f__this.EndLogDialog), false);
                    goto Label_0A02;

                case 14:
                    goto Label_0A02;

                case 15:
                    goto Label_0A66;

                case 0x10:
                    if (AssetManager.getDownloadSize() <= 0L)
                    {
                        goto Label_0AE4;
                    }
                    goto Label_0AAA;

                case 0x11:
                    goto Label_0AAA;

                case 0x12:
                    goto Label_0B1C;

                case 0x13:
                    if (SingletonMonoBehaviour<CommonUI>.Instance.IsBusyLoad())
                    {
                        goto Label_0B9B;
                    }
                    goto Label_0C0B;

                case 20:
                    goto Label_0BD1;

                case 0x15:
                    goto Label_0C38;

                case 0x16:
                    goto Label_0C69;

                case 0x17:
                    if (SingletonMonoBehaviour<CommonUI>.Instance.IsBusyLoad())
                    {
                        goto Label_0CE8;
                    }
                    goto Label_0D19;

                case 0x18:
                    goto Label_0D50;

                case 0x19:
                    goto Label_0D91;

                case 0x1a:
                    goto Label_0DC8;

                case 0x1b:
                    goto Label_0E12;

                case 0x1c:
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
                    goto Label_0F49;

                case 0x1d:
                    Debug.Log("InitScene start next " + SceneList.getSceneName(this.<>f__this.scenetype));
                    if (PlayerPrefs.GetInt(ManagementManager.PushStateSendedKey, 0) == 0)
                    {
                        this.<enableRemotePush>__22 = OptionManager.GetNotiffication();
                        OptionManager.SetNotiffication(this.<enableRemotePush>__22, true);
                        PlayerPrefs.SetInt(ManagementManager.PushStateSendedKey, 1);
                        PlayerPrefs.Save();
                    }
                    this.<>f__this._IsInitialized = true;
                    if (ManagerConfig.ServerDefaultType == "SCRIPT")
                    {
                        this.<>f__this.scenetype = SceneList.Type.DebugTest;
                    }
                    if (this.isLogin)
                    {
                        this.<>f__this.requestTerminalLogin();
                    }
                    else if (this.<>f__this.scenetype == SceneList.Type.Title)
                    {
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.<>f__this.scenetype, SceneManager.FadeType.BLACK, null);
                    }
                    else if (ManagerConfig.UseMock)
                    {
                        this.<>f__this.requestLogin();
                    }
                    else if (SingletonMonoBehaviour<NetworkManager>.Instance.ReadSignup())
                    {
                        this.<>f__this.requestLogin();
                    }
                    else
                    {
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.<>f__this.scenetype, SceneManager.FadeType.NONE, null);
                    }
                    this.$PC = -1;
                    goto Label_10C6;

                default:
                    goto Label_10C6;
            }
            this.<>f__this.isReadGameData = false;
            this.<>f__this.isGetCDNMarkOver = false;
            ManagementManager.tryNewJsonLineErrorCode = string.Empty;
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
            this.<i>__0 = 0;
            while (this.<i>__0 < this.<>f__this.mNetworkJsonServer.Length)
            {
                ManagementManager.tryNewJsonLineErrorCode = ManagementManager.tryNewJsonLineErrorCode + "00";
                this.<i>__0++;
            }
            this.$current = this.<>f__this.TryNewJsonUrl(0, "00");
            this.$PC = 2;
            goto Label_10C8;
        Label_01DF:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 5;
                goto Label_10C8;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, "服务器连接失败，请检查网络连接！", new ErrorDialog.ClickDelegate(this.<>f__this.EndQuitDialog), false);
            goto Label_10C6;
        Label_0235:
            while (!this.<www>__1.isDone)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 6;
                goto Label_10C8;
            }
            this.<>f__this.mCdn = JsonManager.Deserialize<ManagementManager.CDN>(this.<www>__1.text);
            this.<>f__this.mNetworkServer = this.<>f__this.mCdn.list[0].ser;
            this.<>f__this.tResultURL = this.<>f__this.mNetworkServer[0];
            ManagerConfig.DevelopGameServerAddress = this.<>f__this.tResultURL + "/rongame_beta/";
            ManagerConfig.DevelopDataServerAddress = this.<>f__this.mCdn.list[0].cdn[0];
            AssetLoader.tryNewLineErrorCode = string.Empty;
            this.<i>__2 = 0;
            while (this.<i>__2 < this.<>f__this.mCdn.list[0].cdn.Length)
            {
                AssetLoader.tryNewLineErrorCode = AssetLoader.tryNewLineErrorCode + "00";
                this.<i>__2++;
            }
            NetworkManager.tryNewNetworkLineErrorCode = string.Empty;
            this.<i>__3 = 0;
            while (this.<i>__3 < this.<>f__this.mCdn.list[0].ser.Length)
            {
                NetworkManager.tryNewNetworkLineErrorCode = NetworkManager.tryNewNetworkLineErrorCode + "00";
                this.<i>__3++;
            }
            LocalizationManager.Initialize();
            SoundManager.initialize();
            SingletonMonoBehaviour<NetworkManager>.Instance.Initialize();
            SingletonMonoBehaviour<AssetManager>.Instance.Initialize();
            OptionManager.Initialize();
            Input.multiTouchEnabled = false;
            Application.RegisterLogCallback(new Application.LogCallback(this.<>f__this.HandleLog));
            EncryptedPlayerPrefs.keys = new string[] { "2pC0bIYM", "F39UThNh", "ioqrk4Om" };
            if ((Application.isEditor || !ManagerConfig.UseDebugCommand) || this.isLogin)
            {
                this.<>f__this.debugInfoRootObject.gameObject.SetActive(false);
                this.<>f__this.bootModeLabel.text = string.Empty;
                this.<>f__this.buildVersionLabel.text = string.Empty;
                goto Label_0644;
            }
            this.<>f__this.debugInfoRootObject.gameObject.SetActive(true);
            this.<>f__this.bootModeLabel.text = "Boot Mode " + ManagerConfig.ServerDefaultType;
            this.<>f__this.buildVersionLabel.text = "Version " + ManagerConfig.AppBuildDate;
            this.$current = new WaitForSeconds(1f);
            this.$PC = 7;
            goto Label_10C8;
        Label_0498:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 8;
            goto Label_10C8;
        Label_051F:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 9;
            goto Label_10C8;
        Label_05A1:
            if (this.<isDebugMenu>__4)
            {
                this.<>f__this.debugInfoRootObject.gameObject.SetActive(false);
                this.<>f__this.bootModeLabel.text = string.Empty;
                this.<>f__this.buildVersionLabel.text = string.Empty;
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.DebugTest, SceneManager.FadeType.BLACK, null);
                goto Label_10C6;
            }
        Label_0644:
            this.<freeSize1>__8 = CommonServicePluginScript.GetFreeSize(Application.temporaryCachePath);
            this.<freeSize2>__9 = CommonServicePluginScript.GetFreeSize(Application.persistentDataPath);
            this.<freeSize>__10 = (this.<freeSize1>__8 < this.<freeSize2>__9) ? this.<freeSize2>__9 : this.<freeSize1>__8;
            Debug.Log(string.Concat(new object[] { "Boot Cache Free Size ", this.<freeSize1>__8, " ", this.<freeSize2>__9 }));
            if ((this.<freeSize>__10 <= 0L) || (this.<freeSize>__10 >= ManagerConfig.LIMIT_FREE_SIZE))
            {
                goto Label_0758;
            }
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_DISK_FULL"), new ErrorDialog.ClickDelegate(this.<>f__this.EndRetryDialog), false);
        Label_0739:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 10;
                goto Label_10C8;
            }
            goto Label_0644;
        Label_0758:
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
            SingletonMonoBehaviour<DataManager>.Instance.Initialize();
            this.$current = this.<>f__this.StartCoroutine(SingletonMonoBehaviour<DataManager>.Instance.readMasterData());
            this.$PC = 11;
            goto Label_10C8;
        Label_092C:
            while (!this.<>f__this.isReadGameData)
            {
                this.$current = null;
                this.$PC = 12;
                goto Label_10C8;
            }
            this.$current = this.<>f__this.StartCoroutine(SingletonMonoBehaviour<DataManager>.Instance.updateMasterData());
            this.$PC = 13;
            goto Label_10C8;
        Label_0A02:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 14;
                goto Label_10C8;
            }
        Label_0A12:
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE"), new ErrorDialog.ClickDelegate(this.<>f__this.EndRetryDialog), false);
        Label_0A66:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 15;
                goto Label_10C8;
            }
            goto Label_0758;
        Label_0AAA:
            this.<size>__14 = AssetManager.getDownloadSize();
            if (this.<size>__14 > 0L)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x11;
                goto Label_10C8;
            }
        Label_0AE4:
            this.<isLoad>__15 = false;
            if (AssetManager.IsOnline)
            {
                goto Label_0C38;
            }
            SingletonMonoBehaviour<AssetManager>.Instance.InitializeAssetStorage();
        Label_0B1C:
            while (!SingletonMonoBehaviour<AssetManager>.Instance.IsInitializeAssetStorage())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x12;
                goto Label_10C8;
            }
            if (ManagerConfig.ServerDefaultType == "SCRIPT")
            {
                SingletonMonoBehaviour<AssetManager>.Instance.DownloadAssetStorageAll();
            }
            else
            {
                SingletonMonoBehaviour<AssetManager>.Instance.DownloadAssetStorageAttribute("SYSTEM");
            }
            this.<allSize>__16 = AssetManager.getDownloadSize();
            if (this.<allSize>__16 <= 0L)
            {
                goto Label_0C0B;
            }
            if (!ManagerConfig.UseStandaloneAsset)
            {
                goto Label_0BD1;
            }
            this.<isLoad>__15 = true;
            SoundManager.playBgm("NOW_LOADING");
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_BAR_BOOT);
        Label_0B9B:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 0x13;
            goto Label_10C8;
        Label_0BD1:
            this.<size>__17 = AssetManager.getDownloadSize();
            if (this.<size>__17 > 0L)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 20;
                goto Label_10C8;
            }
        Label_0C0B:
            AssetManager.SetOnlineStatus();
            SingletonMonoBehaviour<NetworkManager>.Instance.SetServerSetting();
            this.$current = new WaitForSeconds(0.1f);
            this.$PC = 0x15;
            goto Label_10C8;
        Label_0C38:
            if (!AssetManager.IsOnline)
            {
                goto Label_0D19;
            }
            SingletonMonoBehaviour<AssetManager>.Instance.InitializeAssetStorage();
        Label_0C69:
            while (!SingletonMonoBehaviour<AssetManager>.Instance.IsInitializeAssetStorage())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x16;
                goto Label_10C8;
            }
            if (ManagerConfig.ServerDefaultType == "SCRIPT")
            {
                Debug.Log("huangchen test ");
                SingletonMonoBehaviour<AssetManager>.Instance.DownloadAssetStorageAll();
            }
            else
            {
                SingletonMonoBehaviour<AssetManager>.Instance.DownloadAssetStorageAttribute("SYSTEM");
            }
            this.<allSize>__18 = AssetManager.getDownloadSize();
            if (this.<allSize>__18 <= 0L)
            {
                goto Label_0D19;
            }
            this.<isLoad>__15 = true;
            SoundManager.playBgm("NOW_LOADING");
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_BAR_BOOT);
        Label_0CE8:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 0x17;
            goto Label_10C8;
        Label_0D19:
            if (this.<isLoad>__15)
            {
                SoundManager.fadeoutBgm(1f);
            }
            LocalizationManager.LoadAssetData();
        Label_0D50:
            while (LocalizationManager.IsBusySetAssetData())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x18;
                goto Label_10C8;
            }
            ImageLimitCount.initializeAssetStorage();
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
            AtlasManager.Initialize();
            FlashingIconManager.Initialize();
        Label_0D91:
            while (AtlasManager.IsBusyInitialize())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x19;
                goto Label_10C8;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            SoundManager.initializeAssetStorage();
        Label_0DC8:
            while (SingletonMonoBehaviour<SoundManager>.Instance.IsBusy)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x1a;
                goto Label_10C8;
            }
            SingletonMonoBehaviour<ScriptManager>.Instance.Initialize();
            Debug.Log("AccountingManager:Initialize start");
            SingletonMonoBehaviour<AccountingManager>.Instance.Initialize();
        Label_0E12:
            while (SingletonMonoBehaviour<AccountingManager>.Instance.IsBusyInitialize())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 0x1b;
                goto Label_10C8;
            }
            this.<accountingResult>__19 = SingletonMonoBehaviour<AccountingManager>.Instance.GetInitializeResult();
            Debug.Log("AccountingManager:Initialize end result [" + this.<accountingResult>__19 + "]");
            if (ManagerConfig.UseDebugCommand && (this.<accountingResult>__19 != 0))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Accounting error for debug", "InitializeResult (" + this.<accountingResult>__19 + ")", null, false);
            }
            this.<productList>__20 = SingletonMonoBehaviour<AccountingManager>.Instance.GetProductList();
            if (this.<productList>__20 != null)
            {
                this.<i>__21 = 0;
                while (this.<i>__21 < this.<productList>__20.Length)
                {
                    Debug.Log(string.Concat(new object[] { "    productList[", this.<i>__21, "] ", this.<productList>__20[this.<i>__21] }));
                    this.<i>__21++;
                }
            }
            if (this.<isLoad>__15)
            {
                this.$current = new WaitForSeconds(1f);
                this.$PC = 0x1c;
                goto Label_10C8;
            }
        Label_0F49:
            this.<>f__this.debugInfoRootObject.gameObject.SetActive(false);
            this.<>f__this.bootModeLabel.text = string.Empty;
            this.<>f__this.buildVersionLabel.text = string.Empty;
            this.$current = 0;
            this.$PC = 0x1d;
            goto Label_10C8;
        Label_10C6:
            return false;
        Label_10C8:
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
    private sealed class <TryNewJsonUrl>c__IteratorB : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>errorCode;
        internal int <$>index;
        internal ManagementManager <>f__this;
        internal string <error>__1;
        internal bool <isTryAgain>__0;
        internal WWW <www>__2;
        internal string errorCode;
        internal int index;

        internal void <>m__47(bool isDecide)
        {
            if (isDecide)
            {
                this.<isTryAgain>__0 = false;
            }
            else
            {
                Application.Quit();
            }
        }

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
                    this.<isTryAgain>__0 = false;
                    if (ManagementManager.mNJsonSerIndex == this.<>f__this.mNetworkJsonServer.Length)
                    {
                        ManagementManager.mNJsonSerIndex = 0;
                    }
                    if (this.errorCode != "00")
                    {
                        this.<error>__1 = this.errorCode;
                        ManagementManager.tryNewJsonLineErrorCode = this.<>f__this.CheckErrorCode(this.index, this.<error>__1, ManagementManager.tryNewJsonLineErrorCode);
                    }
                    if (ManagementManager.mEndNJsonSerIndex == ManagementManager.mNJsonSerIndex)
                    {
                        this.<isTryAgain>__0 = true;
                        this.<>f__this.isErrorDialog = true;
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, "与服务器连接中断,\n请检查您的网络连接是否异常。(" + ManagementManager.tryNewJsonLineErrorCode + ")", "重试", "退出", new ErrorDialog.ClickDelegate(this.<>m__47), false);
                    }
                    break;

                case 1:
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(this.<www>__2.error) || string.IsNullOrEmpty(this.<www>__2.text))
                    {
                        ManagementManager.mNJsonSerIndex++;
                        this.$current = this.<>f__this.TryNewJsonUrl(ManagementManager.mNJsonSerIndex - 1, "01");
                        this.$PC = 3;
                        goto Label_0223;
                    }
                    ManagementManager.mEndNJsonSerIndex = ManagementManager.mNJsonSerIndex;
                    ManagerConfig.DevelopCDNAddress = this.<www>__2.url;
                    this.<>f__this.isErrorDialog = false;
                    ManagementManager.mEndNJsonSerIndex = -1;
                    goto Label_0221;

                case 3:
                    this.$PC = -1;
                    goto Label_0221;

                default:
                    goto Label_0221;
            }
            while (this.<isTryAgain>__0)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                goto Label_0223;
            }
            if (ManagementManager.mEndNJsonSerIndex == -1)
            {
                ManagementManager.mEndNJsonSerIndex = 0;
            }
            this.<www>__2 = new WWW(AssetManager.getUrlStringWithUnix(this.<>f__this.mNetworkJsonServer[ManagementManager.mNJsonSerIndex] + "/rongame_beta/rgfate/60_member/network/network_config_android_" + ManagerConfig.AppVer + ".json"));
            Debug.LogError("connect ::: " + this.<www>__2.url);
            this.$current = this.<www>__2;
            this.$PC = 2;
            goto Label_0223;
        Label_0221:
            return false;
        Label_0223:
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

    public class CDN
    {
        public ManagementManager.CDNData[] list;
    }

    public class CDNData
    {
        public string[] cdn;
        public string notice;
        public string[] ser;
    }
}

