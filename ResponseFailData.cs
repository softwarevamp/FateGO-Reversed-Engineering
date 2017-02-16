using System;
using System.Collections.Generic;

public class ResponseFailData
{
    public string action;
    public string csId;
    public string detail;
    public string nid;
    public string resCode;
    public string sandboxAssetsDomain;
    public string sandboxDomain;
    public bool sandboxSeurity;
    public string sandboxWebviewDomain;
    public string title;
    public string url;

    public ResponseFailData(ResponseData data)
    {
        this.Init(data.nid, data.resCode, data.fail);
    }

    public ResponseFailData(string nid, string resCode, Dictionary<string, object> failList)
    {
        this.Init(nid, resCode, failList);
    }

    public ResponseFailData(string nid, string resCode, string failList)
    {
        this.Init(nid, resCode, JsonManager.getDictionary(failList));
    }

    protected void Init(string nid, string resCode, Dictionary<string, object> failList)
    {
        this.nid = nid;
        this.resCode = resCode;
        if (failList.ContainsKey("csId"))
        {
            this.csId = failList["csId"].ToString();
        }
        else
        {
            this.csId = null;
        }
        if (failList.ContainsKey("action"))
        {
            this.action = failList["action"].ToString();
        }
        else
        {
            this.action = null;
        }
        if (failList.ContainsKey("title"))
        {
            this.title = failList["title"].ToString();
        }
        else
        {
            this.title = null;
        }
        if (failList.ContainsKey("detail"))
        {
            this.detail = failList["detail"].ToString();
        }
        else
        {
            this.detail = null;
        }
        if (failList.ContainsKey("url"))
        {
            this.url = failList["url"].ToString();
        }
        else
        {
            this.url = null;
        }
        if (failList.ContainsKey("sandboxSeurity"))
        {
            this.sandboxSeurity = bool.Parse(failList["sandboxSeurity"].ToString());
        }
        else
        {
            this.sandboxSeurity = false;
        }
        if (failList.ContainsKey("sandboxDomain"))
        {
            this.sandboxDomain = failList["sandboxDomain"].ToString();
        }
        else
        {
            this.sandboxDomain = null;
        }
        if (failList.ContainsKey("sandboxAssetsDomain"))
        {
            this.sandboxAssetsDomain = failList["sandboxAssetsDomain"].ToString();
        }
        else
        {
            this.sandboxAssetsDomain = null;
        }
        if (failList.ContainsKey("sandboxWebviewDomain"))
        {
            this.sandboxWebviewDomain = failList["sandboxWebviewDomain"].ToString();
        }
        else
        {
            this.sandboxWebviewDomain = null;
        }
    }
}

