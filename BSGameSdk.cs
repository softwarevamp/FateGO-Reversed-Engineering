using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BSGameSdk : MonoBehaviour
{
    private const string SDK_JAVA_CLASS = "com.bsgamesdk.android.BSGameSdkCenter";

    public static void callSdkApi(string apiName, params object[] args)
    {
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.bsgamesdk.android.BSGameSdkCenter"))
        {
            class2.CallStatic(apiName, args);
        }
    }

    public static void callSdkApiObject(string apiName, params object[] args)
    {
        <callSdkApiObject>c__AnonStorey47 storey = new <callSdkApiObject>c__AnonStorey47 {
            apiName = apiName,
            args = args
        };
        storey.jo = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        object[] objArray1 = new object[] { new AndroidJavaRunnable(storey.<>m__2) };
        storey.jo.Call("runOnUiThread", objArray1);
    }

    public static bool callSdkApiObjectWithCallBack(string apiName, params object[] args)
    {
        AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        return @static.Call<bool>(apiName, args);
    }

    public static void checkLogin()
    {
        callSdkApi("checkLogin", new object[0]);
    }

    public static void createRole(string role, string role_id)
    {
        object[] args = new object[] { role, role_id };
        callSdkApi("createRole", args);
    }

    public static void getUserInfo()
    {
        callSdkApi("getUserInfo", new object[0]);
    }

    public static void init(bool debug, string merchant_id, string app_id, string server_id, string app_key)
    {
        object[] args = new object[] { debug, merchant_id, app_id, server_id, app_key };
        callSdkApi("init", args);
    }

    public static void login()
    {
        callSdkApi("login", new object[0]);
    }

    public static void logout()
    {
        callSdkApi("logout", new object[0]);
    }

    public static void notifyZone(string server_id, string server_name, string role_id, string role_name)
    {
        object[] args = new object[] { server_id, server_name, role_id, role_name };
        callSdkApi("notifyZone", args);
    }

    public static void pay(int uid, string username, string role, string serverId, int total_fee, int game_money, string out_trade_no, string subject, string body, string extension_info)
    {
        object[] args = new object[] { uid, username, role, serverId, total_fee, game_money, out_trade_no, subject, body, extension_info };
        callSdkApi("pay", args);
    }

    public static void PayUo(int money, string productName, int productCount, string tradeNo, string subject, string extInfo)
    {
        object[] args = new object[] { money, productName, productCount, tradeNo, subject, extInfo };
        callSdkApiObject("UoPay", args);
    }

    public static void register()
    {
        callSdkApi("register", new object[0]);
    }

    public static void showToast(string content)
    {
    }

    public static void UoChooseServer(string server_id, string server_name, int lv, string role_id, string role_name, string createtime)
    {
        object[] args = new object[] { role_id, role_name, lv, server_id, server_name, createtime };
        callSdkApiObject("UoChooseServer", args);
    }

    public static void UoCreateRole(string server_id, string server_name, int lv, string role_id, string role_name, string time)
    {
        object[] args = new object[] { role_id, role_name, lv, server_id, server_name, time };
        callSdkApiObject("UoCreateRole", args);
    }

    public static void UODebug(string msg)
    {
        object[] args = new object[] { msg };
        callSdkApiObject("DebugLog", args);
    }

    public static void UoExit()
    {
        callSdkApiObject("UoExit", new object[0]);
    }

    public static void UoInit()
    {
        callSdkApiObject("UoInit", new object[0]);
    }

    public static bool UOIsLogin() => 
        callSdkApiObjectWithCallBack("CheckIsLogin", new object[0]);

    public static void UoLevelUp(string server_id, string server_name, int lv, string role_id, string role_name, string createtime, string levelupTime)
    {
        object[] args = new object[] { role_id, role_name, lv, server_id, server_name, createtime, levelupTime };
        callSdkApiObject("UoLevelUp", args);
    }

    public static void UoLogin()
    {
        callSdkApiObject("UoLogin", new object[0]);
    }

    public static void UoLogout()
    {
        callSdkApiObject("UoLogout", new object[0]);
    }

    public static void xcodeLog(string str)
    {
    }

    public static bool YYBCheckIsLogin()
    {
        bool flag = callSdkApiObjectWithCallBack("CheckIsLogin", new object[0]);
        UODebug("YYBCheckIsLogin" + flag);
        return flag;
    }

    public static void YYBLogin(int type)
    {
        object[] args = new object[] { type };
        callSdkApiObject("YYBlogin", args);
    }

    [CompilerGenerated]
    private sealed class <callSdkApiObject>c__AnonStorey47
    {
        internal string apiName;
        internal object[] args;
        internal AndroidJavaObject jo;

        internal void <>m__2()
        {
            this.jo.Call(this.apiName, this.args);
        }
    }
}

