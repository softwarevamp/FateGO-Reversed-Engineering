using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TerminalPramsManager
{
    private static bool mDebug_IsQuestReleaseAll;
    private static bool mDebug_IsWarStartActionSkip;
    private static enSceneStatus meSceneStatus;
    public static QuestClearHeroineInfo mQuestClearHeroineInfo;
    public static List<ServantSkillEntity> mQuestClearReward_Skill;
    public static List<ServantTreasureDvcEntity> mQuestClearReward_Treasure;
    public static QuestRewardInfo[] mQuestRewardInfos;
    private static string mTerminalWarStartedIds = string.Empty;
    private const string SAVEKEY_Debug_IsQuestReleaseAll = "Debug_IsQuestReleaseAll";
    private const string SAVEKEY_TerminalDispState = "TerminalDispState";
    private const string SAVEKEY_TerminalIsDoneShortcut = "TerminalIsDoneShortcut";
    private const string SAVEKEY_TerminalQuestId = "TerminalQuestId";
    private const string SAVEKEY_TerminalSpotId = "TerminalSpotId";
    private const string SAVEKEY_TerminalWarId = "TerminalWarId";
    private const string SAVEKEY_TerminalWarStartedIds = "TerminalWarStartedIds";

    public static void AutoOff()
    {
        IsAutoResume = false;
        IsAutoShortcut = false;
    }

    private static float GetAutoIntpTime() => 
        ((1f / ((float) Application.targetFrameRate)) * 2f);

    public static float GetIntpTime_Auto(float time) => 
        (!IsAuto() ? time : GetAutoIntpTime());

    public static float GetIntpTime_AutoResume(float time) => 
        (!IsAutoResume ? time : GetAutoIntpTime());

    public static bool IsAuto() => 
        (IsAutoResume || IsAutoShortcut);

    public static bool IsWarStartedId(int war_id)
    {
        char[] separator = new char[] { '\n' };
        foreach (string str in mTerminalWarStartedIds.Replace("\r\n", "\n").Split(separator))
        {
            if (str == war_id.ToString())
            {
                return true;
            }
        }
        return false;
    }

    public static void Load_SaveData()
    {
        string key = "TerminalDispState";
        if (PlayerPrefs.HasKey(key))
        {
            DispState = (eDispState) PlayerPrefs.GetInt(key);
        }
        key = "TerminalWarId";
        if (PlayerPrefs.HasKey(key))
        {
            WarId = PlayerPrefs.GetInt(key);
        }
        key = "TerminalSpotId";
        if (PlayerPrefs.HasKey(key))
        {
            SpotId = PlayerPrefs.GetInt(key);
        }
        key = "TerminalQuestId";
        if (PlayerPrefs.HasKey(key))
        {
            QuestId = PlayerPrefs.GetInt(key);
        }
        key = "TerminalIsDoneShortcut";
        if (PlayerPrefs.HasKey(key))
        {
            IsDoneShortcut = PlayerPrefs.GetInt(key) != 0;
        }
        key = "TerminalWarStartedIds";
        if (PlayerPrefs.HasKey(key))
        {
            mTerminalWarStartedIds = PlayerPrefs.GetString(key);
        }
        key = "Debug_IsQuestReleaseAll";
        if (PlayerPrefs.HasKey(key))
        {
            Debug_IsQuestReleaseAll = PlayerPrefs.GetInt(key) != 0;
        }
    }

    public static enSceneStatus mfGetSceneStatus() => 
        meSceneStatus;

    public static void mfSetSceneStatus(enSceneStatus eSceneStatus)
    {
        meSceneStatus = eSceneStatus;
    }

    public static void PlaySE_Cancel()
    {
        if (!IsAuto())
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        }
    }

    public static void PlaySE_Decide()
    {
        if (!IsAuto())
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
    }

    public static void Save_SaveData()
    {
        PlayerPrefs.SetInt("TerminalDispState", (int) DispState);
        PlayerPrefs.SetInt("TerminalWarId", WarId);
        PlayerPrefs.SetInt("TerminalSpotId", SpotId);
        PlayerPrefs.SetInt("TerminalQuestId", QuestId);
        PlayerPrefs.SetInt("TerminalIsDoneShortcut", !IsDoneShortcut ? 0 : 1);
        PlayerPrefs.SetString("TerminalWarStartedIds", mTerminalWarStartedIds);
        PlayerPrefs.SetInt("Debug_IsQuestReleaseAll", !Debug_IsQuestReleaseAll ? 0 : 1);
        PlayerPrefs.Save();
    }

    public static void SetWarStartedId(int war_id)
    {
        if (!IsWarStartedId(war_id))
        {
            mTerminalWarStartedIds = mTerminalWarStartedIds + war_id + "\n";
            Save_SaveData();
        }
    }

    public static bool Debug_IsQuestReleaseAll
    {
        get => 
            mDebug_IsQuestReleaseAll;
        set
        {
            mDebug_IsQuestReleaseAll = value;
        }
    }

    public static bool Debug_IsWarStartActionSkip
    {
        get => 
            mDebug_IsWarStartActionSkip;
        set
        {
            mDebug_IsWarStartActionSkip = value;
        }
    }

    public static eDispState DispState
    {
        [CompilerGenerated]
        get => 
            <DispState>k__BackingField;
        [CompilerGenerated]
        set
        {
            <DispState>k__BackingField = value;
        }
    }

    public static bool IsAutoResume
    {
        [CompilerGenerated]
        get => 
            <IsAutoResume>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsAutoResume>k__BackingField = value;
        }
    }

    public static bool IsAutoShortcut
    {
        [CompilerGenerated]
        get => 
            <IsAutoShortcut>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsAutoShortcut>k__BackingField = value;
        }
    }

    public static bool IsDispDone_AutoWebView
    {
        [CompilerGenerated]
        get => 
            <IsDispDone_AutoWebView>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsDispDone_AutoWebView>k__BackingField = value;
        }
    }

    public static bool IsDispDone_UIStandFigure
    {
        [CompilerGenerated]
        get => 
            <IsDispDone_UIStandFigure>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsDispDone_UIStandFigure>k__BackingField = value;
        }
    }

    public static bool IsDispUIStandFigure
    {
        [CompilerGenerated]
        get => 
            <IsDispUIStandFigure>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsDispUIStandFigure>k__BackingField = value;
        }
    }

    public static bool IsDoneShortcut
    {
        [CompilerGenerated]
        get => 
            <IsDoneShortcut>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsDoneShortcut>k__BackingField = value;
        }
    }

    public static bool IsPhaseClear
    {
        [CompilerGenerated]
        get => 
            <IsPhaseClear>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsPhaseClear>k__BackingField = value;
        }
    }

    public static bool IsQuestClear
    {
        [CompilerGenerated]
        get => 
            <IsQuestClear>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsQuestClear>k__BackingField = value;
        }
    }

    public static bool IsWarClear
    {
        [CompilerGenerated]
        get => 
            <IsWarClear>k__BackingField;
        [CompilerGenerated]
        set
        {
            <IsWarClear>k__BackingField = value;
        }
    }

    public static int PhaseCnt
    {
        [CompilerGenerated]
        get => 
            <PhaseCnt>k__BackingField;
        [CompilerGenerated]
        set
        {
            <PhaseCnt>k__BackingField = value;
        }
    }

    public static int QuestId
    {
        [CompilerGenerated]
        get => 
            <QuestId>k__BackingField;
        [CompilerGenerated]
        set
        {
            <QuestId>k__BackingField = value;
        }
    }

    public static int SpotId
    {
        [CompilerGenerated]
        get => 
            <SpotId>k__BackingField;
        [CompilerGenerated]
        set
        {
            <SpotId>k__BackingField = value;
        }
    }

    public static int SummonType
    {
        [CompilerGenerated]
        get => 
            <SummonType>k__BackingField;
        [CompilerGenerated]
        set
        {
            <SummonType>k__BackingField = value;
        }
    }

    public static int WarId
    {
        [CompilerGenerated]
        get => 
            <WarId>k__BackingField;
        [CompilerGenerated]
        set
        {
            <WarId>k__BackingField = value;
        }
    }

    public enum eDispState
    {
        None,
        Top,
        Map,
        Caldea,
        Story,
        Hero,
        MAX
    }

    public enum enSceneStatus
    {
        enNone,
        enInitialize,
        enResume,
        enMAX
    }
}

