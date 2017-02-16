using System;
using System.Collections.Generic;

public class PayIOSSuccessRequest : RequestBase
{
    private BankShopEntity bankData;

    public void beginRequest(string out_order_no, string bs_order_no)
    {
        Debug.Log("*-* PayIOSOrderRequest");
        base.addActionField("payiossuccess");
        base.addField("cp_order_no", out_order_no);
        base.addField("order_no", bs_order_no);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PAYIOSSUCCESS, responseList);
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

