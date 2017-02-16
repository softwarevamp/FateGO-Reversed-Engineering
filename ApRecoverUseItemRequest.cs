using System;

public class ApRecoverUseItemRequest : RequestBase
{
    public void beginRequest(int itemId, int num)
    {
        base.addActionField("itemuse");
        base.addField("itemId", itemId);
        base.addField("num", num);
        base.addField("questId", 0);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.ITEM_USE, responseList);
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

