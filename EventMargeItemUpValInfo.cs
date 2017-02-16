using System;

public class EventMargeItemUpValInfo
{
    public int addCount;
    public int baseFuncId;
    public FuncList.TYPE baseFuncType;
    public FunctionGroupEntity funcGroupEntity;
    public bool isEquipUp;
    public bool isFollower;
    public bool isOtherUp;
    public ItemEntity itemEntity;
    public int member;
    public int priority;
    public int rateCount;
    public string servantName;
    public Target.TYPE targetType;

    public EventMargeItemUpValInfo(EventDropItemUpValInfo dropItemInfo)
    {
        this.member = -1;
        this.servantName = string.Empty;
        this.isFollower = false;
        this.isOtherUp = false;
        this.funcGroupEntity = dropItemInfo.funcGroupEntity;
        this.baseFuncId = dropItemInfo.baseFuncId;
        this.baseFuncType = dropItemInfo.baseFuncType;
        this.targetType = dropItemInfo.targetType;
        this.priority = dropItemInfo.priority;
        this.itemEntity = dropItemInfo.itemEntity;
    }

    public EventMargeItemUpValInfo(int member, string servantName, bool isFollower, bool isOtherUp, EventDropItemUpValInfo dropItemInfo)
    {
        this.member = member;
        this.servantName = servantName;
        this.isFollower = isFollower;
        this.isOtherUp = isOtherUp;
        this.funcGroupEntity = dropItemInfo.funcGroupEntity;
        this.baseFuncId = dropItemInfo.baseFuncId;
        this.baseFuncType = dropItemInfo.baseFuncType;
        this.targetType = dropItemInfo.targetType;
        this.priority = dropItemInfo.priority;
        this.itemEntity = dropItemInfo.itemEntity;
    }

    public int CompPriority(EventMargeItemUpValInfo info)
    {
        if (this.isOtherUp != info.isOtherUp)
        {
            return (!this.isOtherUp ? -1 : 1);
        }
        return (this.priority - info.priority);
    }

    public string GetColorString()
    {
        if (this.isOtherUp)
        {
            return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_OTHER_EFFECT");
        }
        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_EFFECT");
    }

    public string GetEventUpString()
    {
        if ((this.funcGroupEntity != null) && !this.funcGroupEntity.isDispValue)
        {
            return null;
        }
        switch (this.baseFuncType)
        {
            case FuncList.TYPE.EVENT_DROP_RATE_UP:
            case FuncList.TYPE.EVENT_POINT_RATE_UP:
                return LocalizationManager.GetEventPointInfo(0, this.addCount, string.Empty);

            case FuncList.TYPE.ENEMY_ENCOUNT_COPY_RATE_UP:
                return LocalizationManager.GetEventPointInfo(0, this.addCount, string.Empty);

            case FuncList.TYPE.ENEMY_ENCOUNT_RATE_UP:
            case FuncList.TYPE.ADD_STATE:
            case FuncList.TYPE.ADD_STATE_SHORT:
                return LocalizationManager.GetEventPointInfo(this.addCount, this.rateCount, string.Empty);
        }
        return LocalizationManager.GetEventPointInfo(this.addCount, this.rateCount, (this.itemEntity == null) ? string.Empty : this.itemEntity.unit);
    }

    public string GetItemName() => 
        ((this.itemEntity == null) ? string.Empty : this.itemEntity.name);

    public string GetNameTitleString()
    {
        if (this.funcGroupEntity != null)
        {
            return this.funcGroupEntity.name;
        }
        FuncList.TYPE baseFuncType = this.baseFuncType;
        if ((baseFuncType != FuncList.TYPE.ADD_STATE) && (baseFuncType != FuncList.TYPE.ADD_STATE_SHORT))
        {
            if (baseFuncType == FuncList.TYPE.EVENT_DROP_RATE_UP)
            {
                return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_RATE_UP_TITLE");
            }
            return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_UP_TITLE");
        }
        if (this.isOtherUp)
        {
            return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_STATE_UP_OTHER");
        }
        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_STATE_UP");
    }

    public string GetNameTotalString()
    {
        if (this.funcGroupEntity != null)
        {
            return this.funcGroupEntity.nameTotal;
        }
        FuncList.TYPE baseFuncType = this.baseFuncType;
        if ((baseFuncType != FuncList.TYPE.ADD_STATE) && (baseFuncType != FuncList.TYPE.ADD_STATE_SHORT))
        {
            return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_UP_TOTAL_TITLE");
        }
        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_STATE_UP_TOTAL_TITLE");
    }

    public string GetServantName()
    {
        if (this.member >= 0)
        {
            return string.Format(LocalizationManager.Get(!this.isFollower ? "PARTY_ORGANIZATION_EVENT_MEMBER" : "PARTY_ORGANIZATION_EVENT_MEMBER_SUPPORT"), this.servantName);
        }
        return string.Empty;
    }

    public string GetTargetString()
    {
        switch (this.baseFuncType)
        {
            case FuncList.TYPE.ADD_STATE:
            case FuncList.TYPE.ADD_STATE_SHORT:
                switch (this.targetType)
                {
                    case Target.TYPE.SELF:
                        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_TARGET_SELF");

                    case Target.TYPE.PT_ALL:
                        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_TARGET_PT_ALL");

                    case Target.TYPE.PT_FULL:
                        return LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_TARGET_PT_FULL");
                }
                break;
        }
        return string.Empty;
    }
}

