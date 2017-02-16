using System;
using System.Runtime.InteropServices;

public class ServantTreasureDvcEntity : DataEntityBase
{
    public int cardId;
    public int condFriendshipRank;
    public int condLv;
    public int condQuestId;
    public int condQuestPhase;
    public int[] damage;
    public int imageIndex;
    public int motion;
    public int num;
    public int priority;
    public int svtId;
    public int treasureDeviceId;

    public bool getEffectExplanation(out string tdName, out string tdExplanation, out int maxLv, out int tdGuageCount, out int tdCardId, int lv)
    {
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasureDeviceId);
        tdCardId = this.cardId;
        if (entity != null)
        {
            return entity.getEffectExplanation(out tdName, out tdExplanation, out maxLv, out tdGuageCount, lv);
        }
        tdName = LocalizationManager.GetUnknownName();
        tdExplanation = string.Empty;
        maxLv = 0;
        tdGuageCount = 0;
        return false;
    }

    public int getLevelMax()
    {
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasureDeviceId);
        return ((entity == null) ? 0 : entity.maxLv);
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.num, ":", this.priority };
        return string.Concat(objArray1);
    }

    public int getServantID() => 
        this.svtId;

    public int getServantIdx() => 
        this.num;

    public bool isUse(long userId, int lv, int friendshipRank, int beforeClearQuestId = -1)
    {
        if (this.condLv > lv)
        {
            return false;
        }
        if (this.condFriendshipRank > friendshipRank)
        {
            return false;
        }
        if ((this.condQuestId > 0) && !CondType.IsQuestPhaseClear(userId, this.condQuestId, this.condQuestPhase, beforeClearQuestId))
        {
            return false;
        }
        return true;
    }
}

