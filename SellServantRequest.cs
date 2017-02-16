using System;
using System.Collections.Generic;

public class SellServantRequest : RequestBase
{
    public void beginRequest(long[] ids)
    {
        Debug.Log("SellServantRequest:beginRequest [" + ids + "] ");
        string str = string.Empty;
        for (int i = 0; i < ids.Length; i++)
        {
            if (i > 0)
            {
                str = str + ",";
            }
            string str2 = str;
            object[] objArray1 = new object[] { str2, "{\"id\":", ids[i], ",\"num\":1}" };
            str = string.Concat(objArray1);
        }
        base.addActionField("shopsellsvt");
        base.addField("sellData", "[" + str + "]");
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SELL_SERVANT, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                base.completed(JsonManager.toJson(success));
                return;
            }
        }
        base.completed("ng");
    }
}

