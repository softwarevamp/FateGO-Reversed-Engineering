using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UniWebView : MonoBehaviour
{
    private bool _backButtonEnable = true;
    private bool _bouncesEnable;
    private string _currentGUID;
    private bool _immersiveMode = true;
    [SerializeField]
    private UniWebViewEdgeInsets _insets = new UniWebViewEdgeInsets(0, 0, 0, 0);
    private int _lastScreenHeight;
    private bool _zoomEnable;
    public bool autoShowWhenLoadComplete;
    public bool loadOnStart;
    public bool toolBarShow;
    public string url;

    public event InsetsForScreenOreitationDelegate InsetsForScreenOreitation;

    public event EvalJavaScriptFinishedDelegate OnEvalJavaScriptFinished;

    public event LoadBeginDelegate OnLoadBegin;

    public event LoadCompleteDelegate OnLoadComplete;

    public event ReceivedKeyCodeDelegate OnReceivedKeyCode;

    public event ReceivedMessageDelegate OnReceivedMessage;

    public event WebViewShouldCloseDelegate OnWebViewShouldClose;

    public void AddJavaScript(string javaScript)
    {
        UniWebViewPlugin.AddJavaScript(base.gameObject.name, javaScript);
    }

    public void AddPermissionRequestTrustSite(string url)
    {
        UniWebViewPlugin.AddPermissionRequestTrustSite(base.gameObject.name, url);
    }

    public void AddUrlScheme(string scheme)
    {
        UniWebViewPlugin.AddUrlScheme(base.gameObject.name, scheme);
    }

    private void Awake()
    {
        this._currentGUID = Guid.NewGuid().ToString();
        base.gameObject.name = base.gameObject.name + this._currentGUID;
        UniWebViewPlugin.Init(base.gameObject.name, this.insets.top, this.insets.left, this.insets.bottom, this.insets.right);
        this._lastScreenHeight = UniWebViewHelper.screenHeight;
    }

    public bool CanGoBack() => 
        UniWebViewPlugin.CanGoBack(base.gameObject.name);

    public bool CanGoForward() => 
        UniWebViewPlugin.CanGoForward(base.gameObject.name);

    public void CleanCache()
    {
        UniWebViewPlugin.CleanCache(base.gameObject.name);
    }

    public void CleanCookie(string key = null)
    {
        UniWebViewPlugin.CleanCookie(base.gameObject.name, key);
    }

    private void EvalJavaScriptFinished(string result)
    {
        if (this.OnEvalJavaScriptFinished != null)
        {
            this.OnEvalJavaScriptFinished(this, result);
        }
    }

    public void EvaluatingJavaScript(string javaScript)
    {
        UniWebViewPlugin.EvaluatingJavaScript(base.gameObject.name, javaScript);
    }

    private void ForceUpdateInsetsInternal(UniWebViewEdgeInsets insets)
    {
        this._insets = insets;
        UniWebViewPlugin.ChangeSize(base.gameObject.name, this.insets.top, this.insets.left, this.insets.bottom, this.insets.right);
    }

    public void GoBack()
    {
        UniWebViewPlugin.GoBack(base.gameObject.name);
    }

    public void GoForward()
    {
        UniWebViewPlugin.GoForward(base.gameObject.name);
    }

    public void Hide()
    {
        UniWebViewPlugin.Dismiss(base.gameObject.name);
    }

    public void HideToolBar(bool animate)
    {
    }

    public void Load()
    {
        string url = !string.IsNullOrEmpty(this.url) ? this.url.Trim() : "about:blank";
        UniWebViewPlugin.Load(base.gameObject.name, url);
    }

    public void Load(string aUrl)
    {
        this.url = aUrl;
        this.Load();
    }

    private void LoadBegin(string url)
    {
        Debug.Log("Begin to load: " + url);
        if (this.OnLoadBegin != null)
        {
            this.OnLoadBegin(this, url);
        }
    }

    private void LoadComplete(string message)
    {
        bool flag = string.Equals(message, string.Empty);
        bool flag2 = this.OnLoadComplete != null;
        if (flag)
        {
            if (flag2)
            {
                this.OnLoadComplete(this, true, null);
            }
            if (this.autoShowWhenLoadComplete)
            {
                this.Show();
            }
        }
        else
        {
            Debug.LogWarning("Web page load failed: " + base.gameObject.name + "; url: " + this.url + "; error:" + message);
            if (flag2)
            {
                this.OnLoadComplete(this, false, message);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator LoadFromJarPackage(string jarFilePath) => 
        new <LoadFromJarPackage>c__Iterator42 { 
            jarFilePath = jarFilePath,
            <$>jarFilePath = jarFilePath,
            <>f__this = this
        };

    public void LoadHTMLString(string htmlString, string baseUrl)
    {
        UniWebViewPlugin.LoadHTMLString(base.gameObject.name, htmlString, baseUrl);
    }

    private void OnDestroy()
    {
        UniWebViewPlugin.Destroy(base.gameObject.name);
        base.gameObject.name = base.gameObject.name.Replace(this._currentGUID, string.Empty);
    }

    private bool OrientationChanged()
    {
        int screenHeight = UniWebViewHelper.screenHeight;
        if (this._lastScreenHeight != screenHeight)
        {
            this._lastScreenHeight = screenHeight;
            return true;
        }
        return false;
    }

    private void ReceivedMessage(string rawMessage)
    {
        UniWebViewMessage message = new UniWebViewMessage(rawMessage);
        if (this.OnReceivedMessage != null)
        {
            this.OnReceivedMessage(this, message);
        }
    }

    public void Reload()
    {
        UniWebViewPlugin.Reload(base.gameObject.name);
    }

    public void RemoveUrlScheme(string scheme)
    {
        UniWebViewPlugin.RemoveUrlScheme(base.gameObject.name, scheme);
    }

    public static void ResetUserAgent()
    {
        UniWebViewPlugin.SetUserAgent(null);
    }

    private void ResizeInternal()
    {
        int screenHeight = UniWebViewHelper.screenHeight;
        int screenWidth = UniWebViewHelper.screenWidth;
        UniWebViewEdgeInsets insets = this.insets;
        if (this.InsetsForScreenOreitation != null)
        {
            UniWebViewOrientation orientation = (screenHeight < screenWidth) ? UniWebViewOrientation.LandScape : UniWebViewOrientation.Portrait;
            insets = this.InsetsForScreenOreitation(this, orientation);
        }
        this.ForceUpdateInsetsInternal(insets);
    }

    public void SetBackgroundColor(Color color)
    {
        UniWebViewPlugin.SetBackgroundColor(base.gameObject.name, color.r, color.g, color.b, color.a);
    }

    public void SetShowSpinnerWhenLoading(bool show)
    {
        UniWebViewPlugin.SetSpinnerShowWhenLoading(base.gameObject.name, show);
    }

    public void SetSpinnerLabelText(string text)
    {
        UniWebViewPlugin.SetSpinnerText(base.gameObject.name, text);
    }

    [Obsolete("SetTransparentBackground is deprecated, please use SetBackgroundColor instead.")]
    public void SetTransparentBackground(bool transparent = true)
    {
        UniWebViewPlugin.TransparentBackground(base.gameObject.name, transparent);
    }

    public static void SetUserAgent(string value)
    {
        UniWebViewPlugin.SetUserAgent(value);
    }

    public void SetUseWideViewPort(bool use)
    {
        UniWebViewPlugin.SetUseWideViewPort(base.gameObject.name, use);
    }

    public void Show()
    {
        this._lastScreenHeight = UniWebViewHelper.screenHeight;
        this.ResizeInternal();
        UniWebViewPlugin.Show(base.gameObject.name);
    }

    public void ShowToolBar(bool animate)
    {
    }

    private void Start()
    {
        if (this.loadOnStart)
        {
            this.Load();
        }
    }

    public void Stop()
    {
        UniWebViewPlugin.Stop(base.gameObject.name);
    }

    private void Update()
    {
        if (this.OrientationChanged())
        {
            this.ResizeInternal();
        }
    }

    private void WebViewDone(string message)
    {
        bool flag = true;
        if (this.OnWebViewShouldClose != null)
        {
            flag = this.OnWebViewShouldClose(this);
        }
        if (flag)
        {
            this.Hide();
            UnityEngine.Object.Destroy(this);
        }
    }

    private void WebViewKeyDown(string message)
    {
        int keyCode = Convert.ToInt32(message);
        if (this.OnReceivedKeyCode != null)
        {
            this.OnReceivedKeyCode(this, keyCode);
        }
    }

    public float alpha
    {
        get => 
            UniWebViewPlugin.GetAlpha(base.gameObject.name);
        set
        {
            UniWebViewPlugin.SetAlpha(base.gameObject.name, Mathf.Clamp01(value));
        }
    }

    public bool backButtonEnable
    {
        get => 
            this._backButtonEnable;
        set
        {
            if (this._backButtonEnable != value)
            {
                this._backButtonEnable = value;
                UniWebViewPlugin.SetBackButtonEnable(base.gameObject.name, this._backButtonEnable);
            }
        }
    }

    public bool bouncesEnable
    {
        get => 
            this._bouncesEnable;
        set
        {
            if (this._bouncesEnable != value)
            {
                this._bouncesEnable = value;
                UniWebViewPlugin.SetBounces(base.gameObject.name, this._bouncesEnable);
            }
        }
    }

    public string currentUrl =>
        UniWebViewPlugin.GetCurrentUrl(base.gameObject.name);

    public bool immersiveMode
    {
        get => 
            this._immersiveMode;
        set
        {
            this._immersiveMode = value;
            UniWebViewPlugin.SetImmersiveModeEnabled(base.gameObject.name, this._immersiveMode);
        }
    }

    public UniWebViewEdgeInsets insets
    {
        get => 
            this._insets;
        set
        {
            if (this._insets != value)
            {
                this.ForceUpdateInsetsInternal(value);
            }
        }
    }

    public string userAgent =>
        UniWebViewPlugin.GetUserAgent(base.gameObject.name);

    public bool zoomEnable
    {
        get => 
            this._zoomEnable;
        set
        {
            if (this._zoomEnable != value)
            {
                this._zoomEnable = value;
                UniWebViewPlugin.SetZoomEnable(base.gameObject.name, this._zoomEnable);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <LoadFromJarPackage>c__Iterator42 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>jarFilePath;
        internal UniWebView <>f__this;
        internal WWW <stream>__0;
        internal string jarFilePath;

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
                    this.<stream>__0 = new WWW(this.jarFilePath);
                    this.$current = this.<stream>__0;
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.<stream>__0.error == null)
                    {
                        this.<>f__this.LoadHTMLString(this.<stream>__0.text, string.Empty);
                        this.$PC = -1;
                        break;
                    }
                    if (this.<>f__this.OnLoadComplete != null)
                    {
                        this.<>f__this.OnLoadComplete(this.<>f__this, false, this.<stream>__0.error);
                    }
                    break;
            }
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

    public delegate void EvalJavaScriptFinishedDelegate(UniWebView webView, string result);

    public delegate UniWebViewEdgeInsets InsetsForScreenOreitationDelegate(UniWebView webView, UniWebViewOrientation orientation);

    public delegate void LoadBeginDelegate(UniWebView webView, string loadingUrl);

    public delegate void LoadCompleteDelegate(UniWebView webView, bool success, string errorMessage);

    public delegate void ReceivedKeyCodeDelegate(UniWebView webView, int keyCode);

    public delegate void ReceivedMessageDelegate(UniWebView webView, UniWebViewMessage message);

    public delegate bool WebViewShouldCloseDelegate(UniWebView webView);
}

