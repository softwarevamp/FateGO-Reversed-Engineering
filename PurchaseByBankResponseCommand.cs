using System;
using System.Collections.Generic;

public class PurchaseByBankResponseCommand : ResponseCommandBase
{
    public override ResponseCommandBase.Result ExecuteResponse(ResponseData data)
    {
        Dictionary<string, object> success = data.success;
        if ((success != null) && success.ContainsKey("result"))
        {
            Debug.Log("PurchaseByBankResponseCommand:result " + success["result"].ToString());
        }
        return ResponseCommandBase.Result.SUCCESS;
    }

    public override ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.PURCHASE_BY_BANK;
}

