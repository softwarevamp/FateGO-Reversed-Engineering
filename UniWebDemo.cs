using System;
using UnityEngine;

public class UniWebDemo : MonoBehaviour
{
    private GameObject _cube;
    private string _errorMessage;
    private Vector3 _moveVector;
    private UniWebView _webView;
    public GameObject cubePrefab;
    public TextMesh tipTextMesh;

    private UniWebViewEdgeInsets InsetsForScreenOreitation(UniWebView webView, UniWebViewOrientation orientation)
    {
        int aBottom = (int) (UniWebViewHelper.screenHeight * 0.5f);
        if (orientation == UniWebViewOrientation.Portrait)
        {
            return new UniWebViewEdgeInsets(5, 5, aBottom, 5);
        }
        return new UniWebViewEdgeInsets(5, 5, aBottom, 5);
    }

    private void OnEvalJavaScriptFinished(UniWebView webView, string result)
    {
        Debug.Log("js result: " + result);
        this.tipTextMesh.text = "<color=#000000>" + result + "</color>";
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, (float) (Screen.height - 150), 150f, 80f), "Open"))
        {
            this._webView = base.GetComponent<UniWebView>();
            if (this._webView == null)
            {
                this._webView = base.gameObject.AddComponent<UniWebView>();
                this._webView.OnReceivedMessage += new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
                this._webView.OnLoadComplete += new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
                this._webView.OnWebViewShouldClose += new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
                this._webView.OnEvalJavaScriptFinished += new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
                this._webView.InsetsForScreenOreitation += new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
            }
            this._webView.url = "http://uniwebview.onevcat.com/demo/index1-1.html";
            this._webView.Load();
            this._errorMessage = null;
        }
        if ((this._webView != null) && GUI.Button(new Rect(150f, (float) (Screen.height - 150), 150f, 80f), "Back"))
        {
            this._webView.GoBack();
        }
        if ((this._webView != null) && GUI.Button(new Rect(300f, (float) (Screen.height - 150), 150f, 80f), "ToolBar"))
        {
            if (this._webView.toolBarShow)
            {
                this._webView.HideToolBar(true);
            }
            else
            {
                this._webView.ShowToolBar(true);
            }
        }
        if (this._errorMessage != null)
        {
            GUI.Label(new Rect(0f, 0f, (float) Screen.width, 80f), this._errorMessage);
        }
    }

    private void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            webView.Show();
        }
        else
        {
            Debug.Log("Something wrong in webview loading: " + errorMessage);
            this._errorMessage = errorMessage;
        }
    }

    private void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log("Received a message from native");
        Debug.Log(message.rawMessage);
        if (string.Equals(message.path, "move"))
        {
            Vector3 zero = Vector3.zero;
            if (string.Equals(message.args["direction"], "up"))
            {
                zero = new Vector3(0f, 0f, 1f);
            }
            else if (string.Equals(message.args["direction"], "down"))
            {
                zero = new Vector3(0f, 0f, -1f);
            }
            else if (string.Equals(message.args["direction"], "left"))
            {
                zero = new Vector3(-1f, 0f, 0f);
            }
            else if (string.Equals(message.args["direction"], "right"))
            {
                zero = new Vector3(1f, 0f, 0f);
            }
            int result = 0;
            if (int.TryParse(message.args["distance"], out result))
            {
                zero *= result;
            }
            this._moveVector = zero;
        }
        else if (string.Equals(message.path, "add"))
        {
            if (this._cube != null)
            {
                UnityEngine.Object.Destroy(this._cube);
            }
            this._cube = UnityEngine.Object.Instantiate<GameObject>(this.cubePrefab);
            this._cube.GetComponent<UniWebViewCube>().webViewDemo = this;
            this._moveVector = Vector3.zero;
        }
        else if (string.Equals(message.path, "close"))
        {
            webView.Hide();
            UnityEngine.Object.Destroy(webView);
            webView.OnReceivedMessage -= new UniWebView.ReceivedMessageDelegate(this.OnReceivedMessage);
            webView.OnLoadComplete -= new UniWebView.LoadCompleteDelegate(this.OnLoadComplete);
            webView.OnWebViewShouldClose -= new UniWebView.WebViewShouldCloseDelegate(this.OnWebViewShouldClose);
            webView.OnEvalJavaScriptFinished -= new UniWebView.EvalJavaScriptFinishedDelegate(this.OnEvalJavaScriptFinished);
            webView.InsetsForScreenOreitation -= new UniWebView.InsetsForScreenOreitationDelegate(this.InsetsForScreenOreitation);
            this._webView = null;
        }
    }

    private bool OnWebViewShouldClose(UniWebView webView)
    {
        if (webView == this._webView)
        {
            this._webView = null;
            return true;
        }
        return false;
    }

    public void ShowAlertInWebview(float time, bool first)
    {
        this._moveVector = Vector3.zero;
        if (first)
        {
            this._webView.EvaluatingJavaScript("sample(" + time + ")");
        }
    }

    private void Start()
    {
        this._cube = UnityEngine.Object.Instantiate<GameObject>(this.cubePrefab);
        this._cube.GetComponent<UniWebViewCube>().webViewDemo = this;
        this._moveVector = Vector3.zero;
    }

    private void Update()
    {
        if (this._cube != null)
        {
            if (this._cube.transform.position.y < -5f)
            {
                UnityEngine.Object.Destroy(this._cube);
                this._cube = null;
            }
            else
            {
                this._cube.transform.Translate((Vector3) (this._moveVector * Time.deltaTime));
            }
        }
    }
}

