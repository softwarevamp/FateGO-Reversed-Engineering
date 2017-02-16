using System;

public class TopHomeRequest : RequestBase
{
    protected static long accessTime;

    public override bool checkExpirationDate() => 
        (NetworkManager.getTime() <= accessTime);

    public static void clearExpirationDate()
    {
        accessTime = 0L;
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockTopLoginRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.HOME, responseList);
        if ((data != null) && data.checkError())
        {
            accessTime = NetworkManager.getTime() + BalanceConfig.RequestTopHomeExpirationDateSec;
            base.completed("ok");
        }
        else
        {
            accessTime = 0L;
            base.completed("ng");
        }
    }
}

