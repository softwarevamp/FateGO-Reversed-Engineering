using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BSCallbackListerner : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache7;
    [CompilerGenerated]
    private static ErrorDialog.ClickDelegate <>f__am$cache8;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map0;
    public const string CALLBACKTYPE_AccountInvalid = "AccountInvalid";
    public const string CALLBACKTYPE_GetUserInfo = "GetUserInfo";
    public const string CALLBACKTYPE_Init = "Init";
    public const string CALLBACKTYPE_IsLogin = "IsLogin";
    public const string CALLBACKTYPE_Login = "Login";
    public const string CALLBACKTYPE_Logout = "Logout";
    public const string CALLBACKTYPE_Pay = "Pay";
    public const string CALLBACKTYPE_Register = "Register";
    public const string CALLBACKTYPE_UserCenter = "UserCenter";
    public bool IsPaySuccess;
    private bool isReqcallback;
    public string PayResultString = string.Empty;
    public const int StatusCode_Error = 300;
    public const int StatusCode_Failed = 200;
    public const int StatusCode_Success = 0x271a;
    public static string User_id = string.Empty;
    public static string User_name = string.Empty;
    public static string User_role = string.Empty;

    public void callbackTopGameData(string res)
    {
    }

    [DebuggerHidden]
    private IEnumerator CheckIsPaySuccess(string bs_trade_no, string out_trade_no) => 
        new <CheckIsPaySuccess>c__Iterator0 { <>f__this = this };

    private void EndReboot(bool isDecide)
    {
        NetworkManager.isRebootBlock = true;
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = () => SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, <>f__am$cache7);
    }

    public void log(string str)
    {
    }

    public void OnAccountInvalidCallback(int code, string data)
    {
        this.log("OnAccountInvalidCallback" + data);
        if (code == 0x271a)
        {
            BSGameSdk.showToast("OnAccountInvalidCallbackSuccess " + data);
        }
    }

    public void OnBSGameSdkCallback(string jsonstr)
    {
        this.log("OnBSGameSdkCallback message: jsonstr=" + jsonstr);
        JsonData data = JsonMapper.ToObject(jsonstr);
        string str = (string) data["callbackType"];
        int code = (int) data["code"];
        JsonData data2 = data["data"];
        string key = str;
        if (key != null)
        {
            int num2;
            if (<>f__switch$map0 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(8) {
                    { 
                        "Login",
                        0
                    },
                    { 
                        "IsLogin",
                        1
                    },
                    { 
                        "Logout",
                        2
                    },
                    { 
                        "GetUserInfo",
                        3
                    },
                    { 
                        "Register",
                        4
                    },
                    { 
                        "Pay",
                        5
                    },
                    { 
                        "Init",
                        6
                    },
                    { 
                        "AccountInvalid",
                        7
                    }
                };
                <>f__switch$map0 = dictionary;
            }
            if (<>f__switch$map0.TryGetValue(key, out num2))
            {
                switch (num2)
                {
                    case 0:
                        this.OnLoginCallback(code, (string) data2);
                        return;

                    case 1:
                        this.OnIsLoginCallback(code, data2);
                        return;

                    case 2:
                        this.OnLogoutCallback(code, (string) data2);
                        return;

                    case 3:
                        this.OnGetUserInfoCallback(code, (string) data2);
                        return;

                    case 4:
                        this.OnRegisterCallback(code, (string) data2);
                        return;

                    case 5:
                        this.OnPayCallback(code, (string) data2);
                        return;

                    case 6:
                        this.OnInitCallback(code, (string) data2);
                        return;

                    case 7:
                        this.OnAccountInvalidCallback(code, (string) data2);
                        return;
                }
            }
        }
    }

    public void OnGetUserInfoCallback(int code, string data)
    {
        this.log("OnGetUserInfoCallback" + data);
        if (code == 0x271a)
        {
            JsonData data2 = JsonMapper.ToObject(data);
            string str = (string) data2["uid"];
            int num = Convert.ToInt32(str);
            string str2 = (string) data2["username"];
            string str3 = (string) data2["lastLoginTime"];
            string str4 = (string) data2["avatar"];
            string str5 = (string) data2["s_avatar"];
            BSGameSdk.showToast("OnGetUserInfoCallbackSuccess  username: " + str2 + " lastLoginTime: " + str3 + " avatar: " + str4 + " s_avatar:" + str5);
        }
        else
        {
            BSGameSdk.showToast(string.Concat(new object[] { "OnGetUserInfoCallbackError  code: ", code, " message: +", data }));
        }
    }

    public virtual void OnInitCallback(int code, string data)
    {
        this.log("OnInitCallback" + data);
        if (code == 0x271a)
        {
            BSGameSdk.showToast("OnInitCallbackSuccess " + data);
            BSGameSdk.login();
        }
        else
        {
            BSGameSdk.showToast(string.Concat(new object[] { "OnInitCallbackError  code: ", code, " message: +", data }));
        }
    }

    public void OnIsLoginCallback(int code, JsonData data)
    {
        this.log("OnIsLoginCallback" + data);
        if (code == 0x271a)
        {
            BSGameSdk.showToast("OnIsLoginCallbackSuccess  isLogin: " + ((bool) data));
        }
        else
        {
            string str = (string) data;
            BSGameSdk.showToast(string.Concat(new object[] { "OnIsLoginCallbackError  code: ", code, " message: +", str }));
        }
    }

    public void OnLoginCallback(int code, string data)
    {
        this.log("OnLoginCallback" + data);
        if (code == 0x271a)
        {
            DataManager.isLogin = true;
            JsonData data2 = JsonMapper.ToObject(data);
            string str = (string) data2["username"];
            User_name = str;
            string str2 = (string) data2["uid"];
            User_id = str2;
            string str3 = (string) data2["nickname"];
            User_role = str3;
            string str4 = (string) data2["access_token"];
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
            request.addField("username", str);
            request.addField("type", "token");
            request.addField("rkuid", str2);
            request.addField("access_token", str4);
            request.addField("rksdkid", "1");
            request.addField("rkchannel", "24");
            request.beginRequest();
            BSGameSdk.showToast("OnLoginCallbackSuccess  uid: " + str2 + " username: " + str + " nickname " + str3);
        }
        else if (code == 0x271b)
        {
            NetworkManager.isRebootBlock = true;
            DataManager.isLogin = false;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, "检测到您的帐号已发生变更，点击\n重新开始游戏，谢谢！", new ErrorDialog.ClickDelegate(this.EndReboot), false);
        }
        else
        {
            DataManager.isLogin = false;
            BSGameSdk.showToast(string.Concat(new object[] { "OnLoginCallbackError  code: ", code, " message: +", data }));
        }
    }

    public void OnLogoutCallback(int code, string data)
    {
        this.log("OnLogoutCallback" + data);
        if (code != 0x271a)
        {
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate (bool i) {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, "帐号注销失败，请重试！", <>f__am$cache8, false);
        }
    }

    public void OnPayCallback(int code, string data)
    {
        this.log("OnPayCallback" + data);
        if (code == 0x271a)
        {
            JsonData data2 = JsonMapper.ToObject(data);
            string str = (string) data2["out_trade_no"];
            string str2 = (string) data2["bs_trade_no"];
            PayBiliOrderRequest.orderId = str;
            base.StartCoroutine(this.wait(str2, str));
            BSGameSdk.showToast("OnPayCallbackSuccess  out_trade_no: " + str + " bs_out_trade_no: " + str2);
        }
        else
        {
            JsonData data3 = JsonMapper.ToObject(data);
            string str3 = (string) data3["message"];
            string str4 = (string) data3["out_trade_no"];
            SingletonMonoBehaviour<AccountingManager>.Instance.OnCancelPaymentStore();
            BSGameSdk.showToast(string.Concat(new object[] { "OnPayCallbackError  code: ", code, "out_trade_no", str4, " message: +", str3 }));
        }
    }

    public void OnRegisterCallback(int code, string data)
    {
        this.log("OnRegisterCallback" + data);
        if (code == 0x271a)
        {
            BSGameSdk.showToast("OnRegisterCallbackSuccess  result: " + data);
        }
        else
        {
            BSGameSdk.showToast(string.Concat(new object[] { "OnRegisterCallbackError  code: ", code, " message: +", data }));
        }
    }

    public void PayResult(string result)
    {
        this.isReqcallback = true;
        this.PayResultString = result;
        MonoBehaviour.print("PayResult ::: " + this.PayResultString);
        if (result.Equals("fail"))
        {
            this.IsPaySuccess = false;
        }
        else
        {
            this.IsPaySuccess = true;
        }
    }

    [DebuggerHidden]
    private IEnumerator wait(string out_trade_no, string cp_order_no = "") => 
        new <wait>c__Iterator1 { 
            out_trade_no = out_trade_no,
            cp_order_no = cp_order_no,
            <$>out_trade_no = out_trade_no,
            <$>cp_order_no = cp_order_no
        };

    [CompilerGenerated]
    private sealed class <CheckIsPaySuccess>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BSCallbackListerner <>f__this;
        internal float <waitTime>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<waitTime>__0 = 0f;
                    this.<waitTime>__0 = Time.time + 80f;
                    this.<>f__this.IsPaySuccess = false;
                    break;

                case 1:
                    this.<>f__this.isReqcallback = false;
                    break;

                default:
                    goto Label_00BC;
            }
            if ((Time.time <= this.<waitTime>__0) && !this.<>f__this.IsPaySuccess)
            {
                this.$current = new WaitForSeconds(3f);
                this.$PC = 1;
                return true;
            }
            SingletonMonoBehaviour<AccountingManager>.Instance.ReciveData(this.<>f__this.PayResultString);
            goto Label_00BC;
            this.$PC = -1;
        Label_00BC:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <wait>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>cp_order_no;
        internal string <$>out_trade_no;
        internal PayBiliSuccessRequest <request>__0;
        internal string cp_order_no;
        internal string out_trade_no;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<request>__0 = NetworkManager.getRequest<PayBiliSuccessRequest>(new NetworkManager.ResultCallbackFunc(SingletonMonoBehaviour<AccountingManager>.Instance.ReciveData));
                    this.<request>__0.beginRequest(this.out_trade_no, this.cp_order_no);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

