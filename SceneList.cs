using System;
using System.Collections.Generic;

public class SceneList
{
    protected static Dictionary<Type, string> nameList;

    static SceneList()
    {
        Dictionary<Type, string> dictionary = new Dictionary<Type, string> {
            { 
                Type.None,
                string.Empty
            },
            { 
                Type.Init,
                "InitScene"
            },
            { 
                Type.DebugTest,
                "DebugTestScene"
            },
            { 
                Type.Title,
                "TitleScene"
            },
            { 
                Type.Battle,
                "BattleScene"
            },
            { 
                Type.DebugTitle,
                "DebugTitleScene"
            },
            { 
                Type.Summon,
                "SummonScene"
            },
            { 
                Type.Shop,
                "ShopScene"
            },
            { 
                Type.Friend,
                "FriendScene"
            },
            { 
                Type.MyRoom,
                "MyRoomScene"
            },
            { 
                Type.Combine,
                "CombineScene"
            },
            { 
                Type.Terminal,
                "TerminalScene"
            },
            { 
                Type.Follower,
                "FollowerScene"
            },
            { 
                Type.FinishBattle,
                "FinishBattleScene"
            },
            { 
                Type.Formation,
                "FormationScene"
            },
            { 
                Type.PartyOrganization,
                "PartyOrganizationScene"
            },
            { 
                Type.ServantList,
                "ServantListScene"
            },
            { 
                Type.ServantEquipList,
                "ServantEquipListScene"
            },
            { 
                Type.EventGacha,
                "EventGachaScene"
            },
            { 
                Type.MasterFormation,
                "MasterFormationScene"
            },
            { 
                Type.BattleDemoScene,
                "BattleDemoScene"
            },
            { 
                Type.SupportSelect,
                "SupportSelectScene"
            },
            { 
                Type.Empty,
                "EmptyScene"
            },
            { 
                Type.SummonEffect,
                "SummonEffectScene"
            }
        };
        nameList = dictionary;
    }

    public static string getSceneName(Type type)
    {
        if (nameList.ContainsKey(type))
        {
            return nameList[type];
        }
        return null;
    }

    public enum Type
    {
        Battle = 10,
        BattleDemoScene = 50,
        Combine = 0x20,
        DebugTest = 4,
        DebugTitle = 0x13,
        Empty = 0x3e8,
        EventGacha = 0x2b,
        FinishBattle = 0x26,
        Follower = 0x23,
        Formation = 0x27,
        Friend = 0x17,
        Init = 0,
        MasterFormation = 0x2c,
        MyRoom = 30,
        None = -1,
        PartyOrganization = 40,
        ServantEquipList = 0x2a,
        ServantList = 0x29,
        Shop = 0x16,
        Summon = 20,
        SummonEffect = 0x3e9,
        SupportSelect = 60,
        Terminal = 0x22,
        Title = 9
    }
}

