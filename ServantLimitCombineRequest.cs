using System;

public class ServantLimitCombineRequest : RequestBase
{
    public void beginRequest(long baseUsrSvtId)
    {
        base.addActionField("cardcombinelimit");
        base.addField("baseUserSvtId", baseUsrSvtId);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CARD_COMBINE_LIMIT, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

