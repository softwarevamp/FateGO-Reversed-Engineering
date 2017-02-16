using System;

public class QuestPhaseEntity : DataEntityBase
{
    public int battleBgId;
    public int battleBgType;
    public int[] classIds;
    public int friendshipExp;
    public int[] individuality;
    public bool isNpcOnly;
    public int phase;
    public int playerExp;
    public int questId;

    public QuestPhaseEntity()
    {
    }

    public QuestPhaseEntity(QuestPhaseEntity cSrc)
    {
        this.questId = cSrc.questId;
        this.phase = cSrc.phase;
        this.isNpcOnly = cSrc.isNpcOnly;
        this.battleBgId = cSrc.battleBgId;
        this.battleBgType = cSrc.battleBgType;
        this.playerExp = cSrc.playerExp;
        this.friendshipExp = cSrc.friendshipExp;
        this.classIds = (cSrc.classIds == null) ? null : ((int[]) cSrc.classIds.Clone());
        this.individuality = (cSrc.individuality == null) ? null : ((int[]) cSrc.individuality.Clone());
    }

    public int getBattleBgId() => 
        this.battleBgId;

    public int getBattleBgTypeValue() => 
        this.battleBgType;

    public int getFriendshipExp() => 
        this.friendshipExp;

    public int[] getIndividuality()
    {
        if (this.individuality == null)
        {
            return new int[0];
        }
        return this.individuality;
    }

    public int getPhase() => 
        this.phase;

    public int getPlayerExp() => 
        this.playerExp;

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.questId, ":", this.phase };
        return string.Concat(objArray1);
    }

    public int getQuestId() => 
        this.questId;
}

