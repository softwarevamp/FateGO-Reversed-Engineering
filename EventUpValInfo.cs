using System;
using System.Collections.Generic;

public class EventUpValInfo
{
    public List<EventDropUpValInfo> dropList;
    public int equipSvtId;
    public EventUpValSetupInfo setupInfo;
    public int svtId;

    public EventUpValInfo(EventUpValSetupInfo setupInfo, int svtId)
    {
        this.dropList = new List<EventDropUpValInfo>();
        this.setupInfo = setupInfo;
        this.svtId = svtId;
    }

    public EventUpValInfo(EventUpValSetupInfo setupInfo, int svtId, int equipSvtId)
    {
        this.dropList = new List<EventDropUpValInfo>();
        this.setupInfo = setupInfo;
        this.svtId = svtId;
        this.equipSvtId = equipSvtId;
    }

    public void ClearEquipSvtId()
    {
        this.equipSvtId = 0;
    }

    public EventDropItemUpValInfo[] GetDropItemList(int member)
    {
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        List<EventDropItemUpValInfo> list = new List<EventDropItemUpValInfo>();
        foreach (EventDropUpValInfo info in this.dropList)
        {
            switch (info.GetFuncType())
            {
                case FuncList.TYPE.ADD_STATE:
                case FuncList.TYPE.ADD_STATE_SHORT:
                case FuncList.TYPE.ENEMY_ENCOUNT_COPY_RATE_UP:
                case FuncList.TYPE.ENEMY_ENCOUNT_RATE_UP:
                {
                    EventDropItemUpValInfo item = new EventDropItemUpValInfo(member, info);
                    list.Add(item);
                    break;
                }
                default:
                    foreach (ItemEntity entity in master.GetIndividualityList(info.individuality))
                    {
                        EventDropItemUpValInfo info3 = new EventDropItemUpValInfo(member, info, entity);
                        list.Add(info3);
                    }
                    break;
            }
        }
        return list.ToArray();
    }

    public bool IsEmpry()
    {
        if (this.dropList.Count > 0)
        {
            return false;
        }
        return true;
    }

    public void SetEquipSvtId(int equipSvtId)
    {
        this.equipSvtId = equipSvtId;
    }
}

