using System;
using System.Runtime.InteropServices;

public class QuestEntity : DataEntityBase
{
    public int actConsume;
    public int[] afterAction;
    public int afterClear;
    public int bannerId;
    public int chapterId;
    public int chapterSubId;
    public int charaIconId;
    public int closedAt;
    public int displayHours;
    public int forceOperation;
    public int giftIconId;
    public int giftId;
    public int hasStartAction;
    public int iconId;
    public int id;
    public int intervalHours;
    public string name;
    public int noticeAt;
    public int openedAt;
    public int priority;
    public int recommendLv;
    public int spotId;
    public int type;

    public QuestEntity()
    {
    }

    public QuestEntity(QuestEntity cSrc)
    {
        this.id = cSrc.id;
        this.name = cSrc.name;
        this.type = cSrc.type;
        this.actConsume = cSrc.actConsume;
        this.spotId = cSrc.spotId;
        this.giftId = cSrc.giftId;
        this.priority = cSrc.priority;
        this.bannerId = cSrc.bannerId;
        this.iconId = cSrc.iconId;
        this.charaIconId = cSrc.charaIconId;
        this.giftIconId = cSrc.giftIconId;
        this.forceOperation = cSrc.forceOperation;
        this.afterClear = cSrc.afterClear;
        this.afterAction = cSrc.afterAction;
        this.displayHours = cSrc.displayHours;
        this.intervalHours = cSrc.intervalHours;
        this.chapterId = cSrc.chapterId;
        this.chapterSubId = cSrc.chapterSubId;
        this.recommendLv = cSrc.recommendLv;
        this.hasStartAction = cSrc.hasStartAction;
        this.noticeAt = cSrc.noticeAt;
        this.openedAt = cSrc.openedAt;
        this.closedAt = cSrc.closedAt;
    }

    public int getActConsume(int ap_calc_val = 0)
    {
        int actConsume = this.actConsume;
        if (ap_calc_val > 0)
        {
            actConsume = (actConsume * ap_calc_val) / 0x3e8;
            if (actConsume <= 0)
            {
                actConsume = 1;
            }
        }
        return actConsume;
    }

    public int getAfterClear() => 
        this.afterClear;

    public int getChapterId() => 
        this.chapterId;

    public int getChapterSubId() => 
        this.chapterSubId;

    public int getCharaIconId() => 
        this.charaIconId;

    public long getClosedAt() => 
        ((long) this.closedAt);

    public int getDisplayHours() => 
        this.displayHours;

    public long GetEndTime(bool is_itvl = false) => 
        (this.GetRestTime(is_itvl) + NetworkManager.getTime());

    public int getForceOperation() => 
        this.forceOperation;

    public int getGiftIconId() => 
        this.giftIconId;

    public int getIntervalHours() => 
        this.intervalHours;

    public int getLimitCount() => 
        (this.charaIconId % 10);

    public long getNoticeAt() => 
        ((long) this.noticeAt);

    public long getOpenedAt() => 
        ((long) this.openedAt);

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getPriority() => 
        this.priority;

    public int getQuestId() => 
        this.id;

    public string getQuestName() => 
        this.name;

    public int getQuestType() => 
        this.type;

    public int getRecommendLv() => 
        this.recommendLv;

    public long GetRestTime(bool is_itvl = false)
    {
        long num = NetworkManager.getTime();
        if ((num < this.openedAt) || (num > this.closedAt))
        {
            return 0L;
        }
        if (!is_itvl)
        {
            return (this.closedAt - num);
        }
        if (this.displayHours == 0)
        {
            return 0L;
        }
        if (this.intervalHours == 0)
        {
            return 0L;
        }
        long num2 = this.displayHours * 0xe10;
        long num3 = this.intervalHours * 0xe10;
        long num4 = num2 + num3;
        long num5 = (num - this.openedAt) % num4;
        if (num5 > num2)
        {
            return 0L;
        }
        return (num2 - num5);
    }

    public int getServantId() => 
        (this.charaIconId / 10);

    public int getSpotId() => 
        this.spotId;

    public TypeFlag GetTypeFlag() => 
        ((TypeFlag) (((int) 1) << this.type));

    public bool IsCaldeaGate() => 
        (this.getSpotId() == SpotEntity.CALDEAGATE_ID);

    public bool IsOpenByTime(bool is_itvl = false) => 
        (this.GetRestTime(is_itvl) > 0L);

    public override void printDebug()
    {
        base.printDebug();
        Debug.Log("QuestEntity:" + this.priority);
    }

    public enum enAfterClear
    {
        CLOSE = 1,
        REPEAT_FIRST = 2,
        REPEAT_LAST = 3,
        RESET_INTERVAL = 4
    }

    public enum enForceOperation
    {
        NONE,
        FORCE_OPEN,
        FORCE_CLOSE
    }

    public enum enType
    {
        EVENT = 5,
        FREE = 2,
        FRIENDSHIP = 3,
        HEROBALLAD = 6,
        MAIN = 1
    }

    public enum TypeFlag
    {
        ALL = 110,
        EVENT = 0x20,
        FREE = 4,
        FRIENDSHIP = 8,
        HEROBALLAD = 0x40,
        MAIN = 2,
        NONE = 0
    }
}

