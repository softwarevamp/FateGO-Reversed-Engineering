using System;

public class UserFormationRequest : RequestBase
{
    public void beginRequest(long userEquipId)
    {
        Debug.Log("*-* beginRequest userEquipId: " + userEquipId);
        base.addActionField("userformationset");
        base.addField("userEquipId", userEquipId);
        base.beginRequest();
    }

    public override string getMockData() => 
        NetworkManager.getMockFile(string.Empty);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_REJECT, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

