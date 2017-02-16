using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{
    protected static bool isBusySetAssetData;
    protected static Dictionary<string, string> lookup = new Dictionary<string, string>();
    [SerializeField]
    protected TextAsset mainTextData;
    protected static string noEntryNameText;
    protected static string unknownNameText;

    public static bool ContainsKey(string key) => 
        lookup.ContainsKey(key);

    public static string Get(string key)
    {
        if (ContainsKey(key))
        {
            return lookup[key];
        }
        return key;
    }

    public static string GetAttackIconInfo(int atk) => 
        string.Format(Get("ATTACK_ICON_INFO"), atk);

    public static string GetAttackInfo(int atk) => 
        string.Format(Get("ATTACK_INFO"), atk);

    public static string GetBeforeTime(long at)
    {
        long num = NetworkManager.getTime() - at;
        if (num < 60L)
        {
            return string.Format(Get("TIME_BEFORE_MINUTES"), 0);
        }
        long num2 = num / 60L;
        if (num2 < 60L)
        {
            return string.Format(Get("TIME_BEFORE_MINUTES"), num2);
        }
        long num3 = num2 / 60L;
        if (num3 < 0x18L)
        {
            return string.Format(Get("TIME_BEFORE_HOURS"), num3);
        }
        long num4 = num3 / 0x18L;
        if (num4 < 0x16dL)
        {
            return string.Format(Get("TIME_BEFORE_DAYS"), num4);
        }
        return Get("TIME_BEFORE_OVER_YEARS");
    }

    public static string GetCostIconInfo(int cost) => 
        string.Format(Get("COST_ICON_INFO"), cost);

    public static string GetCostInfo(int cost) => 
        string.Format(Get("COST_INFO"), cost);

    public static string GetCountInfo(int count) => 
        string.Format(Get("COUNT_INFO"), count);

    public static string GetDate(long time)
    {
        if (time > 0L)
        {
            DateTime time2 = NetworkManager.getLocalDateTime(time);
            return $"{time2.Year:D}/{time2.Month:D2}/{time2.Day:D2}";
        }
        return string.Empty;
    }

    public static string GetDateTime(long time)
    {
        if (time > 0L)
        {
            DateTime time2 = NetworkManager.getLocalDateTime(time);
            return $"{time2.Year:D}/{time2.Month:D2}/{time2.Day:D2} {time2.Hour:D2}:{time2.Minute:D2}";
        }
        return string.Empty;
    }

    public static string GetEventPointInfo(int addPoint, int ratePoint, string unit = "")
    {
        string str = ((addPoint < 0) ? string.Empty : "+") + GetNumberFormat(addPoint.ToString()) + ((unit == null) ? string.Empty : unit);
        int num = ratePoint / 10;
        string str2 = ((ratePoint < 0) ? string.Empty : "+") + GetNumberFormat(num.ToString());
        if ((addPoint != 0) && (ratePoint != 0))
        {
            return string.Format(Get("EVENT_POINT_ADD_RATE_INFO"), str, str2);
        }
        if (addPoint != 0)
        {
            return string.Format(Get("EVENT_POINT_ADD_INFO"), str);
        }
        if (ratePoint != 0)
        {
            return string.Format(Get("EVENT_POINT_RATE_INFO"), str2);
        }
        return Get("EVENT_POINT_NONE_INFO");
    }

    public static string GetHaveUnitInfo(int count) => 
        string.Format(Get("HAVE_UNIT_INFO"), count);

    public static string GetHpIconInfo(int hp) => 
        string.Format(Get("HP_ICON_INFO"), hp);

    public static string GetHpInfo(int hp) => 
        string.Format(Get("HP_INFO"), hp);

    public static string GetIdIconInfo(int id) => 
        string.Format(Get("ID_ICON_INFO"), id);

    public static string GetLevelIconInfo(int lv)
    {
        if (lv > 0)
        {
            return string.Format(Get("LEVEL_ICON_INFO"), lv);
        }
        return string.Empty;
    }

    public static string GetLevelInfo(int lv) => 
        string.Format(Get("LEVEL_INFO"), lv);

    public static string GetLevelList(int[] levelList)
    {
        StringBuilder builder = new StringBuilder();
        int length = levelList.Length;
        for (int i = 0; i < length; i++)
        {
            if (i > 0)
            {
                builder.Append("/");
            }
            builder.Append((levelList[i] <= 0) ? "-" : levelList[i].ToString());
        }
        return builder.ToString();
    }

    public static string GetNoEntryName() => 
        noEntryNameText;

    public static string GetNumberFormat(int data) => 
        GetNumberFormat(data.ToString());

    public static string GetNumberFormat(string data)
    {
        if (data == null)
        {
            return string.Empty;
        }
        string str = string.Empty;
        int num = 3;
        for (int i = data.Length - 1; i >= 0; i--)
        {
            char ch = data[i];
            if ((ch < '0') || (ch > '9'))
            {
                return (data.Substring(0, i + 1) + str);
            }
            if (--num < 0)
            {
                str = ch + "," + str;
                num = 2;
            }
            else
            {
                str = ch + str;
            }
        }
        return str;
    }

    public static string GetPeriod(long startedAt, long endedAt, bool isDispStartTime = false, bool isDispEndTime = false)
    {
        string str = string.Empty;
        if ((startedAt <= 0L) && (endedAt <= 0L))
        {
            return str;
        }
        if (isDispStartTime)
        {
            str = str + GetDateTime(startedAt);
        }
        else
        {
            str = str + GetDate(startedAt);
        }
        str = str + " ~ ";
        if (isDispEndTime)
        {
            return (str + GetDateTime(endedAt));
        }
        return (str + GetDate(endedAt));
    }

    public static string GetPrice2Info(int price) => 
        string.Format(Get("PRICE2_INFO"), GetNumberFormat(price));

    public static string GetPriceInfo(int price) => 
        string.Format(Get("PRICE_INFO"), GetNumberFormat(price));

    public static string GetRarityInfo(int rare) => 
        $"[{rare}]";

    public static string GetRestTime(long at)
    {
        long num = at - NetworkManager.getTime();
        if (num < 0L)
        {
            return Get("TIME_REST_TIMEOVER");
        }
        if (num < 60L)
        {
            return string.Format(Get("TIME_REST_MINUTES"), 0);
        }
        long num2 = num / 60L;
        if (num2 < 60L)
        {
            return string.Format(Get("TIME_REST_MINUTES"), num2);
        }
        long num3 = num2 / 60L;
        if (num3 < 0x18L)
        {
            return string.Format(Get("TIME_REST_HOURS"), num3);
        }
        long num4 = num3 / 0x18L;
        if (num4 < 0x16dL)
        {
            return string.Format(Get("TIME_REST_DAYS"), num4);
        }
        return Get("TIME_REST_OVER_YEARS");
    }

    public static string GetRestTime2(long at)
    {
        long num = at - NetworkManager.getTime();
        if (num < 0L)
        {
            return Get("TIME_REST2_TIMEOVER");
        }
        if (num < 60L)
        {
            return string.Format(Get("TIME_REST2_MINUTES"), 0);
        }
        long num2 = num / 60L;
        if (num2 < 60L)
        {
            return string.Format(Get("TIME_REST2_MINUTES"), num2);
        }
        long num3 = num2 / 60L;
        if (num3 < 0x18L)
        {
            return string.Format(Get("TIME_REST2_HOURS"), num3);
        }
        long num4 = num3 / 0x18L;
        if (num4 < 0x16dL)
        {
            return string.Format(Get("TIME_REST2_DAYS"), num4);
        }
        return Get("TIME_REST2_OVER_YEARS");
    }

    public static string GetStoneInfo(int n) => 
        string.Format(Get("STONE_INFO"), n);

    public static string GetTime(long time)
    {
        if (time > 0L)
        {
            DateTime time2 = NetworkManager.getLocalDateTime(time);
            return $"{time2.Hour:D2}:{time2.Minute:D2}";
        }
        return string.Empty;
    }

    public static string GetUnitInfo(int count) => 
        string.Format(Get("UNIT_INFO"), GetNumberFormat(count));

    public static string GetUnknownName() => 
        unknownNameText;

    public static void Initialize()
    {
        LocalizationManager instance = SingletonMonoBehaviour<LocalizationManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    protected void InitializeLocal()
    {
        if (this.mainTextData != null)
        {
            this.SetTextData(this.mainTextData.text);
        }
    }

    public static bool IsBusySetAssetData() => 
        isBusySetAssetData;

    public static void LoadAssetData()
    {
        <LoadAssetData>c__AnonStorey66 storey = new <LoadAssetData>c__AnonStorey66 {
            ins = SingletonMonoBehaviour<LocalizationManager>.Instance
        };
        if (storey.ins != null)
        {
            isBusySetAssetData = true;
            AssetManager.loadAssetStorage("Localization", new AssetLoader.LoadEndDataHandler(storey.<>m__42));
        }
    }

    protected void SetTextData(string text_data)
    {
        string[] strArray = text_data.Split(new char[] { '뮿', '﻿', '￾', '\r', '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
        string jsonstr = string.Empty;
        for (int i = 0; i < strArray.Length; i++)
        {
            string str2 = strArray[i];
            int index = str2.IndexOf("//");
            if (index < 0)
            {
                jsonstr = jsonstr + str2 + "\n";
            }
            else if (index > 0)
            {
                jsonstr = jsonstr + str2.Substring(0, index - 1) + "\n";
            }
        }
        foreach (KeyValuePair<string, object> pair in JsonManager.getDictionary(jsonstr))
        {
            lookup[pair.Key] = pair.Value.ToString();
        }
        unknownNameText = lookup["UNKNOWN_NAME"];
        noEntryNameText = lookup["NO_ENTRY_NAME"];
    }

    [CompilerGenerated]
    private sealed class <LoadAssetData>c__AnonStorey66
    {
        internal LocalizationManager ins;

        internal void <>m__42(AssetData assetData)
        {
            string text = assetData.GetObject<TextAsset>().text;
            this.ins.SetTextData(text);
            LocalizationManager.isBusySetAssetData = false;
        }
    }
}

