using System;
using System.Collections.Generic;

public class PayIOSOrderRequest : RequestBase
{
    private BankShopEntity bankData;
    public static string orderId;

    public void beginRequest(BankShopEntity bankShop)
    {
        this.bankData = bankShop;
        Debug.Log("*-* PayIOSOrderRequest");
        base.addActionField("payiosorder");
        base.addField("bankShopId", this.bankData.id);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PAYIOSORDER, responseList);
        if ((data != null) && data.checkError())
        {
            int firstStoneNum;
            Dictionary<string, object> success = data.success;
            Debug.Log("payrequestCompleted");
            if (this.bankData.m_bfirst)
            {
                firstStoneNum = this.bankData.GetFirstStoneNum();
            }
            else
            {
                firstStoneNum = this.bankData.GetStoneNum();
            }
        }
        else
        {
            base.completed("ng");
        }
    }
}

