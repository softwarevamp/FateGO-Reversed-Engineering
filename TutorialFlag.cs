using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlag
{
    protected static Dictionary<string, Id> flagNameList;
    protected static readonly string SAVE_KEY = "TutorialFlagProgress";

    static TutorialFlag()
    {
        Dictionary<string, Id> dictionary = new Dictionary<string, Id> {
            { 
                "TUTORIAL_LABEL_STONE_GACHA",
                Id.TUTORIAL_LABEL_STONE_GACHA
            },
            { 
                "TUTORIAL_LABEL_END",
                Id.TUTORIAL_LABEL_END
            },
            { 
                "TUTORIAL_LABEL_SHOP",
                Id.TUTORIAL_LABEL_SHOP
            },
            { 
                "TUTORIAL_LABEL_COMBINE",
                Id.TUTORIAL_LABEL_COMBINE
            },
            { 
                "TUTORIAL_LABEL_FAVORITE1",
                Id.TUTORIAL_LABEL_FAVORITE1
            },
            { 
                "TUTORIAL_LABEL_FAVORITE2",
                Id.TUTORIAL_LABEL_FAVORITE2
            },
            { 
                "TUTORIAL_LABEL_GACHA_SCENE",
                Id.TUTORIAL_LABEL_GACHA_SCENE
            },
            { 
                "TUTORIAL_LABEL_GACHA_SVT_EQUIP",
                Id.TUTORIAL_LABEL_GACHA_SVT_EQUIP
            },
            { 
                "TUTORIAL_LABEL_DECK_SCENE",
                Id.TUTORIAL_LABEL_DECK_SCENE
            },
            { 
                "TUTORIAL_LABEL_DECK_SVT_EQUIP",
                Id.TUTORIAL_LABEL_DECK_SVT_EQUIP
            },
            { 
                "TUTORIAL_LABEL_EVENT_GACHA",
                Id.TUTORIAL_LABEL_EVENT_GACHA
            },
            { 
                "TUTORIAL_LABEL_EVENT_REWARD",
                Id.TUTORIAL_LABEL_EVENT_REWARD
            },
            { 
                "TUTORIAL_LABEL_DECK_IN_SVT_EQUIP",
                Id.TUTORIAL_LABEL_DECK_IN_SVT_EQUIP
            },
            { 
                "TUTORIAL_LABEL_EVENT_MISSION",
                Id.TUTORIAL_LABEL_EVENT_MISSION
            },
            { 
                "TUTORIAL_LABEL_MASHU_CHANGE",
                Id.TUTORIAL_LABEL_MASHU_CHANGE
            }
        };
        flagNameList = dictionary;
    }

    public static void ClearProgress()
    {
        if (!ManagerConfig.UseMock)
        {
            NetworkManager.getRequest<TutorialProgressRequest>(null).beginRequest(-1);
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
        }
    }

    public static void CompleteProgress()
    {
        SetProgress(Progress._3);
    }

    public static bool Get(int flagId) => 
        Get(SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME), flagId);

    public static bool Get(string flagIdName) => 
        Get(GetId(flagIdName));

    public static bool Get(Id flagId) => 
        Get((int) flagId);

    public static bool Get(UserGameEntity userGameEntity, int flagId)
    {
        uint num = ((uint) 1) << (flagId % 100);
        switch ((flagId / 100))
        {
            case 1:
                return ((userGameEntity.tutorial1 & num) != 0L);

            case 2:
                return ((userGameEntity.tutorial2 & num) != 0L);
        }
        return false;
    }

    public static Id GetId(string flagIdName) => 
        flagNameList[flagIdName];

    public static Id[] GetIdList()
    {
        Id[] idArray = new Id[flagNameList.Count];
        int num = 0;
        foreach (KeyValuePair<string, Id> pair in flagNameList)
        {
            idArray[num++] = pair.Value;
        }
        return idArray;
    }

    public static int GetProgress() => 
        PlayerPrefs.GetInt(SAVE_KEY, 0);

    public static bool IsProgressComplete() => 
        (Get(Id.TUTORIAL_LABEL_END) || IsProgressDone(Progress._3));

    public static bool IsProgressDone(int count) => 
        (GetProgress() >= count);

    public static bool IsProgressDone(Progress count) => 
        (Get(Id.TUTORIAL_LABEL_END) || IsProgressDone((int) count));

    public static void Set(int flagId)
    {
        Set(SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME), flagId);
    }

    public static void Set(string flagIdName)
    {
        Set(GetId(flagIdName));
    }

    public static void Set(Id flagId)
    {
        Set((int) flagId);
    }

    public static void Set(UserGameEntity userGameEntity, int flagId)
    {
        uint num = ((uint) 1) << (flagId % 100);
        switch ((flagId / 100))
        {
            case 1:
                userGameEntity.tutorial1 |= num;
                break;

            case 2:
                userGameEntity.tutorial2 |= num;
                break;
        }
    }

    public static void SetProgress(int count)
    {
        if (!ManagerConfig.UseMock)
        {
            if (GetProgress() != count)
            {
                NetworkManager.getRequest<TutorialProgressRequest>(null).beginRequest(count);
            }
            PlayerPrefs.SetInt(SAVE_KEY, count);
            PlayerPrefs.Save();
        }
    }

    public static void SetProgress(Progress count)
    {
        SetProgress((int) count);
    }

    public static string SAVE_KEY1 =>
        SAVE_KEY;

    public enum Id
    {
        NULL = -1,
        TUTORIAL_LABEL_COMBINE = 0x68,
        TUTORIAL_LABEL_DECK_IN_SVT_EQUIP = 0x71,
        TUTORIAL_LABEL_DECK_SCENE = 0x6d,
        TUTORIAL_LABEL_DECK_SVT_EQUIP = 110,
        TUTORIAL_LABEL_END = 0x66,
        TUTORIAL_LABEL_EVENT_GACHA = 0x6f,
        TUTORIAL_LABEL_EVENT_MISSION = 0x72,
        TUTORIAL_LABEL_EVENT_REWARD = 0x70,
        TUTORIAL_LABEL_FAVORITE1 = 0x69,
        TUTORIAL_LABEL_FAVORITE2 = 0x6a,
        TUTORIAL_LABEL_GACHA_SCENE = 0x6b,
        TUTORIAL_LABEL_GACHA_SVT_EQUIP = 0x6c,
        TUTORIAL_LABEL_MASHU_CHANGE = 0x73,
        TUTORIAL_LABEL_SHOP = 0x67,
        TUTORIAL_LABEL_STONE_GACHA = 0x65
    }

    public enum ImageId
    {
        NULL,
        SUMMON_TOP,
        FIRST_EQUIP,
        FORMATION_TOP,
        EQUIP_INFO_1,
        EQUIP_INFO_2,
        SHOP_TOP,
        COMBINE_TOP,
        BATTLE_STATUS,
        BATTLE_COMMANDSPELL,
        BATTLE_MENU,
        EVENT_GACHA,
        EVENT_REWARD,
        EVENT_MISSION
    }

    public enum Progress
    {
        _1 = 1,
        _2 = 2,
        _3 = 3,
        COMPLETE = 3
    }
}

