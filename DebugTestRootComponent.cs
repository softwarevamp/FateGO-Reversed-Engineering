using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class DebugTestRootComponent : SceneRootComponent
{
    public GameObject assetbundleInfoObject;
    public BackViewListViewMenu backSelectMenu;
    public ScriptConnectMenu connectScriptMenu;
    public GameObject debugBase;
    public DebugSignupMenu debugSignupMenu;
    public FigureViewListViewMenu figureSelectMenu;
    public UILabel folderInfoLabel;
    public GameObject NoblePhantasmSelectPanel;
    public ScriptDefaultFilePlayerMenu playScriptDefaultFileMenu;
    public ScriptFilePlayerMenu playScriptFileMenu;
    public ReceiptListViewMenu receiptSelectMenu;
    public GameObject returnButtonObject;
    public GameObject scriptPlayerBase;
    public GameObject scriptPlayerButtonObject;
    public ScriptAssetListViewMenu scriptSelectMenu;
    public ScriptTextViewMenu scriptTextViewMenu;
    protected int ServantLimitCount;
    public UILabel serverInfoLabel;
    public ServerSettingMenu serverSettingMenu;
    public GameObject testBase;
    public DebugListViewMenu topMenu;
    public UILabel userInfoLabel;
    public GameObject userInfoObject;
    public UILabel versionInfoLabel;
    public WebConnectMenu webViewConnectMenu;

    public override void beginInitialize()
    {
        base.beginInitialize();
        this.userInfoObject.SetActive(false);
        if (!ManagerConfig.UseMock)
        {
            if (SingletonMonoBehaviour<NetworkManager>.Instance.ReadAuth() == NetworkManager.ReadResult.OK)
            {
                long userId = NetworkManager.UserId;
                if (userId > 0L)
                {
                    this.userInfoObject.SetActive(true);
                    this.serverInfoLabel.text = "Server " + NetworkManager.UserCreateServer;
                    this.userInfoLabel.text = "UserId " + userId;
                }
            }
            this.assetbundleInfoObject.SetActive(true);
            this.versionInfoLabel.text = AssetManager.GetMasterVersion() + " " + AssetManager.GetDateVersion();
            this.folderInfoLabel.text = "[" + NetworkManager.GetDataServerFolderName() + "]";
        }
        if (ManagerConfig.ServerDefaultType == "SCRIPT")
        {
            this.scriptPlayerButtonObject.SetActive(true);
        }
        else
        {
            this.returnButtonObject.SetActive(true);
        }
        if ((ManagerConfig.ServerDefaultType == "SCRIPT") && ManagerConfig.IsNetworkMock)
        {
            this.scriptPlayerBase.SetActive(true);
        }
        else
        {
            this.debugBase.SetActive(true);
            this.topMenu.Init();
        }
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
    }

    public bool BootSelect()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("DEBUG_TEST_EXIT_APPLICATION_TITLE"), LocalizationManager.Get("DEBUG_TEST_EXIT_APPLICATION_DITAIL"), new CommonConfirmDialog.ClickDelegate(this.EndBootSelect));
        return true;
    }

    private void callbackLogin(string result)
    {
        if (result == "do_signup")
        {
            base.myFSM.SendEvent("REQUEST_DO_SIGNUP");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    public void ClearAssetStorageCache()
    {
        AssetStorageCache.ClearCacheAll(true);
    }

    public void ClearMasterDataCache()
    {
        DataManager.ClearCacheAll();
    }

    public void ClearReboot()
    {
        AssetStorageCache.ClearCache(true);
        Application.Quit();
    }

    public void ClearUserInfo()
    {
        UserSaveData.DeleteSaveData();
    }

    protected void ClickNoblePhantasm(int svtId)
    {
        Dictionary<string, int> data = new Dictionary<string, int> {
            ["servantId"] = svtId,
            ["limitCount"] = this.ServantLimitCount
        };
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Battle, SceneManager.FadeType.BLACK, data);
    }

    public bool ClientTutorialComplete()
    {
        TutorialFlag.CompleteProgress();
        return true;
    }

    protected void EndBackSelect(bool result)
    {
        this.backSelectMenu.Close();
        base.myFSM.SendEvent("FINISHED");
    }

    protected void EndBootSelect(bool result)
    {
        if (result)
        {
            Application.Quit();
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
            base.myFSM.SendEvent("FINISHED");
        }
    }

    protected void EndConnectScriptPlay()
    {
        base.myFSM.SendEvent("SCRIPT_FINISHED");
    }

    protected void EndConnectScriptSelect(bool result)
    {
        this.connectScriptMenu.Close();
        if (result)
        {
            base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_OK");
        }
        else
        {
            base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_CANCEL");
        }
    }

    protected void EndConnectWebView(bool result)
    {
        this.webViewConnectMenu.Close();
        base.myFSM.SendEvent("FINISHED");
    }

    protected void EndFigureSelect(bool result)
    {
        this.figureSelectMenu.Close();
        base.myFSM.SendEvent("FINISHED");
    }

    protected void EndNoblePhantasm(bool result)
    {
    }

    protected void EndPlayDefaultScriptSelect(bool result)
    {
        this.playScriptDefaultFileMenu.Close();
        base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_CANCEL");
    }

    protected void EndPlayScriptSelect(bool result)
    {
        this.playScriptFileMenu.Close();
        base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_CANCEL");
    }

    protected void EndReceiptSelect(bool result)
    {
        this.receiptSelectMenu.Close();
        if (result)
        {
            base.myFSM.SendEvent("RECEIPT_SELECT_OK");
        }
        else
        {
            base.myFSM.SendEvent("RECEIPT_SELECT_CANCEL");
        }
    }

    protected void EndScriptSelect(bool result)
    {
        this.scriptSelectMenu.Close();
        if (result)
        {
            base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_OK");
        }
        else
        {
            base.myFSM.SendEvent("SCRIPT_PLAY_SELECT_CANCEL");
        }
    }

    protected void EndServerSetting(bool result)
    {
        this.serverSettingMenu.Close();
        if (result)
        {
            base.myFSM.SendEvent("SERVER_SETTING_INPUT_OK");
        }
        else
        {
            base.myFSM.SendEvent("SERVER_SETTING_INPUT_CANCEL");
        }
    }

    protected void EndStartPlayScriptPlay()
    {
        base.myFSM.SendEvent("SCRIPT_FINISHED");
    }

    protected void EndTestRequest()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
        base.myFSM.SendEvent("FINISHED");
    }

    public void EntryService()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.EntryService(new NetworkManager.ResultCallbackFunc(this.OnEndEntryService));
    }

    protected void ErrorConnectScriptPlay()
    {
        base.myFSM.SendEvent("SCRIPT_FINISHED");
    }

    protected void ErrorStartPlayScriptPlay()
    {
        base.myFSM.SendEvent("SCRIPT_FINISHED");
    }

    protected void OnEndEntryService(string result)
    {
        base.myFSM.SendEvent("SERVICE_OK");
    }

    public void Reboot()
    {
        SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
    }

    public void ReleaseService()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.ReleseService();
    }

    [DebuggerHidden]
    private IEnumerator RequestConnectListCR(string connectPath, string objectPath, System.Action endCallback, System.Action errorCallback, int jumpLine = -1, bool isView = false) => 
        new <RequestConnectListCR>c__Iterator39 { 
            connectPath = connectPath,
            objectPath = objectPath,
            errorCallback = errorCallback,
            isView = isView,
            jumpLine = jumpLine,
            endCallback = endCallback,
            <$>connectPath = connectPath,
            <$>objectPath = objectPath,
            <$>errorCallback = errorCallback,
            <$>isView = isView,
            <$>jumpLine = jumpLine,
            <$>endCallback = endCallback,
            <>f__this = this
        };

    public void RequestLogin()
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.RequestLogin(new NetworkManager.LoginCallbackFunc(this.callbackLogin), true);
    }

    public bool RequestSignup()
    {
        this.debugSignupMenu.RequestSignup(base.myFSM);
        return true;
    }

    public bool SetupTakeover()
    {
        this.debugSignupMenu.SetupTakeover(base.myFSM);
        return true;
    }

    public bool StartBackSelect()
    {
        this.backSelectMenu.Open(new BackViewListViewMenu.CallbackFunc(this.EndBackSelect));
        return true;
    }

    public bool StartConnectScript(string connectPath, string objectPath, System.Action endCallback, System.Action errorCallback, int jumpLine = -1, bool isView = false)
    {
        base.StartCoroutine(this.RequestConnectListCR(connectPath, objectPath, endCallback, errorCallback, jumpLine, isView));
        return true;
    }

    public bool StartConnectScriptPlay()
    {
        this.StartConnectScript(ScriptManager.GetScriptServerSetting(), ScriptManager.GetScriptObjectSetting(), new System.Action(this.EndConnectScriptPlay), new System.Action(this.ErrorConnectScriptPlay), -1, false);
        return true;
    }

    public bool StartConnectScriptSelect()
    {
        this.connectScriptMenu.Open(new ScriptConnectMenu.CallbackFunc(this.EndConnectScriptSelect));
        return true;
    }

    public bool StartConnectWebView()
    {
        this.webViewConnectMenu.Open(new WebConnectMenu.CallbackFunc(this.EndConnectWebView));
        return true;
    }

    public bool StartDefaultScriptSelect()
    {
        this.playScriptDefaultFileMenu.Open(new ScriptDefaultFilePlayerMenu.CallbackFunc(this.EndPlayDefaultScriptSelect));
        return true;
    }

    public bool StartFigureSelect()
    {
        this.figureSelectMenu.Open(new FigureViewListViewMenu.CallbackFunc(this.EndFigureSelect));
        return true;
    }

    public bool StartFileScript(string playPath, string objectPath, System.Action endCallback, System.Action errorCallback, int jumpLine = -1, bool isView = false)
    {
        <StartFileScript>c__AnonStoreyE8 ye = new <StartFileScript>c__AnonStoreyE8 {
            errorCallback = errorCallback,
            endCallback = endCallback,
            <>f__this = this,
            scriptData = null,
            orgScriptData = null
        };
        Debug.Log("PlayScriptLoadData: " + playPath + "/" + objectPath + " start");
        string path = playPath + "/" + objectPath;
        string message = null;
        if (!Directory.Exists(playPath))
        {
            message = "not find directory [" + playPath + "]";
        }
        else if (!File.Exists(path))
        {
            message = "not find file [" + path + "]";
        }
        else
        {
            try
            {
                string str3 = File.ReadAllText(path);
                ye.scriptData = str3;
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            if (objectPath.EndsWith(".txt"))
            {
                string str4 = playPath + "/" + objectPath.Substring(0, objectPath.Length - 4) + ".org";
                if (File.Exists(str4))
                {
                    try
                    {
                        string str5 = File.ReadAllText(str4);
                        ye.orgScriptData = str5;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        if (message != null)
        {
            Debug.LogError("PlayScriptLoadData: " + message);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("Play Script Error", message, new ErrorDialog.ClickDelegate(ye.<>m__24B), false);
        }
        else
        {
            object[] objArray1 = new object[] { "PlayScriptLoadData: ", objectPath, " end [", ye.scriptData[0].GetHashCode(), "]" };
            Debug.Log(string.Concat(objArray1));
            if (isView)
            {
                this.scriptTextViewMenu.Open((ye.orgScriptData == null) ? ye.scriptData : ye.orgScriptData, jumpLine, new ScriptTextViewMenu.CallbackFunc(ye.<>m__24C));
            }
            else
            {
                ScriptManager.DebugPlay(ScriptManager.GetScriptStartModeSetting(), ye.scriptData, ye.orgScriptData, ScriptManager.GetScriptGenderSetting(), jumpLine, new ScriptManager.CallbackFunc(ye.<>m__24D));
            }
        }
        return true;
    }

    public bool StartFileScriptPlay()
    {
        this.StartFileScript(ScriptManager.GetScriptPlayerPathSetting(), ScriptManager.GetScriptPlayerObjectSetting(), new System.Action(this.EndStartPlayScriptPlay), new System.Action(this.ErrorStartPlayScriptPlay), -1, false);
        return true;
    }

    public bool StartFileScriptSelect()
    {
        this.playScriptFileMenu.Open(new ScriptFilePlayerMenu.CallbackFunc(this.EndPlayScriptSelect));
        return true;
    }

    public bool StartNoblePhantasm()
    {
        <StartNoblePhantasm>c__AnonStoreyE9 ye = new <StartNoblePhantasm>c__AnonStoreyE9 {
            <>f__this = this
        };
        this.NoblePhantasmSelectPanel.SetActive(true);
        ye.limitCountLabel = this.NoblePhantasmSelectPanel.transform.getNodeFromName("LimitCountLabel", true);
        EventDelegate.Add(ye.limitCountLabel.GetComponent<UIButton>().onClick, new EventDelegate.Callback(ye.<>m__24E));
        EventDelegate.Add(this.NoblePhantasmSelectPanel.transform.getNodeFromName("CancelButton", true).GetComponent<UIButton>().onClick, new EventDelegate.Callback(ye.<>m__24F));
        UIScrollView componentInChildren = this.NoblePhantasmSelectPanel.GetComponentInChildren<UIScrollView>();
        UIGrid grid = this.NoblePhantasmSelectPanel.GetComponentInChildren<UIGrid>();
        Font trueTypeFont = null;
        Transform transform = this.NoblePhantasmSelectPanel.transform.getNodeFromName("TitleLabel", true);
        if (transform != null)
        {
            trueTypeFont = transform.GetComponent<UILabel>().trueTypeFont;
        }
        if (grid != null)
        {
            foreach (DataEntityBase base2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityList())
            {
                <StartNoblePhantasm>c__AnonStoreyEA yea = new <StartNoblePhantasm>c__AnonStoreyEA {
                    <>f__this = this,
                    svt = (ServantEntity) base2
                };
                GameObject go = new GameObject();
                UILabel label = go.AddComponent<UILabel>();
                label.trueTypeFont = trueTypeFont;
                label.overflowMethod = UILabel.Overflow.ResizeFreely;
                label.color = Color.black;
                label.fontSize = 0x18;
                label.pivot = UIWidget.Pivot.Left;
                label.text = yea.svt.id + ":" + yea.svt.name;
                NGUITools.AddWidgetCollider(go);
                EventDelegate.Add(go.AddComponent<UIButton>().onClick, new EventDelegate.Callback(yea.<>m__250));
                go.AddComponent<UIDragScrollView>().scrollView = componentInChildren;
                go.transform.parent = grid.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }
        return true;
    }

    public bool StartReceiptSelect()
    {
        this.receiptSelectMenu.Open(new ReceiptListViewMenu.CallbackFunc(this.EndReceiptSelect));
        return true;
    }

    public bool StartScriptSelect()
    {
        AssetManager.resetAssetStorageVersion("ScriptActionEncrypt");
        this.scriptSelectMenu.Open(new ScriptAssetListViewMenu.CallbackFunc(this.EndScriptSelect));
        return true;
    }

    public bool StartServerSettingInput()
    {
        this.serverSettingMenu.Open(new ServerSettingMenu.CallbackFunc(this.EndServerSetting));
        return true;
    }

    public bool StartSignupInput()
    {
        this.debugSignupMenu.Open(base.myFSM);
        return true;
    }

    public bool StartTestRequest()
    {
        ScriptManager.PlayGacha(0x18704, 0, true, "Test", new System.Action(this.EndTestRequest));
        return true;
    }

    public void StartTopInput()
    {
        if (ManagerConfig.UseDebugCommand)
        {
            this.topMenu.StartInput();
        }
    }

    public void SwitchingAllocMem()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.switchingAllocMem();
    }

    public void SwitchingAssetLoad()
    {
        SingletonMonoBehaviour<AssetManager>.Instance.SwitchingDebugStatusOut();
    }

    public void TestService()
    {
        CommonServicePluginScript.AlertDialog("Test");
    }

    [CompilerGenerated]
    private sealed class <RequestConnectListCR>c__Iterator39 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>connectPath;
        internal System.Action <$>endCallback;
        internal System.Action <$>errorCallback;
        internal bool <$>isView;
        internal int <$>jumpLine;
        internal string <$>objectPath;
        internal DebugTestRootComponent <>f__this;
        internal string <errorMessage>__5;
        internal string <fullPath>__1;
        internal WWW <loader>__2;
        internal float <loadProgress>__4;
        internal float <requestTime>__3;
        internal string <scriptData>__0;
        internal string connectPath;
        internal System.Action endCallback;
        internal System.Action errorCallback;
        internal bool isView;
        internal int jumpLine;
        internal string objectPath;

        internal void <>m__251(bool isDecide)
        {
            if (this.errorCallback != null)
            {
                this.errorCallback();
            }
        }

        internal void <>m__252(ScriptTextViewMenu.ResultKind viewResult, int viewJumpLine)
        {
            this.<>f__this.scriptTextViewMenu.Close();
            switch (viewResult)
            {
                case ScriptTextViewMenu.ResultKind.PLAY:
                case ScriptTextViewMenu.ResultKind.JUMP_PLAY:
                    ScriptManager.DebugPlay(ScriptManager.GetScriptStartModeSetting(), this.<scriptData>__0, null, ScriptManager.GetScriptGenderSetting(), viewJumpLine, delegate (bool isExit) {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
                        if (this.endCallback != null)
                        {
                            this.endCallback();
                        }
                    });
                    break;

                default:
                    if (this.endCallback != null)
                    {
                        this.endCallback();
                    }
                    break;
            }
        }

        internal void <>m__253(bool isExit)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
            if (this.endCallback != null)
            {
                this.endCallback();
            }
        }

        internal void <>m__254(bool isExit)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
            if (this.endCallback != null)
            {
                this.endCallback();
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
                    this.<scriptData>__0 = null;
                    Debug.Log("ConnectScriptLoadData: " + this.connectPath + "/" + this.objectPath + " start");
                    this.<fullPath>__1 = this.connectPath + "/" + this.objectPath;
                    if (this.<fullPath>__1.StartsWith("file://"))
                    {
                        this.<fullPath>__1 = "file://" + WWW.EscapeURL(this.<fullPath>__1.Substring(7));
                    }
                    this.<loader>__2 = new WWW(this.<fullPath>__1);
                    this.<requestTime>__3 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__4 = 0f;
                    this.<errorMessage>__5 = null;
                    break;

                case 1:
                    break;

                default:
                    goto Label_02D2;
            }
            while (!this.<loader>__2.isDone)
            {
                if (this.<loader>__2.progress != this.<loadProgress>__4)
                {
                    this.<requestTime>__3 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__4 = this.<loader>__2.progress;
                }
                else if (Time.time >= this.<requestTime>__3)
                {
                    Debug.LogWarning("TimeOut");
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            if (!this.<loader>__2.isDone)
            {
                this.<errorMessage>__5 = "file download time over";
            }
            else if (!string.IsNullOrEmpty(this.<loader>__2.error))
            {
                this.<errorMessage>__5 = this.<loader>__2.error;
            }
            else
            {
                this.<scriptData>__0 = this.<loader>__2.text;
            }
            this.<loader>__2.Dispose();
            if (this.<errorMessage>__5 != null)
            {
                Debug.LogError("ConnectLoadData: " + this.<errorMessage>__5);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("Connect Script Error", this.<errorMessage>__5, new ErrorDialog.ClickDelegate(this.<>m__251), false);
            }
            else
            {
                object[] objArray1 = new object[] { "ConnectScriptLoadData: ", this.objectPath, " end [", this.<scriptData>__0[0].GetHashCode(), "]" };
                Debug.Log(string.Concat(objArray1));
                if (this.isView)
                {
                    this.<>f__this.scriptTextViewMenu.Open(this.<scriptData>__0, this.jumpLine, new ScriptTextViewMenu.CallbackFunc(this.<>m__252));
                }
                else
                {
                    ScriptManager.DebugPlay(ScriptManager.GetScriptStartModeSetting(), this.<scriptData>__0, null, ScriptManager.GetScriptGenderSetting(), this.jumpLine, new ScriptManager.CallbackFunc(this.<>m__253));
                }
            }
            this.$PC = -1;
        Label_02D2:
            return false;
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
    private sealed class <StartFileScript>c__AnonStoreyE8
    {
        internal DebugTestRootComponent <>f__this;
        internal System.Action endCallback;
        internal System.Action errorCallback;
        internal string orgScriptData;
        internal string scriptData;

        internal void <>m__24B(bool isDecide)
        {
            if (this.errorCallback != null)
            {
                this.errorCallback();
            }
        }

        internal void <>m__24C(ScriptTextViewMenu.ResultKind viewResult, int viewJumpLine)
        {
            this.<>f__this.scriptTextViewMenu.Close();
            switch (viewResult)
            {
                case ScriptTextViewMenu.ResultKind.PLAY:
                case ScriptTextViewMenu.ResultKind.JUMP_PLAY:
                    ScriptManager.DebugPlay(ScriptManager.GetScriptStartModeSetting(), this.scriptData, this.orgScriptData, ScriptManager.GetScriptGenderSetting(), viewJumpLine, delegate (bool isExit) {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
                        if (this.endCallback != null)
                        {
                            this.endCallback();
                        }
                    });
                    break;

                default:
                    if (this.endCallback != null)
                    {
                        this.endCallback();
                    }
                    break;
            }
        }

        internal void <>m__24D(bool isExit)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
            if (this.endCallback != null)
            {
                this.endCallback();
            }
        }

        internal void <>m__255(bool isExit)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
            if (this.endCallback != null)
            {
                this.endCallback();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <StartNoblePhantasm>c__AnonStoreyE9
    {
        internal DebugTestRootComponent <>f__this;
        internal Transform limitCountLabel;

        internal void <>m__24E()
        {
            this.<>f__this.ServantLimitCount++;
            this.<>f__this.ServantLimitCount = this.<>f__this.ServantLimitCount % 5;
            this.limitCountLabel.GetComponent<UILabel>().text = "LimitCount:" + this.<>f__this.ServantLimitCount;
        }

        internal void <>m__24F()
        {
            this.<>f__this.NoblePhantasmSelectPanel.SetActive(false);
            this.<>f__this.myFSM.SendEvent("FINISHED");
        }
    }

    [CompilerGenerated]
    private sealed class <StartNoblePhantasm>c__AnonStoreyEA
    {
        internal DebugTestRootComponent <>f__this;
        internal ServantEntity svt;

        internal void <>m__250()
        {
            this.<>f__this.ClickNoblePhantasm(this.svt.id);
        }
    }
}

