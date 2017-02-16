using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class clsQuestCheck : SingletonTemplate<clsQuestCheck>
{
    [CompilerGenerated]
    private static Comparison<QuestReleaseEntity> <>f__am$cache0;

    private List<QuestEntity> GetQuestEntityByQuestId(List<int> qids)
    {
        List<QuestEntity> list = new List<QuestEntity>();
        foreach (int num in qids)
        {
            QuestEntity item = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(num);
            list.Add(item);
        }
        return list;
    }

    public List<QuestEntity> GetReleaseQuestEntityByServantFriendShip(int svt_id) => 
        this.GetReleaseQuestEntityByServantFriendShip(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantFriendShip(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestEntityByServantFriendShip(svt_id, -1, type_flag);

    public List<QuestEntity> GetReleaseQuestEntityByServantFriendShip(int svt_id, int oldFriendShipRank) => 
        this.GetReleaseQuestEntityByServantFriendShip(svt_id, oldFriendShipRank, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantFriendShip(int svt_id, int oldFriendShipRank, QuestEntity.TypeFlag type_flag)
    {
        List<int> qids = this.GetReleaseQuestIdByServantFriendShip(svt_id, oldFriendShipRank, type_flag);
        return this.GetQuestEntityByQuestId(qids);
    }

    public List<QuestEntity> GetReleaseQuestEntityByServantGet(int svt_id, QuestEntity.TypeFlag type_flag = 110)
    {
        List<int> releaseQuestIdByServantGet = this.GetReleaseQuestIdByServantGet(svt_id, type_flag);
        return this.GetQuestEntityByQuestId(releaseQuestIdByServantGet);
    }

    public List<QuestEntity> GetReleaseQuestEntityByServantLimit(int svt_id) => 
        this.GetReleaseQuestEntityByServantLimit(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantLimit(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestEntityByServantLimit(svt_id, -1, type_flag);

    public List<QuestEntity> GetReleaseQuestEntityByServantLimit(int svt_id, int oldLimitCount) => 
        this.GetReleaseQuestEntityByServantLimit(svt_id, oldLimitCount, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantLimit(int svt_id, int oldLimitCount, QuestEntity.TypeFlag type_flag)
    {
        List<int> qids = this.GetReleaseQuestIdByServantLimit(svt_id, oldLimitCount, type_flag);
        return this.GetQuestEntityByQuestId(qids);
    }

    public List<QuestEntity> GetReleaseQuestEntityByServantLv(int svt_id) => 
        this.GetReleaseQuestEntityByServantLv(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantLv(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestEntityByServantLv(svt_id, -1, type_flag);

    public List<QuestEntity> GetReleaseQuestEntityByServantLv(int svt_id, int oldLv) => 
        this.GetReleaseQuestEntityByServantLv(svt_id, oldLv, QuestEntity.TypeFlag.ALL);

    public List<QuestEntity> GetReleaseQuestEntityByServantLv(int svt_id, int oldLv, QuestEntity.TypeFlag type_flag)
    {
        List<int> qids = this.GetReleaseQuestIdByServantLv(svt_id, oldLv, type_flag);
        return this.GetQuestEntityByQuestId(qids);
    }

    public List<int> GetReleaseQuestIdByServantFriendShip(int svt_id) => 
        this.GetReleaseQuestIdByServantFriendShip(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantFriendShip(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestIdByServantFriendShip(svt_id, -1, type_flag);

    public List<int> GetReleaseQuestIdByServantFriendShip(int svt_id, int oldFriendShipRank) => 
        this.GetReleaseQuestIdByServantFriendShip(svt_id, oldFriendShipRank, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantFriendShip(int svt_id, int oldFriendShipRank, QuestEntity.TypeFlag type_flag)
    {
        List<int> collection = new List<int>();
        CondType.Kind type = CondType.Kind.SVT_FRIENDSHIP;
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(type))
        {
            if (svt_id == entity.targetId)
            {
                int id = entity.getQuestId();
                QuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(id);
                if (((type_flag & entity2.GetTypeFlag()) != QuestEntity.TypeFlag.NONE) && this.IsQuestRelease(id, oldFriendShipRank, type))
                {
                    collection.Add(id);
                }
            }
        }
        if (oldFriendShipRank >= 0)
        {
            List<int> list2 = new List<int>(collection);
            collection = this.GetReleaseQuestIdByServantFriendShip(svt_id, -1, type_flag);
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int num4 = collection[i];
                foreach (int num5 in list2)
                {
                    if (num4 == num5)
                    {
                        collection.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        return collection;
    }

    public List<int> GetReleaseQuestIdByServantGet(int svt_id, QuestEntity.TypeFlag type_flag = 110)
    {
        List<int> list = new List<int>();
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(CondType.Kind.SVT_GET))
        {
            if (svt_id == entity.targetId)
            {
                int id = entity.getQuestId();
                QuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(id);
                if (((type_flag & entity2.GetTypeFlag()) != QuestEntity.TypeFlag.NONE) && this.IsQuestRelease(id, -1, CondType.Kind.NONE))
                {
                    list.Add(id);
                }
            }
        }
        ServantGroupEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantGroupMaster>(DataNameKind.Kind.SERVANT_GROUP).getListByServantID(svt_id);
        QuestReleaseEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(CondType.Kind.SVT_GROUP);
        foreach (ServantGroupEntity entity3 in entityArray3)
        {
            foreach (QuestReleaseEntity entity4 in entityArray)
            {
                if (entity3.getServantGroupId() == entity4.targetId)
                {
                    int item = entity4.getQuestId();
                    if (!list.Contains(item))
                    {
                        QuestEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(item);
                        if (((type_flag & entity5.GetTypeFlag()) != QuestEntity.TypeFlag.NONE) && this.IsQuestRelease(item, -1, CondType.Kind.NONE))
                        {
                            list.Add(item);
                        }
                    }
                }
            }
        }
        return list;
    }

    public List<int> GetReleaseQuestIdByServantLimit(int svt_id) => 
        this.GetReleaseQuestIdByServantLimit(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantLimit(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestIdByServantLimit(svt_id, -1, type_flag);

    public List<int> GetReleaseQuestIdByServantLimit(int svt_id, int oldLimitCount) => 
        this.GetReleaseQuestIdByServantLimit(svt_id, oldLimitCount, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantLimit(int svt_id, int oldLimitCount, QuestEntity.TypeFlag type_flag)
    {
        List<int> collection = new List<int>();
        CondType.Kind type = CondType.Kind.SVT_LIMIT;
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(type))
        {
            if (svt_id == entity.targetId)
            {
                int id = entity.getQuestId();
                QuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(id);
                if (((type_flag & entity2.GetTypeFlag()) != QuestEntity.TypeFlag.NONE) && this.IsQuestRelease(id, oldLimitCount, type))
                {
                    collection.Add(id);
                }
            }
        }
        if (oldLimitCount >= 0)
        {
            List<int> list2 = new List<int>(collection);
            collection = this.GetReleaseQuestIdByServantLimit(svt_id, -1, type_flag);
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int num4 = collection[i];
                foreach (int num5 in list2)
                {
                    if (num4 == num5)
                    {
                        collection.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        return collection;
    }

    public List<int> GetReleaseQuestIdByServantLv(int svt_id) => 
        this.GetReleaseQuestIdByServantLv(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantLv(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.GetReleaseQuestIdByServantLv(svt_id, -1, type_flag);

    public List<int> GetReleaseQuestIdByServantLv(int svt_id, int oldLv) => 
        this.GetReleaseQuestIdByServantLv(svt_id, oldLv, QuestEntity.TypeFlag.ALL);

    public List<int> GetReleaseQuestIdByServantLv(int svt_id, int oldLv, QuestEntity.TypeFlag type_flag)
    {
        List<int> list = new List<int>();
        CondType.Kind type = CondType.Kind.SVT_LEVEL;
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(type))
        {
            if (svt_id == entity.targetId)
            {
                int id = entity.getQuestId();
                QuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(id);
                if (((type_flag & entity2.GetTypeFlag()) != QuestEntity.TypeFlag.NONE) && this.IsQuestRelease(id, oldLv, type))
                {
                    list.Add(id);
                }
            }
        }
        return list;
    }

    public bool IsEncountRaidBoss(int eventId, int day)
    {
        EventRaidMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventRaidMaster>(DataNameKind.Kind.EVENT_RAID);
        int raidDeadQuestId = master.GetRaidDeadQuestId(eventId, day);
        if (SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(raidDeadQuestId, false))
        {
            return true;
        }
        if (this.IsQuestRelease(raidDeadQuestId, -1, CondType.Kind.NONE))
        {
            return true;
        }
        foreach (int num2 in master.GetRaidAliveQuestIds(eventId, day))
        {
            if (this.IsQuestRelease(num2, -1, CondType.Kind.NONE))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsLastWarClear() => 
        this.IsWarClear(ConstantMaster.getValue("LAST_WAR_ID"));

    public bool IsQuestClear(int qid, bool is_quest_after_action = false)
    {
        int beforeClearQuestId = -1;
        if (((SingletonMonoBehaviour<SceneManager>.Instance.GetNowSceneName() == SceneList.getSceneName(SceneList.Type.Terminal)) && is_quest_after_action) && SingletonMonoBehaviour<QuestAfterAction>.Instance.IsActiveCommand())
        {
            beforeClearQuestId = TerminalPramsManager.QuestId;
        }
        return CondType.IsQuestClear(qid, beforeClearQuestId, true);
    }

    private bool IsQuestRelease(QuestReleaseEntity qrd, clsMapCtrl_QuestInfo qinf) => 
        this.IsQuestRelease(qrd, -1, qinf);

    private bool IsQuestRelease(QuestReleaseEntity qrd, int old_val = -1) => 
        this.IsQuestRelease(qrd, old_val, null);

    private bool IsQuestRelease(QuestReleaseEntity qrd, int old_val, clsMapCtrl_QuestInfo qinf)
    {
        bool flag = false;
        switch (qrd.type)
        {
            case 0:
                return true;

            case 1:
                return this.IsQuestClear(qrd.getTargetId(), false);

            case 2:
            {
                int itemId = qrd.getTargetId();
                UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, itemId);
                if (entity != null)
                {
                    flag = entity.getUserItemNum() >= qrd.getValue();
                }
                return flag;
            }
            case 3:
                return flag;

            case 4:
                return flag;

            case 5:
                return flag;

            case 6:
                if (this.IsQuestClear(qrd.questId, false))
                {
                    return true;
                }
                if (old_val >= 0)
                {
                    return (old_val >= qrd.getValue());
                }
                return CondType.IsOpen(CondType.Kind.SVT_LEVEL, qrd.getTargetId(), qrd.getValue());

            case 7:
                if (!this.IsQuestClear(qrd.questId, false))
                {
                    if (old_val >= 0)
                    {
                        return (old_val >= qrd.getValue());
                    }
                    return CondType.IsOpen(CondType.Kind.SVT_LIMIT, qrd.getTargetId(), qrd.getValue());
                }
                return true;

            case 8:
                if (!this.IsQuestClear(qrd.questId, false))
                {
                    return CondType.IsOpen(CondType.Kind.SVT_GET, qrd.getTargetId(), qrd.getValue());
                }
                return true;

            case 9:
                if (!this.IsQuestClear(qrd.questId, false))
                {
                    if (old_val >= 0)
                    {
                        return (old_val >= qrd.getValue());
                    }
                    return CondType.IsOpen(CondType.Kind.SVT_FRIENDSHIP, qrd.getTargetId(), qrd.getValue());
                }
                return true;

            case 10:
                if (!this.IsQuestClear(qrd.questId, false))
                {
                    return CondType.IsOpen(CondType.Kind.SVT_GROUP, qrd.getTargetId(), qrd.getValue());
                }
                return true;

            case 11:
                return CondType.IsOpen(CondType.Kind.EVENT, qrd.getTargetId(), qrd.getValue());

            case 12:
            {
                QuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(qrd.getQuestId());
                if (entity2 != null)
                {
                    long num2 = NetworkManager.getTime();
                    if ((entity2.getOpenedAt() <= num2) && (entity2.getClosedAt() > num2))
                    {
                        flag = true;
                    }
                    return flag;
                }
                return flag;
            }
            case 13:
            {
                QuestEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(qrd.getQuestId());
                if (entity3 != null)
                {
                    flag = entity3.IsOpenByTime(true);
                }
                return flag;
            }
            case 14:
            case 15:
            case 0x10:
            case 0x11:
            case 0x12:
            case 0x13:
            case 20:
            case 0x15:
            case 0x16:
            case 0x17:
            case 0x19:
                return flag;

            case 0x18:
                return CondType.IsMissionAchive(qrd.getTargetId());

            case 0x1a:
            {
                int[] numArray;
                int groupId = qrd.getTargetId();
                flag = CondType.IsNotQuestGroupClear(qrd.getQuestId(), groupId, qrd.getValue(), out numArray, true);
                if (qinf != null)
                {
                    qinf.groupId = groupId;
                    qinf.AddSameGroupQuestIds(numArray);
                }
                return flag;
            }
            case 0x1b:
            case 0x1c:
                return flag;

            case 0x1d:
            {
                int num4 = qrd.getTargetId();
                return CondType.IsQuestGroupClear(qrd.getQuestId(), num4, qrd.getValue(), true);
            }
        }
        return flag;
    }

    public bool IsQuestRelease(int quest_id, int old_val = -1, CondType.Kind old_val_qr_type = 0)
    {
        QuestReleaseEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getEntitys<QuestReleaseEntity>(DataNameKind.Kind.QUEST_RELEASE);
        bool flag = entityArray.Length > 0;
        foreach (QuestReleaseEntity entity in entityArray)
        {
            if (entity.getQuestId() == quest_id)
            {
                int num2 = -1;
                if ((old_val >= 0) && (entity.getType() == old_val_qr_type))
                {
                    num2 = old_val;
                }
                if (!this.IsQuestRelease(entity, num2))
                {
                    return false;
                }
            }
        }
        return flag;
    }

    public bool IsQuestReleaseByServantFriendShip(int svt_id) => 
        this.IsQuestReleaseByServantFriendShip(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantFriendShip(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.IsQuestReleaseByServantFriendShip(svt_id, -1, type_flag);

    public bool IsQuestReleaseByServantFriendShip(int svt_id, int oldFriendShipRank) => 
        this.IsQuestReleaseByServantFriendShip(svt_id, oldFriendShipRank, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantFriendShip(int svt_id, int oldFriendShipRank, QuestEntity.TypeFlag type_flag) => 
        (this.GetReleaseQuestIdByServantFriendShip(svt_id, oldFriendShipRank, type_flag).Count > 0);

    public bool IsQuestReleaseByServantGet(int svt_id, QuestEntity.TypeFlag type_flag = 110) => 
        (this.GetReleaseQuestIdByServantGet(svt_id, type_flag).Count > 0);

    public bool IsQuestReleaseByServantLimit(int svt_id) => 
        this.IsQuestReleaseByServantLimit(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantLimit(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.IsQuestReleaseByServantLimit(svt_id, -1, type_flag);

    public bool IsQuestReleaseByServantLimit(int svt_id, int oldLimitCount) => 
        this.IsQuestReleaseByServantLimit(svt_id, oldLimitCount, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantLimit(int svt_id, int oldLimitCount, QuestEntity.TypeFlag type_flag) => 
        (this.GetReleaseQuestIdByServantLimit(svt_id, oldLimitCount, type_flag).Count > 0);

    public bool IsQuestReleaseByServantLv(int svt_id) => 
        this.IsQuestReleaseByServantLv(svt_id, -1, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantLv(int svt_id, QuestEntity.TypeFlag type_flag) => 
        this.IsQuestReleaseByServantLv(svt_id, -1, type_flag);

    public bool IsQuestReleaseByServantLv(int svt_id, int oldLv) => 
        this.IsQuestReleaseByServantLv(svt_id, oldLv, QuestEntity.TypeFlag.ALL);

    public bool IsQuestReleaseByServantLv(int svt_id, int oldLv, QuestEntity.TypeFlag type_flag) => 
        (this.GetReleaseQuestIdByServantLv(svt_id, oldLv, type_flag).Count > 0);

    public bool IsWarClear(int war_id)
    {
        WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(war_id);
        if (entity == null)
        {
            return false;
        }
        return this.IsQuestClear(entity.lastQuestId, false);
    }

    public bool mfCheck_IsQuestNew(int qid)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) qid };
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST).isEntityExistsFromId(args))
        {
            return false;
        }
        return true;
    }

    public int mfGetQuestPhaseByQuestID(int qid)
    {
        UserQuestMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserQuestMaster>(DataNameKind.Kind.USER_QUEST);
        long[] args = new long[] { NetworkManager.UserId, (long) qid };
        if (master.isEntityExistsFromId(args))
        {
            long[] numArray2 = new long[] { NetworkManager.UserId, (long) qid };
            return master.getEntityFromId<UserQuestEntity>(numArray2).getQuestPhase();
        }
        return 0;
    }

    public void mfInit()
    {
    }

    public List<int> mfIsQuestOpenByItemGet(int iItemID)
    {
        List<int> list = new List<int>();
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(CondType.Kind.ITEM_GET))
        {
            if (iItemID == entity.targetId)
            {
                int num2 = entity.getQuestId();
                if (this.IsQuestRelease(num2, -1, CondType.Kind.NONE))
                {
                    list.Add(num2);
                }
            }
        }
        return list;
    }

    public List<int> mfIsQuestOpenByQuestClear(int iQuestID)
    {
        List<int> list = new List<int>();
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByType(CondType.Kind.QUEST_CLEAR))
        {
            if (iQuestID == entity.targetId)
            {
                int num2 = entity.getQuestId();
                if (this.IsQuestRelease(num2, -1, CondType.Kind.NONE))
                {
                    list.Add(num2);
                }
            }
        }
        return list;
    }

    private bool mfQuestReleaseCheckAlreadyClear(int qid)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(qid).getAfterClear() != 1)
        {
            return false;
        }
        if (!this.IsQuestClear(qid, false))
        {
            return false;
        }
        return true;
    }

    public bool mfQuestReleaseCheckGetEntityByQuestID(int iQuestID, out QuestReleaseEntity rQuestRlsNG, clsMapCtrl_QuestInfo qinf)
    {
        rQuestRlsNG = null;
        if (this.mfQuestReleaseCheckAlreadyClear(iQuestID))
        {
            return false;
        }
        QuestReleaseEntity[] collection = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByQuestID(iQuestID);
        if (collection == null)
        {
            Debug.LogError("NG:QuestID[" + iQuestID + "]>Have'nt QuestReleaseRecorde");
            return false;
        }
        List<QuestReleaseEntity> list = new List<QuestReleaseEntity>();
        list.AddRange(collection);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (a, b) => b.getImagePriority() - a.getImagePriority();
        }
        list.Sort(<>f__am$cache0);
        foreach (QuestReleaseEntity entity in list)
        {
            if (!this.IsQuestRelease(entity, qinf))
            {
                rQuestRlsNG = entity;
                return false;
            }
        }
        return true;
    }

    public void PlayGacha(long user_svt_id, int svt_id, int limit_count, System.Action end_act)
    {
        this.PlayGacha(user_svt_id, svt_id, limit_count, false, end_act);
    }

    public void PlayGacha(long user_svt_id, int svt_id, int limit_count, bool is_event_svt_get, System.Action end_act)
    {
        <PlayGacha>c__AnonStoreyE6 ye = new <PlayGacha>c__AnonStoreyE6 {
            end_act = end_act
        };
        string message = null;
        if (user_svt_id > 0L)
        {
            UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(user_svt_id);
            if (entity != null)
            {
                EventServantEntity entity2 = entity.getEventServant(true);
                if (entity2 != null)
                {
                    if (entity.IsEventJoin())
                    {
                        message = entity2.joinMessage;
                    }
                    else if (is_event_svt_get)
                    {
                        message = entity2.getMessage;
                    }
                }
            }
        }
        List<QuestEntity> releaseQuestEntityByServantGet = this.GetReleaseQuestEntityByServantGet(svt_id, QuestEntity.TypeFlag.FRIENDSHIP);
        if ((releaseQuestEntityByServantGet != null) && (releaseQuestEntityByServantGet.Count > 0))
        {
            message = string.Format(LocalizationManager.Get("SUMMON_OPEN_FRINEDQUEST_NAME"), releaseQuestEntityByServantGet[0].name);
        }
        ScriptManager.PlayGacha(user_svt_id, svt_id, limit_count, true, message, new System.Action(ye.<>m__248));
    }

    public void PlayQuestStartAction(System.Action end_act = null)
    {
        int warId = TerminalPramsManager.WarId;
        int questId = TerminalPramsManager.QuestId;
        if (this.mfCheck_IsQuestNew(questId))
        {
            ScriptManager.PlayQuestStart(warId, questId, end_act);
        }
        else
        {
            end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <PlayGacha>c__AnonStoreyE6
    {
        internal System.Action end_act;

        internal void <>m__248()
        {
            this.end_act.Call();
        }
    }
}

