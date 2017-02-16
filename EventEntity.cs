using System;
using System.Runtime.InteropServices;

public class EventEntity : DataEntityBase
{
    public int bannerId;
    public int bannerPriority;
    public string detail;
    public long endedAt;
    public long finishedAt;
    public int iconId;
    public int id;
    public int intervalHours;
    public string linkBody;
    public int linkType;
    public long materialOpenedAt;
    public int myroomBgId;
    public int myroomBgmId;
    public string name;
    public long noticeAt;
    public int noticeBannerId;
    public int openHours;
    public int openType;
    protected long shopClosedAt;
    public long startedAt;
    public int type;

    public long cmpShopClosedAt(EventEntity entity)
    {
        if (this.shopClosedAt == entity.shopClosedAt)
        {
            return (long) (this.id - entity.id);
        }
        if (this.shopClosedAt == 0)
        {
            return 1L;
        }
        if (entity.shopClosedAt == 0)
        {
            return -1L;
        }
        return (this.shopClosedAt - entity.shopClosedAt);
    }

    public int getBannerID_OfTime()
    {
        long num = NetworkManager.getTime();
        if (num >= this.getEventNoticeAt())
        {
            if (num < this.getEventStartedAt())
            {
                return this.noticeBannerId;
            }
            if (num < this.getEventFinishedAt())
            {
                return this.bannerId;
            }
        }
        return 0;
    }

    public long GetEndTime(bool is_finishedAt = true)
    {
        long restTime = this.GetRestTime(is_finishedAt);
        if (restTime > 0L)
        {
            return (restTime + NetworkManager.getTime());
        }
        return (!is_finishedAt ? this.endedAt : this.finishedAt);
    }

    public long GetEndTime_ShopOrReward()
    {
        ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
        int eventId = this.getEventId();
        long num2 = 0L;
        if (this.IsOpen(true))
        {
            bool flag = this.IsReward();
            if (!flag)
            {
                flag = master.GetEventEntitiyList(eventId).Length > 0;
            }
            if (flag)
            {
                num2 = this.getEventFinishedAt();
            }
        }
        long shopEndTime = this.GetShopEndTime();
        return ((num2 <= shopEndTime) ? shopEndTime : num2);
    }

    public int getEventBannerID() => 
        this.bannerId;

    public int getEventBannerPriority() => 
        this.bannerPriority;

    public string getEventDetail() => 
        this.detail;

    public long getEventEndedAt() => 
        this.endedAt;

    public long getEventFinishedAt() => 
        this.finishedAt;

    public int getEventIconID() => 
        this.iconId;

    public int getEventId() => 
        this.id;

    public int getEventIntervalHours() => 
        this.intervalHours;

    public string getEventLinkBody() => 
        this.linkBody;

    public int getEventLinkType() => 
        this.linkType;

    public string getEventName() => 
        this.name;

    public long getEventNoticeAt() => 
        this.noticeAt;

    public int getEventOpenHours() => 
        this.openHours;

    public long getEventStartedAt() => 
        this.startedAt;

    public int getEventType() => 
        this.type;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public long GetRestTime(bool is_finishedAt = true) => 
        this.GetRestTime(!is_finishedAt ? this.endedAt : this.finishedAt);

    private long GetRestTime(long end_time)
    {
        long num = NetworkManager.getTime();
        if ((num < this.startedAt) || (num > end_time))
        {
            return 0L;
        }
        if (this.openType == 1)
        {
            return (end_time - num);
        }
        if (this.openHours == 0)
        {
            return 0L;
        }
        if (this.intervalHours == 0)
        {
            return 0L;
        }
        long num2 = this.openHours * 0xe10;
        long num3 = this.intervalHours * 0xe10;
        long num4 = num2 + num3;
        long num5 = (num - this.startedAt) % num4;
        if (num5 > num2)
        {
            return 0L;
        }
        return (num2 - num5);
    }

    public long getShopClosedAt() => 
        this.shopClosedAt;

    public long GetShopEndTime()
    {
        ShopMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP);
        int eventId = this.getEventId();
        long closedAt = 0L;
        ShopEntity[] enableEventEntitiyList = master.GetEnableEventEntitiyList(eventId);
        if (enableEventEntitiyList.Length > 0)
        {
            foreach (ShopEntity entity in enableEventEntitiyList)
            {
                if (closedAt < entity.closedAt)
                {
                    closedAt = entity.closedAt;
                }
            }
        }
        return closedAt;
    }

    public bool isCheckEventDetail() => 
        (12 == this.type);

    public bool IsOpen(bool is_finishedAt = true) => 
        (this.GetRestTime(is_finishedAt) > 0L);

    public bool IsReward() => 
        IsReward(this.getEventId());

    public static bool IsReward(int event_id)
    {
        EventDetailEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId(event_id);
        return ((entity != null) && entity.IsReward());
    }

    public void setShopClosedAt(long shopCloseedAt)
    {
        this.shopClosedAt = shopCloseedAt;
    }

    public enum EventOpenType
    {
        ONCE = 1,
        REPEAT = 2
    }
}

