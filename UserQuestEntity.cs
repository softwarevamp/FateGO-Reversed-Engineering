using System;

public class UserQuestEntity : DataEntityBase
{
    public int clearNum;
    public long firstClearAt;
    public bool isEternalOpen;
    public int keyCountRemain;
    public long keyExpireAt;
    public long lastStartedAt;
    public int questId;
    public int questPhase;
    public long userId;

    public int getClearNum()
    {
        if (this.IsResetInterval())
        {
            return 0;
        }
        return this.clearNum;
    }

    public long getFirstClearAt() => 
        this.firstClearAt;

    public bool getIsEternalOpen() => 
        this.isEternalOpen;

    public int getKeyCountRemain() => 
        this.keyCountRemain;

    public long getKeyExpireAt() => 
        this.keyExpireAt;

    public long getLastStartedAt() => 
        this.lastStartedAt;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.userId, ":", this.questId };
        return string.Concat(objArray1);
    }

    public int getQuestId() => 
        this.questId;

    public int getQuestPhase()
    {
        if (this.IsResetInterval())
        {
            return 0;
        }
        return this.questPhase;
    }

    public long getUserId() => 
        this.userId;

    public bool IsResetInterval()
    {
        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(this.getQuestId());
        if (entity == null)
        {
            return false;
        }
        if (entity.getAfterClear() != 4)
        {
            return false;
        }
        long num = NetworkManager.getTime();
        long num2 = entity.getIntervalHours() * 0xe10;
        long num3 = this.getLastStartedAt() + num2;
        if (num < num3)
        {
            return false;
        }
        return true;
    }
}

