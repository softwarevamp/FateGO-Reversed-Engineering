using System;
using System.Runtime.InteropServices;

public class CondType
{
    private static int EventId;

    public static int GetMissionAchiveNum(int condId)
    {
        int num = 0;
        if (condId == 0)
        {
            foreach (int num2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getMissionIdListByEvent(EventId))
            {
                num += !IsMissionAchive(num2) ? 0 : 1;
            }
            return num;
        }
        return (!IsMissionAchive(condId) ? 0 : 1);
    }

    public static int GetMIssionClearNum(int condId)
    {
        int num = 0;
        if (condId == 0)
        {
            foreach (int num2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getMissionIdListByEvent(EventId))
            {
                num += !IsMissionClear(num2) ? 0 : 1;
            }
            return num;
        }
        return (!IsMissionClear(condId) ? 0 : 1);
    }

    public static int GetNumMissionCondDetail(int condId, int condVal)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) condId };
        UserEventMissionCondDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT_MISSION_COND_DETAIL).getEntityFromId<UserEventMissionCondDetailEntity>(args);
        if (entity == null)
        {
            return 0;
        }
        int progressNum = (int) entity.progressNum;
        return ((progressNum < condVal) ? progressNum : condVal);
    }

    public static int GetProgressNum(Kind condType, int targetId, int condValue, int eventId)
    {
        EventId = eventId;
        switch (condType)
        {
            case Kind.QUEST_CLEAR:
                return GetQuestClear(targetId, condValue);

            case Kind.SVT_LEVEL:
                return GetSvtLv(targetId, condValue);

            case Kind.SVT_LIMIT:
                return GetSvtLimitCnt(targetId, condValue);

            case Kind.SVT_GET:
                return GetSvtGetNum(targetId);

            case Kind.SVT_FRIENDSHIP:
                return GetSvtFriendShip(targetId, condValue);

            case Kind.MISISION_CONDITION_DETAIL:
                return GetNumMissionCondDetail(targetId, condValue);

            case Kind.EVENT_MISSION_CLEAR:
                return GetMIssionClearNum(targetId);

            case Kind.EVENT_MISSION_ACHIEVE:
                return GetMissionAchiveNum(targetId);

            case Kind.QUEST_CLEAR_NUM:
                return GetQuestClearNum(targetId, condValue);
        }
        return 0;
    }

    public static int GetQuestClear(int condId, int condVal)
    {
        int num = 0;
        if (condId == 0)
        {
            foreach (int num2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP).GetQuestIdListByEventId(EventId))
            {
                if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(NetworkManager.UserId, num2) != null)
                {
                    num += !IsQuestClear(num2, -1, false) ? 0 : 1;
                }
            }
            return num;
        }
        return (!IsQuestClear(condId, -1, false) ? 0 : 1);
    }

    public static int GetQuestClearNum(int condId, int condVal)
    {
        int num = 0;
        if (condId == 0)
        {
            foreach (int num2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP).GetQuestIdListByEventId(EventId))
            {
                UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(NetworkManager.UserId, num2);
                if (entity != null)
                {
                    num += entity.getClearNum();
                }
            }
        }
        else
        {
            UserQuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(NetworkManager.UserId, condId);
            if (entity2 != null)
            {
                num = entity2.getClearNum();
            }
        }
        return ((num < condVal) ? num : condVal);
    }

    public static int GetSvtFriendShip(int condId, int condVal)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, condId);
        if (entity == null)
        {
            return 0;
        }
        int friendshipRank = entity.friendshipRank;
        return ((friendshipRank < condVal) ? friendshipRank : condVal);
    }

    public static int GetSvtGetNum(int condId) => 
        (!IsServantGet(condId) ? 0 : 1);

    public static int GetSvtLimitCnt(int condId, int condVal)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, condId);
        if (entity == null)
        {
            return 0;
        }
        int maxLimitCount = entity.maxLimitCount;
        return ((maxLimitCount < condVal) ? maxLimitCount : condVal);
    }

    public static int GetSvtLv(int condId, int condVal)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, condId);
        if (entity == null)
        {
            return 0;
        }
        int maxLv = entity.maxLv;
        return ((maxLv < condVal) ? maxLv : condVal);
    }

    public static bool IsConst(Kind condType) => 
        (condType == Kind.NONE);

    public static bool IsEvent(int condId)
    {
        EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(condId);
        return ((entity != null) && entity.IsOpen(true));
    }

    public static bool IsMissionAchive(int condId)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) condId };
        UserEventMissionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT_MISSION).getEntityFromId<UserEventMissionEntity>(args);
        return ((entity != null) && (entity.missionProgressType == 5));
    }

    public static bool IsMissionClear(int condId)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) condId };
        UserEventMissionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT_MISSION).getEntityFromId<UserEventMissionEntity>(args);
        return ((entity != null) && ((entity.missionProgressType == 4) || (entity.missionProgressType == 5)));
    }

    public static bool IsMissionCondDetail(int condId, int condVal)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) condId };
        UserEventMissionCondDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT_MISSION_COND_DETAIL).getEntityFromId<UserEventMissionCondDetailEntity>(args);
        return ((entity != null) && (entity.progressNum >= condVal));
    }

    public static bool IsNotQuestGroupClear(int questId, int groupId, int condVal, out int[] sameGroupQuestIds, bool isCheckResetFlag = false)
    {
        QuestGroupMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP);
        UserQuestMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST);
        sameGroupQuestIds = master.GetQuestIdListByGroupId(groupId, QuestGroupType.Type.QUEST_RELEASE);
        int num = 0;
        foreach (int num2 in sameGroupQuestIds)
        {
            if (questId != num2)
            {
                if (IsQuestClear(num2, -1, isCheckResetFlag))
                {
                    num++;
                }
                else
                {
                    UserQuestEntity entity = master2.getEntityFromId(NetworkManager.UserId, num2);
                    if ((entity != null) && (entity.getQuestPhase() >= 1))
                    {
                        num++;
                    }
                }
            }
        }
        return (num < condVal);
    }

    public static bool IsOpen(Kind condType, int targetId, int condValue)
    {
        switch (condType)
        {
            case Kind.NONE:
                return true;

            case Kind.QUEST_CLEAR:
                return IsQuestClear(targetId, condValue, false);

            case Kind.SVT_LEVEL:
                return IsServantLevel(targetId, condValue);

            case Kind.SVT_LIMIT:
                return IsServantLimit(targetId, condValue);

            case Kind.SVT_GET:
                return IsServantGet(targetId);

            case Kind.SVT_FRIENDSHIP:
                return IsServantFriendship(targetId, condValue);

            case Kind.SVT_GROUP:
                return IsServantGroup(targetId);

            case Kind.EVENT:
                return IsEvent(targetId);

            case Kind.PURCHASE_QP_SHOP:
                return IsPurchaseQpShop(targetId);

            case Kind.PURCHASE_STONE_SHOP:
                return IsPurchaseStoneShop(targetId);

            case Kind.MISISION_CONDITION_DETAIL:
                return IsMissionCondDetail(targetId, condValue);

            case Kind.EVENT_MISSION_CLEAR:
                return IsMissionClear(targetId);

            case Kind.EVENT_MISSION_ACHIEVE:
                return IsMissionAchive(targetId);

            case Kind.QUEST_CLEAR_NUM:
                return IsQuestClearNum(targetId, condValue);
        }
        return false;
    }

    public static bool IsOpen(Kind condType, int condValue, long userSvtId)
    {
        switch (condType)
        {
            case Kind.NONE:
                return true;

            case Kind.QUEST_CLEAR:
                return IsQuestClear(SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId).userId, condValue, -1);

            case Kind.SVT_LEVEL:
                return IsServantLevel(userSvtId, condValue);

            case Kind.SVT_LIMIT:
                return IsServantLimit(userSvtId, condValue);

            case Kind.SVT_GET:
                return IsServantGet(userSvtId, condValue);

            case Kind.SVT_FRIENDSHIP:
                return IsServantFriendship(userSvtId, condValue);

            case Kind.SVT_GROUP:
                return IsServantGroup(userSvtId, condValue);

            case Kind.EVENT:
                return IsEvent(condValue);

            case Kind.PURCHASE_QP_SHOP:
                return IsPurchaseQpShop(condValue);

            case Kind.PURCHASE_STONE_SHOP:
                return IsPurchaseStoneShop(condValue);
        }
        return false;
    }

    public static bool IsOpen(Kind condType, int condValue, long userId, int svtId)
    {
        switch (condType)
        {
            case Kind.NONE:
                return true;

            case Kind.QUEST_CLEAR:
                return IsQuestClear(userId, condValue, -1);

            case Kind.SVT_LEVEL:
                return IsServantLevel(userId, svtId, condValue);

            case Kind.SVT_LIMIT:
                return IsServantLimit(userId, svtId, condValue);

            case Kind.SVT_GET:
                return IsServantGet(userId, condValue);

            case Kind.SVT_FRIENDSHIP:
                return IsServantFriendship(userId, svtId, condValue);

            case Kind.SVT_GROUP:
                return IsServantGroup(userId, condValue);

            case Kind.EVENT:
                return IsEvent(condValue);

            case Kind.PURCHASE_QP_SHOP:
                return IsPurchaseQpShop(userId, condValue);

            case Kind.PURCHASE_STONE_SHOP:
                return IsPurchaseStoneShop(userId, condValue);
        }
        return false;
    }

    public static bool IsPurchaseQpShop(int condId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId).IsCondType();

    public static bool IsPurchaseQpShop(long userId, int condId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId).IsCondType(userId);

    public static bool IsPurchaseStoneShop(int condId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId).IsCondType();

    public static bool IsPurchaseStoneShop(long userId, int condId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId).IsCondType(userId);

    public static bool IsQuestClear(int condQuestId, int beforeClearQuestId = -1, bool isCheckResetFlag = false) => 
        IsQuestClear(NetworkManager.UserId, condQuestId, beforeClearQuestId);

    public static bool IsQuestClear(long userId, int condQuestId, int beforeClearQuestId = -1)
    {
        UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(userId, condQuestId);
        if (entity == null)
        {
            return false;
        }
        int num = entity.getClearNum();
        if ((beforeClearQuestId > 0) && (beforeClearQuestId == condQuestId))
        {
            num--;
        }
        return (num > 0);
    }

    public static bool IsQuestClearNum(int condId, int condVal)
    {
        UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(NetworkManager.UserId, condId);
        return (entity?.getClearNum() >= condVal);
    }

    public static bool IsQuestGroupClear(int questId, int groupId, int condVal, bool isCheckResetFlag = false)
    {
        QuestGroupMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP);
        UserQuestMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST);
        int[] questIdListByGroupId = master.GetQuestIdListByGroupId(groupId, QuestGroupType.Type.QUEST_RELEASE);
        int num = 0;
        foreach (int num2 in questIdListByGroupId)
        {
            if ((questId != num2) && IsQuestClear(num2, -1, isCheckResetFlag))
            {
                num++;
            }
        }
        return (num >= condVal);
    }

    public static bool IsQuestPhaseClear(int condQuestId, int condQuestPhase, int beforeClearQuestId = -1) => 
        IsQuestPhaseClear(NetworkManager.UserId, condQuestId, condQuestPhase, beforeClearQuestId);

    public static bool IsQuestPhaseClear(long userId, int condQuestId, int condQuestPhase, int beforeClearQuestId = -1)
    {
        UserQuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).getEntityFromId(userId, condQuestId);
        if (entity == null)
        {
            return false;
        }
        int num = entity.getQuestPhase();
        if ((beforeClearQuestId > 0) && (beforeClearQuestId == condQuestId))
        {
            num--;
        }
        return (num >= condQuestPhase);
    }

    public static bool IsServantFriendship(int svtId, int condFriendshipRank) => 
        IsServantFriendship(NetworkManager.UserId, svtId, condFriendshipRank);

    public static bool IsServantFriendship(long userSvtId, int condFriendshipRank)
    {
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        return IsServantFriendship(entity.userId, entity.svtId, condFriendshipRank);
    }

    public static bool IsServantFriendship(long userId, int svtId, int condFriendshipRank)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(userId, svtId);
        return ((entity != null) && (entity.friendshipRank >= condFriendshipRank));
    }

    public static bool IsServantGet(int condSvtId) => 
        IsServantGet(NetworkManager.UserId, condSvtId);

    public static bool IsServantGet(long userSvtId)
    {
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        return IsServantGet(entity.userId, entity.svtId);
    }

    public static bool IsServantGet(long userId, int condSvtId)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(userId, condSvtId);
        return ((entity != null) && (entity.status == 2));
    }

    public static bool IsServantGroup(int condGroup) => 
        IsServantGroup(NetworkManager.UserId, condGroup);

    public static bool IsServantGroup(long userId, int condGroup)
    {
        UserServantCollectionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION);
        foreach (ServantGroupEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantGroupMaster>(DataNameKind.Kind.SERVANT_GROUP).getEntityListById(condGroup))
        {
            UserServantCollectionEntity entity2 = master.getEntityFromId(userId, entity.svtId);
            if ((entity2 != null) && (entity2.status == 2))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsServantLevel(int svtId, int condLv) => 
        IsServantLevel(NetworkManager.UserId, svtId, condLv);

    public static bool IsServantLevel(long userSvtId, int condLv)
    {
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        return ((entity != null) && (entity.lv >= condLv));
    }

    public static bool IsServantLevel(long userId, int svtId, int condLv)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(userId, svtId);
        return ((entity != null) && (entity.maxLv >= condLv));
    }

    public static bool IsServantLimit(int svtId, int condLimitCount) => 
        IsServantLimit(NetworkManager.UserId, svtId, condLimitCount);

    public static bool IsServantLimit(long userSvtId, int condLimitCount)
    {
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        return ((entity != null) && (entity.limitCount >= condLimitCount));
    }

    public static bool IsServantLimit(long userId, int svtId, int condLimitCount)
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(userId, svtId);
        return ((entity != null) && (entity.maxLimitCount >= condLimitCount));
    }

    public static string OpenConditionText(Kind condType, int condValue)
    {
        switch (condType)
        {
            case Kind.NONE:
                return LocalizationManager.Get("COND_TYPE_NONE");

            case Kind.QUEST_CLEAR:
                return OpenConditionTextQuestClear(condValue);

            case Kind.SVT_LEVEL:
                return OpenConditionTextServantLevel(condValue);

            case Kind.SVT_LIMIT:
                return OpenConditionTextServantLimit(condValue);

            case Kind.SVT_GET:
                return OpenConditionTextServantGet(condValue);

            case Kind.SVT_FRIENDSHIP:
                return OpenConditionTextServantFriendship(condValue);

            case Kind.SVT_GROUP:
                return OpenConditionTextServantGroup(condValue);

            case Kind.EVENT:
                return OpenConditionTextEvent(condValue);

            case Kind.PURCHASE_QP_SHOP:
                return OpenConditionTextPurchaseQpShop(condValue);

            case Kind.PURCHASE_STONE_SHOP:
                return OpenConditionTextPurchaseStoneShop(condValue);
        }
        return LocalizationManager.GetUnknownName();
    }

    public static string OpenConditionTextEvent(int condId)
    {
        EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(condId);
        return string.Format(LocalizationManager.Get("COND_TYPE_EVENT"), entity.name);
    }

    public static string OpenConditionTextPurchaseQpShop(int condId)
    {
        ShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId);
        return string.Format(LocalizationManager.Get("COND_TYPE_PURCHASE_QP_SHOP"), entity.name);
    }

    public static string OpenConditionTextPurchaseStoneShop(int condId)
    {
        ShopEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SHOP).getEntityFromId<ShopEntity>(condId);
        return string.Format(LocalizationManager.Get("COND_TYPE_PURCHASE_STONE_SHOP"), entity.name);
    }

    public static string OpenConditionTextQuestClear(int condQuestId)
    {
        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(condQuestId);
        if (entity != null)
        {
            return string.Format(LocalizationManager.Get("COND_TYPE_QUEST_CLEAR"), entity.name);
        }
        return LocalizationManager.GetUnknownName();
    }

    public static string OpenConditionTextQuestPhaseClear(int condQuestId, int condQuestPhase)
    {
        if (condQuestPhase <= 0)
        {
            return OpenConditionTextQuestClear(condQuestId);
        }
        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(condQuestId);
        if (entity != null)
        {
            return string.Format(LocalizationManager.Get("COND_TYPE_QUEST_PHASE_CLEAR"), entity.name, string.Empty + condQuestPhase);
        }
        return LocalizationManager.GetUnknownName();
    }

    public static string OpenConditionTextServantFriendship(int condFriendshipRank) => 
        string.Format(LocalizationManager.Get("COND_TYPE_SERVANT_FRIENDSHIP"), condFriendshipRank);

    public static string OpenConditionTextServantGet(int condSvtId)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(condSvtId);
        return string.Format(LocalizationManager.Get("COND_TYPE_SERVANT_GET"), entity.name);
    }

    public static string OpenConditionTextServantGroup(int condGroup) => 
        string.Format(LocalizationManager.Get("COND_TYPE_SERVANT_GROUP"), condGroup);

    public static string OpenConditionTextServantLevel(int condLv) => 
        string.Format(LocalizationManager.Get("COND_TYPE_SERVANT_LEVEL"), condLv);

    public static string OpenConditionTextServantLimit(int condLimitCount) => 
        string.Format(LocalizationManager.Get("COND_TYPE_SERVANT_LIMIT_COUNT"), condLimitCount);

    public enum Kind
    {
        NONE,
        QUEST_CLEAR,
        ITEM_GET,
        USE_ITEM_ETERNITY,
        USE_ITEM_TIME,
        USE_ITEM_COUNT,
        SVT_LEVEL,
        SVT_LIMIT,
        SVT_GET,
        SVT_FRIENDSHIP,
        SVT_GROUP,
        EVENT,
        DATE,
        WEEKDAY,
        PURCHASE_QP_SHOP,
        PURCHASE_STONE_SHOP,
        WAR_CLEAR,
        FLAG,
        SVT_COUNT_STOP,
        BIRTH_DAY,
        EVENT_END,
        SVT_EVENT_JOIN,
        MISISION_CONDITION_DETAIL,
        EVENT_MISSION_CLEAR,
        EVENT_MISSION_ACHIEVE,
        QUEST_CLEAR_NUM,
        NOT_QUEST_GROUP_CLEAR,
        RAID_ALIVE,
        RAID_DEAD,
        QUEST_GROUP_CLEAR
    }
}

