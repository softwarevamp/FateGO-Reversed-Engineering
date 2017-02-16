using System;
using System.Collections.Generic;

public class ServantCombineRequest : RequestBase
{
    public void beginRequest(long baseUsrSvtId, string materialSvtIds, int useQp, int getExp)
    {
        base.addActionField("cardcombine");
        base.addField("baseUserSvtId", baseUsrSvtId);
        base.addField("materialUserSvtIds", materialSvtIds);
        base.addField("useQp", useQp);
        base.addField("getExp", getExp);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockSvtCombineResponse");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.COMBINE_SERVANT, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                string result = success["successResult"].ToString();
                base.completed(result);
                return;
            }
        }
        base.completed("ng");
    }
}

