using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AssetManager : SingletonMonoBehaviour<AssetManager>
{
    [SerializeField]
    protected bool _DispLog = true;
    protected static string assetBundleDateVersion = string.Empty;
    protected List<AssetData> assetBundleList = new List<AssetData>();
    protected static string assetBundleMasterVersion = string.Empty;
    protected List<AssetData> assetBundleReleaseList = new List<AssetData>();
    protected List<AssetData> assetResourceList = new List<AssetData>();
    protected static string backCacheListFileName;
    protected static readonly string backConfigFileName = "AssetStorageBack.txt";
    protected static string cacheListFileName;
    protected static string cachePathName;
    protected static string cacheTxTListFileName = string.Empty;
    protected static readonly string configFileName = "AssetStorage.txt";
    protected static string createCacheListFileName;
    protected static readonly string createConfigFileName = "AssetStorageCreate.txt";
    [SerializeField]
    protected UILabel debugStatusLabel;
    protected List<AssetLoader> downLoadList = new List<AssetLoader>();
    protected long downloadSize;
    protected Queue<LoadWaitStatus> downLoadWaitList = new Queue<LoadWaitStatus>();
    protected const int FILE_READ_RETRY_COUNT = 3;
    protected IEnumerator initCRW;
    protected bool isCancelDownload;
    protected bool isEnforceBoot;
    protected bool isErrorDialog;
    protected bool isInitEnd = true;
    protected bool isInitFirst = true;
    protected bool isOutDebugStatus;
    protected bool isPauseDownload;
    protected bool isRequestDebugStatusClear;
    [SerializeField]
    protected bool isUseDebugStatus;
    protected string requestDebugStatus;
    protected float requestWriteCounter = -1f;
    protected string rquestConfigWriteData = string.Empty;
    protected static string[] shieldingWord;

    protected bool AddCallbackLoadStatus(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        foreach (AssetLoader loader in this.downLoadList)
        {
            if (loader.IsSame(type, name))
            {
                loader.AddCallback(callbackFunc);
                return true;
            }
        }
        foreach (LoadWaitStatus status in this.downLoadWaitList)
        {
            if (status.IsSame(type, name))
            {
                status.AddCallback(callbackFunc);
                return true;
            }
        }
        return false;
    }

    protected bool AddEntryLoadStatus(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        foreach (AssetLoader loader in this.downLoadList)
        {
            if (loader.IsSame(type, name))
            {
                loader.AddEntry();
                loader.AddCallback(callbackFunc);
                return true;
            }
        }
        foreach (LoadWaitStatus status in this.downLoadWaitList)
        {
            if (status.IsSame(type, name))
            {
                status.AddEntry();
                status.AddCallback(callbackFunc);
                return true;
            }
        }
        return false;
    }

    protected void AddLoadWaitStatus(AssetData info)
    {
        this.downloadSize += info.Size;
        LoadWaitStatus item = new LoadWaitStatus(info);
        this.downLoadWaitList.Enqueue(item);
    }

    protected void AddLoadWaitStatus(System.Action callbackFunc)
    {
        LoadWaitStatus item = new LoadWaitStatus(callbackFunc);
        this.downLoadWaitList.Enqueue(item);
    }

    protected void AddLoadWaitStatus(AssetData info, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        this.downloadSize += info.Size;
        LoadWaitStatus item = new LoadWaitStatus(info, callbackFunc);
        this.downLoadWaitList.Enqueue(item);
    }

    public static void cancelDownloadAssetStorage()
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.CancelDownloadAssetStorage();
        }
    }

    public void CancelDownloadAssetStorage()
    {
        Queue<LoadWaitStatus> queue = new Queue<LoadWaitStatus>();
        while (this.downLoadWaitList.Count > 0)
        {
            LoadWaitStatus item = this.downLoadWaitList.Dequeue();
            AssetData info = item.Info;
            if ((info != null) && (info.EntryCount <= 0))
            {
                this.downloadSize -= info.Size;
            }
            else
            {
                queue.Enqueue(item);
            }
        }
        this.downLoadWaitList = queue;
        this.isCancelDownload = true;
    }

    public static bool CheckDateVersion(string dateVersion) => 
        assetBundleDateVersion.StartsWith(dateVersion);

    protected bool CheckEntryLoadStatus(AssetData.Type type, string name)
    {
        foreach (AssetLoader loader in this.downLoadList)
        {
            if (loader.IsSame(type, name))
            {
                return true;
            }
        }
        foreach (LoadWaitStatus status in this.downLoadWaitList)
        {
            if (status.IsSame(type, name))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckVersion(string masterVersion, string dateVersion) => 
        ((assetBundleMasterVersion == masterVersion) && (assetBundleDateVersion == dateVersion));

    public static bool compAssetStorage(AssetData data, string name)
    {
        if (data == null)
        {
            return (name?.Length == 0);
        }
        if (name == null)
        {
            return (name.Length == 0);
        }
        return data.Name.Equals(name);
    }

    public static bool compAssetStorage(string name1, string name2)
    {
        if (name1 == null)
        {
            return (name2?.Length == 0);
        }
        if (name2 == null)
        {
            return (name1.Length == 0);
        }
        return name1.Equals(name2);
    }

    public static bool compAssetStorageList(AssetData[] assetList, string[] list)
    {
        if (assetList == null)
        {
            return (list?.Length == 0);
        }
        if (assetList.Length == 0)
        {
            return (list?.Length == 0);
        }
        if (list == null)
        {
            return false;
        }
        if (assetList.Length != list.Length)
        {
            return false;
        }
        int length = assetList.Length;
        for (int i = 0; i < length; i++)
        {
            if (assetList[i] == null)
            {
                if (list[i] != null)
                {
                    return false;
                }
            }
            else
            {
                if (list[i] == null)
                {
                    return false;
                }
                if (!assetList[i].Name.Equals(list[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool compAssetStorageList(string[] list1, string[] list2)
    {
        if (list1 == null)
        {
            return (list2?.Length == 0);
        }
        if (list1.Length == 0)
        {
            return (list2?.Length == 0);
        }
        if (list2 == null)
        {
            return false;
        }
        if (list1.Length != list2.Length)
        {
            return false;
        }
        int length = list1.Length;
        for (int i = 0; i < length; i++)
        {
            if (list1[i] == null)
            {
                if (list2[i] != null)
                {
                    return false;
                }
            }
            else
            {
                if (list2[i] == null)
                {
                    return false;
                }
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected void ConfigWriteRequest(bool isFast)
    {
        if (this.assetBundleList.Count > 0)
        {
            StringBuilder builder = new StringBuilder(this.assetBundleList.Count * 0x22);
            builder.Append("@" + assetBundleMasterVersion);
            if (!string.IsNullOrEmpty(assetBundleDateVersion))
            {
                builder.Append("," + assetBundleDateVersion);
            }
            builder.Append("\n");
            for (int i = 0; i < this.assetBundleList.Count; i++)
            {
                AssetData data = this.assetBundleList[i];
                object[] args = new object[] { data.NowVersion, data.Attrib, data.Size, data.Crc, data.NewName };
                builder.AppendFormat("{0},{1},{2},{3},{4}\n", args);
            }
            string rquestConfigWriteData = this.rquestConfigWriteData;
            lock (rquestConfigWriteData)
            {
                string s = builder.ToString();
                uint num2 = Crc32.Compute(Encoding.UTF8.GetBytes(s));
                Debug.Log(string.Concat(new object[] { "CRC ConfigWriteRequest :[", s.Length, "] ", num2 }));
                object[] objArray3 = new object[] { "~", num2, "\n", s };
                string str = string.Concat(objArray3);
                this.rquestConfigWriteData = CryptData.TextEncrypt(str);
                if (isFast)
                {
                    this.requestWriteCounter = 0f;
                }
                else if (this.requestWriteCounter < 0f)
                {
                    this.requestWriteCounter = 1f;
                }
            }
        }
    }

    public static void debugLog()
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.DebugLog();
        }
    }

    public void DebugLog()
    {
        Debug.Log("AssetManager infomation");
        foreach (LoadWaitStatus status in this.downLoadWaitList)
        {
            AssetData info = status.Info;
            if (info != null)
            {
                Debug.Log("    Wait Load [" + info.Name + "]");
            }
        }
        foreach (AssetLoader loader in this.downLoadList)
        {
            Debug.Log("    Loading [" + loader.Name + "]");
        }
        foreach (AssetData data2 in this.assetBundleList)
        {
            if (!data2.IsEmpty)
            {
                Debug.Log(string.Concat(new object[] { "    Load Storage [", data2.Name, "] ", data2.EntryCount }));
            }
        }
        foreach (AssetData data3 in this.assetResourceList)
        {
            Debug.Log(string.Concat(new object[] { "    Load Resource [", data3.Name, "] ", data3.EntryCount }));
        }
    }

    public static bool downloadAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.DownloadAssetStorage(name, callbackFunc);
    }

    public static bool downloadAssetStorage(string[] nameList, System.Action callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.DownloadAssetStorage(nameList, callbackFunc);
    }

    public bool DownloadAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (name != null)
        {
            if (this.AddCallbackLoadStatus(AssetData.Type.ASSET_STORAGE, name, callbackFunc))
            {
                return true;
            }
            foreach (AssetData data in this.assetBundleList)
            {
                if (data.IsSame(name))
                {
                    if (data.IsNeedUpdateVersion())
                    {
                        this.AddLoadWaitStatus(data, callbackFunc);
                        Debug.Log("DownloadAssetStorage [" + name + "]");
                        this.LoadStart();
                    }
                    else if (callbackFunc != null)
                    {
                        callbackFunc(data);
                    }
                    return true;
                }
            }
            Debug.LogError("AssetStorageList not found [" + name + "]");
        }
        return false;
    }

    public bool DownloadAssetStorage(string[] nameList, System.Action callbackFunc)
    {
        int length = nameList.Length;
        bool flag = true;
        for (int i = 0; i < length; i++)
        {
            if (nameList[i] != null)
            {
                flag = flag && this.DownloadAssetStorage(nameList[i], null);
            }
        }
        this.AddLoadWaitStatus(callbackFunc);
        Debug.Log("DownloadAssetStorage");
        this.LoadStart();
        return flag;
    }

    public void DownloadAssetStorageAll()
    {
        if (this._DispLog)
        {
            Debug.Log("DownloadAssetStorageAll");
        }
        int count = this.assetBundleList.Count;
        for (int i = 0; i < count; i++)
        {
            AssetData info = this.assetBundleList[i];
            if (info.IsNeedUpdateVersion())
            {
                this.AddLoadWaitStatus(info);
            }
        }
        this.LoadStart();
    }

    public void DownloadAssetStorageAttribute(string attrib)
    {
        if (this._DispLog)
        {
            Debug.Log("DownloadAssetStorageAttribute [" + attrib + "]");
        }
        int count = this.assetBundleList.Count;
        for (int i = 0; i < count; i++)
        {
            AssetData info = this.assetBundleList[i];
            if (info.IsDownloadOldVersion())
            {
                this.AddLoadWaitStatus(info);
            }
            else if ((attrib == info.Attrib) && info.IsNeedUpdateVersion())
            {
                this.AddLoadWaitStatus(info);
            }
        }
        this.LoadStart();
    }

    public void DownloadAssetStorageAttribute(string[] attribList)
    {
        if (attribList.Length > 0)
        {
            if (attribList.Length == 1)
            {
                this.DownloadAssetStorageAttribute(attribList[0]);
            }
            else
            {
                string str = attribList[0];
                int length = attribList.Length;
                for (int i = 1; i < length; i++)
                {
                    str = str + ", " + attribList[i];
                }
                if (this._DispLog)
                {
                    Debug.Log("DownloadAssetStorageAttribute [" + str + "]");
                }
                int count = this.assetBundleList.Count;
                for (int j = 0; j < count; j++)
                {
                    AssetData info = this.assetBundleList[j];
                    if (info.IsDownloadOldVersion())
                    {
                        this.AddLoadWaitStatus(info);
                    }
                    else if (info.IsNeedUpdateVersion())
                    {
                        length = attribList.Length;
                        for (int k = 0; k < length; k++)
                        {
                            if (attribList[k] == info.Attrib)
                            {
                                this.AddLoadWaitStatus(info);
                                break;
                            }
                        }
                    }
                }
                this.LoadStart();
            }
        }
    }

    protected void EndDebuStatusFadeout()
    {
        this.debugStatusLabel.gameObject.SetActive(false);
        this.debugStatusLabel.text = string.Empty;
    }

    public static AssetData getAsset(AssetData.Type type, string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAsset(type, name);
    }

    public static bool getAsset(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAsset(name, callbackFunc);
    }

    public static bool getAsset(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAsset(type, name, callbackFunc);
    }

    public AssetData GetAsset(string name)
    {
        foreach (AssetData data in this.assetBundleList)
        {
            if (data.IsSame(name))
            {
                return data;
            }
        }
        foreach (AssetData data2 in this.assetResourceList)
        {
            if (data2.IsSame(name))
            {
                return data2;
            }
        }
        return null;
    }

    public AssetData GetAsset(AssetData.Type type, string name)
    {
        switch (type)
        {
            case AssetData.Type.ASSET_STORAGE:
                foreach (AssetData data in this.assetBundleList)
                {
                    if (data.IsSame(name))
                    {
                        return data;
                    }
                }
                break;

            case AssetData.Type.ASSET_RESOURCE:
                foreach (AssetData data2 in this.assetResourceList)
                {
                    if (data2.IsSame(name))
                    {
                        return data2;
                    }
                }
                break;
        }
        return null;
    }

    public bool GetAsset(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (!this.AddCallbackLoadStatus(AssetData.Type.ASSET_STORAGE, name, callbackFunc))
        {
            AssetData asset = this.GetAsset(AssetData.Type.ASSET_STORAGE, name);
            if (asset != null)
            {
                if (callbackFunc != null)
                {
                    callbackFunc(asset);
                }
                return true;
            }
            if (!this.AddCallbackLoadStatus(AssetData.Type.ASSET_RESOURCE, name, callbackFunc))
            {
                asset = this.GetAsset(AssetData.Type.ASSET_RESOURCE, name);
                if (asset == null)
                {
                    return false;
                }
                if (callbackFunc != null)
                {
                    callbackFunc(asset);
                }
            }
        }
        return true;
    }

    public bool GetAsset(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (!this.AddCallbackLoadStatus(type, name, callbackFunc))
        {
            AssetData asset = this.GetAsset(type, name);
            if (asset == null)
            {
                return false;
            }
            if (callbackFunc != null)
            {
                callbackFunc(asset);
            }
        }
        return true;
    }

    public static AssetData getAssetResource(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAssetResource(name);
    }

    public AssetData GetAssetResource(string name)
    {
        foreach (AssetData data in this.assetResourceList)
        {
            if (data.IsSame(name))
            {
                return (!data.IsEmpty ? data : null);
            }
        }
        return null;
    }

    public static AssetData getAssetStorage(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAssetStorage(name);
    }

    public static AssetData[] getAssetStorage(string[] nameList)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAssetStorage(nameList);
    }

    public static bool getAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAssetStorage(name, callbackFunc);
    }

    public AssetData GetAssetStorage(string name)
    {
        foreach (AssetData data in this.assetBundleList)
        {
            if (data.IsSame(name))
            {
                return (!data.IsEmpty ? data : null);
            }
        }
        return null;
    }

    public AssetData[] GetAssetStorage(string[] nameList)
    {
        int length = nameList.Length;
        AssetData[] dataArray = new AssetData[length];
        for (int i = 0; i < length; i++)
        {
            string name = nameList[i];
            if (name != null)
            {
                foreach (AssetData data in this.assetBundleList)
                {
                    if (data.IsSame(name))
                    {
                        if (!data.IsEmpty)
                        {
                            dataArray[i] = data;
                        }
                        break;
                    }
                }
            }
        }
        return dataArray;
    }

    public bool GetAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (name != null)
        {
            if (this.AddCallbackLoadStatus(AssetData.Type.ASSET_STORAGE, name, callbackFunc))
            {
                return true;
            }
            foreach (AssetData data in this.assetBundleList)
            {
                if (data.IsSame(name))
                {
                    if (data.IsEmpty)
                    {
                        return false;
                    }
                    if (callbackFunc != null)
                    {
                        callbackFunc(data);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public static string[] getAssetStorageList(string path)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetAssetStorageList(path);
    }

    public string[] GetAssetStorageList(string path)
    {
        string str = path + "/";
        int count = this.assetBundleList.Count;
        List<string> list = new List<string>();
        for (int i = 0; i < count; i++)
        {
            AssetData data = this.assetBundleList[i];
            if (data.Name.StartsWith(str))
            {
                list.Add(data.Name);
            }
        }
        return list.ToArray();
    }

    public static string GetDateVersion() => 
        assetBundleDateVersion;

    public bool GetDebugStatusOut() => 
        this.isOutDebugStatus;

    public static long getDownloadSize()
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.GetDownloadSize();
    }

    public long GetDownloadSize()
    {
        long downloadSize = this.downloadSize;
        foreach (AssetLoader loader in this.downLoadList)
        {
            downloadSize -= loader.LoadSize;
        }
        return downloadSize;
    }

    public static string GetMasterVersion() => 
        assetBundleMasterVersion;

    public static string getShaName(string name)
    {
        SHA1 sha = new SHA1CryptoServiceProvider();
        byte[] bytes = new UTF8Encoding().GetBytes(name);
        byte[] buffer2 = sha.ComputeHash(bytes);
        StringBuilder builder = new StringBuilder();
        foreach (byte num in buffer2)
        {
            builder.AppendFormat("{0,0:x2}", num ^ 170);
        }
        builder.Append(".bin");
        return builder.ToString();
    }

    public static string getUrlString(AssetData data)
    {
        string str2;
        string str = data.NewName.Replace('/', '@');
        if (data.GetExt() == null)
        {
            string str3 = getShaName(str + ".unity3d");
            str2 = DataServerAddress + str3;
        }
        else
        {
            str2 = DataServerAddress + str;
        }
        if (!str2.StartsWith("file://") && !str2.StartsWith("jar:file://"))
        {
            return str2;
        }
        if (data.GetExt() == null)
        {
            string str4 = string.Empty;
            if (str.Contains("%"))
            {
                char[] separator = new char[] { '%' };
                char[] chArray2 = new char[] { '%' };
                str4 = str.Split(separator)[0] + str.Split(chArray2)[2];
            }
            else
            {
                str4 = str;
            }
            return (DataServerAddress + getShaName(str4 + ".unity3d"));
        }
        return (DataServerAddress + str);
    }

    public static string getUrlString(string url)
    {
        if (!url.StartsWith("file://") && !url.StartsWith("jar:file://"))
        {
            long num = NetworkManager.getTime() / 300L;
            return (url + "?t=" + num);
        }
        return url;
    }

    public static string getUrlStringWithUnix(string url)
    {
        if (!url.StartsWith("file://") && !url.StartsWith("jar:file://"))
        {
            long num = NetworkManager.getTime();
            return (url + "?t=" + num);
        }
        return url;
    }

    [DebuggerHidden]
    private IEnumerator InitCR() => 
        new <InitCR>c__IteratorA { <>f__this = this };

    public void Initialize()
    {
        if (this.initCRW != null)
        {
            base.StopCoroutine(this.initCRW);
            this.initCRW = null;
        }
        this.assetBundleReleaseList.Clear();
        if (this.assetBundleList.Count > 0)
        {
            int count = this.assetBundleList.Count;
            for (int i = 0; i < count; i++)
            {
                this.assetBundleList[i].ReleaseData();
            }
            this.assetBundleList.Clear();
        }
        if (this.assetResourceList.Count > 0)
        {
            int num3 = this.assetResourceList.Count;
            for (int j = 0; j < num3; j++)
            {
                this.assetResourceList[j].ReleaseData();
            }
            this.assetResourceList.Clear();
        }
        if (this.downLoadList.Count > 0)
        {
            int num5 = this.downLoadList.Count;
            for (int k = 0; k < num5; k++)
            {
                AssetLoader loader = this.downLoadList[k];
                UnityEngine.Object.Destroy(loader);
            }
            this.downLoadList.Clear();
        }
        this.downLoadWaitList.Clear();
        this.downloadSize = 0L;
        this.isCancelDownload = false;
        cachePathName = AssetStorageCache.GetPath();
        cacheListFileName = cachePathName + configFileName;
        createCacheListFileName = cachePathName + createConfigFileName;
        backCacheListFileName = cachePathName + backConfigFileName;
        if (this._DispLog)
        {
            Debug.Log("AssetStorage cache path [" + cachePathName + "]");
        }
        if (this.isInitFirst)
        {
            this.isOutDebugStatus = this.isUseDebugStatus;
        }
        this.isInitFirst = false;
        char[] separator = new char[] { ',' };
        shieldingWord = Resources.Load<TextAsset>("Fonts/ShieldingWord").text.Split(separator);
    }

    public void InitializeAssetStorage()
    {
        if (this.initCRW == null)
        {
            this.isInitEnd = false;
            this.initCRW = this.InitCR();
            base.StartCoroutine(this.initCRW);
        }
    }

    public static bool isExistAssetStorage(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.IsExistAssetStorage(name);
    }

    public static bool isExistAssetStorage(string[] nameList)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.IsExistAssetStorage(nameList);
    }

    public bool IsExistAssetStorage(string name)
    {
        foreach (AssetData data in this.assetBundleList)
        {
            if (data.IsSame(name))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsExistAssetStorage(string[] nameList)
    {
        int length = nameList.Length;
        for (int i = 0; i < length; i++)
        {
            string name = nameList[i];
            if (name != null)
            {
                bool flag = false;
                foreach (AssetData data in this.assetBundleList)
                {
                    if (data.IsSame(name))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsInitializeAssetStorage() => 
        this.isInitEnd;

    public bool IsShieldingWord(string name)
    {
        bool flag = false;
        foreach (string str in shieldingWord)
        {
            if (str == name)
            {
                flag = true;
            }
        }
        return flag;
    }

    protected void LateUpdate()
    {
        foreach (AssetData data in this.assetBundleReleaseList)
        {
            if ((data.EntryCount == 0) && !data.IsEmpty)
            {
                data.ReleaseData();
            }
        }
        this.assetBundleReleaseList.Clear();
        if (this.requestWriteCounter >= 0f)
        {
            float deltaTime = Time.deltaTime;
            if (((this.requestWriteCounter -= deltaTime) <= 0f) && !this.isErrorDialog)
            {
                if (this._DispLog)
                {
                    Debug.Log("AssetManager: write cache list file [" + cacheListFileName + "]");
                }
                string message = null;
                string key = null;
                try
                {
                    long freeSize = CommonServicePluginScript.GetFreeSize(AssetStorageCache.GetPath());
                    if ((freeSize > 0L) && (freeSize < ManagerConfig.LIMIT_FREE_SIZE))
                    {
                        throw new IOException("Disk full");
                    }
                    if (File.Exists(createCacheListFileName))
                    {
                        File.Delete(createCacheListFileName);
                    }
                    StreamWriter writer = new StreamWriter(createCacheListFileName, false, Encoding.UTF8);
                    writer.Write(this.rquestConfigWriteData);
                    writer.Close();
                    if (File.Exists(backCacheListFileName))
                    {
                        File.Delete(backCacheListFileName);
                    }
                    if (File.Exists(cacheListFileName))
                    {
                        File.Move(cacheListFileName, backCacheListFileName);
                    }
                    File.Move(createCacheListFileName, cacheListFileName);
                    this.rquestConfigWriteData = string.Empty;
                    this.requestWriteCounter = -1f;
                }
                catch (IOException exception)
                {
                    Debug.LogError("error io " + exception.Message);
                    message = exception.Message;
                    if (message.StartsWith("Disk full"))
                    {
                        key = "NETWORK_ERROR_DISK_FULL";
                    }
                }
                catch (Exception exception2)
                {
                    Debug.LogError("error " + exception2.Message);
                    this.requestWriteCounter = 1f;
                    message = exception2.Message;
                }
                if (message != null)
                {
                    if (ManagerConfig.UseDebugCommand && (ManagerConfig.ServerDefaultType != "SCRIPT"))
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Download error for debug", message, null, false);
                    }
                    this.isErrorDialog = true;
                    if (key != null)
                    {
                        if (ManagementManager.IsDuringStartup)
                        {
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryBootDialog(string.Empty, LocalizationManager.Get(key), new ErrorDialog.ClickDelegate(this.OnClickRetryDialog), false);
                        }
                        else
                        {
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get(key), new ErrorDialog.ClickDelegate(this.OnClickRetryDialog), false);
                        }
                    }
                    else if (ManagementManager.IsDuringStartup)
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryBootDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_BOOT_RETRY_MESSAGE"), new ErrorDialog.ClickDelegate(this.OnClickRetryDialog), false);
                    }
                    else
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE"), new ErrorDialog.ClickDelegate(this.OnClickRetryDialog), false);
                    }
                }
                if (this._DispLog)
                {
                    Debug.Log("AssetManager: end write cache list file");
                }
            }
        }
        if (this.requestDebugStatus != null)
        {
            this.debugStatusLabel.gameObject.SetActive(true);
            this.debugStatusLabel.text = this.requestDebugStatus;
            this.requestDebugStatus = null;
            TweenAlpha component = this.debugStatusLabel.gameObject.GetComponent<TweenAlpha>();
            if ((component != null) && component.enabled)
            {
                component.enabled = false;
            }
            this.debugStatusLabel.alpha = 1f;
        }
        else if (this.isRequestDebugStatusClear)
        {
            this.isRequestDebugStatusClear = false;
            TweenAlpha alpha2 = TweenAlpha.Begin(this.debugStatusLabel.gameObject, 1f, 0f);
            alpha2.eventReceiver = base.gameObject;
            alpha2.callWhenFinished = "EndDebuStatusFadeout";
        }
    }

    public static bool loadAsset(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.LoadAsset(type, name, callbackFunc);
    }

    public bool LoadAsset(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (!this.LoadAssetStorage(name, callbackFunc))
        {
            AssetData data = this.LoadAssetResource(name);
            if (data == null)
            {
                return false;
            }
            if (callbackFunc != null)
            {
                callbackFunc(data);
            }
        }
        return true;
    }

    public bool LoadAsset(AssetData.Type type, string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetData.Type type2 = type;
        if (type2 != AssetData.Type.ASSET_STORAGE)
        {
            if (type2 == AssetData.Type.ASSET_RESOURCE)
            {
                AssetData data = this.LoadAssetResource(name);
                if (data == null)
                {
                    return false;
                }
                if (callbackFunc != null)
                {
                    callbackFunc(data);
                }
                return true;
            }
            Debug.LogError("aseet type error [" + type + "]");
            return false;
        }
        return this.LoadAssetStorage(name, callbackFunc);
    }

    public static AssetData loadAssetResource(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.LoadAssetResource(name);
    }

    public AssetData LoadAssetResource(string name)
    {
        foreach (AssetData data in this.assetResourceList)
        {
            if (data.IsSame(name))
            {
                data.AddEntry();
                return data;
            }
        }
        AssetData item = new AssetData(AssetData.Type.ASSET_RESOURCE, name);
        if (item.SetResource())
        {
            item.AddEntry();
            this.assetResourceList.Add(item);
            return item;
        }
        return null;
    }

    public static bool loadAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.LoadAssetStorage(name, callbackFunc);
    }

    public static bool loadAssetStorage(string[] nameList, System.Action callbackFunc)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        return instance?.LoadAssetStorage(nameList, callbackFunc);
    }

    public bool LoadAssetStorage(string name, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        if (name != null)
        {
            foreach (AssetData data in this.assetBundleReleaseList)
            {
                if (data.IsSame(name))
                {
                    this.assetBundleReleaseList.Remove(data);
                    if (data.IsEmpty)
                    {
                        break;
                    }
                    data.AddEntry();
                    if (callbackFunc != null)
                    {
                        callbackFunc(data);
                    }
                    return true;
                }
            }
            if (this.AddEntryLoadStatus(AssetData.Type.ASSET_STORAGE, name, callbackFunc))
            {
                return true;
            }
            foreach (AssetData data2 in this.assetBundleList)
            {
                if (data2.IsSame(name))
                {
                    if (data2.IsEmpty)
                    {
                        data2.AddEntry();
                        this.AddLoadWaitStatus(data2, callbackFunc);
                        Debug.Log("LoadAssetStorage [" + name + "]");
                        this.LoadStart();
                    }
                    else
                    {
                        data2.AddEntry();
                        if (callbackFunc != null)
                        {
                            callbackFunc(data2);
                        }
                    }
                    return true;
                }
            }
            Debug.LogError("AssetStorageList not found [" + name + "]");
        }
        return false;
    }

    public bool LoadAssetStorage(string[] nameList, System.Action callbackFunc)
    {
        int length = nameList.Length;
        bool flag = true;
        for (int i = 0; i < length; i++)
        {
            if (nameList[i] != null)
            {
                flag = flag && this.LoadAssetStorage(nameList[i], null);
            }
        }
        this.AddLoadWaitStatus(callbackFunc);
        Debug.Log("LoadAssetStorage");
        this.LoadStart();
        return flag;
    }

    public static bool LoadIsBusy() => 
        (!SingletonMonoBehaviour<AssetManager>.Instance.isInitEnd || (SingletonMonoBehaviour<AssetManager>.Instance.downLoadList.Count > 0));

    protected bool LoadStart()
    {
        if (NetworkManager.IsRebootBlock)
        {
            Debug.Log("LoadStart: reboot block");
            return false;
        }
        if (this.isPauseDownload)
        {
            Debug.Log("LoadStart: pause download");
            return false;
        }
        if (this._DispLog)
        {
            Debug.Log("LoadStart: downLoadList " + this.downLoadList.Count);
        }
        if (this._DispLog)
        {
            foreach (AssetLoader loader in this.downLoadList)
            {
                Debug.Log("    Loading [" + loader.Name + "]");
            }
        }
        if (this.downLoadWaitList.Count <= 0)
        {
            if (this._DispLog)
            {
                Debug.Log("LoadStart: downLoadWaitListCount " + this.downLoadWaitList.Count);
            }
            return false;
        }
        if (this.downLoadList.Count > 0)
        {
            if (this._DispLog)
            {
                Debug.Log("LoadStart: downLoadList " + this.downLoadList.Count);
            }
            return false;
        }
        LoadWaitStatus status = this.downLoadWaitList.Dequeue();
        if (!string.IsNullOrEmpty(status.Name))
        {
            AssetLoader item = base.gameObject.AddComponent<AssetLoader>();
            if (this.isOutDebugStatus)
            {
                bool flag = status.Info.IsNeedUpdateVersion();
                this.requestDebugStatus = "Loading (" + (!flag ? "[E0E0FF]" : "[FFFF40]") + status.Name + "[-])";
                this.isRequestDebugStatusClear = false;
            }
            this.downLoadList.Add(item);
            item.Init(status.Info);
            item.AddCallback(status.DataCallbackFunc);
            item.StartLoad(new AssetLoader.LoadEndHandler(this.OnEndLoadAssetStorage));
            if (this._DispLog)
            {
                Debug.Log("LoadStart: assetBundleFirstLoadStatus");
            }
            return true;
        }
        if (status.CallbackFunc != null)
        {
            status.CallbackFunc();
        }
        if (this._DispLog)
        {
            Debug.Log("LoadStart: assetBundleFirstLoadStatus wait empty");
        }
        return this.LoadStart();
    }

    protected void OnClickRetryDialog(bool isDecide)
    {
        Debug.Log("OnClickRetryDialog " + isDecide);
        if (isDecide)
        {
            this.isErrorDialog = false;
        }
        else if (ManagementManager.IsDuringStartup)
        {
            Application.Quit();
        }
        else
        {
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void OnClickRetryScriptDialog(bool isDecide)
    {
        Debug.Log("OnClickRetryScriptDialog " + isDecide);
        this.isEnforceBoot = !isDecide;
        this.isErrorDialog = false;
    }

    protected void OnClickWaitDebugDialog(bool isDecide)
    {
        this.isErrorDialog = false;
    }

    protected void OnEndLoadAssetStorage(AssetLoader loader)
    {
        if (this._DispLog)
        {
            Debug.Log("OnEndLoad: " + loader.Name);
        }
        bool isRequestDownload = loader.IsRequestDownload;
        this.isRequestDebugStatusClear = true;
        int num = -1;
        for (int i = 0; i < this.downLoadList.Count; i++)
        {
            AssetLoader loader2 = this.downLoadList[i];
            if (loader2 == loader)
            {
                num = i;
                this.downloadSize -= loader.Size;
                this.downLoadList.RemoveAt(i);
                break;
            }
        }
        if (this._DispLog)
        {
            Debug.Log(string.Concat(new object[] { "OnEndLoadAssetStorage [", loader.Name, "] ", num }));
        }
        if (isRequestDownload)
        {
            this.ConfigWriteRequest(false);
        }
        this.LoadStart();
    }

    public static void pauseDownloadAssetStorage()
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.PauseDownloadAssetStorage();
        }
    }

    public void PauseDownloadAssetStorage()
    {
        if (!this.isPauseDownload)
        {
            this.isPauseDownload = true;
        }
    }

    public static void releaseAsset(AssetData assetInfo)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAsset(assetInfo);
        }
    }

    public static void releaseAsset(AssetData[] assetInfoList)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAsset(assetInfoList);
        }
    }

    public static void releaseAsset(AssetData.Type type, string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAsset(type, name);
        }
    }

    public static void releaseAsset(AssetData.Type type, string[] nameList)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAsset(type, nameList);
        }
    }

    public void ReleaseAsset(AssetData assetInfo)
    {
        if (assetInfo != null)
        {
            switch (assetInfo.DataType)
            {
                case AssetData.Type.ASSET_STORAGE:
                {
                    int count = this.assetBundleList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (assetInfo == this.assetBundleList[i])
                        {
                            assetInfo.RemoveEntry();
                            return;
                        }
                    }
                    break;
                }
                case AssetData.Type.ASSET_RESOURCE:
                {
                    int num3 = this.assetResourceList.Count;
                    for (int j = 0; j < num3; j++)
                    {
                        if (assetInfo == this.assetResourceList[j])
                        {
                            if (assetInfo.RemoveEntry())
                            {
                                this.assetResourceList.RemoveAt(j);
                            }
                            return;
                        }
                    }
                    break;
                }
            }
        }
    }

    public void ReleaseAsset(AssetData[] assetInfoList)
    {
        foreach (AssetData data in assetInfoList)
        {
            this.ReleaseAsset(data);
        }
    }

    public void ReleaseAsset(AssetData.Type type, string name)
    {
        switch (type)
        {
            case AssetData.Type.ASSET_STORAGE:
                for (int i = 0; i < this.assetBundleList.Count; i++)
                {
                    AssetData data = this.assetBundleList[i];
                    if (data.IsSame(type, name))
                    {
                        data.RemoveEntry();
                        return;
                    }
                }
                break;

            case AssetData.Type.ASSET_RESOURCE:
                for (int j = 0; j < this.assetResourceList.Count; j++)
                {
                    AssetData data2 = this.assetResourceList[j];
                    if (data2.IsSame(type, name))
                    {
                        if (data2.RemoveEntry())
                        {
                            this.assetResourceList.RemoveAt(j);
                        }
                        return;
                    }
                }
                break;
        }
    }

    public void ReleaseAsset(AssetData.Type type, string[] nameList)
    {
        foreach (string str in nameList)
        {
            this.ReleaseAsset(type, str);
        }
    }

    public static void releaseAssetResource(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAssetResource(name);
        }
    }

    public void ReleaseAssetResource(string name)
    {
        int count = this.assetResourceList.Count;
        for (int i = 0; i < count; i++)
        {
            AssetData data = this.assetResourceList[i];
            if (data.IsSame(name))
            {
                if (data.RemoveEntry())
                {
                    this.assetResourceList.RemoveAt(i);
                }
                return;
            }
        }
    }

    public static void releaseAssetStorage(string name)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAssetStorage(name);
        }
    }

    public static void releaseAssetStorage(string[] nameList)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseAssetStorage(nameList);
        }
    }

    public void ReleaseAssetStorage(string name)
    {
        if (name != null)
        {
            int count = this.assetBundleList.Count;
            for (int i = 0; i < count; i++)
            {
                AssetData data = this.assetBundleList[i];
                if (data.IsSame(name))
                {
                    data.RemoveEntry();
                    return;
                }
            }
        }
    }

    public void ReleaseAssetStorage(string[] nameList)
    {
        foreach (string str in nameList)
        {
            if (str != null)
            {
                int count = this.assetBundleList.Count;
                for (int i = 0; i < count; i++)
                {
                    AssetData data = this.assetBundleList[i];
                    if (data.IsSame(str))
                    {
                        data.RemoveEntry();
                        break;
                    }
                }
            }
        }
    }

    public void ReleaseReservation(AssetData info)
    {
        if ((info.EntryCount <= 0) && !info.IsEmpty)
        {
            foreach (AssetData data in this.assetBundleReleaseList)
            {
                if (data == info)
                {
                    return;
                }
            }
            this.assetBundleReleaseList.Add(info);
        }
    }

    public static void resetAssetStorageVersion(string path)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ResetAssetStorageVersion(path);
        }
    }

    public void ResetAssetStorageVersion(string path)
    {
        string str = path + "/";
        int count = this.assetBundleList.Count;
        for (int i = 0; i < count; i++)
        {
            AssetData data = this.assetBundleList[i];
            if (data.Name.StartsWith(str))
            {
                data.ResetVersion();
            }
        }
    }

    public static void resumeDownloadAssetStorage()
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.ResumeDownloadAssetStorage();
        }
    }

    public void ResumeDownloadAssetStorage()
    {
        if (this.isPauseDownload)
        {
            this.isPauseDownload = false;
            this.LoadStart();
        }
    }

    public void setDebugStatusOut(bool isUse)
    {
        AssetManager instance = SingletonMonoBehaviour<AssetManager>.Instance;
        if (instance != null)
        {
            instance.SetDebugStatusOut(isUse);
        }
    }

    public void SetDebugStatusOut(bool isUse)
    {
        this.isOutDebugStatus = isUse;
    }

    public static void SetOfflineStatus()
    {
        PlayerPrefs.SetString("Asset", "offline");
        PlayerPrefs.Save();
    }

    public static bool SetOnlineStatus()
    {
        if (PlayerPrefs.GetString("Asset", "offline") == "online")
        {
            return false;
        }
        PlayerPrefs.SetString("Asset", "online");
        PlayerPrefs.Save();
        return true;
    }

    public bool SwitchingDebugStatusOut()
    {
        this.isOutDebugStatus = !this.isOutDebugStatus;
        return this.isOutDebugStatus;
    }

    public static string AssetStorageFileAddress =>
        (ManagerConfig.DevelopGameServerAddress + "rgfate/60_member/network/" + configFileName);

    public static string CacheListFile =>
        (AssetStorageCache.GetPath() + configFileName);

    public static string CachePathName =>
        cachePathName;

    public static string ConfigFileAddress =>
        (DataServerAddress + configFileName);

    public static string DataServerAddress =>
        NetworkManager.getDataUrl();

    public bool DispLog =>
        this._DispLog;

    public static bool IsOnline
    {
        get
        {
            Debug.Log("AssetManager: check online " + (!ManagerConfig.UseStandaloneAsset ? PlayerPrefs.GetString("Asset", "offline") : "Standalone"));
            return (PlayerPrefs.GetString("Asset", "offline") == "online");
        }
    }

    [CompilerGenerated]
    private sealed class <InitCR>c__IteratorA : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<AssetData>.Enumerator <$s_162>__45;
        internal AssetManager <>f__this;
        internal AssetData <assetInfo>__46;
        internal string <attrib>__18;
        internal string <attrib>__40;
        internal string <configFileUrl>__23;
        internal uint <crc>__13;
        internal uint <crc>__20;
        internal uint <crc>__32;
        internal uint <crc>__42;
        internal string <crcString>__11;
        internal string <crcString>__30;
        internal string <dataTxT>__7;
        internal WWW <dataTxTWWW>__9;
        internal Exception <e>__14;
        internal Exception <e>__8;
        internal string <errorCode>__28;
        internal string <errorCode>__4;
        internal int <i>__16;
        internal int <i>__35;
        internal bool <isLocalPath>__1;
        internal bool <isOnline>__0;
        internal string[] <lineData>__15;
        internal string[] <lineData>__36;
        internal string[] <listData>__3;
        internal string[] <listData>__33;
        internal string <loadData>__2;
        internal string <loadData>__25;
        internal string <loadDateVersion>__38;
        internal WWW <loader>__24;
        internal string <loadMasterVersion>__37;
        internal float <loadProgress>__27;
        internal AssetDataListInfo <margeInfo>__34;
        internal string <name>__21;
        internal string <name>__43;
        internal AssetData <newAssetInfo>__22;
        internal AssetData <newAssetInfo>__47;
        internal string <newname>__44;
        internal byte[] <readData>__12;
        internal byte[] <readData>__31;
        internal string <readPath>__6;
        internal float <requestTime>__26;
        internal int <retryCount>__5;
        internal int <ri>__10;
        internal int <ri>__29;
        internal int <size>__19;
        internal int <size>__41;
        internal int <version>__17;
        internal int <version>__39;

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
                    if (this.<>f__this._DispLog)
                    {
                        Debug.Log("AssetManager: LoadCacheListCR()");
                    }
                    if (!Directory.Exists(AssetManager.cachePathName))
                    {
                        Directory.CreateDirectory(AssetManager.cachePathName);
                    }
                    this.<isOnline>__0 = AssetManager.IsOnline;
                    this.<isLocalPath>__1 = false;
                    AssetManager.cacheTxTListFileName = Application.streamingAssetsPath + "/AssetStorages/Android/AssetStorage.txt";
                    if (File.Exists(AssetManager.cacheListFileName))
                    {
                        this.<isLocalPath>__1 = true;
                    }
                    else
                    {
                        this.<isLocalPath>__1 = true;
                    }
                    Debug.LogError(this.<isLocalPath>__1);
                    if ((this.<>f__this.assetBundleList.Count != 0) || !this.<isLocalPath>__1)
                    {
                        goto Label_06FA;
                    }
                    if (this.<>f__this._DispLog)
                    {
                        Debug.Log("AssetManager load cache config [" + AssetManager.cacheListFileName + "]");
                    }
                    this.<loadData>__2 = null;
                    this.<listData>__3 = null;
                    this.<errorCode>__4 = null;
                    this.<retryCount>__5 = 3;
                    this.<readPath>__6 = AssetManager.cacheListFileName;
                    if (!File.Exists(AssetManager.cacheListFileName))
                    {
                        this.<readPath>__6 = AssetManager.cacheTxTListFileName;
                    }
                    Debug.LogError(this.<readPath>__6);
                    while (this.<retryCount>__5-- > 0)
                    {
                        if (File.Exists(AssetManager.cacheListFileName))
                        {
                            try
                            {
                                this.<errorCode>__4 = null;
                                this.<dataTxT>__7 = File.ReadAllText(this.<readPath>__6);
                                this.<loadData>__2 = CryptData.TextDecrypt(this.<dataTxT>__7);
                                break;
                            }
                            catch (Exception exception)
                            {
                                this.<e>__8 = exception;
                                Debug.LogError("error " + this.<e>__8.Message);
                                this.<loadData>__2 = null;
                                this.<errorCode>__4 = this.<e>__8.Message;
                            }
                            continue;
                        }
                        this.<dataTxTWWW>__9 = new WWW(this.<readPath>__6);
                        this.$current = this.<dataTxTWWW>__9;
                        this.$PC = 1;
                        goto Label_111C;
                    Label_024D:
                        Debug.LogError("error " + this.<dataTxTWWW>__9.error);
                        this.<loadData>__2 = null;
                        this.<errorCode>__4 = this.<dataTxTWWW>__9.error;
                    }
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(this.<dataTxTWWW>__9.error))
                    {
                        goto Label_024D;
                    }
                    this.<errorCode>__4 = null;
                    this.<loadData>__2 = CryptData.TextDecrypt(this.<dataTxTWWW>__9.text);
                    break;

                case 2:
                    goto Label_06DE;

                case 3:
                    goto Label_0835;

                case 4:
                    goto Label_0AFC;

                case 5:
                    goto Label_0BB1;

                case 6:
                    goto Label_0C13;

                case 7:
                    goto Label_0C66;

                case 8:
                    goto Label_0747;

                case 9:
                    this.<>f__this.isCancelDownload = false;
                    this.<>f__this.isInitEnd = true;
                    this.<>f__this.initCRW = null;
                    this.$PC = -1;
                    goto Label_111A;

                default:
                    goto Label_111A;
            }
            if (this.<loadData>__2 != null)
            {
                try
                {
                    char[] trimChars = new char[] { 0xfeff };
                    this.<loadData>__2 = this.<loadData>__2.Trim(trimChars);
                    char[] anyOf = new char[] { '\r', '\n' };
                    this.<ri>__10 = this.<loadData>__2.IndexOfAny(anyOf);
                    if (this.<ri>__10 > 1)
                    {
                        this.<crcString>__11 = this.<loadData>__2.Substring(0, this.<ri>__10);
                        if (this.<crcString>__11.StartsWith("~"))
                        {
                            this.<crcString>__11 = this.<crcString>__11.Substring(1);
                            this.<loadData>__2 = this.<loadData>__2.Substring(this.<ri>__10 + 1);
                            this.<readData>__12 = Encoding.UTF8.GetBytes(this.<loadData>__2);
                            this.<crc>__13 = Crc32.Compute(this.<readData>__12);
                            if (this.<>f__this._DispLog)
                            {
                                Debug.Log(string.Concat(new object[] { "CRC (cache) :[", this.<loadData>__2.Length, "] ", this.<crcString>__11, " ", this.<crc>__13 }));
                            }
                            if (uint.Parse(this.<crcString>__11) == this.<crc>__13)
                            {
                                char[] separator = new char[] { '\r', '\n' };
                                this.<listData>__3 = this.<loadData>__2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else
                            {
                                this.<errorCode>__4 = "AssetStorage boot load crc error : チェックサム値が不一致";
                            }
                        }
                        else
                        {
                            this.<errorCode>__4 = "AssetStorage boot load error : 読み込んだファイルの先頭がチェックサムデータではなかった";
                        }
                    }
                    else
                    {
                        this.<errorCode>__4 = "AssetStorage boot load error : ファイル先頭の１行目の内容が空";
                    }
                }
                catch (Exception exception2)
                {
                    this.<e>__14 = exception2;
                    this.<errorCode>__4 = "AssetStorage boot load error : " + this.<e>__14.Message;
                }
            }
            if (this.<listData>__3 != null)
            {
                if ((this.<listData>__3.Length > 0) && this.<listData>__3[0].StartsWith("@"))
                {
                    char[] chArray4 = new char[] { ',' };
                    this.<lineData>__15 = this.<listData>__3[0].Split(chArray4);
                    AssetManager.assetBundleMasterVersion = this.<lineData>__15[0].Substring(1);
                    AssetManager.assetBundleDateVersion = (this.<lineData>__15.Length <= 1) ? string.Empty : this.<lineData>__15[1];
                    this.<i>__16 = 1;
                    while (this.<i>__16 < this.<listData>__3.Length)
                    {
                        char[] chArray5 = new char[] { ',' };
                        this.<lineData>__15 = this.<listData>__3[this.<i>__16].Split(chArray5);
                        if (this.<lineData>__15.Length != 5)
                        {
                            break;
                        }
                        this.<version>__17 = int.Parse(this.<lineData>__15[0].Trim());
                        this.<attrib>__18 = this.<lineData>__15[1];
                        this.<size>__19 = int.Parse(this.<lineData>__15[2].Trim());
                        this.<crc>__20 = uint.Parse(this.<lineData>__15[3].Trim());
                        this.<name>__21 = this.<lineData>__15[4];
                        this.<newAssetInfo>__22 = null;
                        if (!this.<isOnline>__0)
                        {
                            this.<newAssetInfo>__22 = new AssetData(AssetData.Type.ASSET_STORAGE, this.<name>__21, 0, this.<version>__17, this.<attrib>__18, this.<size>__19, this.<crc>__20);
                        }
                        else
                        {
                            this.<newAssetInfo>__22 = new AssetData(AssetData.Type.ASSET_STORAGE, this.<name>__21, this.<version>__17, this.<attrib>__18, this.<size>__19, this.<crc>__20);
                        }
                        this.<>f__this.assetBundleList.Add(this.<newAssetInfo>__22);
                        this.<i>__16++;
                    }
                }
                else
                {
                    this.<errorCode>__4 = "AssetStorage boot load error : アセットバンドルリスト作成失敗";
                }
            }
            if (this.<errorCode>__4 == null)
            {
                goto Label_06EE;
            }
            AssetManager.assetBundleMasterVersion = string.Empty;
            AssetManager.assetBundleDateVersion = string.Empty;
            this.<>f__this.assetBundleList.Clear();
            if (!ManagerConfig.UseDebugCommand || (ManagerConfig.ServerDefaultType == "SCRIPT"))
            {
                goto Label_06EE;
            }
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Download error for debug", this.<errorCode>__4, new ErrorDialog.ClickDelegate(this.<>f__this.OnClickWaitDebugDialog), true);
        Label_06DE:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 2;
                goto Label_111C;
            }
        Label_06EE:
            this.<>f__this.ConfigWriteRequest(true);
        Label_06FA:
            if (!AssetManager.IsOnline)
            {
                goto Label_10D7;
            }
            this.<configFileUrl>__23 = AssetManager.AssetStorageFileAddress;
            if (this.<>f__this._DispLog)
            {
                Debug.Log("AssetManager down load config [" + this.<configFileUrl>__23 + "]");
            }
            this.<loader>__24 = null;
            this.<loadData>__25 = null;
        Label_0747:
            if (this.<>f__this._DispLog)
            {
                Debug.Log("AssetManager config load start [" + this.<configFileUrl>__23 + "]");
            }
            Debug.LogError("connect ::: " + AssetManager.getUrlStringWithUnix(this.<configFileUrl>__23));
            this.<loader>__24 = new WWW(AssetManager.getUrlStringWithUnix(this.<configFileUrl>__23));
            this.<requestTime>__26 = Time.time + ManagerConfig.TIMEOUT;
            this.<loadProgress>__27 = 0f;
        Label_0835:
            while (!this.<loader>__24.isDone)
            {
                if (this.<loader>__24.progress != this.<loadProgress>__27)
                {
                    this.<requestTime>__26 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__27 = this.<loader>__24.progress;
                }
                else if (Time.time >= this.<requestTime>__26)
                {
                    Debug.LogWarning("TimeOut");
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 3;
                goto Label_111C;
            }
            if (this.<>f__this._DispLog)
            {
                Debug.Log("AssetStorageLoad is Done? " + this.<loader>__24.isDone);
            }
            this.<errorCode>__28 = null;
            if (!this.<loader>__24.isDone)
            {
                this.<errorCode>__28 = "AssetStorageList download time over";
                Debug.LogError(this.<errorCode>__28);
            }
            else if (!string.IsNullOrEmpty(this.<loader>__24.error))
            {
                this.<errorCode>__28 = this.<loader>__24.error;
                Debug.LogError(this.<errorCode>__28);
            }
            else
            {
                this.<loadData>__25 = CryptData.TextDecrypt(this.<loader>__24.text);
                if (string.IsNullOrEmpty(this.<loadData>__25))
                {
                    this.<errorCode>__28 = "AssetStorageList download decrypt error";
                }
                else
                {
                    char[] chArray6 = new char[] { 0xfeff };
                    this.<loadData>__25 = this.<loadData>__25.Trim(chArray6);
                    if (this.<loadData>__25.StartsWith("~"))
                    {
                        char[] chArray7 = new char[] { '\r', '\n' };
                        this.<ri>__29 = this.<loadData>__25.IndexOfAny(chArray7);
                        if (this.<ri>__29 > 1)
                        {
                            this.<crcString>__30 = this.<loadData>__25.Substring(1, this.<ri>__29 - 1);
                            this.<loadData>__25 = this.<loadData>__25.Substring(this.<ri>__29 + 1);
                            this.<readData>__31 = Encoding.UTF8.GetBytes(this.<loadData>__25);
                            this.<crc>__32 = Crc32.Compute(this.<readData>__31);
                            if (this.<>f__this._DispLog)
                            {
                                Debug.Log(string.Concat(new object[] { "CRC (server) :[", this.<loadData>__25.Length, "] ", this.<crcString>__30, " ", this.<crc>__32 }));
                            }
                            if (uint.Parse(this.<crcString>__30) == this.<crc>__32)
                            {
                                goto Label_0CAC;
                            }
                            this.<errorCode>__28 = "AssetStorageList download error";
                        }
                    }
                    this.<errorCode>__28 = "AssetStorageList download data error";
                }
            }
            if (this.<loader>__24 != null)
            {
                this.<loader>__24.Dispose();
                this.<loader>__24 = null;
            }
            this.<loadData>__25 = null;
            if (this.<errorCode>__28 == null)
            {
                goto Label_0C8B;
            }
            if (!ManagerConfig.UseDebugCommand || (ManagerConfig.ServerDefaultType == "SCRIPT"))
            {
                goto Label_0B0C;
            }
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Download error for debug", this.<errorCode>__28, new ErrorDialog.ClickDelegate(this.<>f__this.OnClickWaitDebugDialog), false);
        Label_0AFC:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 4;
                goto Label_111C;
            }
        Label_0B0C:
            this.<>f__this.isErrorDialog = true;
            this.<>f__this.isEnforceBoot = false;
            if (ManagerConfig.ServerDefaultType == "SCRIPT")
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_BOOT_SCRIPT_ASSET_MESSAGE"), LocalizationManager.Get("NETWORK_ERROR_BOOT_SCRIPT_ASSET_DECIDE"), LocalizationManager.Get("NETWORK_ERROR_BOOT_SCRIPT_ASSET_CANCEL"), new ErrorDialog.ClickDelegate(this.<>f__this.OnClickRetryScriptDialog), false);
                goto Label_0C66;
            }
            if (!ManagementManager.IsDuringStartup)
            {
                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("04");
                goto Label_0C13;
            }
            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewNetwork("04");
        Label_0BB1:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 5;
                goto Label_111C;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.InitCR());
            goto Label_111A;
        Label_0C13:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 6;
                goto Label_111C;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.InitCR());
            goto Label_111A;
        Label_0C66:
            if (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 7;
                goto Label_111C;
            }
            if (this.<>f__this.isEnforceBoot)
            {
                goto Label_0CAC;
            }
        Label_0C8B:
            this.$current = new WaitForSeconds(1f);
            this.$PC = 8;
            goto Label_111C;
        Label_0CAC:
            if (this.<loadData>__25 != null)
            {
                char[] chArray8 = new char[] { '\r', '\n' };
                this.<listData>__33 = this.<loadData>__25.Split(chArray8, StringSplitOptions.RemoveEmptyEntries);
                this.<margeInfo>__34 = new AssetDataListInfo();
                this.<i>__35 = 0;
                while (this.<i>__35 < this.<listData>__33.Length)
                {
                    char[] chArray9 = new char[] { ',' };
                    this.<lineData>__36 = this.<listData>__33[this.<i>__35].Split(chArray9);
                    if (this.<lineData>__36.Length >= 1)
                    {
                        if (this.<lineData>__36[0].StartsWith("@"))
                        {
                            this.<loadMasterVersion>__37 = this.<lineData>__36[0].Substring(1);
                            this.<loadDateVersion>__38 = (this.<lineData>__36.Length <= 1) ? string.Empty : this.<lineData>__36[1];
                            this.<margeInfo>__34.masterVersion = this.<loadMasterVersion>__37;
                            this.<margeInfo>__34.dateVersion = this.<loadDateVersion>__38;
                            goto Label_1074;
                        }
                        if (this.<lineData>__36[0].StartsWith("~") || (this.<lineData>__36[0].IndexOf('~') == 1))
                        {
                            goto Label_1074;
                        }
                    }
                    if (this.<lineData>__36.Length != 5)
                    {
                        break;
                    }
                    this.<version>__39 = int.Parse(this.<lineData>__36[0].Trim());
                    this.<attrib>__40 = this.<lineData>__36[1];
                    this.<size>__41 = int.Parse(this.<lineData>__36[2].Trim());
                    this.<crc>__42 = uint.Parse(this.<lineData>__36[3].Trim());
                    this.<name>__43 = this.<lineData>__36[4];
                    this.<newname>__44 = string.Empty;
                    if (this.<lineData>__36[4].Contains("%"))
                    {
                        char[] chArray10 = new char[] { '%' };
                        char[] chArray11 = new char[] { '%' };
                        this.<newname>__44 = this.<lineData>__36[4].Split(chArray10)[0] + this.<lineData>__36[4].Split(chArray11)[2];
                    }
                    else
                    {
                        this.<newname>__44 = this.<lineData>__36[4];
                    }
                    this.<$s_162>__45 = this.<>f__this.assetBundleList.GetEnumerator();
                    try
                    {
                        while (this.<$s_162>__45.MoveNext())
                        {
                            this.<assetInfo>__46 = this.<$s_162>__45.Current;
                            if (this.<assetInfo>__46.IsSame(this.<newname>__44))
                            {
                                if (this.<>f__this._DispLog)
                                {
                                    Debug.Log(string.Concat(new object[] { "    [", this.<version>__39, ",", this.<name>__43, "] update" }));
                                }
                                this.<assetInfo>__46.NewName = this.<name>__43;
                                this.<assetInfo>__46.Name = this.<newname>__44;
                                this.<assetInfo>__46.SetUpdateInfo(this.<version>__39, this.<attrib>__40, this.<size>__41, this.<crc>__42);
                                this.<margeInfo>__34.dataList.Add(this.<assetInfo>__46);
                                this.<name>__43 = null;
                                goto Label_0FD4;
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_162>__45.Dispose();
                    }
                Label_0FD4:
                    if (!string.IsNullOrEmpty(this.<name>__43))
                    {
                        if (this.<>f__this._DispLog)
                        {
                            Debug.Log(string.Concat(new object[] { "    [", this.<version>__39, ",", this.<name>__43, "] new" }));
                        }
                        this.<newAssetInfo>__47 = new AssetData(AssetData.Type.ASSET_STORAGE, this.<name>__43, 0, this.<version>__39, this.<attrib>__40, this.<size>__41, this.<crc>__42);
                        this.<margeInfo>__34.dataList.Add(this.<newAssetInfo>__47);
                    }
                Label_1074:
                    this.<i>__35++;
                }
                AssetManager.assetBundleMasterVersion = this.<margeInfo>__34.masterVersion;
                AssetManager.assetBundleDateVersion = this.<margeInfo>__34.dateVersion;
                this.<>f__this.assetBundleList = this.<margeInfo>__34.dataList;
                this.<>f__this.ConfigWriteRequest(true);
            }
        Label_10D7:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 9;
            goto Label_111C;
        Label_111A:
            return false;
        Label_111C:
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

    protected class LoadWaitStatus
    {
        protected System.Action callbackFunc;
        protected AssetLoader.LoadEndDataHandler callbackFunc2;
        protected AssetData data;

        public LoadWaitStatus(AssetData data)
        {
            this.data = data;
        }

        public LoadWaitStatus(System.Action callbackFunc)
        {
            this.callbackFunc = callbackFunc;
        }

        public LoadWaitStatus(AssetData data, AssetLoader.LoadEndDataHandler callbackFunc)
        {
            this.data = data;
            this.callbackFunc2 = callbackFunc;
        }

        public void AddCallback(AssetLoader.LoadEndDataHandler callbackFunc)
        {
            this.callbackFunc2 = (AssetLoader.LoadEndDataHandler) Delegate.Combine(this.callbackFunc2, callbackFunc);
        }

        public void AddEntry()
        {
            this.data.AddEntry();
        }

        public bool IsSame(AssetData.Type type, string name) => 
            ((this.data != null) && this.data.IsSame(type, name));

        public System.Action CallbackFunc =>
            this.callbackFunc;

        public AssetLoader.LoadEndDataHandler DataCallbackFunc =>
            this.callbackFunc2;

        public AssetData Info =>
            this.data;

        public string Name
        {
            get
            {
                if (this.data != null)
                {
                    return this.data.Name;
                }
                return string.Empty;
            }
        }
    }
}

