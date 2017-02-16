using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class QuestTree : SingletonTemplate<QuestTree>
{
    [CompilerGenerated]
    private static Comparison<clsMapCtrl_QuestInfo> <>f__am$cache6;
    [CompilerGenerated]
    private static Comparison<clsMapCtrl_WarInfo> <>f__am$cache7;
    private clsMapCtrl_RootInfo mcMapCtrlP = new clsMapCtrl_RootInfo();
    private List<clsMapCtrl_MapGimmickInfo> mlMapGimmickInfoAll = new List<clsMapCtrl_MapGimmickInfo>();
    private List<clsMapCtrl_QuestInfo> mlQuestInfoAll = new List<clsMapCtrl_QuestInfo>();
    private List<clsMapCtrl_SpotInfo> mlSpotInfoAll = new List<clsMapCtrl_SpotInfo>();
    private List<clsMapCtrl_SpotRoadInfo> mlSpotRoadInfoAll = new List<clsMapCtrl_SpotRoadInfo>();
    private List<clsMapCtrl_WarInfo> mlWarInfoAll = new List<clsMapCtrl_WarInfo>();

    public bool CheckMapGimmickCond(clsMapCtrl_MapGimmickInfo map_gimmick_inf)
    {
        long time = NetworkManager.getTime();
        MapGimmickEntity entity = map_gimmick_inf.mfGetMine();
        if (entity == null)
        {
            return false;
        }
        if (!this.CheckSpotCond(entity.dispCondType, entity.dispTargetId))
        {
            return false;
        }
        if (!this.CheckSpotCond(entity.dispCondType2, entity.dispTargetId2))
        {
            return false;
        }
        return entity.IsEnableTime(time);
    }

    public bool CheckSpotCond(int spot_cond_type, int target_id)
    {
        bool flag = false;
        int missionId = 0;
        if ((TerminalSceneComponent.Instance != null) && (TerminalSceneComponent.Instance.TransitionInfo != null))
        {
            missionId = TerminalSceneComponent.Instance.TransitionInfo.missionId;
        }
        bool flag2 = missionId == 0;
        switch (spot_cond_type)
        {
            case 1:
                return true;

            case 2:
                if (!TerminalPramsManager.Debug_IsQuestReleaseAll)
                {
                    if (this.mcQuestCheckP.IsQuestClear(target_id, flag2))
                    {
                        flag = true;
                    }
                    return flag;
                }
                return true;

            case 3:
                if (!TerminalPramsManager.Debug_IsQuestReleaseAll)
                {
                    if (!this.mcQuestCheckP.IsQuestClear(target_id, flag2))
                    {
                        flag = true;
                    }
                    return flag;
                }
                return false;

            case 4:
                return false;

            case 5:
                if (!TerminalPramsManager.Debug_IsQuestReleaseAll)
                {
                    if (target_id == missionId)
                    {
                        return false;
                    }
                    return CondType.IsMissionAchive(target_id);
                }
                return true;
        }
        return flag;
    }

    private long GetEndTime(clsMapCtrl_QuestInfo qinf)
    {
        long endTime = 0L;
        int qid = qinf.mfGetQuestID();
        foreach (QuestReleaseEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestReleaseMaster>(DataNameKind.Kind.QUEST_RELEASE).getListByQuestID(qid))
        {
            EventQuestEntity entity2;
            switch (entity.getType())
            {
                case 11:
                {
                    entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventQuestMaster>(DataNameKind.Kind.EVENT_QUEST).getData(qid, GameEventType.TYPE.NONE);
                    if (entity2 != null)
                    {
                        break;
                    }
                    continue;
                }
                case 12:
                    endTime = qinf.mfGetMine().getClosedAt();
                    goto Label_00ED;

                case 13:
                    endTime = qinf.mfGetMine().GetEndTime(true);
                    goto Label_00ED;

                default:
                    goto Label_00ED;
            }
            EventEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(entity2.getEventId());
            if (((entity3 == null) || (entity3.getEventType() != 7)) || !entity3.IsOpen(true))
            {
                continue;
            }
            endTime = entity3.GetEndTime(true);
        Label_00ED:
            if (endTime > 0L)
            {
                return endTime;
            }
        }
        return endTime;
    }

    public int GetQuestCount(int war_id)
    {
        clsMapCtrl_WarInfo info = this.mfGetWarInfoByWarID(war_id);
        if (info == null)
        {
            return 0;
        }
        int num = 0;
        foreach (clsMapCtrl_SpotInfo info2 in info.mfGetSpotListsP())
        {
            if (info2.mfGetDispType() == clsMapCtrl_SpotInfo.enDispType.Normal)
            {
                num += info2.mfGetQuestcount();
            }
        }
        return num;
    }

    public clsMapCtrl_QuestInfo GetQuestInfo(int quest_id)
    {
        foreach (clsMapCtrl_QuestInfo info in this.mlQuestInfoAll)
        {
            if (info.mfGetQuestID() == quest_id)
            {
                return info;
            }
        }
        return null;
    }

    private List<clsMapCtrl_QuestInfo> GetQuestInfoByWarId(int war_id)
    {
        List<clsMapCtrl_QuestInfo> list = new List<clsMapCtrl_QuestInfo>();
        foreach (clsMapCtrl_QuestInfo info in this.mlQuestInfoAll)
        {
            if (info.mfGetWarID() == war_id)
            {
                list.Add(info);
            }
        }
        return list;
    }

    public int GetWarID_ByQuestID(int quest_id)
    {
        foreach (clsMapCtrl_QuestInfo info in this.mlQuestInfoAll)
        {
            if (info.mfGetMine().getQuestId() == quest_id)
            {
                return info.mfGetWarID();
            }
        }
        return 0;
    }

    public List<clsMapCtrl_WarInfo> GetWarInfoAll() => 
        this.mlWarInfoAll;

    public List<clsMapCtrl_WarInfo> GetWarInfoAll_OrderReverse()
    {
        List<clsMapCtrl_WarInfo> list = new List<clsMapCtrl_WarInfo>(this.mlWarInfoAll);
        list.Reverse();
        return list;
    }

    public clsMapCtrl_WarInfo GetWarInfoByEventID(int event_id)
    {
        if (this.mcMapCtrlP != null)
        {
            return this.mcMapCtrlP.mfGetChildByEventID(event_id);
        }
        return null;
    }

    public void Init()
    {
        this.mcQuestCheckP.mfInit();
        if (SingletonMonoBehaviour<QuestAfterAction>.Instance != null)
        {
            SingletonMonoBehaviour<QuestAfterAction>.Instance.Init();
            SingletonMonoBehaviour<QuestAfterAction>.Instance.CreateCommandBuf();
        }
        this.mfBaseTree_Make();
        this.mfBaseTree_OpenCheck();
    }

    public bool IsActiveEventWar(int war_id)
    {
        WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(war_id);
        if (entity == null)
        {
            return false;
        }
        EventEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(entity.eventId);
        if (entity2 == null)
        {
            return false;
        }
        long num = NetworkManager.getTime();
        return (this.IsWarOpen(war_id) && (num < entity2.getEventEndedAt()));
    }

    private bool IsWarNew(List<clsMapCtrl_QuestInfo> qinfs)
    {
        foreach (clsMapCtrl_QuestInfo info in qinfs)
        {
            if (!this.mcQuestCheckP.mfCheck_IsQuestNew(info.mfGetQuestID()))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsWarNew(int war_id)
    {
        List<clsMapCtrl_QuestInfo> questInfoByWarId = this.GetQuestInfoByWarId(war_id);
        return this.IsWarNew(questInfoByWarId);
    }

    public bool IsWarOpen(int war_id)
    {
        List<clsMapCtrl_QuestInfo> questInfoByWarId = this.GetQuestInfoByWarId(war_id);
        if (this.mcQuestCheckP.IsWarClear(war_id))
        {
            return true;
        }
        foreach (clsMapCtrl_QuestInfo info in questInfoByWarId)
        {
            if (info.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Normal)
            {
                return true;
            }
        }
        return false;
    }

    private void mfBaseTree_ClearCheck_War()
    {
        for (int i = 0; i < this.mlWarInfoAll.Count; i++)
        {
            clsMapCtrl_WarInfo info = this.mlWarInfoAll[i];
            int num2 = info.mfGetWarID();
            List<clsMapCtrl_QuestInfo> questInfoByWarId = this.GetQuestInfoByWarId(num2);
            clsMapCtrl_WarInfo.enStatus none = clsMapCtrl_WarInfo.enStatus.None;
            bool flag = this.IsWarOpen(num2);
            int eventId = info.GetEventId();
            if (!flag && (eventId > 0))
            {
                EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(eventId);
                if (entity != null)
                {
                    long num4 = NetworkManager.getTime();
                    if ((num4 >= entity.getEventNoticeAt()) && (num4 < entity.getEventFinishedAt()))
                    {
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                none = clsMapCtrl_WarInfo.enStatus.Normal;
                if (this.IsWarNew(questInfoByWarId))
                {
                    none = clsMapCtrl_WarInfo.enStatus.New;
                }
                else
                {
                    if (this.mcQuestCheckP.IsWarClear(num2))
                    {
                        none = clsMapCtrl_WarInfo.enStatus.Clear;
                    }
                    bool flag3 = true;
                    foreach (clsMapCtrl_QuestInfo info2 in questInfoByWarId)
                    {
                        if (!this.mcQuestCheckP.IsQuestClear(info2.mfGetQuestID(), false))
                        {
                            flag3 = false;
                            break;
                        }
                    }
                    if (flag3)
                    {
                        none = clsMapCtrl_WarInfo.enStatus.Complete;
                    }
                }
            }
            info.mfSetStatus(none);
        }
    }

    private void mfBaseTree_Make()
    {
        this.mcMapCtrlP.mfReset();
        this.mlQuestInfoAll.Clear();
        this.mlSpotInfoAll.Clear();
        this.mlSpotRoadInfoAll.Clear();
        this.mlMapGimmickInfoAll.Clear();
        this.mlWarInfoAll.Clear();
        EventQuestMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventQuestMaster>(DataNameKind.Kind.EVENT_QUEST);
        foreach (WarEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<WarEntity>(DataNameKind.Kind.WAR))
        {
            clsMapCtrl_WarInfo item = this.mcMapCtrlP.mfAddChild(entity.getWarId());
            this.mlWarInfoAll.Add(item);
            foreach (SpotEntity entity2 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT).getList(entity.id))
            {
                clsMapCtrl_SpotInfo info2 = item.mfAddChild(entity2.getSpotId());
                this.mlSpotInfoAll.Add(info2);
                foreach (QuestEntity entity3 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getList(entity2.id))
                {
                    clsMapCtrl_QuestInfo qinf = info2.mfAddChild(entity3.getQuestId());
                    qinf.mfSetWarID(item.mfGetWarID());
                    qinf.mMapCtrl_WarInfo = item;
                    qinf.mMapCtrl_SpotInfo = info2;
                    long endTime = this.GetEndTime(qinf);
                    qinf.SetEndTime(endTime);
                    this.mlQuestInfoAll.Add(qinf);
                    foreach (QuestPhaseEntity entity4 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestPhaseMaster>(DataNameKind.Kind.QUEST_PHASE).getList(entity3.getQuestId()))
                    {
                        qinf.mfAddChild(entity4.getQuestId(), entity4.getPhase());
                    }
                }
            }
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = (a, b) => b.mfGetMine().getPriority() - a.mfGetMine().getPriority();
            }
            this.mlQuestInfoAll.Sort(<>f__am$cache6);
            foreach (SpotRoadEntity entity5 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotRoadMaster>(DataNameKind.Kind.SPOT_ROAD).getList(entity.id))
            {
                clsMapCtrl_SpotRoadInfo info4 = item.mfAddSpotRoad(entity5.getSpotRoadId());
                this.mlSpotRoadInfoAll.Add(info4);
            }
            foreach (MapGimmickEntity entity6 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<MapGimmickMaster>(DataNameKind.Kind.MAP_GIMMICK).getList(entity.id))
            {
                clsMapCtrl_MapGimmickInfo info5 = item.mfAddMapGimmick(entity6.id);
                this.mlMapGimmickInfoAll.Add(info5);
            }
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = (a, b) => b.mfGetMine().priority - a.mfGetMine().priority;
        }
        this.mlWarInfoAll.Sort(<>f__am$cache7);
    }

    private void mfBaseTree_OpenCheck()
    {
        this.mfBaseTree_OpenCheck_Quest();
        this.mfBaseTree_StatusCheck_Spot();
        this.mfBaseTree_ClearCheck_War();
    }

    private void mfBaseTree_OpenCheck_Quest()
    {
        long num2 = NetworkManager.getTime();
        QuestReleaseEntity rQuestRlsNG = new QuestReleaseEntity();
        int count = this.mlQuestInfoAll.Count;
        for (int i = 0; i < count; i++)
        {
            clsMapCtrl_QuestInfo qinf = this.mlQuestInfoAll[i];
            QuestEntity entity2 = qinf.mfGetMine();
            int iQuestID = qinf.mfGetQuestID();
            bool flag = this.mcQuestCheckP.mfQuestReleaseCheckGetEntityByQuestID(iQuestID, out rQuestRlsNG, qinf);
            qinf.mfSetDispType(clsMapCtrl_QuestInfo.enDispType.None, CondType.Kind.NONE, 0, 0, 0);
            qinf.mfSetTouchType(clsMapCtrl_QuestInfo.enTouchType.Disable);
            if (flag)
            {
                qinf.mfSetDispType(clsMapCtrl_QuestInfo.enDispType.Normal, CondType.Kind.NONE, 0, 0, 0);
                qinf.mfSetTouchType(clsMapCtrl_QuestInfo.enTouchType.Enable);
            }
            else if (((rQuestRlsNG == null) || (entity2.getClosedAt() <= num2)) || ((rQuestRlsNG.getClosedMessageId() == 0) || (entity2.getNoticeAt() > num2)))
            {
                qinf.mfSetDispType(clsMapCtrl_QuestInfo.enDispType.None, CondType.Kind.NONE, 0, 0, 0);
                qinf.mfSetTouchType(clsMapCtrl_QuestInfo.enTouchType.Disable);
            }
            else
            {
                QuestReleaseEntity entity3 = rQuestRlsNG;
                qinf.mfSetDispType(clsMapCtrl_QuestInfo.enDispType.Closed, (CondType.Kind) entity3.getType(), entity3.getTargetId(), entity3.getValue(), entity3.getClosedMessageId());
                qinf.mfSetTouchType(clsMapCtrl_QuestInfo.enTouchType.Disable);
            }
            QuestEntity.enForceOperation operation = (QuestEntity.enForceOperation) entity2.getForceOperation();
            if (TerminalPramsManager.Debug_IsQuestReleaseAll)
            {
                operation = QuestEntity.enForceOperation.FORCE_OPEN;
            }
            if (operation != QuestEntity.enForceOperation.NONE)
            {
                clsMapCtrl_QuestInfo.enDispType normal = clsMapCtrl_QuestInfo.enDispType.Normal;
                clsMapCtrl_QuestInfo.enTouchType enable = clsMapCtrl_QuestInfo.enTouchType.Enable;
                if (operation == QuestEntity.enForceOperation.FORCE_CLOSE)
                {
                    normal = clsMapCtrl_QuestInfo.enDispType.None;
                    enable = clsMapCtrl_QuestInfo.enTouchType.Disable;
                }
                qinf.mfSetDispType(normal, CondType.Kind.NONE, 0, 0, 0);
                qinf.mfSetTouchType(enable);
            }
            bool tIsNew = this.mcQuestCheckP.mfCheck_IsQuestNew(iQuestID);
            qinf.mfSetIsNew(tIsNew);
            int iQuestPhase = this.mcQuestCheckP.mfGetQuestPhaseByQuestID(iQuestID);
            qinf.mfSetQuestPhase(iQuestPhase);
        }
    }

    private void mfBaseTree_StatusCheck_Spot()
    {
        int num;
        Hashtable hashtable = new Hashtable();
        foreach (clsMapCtrl_QuestInfo info in this.mlQuestInfoAll)
        {
            if (!this.mcQuestCheckP.IsQuestClear(info.mfGetQuestID(), false) && (info.mfGetDispType() != clsMapCtrl_QuestInfo.enDispType.None))
            {
                num = info.mfGetSpotID();
                if (hashtable.ContainsKey(num))
                {
                    int num2 = (int) hashtable[num];
                    num2++;
                    hashtable[num] = num2;
                }
                else
                {
                    hashtable.Add(num, 1);
                }
            }
        }
        SpotEntity entity = null;
        foreach (clsMapCtrl_SpotInfo info2 in this.mlSpotInfoAll)
        {
            num = info2.mfGetSpotID();
            info2.mfSetDispType(clsMapCtrl_SpotInfo.enDispType.None);
            info2.mfSetTouchType(clsMapCtrl_SpotInfo.enTouchType.Disable);
            entity = info2.mfGetMine();
            if (this.CheckSpotCond(entity.getDispCondType1(), entity.getDispTargetId1()) && this.CheckSpotCond(entity.getDispCondType2(), entity.getDispTargetId2()))
            {
                if (this.CheckSpotCond(entity.getActiveCondType(), entity.getActiveTargetId()))
                {
                    info2.mfSetDispType(clsMapCtrl_SpotInfo.enDispType.Normal);
                    info2.mfSetTouchType(clsMapCtrl_SpotInfo.enTouchType.Enable);
                }
                else
                {
                    info2.mfSetDispType(clsMapCtrl_SpotInfo.enDispType.Glay);
                    info2.mfSetTouchType(clsMapCtrl_SpotInfo.enTouchType.Disable);
                }
            }
            else
            {
                info2.mfSetDispType(clsMapCtrl_SpotInfo.enDispType.None);
                info2.mfSetTouchType(clsMapCtrl_SpotInfo.enTouchType.Disable);
            }
            info2.mfSetQuestcount(0);
            if (hashtable.ContainsKey(num))
            {
                int iQuestcount = (int) hashtable[num];
                info2.mfSetQuestcount(iQuestcount);
            }
        }
        foreach (clsMapCtrl_SpotRoadInfo info3 in this.mlSpotRoadInfoAll)
        {
            info3.mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.None);
            SpotRoadEntity entity2 = info3.mfGetMine();
            if (this.CheckSpotCond(entity2.getDispCondType(), entity2.getDispTargetId()))
            {
                if (this.CheckSpotCond(entity2.getActiveCondType(), entity2.getActiveTargetId()))
                {
                    info3.mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.Normal);
                }
                else
                {
                    info3.mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.Glay);
                }
            }
            else
            {
                info3.mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.None);
            }
        }
        foreach (clsMapCtrl_MapGimmickInfo info4 in this.mlMapGimmickInfoAll)
        {
            info4.mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.None);
            if (this.CheckMapGimmickCond(info4))
            {
                info4.mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.Normal);
            }
            else
            {
                info4.mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.None);
            }
        }
    }

    public clsMapCtrl_RootInfo mfGetMapCtrlP() => 
        this.mcMapCtrlP;

    public List<clsMapCtrl_QuestInfo> mfGetQuestInfoListP() => 
        this.mlQuestInfoAll;

    public WarEntity mfGetWarEntityByWarID(int iWarID)
    {
        clsMapCtrl_WarInfo info = null;
        info = this.mfGetWarInfoByWarID(iWarID);
        if (info != null)
        {
            return info.mfGetMine();
        }
        return null;
    }

    public clsMapCtrl_WarInfo mfGetWarInfoByWarID(int iWarID)
    {
        if (this.mcMapCtrlP != null)
        {
            return this.mcMapCtrlP.mfGetChildByWarID(iWarID);
        }
        return null;
    }

    private clsQuestCheck mcQuestCheckP =>
        SingletonTemplate<clsQuestCheck>.Instance;
}

