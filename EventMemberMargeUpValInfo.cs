using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventMemberMargeUpValInfo
{
    [CompilerGenerated]
    private static Comparison<EventMargeItemUpValInfo> <>f__am$cache4;
    public bool isFollower;
    public List<EventMargeItemUpValInfo> margeList;
    public int member;
    public string servantName;

    public EventMemberMargeUpValInfo(int member, bool isFollower)
    {
        this.margeList = new List<EventMargeItemUpValInfo>();
        this.member = member;
        this.servantName = string.Empty;
        this.isFollower = isFollower;
    }

    public EventMemberMargeUpValInfo(int member, string servantName, bool isFollower)
    {
        this.margeList = new List<EventMargeItemUpValInfo>();
        this.member = member;
        this.servantName = servantName;
        this.isFollower = isFollower;
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
            bool isOtherUp = this.member != dropItemInfo.member;
            bool flag2 = false;
            switch (dropItemInfo.targetType)
            {
                case Target.TYPE.PT_ALL:
                case Target.TYPE.PT_FULL:
                    flag2 = true;
                    break;
            }
            if (!isOtherUp || flag2)
            {
                if ((dropItemInfo.baseFuncType != FuncList.TYPE.ADD_STATE) && (dropItemInfo.baseFuncType != FuncList.TYPE.ADD_STATE_SHORT))
                {
                    if ((dropItemInfo.baseFuncType != FuncList.TYPE.ENEMY_ENCOUNT_COPY_RATE_UP) && (dropItemInfo.baseFuncType != FuncList.TYPE.ENEMY_ENCOUNT_RATE_UP))
                    {
                        EventMargeItemUpValInfo item = null;
                        foreach (EventMargeItemUpValInfo info6 in this.margeList)
                        {
                            if (((info6.baseFuncId == dropItemInfo.baseFuncId) && (info6.itemEntity == dropItemInfo.itemEntity)) && (info6.isOtherUp == isOtherUp))
                            {
                                item = info6;
                                break;
                            }
                        }
                        if (item == null)
                        {
                            item = new EventMargeItemUpValInfo(this.member, this.servantName, this.isFollower, isOtherUp, dropItemInfo);
                            this.margeList.Add(item);
                        }
                        item.addCount += dropItemInfo.addCount;
                        item.rateCount += dropItemInfo.rateCount;
                    }
                    else
                    {
                        EventMargeItemUpValInfo info3 = null;
                        foreach (EventMargeItemUpValInfo info4 in this.margeList)
                        {
                            if ((info4.baseFuncId == dropItemInfo.baseFuncId) && (info4.isOtherUp == isOtherUp))
                            {
                                info3 = info4;
                                break;
                            }
                        }
                        if (info3 == null)
                        {
                            info3 = new EventMargeItemUpValInfo(this.member, this.servantName, this.isFollower, isOtherUp, dropItemInfo);
                            this.margeList.Add(info3);
                        }
                        info3.addCount += dropItemInfo.addCount;
                        info3.rateCount += dropItemInfo.rateCount;
                    }
                }
                else
                {
                    EventMargeItemUpValInfo info = null;
                    foreach (EventMargeItemUpValInfo info2 in this.margeList)
                    {
                        if ((info2.baseFuncId == dropItemInfo.baseFuncId) && (info2.isOtherUp == isOtherUp))
                        {
                            info = info2;
                            break;
                        }
                    }
                    if (info == null)
                    {
                        info = new EventMargeItemUpValInfo(this.member, this.servantName, this.isFollower, isOtherUp, dropItemInfo);
                        this.margeList.Add(info);
                    }
                    info.addCount += dropItemInfo.addCount;
                    info.rateCount += dropItemInfo.rateCount;
                }
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

