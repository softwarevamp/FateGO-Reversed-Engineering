using System;

public class FollowerSetupRequest : RequestBase
{
    public bool beginRequest(SupportServantData supportServantData)
    {
        string str = string.Empty;
        base.addActionField("followersetup");
        int num = 0;
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            string str2 = supportServantData.updateCheck(i);
            if (str2 != null)
            {
                if (num > 0)
                {
                    str = str + ",";
                }
                str = str + str2;
                num++;
            }
        }
        if (num == 0)
        {
            return false;
        }
        Debug.Log("DeckSetupFollowerSetupRequest : " + str + " [" + str + "]");
        base.addField("followerData", "[" + str + "]");
        base.beginRequest();
        return true;
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SUPPORT_SERVANT, responseList);
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

