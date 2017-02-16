using System;

public class BattleCommandData
{
    public int _loadsvtLimit;
    private int actionIndex;
    private int addAtk;
    private int addCritical;
    private int addTdGauge;
    public int attri;
    private int chainCount;
    private bool critical;
    private bool flash;
    public bool flgEventJoin;
    public int follower;
    public int markindex;
    private int samecount;
    private bool sameflg;
    public int starBonus;
    public int starcount;
    public int svtId;
    public int svtlimit;
    public int treasureDvc;
    public int type;
    public int uniqueId;

    public BattleCommandData()
    {
        this._loadsvtLimit = -1;
    }

    public BattleCommandData(BattleCommandData command)
    {
        this._loadsvtLimit = -1;
        if (command != null)
        {
            this.type = command.type;
            this.svtId = command.svtId;
            this.svtlimit = command.svtlimit;
            this.loadSvtLimit = command.loadSvtLimit;
            this.attri = command.attri;
            this.uniqueId = command.uniqueId;
            this.markindex = command.markindex;
            this.treasureDvc = command.treasureDvc;
            this.starcount = command.starcount;
            this.flgEventJoin = command.flgEventJoin;
            this.setFollowerType(command.getFollowerType());
        }
    }

    public BattleCommandData(BattleCommand.TYPE type, int svtId, int limitCount)
    {
        this._loadsvtLimit = -1;
        this.type = (int) type;
        this.svtId = svtId;
        this.svtlimit = limitCount;
        this.loadSvtLimit = -1;
        this.attri = 0;
        this.uniqueId = 0;
        this.markindex = 0;
        this.treasureDvc = 0;
        this.starcount = 0;
        this.critical = false;
    }

    public BattleCommandData(BattleCommand.TYPE type, int svtId, int limitCount, int loadLimitCount)
    {
        this._loadsvtLimit = -1;
        this.type = (int) type;
        this.svtId = svtId;
        this.svtlimit = limitCount;
        this.loadSvtLimit = loadLimitCount;
        this.attri = 0;
        this.uniqueId = 0;
        this.markindex = 0;
        this.treasureDvc = 0;
        this.starcount = 0;
        this.critical = false;
    }

    public void addCriticalPoint(int count)
    {
        Debug.Log(string.Empty);
        this.starBonus++;
        if (ConstantMaster.getValue("PER_SAME_COMMAND") <= this.starBonus)
        {
            this.starBonus = 0;
            this.starcount += ConstantMaster.getValue("EXTRA_CRITICAL_RATE");
        }
        this.starcount += count;
        if (0x3e8 < this.starcount)
        {
            this.starcount = 0x3e8;
        }
    }

    public bool checkCriticalMax() => 
        (0x3e8 <= this.starcount);

    public bool checkCriticalRate(int count) => 
        this.checkCriticalRate(this.starcount, count);

    public bool checkCriticalRate(int rate, int count)
    {
        this.critical = count < rate;
        return this.critical;
    }

    public bool checkLastAttack() => 
        (this.isAddAttack() || ((this.actionIndex == 2) && !this.isThree()));

    public int getAttri() => 
        this.attri;

    public int getChainBonus() => 
        (100 * this.chainCount);

    public int getCommandType() => 
        this.type;

    public int getCriticalPoint() => 
        (this.starcount / 10);

    public Follower.Type getFollowerType() => 
        ((Follower.Type) this.follower);

    public int[] getIndividualities() => 
        CardMaster.getIndividualities(this.type, this.actionIndex);

    public int getServantId() => 
        this.svtId;

    public int getServantLimitCount() => 
        this.svtlimit;

    public int getUniqueId() => 
        this.uniqueId;

    public bool isAddAttack() => 
        BattleCommand.isADDATTACK(this.type);

    public bool isArts() => 
        BattleCommand.isARTS(this.type);

    public bool isBlank() => 
        BattleCommand.isBLANK(this.type);

    public bool isBuster() => 
        BattleCommand.isBUSTER(this.type);

    public bool isBusterChain() => 
        (this.isFlash() && this.isBuster());

    public bool isCritical() => 
        this.critical;

    public bool isFlash() => 
        this.flash;

    public bool isGrand() => 
        (this.isThree() && this.isFlash());

    public bool isPair() => 
        (this.sameflg && (this.samecount == 2));

    public bool isQuick() => 
        BattleCommand.isQUICK(this.type);

    public bool isSingle() => 
        (this.isThree() && !this.isFlash());

    public bool isThree() => 
        (this.sameflg && (this.samecount == 3));

    public bool isTreasureDvc() => 
        (0 < this.treasureDvc);

    public void resetCriticalPoint()
    {
        this.starcount = 0;
    }

    public void setCombo(BattleComboData combo, int index)
    {
        this.flash = combo.flash;
        this.actionIndex = index;
        if (index < combo.sameflg.Length)
        {
            this.sameflg = combo.sameflg[this.actionIndex];
        }
        this.samecount = combo.samecount;
        this.addAtk = combo.addAtk;
        this.addCritical = combo.addCritical;
        this.addTdGauge = combo.addTdGauge;
        this.chainCount = combo.tdChain;
    }

    public void setFollowerType(Follower.Type type)
    {
        this.follower = (int) type;
    }

    public void setTypeAddAttack()
    {
        this.treasureDvc = 0;
        this.starcount = 0;
        this.type = 4;
    }

    public int ActionIndex
    {
        get => 
            this.actionIndex;
        set
        {
            this.actionIndex = value;
        }
    }

    public int AddAtk =>
        this.addAtk;

    public int AddCritical =>
        this.addCritical;

    public int AddTdGauge =>
        this.addTdGauge;

    public int ChainCount
    {
        get => 
            this.chainCount;
        set
        {
            this.chainCount = value;
        }
    }

    public int loadSvtLimit
    {
        get
        {
            if (this._loadsvtLimit != -1)
            {
                return this._loadsvtLimit;
            }
            return this.svtlimit;
        }
        set
        {
            this._loadsvtLimit = value;
        }
    }
}

