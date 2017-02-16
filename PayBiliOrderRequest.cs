using System;
using System.Collections.Generic;

public class PayBiliOrderRequest : RequestBase
{
    private BankShopEntity bankData;
    public static string orderId;

    public void beginRequest(BankShopEntity bankShop)
    {
        this.bankData = bankShop;
        Debug.LogError(this.bankData.GetStoneNum());
        Debug.Log("*-* PayBiliOrderRequest");
        base.addActionField("paybiliorder");
        base.addField("bankShopId", this.bankData.id);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PAYBILIORDER, responseList);
        if ((data != null) && data.checkError())
        {
            int firstStoneNum;
            Dictionary<string, object> success = data.success;
            if (this.bankData.m_bfirst)
            {
                firstStoneNum = this.bankData.GetFirstStoneNum();
            }
            else
            {
                firstStoneNum = this.bankData.GetStoneNum();
            }
            BSGameSdk.pay(int.Parse(BSCallbackListerner.User_id), BSCallbackListerner.User_name, NetworkManager.GetUserName(), "248", this.bankData.googlePrice, firstStoneNum, success["out_trade_no"].ToString(), this.bankData.name, this.bankData.priceDetail, "111");
        }
    }
}

