using System;

public class BattleEntity : DataEntityBase
{
    public BattleInfoData battleInfo;
    public int battleType;
    public int commandSpellCnt;
    public int commandSpellMax;
    public long followerId;
    public int followerType;
    public long id;
    public int questId;
    public int questPhase;
    public int rankingId;
    public int result;
    public int seed;
    public int status;
    public long targetId;
    public long userId;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

