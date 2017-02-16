using System;

public class BattleServantActionHistory
{
    private TYPE actType;
    private int targetUniqueId;
    private int waveCount;

    public BattleServantActionHistory(TYPE type, int in_targetUniqueID, int wavecount)
    {
        this.actType = type;
        this.targetUniqueId = in_targetUniqueID;
        this.waveCount = wavecount;
    }

    public int getReactionTarget() => 
        this.targetUniqueId;

    public bool isDamage() => 
        ((((this.actType == TYPE.DAMAGE_COMMAND) || (this.actType == TYPE.DAMAGE_TD)) || (this.actType == TYPE.HPLOSS)) || (this.actType == TYPE.INSTANT_DEATH));

    public enum TYPE
    {
        NONE,
        DAMAGE_COMMAND,
        DAMAGE_TD,
        HPLOSS,
        INSTANT_DEATH,
        REDUCE_HP
    }
}

