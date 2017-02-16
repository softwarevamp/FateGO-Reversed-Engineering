using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class NetworkManager : SingletonMonoBehaviour<NetworkManager>
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map7;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map8;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map9;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapA;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapB;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapC;
    protected static string authKey = null;
    protected static readonly string AUTHMD5_KEY_TYPE = "TheCheckKey";
    protected string bate64ErrorString = string.Empty;
    protected RequestBase cacheRefreshWork;
    protected ResponseData[] cacheRefreshWorkResponseList;
    protected ResponseCommandBase[] commandList;
    protected Dictionary<string, ResponseCommandBase> commandLookup;
    protected Queue<RequestBase> communicationWaitList = new Queue<RequestBase>();
    protected RequestBase communicationWork;
    protected ResponseData[] communicationWorkResponseList;
    protected static string dataServerAddress = string.Empty;
    protected static string dataServerFolder = string.Empty;
    protected static string dataServerRedirectAddress = null;
    protected static string dataServerSettingAddress = string.Empty;
    protected static int day = 0;
    protected static DateTime dtUnix_8_Epoch = new DateTime(0x7b2, 1, 1, 8, 0, 0, DateTimeKind.Utc);
    protected static DateTime dtUnixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    protected System.Action errorCallbackFunc;
    protected static string failDataUrl = string.Empty;
    protected static string friendCode = null;
    public static string gameServerAddress = string.Empty;
    protected static string gameServerRedirectAddress = null;
    protected static string gameServerSettingAddress = string.Empty;
    protected static int genderType = 0;
    protected bool isBate64Retry;
    protected static bool isErrorServerTimeLimitOver = false;
    protected static bool isLogin = false;
    public static bool isRebootBlock = true;
    protected bool isWaitDebugDialog;
    protected LoginCallbackFunc loginCallbackFunc;
    protected string loginResponseResult;
    [HideInInspector, SerializeField]
    protected string mAndroidApiKey = string.Empty;
    [SerializeField, HideInInspector]
    protected string mAndroidProjectId = string.Empty;
    [HideInInspector, SerializeField]
    protected string mCv = string.Empty;
    [HideInInspector, SerializeField]
    protected string mIosApplicationId = string.Empty;
    [SerializeField, HideInInspector]
    protected string mMk = string.Empty;
    protected static int month = 0;
    public static int[] PlatformManagement = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    protected static string registrationId = null;
    protected static string registrationVersion = null;
    protected static string secretKey = null;
    protected static long serverOffsetTime = 0L;
    protected static bool serverRedirectSecurity = false;
    protected static bool serverSecurity = false;
    protected static bool serverSettingSecurity = false;
    protected static string serverSettingType = string.Empty;
    public static long serverTime = 0L;
    protected static string sessionId = null;
    protected StoreCallbackFunc storeCallbackFunc;
    public static string tryNewNetworkLineErrorCode = string.Empty;
    protected static string userAgent = null;
    protected static string userCreateServer = null;
    public static string userId = null;
    protected static string userName = null;
    protected static readonly long WebCacheTime = 300L;
    protected static string webServerAddress = string.Empty;
    protected static string webServerRedirectAddress = null;
    protected static string webServerSettingAddress = string.Empty;

    protected void AddWaitStatus(RequestBase request)
    {
        if (this.communicationWork == request)
        {
            Debug.LogError("AddWaitStatus already entry request error");
        }
        else
        {
            foreach (RequestBase base2 in this.communicationWaitList)
            {
                if (base2 == request)
                {
                    Debug.LogError("AddWaitStatus already entry request error");
                    return;
                }
            }
            this.communicationWaitList.Enqueue(request);
        }
    }

    public bool CheckServerLimitTime()
    {
        if (isErrorServerTimeLimitOver)
        {
            return false;
        }
        if (isLogin)
        {
            long num2 = getTime() - serverTime;
            Debug.Log(string.Concat(new object[] { "NetworkManager:ConnectElapsedTimeErrorCheck [", num2, "] limit ", BalanceConfig.ServerTimeOverLimit }));
            if ((num2 < 0L) || (num2 > BalanceConfig.ServerTimeOverLimit))
            {
                isErrorServerTimeLimitOver = true;
                isRebootBlock = true;
                this.ErrorDialog(string.Empty, LocalizationManager.Get("SYSTEM_ERROR_SERVER_TIME_LIMIT_OVER"), null, new System.Action(this.OnClickErrorReboot));
                return false;
            }
        }
        return true;
    }

    public static bool CheckUserCreateServer()
    {
        Debug.Log("CheckUserCreateServer [" + gameServerAddress + "] [" + userCreateServer + "]");
        return (string.IsNullOrEmpty(userCreateServer) || gameServerAddress.Equals(userCreateServer));
    }

    public void ClearAuth()
    {
        userId = null;
        userCreateServer = null;
    }

    public void ClearTopLoginResult()
    {
        this.loginResponseResult = null;
    }

    public static bool CommunicationIsBusy() => 
        (SingletonMonoBehaviour<NetworkManager>.Instance.communicationWork != null);

    protected bool CommunicationStart()
    {
        if (this.communicationWaitList.Count <= 0)
        {
            return false;
        }
        if (this.communicationWork != null)
        {
            return false;
        }
        this.communicationWork = this.communicationWaitList.Dequeue();
        base.StartCoroutine(this.RequestCR(this.communicationWork, 0f));
        return true;
    }

    public static void DeleteSaveData()
    {
        string path = getOldAuthFileName();
        string str2 = getAuthFileName(false);
        string str3 = getAuthFileName(true);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        if (File.Exists(str2))
        {
            File.Delete(str2);
        }
        if (File.Exists(str3))
        {
            File.Delete(str3);
        }
        DeleteSignupData();
    }

    public static void DeleteSignupData()
    {
        string path = getOldSignupFileName();
        string str2 = getSignupFileName();
        string str3 = getFriendCodeFileName();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        if (File.Exists(str2))
        {
            File.Delete(str2);
        }
        if (File.Exists(str3))
        {
            File.Delete(str3);
        }
    }

    protected void EndCloseUserDelete()
    {
        isRebootBlock = true;
        SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
    }

    protected void EndErrorDialog(bool isDecide)
    {
        this.EndErrorWebView();
    }

    protected void EndErrorWebView()
    {
        System.Action errorCallbackFunc = this.errorCallbackFunc;
        if (errorCallbackFunc != null)
        {
            this.errorCallbackFunc = null;
            errorCallbackFunc();
        }
    }

    protected void EndGetStoreUrl(string url)
    {
        if (url != null)
        {
            WebViewPluginScript.LaunchView(url);
        }
        Application.Quit();
    }

    protected void EndLogin()
    {
        LoginCallbackFunc loginCallbackFunc = this.loginCallbackFunc;
        string loginResponseResult = this.loginResponseResult;
        isLogin = true;
        if (loginCallbackFunc != null)
        {
            this.loginCallbackFunc = null;
            loginCallbackFunc(loginResponseResult);
        }
    }

    protected void EndTopLogin(string result)
    {
        this.loginResponseResult = result;
        this.EndLogin();
    }

    public void EntryService(ResultCallbackFunc callback)
    {
        Debug.Log("NetworkManager::EntryService");
        string result = null;
        if (ManagerConfig.UseAppServer)
        {
            result = NetworkServicePluginScript.EntryService(null);
        }
        Debug.Log("NetworkManager::EntryService result [" + result + "]");
        if (callback != null)
        {
            callback(result);
        }
    }

    protected void ErrorDialog(string errorTitle, string errorDetail, string errorUrl, System.Action callback)
    {
        this.errorCallbackFunc = callback;
        if (errorUrl != null)
        {
            WebViewManager.OpenView(errorTitle, errorUrl, new System.Action(this.EndErrorWebView));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(errorTitle, errorDetail, new ErrorDialog.ClickDelegate(this.EndErrorDialog), false);
        }
    }

    public static ResponseCommandBase.Result ExecuteCommand(ResponseData data)
    {
        ResponseCommandBase.Result sUCCESS;
        try
        {
            ResponseCommandBase base2 = SingletonMonoBehaviour<NetworkManager>.Instance.commandLookup[data.nid];
            Debug.Log("data.nid:" + data.nid);
            try
            {
                return base2.ExecuteResponse(data);
            }
            catch (Exception exception)
            {
                Debug.LogError("RequestCommand error [" + data.nid + "] " + exception.Message);
                return ResponseCommandBase.Result.ERROR;
            }
        }
        catch
        {
            sUCCESS = ResponseCommandBase.Result.SUCCESS;
        }
        return sUCCESS;
    }

    public static string getActionUrl(bool isSecurity = false)
    {
        object[] objArray1 = new object[] { gameServerAddress, "rgfate/60_", SingletonMonoBehaviour<DataManager>.Instance.serverId, "/ac.php" };
        return string.Concat(objArray1);
    }

    public static string GetApiCode()
    {
        string mAndroidApiKey = SingletonMonoBehaviour<NetworkManager>.Instance.mAndroidApiKey;
        return (!string.IsNullOrEmpty(mAndroidApiKey) ? CryptData.Decrypt(mAndroidApiKey, false) : string.Empty);
    }

    protected static string GetAuthCode(SortedDictionary<string, string> headers)
    {
        string str = string.Empty;
        foreach (KeyValuePair<string, string> pair in headers)
        {
            if (str != string.Empty)
            {
                string str2 = str;
                string[] textArray1 = new string[] { str2, "&", pair.Key, "=", pair.Value };
                str = string.Concat(textArray1);
            }
            else
            {
                str = str + pair.Key + "=" + pair.Value;
            }
        }
        Debug.Log("GetAuthCode : parameterString [" + str + "]");
        if (secretKey == null)
        {
            return null;
        }
        SHA1 sha = new SHA1CryptoServiceProvider();
        byte[] bytes = new UTF8Encoding().GetBytes(str + ":" + secretKey);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }

    protected static string getAuthFileName(bool isSlave)
    {
        if (isSlave)
        {
            return (Application.persistentDataPath + "/authsave2.dat");
        }
        return (Application.persistentDataPath + "/authsave.dat");
    }

    public static string getBaseMockUrl() => 
        gameServerAddress;

    public static string getBaseUrl(bool isSecurity = false) => 
        gameServerAddress;

    public static string GetCv()
    {
        string mCv = SingletonMonoBehaviour<NetworkManager>.Instance.mCv;
        return (!string.IsNullOrEmpty(mCv) ? CryptData.Decrypt(mCv, false) : string.Empty);
    }

    public static string GetDataServerFolderName() => 
        dataServerFolder;

    public static string GetDataServerSetting() => 
        dataServerSettingAddress;

    public static string GetDataServerSetting(string type)
    {
        if (type == "DEFAULT")
        {
            type = ManagerConfig.ServerDefaultType;
        }
        string key = type;
        if (key != null)
        {
            int num;
            if (<>f__switch$map9 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x17) {
                    { 
                        "RELEASE",
                        0
                    },
                    { 
                        "RELEASE_ANDROID_REVIEW",
                        0
                    },
                    { 
                        "RELEASE_IOS_REVIEW",
                        0
                    },
                    { 
                        "STAGING",
                        1
                    },
                    { 
                        "CLONE",
                        2
                    },
                    { 
                        "RELEASE_CLONE_GAME_SERVAER",
                        3
                    },
                    { 
                        "RELEASE_CLONE",
                        3
                    },
                    { 
                        "DEBUG",
                        4
                    },
                    { 
                        "INHOUSE",
                        4
                    },
                    { 
                        "WEBVIEW",
                        4
                    },
                    { 
                        "EVENTDEV",
                        5
                    },
                    { 
                        "DEVELOP",
                        6
                    },
                    { 
                        "GAME_SERVER",
                        6
                    },
                    { 
                        "QA1",
                        7
                    },
                    { 
                        "QA2",
                        8
                    },
                    { 
                        "QA3",
                        9
                    },
                    { 
                        "QA4",
                        10
                    },
                    { 
                        "DEV1",
                        11
                    },
                    { 
                        "DEV2",
                        12
                    },
                    { 
                        "DEV3",
                        13
                    },
                    { 
                        "DEV4",
                        14
                    },
                    { 
                        "SCRIPT",
                        15
                    },
                    { 
                        "CUSTOM",
                        0x10
                    }
                };
                <>f__switch$map9 = dictionary;
            }
            if (<>f__switch$map9.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return ManagerConfig.ReleaseDataServerAddress;

                    case 1:
                        return ManagerConfig.StagingDataServerAddress;

                    case 2:
                        return ManagerConfig.GameCloneDataServerAddress;

                    case 3:
                        return ManagerConfig.ReleaseCloneDataServerAddress;

                    case 4:
                        return ManagerConfig.DebugDataServerAddress;

                    case 5:
                        return ManagerConfig.EventDevDataServerAddress;

                    case 6:
                        return ManagerConfig.DevelopDataServerAddress;

                    case 7:
                        return ManagerConfig.Qa1DataServerAddress;

                    case 8:
                        return ManagerConfig.Qa2DataServerAddress;

                    case 9:
                        return ManagerConfig.Qa3DataServerAddress;

                    case 10:
                        return ManagerConfig.Qa4DataServerAddress;

                    case 11:
                        return ManagerConfig.Dev1DataServerAddress;

                    case 12:
                        return ManagerConfig.Dev2DataServerAddress;

                    case 13:
                        return ManagerConfig.Dev3DataServerAddress;

                    case 14:
                        return ManagerConfig.Dev4DataServerAddress;

                    case 15:
                        return ManagerConfig.ScriptDataServerAddress;

                    case 0x10:
                        return dataServerSettingAddress;
                }
            }
        }
        return string.Empty;
    }

    public static string getDataUrl() => 
        dataServerAddress;

    public static DateTime getDateTime() => 
        DateTime.Now.ToUniversalTime().AddSeconds((double) serverOffsetTime);

    public static DateTime getDateTime(long t) => 
        dtUnixEpoch.AddSeconds((double) t).ToUniversalTime();

    public string GetDispFriendCode()
    {
        if (friendCode != null)
        {
            return LocalizationManager.GetNumberFormat(friendCode);
        }
        return null;
    }

    public string GetFriendCode() => 
        friendCode;

    protected static string getFriendCodeFileName() => 
        (Application.persistentDataPath + "/friendcodesave.dat");

    public static string GetGameServerSetting() => 
        gameServerSettingAddress;

    public static string GetGameServerSetting(string type)
    {
        if (type == "DEFAULT")
        {
            type = ManagerConfig.ServerDefaultType;
        }
        string key = type;
        if (key != null)
        {
            int num;
            if (<>f__switch$map8 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x17) {
                    { 
                        "RELEASE",
                        0
                    },
                    { 
                        "RELEASE_ANDROID_REVIEW",
                        0
                    },
                    { 
                        "RELEASE_IOS_REVIEW",
                        0
                    },
                    { 
                        "STAGING",
                        1
                    },
                    { 
                        "CLONE",
                        2
                    },
                    { 
                        "RELEASE_CLONE_GAME_SERVAER",
                        3
                    },
                    { 
                        "RELEASE_CLONE",
                        3
                    },
                    { 
                        "DEBUG",
                        4
                    },
                    { 
                        "INHOUSE",
                        4
                    },
                    { 
                        "WEBVIEW",
                        4
                    },
                    { 
                        "EVENTDEV",
                        5
                    },
                    { 
                        "GAME_SERVER",
                        6
                    },
                    { 
                        "DEVELOP",
                        6
                    },
                    { 
                        "QA1",
                        7
                    },
                    { 
                        "QA2",
                        8
                    },
                    { 
                        "QA3",
                        9
                    },
                    { 
                        "QA4",
                        10
                    },
                    { 
                        "DEV1",
                        11
                    },
                    { 
                        "DEV2",
                        12
                    },
                    { 
                        "DEV3",
                        13
                    },
                    { 
                        "DEV4",
                        14
                    },
                    { 
                        "SCRIPT",
                        15
                    },
                    { 
                        "CUSTOM",
                        0x10
                    }
                };
                <>f__switch$map8 = dictionary;
            }
            if (<>f__switch$map8.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return ManagerConfig.ReleaseGameServerAddress;

                    case 1:
                        return ManagerConfig.StagingGameServerAddress;

                    case 2:
                        return ManagerConfig.GameCloneGameServerAddress;

                    case 3:
                        return ManagerConfig.ReleaseCloneGameServerAddress;

                    case 4:
                        return ManagerConfig.DebugGameServerAddress;

                    case 5:
                        return ManagerConfig.EventDevGameServerAddress;

                    case 6:
                        return ManagerConfig.DevelopGameServerAddress;

                    case 7:
                        return ManagerConfig.Qa1GameServerAddress;

                    case 8:
                        return ManagerConfig.Qa2GameServerAddress;

                    case 9:
                        return ManagerConfig.Qa3GameServerAddress;

                    case 10:
                        return ManagerConfig.Qa4GameServerAddress;

                    case 11:
                        return ManagerConfig.Dev1GameServerAddress;

                    case 12:
                        return ManagerConfig.Dev2GameServerAddress;

                    case 13:
                        return ManagerConfig.Dev3GameServerAddress;

                    case 14:
                        return ManagerConfig.Dev4GameServerAddress;

                    case 15:
                        return ManagerConfig.ScriptGameServerAddress;

                    case 0x10:
                        return gameServerSettingAddress;
                }
            }
        }
        return string.Empty;
    }

    public static DateTime getLocalDateTime() => 
        DateTime.Now.AddSeconds((double) serverOffsetTime);

    public static DateTime getLocalDateTime(long t) => 
        dtUnixEpoch.AddSeconds((double) t).ToLocalTime();

    public static long getLocalTime() => 
        getTime(getLocalDateTime());

    public static string GetMk()
    {
        string mMk = SingletonMonoBehaviour<NetworkManager>.Instance.mMk;
        return (!string.IsNullOrEmpty(mMk) ? CryptData.Decrypt(mMk, false) : string.Empty);
    }

    public static string getMockFile(string path)
    {
        if (path != null)
        {
            TextAsset assetToUnload = Resources.Load("Mock/" + path) as TextAsset;
            if (assetToUnload != null)
            {
                string text = assetToUnload.text;
                Resources.UnloadAsset(assetToUnload);
                return text;
            }
        }
        return null;
    }

    public static long getNextDayTime()
    {
        DateTime time = getLocalDateTime();
        int year = time.Year;
        int month = time.Month;
        int day = time.Day + 1;
        if (day > DateTime.DaysInMonth(year, month))
        {
            day = 1;
            if (month >= 12)
            {
                month = 1;
                year++;
            }
            else
            {
                month++;
            }
        }
        DateTime dateTime = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        return getTime(dateTime);
    }

    public static long getNextDayTime(int hour)
    {
        DateTime time = getLocalDateTime();
        long num = getTime();
        if (time.Hour >= hour)
        {
            num += 0x15180L;
        }
        return (num - (((((time.Hour - hour) * 60) + time.Minute) * 60) + time.Second));
    }

    public static long getNextMonthTime()
    {
        DateTime time = getLocalDateTime();
        int year = time.Year;
        int month = time.Month;
        if (month >= 12)
        {
            month = 1;
            year++;
        }
        else
        {
            month++;
        }
        DateTime dateTime = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        return getTime(dateTime);
    }

    public static long getNextMonthTime(int day)
    {
        DateTime now = DateTime.Now;
        int year = now.Year;
        int month = now.Month;
        if (now.Day < day)
        {
            if (month >= 12)
            {
                month = 1;
                year++;
            }
            else
            {
                month++;
            }
        }
        if (day > DateTime.DaysInMonth(year, month))
        {
            day = 1;
            if (month >= 12)
            {
                month = 1;
                year++;
            }
            else
            {
                month++;
            }
        }
        DateTime dateTime = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        return getTime(dateTime);
    }

    protected static string getOldAuthFileName() => 
        (Application.temporaryCachePath + "/authsave.dat");

    protected static string getOldSignupFileName() => 
        (Application.temporaryCachePath + "/signupsave.dat");

    public static T getRequest<T>(ResultCallbackFunc func) where T: RequestBase, new()
    {
        T local = Activator.CreateInstance<T>();
        T local1 = local;
        local1.CallBack = (ResultCallbackFunc) Delegate.Combine(local1.CallBack, func);
        return local;
    }

    public static bool GetSecurityServerSetting() => 
        serverSettingSecurity;

    public static bool GetSecurityServerSetting(string type)
    {
        if (type == "DEFAULT")
        {
            type = ManagerConfig.ServerDefaultType;
        }
        string key = type;
        if (key != null)
        {
            int num;
            if (<>f__switch$map7 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x16) {
                    { 
                        "RELEASE",
                        0
                    },
                    { 
                        "RELEASE_ANDROID_REVIEW",
                        0
                    },
                    { 
                        "RELEASE_IOS_REVIEW",
                        0
                    },
                    { 
                        "STAGING",
                        1
                    },
                    { 
                        "CLONE",
                        2
                    },
                    { 
                        "RELEASE_CLONE_GAME_SERVAER",
                        3
                    },
                    { 
                        "RELEASE_CLONE",
                        3
                    },
                    { 
                        "DEBUG",
                        4
                    },
                    { 
                        "INHOUSE",
                        4
                    },
                    { 
                        "WEBVIEW",
                        4
                    },
                    { 
                        "EVENTDEV",
                        5
                    },
                    { 
                        "GAME_SERVER",
                        6
                    },
                    { 
                        "DEVELOP",
                        6
                    },
                    { 
                        "QA1",
                        7
                    },
                    { 
                        "QA2",
                        7
                    },
                    { 
                        "QA3",
                        7
                    },
                    { 
                        "QA4",
                        7
                    },
                    { 
                        "DEV1",
                        8
                    },
                    { 
                        "DEV2",
                        8
                    },
                    { 
                        "DEV3",
                        8
                    },
                    { 
                        "DEV4",
                        8
                    },
                    { 
                        "CUSTOM",
                        9
                    }
                };
                <>f__switch$map7 = dictionary;
            }
            if (<>f__switch$map7.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return ManagerConfig.ReleaseNetworkSecurity;

                    case 1:
                        return ManagerConfig.StagingNetworkSecurity;

                    case 2:
                        return ManagerConfig.GameCloneNetworkSecurity;

                    case 3:
                        return ManagerConfig.ReleaseCloneNetworkSecurity;

                    case 4:
                        return ManagerConfig.DebugNetworkSecurity;

                    case 5:
                        return ManagerConfig.EventDevNetworkSecurity;

                    case 6:
                        return ManagerConfig.DevelopNetworkSecurity;

                    case 7:
                        return ManagerConfig.QaNetworkSecurity;

                    case 8:
                        return ManagerConfig.DevNetworkSecurity;

                    case 9:
                        return serverSettingSecurity;
                }
            }
        }
        return false;
    }

    protected static string getServerSettingFileName() => 
        (Application.persistentDataPath + "/serversave.dat");

    public static string GetServerSettingType() => 
        serverSettingType;

    public void GetSignup(out string userName, out int genderType, out int month, out int day)
    {
        userName = NetworkManager.userName;
        genderType = NetworkManager.genderType;
        month = NetworkManager.month;
        day = NetworkManager.day;
    }

    protected static string getSignupFileName() => 
        (Application.persistentDataPath + "/signupsave.dat");

    public static void getStoreUrl(string storeName, string id, StoreCallbackFunc callback)
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.GetStoreUrl(storeName, id, callback);
    }

    protected void GetStoreUrl(string storeName, string id, StoreCallbackFunc callback)
    {
        base.StartCoroutine(this.RequestApplicationUpdateCR(storeName, id, callback));
    }

    public static long getTime() => 
        getTime(getDateTime());

    public static long getTime(DateTime dateTime) => 
        ((long) dateTime.Subtract(dtUnixEpoch).TotalSeconds);

    public string GetTopLoginResult() => 
        this.loginResponseResult;

    public static string GetUserName() => 
        userName;

    public static DateTime getUtc_8DateTime(long t) => 
        dtUnix_8_Epoch.AddSeconds((double) t);

    public static string GetWebServerSetting() => 
        webServerSettingAddress;

    public static string GetWebServerSetting(string type)
    {
        if ((type == "DEFAULT") || (type == "CUSTOM"))
        {
            type = ManagerConfig.ServerDefaultType;
        }
        string key = type;
        if (key != null)
        {
            int num;
            if (<>f__switch$mapA == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x16) {
                    { 
                        "RELEASE",
                        0
                    },
                    { 
                        "RELEASE_ANDROID_REVIEW",
                        0
                    },
                    { 
                        "RELEASE_IOS_REVIEW",
                        0
                    },
                    { 
                        "STAGING",
                        1
                    },
                    { 
                        "CLONE",
                        2
                    },
                    { 
                        "RELEASE_CLONE_GAME_SERVAER",
                        3
                    },
                    { 
                        "RELEASE_CLONE",
                        3
                    },
                    { 
                        "DEBUG",
                        4
                    },
                    { 
                        "INHOUSE",
                        4
                    },
                    { 
                        "WEBVIEW",
                        4
                    },
                    { 
                        "EVENTDEV",
                        5
                    },
                    { 
                        "GAME_SERVER",
                        6
                    },
                    { 
                        "DEVELOP",
                        6
                    },
                    { 
                        "QA1",
                        7
                    },
                    { 
                        "QA2",
                        7
                    },
                    { 
                        "QA3",
                        7
                    },
                    { 
                        "QA4",
                        7
                    },
                    { 
                        "DEV1",
                        8
                    },
                    { 
                        "DEV2",
                        8
                    },
                    { 
                        "DEV3",
                        8
                    },
                    { 
                        "DEV4",
                        8
                    },
                    { 
                        "CUSTOM",
                        9
                    }
                };
                <>f__switch$mapA = dictionary;
            }
            if (<>f__switch$mapA.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return ManagerConfig.ReleaseWebServerAddress;

                    case 1:
                        return ManagerConfig.StagingWebServerAddress;

                    case 2:
                        return ManagerConfig.GameCloneWebServerAddress;

                    case 3:
                        return ManagerConfig.ReleaseCloneWebServerAddress;

                    case 4:
                        return ManagerConfig.DebugWebServerAddress;

                    case 5:
                        return ManagerConfig.EventDevWebServerAddress;

                    case 6:
                        return ManagerConfig.DevelopWebServerAddress;

                    case 7:
                        return ManagerConfig.QaWebServerAddress;

                    case 8:
                        return ManagerConfig.DevWebServerAddress;

                    case 9:
                        return webServerSettingAddress;
                }
            }
        }
        return string.Empty;
    }

    public static string getWebUrl(string path, bool isSecurity = false, bool isWebView = true)
    {
        string str = string.Empty;
        if (path == null)
        {
            str = ((!isSecurity || !serverSecurity) ? "http://" : "https://") + webServerAddress + (!isWebView ? string.Empty : "/webview/");
        }
        else if (path.StartsWith("file:"))
        {
            str = path;
        }
        else if (path.StartsWith("http:") || path.StartsWith("https:"))
        {
            str = path;
        }
        else
        {
            str = ((!isSecurity || !serverSecurity) ? "http://" : "https://") + webServerAddress + (!isWebView ? string.Empty : "/webview/") + path;
        }
        if (!isWebView || (!str.StartsWith("http:") && !str.StartsWith("https:")))
        {
            return str;
        }
        long num = getTime();
        long num2 = num / WebCacheTime;
        if (str.IndexOf(webServerAddress) >= 0)
        {
            if (str.IndexOf("?") >= 0)
            {
                return (str + "&v=" + num2);
            }
            return (str + "?v=" + num2);
        }
        if (str.IndexOf("?") >= 0)
        {
            if (str.IndexOf("lastAccessTime=") < 0)
            {
                str = str + "&lastAccessTime=" + num;
            }
            return str;
        }
        return (str + "?lastAccessTime=" + num);
    }

    public void Initialize()
    {
        this.ClearAuth();
        this.ReadServerSetting();
        if (this.commandList == null)
        {
            this.commandList = new List<ResponseCommandBase> { 
                new PurchaseByBankResponseCommand(),
                new MissionNotifyResponseCommand()
            }.ToArray();
            this.commandLookup = new Dictionary<string, ResponseCommandBase>();
            for (int i = 0; i < this.commandList.Length; i++)
            {
                ResponseCommandBase base2 = this.commandList[i];
                this.commandLookup.Add(base2.GetCommandName(), base2);
            }
        }
        userAgent = NetworkServicePluginScript.GetUserAgentString();
        if (ManagerConfig.UseAppServer)
        {
            NetworkServicePluginScript.CheckService();
        }
        this.communicationWaitList.Clear();
        this.communicationWork = null;
        this.communicationWorkResponseList = null;
        this.cacheRefreshWork = null;
        this.cacheRefreshWorkResponseList = null;
        isErrorServerTimeLimitOver = false;
        isRebootBlock = false;
        isLogin = false;
        sessionId = null;
        dataServerFolder = string.Empty;
        SingletonMonoBehaviour<DataManager>.Instance.Initialize();
        SingletonMonoBehaviour<NotificationManager>.Instance.Initialize();
    }

    public string LoadAuthMd5(string fname)
    {
        string str = null;
        if (File.Exists(fname))
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(fname));
            try
            {
                str = EncryptedPlayerPrefs.Md5(reader.ReadString());
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                str = null;
            }
            finally
            {
                if (reader != null)
                {
                    ((IDisposable) reader).Dispose();
                }
            }
        }
        return str;
    }

    protected void OnApplicationPause(bool isPause)
    {
        if (!isPause)
        {
            this.CheckServerLimitTime();
        }
    }

    protected void OnClickBate64ErrorRetryDialog(bool isDecide)
    {
        this.communicationWorkResponseList = null;
        if (isDecide)
        {
            this.isBate64Retry = true;
            Debug.LogError("bate64Test:" + this.bate64ErrorString);
            this.communicationWork.replaceField("try", this.bate64ErrorString);
            base.StartCoroutine(this.RequestCR(this.communicationWork, 1f));
        }
        else
        {
            Application.Quit();
        }
    }

    protected void OnClickErrorDialog(bool isDecide)
    {
        Application.Quit();
    }

    protected void OnClickErrorGotoLogin()
    {
        Debug.LogError("gotologin");
    }

    protected void OnClickErrorReboot()
    {
        if (ManagementManager.IsDuringStartup)
        {
            Application.Quit();
        }
        else
        {
            if (this.communicationWork is PurchaseByBankRequest)
            {
                AccountingManager.RecoverStatusReset();
            }
            isRebootBlock = true;
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void OnClickErrorRetryDialog(bool isDecide)
    {
        this.communicationWorkResponseList = null;
        if (isDecide)
        {
            base.StartCoroutine(this.RequestCR(this.communicationWork, 1f));
        }
        else if (ManagementManager.IsDuringStartup)
        {
            Application.Quit();
        }
        else
        {
            if (this.communicationWork is PurchaseByBankRequest)
            {
                AccountingManager.RecoverStatusReset();
            }
            isRebootBlock = true;
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void OnClickErrorStay()
    {
        this.OnEndCommunication(this.communicationWork, this.communicationWorkResponseList);
    }

    protected void OnClickErrorTerminal()
    {
        if (ManagementManager.IsDuringStartup)
        {
            Application.Quit();
        }
        else
        {
            if (this.communicationWork is PurchaseByBankRequest)
            {
                AccountingManager.RecoverStatusReset();
            }
            isRebootBlock = true;
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void OnClickReloadCache()
    {
        this.cacheRefreshWork = this.communicationWork;
        this.cacheRefreshWorkResponseList = this.communicationWorkResponseList;
        this.communicationWork = getRequest<TopRefreshRequest>(null);
        this.communicationWorkResponseList = null;
        this.communicationWork.addActionField("refreshtop");
        this.communicationWork.addBaseField();
        base.StartCoroutine(this.RequestCR(this.communicationWork, 1f));
    }

    protected void OnClickStore()
    {
        Debug.LogError("openUrl:" + failDataUrl);
        Application.OpenURL(failDataUrl);
        Application.Quit();
    }

    protected void OnClickUserDelete(bool isDecide)
    {
        if (isDecide)
        {
            UserSaveData.DeleteSaveData();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.EndCloseUserDelete));
    }

    protected void OnClickWaitDebugDialog(bool isDecide)
    {
        this.isWaitDebugDialog = false;
    }

    protected void OnEndCommunication(RequestBase requestWork, ResponseData[] responseList)
    {
        if (this.communicationWork == requestWork)
        {
            this.communicationWorkResponseList = null;
            this.communicationWork = null;
            if (this.cacheRefreshWork != null)
            {
                requestWork = this.cacheRefreshWork;
                responseList = this.cacheRefreshWorkResponseList;
                this.cacheRefreshWork = null;
                this.cacheRefreshWorkResponseList = null;
            }
        }
        requestWork.requestCompleted(responseList);
        base.Invoke("CommunicationStart", 0f);
    }

    public ReadResult ReadAuth()
    {
        if (!ManagerConfig.UseMock)
        {
            int num = 5;
            bool flag = false;
            ReadResult nONE = ReadResult.NONE;
            bool isSlave = false;
            int num2 = this.SyncAuth();
            if (num2 == 2)
            {
                return ReadResult.MD5_ERROR;
            }
            if (num2 != 0)
            {
                Debug.LogError("auth not found");
            }
            while (num-- > 0)
            {
                string path = getAuthFileName(isSlave);
                isSlave = !isSlave;
                Debug.Log("NetworkManager::read " + path);
                if (File.Exists(path))
                {
                    flag = true;
                    bool flag3 = false;
                    using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
                    {
                        string strToEncrypt = null;
                        string jsonstr = null;
                        try
                        {
                            strToEncrypt = reader.ReadString();
                            string defaultValue = EncryptedPlayerPrefs.Md5(strToEncrypt);
                            string str5 = PlayerPrefs.GetString(AUTHMD5_KEY_TYPE, defaultValue);
                            if (defaultValue != str5)
                            {
                                Debug.LogError("invalid md5");
                                strToEncrypt = null;
                                nONE = ReadResult.READ_ERROR;
                                continue;
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.LogError(exception.Message);
                            strToEncrypt = null;
                            nONE = ReadResult.READ_ERROR;
                        }
                        if (strToEncrypt == null)
                        {
                            continue;
                        }
                        try
                        {
                            jsonstr = CryptData.Decrypt(strToEncrypt, false);
                        }
                        catch (Exception exception2)
                        {
                            Debug.LogError(exception2.Message);
                            jsonstr = null;
                            nONE = ReadResult.CRYPT_ERROR;
                        }
                        if (jsonstr == null)
                        {
                            continue;
                        }
                        try
                        {
                            Dictionary<string, object> dictionary = JsonManager.getDictionary(jsonstr);
                            userCreateServer = dictionary["userCreateServer"].ToString();
                            userId = dictionary["userId"].ToString();
                            authKey = dictionary["authKey"].ToString();
                            secretKey = dictionary["secretKey"].ToString();
                            flag3 = true;
                        }
                        catch (Exception exception3)
                        {
                            Debug.LogError(exception3.Message);
                            nONE = ReadResult.JSON_ERROR;
                        }
                        if (!flag3)
                        {
                            continue;
                        }
                    }
                    if (flag3)
                    {
                        return ReadResult.OK;
                    }
                }
            }
            this.ClearAuth();
            if (flag)
            {
                return nONE;
            }
        }
        return ReadResult.NONE;
    }

    public bool ReadFriendCode()
    {
        if (!ManagerConfig.UseMock)
        {
            string path = getFriendCodeFileName();
            Debug.Log("NetworkManager::read " + path);
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                try
                {
                    friendCode = reader.ReadString();
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        ((IDisposable) reader).Dispose();
                    }
                }
            }
        }
        return false;
    }

    public bool ReadOldSignup()
    {
        if (!ManagerConfig.UseMock)
        {
            string path = getOldSignupFileName();
            Debug.Log("NetworkManager::oldRead " + path);
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                try
                {
                    userName = reader.ReadString();
                    genderType = reader.ReadInt32();
                    month = reader.ReadInt32();
                    day = reader.ReadInt32();
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        ((IDisposable) reader).Dispose();
                    }
                }
            }
        }
        return false;
    }

    public bool ReadServerSetting()
    {
        if (ManagerConfig.UseDebugCommand)
        {
            string path = getServerSettingFileName();
            Debug.Log("NetworkManager::read " + path);
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                try
                {
                    serverSettingType = reader.ReadString();
                    serverSettingSecurity = reader.ReadBoolean();
                    gameServerSettingAddress = reader.ReadString();
                    dataServerSettingAddress = reader.ReadString();
                    webServerSettingAddress = reader.ReadString();
                    this.SetServerSetting();
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        ((IDisposable) reader).Dispose();
                    }
                }
            }
        }
        this.SetServerSetting("DEFAULT", false, string.Empty, string.Empty, string.Empty);
        return false;
    }

    public bool ReadSignup()
    {
        if (!ManagerConfig.UseMock)
        {
            string path = getSignupFileName();
            Debug.Log("NetworkManager::read " + path);
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                try
                {
                    userName = reader.ReadString();
                    genderType = reader.ReadInt32();
                    month = reader.ReadInt32();
                    day = reader.ReadInt32();
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        ((IDisposable) reader).Dispose();
                    }
                }
            }
        }
        return false;
    }

    public void ReleseService()
    {
        Debug.Log("NetworkManager::ReleseService");
    }

    public static void ReplaceBaseField(RequestBase request, bool isRefreshTime = false)
    {
        request.replaceField("appVer", ManagerConfig.AppVer);
        request.replaceField("dataVer", SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion());
        request.replaceField("dateVer", SingletonMonoBehaviour<DataManager>.Instance.getMasterDateVersion());
        string data = CryptData.EncryptMD5Usk(PlayerPrefs.GetString("usk"));
        request.replaceField("usk", data);
        if (isRefreshTime)
        {
            request.replaceField("lastAccessTime", getTime());
        }
    }

    [DebuggerHidden]
    private IEnumerator RequestApplicationUpdateCR(string storeName, string id, StoreCallbackFunc callback) => 
        new <RequestApplicationUpdateCR>c__IteratorE { 
            storeName = storeName,
            id = id,
            callback = callback,
            <$>storeName = storeName,
            <$>id = id,
            <$>callback = callback
        };

    [DebuggerHidden]
    private IEnumerator RequestCR(RequestBase work, float delay) => 
        new <RequestCR>c__IteratorD { 
            work = work,
            delay = delay,
            <$>work = work,
            <$>delay = delay,
            <>f__this = this
        };

    public void RequestLogin(LoginCallbackFunc callback, bool isEnforce = true)
    {
        this.loginCallbackFunc = callback;
        TopLoginRequest request = getRequest<TopLoginRequest>(new ResultCallbackFunc(this.EndTopLogin));
        request.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
        request.addField("os", SystemInfo.operatingSystem);
        request.addField("ptype", SystemInfo.deviceModel);
        request.addField("userAgent", 1);
        request.addField("rgsid", SingletonMonoBehaviour<DataManager>.Instance.serverId.ToString());
        request.addField("rguid", PlayerPrefs.GetString("rguid"));
        request.addField("rgusk", PlayerPrefs.GetString("usk"));
        request.addField("idfa", string.Empty);
        request.addField("t", 0x4faf);
        request.addField("v", "1.0.1");
        request.addField("s", 1);
        request.addField("mac", (double) 0.0);
        request.addField("imei", string.Empty);
        request.addField("type", "login");
        request.addField("nickname", PlayerPrefs.GetString("nickname"));
        request.addField("rksdkid", 1);
        request.addField("rkchannel", "24");
        if (TopGameDataRequest.checkResetTime())
        {
            isRebootBlock = true;
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
        else if (!isEnforce && request.checkExpirationDate())
        {
            this.EndLogin();
        }
        else
        {
            request.beginRequest();
        }
    }

    public static void RequestStart(RequestBase request)
    {
        if (SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            SingletonMonoBehaviour<NetworkManager>.Instance.AddWaitStatus(request);
            SingletonMonoBehaviour<NetworkManager>.Instance.CommunicationStart();
        }
    }

    public void SetAuth(string userId, string authKey, string secretKey)
    {
        if (userId != null)
        {
            NetworkManager.userId = userId;
            NetworkManager.authKey = authKey;
            NetworkManager.secretKey = secretKey;
        }
    }

    public static void SetBaseField(RequestBase request)
    {
        if (userId != null)
        {
            request.addField("userId", userId);
        }
        request.addField("appVer", ManagerConfig.AppVer);
        request.addField("dataVer", SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion());
        request.addField("dateVer", SingletonMonoBehaviour<DataManager>.Instance.getMasterDateVersion());
        request.addField("lastAccessTime", getTime());
        request.addField("try", string.Empty);
        if (ManagerConfig.DevelopmentAuthCode.Length > 0)
        {
            request.addField("developmentAuthCode", ManagerConfig.DevelopmentAuthCode);
        }
    }

    public static void SetDataServerAddress(string addres)
    {
        dataServerAddress = addres;
    }

    public static void SetDataServerFolderName(string folderName)
    {
        dataServerFolder = (folderName == null) ? string.Empty : folderName;
    }

    public static void SetDataServerRedirect(string address)
    {
        dataServerRedirectAddress = address;
    }

    public void SetFriendCode(string friendCode)
    {
        if (friendCode != null)
        {
            NetworkManager.friendCode = friendCode;
        }
    }

    public static void SetGameServerAddress(string addres)
    {
        gameServerAddress = addres;
    }

    public static void SetGameServerRedirect(string address, bool isSecurity)
    {
        serverRedirectSecurity = isSecurity;
        gameServerRedirectAddress = address;
    }

    public void SetServerSetting()
    {
        if (gameServerRedirectAddress != null)
        {
            serverSecurity = serverRedirectSecurity;
            gameServerAddress = gameServerRedirectAddress + "/";
        }
        else
        {
            serverSecurity = GetSecurityServerSetting(serverSettingType);
            gameServerAddress = GetGameServerSetting(serverSettingType);
        }
        if (!AssetManager.IsOnline)
        {
            dataServerAddress = Application.streamingAssetsPath + "/AssetStorages/" + ManagerConfig.PlatformName + "/";
            if (!dataServerAddress.StartsWith("file://") && !dataServerAddress.StartsWith("jar:file://"))
            {
                dataServerAddress = "file://" + dataServerAddress;
            }
        }
        else if (dataServerRedirectAddress != null)
        {
            string[] textArray1 = new string[] { dataServerRedirectAddress, "/", dataServerFolder, ManagerConfig.PlatformName, "/" };
            dataServerAddress = string.Concat(textArray1);
        }
        else
        {
            string[] textArray2 = new string[] { GetDataServerSetting(serverSettingType), "/NewResources/", dataServerFolder, ManagerConfig.PlatformName, "/" };
            dataServerAddress = string.Concat(textArray2);
        }
        if (webServerRedirectAddress != null)
        {
            webServerAddress = webServerRedirectAddress;
        }
        else
        {
            webServerAddress = GetWebServerSetting(serverSettingType);
        }
    }

    public void SetServerSetting(string type, bool isSecuritey, string gameAddress, string dataAddress, string webAddress)
    {
        serverSettingType = type;
        serverSettingSecurity = isSecuritey;
        gameServerSettingAddress = gameAddress;
        dataServerSettingAddress = dataAddress;
        webServerSettingAddress = webAddress;
        this.SetServerSetting();
    }

    public void SetSignup(string userName, int genderType)
    {
        if (userName != null)
        {
            NetworkManager.userName = userName;
            NetworkManager.genderType = genderType;
        }
    }

    public void SetSignup(string userName, int genderType, int month, int day)
    {
        if (userName != null)
        {
            NetworkManager.userName = userName;
            NetworkManager.genderType = genderType;
            NetworkManager.month = month;
            NetworkManager.day = day;
        }
    }

    public static void SetWebServerRedirect(string address)
    {
        webServerRedirectAddress = address;
    }

    protected int SyncAuth()
    {
        int num2 = 10;
        while (num2-- > 0)
        {
            try
            {
                string str = PlayerPrefs.GetString(AUTHMD5_KEY_TYPE, string.Empty);
                string str2 = this.LoadAuthMd5(getAuthFileName(false));
                string str3 = this.LoadAuthMd5(getAuthFileName(true));
                if ((str2 == null) && (this.LoadAuthMd5(getOldAuthFileName()) != null))
                {
                    File.Copy(getOldAuthFileName(), getAuthFileName(false), true);
                    if (this.ReadOldSignup())
                    {
                        this.WriteSignup();
                        File.Delete(getOldSignupFileName());
                    }
                    if (SingletonMonoBehaviour<AccountingManager>.Instance.ReadOldPayment())
                    {
                        SingletonMonoBehaviour<AccountingManager>.Instance.WritePayment();
                        File.Delete(AccountingManager.GetOldPaymentFileName());
                    }
                    continue;
                }
                if ((str2 == null) && (str3 == null))
                {
                    return 1;
                }
                if ((str2 == null) && (str3 != null))
                {
                    File.Copy(getAuthFileName(true), getAuthFileName(false), true);
                    str2 = this.LoadAuthMd5(getAuthFileName(false));
                }
                if ((str3 == null) && (str2 != null))
                {
                    File.Copy(getAuthFileName(false), getAuthFileName(true), true);
                    str3 = this.LoadAuthMd5(getAuthFileName(true));
                }
                if ((str2 == str3) && (str2 == str))
                {
                    return 0;
                }
                if ((str2 == str3) && (str2 != str))
                {
                    PlayerPrefs.SetString(AUTHMD5_KEY_TYPE, str2);
                    PlayerPrefs.Save();
                    continue;
                }
                if (str2 == str3)
                {
                    continue;
                }
                if (str2 == str)
                {
                    File.Copy(getAuthFileName(false), getAuthFileName(true), true);
                    continue;
                }
                if (str3 == str)
                {
                    File.Copy(getAuthFileName(true), getAuthFileName(false), true);
                    continue;
                }
                return 2;
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
                continue;
            }
        }
        return 0;
    }

    protected void WarningDialog(string errorTitle, string errorDetail, string errorUrl, System.Action callback)
    {
        this.errorCallbackFunc = callback;
        if (errorUrl != null)
        {
            WebViewManager.OpenView(errorTitle, errorUrl, new System.Action(this.EndErrorWebView));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog(errorTitle, errorDetail, new ErrorDialog.ClickDelegate(this.EndErrorDialog), false);
        }
    }

    public void WriteAuth()
    {
        if (!ManagerConfig.UseMock && (userId != null))
        {
            this.WriteAuthFile(getAuthFileName(false));
            this.WriteAuthFile(getAuthFileName(true));
        }
    }

    protected bool WriteAuthFile(string fname)
    {
        for (int i = 3; i >= 0; i--)
        {
            try
            {
                string str = string.Empty;
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(fname)))
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object> {
                        { 
                            "SaveDataVer",
                            ManagerConfig.SaveDataVer
                        },
                        { 
                            "userCreateServer",
                            gameServerAddress
                        },
                        { 
                            "authKey",
                            authKey
                        },
                        { 
                            "secretKey",
                            secretKey
                        }
                    };
                    str = CryptData.Encrypt(JsonManager.toJson(dictionary), false);
                    writer.Write(str);
                }
                string str2 = EncryptedPlayerPrefs.Md5(str);
                PlayerPrefs.SetString(AUTHMD5_KEY_TYPE, str2);
                PlayerPrefs.Save();
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        return false;
    }

    public void WriteFriendCode()
    {
        if (!ManagerConfig.UseMock && (userId != null))
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(getFriendCodeFileName())))
            {
                writer.Write(friendCode);
            }
        }
    }

    public void WriteServerSetting()
    {
        if (ManagerConfig.UseDebugCommand)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(getServerSettingFileName())))
            {
                writer.Write(serverSettingType);
                writer.Write(serverSettingSecurity);
                writer.Write(gameServerSettingAddress);
                writer.Write(dataServerSettingAddress);
                writer.Write(webServerSettingAddress);
            }
        }
    }

    public void WriteSignup()
    {
        if (!ManagerConfig.UseMock && (userId != null))
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(getSignupFileName())))
            {
                writer.Write(userName);
                writer.Write(genderType);
                writer.Write(month);
                writer.Write(day);
            }
        }
    }

    public static bool IsLogin =>
        isLogin;

    public static bool IsRebootBlock =>
        isRebootBlock;

    public string LoginResponseResult
    {
        set
        {
            this.loginResponseResult = value;
        }
    }

    public static string UserCreateServer =>
        userCreateServer;

    public static long UserId
    {
        get
        {
            if (userId != null)
            {
                return long.Parse(userId);
            }
            return -1L;
        }
    }

    [CompilerGenerated]
    private sealed class <RequestApplicationUpdateCR>c__IteratorE : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal NetworkManager.StoreCallbackFunc <$>callback;
        internal string <$>id;
        internal string <$>storeName;
        internal string <data>__1;
        internal Dictionary<string, object>[] <results>__4;
        internal Dictionary<string, object> <rootParam>__3;
        internal string <url>__0;
        internal WWW <www>__2;
        internal NetworkManager.StoreCallbackFunc callback;
        internal string id;
        internal string storeName;

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
                    if (this.storeName == null)
                    {
                        this.storeName = "Android";
                    }
                    this.<url>__0 = null;
                    if (this.storeName != "iOS")
                    {
                        if (this.storeName == "Android")
                        {
                            if (this.id == null)
                            {
                                this.id = ManagerConfig.AndroidPackageName;
                            }
                            this.<url>__0 = "http://market.android.com/details?id=" + this.id;
                        }
                        goto Label_0208;
                    }
                    this.<data>__1 = null;
                    if (this.id == null)
                    {
                        this.id = ManagerConfig.iOSApplicationID;
                    }
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(this.<www>__2.error) || !string.IsNullOrEmpty(this.<www>__2.error))
                    {
                        this.<www>__2.Dispose();
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 2;
                        goto Label_0249;
                    }
                    this.<data>__1 = this.<www>__2.text;
                    this.<www>__2.Dispose();
                    this.<rootParam>__3 = JsonManager.getDictionary(this.<data>__1);
                    if (this.<rootParam>__3.ContainsKey("results"))
                    {
                        this.<results>__4 = JsonManager.DeserializeArray<Dictionary<string, object>>(this.<rootParam>__3["results"]);
                        if (((this.<results>__4 != null) && (this.<results>__4.Length > 0)) && ((this.<results>__4[0] != null) && this.<results>__4[0].ContainsKey("trackViewUrl")))
                        {
                            this.<url>__0 = this.<results>__4[0]["trackViewUrl"].ToString();
                        }
                    }
                    goto Label_0208;

                case 2:
                    break;

                case 3:
                    if (this.callback != null)
                    {
                        this.callback(this.<url>__0);
                    }
                    this.$PC = -1;
                    goto Label_0247;

                default:
                    goto Label_0247;
            }
            this.<www>__2 = new WWW("https://itunes.apple.com/lookup?id=" + this.id + "&country=jp");
            this.$current = this.<www>__2;
            this.$PC = 1;
            goto Label_0249;
        Label_0208:
            this.$current = new WaitForSeconds(0.5f);
            this.$PC = 3;
            goto Label_0249;
        Label_0247:
            return false;
        Label_0249:
            return true;
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
    private sealed class <RequestCR>c__IteratorD : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>delay;
        internal RequestBase <$>work;
        internal Dictionary<string, string>.Enumerator <$s_254>__16;
        internal Dictionary<string, string>.Enumerator <$s_255>__29;
        internal string[] <$s_256>__33;
        internal int <$s_257>__34;
        internal int[] <$s_258>__40;
        internal int <$s_259>__41;
        internal int[] <$s_260>__43;
        internal int <$s_261>__44;
        internal int[] <$s_262>__46;
        internal int <$s_263>__47;
        internal NetworkManager <>f__this;
        internal string <authCode>__18;
        internal SortedDictionary<string, string> <authParams>__13;
        internal Dictionary<string, object> <cache>__49;
        internal string <data>__0;
        internal KeyValuePair<string, string> <entry>__17;
        internal string <errorAction>__3;
        internal string <errorCode>__1;
        internal string <errorDetail>__54;
        internal string <errorLocalizeCode>__2;
        internal string <errorTitle>__53;
        internal string <errorUrl>__55;
        internal Exception <ex>__12;
        internal Exception <ex>__28;
        internal Exception <ex>__37;
        internal ResponseFailData <failData>__4;
        internal WWWForm <form>__14;
        internal Dictionary<string, string> <headers>__15;
        internal int <i>__38;
        internal int <i>__50;
        internal int <id>__42;
        internal int <id>__45;
        internal int <id>__48;
        internal int <index>__20;
        internal bool <isBase64Exception>__22;
        internal bool <isBase64Exception>__9;
        internal KeyValuePair<string, string> <kvp>__30;
        internal float <loadProgress>__25;
        internal float <requestTime>__24;
        internal ResponseData <response>__39;
        internal ResponseData <response>__51;
        internal ResponseData[] <responseList>__5;
        internal ResponseCommandBase.Result <result>__52;
        internal string <ss1>__31;
        internal string[] <ssl1>__32;
        internal string <sss>__35;
        internal long <startTime>__6;
        internal string <text>__11;
        internal string <text>__27;
        internal string <textData>__10;
        internal string <textData>__26;
        internal float <timeout>__23;
        internal string <url>__19;
        internal string <url>__7;
        internal WWW <www>__21;
        internal WWW <www>__8;
        internal Dictionary<string, object> <wwwparam>__36;
        internal float delay;
        internal RequestBase work;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            string str;
            Dictionary<string, int> dictionary;
            int num2;
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    Debug.Log("NetworkRequest start [" + this.work.getURL() + "]");
                    break;

                case 1:
                    if (this.delay <= 0f)
                    {
                        goto Label_00DD;
                    }
                    this.$current = new WaitForSeconds(this.delay);
                    this.$PC = 2;
                    goto Label_171B;

                case 2:
                    goto Label_00DD;

                case 3:
                    this.<textData>__10 = string.Empty;
                    try
                    {
                        if (!this.<>f__this.isBate64Retry)
                        {
                            this.<text>__11 = WWW.UnEscapeURL(this.<www>__8.text);
                            this.<textData>__10 = CryptData.ResponseDecrypt(this.<text>__11);
                        }
                        else
                        {
                            this.<>f__this.isBate64Retry = false;
                            this.<textData>__10 = this.<www>__8.text;
                        }
                    }
                    catch (Exception exception)
                    {
                        this.<ex>__12 = exception;
                        Debug.Log(this.<ex>__12);
                        this.<isBase64Exception>__9 = true;
                    }
                    if (!this.<isBase64Exception>__9)
                    {
                        if (!string.IsNullOrEmpty(this.<www>__8.error))
                        {
                            Debug.LogWarning("NetworkMockLoadError " + this.<www>__8.error);
                        }
                        else
                        {
                            this.<data>__0 = this.<textData>__10;
                        }
                        this.<www>__8.Dispose();
                        goto Label_02B2;
                    }
                    this.<isBase64Exception>__9 = false;
                    SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                    SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("05");
                    goto Label_0225;

                case 4:
                    goto Label_0225;

                case 5:
                    goto Label_056F;

                case 6:
                    goto Label_05F5;

                case 7:
                    goto Label_0721;

                case 8:
                    goto Label_11AA;

                case 9:
                    goto Label_1357;

                case 10:
                    goto Label_13C5;

                default:
                    goto Label_1719;
            }
        Label_0064:
            this.<data>__0 = null;
            this.<errorCode>__1 = null;
            this.<errorLocalizeCode>__2 = null;
            this.<errorAction>__3 = null;
            this.<failData>__4 = null;
            this.<responseList>__5 = null;
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            goto Label_171B;
        Label_00DD:
            this.<startTime>__6 = NetworkManager.getTime(DateTime.Now.ToUniversalTime());
            if (!ManagerConfig.UseMock)
            {
                if (this.<data>__0 != null)
                {
                    goto Label_0E81;
                }
                this.<form>__14 = this.work.getWWWForm(out this.<authParams>__13);
                this.<headers>__15 = new Dictionary<string, string>();
                this.<$s_254>__16 = this.<form>__14.headers.GetEnumerator();
                try
                {
                    while (this.<$s_254>__16.MoveNext())
                    {
                        this.<entry>__17 = this.<$s_254>__16.Current;
                        string key = Convert.ToString(this.<entry>__17.Key);
                        this.<headers>__15.Add(key, Convert.ToString(this.<entry>__17.Value));
                    }
                }
                finally
                {
                    this.<$s_254>__16.Dispose();
                }
                if (NetworkManager.userAgent != null)
                {
                    this.<headers>__15.Add("User-Agent", NetworkManager.userAgent);
                }
                if (NetworkManager.sessionId != null)
                {
                    this.<headers>__15.Add("Cookie", NetworkManager.sessionId);
                }
                this.<authCode>__18 = NetworkManager.GetAuthCode(this.<authParams>__13);
                if (this.<authCode>__18 != null)
                {
                    this.<form>__14.AddField("authCode", this.<authCode>__18);
                }
                this.<url>__19 = this.work.getURL();
                if (NetworkManager.userId != null)
                {
                    this.<index>__20 = this.<url>__19.IndexOf("?");
                    this.<url>__19 = this.<url>__19 + ((this.<index>__20 >= 0) ? "&" : "?") + "_userId=" + NetworkManager.userId;
                }
                this.<www>__21 = new WWW(this.<url>__19, this.<form>__14.data, this.<headers>__15);
                this.<isBase64Exception>__22 = false;
                this.<timeout>__23 = !(this.work is TopGameDataRequest) ? ManagerConfig.TIMEOUT : ManagerConfig.GAME_DATA_TIMEOUT;
                this.<requestTime>__24 = Time.time + this.<timeout>__23;
                this.<loadProgress>__25 = 0f;
                goto Label_056F;
            }
            if (!ManagerConfig.IsNetworkMock)
            {
                goto Label_02B2;
            }
            this.<url>__7 = this.work.getMockURL();
            if (this.<url>__7 == null)
            {
                goto Label_02B2;
            }
            this.<www>__8 = new WWW(this.<url>__7);
            this.<isBase64Exception>__9 = false;
            this.$current = this.<www>__8;
            this.$PC = 3;
            goto Label_171B;
        Label_0225:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 4;
                goto Label_171B;
            }
            this.<www>__8.Dispose();
            this.<>f__this.StartCoroutine(this.<>f__this.RequestCR(this.work, this.delay));
            goto Label_1719;
        Label_02B2:
            if (this.<data>__0 != null)
            {
                goto Label_0E81;
            }
            this.<data>__0 = this.work.getMockData();
            if (this.<data>__0 != string.Empty)
            {
                goto Label_0E81;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            this.<>f__this.OnEndCommunication(this.work, null);
            goto Label_1712;
        Label_056F:
            while (!this.<www>__21.isDone)
            {
                if (this.<www>__21.progress != this.<loadProgress>__25)
                {
                    this.<requestTime>__24 = Time.time + this.<timeout>__23;
                    this.<loadProgress>__25 = this.<www>__21.progress;
                }
                else if (Time.time >= this.<requestTime>__24)
                {
                    Debug.LogWarning("TimeOut");
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 5;
                goto Label_171B;
            }
            if (!this.<www>__21.isDone)
            {
                this.<errorCode>__1 = "connect time over";
                this.<errorAction>__3 = "retry";
                goto Label_0E81;
            }
            if (string.IsNullOrEmpty(this.<www>__21.error))
            {
                Debug.LogError("www.text:::::::" + this.<www>__21.text);
                this.<textData>__26 = string.Empty;
                try
                {
                    if (!this.<>f__this.isBate64Retry)
                    {
                        this.<text>__27 = WWW.UnEscapeURL(this.<www>__21.text);
                        this.<textData>__26 = CryptData.ResponseDecrypt(this.<text>__27);
                    }
                    else
                    {
                        this.<textData>__26 = this.<www>__21.text;
                        this.<>f__this.isBate64Retry = false;
                    }
                }
                catch (Exception exception2)
                {
                    this.<ex>__28 = exception2;
                    Debug.Log(this.<ex>__28);
                    this.<isBase64Exception>__22 = true;
                }
                if (!this.<isBase64Exception>__22)
                {
                    this.<data>__0 = this.<textData>__26;
                    ManagementManager.mEndNSerIndex = ManagementManager.mNSerIndex;
                    if (((NetworkManager.sessionId == null) && (this.work is TopLoginRequest)) && (this.<www>__21.responseHeaders.Count > 0))
                    {
                        this.<$s_255>__29 = this.<www>__21.responseHeaders.GetEnumerator();
                        try
                        {
                            while (this.<$s_255>__29.MoveNext())
                            {
                                this.<kvp>__30 = this.<$s_255>__29.Current;
                                Debug.Log("    [" + this.<kvp>__30.Key + "] " + this.<kvp>__30.Value);
                            }
                        }
                        finally
                        {
                            this.<$s_255>__29.Dispose();
                        }
                        if (this.<www>__21.responseHeaders.ContainsKey("SET-COOKIE"))
                        {
                            this.<ss1>__31 = this.<www>__21.responseHeaders["SET-COOKIE"];
                            char[] separator = new char[] { ' ', ';' };
                            this.<ssl1>__32 = this.<ss1>__31.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            NetworkManager.sessionId = string.Empty;
                            this.<$s_256>__33 = this.<ssl1>__32;
                            this.<$s_257>__34 = 0;
                            while (this.<$s_257>__34 < this.<$s_256>__33.Length)
                            {
                                this.<sss>__35 = this.<$s_256>__33[this.<$s_257>__34];
                                if (this.<sss>__35.StartsWith("path="))
                                {
                                    break;
                                }
                                NetworkManager.sessionId = NetworkManager.sessionId + this.<sss>__35 + ";";
                                this.<$s_257>__34++;
                            }
                        }
                    }
                    goto Label_0E81;
                }
                this.<isBase64Exception>__22 = false;
                SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("05");
                goto Label_0721;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("01");
        Label_05F5:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 6;
                goto Label_171B;
            }
            this.<www>__21.Dispose();
            this.<>f__this.StartCoroutine(this.<>f__this.RequestCR(this.work, this.delay));
            goto Label_1719;
        Label_0721:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 7;
                goto Label_171B;
            }
            this.<www>__21.Dispose();
            this.<>f__this.StartCoroutine(this.<>f__this.RequestCR(this.work, this.delay));
            goto Label_1719;
        Label_0E81:
            while (this.<errorCode>__1 == null)
            {
                if (this.<data>__0 == null)
                {
                    this.<errorCode>__1 = "none data";
                    this.<errorLocalizeCode>__2 = "NETWORK_ERROR_SERVER_CONNECT_MESSAGE";
                    this.<errorAction>__3 = "goto_title";
                    break;
                }
                Debug.Log("data " + this.<data>__0);
                if (this.<data>__0 == "Internal Server Error")
                {
                    this.<errorCode>__1 = "internal server error";
                    this.<errorLocalizeCode>__2 = "NETWORK_ERROR_SERVER_BUSY_MESSAGE";
                    this.<errorAction>__3 = "retry";
                    break;
                }
                this.<wwwparam>__36 = null;
                try
                {
                    this.<wwwparam>__36 = JsonManager.getDictionary(this.<data>__0);
                }
                catch (Exception exception3)
                {
                    this.<ex>__37 = exception3;
                    Debug.LogError(this.<ex>__37);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, "服务器连接失败，请检查网络连接！", new ErrorDialog.ClickDelegate(this.<>f__this.OnClickErrorDialog), false);
                    goto Label_1719;
                }
                if (this.<wwwparam>__36.ContainsKey("response"))
                {
                    Debug.Log("    responseList -->");
                    this.<responseList>__5 = JsonManager.DeserializeArray<ResponseData>(this.<wwwparam>__36["response"]);
                    Debug.Log("--> responseList");
                    this.<i>__38 = 0;
                    while (this.<i>__38 < this.<responseList>__5.Length)
                    {
                        this.<response>__39 = this.<responseList>__5[this.<i>__38];
                        if (!string.IsNullOrEmpty(this.<response>__39.usk))
                        {
                            DataManager.isBattleLive = this.<response>__39.isBattleLive;
                            PlayerPrefs.SetString("usk", this.<response>__39.usk);
                            if (this.<response>__39.svtIds != null)
                            {
                                DataManager.svtIds.Clear();
                                this.<$s_258>__40 = this.<response>__39.svtIds;
                                this.<$s_259>__41 = 0;
                                while (this.<$s_259>__41 < this.<$s_258>__40.Length)
                                {
                                    this.<id>__42 = this.<$s_258>__40[this.<$s_259>__41];
                                    DataManager.svtIds.Add(this.<id>__42);
                                    this.<$s_259>__41++;
                                }
                            }
                            if (this.<response>__39.questIds != null)
                            {
                                DataManager.questIds.Clear();
                                this.<$s_260>__43 = this.<response>__39.questIds;
                                this.<$s_261>__44 = 0;
                                while (this.<$s_261>__44 < this.<$s_260>__43.Length)
                                {
                                    this.<id>__45 = this.<$s_260>__43[this.<$s_261>__44];
                                    DataManager.questIds.Add(this.<id>__45);
                                    this.<$s_261>__44++;
                                }
                            }
                            if (this.<response>__39.mstSkillIds != null)
                            {
                                DataManager.mstSkillIds.Clear();
                                this.<$s_262>__46 = this.<response>__39.mstSkillIds;
                                this.<$s_263>__47 = 0;
                                while (this.<$s_263>__47 < this.<$s_262>__46.Length)
                                {
                                    this.<id>__48 = this.<$s_262>__46[this.<$s_263>__47];
                                    DataManager.mstSkillIds.Add(this.<id>__48);
                                    this.<$s_263>__47++;
                                }
                            }
                        }
                        if (!this.<response>__39.checkError())
                        {
                            string[] textArray1 = new string[] { "response error ", this.<response>__39.nid, " (", this.<response>__39.resCode, ")" };
                            this.<errorCode>__1 = string.Concat(textArray1);
                            this.<failData>__4 = new ResponseFailData(this.<response>__39);
                            this.<errorAction>__3 = !string.IsNullOrEmpty(this.<failData>__4.action) ? this.<failData>__4.action : "retry";
                            break;
                        }
                        this.<i>__38++;
                    }
                }
                else
                {
                    this.<errorCode>__1 = "none response data";
                    this.<errorLocalizeCode>__2 = "NETWORK_ERROR_SERVER_BUSY_MESSAGE";
                    this.<errorAction>__3 = "retry";
                }
                if (this.<errorCode>__1 == null)
                {
                    if (this.<wwwparam>__36.ContainsKey("cache"))
                    {
                        this.<cache>__49 = (Dictionary<string, object>) this.<wwwparam>__36["cache"];
                        if (this.<cache>__49.ContainsKey("serverTime"))
                        {
                            NetworkManager.serverTime = long.Parse(this.<cache>__49["serverTime"].ToString());
                            NetworkManager.serverOffsetTime = NetworkManager.serverTime - this.<startTime>__6;
                            Debug.Log(string.Concat(new object[] { "ServerTime ", NetworkManager.serverTime, " [", NetworkManager.serverOffsetTime, "]" }));
                        }
                        SingletonMonoBehaviour<DataManager>.Instance.updateJsonData(this.<wwwparam>__36["cache"]);
                    }
                    this.<i>__50 = 0;
                    while (this.<i>__50 < this.<responseList>__5.Length)
                    {
                        this.<response>__51 = this.<responseList>__5[this.<i>__50];
                        this.<result>__52 = NetworkManager.ExecuteCommand(this.<response>__51);
                        if (this.<result>__52 != ResponseCommandBase.Result.SUCCESS)
                        {
                            this.<errorCode>__1 = "response execute error " + this.<response>__51.nid;
                            this.<failData>__4 = new ResponseFailData(this.<response>__51);
                            this.<errorAction>__3 = !string.IsNullOrEmpty(this.<failData>__4.action) ? this.<failData>__4.action : "retry";
                            break;
                        }
                        this.<i>__50++;
                    }
                }
                break;
            }
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            if (this.<errorCode>__1 == null)
            {
                this.<>f__this.OnEndCommunication(this.work, this.<responseList>__5);
                goto Label_1712;
            }
            Debug.LogError(this.<errorCode>__1);
            this.<>f__this.communicationWorkResponseList = this.<responseList>__5;
            this.<responseList>__5 = null;
            if (this.<errorLocalizeCode>__2 == null)
            {
                str = this.<errorAction>__3;
                if (str != null)
                {
                    if (NetworkManager.<>f__switch$mapB == null)
                    {
                        dictionary = new Dictionary<string, int>(4) {
                            { 
                                "stay",
                                0
                            },
                            { 
                                "retry",
                                1
                            },
                            { 
                                "reload_cache",
                                1
                            },
                            { 
                                "goto_login_access",
                                2
                            }
                        };
                        NetworkManager.<>f__switch$mapB = dictionary;
                    }
                    if (NetworkManager.<>f__switch$mapB.TryGetValue(str, out num2))
                    {
                        switch (num2)
                        {
                            case 0:
                                this.<errorLocalizeCode>__2 = "NETWORK_ERROR_SERVER_CANCEL_MESSAGE";
                                goto Label_0FB4;

                            case 1:
                                this.<errorLocalizeCode>__2 = !ManagementManager.IsDuringStartup ? "NETWORK_ERROR_TIME_OVER_MESSAGE" : "NETWORK_ERROR_BOOT_RETRY_MESSAGE";
                                goto Label_0FB4;

                            case 2:
                                goto Label_0FB4;
                        }
                    }
                }
                this.<errorLocalizeCode>__2 = !ManagementManager.IsDuringStartup ? "NETWORK_ERROR_SERVER_CONNECT_MESSAGE" : "NETWORK_ERROR_BOOT_MESSAGE";
            }
        Label_0FB4:
            this.<errorTitle>__53 = string.Empty;
            this.<errorDetail>__54 = (this.<errorLocalizeCode>__2 == null) ? string.Empty : LocalizationManager.Get(this.<errorLocalizeCode>__2);
            this.<errorUrl>__55 = null;
            if (this.<failData>__4 != null)
            {
                if (!string.IsNullOrEmpty(this.<failData>__4.title))
                {
                    this.<errorTitle>__53 = this.<failData>__4.title;
                }
                if (!string.IsNullOrEmpty(this.<failData>__4.detail))
                {
                    this.<errorDetail>__54 = this.<failData>__4.detail;
                    Debug.LogError(this.<failData>__4.detail);
                }
                if (!string.IsNullOrEmpty(this.<errorDetail>__54) && (this.<failData>__4.csId != null))
                {
                    this.<errorDetail>__54 = this.<errorDetail>__54 + "\n" + string.Format(LocalizationManager.Get("NETWORK_ERROR_CSID_CODE"), this.<failData>__4.csId);
                }
                if (this.<failData>__4.url != null)
                {
                    NetworkManager.failDataUrl = this.<failData>__4.url;
                    this.<errorUrl>__55 = this.<failData>__4.url;
                }
            }
            if (((!ManagerConfig.UseDebugCommand || (ManagerConfig.ServerDefaultType == "SCRIPT")) || ((this.<errorAction>__3 == "reconnection") || (this.<errorAction>__3 == "invalid_battle"))) || (((this.<errorAction>__3 == "stay") || (this.<errorAction>__3 == "reload_cache")) || (this.<errorAction>__3 == "goto_login_access")))
            {
                goto Label_11BA;
            }
            this.<>f__this.isWaitDebugDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Network error for debug", this.<errorCode>__1, new ErrorDialog.ClickDelegate(this.<>f__this.OnClickWaitDebugDialog), false);
        Label_11AA:
            while (this.<>f__this.isWaitDebugDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 8;
                goto Label_171B;
            }
        Label_11BA:
            str = this.<errorAction>__3;
            if (str != null)
            {
                if (NetworkManager.<>f__switch$mapC == null)
                {
                    dictionary = new Dictionary<string, int>(13) {
                        { 
                            "stay",
                            0
                        },
                        { 
                            "retry",
                            1
                        },
                        { 
                            "reload_cache",
                            2
                        },
                        { 
                            "goto_title",
                            3
                        },
                        { 
                            "goto_login_access",
                            4
                        },
                        { 
                            "data_update",
                            5
                        },
                        { 
                            "mainte",
                            6
                        },
                        { 
                            "do_signup",
                            7
                        },
                        { 
                            "invalid_user",
                            8
                        },
                        { 
                            "app_version_up",
                            9
                        },
                        { 
                            "reconnection",
                            10
                        },
                        { 
                            "invalid_battle",
                            11
                        },
                        { 
                            "goto_login",
                            12
                        }
                    };
                    NetworkManager.<>f__switch$mapC = dictionary;
                }
                if (NetworkManager.<>f__switch$mapC.TryGetValue(str, out num2))
                {
                    switch (num2)
                    {
                        case 0:
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorStay));
                            goto Label_16CB;

                        case 1:
                            if (!ManagementManager.IsDuringStartup)
                            {
                                SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("02");
                                goto Label_13C5;
                            }
                            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("02");
                            goto Label_1357;

                        case 2:
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickReloadCache));
                            goto Label_16CB;

                        case 3:
                            NetworkManager.isRebootBlock = true;
                            this.<>f__this.ErrorDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorReboot));
                            goto Label_16CB;

                        case 4:
                            NetworkManager.isRebootBlock = true;
                            if (!string.IsNullOrEmpty(this.<errorDetail>__54) || !string.IsNullOrEmpty(this.<errorUrl>__55))
                            {
                                this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorTerminal));
                            }
                            else
                            {
                                this.<>f__this.OnClickErrorTerminal();
                            }
                            goto Label_16CB;

                        case 5:
                            NetworkManager.isRebootBlock = true;
                            DataManager.ClearCacheAll();
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorReboot));
                            goto Label_16CB;

                        case 6:
                            NetworkManager.isRebootBlock = true;
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorReboot));
                            goto Label_16CB;

                        case 7:
                            NetworkManager.DeleteSignupData();
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorStay));
                            goto Label_16CB;

                        case 8:
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(this.<errorTitle>__53, this.<errorDetail>__54, LocalizationManager.Get("NETWORK_USER_DELETE_DECIDE"), LocalizationManager.Get("NETWORK_USER_DELETE_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.<>f__this.OnClickUserDelete));
                            goto Label_16CB;

                        case 9:
                            NetworkManager.isRebootBlock = true;
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, null, new System.Action(this.<>f__this.OnClickStore));
                            goto Label_16CB;

                        case 10:
                            NetworkManager.SetGameServerRedirect(this.<failData>__4.sandboxDomain, this.<failData>__4.sandboxSeurity);
                            NetworkManager.SetDataServerRedirect(this.<failData>__4.sandboxAssetsDomain);
                            NetworkManager.SetWebServerRedirect(this.<failData>__4.sandboxWebviewDomain);
                            this.<>f__this.SetServerSetting();
                            goto Label_16CB;

                        case 11:
                            BattleData.deleteSaveData();
                            this.<>f__this.OnEndCommunication(this.<>f__this.communicationWork, this.<>f__this.communicationWorkResponseList);
                            goto Label_16CB;

                        case 12:
                            this.<>f__this.WarningDialog(this.<errorTitle>__53, this.<errorDetail>__54, this.<errorUrl>__55, new System.Action(this.<>f__this.OnClickErrorStay));
                            goto Label_16CB;
                    }
                }
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(this.<errorTitle>__53, this.<errorDetail>__54, new ErrorDialog.ClickDelegate(this.<>f__this.OnClickErrorDialog), false);
            goto Label_16CB;
        Label_1357:
            if (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 9;
                goto Label_171B;
            }
            this.<>f__this.StartCoroutine(this.<>f__this.RequestCR(this.work, this.delay));
            goto Label_1719;
        Label_13C5:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 10;
                goto Label_171B;
            }
            this.<>f__this.StartCoroutine(this.<>f__this.RequestCR(this.work, this.delay));
            goto Label_1719;
        Label_16CB:
            this.<responseList>__5 = null;
            if (this.<errorAction>__3 == "reconnection")
            {
                goto Label_0064;
            }
        Label_1712:
            this.$PC = -1;
        Label_1719:
            return false;
        Label_171B:
            return true;
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

    public delegate void LoginCallbackFunc(string result);

    public enum ReadResult
    {
        OK,
        NONE,
        READ_ERROR,
        CRYPT_ERROR,
        JSON_ERROR,
        VERSION_ERROR,
        MD5_ERROR
    }

    public delegate void ResultCallbackFunc(string result);

    public delegate void StoreCallbackFunc(string url);
}

