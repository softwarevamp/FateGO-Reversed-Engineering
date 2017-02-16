using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class clsMapCtrl_RootInfo
{
    [CompilerGenerated]
    private static Comparison<clsMapCtrl_WarInfo> <>f__am$cache1;
    [CompilerGenerated]
    private static Comparison<clsMapCtrl_WarInfo> <>f__am$cache2;
    private List<clsMapCtrl_WarInfo> mcWarInfos = new List<clsMapCtrl_WarInfo>();

    public clsMapCtrl_RootInfo()
    {
        this.mcWarInfos.Clear();
    }

    public List<clsMapCtrl_WarInfo> GetChapterWarList()
    {
        List<clsMapCtrl_WarInfo> list = new List<clsMapCtrl_WarInfo>();
        foreach (clsMapCtrl_WarInfo info in this.mcWarInfos)
        {
            if (((info.GetEventId() <= 0) && (WarEntity.CALDEAGATE_ID != info.mfGetWarID())) && (info.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.None))
            {
                list.Add(info);
            }
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (a, b) => b.mfGetMine().priority - a.mfGetMine().priority;
        }
        list.Sort(<>f__am$cache1);
        return list;
    }

    private List<clsMapCtrl_WarInfo> GetEventWarList()
    {
        List<clsMapCtrl_WarInfo> list = new List<clsMapCtrl_WarInfo>();
        foreach (clsMapCtrl_WarInfo info in this.mcWarInfos)
        {
            int eventId = info.GetEventId();
            if (eventId > 0)
            {
                EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(eventId);
                if ((entity != null) && (entity.getEventType() == 12))
                {
                    list.Add(info);
                }
            }
        }
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = (a, b) => b.mfGetMine().priority - a.mfGetMine().priority;
        }
        list.Sort(<>f__am$cache2);
        return list;
    }

    public List<clsMapCtrl_WarInfo> GetEventWarList_AfterEnd()
    {
        List<clsMapCtrl_WarInfo> list = new List<clsMapCtrl_WarInfo>();
        List<clsMapCtrl_WarInfo> eventWarList = this.GetEventWarList();
        long num = NetworkManager.getTime();
        foreach (clsMapCtrl_WarInfo info in eventWarList)
        {
            EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(info.GetEventId());
            if ((num >= entity.getEventEndedAt()) && (num < entity.getEventFinishedAt()))
            {
                list.Add(info);
            }
        }
        return list;
    }

    public List<clsMapCtrl_WarInfo> GetEventWarList_BeforeEnd()
    {
        List<clsMapCtrl_WarInfo> list = new List<clsMapCtrl_WarInfo>();
        List<clsMapCtrl_WarInfo> eventWarList = this.GetEventWarList();
        long num = NetworkManager.getTime();
        foreach (clsMapCtrl_WarInfo info in eventWarList)
        {
            EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(info.GetEventId());
            if ((num >= entity.getEventNoticeAt()) && (num < entity.getEventEndedAt()))
            {
                list.Add(info);
            }
        }
        return list;
    }

    public clsMapCtrl_WarInfo mfAddChild(int war_id)
    {
        clsMapCtrl_WarInfo item = new clsMapCtrl_WarInfo();
        item.mfSetMine(war_id);
        this.mcWarInfos.Add(item);
        return item;
    }

    public clsMapCtrl_WarInfo mfGetChildByEventID(int event_id)
    {
        if (this.mcWarInfos != null)
        {
            foreach (clsMapCtrl_WarInfo info in this.mcWarInfos)
            {
                if (info.mfGetMine().eventId == event_id)
                {
                    return info;
                }
            }
        }
        return null;
    }

    public clsMapCtrl_WarInfo mfGetChildByWarID(int iWarID)
    {
        if (this.mcWarInfos != null)
        {
            foreach (clsMapCtrl_WarInfo info in this.mcWarInfos)
            {
                if (info.mfGetMine().id == iWarID)
                {
                    return info;
                }
            }
        }
        return null;
    }

    public List<clsMapCtrl_WarInfo> mfGetChildListsP()
    {
        if (this.mcWarInfos != null)
        {
            return this.mcWarInfos;
        }
        return null;
    }

    public void mfReset()
    {
        if (this.mcWarInfos != null)
        {
            for (int i = 0; i < this.mcWarInfos.Count; i++)
            {
                this.mcWarInfos[i].mfReset();
            }
            this.mcWarInfos.Clear();
        }
    }
}

