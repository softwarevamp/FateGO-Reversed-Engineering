using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventPartyMargeUpValInfo
{
    [CompilerGenerated]
    private static Comparison<EventMargeItemUpValInfo> <>f__am$cache4;
    public bool[] isFollowerList;
    public List<EventMargeItemUpValInfo> margeList = new List<EventMargeItemUpValInfo>();
    public string[] servantNameList;
    public int[] svtIdList;

    public EventPartyMargeUpValInfo(int[] svtIdList, string[] servantNameList, bool[] isFollowerList)
    {
        this.svtIdList = svtIdList;
        this.servantNameList = servantNameList;
        this.isFollowerList = isFollowerList;
    }

    public void Add(EventDropItemUpValInfo[] dropItemInfoList)
    {
        if (dropItemInfoList != null)
        {
            foreach (EventDropItemUpValInfo info in dropItemInfoList)
            {
                this.Add(info);
            }
        }
    }

    public void Add(EventDropItemUpValInfo dropItemInfo)
    {
        if (dropItemInfo != null)
        {
            bool flag = false;
            switch (dropItemInfo.targetType)
            {
                case Target.TYPE.PT_ALL:
                case Target.TYPE.PT_FULL:
                    flag = true;
                    break;
            }
            if ((dropItemInfo.baseFuncType != FuncList.TYPE.ADD_STATE) && (dropItemInfo.baseFuncType != FuncList.TYPE.ADD_STATE_SHORT))
            {
                if ((dropItemInfo.baseFuncType != FuncList.TYPE.ENEMY_ENCOUNT_COPY_RATE_UP) && (dropItemInfo.baseFuncType != FuncList.TYPE.ENEMY_ENCOUNT_RATE_UP))
                {
                    EventMargeItemUpValInfo item = null;
                    foreach (EventMargeItemUpValInfo info8 in this.margeList)
                    {
                        if ((info8.baseFuncId == dropItemInfo.baseFuncId) && (info8.itemEntity == dropItemInfo.itemEntity))
                        {
                            item = info8;
                            break;
                        }
                    }
                    if (item == null)
                    {
                        item = new EventMargeItemUpValInfo(dropItemInfo);
                        this.margeList.Add(item);
                    }
                    item.isEquipUp |= dropItemInfo.isEquipUp;
                    item.addCount += dropItemInfo.addCount;
                    item.rateCount += dropItemInfo.rateCount;
                }
                else
                {
                    EventMargeItemUpValInfo info5 = null;
                    foreach (EventMargeItemUpValInfo info6 in this.margeList)
                    {
                        if (info6.baseFuncId == dropItemInfo.baseFuncId)
                        {
                            info5 = info6;
                            break;
                        }
                    }
                    if (info5 == null)
                    {
                        info5 = new EventMargeItemUpValInfo(dropItemInfo);
                        this.margeList.Add(info5);
                    }
                    info5.isEquipUp |= dropItemInfo.isEquipUp;
                    info5.addCount += dropItemInfo.addCount;
                    info5.rateCount += dropItemInfo.rateCount;
                }
            }
            else if (flag)
            {
                for (int i = 0; i < this.svtIdList.Length; i++)
                {
                    if (this.svtIdList[i] <= 0)
                    {
                        continue;
                    }
                    EventMargeItemUpValInfo info = null;
                    foreach (EventMargeItemUpValInfo info2 in this.margeList)
                    {
                        if ((info2.member == i) && (info2.baseFuncId == dropItemInfo.baseFuncId))
                        {
                            info = info2;
                            break;
                        }
                    }
                    if (info == null)
                    {
                        info = new EventMargeItemUpValInfo(i, this.servantNameList[i], this.isFollowerList[i], false, dropItemInfo);
                        this.margeList.Add(info);
                    }
                    info.isEquipUp |= dropItemInfo.isEquipUp;
                    info.addCount += dropItemInfo.addCount;
                    info.rateCount += dropItemInfo.rateCount;
                }
            }
            else
            {
                EventMargeItemUpValInfo info3 = null;
                foreach (EventMargeItemUpValInfo info4 in this.margeList)
                {
                    if ((info4.member == dropItemInfo.member) && (info4.baseFuncId == dropItemInfo.baseFuncId))
                    {
                        info3 = info4;
                        break;
                    }
                }
                if (info3 == null)
                {
                    info3 = new EventMargeItemUpValInfo(dropItemInfo.member, this.servantNameList[dropItemInfo.member], this.isFollowerList[dropItemInfo.member], false, dropItemInfo);
                    this.margeList.Add(info3);
                }
                info3.isEquipUp |= dropItemInfo.isEquipUp;
                info3.addCount += dropItemInfo.addCount;
                info3.rateCount += dropItemInfo.rateCount;
            }
        }
    }

    public int GetCount() => 
        this.margeList.Count;

    public EventMargeItemUpValInfo[] GetList()
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = (a, b) => a.CompPriority(b);
        }
        this.margeList.Sort(<>f__am$cache4);
        return this.margeList.ToArray();
    }

    public EventMargeItemUpValInfo GetMargeItem(int index) => 
        ((this.margeList.Count <= index) ? null : this.margeList[index]);

    public bool IsEmpry()
    {
        if (this.margeList.Count > 0)
        {
            return false;
        }
        return true;
    }
}

