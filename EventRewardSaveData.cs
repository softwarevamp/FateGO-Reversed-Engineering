using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class EventRewardSaveData
{
    private const string KEY_GACHA_MOVEIDX = "GachaMoveIdx_";
    private const string KEY_GACHAIDX = "GahcaIdx_";
    private const string KEY_MISSION_FILTER = "EventMission_";
    private const string KEY_MISSIONID = "MissionId_";

    public static void LoadBoxGachaData(int eventId)
    {
        GachaIdx = 0;
        GachaMoveIdx = 0;
        string key = "GahcaIdx_" + eventId;
        if (PlayerPrefs.HasKey(key))
        {
            GachaIdx = PlayerPrefs.GetInt(key);
        }
        key = "GachaMoveIdx_" + eventId;
        if (PlayerPrefs.HasKey(key))
        {
            GachaMoveIdx = PlayerPrefs.GetInt(key);
        }
    }

    public static void LoadMissionData(int eventId)
    {
        MissionId = 0;
        FilterId = 0;
        string key = "MissionId_" + eventId;
        if (PlayerPrefs.HasKey(key))
        {
            MissionId = PlayerPrefs.GetInt(key);
        }
        key = "EventMission_" + eventId;
        if (PlayerPrefs.HasKey(key))
        {
            FilterId = PlayerPrefs.GetInt(key);
        }
    }

    public static void SaveBoxGachaData(int eventId)
    {
        PlayerPrefs.SetInt("GahcaIdx_" + eventId, GachaIdx);
        PlayerPrefs.SetInt("GachaMoveIdx_" + eventId, GachaMoveIdx);
        PlayerPrefs.Save();
    }

    public static void SaveMissionData(int eventId)
    {
        PlayerPrefs.SetInt("MissionId_" + eventId, MissionId);
        PlayerPrefs.SetInt("EventMission_" + eventId, FilterId);
        PlayerPrefs.Save();
    }

    public static int FilterId
    {
        [CompilerGenerated]
        get => 
            <FilterId>k__BackingField;
        [CompilerGenerated]
        set
        {
            <FilterId>k__BackingField = value;
        }
    }

    public static int GachaIdx
    {
        [CompilerGenerated]
        get => 
            <GachaIdx>k__BackingField;
        [CompilerGenerated]
        set
        {
            <GachaIdx>k__BackingField = value;
        }
    }

    public static int GachaMoveIdx
    {
        [CompilerGenerated]
        get => 
            <GachaMoveIdx>k__BackingField;
        [CompilerGenerated]
        set
        {
            <GachaMoveIdx>k__BackingField = value;
        }
    }

    public static int MissionId
    {
        [CompilerGenerated]
        get => 
            <MissionId>k__BackingField;
        [CompilerGenerated]
        set
        {
            <MissionId>k__BackingField = value;
        }
    }
}

