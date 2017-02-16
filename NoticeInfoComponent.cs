using System;
using UnityEngine;

public class NoticeInfoComponent : MonoBehaviour
{
    public string dynamicPath;
    public static bool messageIsLabel;
    public GameObject mGameHelpObject;
    public PlayMakerFSM myRoomFsm;
    public string path;
    public static string TermID = "2";
    public string title;

    private void onEndWebView()
    {
        messageIsLabel = false;
        this.myRoomFsm.SendEvent("CLOSE_WEBVIEW");
    }

    public void OnGameHelpPanelClose()
    {
        this.mGameHelpObject.SetActive(false);
        this.myRoomFsm.SendEvent("CLOSE_NOTICE");
    }

    public void openWebView()
    {
        if (string.IsNullOrEmpty(this.dynamicPath))
        {
            WebViewManager.OpenView(this.title, this.path, new System.Action(this.onEndWebView));
        }
        else
        {
            WebViewManager.OpenViewDynamic(this.title, this.dynamicPath, new System.Action(this.onEndWebView));
        }
    }

    public void setCreditWebViewInfo()
    {
        this.title = LocalizationManager.Get("WEB_VIEW_TITLE_CREDIT");
        this.path = "credit/index.html";
        this.dynamicPath = string.Empty;
        TermID = "3";
    }

    public void setHelpWebViewInfo()
    {
        this.mGameHelpObject.SetActive(true);
    }

    public void setInfomationWebViewInfo()
    {
        this.title = LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION");
        if (!ManagerConfig.UseWebViewAuthoring)
        {
            this.path = string.Empty;
            this.dynamicPath = "InformationTop";
        }
        else
        {
            this.path = "index.html";
            this.dynamicPath = string.Empty;
        }
        TermID = "6";
    }

    public void setInquiryWebViewInfo()
    {
        this.title = LocalizationManager.Get("WEB_VIEW_TITLE_CONTACT_US");
        this.path = string.Empty;
        this.dynamicPath = "InquiryTop";
        TermID = "1";
    }

    public void setNoticeInfo()
    {
        base.gameObject.SetActive(true);
        this.title = string.Empty;
        this.path = string.Empty;
        this.dynamicPath = string.Empty;
    }

    public void setRightWebViewInfo()
    {
        this.title = LocalizationManager.Get("WEB_VIEW_TITLE_RIGHT_NOTATION");
        this.path = "license/index.html";
        this.dynamicPath = string.Empty;
        TermID = "4";
    }

    public void setRulesWebViewInfo()
    {
        this.title = LocalizationManager.Get("WEB_VIEW_TITLE_TERMS_OF_USE");
        this.path = "userpolicy/index.html";
        this.dynamicPath = string.Empty;
        TermID = "2";
    }

    private void Start()
    {
        base.gameObject.transform.localPosition = new Vector3(1200f, 0f, 0f);
    }
}

