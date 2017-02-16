using System;

public class ClientApiOutputLogRequest : RequestBase
{
    public void beginRequest(string title, string message, int code)
    {
        base.addField("title", title);
        base.addField("msg", message);
        base.addField("code", code);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "ClientApi/OutputLog");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CODE_INPUT, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

