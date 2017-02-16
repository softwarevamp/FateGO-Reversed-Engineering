using System;

public class TestRequest : RequestBase
{
    public void beginRequest(string address)
    {
        base.addField("address", address);
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        "https://www.fate-go.jp/api/preregist/entry";

    public override void requestCompleted(ResponseData[] responseList)
    {
        base.completed("ok");
    }
}

