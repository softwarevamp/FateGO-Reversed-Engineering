using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UserNameChangeRequest : RequestBase
{
    private string resMsg = "MockChangeUserNameResponse";

    public void beginRequest(string changeName, int changeType, int isCreateRole = 0)
    {
        base.addActionField("profileeditname");
        base.addField("name", changeName);
        base.addField("genderType", changeType);
        base.addField("isCreateRole", isCreateRole);
        base.beginRequest();
    }

    [DllImport("__Internal")]
    public static extern void createrole(string r, string rid);
    public override string getMockData() => 
        NetworkManager.getMockFile(this.resMsg);

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CHANGE_USERNAME, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            int num = int.Parse(success["isCreateRole"].ToString());
            string str = success["name"].ToString();
            PlayerPrefs.SetString("nickname", str);
            if (num == 1)
            {
                BSGameSdk.createRole(str, NetworkManager.UserId.ToString());
            }
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

