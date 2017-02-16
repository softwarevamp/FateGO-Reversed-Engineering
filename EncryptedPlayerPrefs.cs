using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EncryptedPlayerPrefs
{
    public static string[] keys;
    private static string privateKey = "t68aZyLxlMWVjw8lWgdZ";

    public static bool CheckEncryption(string key, string type, string value)
    {
        int @int = PlayerPrefs.GetInt(key + "_used_key");
        string str = keys[@int];
        string str2 = Md5(key + "_" + type + "_" + privateKey + "_" + str + "_" + value);
        if (!PlayerPrefs.HasKey(key + "_encryption_check"))
        {
            return false;
        }
        return (PlayerPrefs.GetString(key + "_encryption_check") == str2);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(key + "_encryption_check");
        PlayerPrefs.DeleteKey(key + "_used_key");
    }

    public static float GetFloat(string key) => 
        GetFloat(key, 0f);

    public static float GetFloat(string key, float defaultValue)
    {
        float @float = PlayerPrefs.GetFloat(key);
        if (!CheckEncryption(key, "float", Mathf.Floor(@float * 1000f).ToString()))
        {
            return defaultValue;
        }
        return @float;
    }

    public static int GetInt(string key) => 
        GetInt(key, 0);

    public static int GetInt(string key, int defaultValue)
    {
        int @int = PlayerPrefs.GetInt(key);
        if (!CheckEncryption(key, "int", @int.ToString()))
        {
            return defaultValue;
        }
        return @int;
    }

    public static long GetLong(string key) => 
        GetLong(key, 0L);

    public static long GetLong(string key, long defaultValue)
    {
        string str = PlayerPrefs.GetString(key);
        if (CheckEncryption(key, "long", str))
        {
            long result = defaultValue;
            long.TryParse(str, out result);
            return result;
        }
        return (long) GetInt(key);
    }

    public static string GetString(string key) => 
        GetString(key, string.Empty);

    public static string GetString(string key, string defaultValue)
    {
        string str = PlayerPrefs.GetString(key);
        if (!CheckEncryption(key, "string", str))
        {
            return defaultValue;
        }
        return str;
    }

    public static bool HasKey(string key) => 
        PlayerPrefs.HasKey(key);

    public static string Md5(string strToEncrypt)
    {
        byte[] bytes = new UTF8Encoding().GetBytes(strToEncrypt);
        byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
        string str = string.Empty;
        for (int i = 0; i < buffer2.Length; i++)
        {
            str = str + Convert.ToString(buffer2[i], 0x10).PadLeft(2, '0');
        }
        return str.PadLeft(0x20, '0');
    }

    public static void SaveEncryption(string key, string type, string value)
    {
        int index = (int) Mathf.Floor(UnityEngine.Random.Range((float) 0f, (float) 0.99f) * keys.Length);
        string str = keys[index];
        string str2 = Md5(key + "_" + type + "_" + privateKey + "_" + str + "_" + value);
        PlayerPrefs.SetString(key + "_encryption_check", str2);
        PlayerPrefs.SetInt(key + "_used_key", index);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        SaveEncryption(key, "float", Mathf.Floor(value * 1000f).ToString());
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        SaveEncryption(key, "int", value.ToString());
    }

    public static void SetLong(string key, long value)
    {
        PlayerPrefs.SetString(key, value.ToString());
        SaveEncryption(key, "long", value.ToString());
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        SaveEncryption(key, "string", value);
    }
}

