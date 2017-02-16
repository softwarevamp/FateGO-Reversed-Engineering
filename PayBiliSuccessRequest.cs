using System;
using System.Collections.Generic;

public class PayBiliSuccessRequest : RequestBase
{
    private BankShopEntity bankData;
    private string order_NO;

    public void beginRequest(string order_no, string cp_order_no)
    {
        Debug.Log("*-* PayBiliOrderRequest");
        base.addActionField("paybilisuccess");
        base.addField("order_no", order_no);
        base.addField("cp_order_no", cp_order_no);
        this.order_NO = order_no;
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PAYBILISUCCESS, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                string result = string.Empty;
                if (success.ContainsKey("result"))
                {
                    result = success["result"].ToString();
                }
                base.completed(result);
                return;
            }
        }
        base.completed("ng");
    }
}

