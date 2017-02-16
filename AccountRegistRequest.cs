using System;
using System.Collections.Generic;

public class AccountRegistRequest : RequestBase
{
    public override string getMockData() => 
        NetworkManager.getMockFile("MockAccountRegistRequest");

    public override string getURL() => 
        (NetworkManager.getBaseUrl(true) + "account/regist");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.ACCOUNT_REGIST, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                string userId = success["userId"].ToString();
                string authKey = success["authKey"].ToString();
                string secretKey = success["secretKey"].ToString();
                Debug.Log("auth " + userId + " " + authKey + " " + secretKey);
                SingletonMonoBehaviour<NetworkManager>.Instance.SetAuth(userId, authKey, secretKey);
                SingletonMonoBehaviour<NetworkManager>.Instance.WriteAuth();
                base.completed("ok");
                return;
            }
        }
        base.completed("ng");
    }
}

