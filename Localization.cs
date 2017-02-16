using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Localization
{
    public static LoadFunction loadFunction;
    public static bool localizationHasBeenSet = false;
    private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
    private static string mLanguage;
    private static int mLanguageIndex = -1;
    private static string[] mLanguages = null;
    private static bool mMerging = false;
    private static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();
    public static OnLocalizeNotification onLocalize;

    private static void AddCSV(BetterList<string> newValues, string[] newLanguages, Dictionary<string, int> languageIndices)
    {
        if (newValues.size >= 2)
        {
            string str = newValues[0];
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = ExtractStrings(newValues, newLanguages, languageIndices);
                if (mDictionary.ContainsKey(str))
                {
                    mDictionary[str] = strArray;
                    if (newLanguages == null)
                    {
                        Debug.LogWarning("Localization key '" + str + "' is already present");
                    }
                }
                else
                {
                    try
                    {
                        mDictionary.Add(str, strArray);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError("Unable to add '" + str + "' to the Localization dictionary.\n" + exception.Message);
                    }
                }
            }
        }
    }

    public static bool Exists(string key)
    {
        if (!localizationHasBeenSet)
        {
            language = PlayerPrefs.GetString("Language", "English");
        }
        string str = key + " Mobile";
        return (mDictionary.ContainsKey(str) || (mOldDictionary.ContainsKey(str) || (mDictionary.ContainsKey(key) || mOldDictionary.ContainsKey(key))));
    }

    private static string[] ExtractStrings(BetterList<string> added, string[] newLanguages, Dictionary<string, int> languageIndices)
    {
        string[] strArray2;
        if (newLanguages == null)
        {
            string[] strArray = new string[mLanguages.Length];
            int num = 1;
            int num2 = Mathf.Min(added.size, strArray.Length + 1);
            while (num < num2)
            {
                strArray[num - 1] = added[num];
                num++;
            }
            return strArray;
        }
        string key = added[0];
        if (!mDictionary.TryGetValue(key, out strArray2))
        {
            strArray2 = new string[mLanguages.Length];
        }
        int index = 0;
        int length = newLanguages.Length;
        while (index < length)
        {
            string str2 = newLanguages[index];
            int num5 = languageIndices[str2];
            strArray2[num5] = added[index + 1];
            index++;
        }
        return strArray2;
    }

    public static string Format(string key, params object[] parameters) => 
        string.Format(Get(key), parameters);

    public static string Get(string key)
    {
        string str2;
        string[] strArray;
        if (!localizationHasBeenSet)
        {
            LoadDictionary(PlayerPrefs.GetString("Language", "English"));
        }
        if (mLanguages == null)
        {
            Debug.LogError("No localization data present");
            return null;
        }
        string language = Localization.language;
        if (mLanguageIndex == -1)
        {
            for (int i = 0; i < mLanguages.Length; i++)
            {
                if (mLanguages[i] == language)
                {
                    mLanguageIndex = i;
                    break;
                }
            }
        }
        if (mLanguageIndex == -1)
        {
            mLanguageIndex = 0;
            mLanguage = mLanguages[0];
            Debug.LogWarning("Language not found: " + language);
        }
        string str3 = key + " Mobile";
        if (((mLanguageIndex != -1) && mDictionary.TryGetValue(str3, out strArray)) && (mLanguageIndex < strArray.Length))
        {
            return strArray[mLanguageIndex];
        }
        if (mOldDictionary.TryGetValue(str3, out str2))
        {
            return str2;
        }
        if (((mLanguageIndex != -1) && mDictionary.TryGetValue(key, out strArray)) && (mLanguageIndex < strArray.Length))
        {
            return strArray[mLanguageIndex];
        }
        if (mOldDictionary.TryGetValue(key, out str2))
        {
            return str2;
        }
        return key;
    }

    private static bool HasLanguage(string languageName)
    {
        int index = 0;
        int length = mLanguages.Length;
        while (index < length)
        {
            if (mLanguages[index] == languageName)
            {
                return true;
            }
            index++;
        }
        return false;
    }

    public static void Load(TextAsset asset)
    {
        Set(asset.name, new ByteReader(asset).ReadDictionary());
    }

    private static bool LoadAndSelect(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if ((mDictionary.Count == 0) && !LoadDictionary(value))
            {
                return false;
            }
            if (SelectLanguage(value))
            {
                return true;
            }
        }
        if (mOldDictionary.Count > 0)
        {
            return true;
        }
        mOldDictionary.Clear();
        mDictionary.Clear();
        if (string.IsNullOrEmpty(value))
        {
            PlayerPrefs.DeleteKey("Language");
        }
        return false;
    }

    public static bool LoadCSV(TextAsset asset, bool merge = false) => 
        LoadCSV(asset.bytes, asset, merge);

    public static bool LoadCSV(byte[] bytes, bool merge = false) => 
        LoadCSV(bytes, null, merge);

    private static bool LoadCSV(byte[] bytes, TextAsset asset, bool merge = false)
    {
        BetterList<string> list2;
        if (bytes == null)
        {
            return false;
        }
        ByteReader reader = new ByteReader(bytes);
        BetterList<string> list = reader.ReadCSV();
        if (list.size < 2)
        {
            return false;
        }
        list.RemoveAt(0);
        string[] newLanguages = null;
        if (string.IsNullOrEmpty(mLanguage))
        {
            localizationHasBeenSet = false;
        }
        if ((!localizationHasBeenSet || (!merge && !mMerging)) || ((mLanguages == null) || (mLanguages.Length == 0)))
        {
            mDictionary.Clear();
            mLanguages = new string[list.size];
            if (!localizationHasBeenSet)
            {
                mLanguage = PlayerPrefs.GetString("Language", list[0]);
                localizationHasBeenSet = true;
            }
            for (int j = 0; j < list.size; j++)
            {
                mLanguages[j] = list[j];
                if (mLanguages[j] == mLanguage)
                {
                    mLanguageIndex = j;
                }
            }
        }
        else
        {
            newLanguages = new string[list.size];
            for (int k = 0; k < list.size; k++)
            {
                newLanguages[k] = list[k];
            }
            for (int m = 0; m < list.size; m++)
            {
                if (!HasLanguage(list[m]))
                {
                    int newSize = mLanguages.Length + 1;
                    Array.Resize<string>(ref mLanguages, newSize);
                    mLanguages[newSize - 1] = list[m];
                    Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
                    foreach (KeyValuePair<string, string[]> pair in mDictionary)
                    {
                        string[] array = pair.Value;
                        Array.Resize<string>(ref array, newSize);
                        array[newSize - 1] = array[0];
                        dictionary.Add(pair.Key, array);
                    }
                    mDictionary = dictionary;
                }
            }
        }
        Dictionary<string, int> languageIndices = new Dictionary<string, int>();
        for (int i = 0; i < mLanguages.Length; i++)
        {
            languageIndices.Add(mLanguages[i], i);
        }
    Label_022D:
        list2 = reader.ReadCSV();
        if ((list2 != null) && (list2.size != 0))
        {
            if (!string.IsNullOrEmpty(list2[0]))
            {
                AddCSV(list2, newLanguages, languageIndices);
            }
            goto Label_022D;
        }
        if (!mMerging && (Localization.onLocalize != null))
        {
            mMerging = true;
            OnLocalizeNotification onLocalize = Localization.onLocalize;
            Localization.onLocalize = null;
            onLocalize();
            Localization.onLocalize = onLocalize;
            mMerging = false;
        }
        return true;
    }

    private static bool LoadDictionary(string value)
    {
        byte[] bytes = null;
        if (!localizationHasBeenSet)
        {
            if (loadFunction == null)
            {
                TextAsset asset = Resources.Load<TextAsset>("Localization");
                if (asset != null)
                {
                    bytes = asset.bytes;
                }
            }
            else
            {
                bytes = loadFunction("Localization");
            }
            localizationHasBeenSet = true;
        }
        if (LoadCSV(bytes, false))
        {
            return true;
        }
        if (string.IsNullOrEmpty(value))
        {
            value = mLanguage;
        }
        if (!string.IsNullOrEmpty(value))
        {
            if (loadFunction == null)
            {
                TextAsset asset2 = Resources.Load<TextAsset>(value);
                if (asset2 != null)
                {
                    bytes = asset2.bytes;
                }
            }
            else
            {
                bytes = loadFunction(value);
            }
            if (bytes != null)
            {
                Set(value, bytes);
                return true;
            }
        }
        return false;
    }

    [Obsolete("Use Localization.Get instead")]
    public static string Localize(string key) => 
        Get(key);

    private static bool SelectLanguage(string language)
    {
        mLanguageIndex = -1;
        if (mDictionary.Count != 0)
        {
            int index = 0;
            int length = mLanguages.Length;
            while (index < length)
            {
                if (mLanguages[index] == language)
                {
                    mOldDictionary.Clear();
                    mLanguageIndex = index;
                    mLanguage = language;
                    PlayerPrefs.SetString("Language", mLanguage);
                    if (onLocalize != null)
                    {
                        onLocalize();
                    }
                    UIRoot.Broadcast("OnLocalize");
                    return true;
                }
                index++;
            }
        }
        return false;
    }

    public static void Set(string languageName, Dictionary<string, string> dictionary)
    {
        mLanguage = languageName;
        PlayerPrefs.SetString("Language", mLanguage);
        mOldDictionary = dictionary;
        localizationHasBeenSet = true;
        mLanguageIndex = -1;
        mLanguages = new string[] { languageName };
        if (onLocalize != null)
        {
            onLocalize();
        }
        UIRoot.Broadcast("OnLocalize");
    }

    public static void Set(string languageName, byte[] bytes)
    {
        Set(languageName, new ByteReader(bytes).ReadDictionary());
    }

    public static void Set(string key, string value)
    {
        if (mOldDictionary.ContainsKey(key))
        {
            mOldDictionary[key] = value;
        }
        else
        {
            mOldDictionary.Add(key, value);
        }
    }

    public static Dictionary<string, string[]> dictionary
    {
        get
        {
            if (!localizationHasBeenSet)
            {
                LoadDictionary(PlayerPrefs.GetString("Language", "English"));
            }
            return mDictionary;
        }
        set
        {
            localizationHasBeenSet = value != null;
            mDictionary = value;
        }
    }

    [Obsolete("Localization is now always active. You no longer need to check this property.")]
    public static bool isActive =>
        true;

    public static string[] knownLanguages
    {
        get
        {
            if (!localizationHasBeenSet)
            {
                LoadDictionary(PlayerPrefs.GetString("Language", "English"));
            }
            return mLanguages;
        }
    }

    public static string language
    {
        get
        {
            if (string.IsNullOrEmpty(mLanguage))
            {
                localizationHasBeenSet = true;
                mLanguage = PlayerPrefs.GetString("Language", "English");
                LoadAndSelect(mLanguage);
            }
            return mLanguage;
        }
        set
        {
            if (mLanguage != value)
            {
                mLanguage = value;
                LoadAndSelect(value);
            }
        }
    }

    public delegate byte[] LoadFunction(string path);

    public delegate void OnLocalizeNotification();
}

