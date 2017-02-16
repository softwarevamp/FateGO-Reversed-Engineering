using System;
using UnityEngine;

public class BLGameSdkDelegate : MonoBehaviour
{
    public static string ACCESS_TOKEN;

    public void callbackTopGameData(string res)
    {
    }

    public void didGetUserInfoFailureWithError()
    {
    }

    public void didGetUserInfoSuccessWithBLAuth()
    {
    }

    public void didLoginFailureWithError(string code)
    {
        if (int.Parse(code) == 1)
        {
            GameObject.Find("TitleScene").GetComponent<TitleRootComponent>().SetBoxCLient(true);
        }
    }

    public void didLoginSuccessWithAccessKey(string accessKey)
    {
        ACCESS_TOKEN = accessKey;
        DataManager.isLogin = true;
        LoginToMemberCenterRequest request = NetworkManager.getRequest<LoginToMemberCenterRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopGameData));
        request.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
        request.addField("t", "22360");
        request.addField("v", "1.0.1");
        request.addField("version", SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion());
        request.addField("s", "1");
        request.addField("mac", "00000000000000E0");
        request.addField("os", SystemInfo.operatingSystem);
        request.addField("ptype", SystemInfo.deviceModel);
        request.addField("imei", "aaaaa");
        request.addField("username", "username");
        request.addField("type", "token");
        request.addField("rkuid", "1");
        request.addField("access_token", ACCESS_TOKEN);
        request.addField("rksdkid", "1");
        request.beginRequest();
    }

    public void didLogout()
    {
        Application.Quit();
    }

    public void didPayFailureWithPayment()
    {
        SingletonMonoBehaviour<AccountingManager>.Instance.ReciveData("fail");
    }

    public void didPaySuccessWithPayment(string payment)
    {
        char[] separator = new char[] { ',' };
        string[] strArray = payment.Split(separator);
        string str = strArray[0];
        string str2 = strArray[1];
        NetworkManager.getRequest<PayIOSSuccessRequest>(new NetworkManager.ResultCallbackFunc(SingletonMonoBehaviour<AccountingManager>.Instance.ReciveData)).beginRequest(str, str2);
    }
}

