using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TopLoginRequest : RequestBase
{
    protected static long accessTime;
    protected static long resetTime;

    public void beginRequest()
    {
        base.addField("assetbundleFolder", NetworkManager.GetDataServerFolderName());
        base.beginRequest();
    }

    public override bool checkExpirationDate()
    {
        long num = NetworkManager.getTime();
        return ((num <= resetTime) || (num <= accessTime));
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockTopLoginRequest");

    public override string getURL()
    {
        object[] objArray1 = new object[] { NetworkManager.getBaseUrl(true), "rgfate/60_", SingletonMonoBehaviour<DataManager>.Instance.serverId, "/login.php" };
        return string.Concat(objArray1);
    }

    [DllImport("__Internal")]
    public static extern void notifyzone(string serverID, string serverName, string roleID, string roleName);
    public override void requestCompleted(ResponseData[] responseList)
    {
        TopHomeRequest.clearExpirationDate();
        ResponseData data = responseList[0];
        if (data != null)
        {
            PlayerPrefs.SetString("usk", data.usk);
            if (data.checkError())
            {
                Dictionary<string, object> success = data.success;
                if (success != null)
                {
                    NetworkManager.userId = success["sguid"].ToString();
                    PlayerPrefs.SetString("sguid", success["sguid"].ToString());
                    string str = success["nickname"].ToString();
                    PlayerPrefs.SetString("nickname", str);
                    PlayerPrefs.SetString("announcement", JsonManager.toJson(data.success));
                    PlayerPrefs.SetString("sgtype", success["sgtype"].ToString());
                    PlayerPrefs.SetString("sgtag", success["sgtag"].ToString());
                    long num = NetworkManager.getNextDayTime(BalanceConfig.RequestTopLoginResetTime1);
                    long num2 = NetworkManager.getNextDayTime(BalanceConfig.RequestTopLoginResetTime2);
                    string str2 = success["platformManagement"].ToString();
                    string str3 = success["createTime"].ToString();
                    PlayerPrefs.SetString("createTime", str3);
                    int num3 = int.Parse(success["level"].ToString());
                    if (!string.IsNullOrEmpty(str2))
                    {
                        char[] separator = new char[] { ',' };
                        string[] strArray = str2.Split(separator);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            char[] chArray2 = new char[] { ':' };
                            string[] strArray2 = strArray[i].Split(chArray2);
                            int num5 = int.Parse(strArray2[0]);
                            int num6 = int.Parse(strArray2[1]);
                            NetworkManager.PlatformManagement[num5 - 1] = num6;
                        }
                    }
                    accessTime = (num > num2) ? num2 : num;
                    base.completed(success["type"].ToString());
                    BSGameSdk.notifyZone("248", "bili区", NetworkManager.userId, str);
                    return;
                }
            }
        }
        accessTime = 0L;
        base.completed("ng");
    }
}

