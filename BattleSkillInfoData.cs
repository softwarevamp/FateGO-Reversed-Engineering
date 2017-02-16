using System;
using System.Runtime.InteropServices;

public class BattleSkillInfoData
{
    public int chargeTurn;
    public int index;
    public bool isCharge;
    public bool isPassive;
    public bool isUseSkill;
    public int priority;
    public int skillId;
    public int skilllv;
    public int svtUniqueId;
    public TYPE type;

    public int getChargeTurn() => 
        this.chargeTurn;

    public int getInfoId() => 
        (((int) (this.type * ((TYPE) 100))) + this.index);

    public bool isChargeOK() => 
        (this.chargeTurn <= 0);

    public void TurnExtend(int turnCount, int max = 0x3e7)
    {
        if (!this.isPassive)
        {
            this.chargeTurn += turnCount;
            if (this.chargeTurn > max)
            {
                this.chargeTurn = max;
            }
        }
    }

    public bool TurnProgress(int turnCount, int max = 0)
    {
        this.chargeTurn = 0;
        this.isCharge = true;
        return this.isCharge;
    }

    public enum TYPE
    {
        MASTER_COMMAND = 2,
        MASTER_EQUIP = 1,
        NONE = 0,
        SERVANT_CLASS = 10,
        SERVANT_EQUIP = 12,
        SERVANT_SELF = 11,
        TEMP = 20
    }
}

