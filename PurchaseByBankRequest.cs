using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseByBankRequest : RequestBase
{
    public void beginRequest(string purchaseBaseData, string purchaseData)
    {
        Debug.Log("PurchaseByBankRequest:beginRequest [" + purchaseData + "]");
        base.addField("purchaseData", purchaseData);
        if (!string.IsNullOrEmpty(purchaseBaseData))
        {
            base.addField("purchaseDataBase64", purchaseBaseData);
        }
        base.addBaseField();
        base.WriteParameter();
        NetworkManager.RequestStart(this);
    }

    protected override string getParameterFileName() => 
        (Application.persistentDataPath + "/purchasebybankrequestsave.dat");

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "shop/purchaseByBank");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.PURCHASE_BY_BANK, responseList);
        if (data != null)
        {
            if (data.checkError())
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
            else
            {
                Dictionary<string, object> fail = data.fail;
                if (fail != null)
                {
                    string str2 = "ng";
                    if (fail.ContainsKey("result"))
                    {
                        str2 = fail["result"].ToString();
                    }
                    base.completed(str2);
                    return;
                }
            }
        }
        base.completed("ng");
    }
}

