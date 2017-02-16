using System;
using UnityEngine;

public class WebViewManager : SingletonMonoBehaviour<WebViewManager>
{
    [SerializeField]
    protected UIPanel basePanel;
    [SerializeField]
    protected UIWidget baseWindow;
    protected System.Action callbackFunc;
    protected static readonly float CLOSE_TIME;
    [SerializeField]
    protected Camera commonCamera;
    protected string errorMessage;
    protected bool isButtonEnable;
    public GameObject m_bg;
    public GameObject m_screen;
    public GameObject m_ScrollBar;
    public GameObject m_ScrollView;
    public GameObject m_test;
    protected static readonly float OPEN_TIME = 0.2f;
    [SerializeField]
    protected UILabel titleLabel;
    protected UniWebView webView;
    [SerializeField]
    protected UIWidget webViewBase;
    [SerializeField]
    protected UIWidget webViewScreen;

    protected void EndClose()
    {
        this.webViewBase.gameObject.SetActive(false);
        TouchEffectManager.SetBlock(false);
        SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    private UniWebViewEdgeInsets InsetsForScreenOreitation(UniWebView webView, UniWebViewOrientation orientation)
    {
        int screenWidth = UniWebViewHelper.screenWidth;
        int num2 = (screenWidth * 0x39) / 0x400;
        int aLeft = (screenWidth * 0) / 0x400;
        int num4 = 0;
        return new UniWebViewEdgeInsets(((num2 + aLeft) + num4) - 2, aLeft, aLeft + num4, aLeft);
    }

    public void OnClickBack()
    {
        if (this.webView != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.webView.GoBack();
        }
    }

    public void OnClickClose()
    {
        if (this.webView != null)
        {
            this.webView.SetShowSpinnerWhenLoading(false);
            this.webView.Hide();
            UnityEngine.Object.Destroy(this.webView);
            this.webView.OnReceivedMessage -= new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
            this.webView.OnLoadComplete -= new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
            this.webView.OnWebViewShouldClose -= new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
            this.webView.OnEvalJavaScriptFinished -= new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
            this.webView.InsetsForScreenOreitation -= new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
            this.webView = null;
        }
        if (this.isButtonEnable)
        {
            this.isButtonEnable = false;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.EndClose();
        }
    }

    private void OnEvalJavaScriptFinished(UniWebView webView, string result)
    {
        Debug.Log("js result: " + result);
    }

    protected void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            webView.Show();
        }
        else
        {
            Debug.Log("Something wrong in webview loading: " + errorMessage);
            this.errorMessage = errorMessage;
        }
    }

    protected void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        if (this.isButtonEnable)
        {
            Debug.Log(message.rawMessage);
            if (string.Equals(message.path, "browser"))
            {
                Application.OpenURL(NetworkManager.getWebUrl(message.args["url"], false, true));
            }
            else if (string.Equals(message.path, "dialogOK"))
            {
                string title = !message.args.ContainsKey("title") ? null : message.args["title"];
                WebViewPluginScript.MessageDialog(title, message.args["msg"]);
            }
            else if (string.Equals(message.path, "transition"))
            {
                if (webView == this.webView)
                {
                    this.OnClickClose();
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(message.args["scene"], SceneManager.FadeType.BLACK, null);
                }
            }
            else if (string.Equals(message.path, "appStore"))
            {
                string id = !message.args.ContainsKey("id") ? null : message.args["id"];
                NetworkManager.getStoreUrl("iOS", id, new NetworkManager.StoreCallbackFunc(this.OnWebViewStore));
            }
            else if (string.Equals(message.path, "googlePlay"))
            {
                string str4 = !message.args.ContainsKey("id") ? null : message.args["id"];
                NetworkManager.getStoreUrl("Android", str4, new NetworkManager.StoreCallbackFunc(this.OnWebViewStore));
            }
            else if (string.Equals(message.path, "line"))
            {
                if (message.args.ContainsKey("msg"))
                {
                    WebViewPluginScript.SendLine(message.args["msg"]);
                }
            }
            else if (string.Equals(message.path, "facebook"))
            {
                if (message.args.ContainsKey("msg"))
                {
                    WebViewPluginScript.SendFaceBook(message.args["msg"]);
                }
            }
            else if (string.Equals(message.path, "twitter"))
            {
                if (message.args.ContainsKey("msg"))
                {
                    WebViewPluginScript.SendTwitter(message.args["msg"]);
                }
            }
            else if (string.Equals(message.path, "close"))
            {
                if (webView == this.webView)
                {
                    this.OnClickClose();
                }
            }
            else if (string.Equals(message.path, "mail"))
            {
                bool isInquiry = message.args.ContainsKey("inquiry");
                this.OpenSupportMail(isInquiry);
            }
            else if (string.Equals(message.path, "AnnouncementUrlClose") && (this.webView != null))
            {
                this.webView.SetShowSpinnerWhenLoading(false);
                this.webView.Hide();
                UnityEngine.Object.Destroy(this.webView);
                this.webView.OnReceivedMessage -= new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
                this.webView.OnLoadComplete -= new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
                this.webView.OnWebViewShouldClose -= new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
                this.webView.OnEvalJavaScriptFinished -= new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
                this.webView.InsetsForScreenOreitation -= new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
                this.webView = null;
            }
        }
    }

    private bool OnWebViewShouldClose(UniWebView webView)
    {
        if (webView == this.webView)
        {
            this.OnClickClose();
            return true;
        }
        return false;
    }

    protected void OnWebViewStore(string url)
    {
        if (url != null)
        {
            this.OpenViewLocal(string.Empty, url, null, this.callbackFunc);
        }
    }

    public bool OpenAnnouncementPanel(string title, System.Action callbackFunc)
    {
        this.m_bg.SetActive(true);
        this.m_screen.SetActive(false);
        this.m_test.SetActive(true);
        this.m_ScrollBar.SetActive(true);
        this.m_ScrollView.SetActive(true);
        this.callbackFunc = callbackFunc;
        this.titleLabel.text = title;
        if (!this.isButtonEnable)
        {
            this.webViewBase.gameObject.SetActive(true);
            TouchEffectManager.SetBlock(true);
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            this.EndOpen();
        }
        return true;
    }

    public static bool OpenNews(string title, int id, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        return instance?.OpenNewsLocal(title, id, callbackFunc);
    }

    protected bool OpenNewsLocal(string title, int id, System.Action callbackFunc)
    {
        NewsEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NewsMaster>(DataNameKind.Kind.NEWS).getEntityFromId<NewsEntity>(id);
        if (entity != null)
        {
            switch (((News.Type) entity.type))
            {
                case News.Type.HTML_TEXT:
                    return this.OpenViewLocal(title, null, entity.detail, callbackFunc);

                case News.Type.HTML_URL:
                    return this.OpenViewLocal(title, entity.detail, null, callbackFunc);
            }
            Debug.LogError(string.Concat(new object[] { "Not Suppert News [", id, "] Type [", entity.type, "]" }));
        }
        else
        {
            Debug.LogError("Empty News [" + id + "]");
        }
        return false;
    }

    public static bool OpenStringView(string title, string text, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        return instance?.OpenViewLocal(title, null, text, callbackFunc);
    }

    protected void OpenSupportMail(bool isInquiry)
    {
        string str = LocalizationManager.Get("SUPPORTMAIL_ADDRESS");
        string key = !isInquiry ? "SUPPORTMAIL_SUBJECT_SUGGEST" : "SUPPORTMAIL_SUBJECT_INQUIRY";
        string stringToEscape = LocalizationManager.Get(key);
        string friendCode = SingletonMonoBehaviour<NetworkManager>.Instance.GetFriendCode();
        string operatingSystem = SystemInfo.operatingSystem;
        string deviceModel = SystemInfo.deviceModel;
        string str7 = Uri.EscapeDataString(string.Format(LocalizationManager.Get("SUPPORTMAIL_BODY"), friendCode, operatingSystem, deviceModel));
        stringToEscape = Uri.EscapeDataString(stringToEscape);
        Application.OpenURL("mailto:" + str + "?subject=" + stringToEscape + "&body=" + str7);
    }

    public static bool OpenUniWebView(string title, string url, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        return instance?.OpenUniWebViewPanel(title, url, callbackFunc);
    }

    public bool OpenUniWebViewPanel(string title, string path, System.Action callbackFunc)
    {
        string str = NetworkManager.getWebUrl(path, false, true);
        Debug.Log("OpenView: url [" + path + "] " + str);
        this.titleLabel.text = title;
        this.webView = base.GetComponent<UniWebView>();
        if (this.webView == null)
        {
            this.webView = base.gameObject.AddComponent<UniWebView>();
            this.webView.OnReceivedMessage += new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
            this.webView.OnLoadComplete += new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
            this.webView.OnWebViewShouldClose += new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
            this.webView.OnEvalJavaScriptFinished += new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
            this.webView.InsetsForScreenOreitation += new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
            this.webView.backButtonEnable = true;
        }
        this.webView.SetShowSpinnerWhenLoading(true);
        this.errorMessage = null;
        this.webView.url = str;
        this.webView.Load();
        return true;
    }

    public static bool OpenView(string title, string path, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        return instance?.OpenViewLocal(title, path, null, callbackFunc);
    }

    public static bool OpenViewDynamic(string title, string path, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        if (instance == null)
        {
            return false;
        }
        string str = NetworkManager.getBaseUrl(false) + "WebView/" + path;
        return instance.OpenViewLocal(title, str, null, callbackFunc);
    }

    protected bool OpenViewLocal(string title, string path, string data, System.Action callbackFunc)
    {
        this.callbackFunc = callbackFunc;
        Debug.Log("OpenView: url [" + path + "] " + NetworkManager.getWebUrl(path, false, true));
        this.titleLabel.text = title;
        this.webView = base.GetComponent<UniWebView>();
        if (this.webView == null)
        {
            this.webView = base.gameObject.AddComponent<UniWebView>();
            this.webView.OnReceivedMessage += new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
            this.webView.OnLoadComplete += new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
            this.webView.OnWebViewShouldClose += new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
            this.webView.OnEvalJavaScriptFinished += new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
            this.webView.InsetsForScreenOreitation += new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
            this.webView.backButtonEnable = true;
        }
        this.webView.SetShowSpinnerWhenLoading(true);
        this.errorMessage = null;
        if (!this.isButtonEnable)
        {
            this.webViewBase.gameObject.SetActive(true);
            TouchEffectManager.SetBlock(true);
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            int screenWidth = UniWebViewHelper.screenWidth;
            int screenHeight = UniWebViewHelper.screenHeight;
            this.baseWindow.transform.localScale = Vector3.zero;
            this.baseWindow.transform.localScale = Vector3.one;
            this.EndOpen();
        }
        return true;
    }

    public static bool OpenWebView(string title, System.Action callbackFunc)
    {
        WebViewManager instance = SingletonMonoBehaviour<WebViewManager>.Instance;
        return instance?.OpenAnnouncementPanel(title, callbackFunc);
    }

    public bool IsBusy =>
        ((this.webViewBase != null) && this.webViewBase.gameObject.active);
}

