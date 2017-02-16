using System;

public class PurchaseByStoneRequest : RequestBase
{
    private int id;
    private int num;

    public void beginRequest(int id, int num)
    {
        Debug.Log(string.Concat(new object[] { "PurchaseByStoneRequest:beginRequest [", id, "] ", num }));
        this.id = id;
        this.num = num;
        base.addField("id", id);
        base.addField("num", num);
        base.addActionField("shoppurchasebystone");
        base.beginRequest();
        Debug.Log("    [lastAccessTime]:" + base.paramString["lastAccessTime"]);
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PURCHASE_BY_STONE, responseList);
        if ((data != null) && data.checkError())
        {
            string result = "ok";
            base.completed(result);
        }
        else
        {
            base.completed("ng");
        }
    }
}

