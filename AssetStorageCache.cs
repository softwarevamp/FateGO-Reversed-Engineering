using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class AssetStorageCache
{
    public static void ClearCache(bool isStandalone = true)
    {
        string cacheListFile = AssetManager.CacheListFile;
        Debug.Log("Delete File [" + cacheListFile + "]");
        if (File.Exists(cacheListFile))
        {
            File.Delete(cacheListFile);
        }
        if (isStandalone)
        {
            PlayerPrefs.DeleteKey("Asset");
            PlayerPrefs.Save();
        }
    }

    public static void ClearCacheAll(bool isStandalone = true)
    {
        string path = GetPath();
        Debug.Log("Delete Directory [" + path + "]");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        if (isStandalone)
        {
            PlayerPrefs.DeleteKey("Asset");
            PlayerPrefs.Save();
        }
    }

    public static string GetPath() => 
        (Application.persistentDataPath + "/AssetCaches/" + ManagerConfig.PlatformName + "/");
}

