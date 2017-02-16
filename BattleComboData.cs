using System;

public class BattleComboData
{
    public int addAtk;
    public int addCritical;
    public int addTdGauge;
    public bool chainError;
    public bool flash = false;
    public int flashtype;
    public int samecount = 0;
    public bool[] sameflg = new bool[3];
    public int tdChain;

    public bool isChainError() => 
        this.chainError;

    public bool isExtraAttack() => 
        (3 <= this.samecount);

    public bool isGrand() => 
        ((3 <= this.samecount) && this.flash);
}

