using System;

public class PurchaseRequest : RequestBase
{
    public void beginRequest(int id, int num)
    {
        Debug.Log(string.Concat(new object[] { "PurchaseRequest:beginRequest [", id, "] ", num }));
        base.addActionField("shoppurchase");
        base.addField("id", id);
        base.addField("num", num);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PURCHASE, responseList);
        if (((data != null) && data.checkError()) && (data.success != null))
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

