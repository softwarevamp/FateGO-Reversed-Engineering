using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetData
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2;
    protected AssetBundle assetBundle;
    protected string attrib;
    protected uint crc;
    protected int entryCount;
    protected string name;
    protected string new_name;
    protected int newVersion;
    protected int nowVersion;
    protected UnityEngine.Object[] objectList;
    protected int size;
    protected Type type;

    public AssetData(Type type, string name)
    {
        this.type = type;
        string str = string.Empty;
        if (name.Contains("%"))
        {
            char[] separator = new char[] { '%' };
            char[] chArray2 = new char[] { '%' };
            str = name.Split(separator)[0] + name.Split(chArray2)[2];
        }
        else
        {
            str = name;
        }
        this.name = str;
        this.new_name = name;
        this.attrib = string.Empty;
        this.nowVersion = 0;
        this.newVersion = 0;
        this.size = 0;
        this.crc = 0;
    }

    public AssetData(Type type, string name, int version, string attrib, int size, uint crc)
    {
        this.type = type;
        string str = string.Empty;
        if (name.Contains("%"))
        {
            char[] separator = new char[] { '%' };
            char[] chArray2 = new char[] { '%' };
            str = name.Split(separator)[0] + name.Split(chArray2)[2];
        }
        else
        {
            str = name;
        }
        this.new_name = name;
        this.name = str;
        this.attrib = attrib;
        this.nowVersion = version;
        this.newVersion = version;
        this.size = size;
        this.crc = crc;
    }

    public AssetData(Type type, string name, int nowVersion, int newVersion, string attrib, int size, uint crc)
    {
        this.type = type;
        string str = string.Empty;
        if (name.Contains("%"))
        {
            char[] separator = new char[] { '%' };
            char[] chArray2 = new char[] { '%' };
            str = name.Split(separator)[0] + name.Split(chArray2)[2];
        }
        else
        {
            str = name;
        }
        this.new_name = name;
        this.name = str;
        this.attrib = attrib;
        this.nowVersion = nowVersion;
        this.newVersion = newVersion;
        this.size = size;
        this.crc = crc;
    }

    public void AddEntry()
    {
        if (this.entryCount >= 0)
        {
            this.entryCount++;
        }
    }

    protected string GetBaseName()
    {
        if (this.name != null)
        {
            int startIndex = this.name.LastIndexOf('/');
            int num2 = this.name.LastIndexOf('.');
            startIndex = (startIndex >= 0) ? (startIndex + 1) : 0;
            num2 = (num2 >= 0) ? num2 : this.name.Length;
            if (startIndex < num2)
            {
                return this.name.Substring(startIndex, num2 - startIndex);
            }
        }
        return null;
    }

    public string GetDecryptObjectText(string name)
    {
        if (this.objectList != null)
        {
            if (name != null)
            {
                foreach (UnityEngine.Object obj2 in this.objectList)
                {
                    if (obj2.name.Equals(name))
                    {
                        if (obj2 is TextAsset)
                        {
                            return CryptData.TextDecrypt((obj2 as TextAsset).text);
                        }
                        if (obj2 is DataAsset)
                        {
                            return CryptData.TextDecrypt((obj2 as DataAsset).text);
                        }
                    }
                }
            }
            else if (this.objectList.Length > 0)
            {
                foreach (UnityEngine.Object obj3 in this.objectList)
                {
                    if (obj3 is TextAsset)
                    {
                        return CryptData.TextDecrypt((obj3 as TextAsset).text);
                    }
                    if (obj3 is DataAsset)
                    {
                        return CryptData.TextDecrypt((obj3 as DataAsset).text);
                    }
                }
            }
        }
        return null;
    }

    public string GetExt()
    {
        if (this.name != null)
        {
            int num = this.name.LastIndexOf('.');
            if ((num >= 0) && (this.name.Length > num))
            {
                return this.name.Substring(num + 1);
            }
        }
        return null;
    }

    public UnityEngine.Object GetObject()
    {
        if (this.objectList == null)
        {
            return null;
        }
        if (this.objectList.Length <= 0)
        {
            return null;
        }
        return this.objectList[0];
    }

    public T GetObject<T>() where T: UnityEngine.Object
    {
        if (this.objectList != null)
        {
            foreach (UnityEngine.Object obj2 in this.objectList)
            {
                if (obj2 is T)
                {
                    return (obj2 as T);
                }
            }
        }
        return null;
    }

    public UnityEngine.Object GetObject(string name)
    {
        if (this.objectList != null)
        {
            if (name != null)
            {
                foreach (UnityEngine.Object obj2 in this.objectList)
                {
                    if (obj2.name.Equals(name))
                    {
                        return obj2;
                    }
                }
            }
            else if (this.objectList.Length > 0)
            {
                return this.objectList[0];
            }
        }
        return null;
    }

    public T GetObject<T>(string name) where T: UnityEngine.Object
    {
        if (this.objectList != null)
        {
            if (name != null)
            {
                foreach (UnityEngine.Object obj2 in this.objectList)
                {
                    if (obj2.name.Equals(name) && (obj2 is T))
                    {
                        return (obj2 as T);
                    }
                }
            }
            else
            {
                foreach (UnityEngine.Object obj3 in this.objectList)
                {
                    if (obj3 is T)
                    {
                        return (obj3 as T);
                    }
                }
            }
        }
        return null;
    }

    public UnityEngine.Object[] GetObjectList() => 
        this.objectList;

    public T[] GetObjectList<T>() where T: UnityEngine.Object
    {
        if (this.objectList == null)
        {
            return null;
        }
        int num = 0;
        foreach (UnityEngine.Object obj2 in this.objectList)
        {
            if (obj2 is T)
            {
                num++;
            }
        }
        if (num == 0)
        {
            return null;
        }
        T[] localArray = new T[num];
        int num3 = 0;
        foreach (UnityEngine.Object obj3 in this.objectList)
        {
            if (obj3 is T)
            {
                localArray[num3++] = obj3 as T;
            }
        }
        return localArray;
    }

    public string[] GetObjectNameList()
    {
        List<string> list = new List<string>();
        if (this.objectList != null)
        {
            switch (this.type)
            {
                case Type.ASSET_STORAGE:
                    Debug.Log("AssetData: GetObjectList asset storage [" + this.name + "]");
                    break;

                case Type.ASSET_RESOURCE:
                    Debug.Log("AssetData: GetObjectList asset resource [" + this.name + "]");
                    break;
            }
            foreach (UnityEngine.Object obj2 in this.objectList)
            {
                Debug.Log("    [" + obj2.name + "]");
                list.Add(obj2.name);
            }
        }
        return list.ToArray();
    }

    public string GetObjectText(string name)
    {
        if (this.objectList != null)
        {
            if (name != null)
            {
                foreach (UnityEngine.Object obj2 in this.objectList)
                {
                    if (obj2.name.Equals(name))
                    {
                        if (obj2 is TextAsset)
                        {
                            return (obj2 as TextAsset).text;
                        }
                        if (obj2 is DataAsset)
                        {
                            return (obj2 as DataAsset).text;
                        }
                    }
                }
            }
            else if (this.objectList.Length > 0)
            {
                foreach (UnityEngine.Object obj3 in this.objectList)
                {
                    if (obj3 is TextAsset)
                    {
                        return (obj3 as TextAsset).text;
                    }
                    if (obj3 is DataAsset)
                    {
                        return (obj3 as DataAsset).text;
                    }
                }
            }
        }
        return null;
    }

    public bool IsDownloadOldVersion() => 
        ((this.nowVersion > 0) && (this.nowVersion != this.newVersion));

    public bool IsNeedUpdateVersion() => 
        (this.nowVersion != this.newVersion);

    public bool IsSame(string name) => 
        this.name.Equals(name);

    public bool IsSame(Type type, string name) => 
        ((this.type == type) && this.name.Equals(name));

    public void ReleaseData()
    {
        if (this.type != Type.ASSET_RESOURCE)
        {
            if (this.assetBundle != null)
            {
                this.assetBundle.Unload(false);
                this.assetBundle = null;
            }
            this.objectList = null;
        }
        else
        {
            foreach (UnityEngine.Object obj2 in this.objectList)
            {
                if (!(obj2 is DataAsset))
                {
                    Resources.UnloadAsset(obj2);
                }
            }
            this.objectList = null;
        }
        this.entryCount = 0;
    }

    public bool RemoveEntry()
    {
        if (this.entryCount > 0)
        {
            this.entryCount--;
            if (this.entryCount == 0)
            {
                SingletonMonoBehaviour<AssetManager>.Instance.ReleaseReservation(this);
                return true;
            }
        }
        else
        {
            Debug.LogError("AssetData: RemoveEntry count error [" + this.name + "]");
        }
        return false;
    }

    public bool RemoveEntryAll()
    {
        if (this.entryCount > 0)
        {
            this.entryCount = 0;
            SingletonMonoBehaviour<AssetManager>.Instance.ReleaseReservation(this);
            return true;
        }
        return false;
    }

    public void ResetVersion()
    {
        this.nowVersion = 0;
    }

    public bool SetAssetBundleData(AssetBundle bundle)
    {
        if (this.objectList == null)
        {
            if (this.type != Type.ASSET_STORAGE)
            {
                return false;
            }
            if (bundle == null)
            {
                return false;
            }
            if (this.GetExt() == null)
            {
                this.assetBundle = bundle;
                this.objectList = this.assetBundle.LoadAllAssets();
                return true;
            }
            Debug.LogError("Data is not AssetBundle! : " + this.Name);
        }
        return false;
    }

    public bool SetData(WWW data)
    {
        if (this.objectList == null)
        {
            if (this.type != Type.ASSET_STORAGE)
            {
                return false;
            }
            if (data == null)
            {
                return false;
            }
            string ext = this.GetExt();
            if (ext == null)
            {
                this.assetBundle = data.assetBundle;
                this.objectList = this.assetBundle.LoadAllAssets();
                return true;
            }
            UnityEngine.Object texture = null;
            string key = ext;
            if (key != null)
            {
                int num;
                if (<>f__switch$map2 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(10) {
                        { 
                            "txt",
                            0
                        },
                        { 
                            "html",
                            0
                        },
                        { 
                            "htm",
                            0
                        },
                        { 
                            "xml",
                            0
                        },
                        { 
                            "bytes",
                            1
                        },
                        { 
                            "png",
                            2
                        },
                        { 
                            "jpg",
                            2
                        },
                        { 
                            "jpeg",
                            2
                        },
                        { 
                            "wav",
                            3
                        },
                        { 
                            "ogg",
                            3
                        }
                    };
                    <>f__switch$map2 = dictionary;
                }
                if (<>f__switch$map2.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            texture = new DataAsset(data.text);
                            break;

                        case 1:
                            texture = new DataAsset(data.bytes);
                            break;

                        case 2:
                            texture = data.texture;
                            break;

                        case 3:
                            texture = data.audioClip;
                            break;
                    }
                }
            }
            if (texture != null)
            {
                texture.name = this.GetBaseName();
                this.objectList = new UnityEngine.Object[] { texture };
                return true;
            }
        }
        return false;
    }

    public bool SetResource()
    {
        if (this.objectList != null)
        {
            return false;
        }
        if (this.type != Type.ASSET_RESOURCE)
        {
            return false;
        }
        this.objectList = Resources.LoadAll(this.Path);
        if (this.objectList.Length <= 0)
        {
            Debug.LogError("file load error [" + this.Path + "]");
            this.objectList = null;
            return false;
        }
        return true;
    }

    public bool SetUpdateInfo(int version, string attrib, int size, uint crc)
    {
        this.newVersion = version;
        this.attrib = attrib;
        this.size = size;
        if (this.crc != crc)
        {
            this.crc = crc;
            this.nowVersion = 0;
        }
        return (this.nowVersion < version);
    }

    public bool UnloadAssetBundleEntry()
    {
        if (((this.entryCount > 0) && (this.type != Type.ASSET_RESOURCE)) && (this.assetBundle != null))
        {
            this.assetBundle.Unload(false);
            this.assetBundle = null;
            return true;
        }
        return false;
    }

    public bool UpdateVersion()
    {
        int nowVersion = this.nowVersion;
        this.nowVersion = this.newVersion;
        return (nowVersion != this.newVersion);
    }

    public string Attrib =>
        this.attrib;

    public uint Crc =>
        this.crc;

    public Type DataType =>
        this.type;

    public int EntryCount =>
        this.entryCount;

    public bool IsAssetBundle =>
        ((this.type == Type.ASSET_STORAGE) && (this.GetExt() == null));

    public bool IsEmpty =>
        (this.objectList == null);

    public string LastName
    {
        get
        {
            int num = this.name.LastIndexOf('/');
            if (num >= 0)
            {
                return this.name.Substring(num + 1);
            }
            return this.name;
        }
    }

    public string Name
    {
        get => 
            this.name;
        set
        {
            this.name = value;
        }
    }

    public string NewName
    {
        get => 
            this.new_name;
        set
        {
            this.new_name = value;
        }
    }

    public string NewPath
    {
        get
        {
            Type type = this.type;
            if (type != Type.ASSET_STORAGE)
            {
                if (type == Type.ASSET_RESOURCE)
                {
                    return this.name;
                }
                return string.Empty;
            }
            if (this.GetExt() == null)
            {
                string str2 = AssetManager.getShaName(this.new_name.Replace('/', '@') + ".unity3d");
                return (AssetManager.CachePathName + str2);
            }
            return (AssetManager.CachePathName + this.name.Replace('/', '@'));
        }
    }

    public int NowVersion =>
        this.nowVersion;

    public string Path
    {
        get
        {
            Type type = this.type;
            if (type != Type.ASSET_STORAGE)
            {
                if (type == Type.ASSET_RESOURCE)
                {
                    return this.name;
                }
                return string.Empty;
            }
            if (this.GetExt() == null)
            {
                string str2 = AssetManager.getShaName(this.name.Replace('/', '@') + ".unity3d");
                return (AssetManager.CachePathName + str2);
            }
            return (AssetManager.CachePathName + this.name.Replace('/', '@'));
        }
    }

    public int Size =>
        this.size;

    public string Url
    {
        get
        {
            if ((this.nowVersion >= 0) && this.IsNeedUpdateVersion())
            {
                return AssetManager.getUrlString(this);
            }
            return string.Empty;
        }
    }

    public enum Type
    {
        ASSET_STORAGE,
        ASSET_RESOURCE,
        ASSET_AUDIO
    }
}

