using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleServantData
{
    [CompilerGenerated]
    private static Predicate<BattleSkillInfoData> <>f__am$cache50;
    [CompilerGenerated]
    private static Predicate<BattleSkillInfoData> <>f__am$cache51;
    public LinkedList<BattleServantActionHistory> actionHistory = new LinkedList<BattleServantActionHistory>();
    public int actPriority;
    public int aiId;
    private AiState aiState = new AiState();
    public int atk;
    public BattleBuffData buffData;
    public ServantCardEntity[] commandent;
    public Hashtable commandtable;
    public int criticalRate;
    public DEADTYPE deadtype;
    public int deathRate;
    public int deckIndex = -1;
    private BattleDeckServantData deckSvt;
    public int displayType;
    public int downstarrate;
    public int downtdrate;
    public int dressDispId = -1;
    private DropInfo[] droplist;
    public int equipatk;
    public int equiphp;
    private BattleUserServantData[] equipList;
    public int exceedCount;
    public bool flgEntryFunction;
    public bool flgEventJoin;
    public Follower.Type followerType;
    public int hp;
    public int index;
    public bool isAppear;
    public bool isBuffProgressFlg;
    public bool isChargeSkill;
    public bool isDeadAnime;
    public bool isEnemy;
    public bool isEntry;
    public bool isLoad;
    public bool isUseNP;
    public bool isWaitRepop;
    public int level;
    private int limitcount;
    public int lineMaxNp = 100;
    public int maxActNum;
    public int maxhp;
    public int maxleve;
    public int maxtpturn;
    public int nexttpturn;
    public int np;
    public int npcSvtClassId;
    public int npcSvtType;
    public int nplineCount = 3;
    public int overkillTargetId;
    private List<GameObject> paramobjelist = new List<GameObject>();
    public int reducedhp;
    public bool reservationspecial;
    public int restAttackCount;
    public int roleType = 1;
    public List<BattleSkillInfoData> skillInfoList = new List<BattleSkillInfoData>();
    public STATUS status;
    private ServantEntity svtdata;
    public int svtId;
    private int[] svtIndividuality;
    private ServantLimitAddEntity svtlimitaddent;
    private ServantLimitEntity svtlimitDispent;
    private ServantLimitEntity svtlimitent;
    public string svtName;
    private ServantTreasureDvcEntity SvtTDvc;
    public int svtType;
    private TreasureDvcEntity TDvc;
    private TreasureDvcLvEntity TDvcLv;
    private int tmp_npper;
    public int tmpNp;
    public int transformIndex = -1;
    public int transformSvtId;
    public long transformUserSvtId = -1L;
    private int treasuredvcId;
    private int treasuredvcLevel;
    public int uniqueId;
    public long userSvtId;

    public void addBuff(BattleBuffData.BuffData buff, bool changeMaxHpflg = false)
    {
        int num = this.getMaxHp();
        this.buffData.addBuff(buff);
        if (changeMaxHpflg)
        {
            this.checkUpdateUpdownHp(num, true);
        }
    }

    public void addDamage(int damage)
    {
        this.hp -= damage;
        if (this.hp <= 0)
        {
            this.hp = 0;
        }
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("changeHp", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void addNp(int intp, bool flg = true)
    {
        if (!this.isAddNpGauge())
        {
            intp = 0;
        }
        this.np += intp;
        if (((0 < intp) && ((this.lineMaxNp * 0.99f) <= this.np)) && (this.np < this.lineMaxNp))
        {
            this.np = this.lineMaxNp;
        }
        if (this.getMaxNp() < this.np)
        {
            this.np = this.getMaxNp();
        }
        if (this.np <= 0)
        {
            this.np = 0;
        }
        if (flg)
        {
            foreach (GameObject obj2 in this.paramobjelist)
            {
                obj2.SendMessage("updateNp", this, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void addNpPer(float per)
    {
        this.addNp(Mathf.FloorToInt(this.lineMaxNp * per), true);
    }

    public void addParamObject(GameObject obj)
    {
        if (!this.paramobjelist.Contains(obj))
        {
            this.paramobjelist.Add(obj);
        }
    }

    public void addSkillInfo(BattleSkillInfoData.TYPE type, int index, int skillId, int skillLv)
    {
        foreach (BattleSkillInfoData data in this.skillInfoList)
        {
            if (((data.type == type) && (data.index == index)) && ((data.skillId == skillId) && (data.skilllv == skillLv)))
            {
                return;
            }
        }
        BattleSkillInfoData item = null;
        foreach (BattleSkillInfoData data3 in this.skillInfoList)
        {
            if ((data3.type == type) && (data3.index == index))
            {
                item = data3;
                break;
            }
        }
        if (item != null)
        {
            this.skillInfoList.Remove(item);
        }
        BattleSkillInfoData data4 = new BattleSkillInfoData {
            type = type,
            svtUniqueId = this.getUniqueID(),
            index = index,
            skillId = skillId,
            skilllv = skillLv
        };
        SkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
        SkillEntity entity = null;
        long[] args = new long[] { (long) skillId };
        if (master.isEntityExistsFromId(args))
        {
            entity = master.getEntityFromId<SkillEntity>(skillId);
        }
        if (entity != null)
        {
            data4.isUseSkill = true;
            data4.isPassive = entity.isPassive();
            SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillId, skillLv);
            data4.priority = entity2.priority;
            data4.chargeTurn = 0;
        }
        else
        {
            data4.isUseSkill = false;
        }
        this.skillInfoList.Add(data4);
    }

    public void changeNp(int intp, bool flg = true)
    {
        this.np = intp;
        if (this.getMaxNp() < this.np)
        {
            this.np = this.getMaxNp();
        }
        if (this.np <= 0)
        {
            this.np = 0;
        }
        if (flg)
        {
            foreach (GameObject obj2 in this.paramobjelist)
            {
                obj2.SendMessage("changeNp", this, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void changeTransformServant()
    {
        this.svtId = this.transformSvtId;
        this.svtdata = ((ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT)).getEntityFromId<ServantEntity>(this.svtId);
        if (this.svtdata == null)
        {
            Debug.LogError("setServantData: Servant not found : " + this.getSvtId());
        }
        ServantLimitMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT);
        long[] args = new long[] { (long) this.getSvtId(), (long) this.limitcount };
        if (master2.isEntityExistsFromId(args))
        {
            this.svtlimitent = master2.getEntityFromId<ServantLimitEntity>(this.getSvtId(), this.limitcount);
        }
        else
        {
            this.svtlimitent = master2.getEntityFromId<ServantLimitEntity>(0x18704, 0);
        }
        long[] numArray2 = new long[] { (long) this.getSvtId(), (long) this.getDispLimitCount() };
        if (master2.isEntityExistsFromId(numArray2))
        {
            this.svtlimitDispent = master2.getEntityFromId<ServantLimitEntity>(this.getSvtId(), this.getDispLimitCount());
        }
        else
        {
            this.svtlimitDispent = master2.getEntityFromId<ServantLimitEntity>(0x18704, 0);
        }
        ServantLimitAddMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        long[] numArray3 = new long[] { (long) this.svtdata.id, (long) this.getDispLimitCount() };
        if (master3.isEntityExistsFromId(numArray3))
        {
            this.svtlimitaddent = master3.getEntityFromId<ServantLimitAddEntity>(this.svtdata.id, this.getDispLimitCount());
        }
        else
        {
            this.svtlimitaddent = null;
        }
        if (0 < this.treasuredvcId)
        {
            this.TDvc = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasuredvcId);
            this.TDvcLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(this.treasuredvcId, this.treasuredvcLevel);
            this.SvtTDvc = ServantTreasureDvcMaster.getEntityFromIDID(this.svtId, this.treasuredvcId);
            this.nplineCount = this.TDvcLv.gaugeCount;
        }
        else
        {
            this.TDvc = null;
            this.nplineCount = 0;
        }
    }

    public bool checkAvoidance(BattleServantData targetSvt) => 
        this.buffData.checkAvoidance(targetSvt.getWeaponIndividualities());

    public bool checkAvoidInstantDeath(BattleServantData targetSvt) => 
        this.buffData.checkAvoidInstantDeath(targetSvt.getIndividualities());

    public bool checkBreakAvoidance(BattleServantData targetSvt) => 
        this.buffData.checkBreakAvoidance(targetSvt.getIndividualities());

    public bool checkBuffAvoid(int[] insividualities) => 
        this.buffData.checkBuffAvoid(insividualities);

    public bool checkBuffGroup(int buffGroup) => 
        this.buffData.checkBuffGroup(buffGroup);

    public bool checkBuffId(int[] buffIdlist) => 
        this.buffData.checkBuffId(buffIdlist);

    public bool checkBuffIndividualities(int[] indv) => 
        this.buffData.checkBuffIndividualities(indv);

    private static bool checkEndBuff(BattleBuffData buff) => 
        true;

    public bool checkEnemy() => 
        this.isEnemy;

    public bool checkID(int id) => 
        (this.uniqueId == id);

    public bool checkIndividualities(int[] indv) => 
        Individuality.CheckIndividualities(this.getIndividualities(), indv);

    public bool checkInvincible(BattleServantData targetSvt) => 
        this.buffData.checkInvincible(targetSvt.getWeaponIndividualities());

    public bool checkOverKill(int targetId) => 
        (this.overkillTargetId == targetId);

    public bool checkPerHP(int val, bool flg)
    {
        int num = this.getNowHp();
        int num2 = this.getMaxHp();
        if (flg)
        {
            return (val <= ((num * 100) / num2));
        }
        return (((num * 100) / num2) <= val);
    }

    public bool checkPierceInvincible(BattleServantData targetSvt) => 
        this.buffData.checkPierceInvincible(targetSvt.getWeaponIndividualities());

    public bool checkPlayer() => 
        !this.isEnemy;

    public bool checkReadySpecail() => 
        this.reservationspecial;

    public bool checkRegainNPUsedNoble()
    {
        if (this.isEnemy)
        {
            return false;
        }
        int intp = this.buffData.checkRegainNPUsedNoble(this);
        this.addNp(intp, false);
        return true;
    }

    public bool checkScriptValue(string key, int value) => 
        this.deckSvt?.checkScript(key, value);

    public bool checkUpdateUpdownHp(int before_maxhp, bool isHeal)
    {
        int num = this.getMaxHp();
        if (num < before_maxhp)
        {
            if (num < this.hp)
            {
                int num2 = this.hp - num;
                this.hp = num;
                this.reducedhp -= num2;
                if (this.reducedhp < 0)
                {
                    this.reducedhp = 0;
                }
            }
        }
        else if (isHeal && (before_maxhp < num))
        {
            int num3 = num - before_maxhp;
            this.hp += num3;
        }
        return (num != before_maxhp);
    }

    public bool checkUseTDvc(bool flg = true)
    {
        if (!this.hasTreasureDvc())
        {
            return false;
        }
        if (flg && !this.isNobleAction())
        {
            return false;
        }
        if (this.isEnemy)
        {
            return (this.nexttpturn <= 0);
        }
        if (!this.followerType.isUseTreasure())
        {
            return false;
        }
        return (this.lineMaxNp <= this.np);
    }

    public void delParamObject(GameObject obj)
    {
        if (this.paramobjelist.Contains(obj))
        {
            this.paramobjelist.Remove(obj);
        }
    }

    public void fixUpdateStatus()
    {
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("fixUpdateStatus", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public BattleSkillInfoData[] getActiveSkillInfos()
    {
        if (<>f__am$cache51 == null)
        {
            <>f__am$cache51 = s => !s.isPassive && s.isUseSkill;
        }
        return this.skillInfoList.FindAll(<>f__am$cache51).ToArray();
    }

    public AiState getAiState() => 
        this.aiState;

    public int getAttackBaseNp(BattleCommandData command, bool isNoble)
    {
        if (this.TDvcLv != null)
        {
            if (isNoble)
            {
                return this.TDvcLv.tdPoint;
            }
            if (command.isQuick())
            {
                return this.TDvcLv.tdPointQ;
            }
            if (command.isArts())
            {
                return this.TDvcLv.tdPointA;
            }
            if (command.isBuster())
            {
                return this.TDvcLv.tdPointB;
            }
            if (command.isAddAttack())
            {
                return this.TDvcLv.tdPointEx;
            }
        }
        return 0;
    }

    public int getAttackCount(BattleCommandData command)
    {
        if (!this.commandtable.ContainsKey(command.type))
        {
            return 1;
        }
        ServantCardEntity entity = (ServantCardEntity) this.commandtable[command.type];
        if (command.isFlash())
        {
            if (command.isThree())
            {
                return entity.grandDamage.Length;
            }
            if (command.isPair())
            {
                return entity.unisonDamage.Length;
            }
            return entity.trinityDamage.Length;
        }
        if (command.isThree())
        {
            return entity.singleDamage.Length;
        }
        return entity.normalDamage.Length;
    }

    public int[] getAttackRaito(BattleCommandData command)
    {
        if (!this.commandtable.ContainsKey(command.type))
        {
            return new int[] { 100 };
        }
        ServantCardEntity entity = (ServantCardEntity) this.commandtable[command.type];
        if (command.isFlash())
        {
            if (command.isThree())
            {
                return entity.grandDamage;
            }
            if (command.isPair())
            {
                return entity.unisonDamage;
            }
            return entity.trinityDamage;
        }
        if (command.isThree())
        {
            return entity.singleDamage;
        }
        return entity.normalDamage;
    }

    public int getAttackType(BattleCommandData command)
    {
        if (!this.commandtable.ContainsKey(command.type))
        {
            return 1;
        }
        ServantCardEntity entity = (ServantCardEntity) this.commandtable[command.type];
        return entity.attackType;
    }

    public int getAttri() => 
        this.svtdata.attri;

    public int getBaseATK() => 
        this.atk;

    public int getBaseStarRate() => 
        this.svtdata.starRate;

    public float getBuffDamageValue(BattleServantData targetSvt) => 
        ((float) this.buffData.getDamegeValue(targetSvt));

    public BattleBuffData getBuffData() => 
        this.buffData;

    public float getBuffGRANTSTATEMagnification(int[] individualities) => 
        this.buffData.getGrantStateMagnification(individualities);

    public float getBuffNonResistInstantDeath(BattleServantData targetSvt) => 
        this.buffData.getBuffNonResistInstantDeath(targetSvt);

    public float getBuffResistInstantDeath(BattleServantData targetSvt) => 
        this.buffData.getBuffResistInstantDeath(targetSvt);

    public float getBuffSelfDamageValue(BattleServantData targetSvt) => 
        ((float) this.buffData.getSelfDamageValue(targetSvt));

    public float getBuffTOLERANCEMagnification(int[] individualities) => 
        this.buffData.getToleranceMagnification(individualities);

    public float getClassAtk() => 
        ServantClassMaster.getClassAtk(this.getClassId());

    public int getClassId()
    {
        if (this.npcSvtClassId != 0)
        {
            return this.npcSvtClassId;
        }
        return this.svtdata.classId;
    }

    public float getCommandCardATK(BattleCommandData command, BattleServantData targetSvt)
    {
        float num = 1f;
        if (this.isEnemy)
        {
            if (BattleCommand.isARTS(command.getCommandType()))
            {
                num = ConstantMaster.getRateValue("ENEMY_ATTACK_RATE_ARTS");
            }
            else if (BattleCommand.isQUICK(command.getCommandType()))
            {
                num = ConstantMaster.getRateValue("ENEMY_ATTACK_RATE_QUICK");
            }
            else if (BattleCommand.isBUSTER(command.getCommandType()))
            {
                num = ConstantMaster.getRateValue("ENEMY_ATTACK_RATE_BUSTER");
            }
            else
            {
                num = BattleCommand.getMagnification(command);
            }
        }
        else
        {
            num = BattleCommand.getMagnification(command);
        }
        float num2 = this.buffData.getCommandAtk(command, targetSvt);
        num *= num2;
        return (num + (((float) command.AddAtk) / 1000f));
    }

    public float getCommandCardNP(BattleCommandData command, BattleServantData targetSvt)
    {
        float num = BattleCommand.getNpMagnification(command);
        float num2 = this.buffData.getCommandNp(command, targetSvt);
        num *= num2;
        return (num + (((float) command.AddTdGauge) / 1000f));
    }

    public int getCommandDispLimitCount()
    {
        int commandCardLimitCount = this.commandCardLimitCount;
        if (0 < this.dressDispId)
        {
            return this.dressDispId;
        }
        if (commandCardLimitCount == 0)
        {
            commandCardLimitCount = this.limitcount;
            if (this.isEnemy)
            {
                ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
                long[] args = new long[] { (long) this.svtId, (long) this.limitcount };
                if (master.isEntityExistsFromId(args))
                {
                    commandCardLimitCount = master.getEntityFromId<ServantLimitAddEntity>(this.svtId, this.limitcount).battleCharaLimitCount;
                }
            }
            return commandCardLimitCount;
        }
        return ImageLimitCount.GetLimitCountByImageLimit(commandCardLimitCount - 1);
    }

    public int[] getCommandList() => 
        this.svtdata.cardIds;

    public BattleBuffData.BuffData[] getCommandSideEffect(int[] individualities) => 
        this.buffData.getCommandSideEffectFunction(individualities);

    public float getCommandStar(BattleCommandData command, BattleServantData targetSvt)
    {
        float num = BattleCommand.getCriticalMagnification(command);
        float num2 = this.buffData.getCommandStar(command, targetSvt);
        num *= num2;
        return (num + (((float) command.AddCritical) / 1000f));
    }

    public int getCountMaxNp() => 
        (3 * this.lineMaxNp);

    public int getCriticalRate() => 
        (this.criticalRate + this.getUpDownCriticalRate());

    public int getCriticalWeight(int[] indvlist) => 
        Mathf.FloorToInt(this.svtlimitent.criticalWeight * this.buffData.getStarWeightMagnification(indvlist));

    public BattleBuffData.BuffData[] getDeadAttackSideEffect(int[] individualities) => 
        this.buffData.getDeadAttackSideEffectFunction(individualities);

    public BattleBuffData.BuffData[] getDeadBufflist() => 
        this.buffData.getDeadFunction();

    public int getDeadTargetUniqueId()
    {
        BattleServantActionHistory[] historyArray = this.actionHistory.ToArray<BattleServantActionHistory>();
        int index = historyArray.Length - 1;
        while (index >= 0)
        {
            return historyArray[index].getReactionTarget();
        }
        return -1;
    }

    public float getDeathRate() => 
        (((float) this.deathRate) / 1000f);

    public int getDeckIndex()
    {
        if (this.deckIndex < 0)
        {
            return this.index;
        }
        return this.deckIndex;
    }

    public int getDefenceBaseNp()
    {
        if (this.TDvcLv != null)
        {
            return this.TDvcLv.tdPointDef;
        }
        return 0;
    }

    public int getDispLimitCount()
    {
        int dispLimitCount = this.dispLimitCount;
        if (dispLimitCount == 0)
        {
            dispLimitCount = this.limitcount;
            if (this.isEnemy)
            {
                ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
                long[] args = new long[] { (long) this.svtId, (long) this.limitcount };
                if (master.isEntityExistsFromId(args))
                {
                    dispLimitCount = master.getEntityFromId<ServantLimitAddEntity>(this.svtId, this.limitcount).battleCharaLimitCount;
                }
            }
            return dispLimitCount;
        }
        return ImageLimitCount.GetLimitCountByImageLimit(dispLimitCount - 1);
    }

    public int getDownBaseStarRate() => 
        this.downstarrate;

    public DropInfo[] getDropItem() => 
        this.droplist;

    public int getEffectFolder() => 
        this.svtlimitDispent.effectFolder;

    public BattleUserServantData[] getEquipBattleUserServantList() => 
        this.equipList;

    public float getheadUpY() => 
        this.SvtEnt.getHeadUpY();

    public int[] getIndividualities()
    {
        List<int> first = new List<int>();
        if (this.svtIndividuality != null)
        {
            first.AddRange(this.svtIndividuality);
        }
        else
        {
            first.AddRange(this.SvtEnt.getIndividuality());
        }
        first.AddRange(this.buffData.getAddIndividualities());
        return first.Except<int>(this.buffData.getSubIndividualities()).ToArray<int>();
    }

    public int getLevel() => 
        this.level;

    public string getLevelLabel() => 
        $"{this.level}";

    public int getLimitCount() => 
        this.limitcount;

    public int getLimitImageIndex() => 
        ServantAssetLoadManager.GetLimitImageIndex(this.getSvtId(), this.getDispLimitCount());

    public int getMaxHp()
    {
        int num = Mathf.FloorToInt((this.maxhp + (this.maxhp * this.buffData.getUpDownMaxHpMagnification(this))) + this.buffData.getUpDownMaxHp(this));
        if (num < 1)
        {
            num = 1;
        }
        return num;
    }

    public int getMaxLevel() => 
        this.maxleve;

    public int getMaxNextTDTurn() => 
        this.maxtpturn;

    public int getMaxNp() => 
        (this.nplineCount * this.lineMaxNp);

    public int getMotionId(BattleCommandData command)
    {
        if (!this.commandtable.ContainsKey(command.type))
        {
            return 40;
        }
        ServantCardEntity entity = (ServantCardEntity) this.commandtable[command.type];
        return entity.motion;
    }

    public int getNextTDTurn() => 
        this.nexttpturn;

    public int getNowHp() => 
        this.hp;

    public int getNp() => 
        this.np;

    public int getNpPer() => 
        this.tmp_npper;

    public BattleSkillInfoData[] getPassiveSkills()
    {
        if (<>f__am$cache50 == null)
        {
            <>f__am$cache50 = s => s.isPassive;
        }
        return this.skillInfoList.FindAll(<>f__am$cache50).ToArray();
    }

    public int getRarity() => 
        this.svtlimitDispent.rarity;

    public int[] getRecBuffList() => 
        this.buffData.getRectBuffList();

    public int getRevengeTargetUniqueId()
    {
        BattleServantActionHistory[] historyArray = this.actionHistory.ToArray<BattleServantActionHistory>();
        for (int i = historyArray.Length - 1; i >= 0; i--)
        {
            if (historyArray[i].isDamage())
            {
                return historyArray[i].getReactionTarget();
            }
        }
        return -1;
    }

    public string getSaveData()
    {
        SaveData data = new SaveData {
            hp = this.hp,
            deckIndex = this.deckIndex,
            np = this.np,
            nextNp = this.nexttpturn,
            isEntry = !this.isEntry ? 0 : 1,
            isDeadAnime = !this.isDeadAnime ? 0 : 1,
            aistateSave = this.aiState.getSaveData(),
            buffDataSave = this.buffData.getSaveData(),
            skillinfoid_list = new int[this.skillInfoList.Count],
            skillcharge_list = new int[this.skillInfoList.Count]
        };
        for (int i = 0; i < this.skillInfoList.Count; i++)
        {
            BattleSkillInfoData data2 = this.skillInfoList[i];
            data.skillinfoid_list[i] = data2.getInfoId();
            data.skillcharge_list[i] = data2.getChargeTurn();
        }
        data.transformIndex = this.transformIndex;
        return JsonManager.toJson(data);
    }

    public BattleSkillInfoData getSelfSkillInfo(int index)
    {
        <getSelfSkillInfo>c__AnonStorey7E storeye = new <getSelfSkillInfo>c__AnonStorey7E {
            index = index
        };
        return this.skillInfoList.Find(new Predicate<BattleSkillInfoData>(storeye.<>m__9E));
    }

    public string getServantName()
    {
        if (this.svtName != null)
        {
            return this.svtName;
        }
        return this.svtdata.name;
    }

    public string getServantShortName()
    {
        if (this.svtName != null)
        {
            return this.svtName;
        }
        return this.svtdata.battleName;
    }

    public BattleBuffData.BuffData[] getStartWaveBuff() => 
        this.buffData.getStartWaveFunction();

    public int getStepRate() => 
        this.svtlimitDispent.stepProbability;

    public string getStrParam() => 
        this.svtlimitDispent.strParam;

    public int getSvtId() => 
        this.svtdata.id;

    public float getTdRate() => 
        (((float) this.downtdrate) / 1000f);

    public BattleSkillInfoData getTempSkillInfo(int index)
    {
        <getTempSkillInfo>c__AnonStorey7F storeyf = new <getTempSkillInfo>c__AnonStorey7F {
            index = index
        };
        return this.skillInfoList.Find(new Predicate<BattleSkillInfoData>(storeyf.<>m__9F));
    }

    public int getThisTurnActCount() => 
        (this.maxActNum - this.restAttackCount);

    public int getTreasureDvcCardId() => 
        this.SvtTDvc.cardId;

    public int[] getTreasureDvcHitRaito() => 
        this.SvtTDvc.damage;

    public int getTreasureDvcId() => 
        this.treasuredvcId;

    public int getTreasureDvcLevel() => 
        this.treasuredvcLevel;

    public int getTreasureDvcMotionId() => 
        this.SvtTDvc.motion;

    public string getTreasureDvcName()
    {
        if (this.TDvc == null)
        {
            return string.Empty;
        }
        return this.TDvc.name;
    }

    public string getTreasureDvcRuby()
    {
        if (this.TDvc == null)
        {
            return string.Empty;
        }
        return this.TDvc.ruby;
    }

    public BattleBuffData.BuffData[] getTTurnEndBufflist() => 
        this.buffData.getTTurnEndFunction();

    public int getUniqueID() => 
        this.uniqueId;

    public float getUpDownAtk(BattleServantData targetSvt) => 
        this.buffData.getUpDownAtk(targetSvt);

    public float getUpdownCriticalAtk(BattleServantData targetSvt) => 
        this.buffData.getCriticalDamage(targetSvt);

    public int getUpDownCriticalRate() => 
        this.buffData.getUpDownCriticalRate();

    public float getUpDownDamageDropNp(BattleServantData targetSvt) => 
        this.buffData.getDamageDropNpMagnification(targetSvt);

    public float getUpDownDef(BattleServantData targetSvt, bool pierce) => 
        this.buffData.getUpDownDef(targetSvt, pierce);

    public float getUpDownDropNp(BattleServantData targetSvt) => 
        this.buffData.getDropNpMagnification(targetSvt);

    public float getUpdownDropStar(BattleCommandData command) => 
        this.buffData.getCriticalPointMagnification(command.getIndividualities());

    public float getUpDownGiveHeal() => 
        this.buffData.getGiveHealMagnification();

    public int getUpDownHeal(out int digit) => 
        this.buffData.getHealMagnification(out digit);

    public float getUpdownNpAtk(BattleServantData targetSvt) => 
        this.buffData.getNPDamageMagnification(targetSvt);

    public float getUpdownPower(BattleServantData targetSvt) => 
        this.buffData.getDamageMagnification(targetSvt);

    public float getUpdownSelfDamage(BattleServantData targetSvt) => 
        this.buffData.getSelfDamageMagnification(targetSvt);

    public float getUpDownSpecialDef(BattleServantData targetSvt) => 
        this.buffData.getUpDownSpecialDef(targetSvt);

    public long getUserSvtId() => 
        this.userSvtId;

    public Color getWeaponColor() => 
        this.svtlimitDispent.getWeaponColor();

    public int getWeaponGroup() => 
        this.svtlimitDispent.weaponGroup;

    public int[] getWeaponIndividualities() => 
        new int[] { this.SvtEnt.attackAttri };

    public int getWeaponScale() => 
        this.svtlimitDispent.weaponScale;

    public bool hasTreasureDvc() => 
        (this.TDvc != null);

    public bool healHp(int heal)
    {
        int num = this.getMaxHp();
        if (this.buffData.checkNoHeal())
        {
            return false;
        }
        this.hp += heal;
        this.hp = (num >= this.hp) ? this.hp : num;
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("changeHp", this, SendMessageOptions.DontRequireReceiver);
        }
        return true;
    }

    public void InitializeBuff()
    {
        int num = -1;
        if (this.buffData == null)
        {
            this.buffData = new BattleBuffData();
        }
        else
        {
            num = this.getMaxHp();
        }
        this.buffData.Initialize();
        if (0 < num)
        {
            this.checkUpdateUpdownHp(num, false);
        }
    }

    public void initTacticalFaze()
    {
        this.setOverKillTargetId(-1);
        this.resetActionHistory();
    }

    public bool isAction() => 
        !this.buffData.checkDontAct();

    public bool isAddNpGauge() => 
        (this.followerType != Follower.Type.NOT_FRIEND);

    public bool isAfterImage() => 
        false;

    public bool isAlive() => 
        (0 < this.hp);

    public bool isDead() => 
        (this.hp <= 0);

    public bool isDeadAnimation() => 
        this.isDeadAnime;

    public bool isDeadEscape() => 
        (DEADTYPE.ESCAPE == this.deadtype);

    public bool isGuts()
    {
        if (this.isDeadAnimation())
        {
            return false;
        }
        return this.buffData.isGuts();
    }

    public bool isHeroine() => 
        this.SvtEnt.checkIsHeroineSvt();

    public bool isJustHit() => 
        false;

    public bool isNobleAction() => 
        !this.buffData.checkDontNoble();

    public bool isOverKill() => 
        (this.overkillTargetId != -1);

    public bool isSphitBuff() => 
        this.buffData.isSphitBuff();

    public bool isTDSeraled()
    {
        if (this.TDvcLv != null)
        {
            return (this.TDvcLv.funcId.Length <= 0);
        }
        return true;
    }

    public bool isUpHate() => 
        this.buffData.isUpHate();

    public bool isUseSelfSkill(int index)
    {
        <isUseSelfSkill>c__AnonStorey7D storeyd = new <isUseSelfSkill>c__AnonStorey7D {
            index = index
        };
        BattleSkillInfoData data = this.skillInfoList.Find(new Predicate<BattleSkillInfoData>(storeyd.<>m__9D));
        if (data == null)
        {
            return false;
        }
        if (data.isPassive)
        {
            return false;
        }
        return data.isUseSkill;
    }

    public bool isUseSkill() => 
        (this.isAction() && !this.buffData.checkDontSkill());

    public bool isUseTreasureDvc() => 
        !this.buffData.checkDontNoble();

    public void loadTransformServant(BattleInfoData battleInfo)
    {
        if (this.transformIndex >= 0)
        {
            this.setTransformServant(battleInfo, this.transformIndex);
            this.changeTransformServant();
        }
    }

    public bool playDead()
    {
        this.hp = 0;
        return true;
    }

    public bool provisionalDamage(int damage)
    {
        this.reducedhp += damage;
        return (this.reducedhp < this.hp);
    }

    public void provisionalHeal(int heal)
    {
        int num = this.getMaxHp() - this.hp;
        this.reducedhp -= heal;
        if (num < -this.reducedhp)
        {
            this.reducedhp = -num;
        }
    }

    public void recordUse()
    {
        this.buffData.startBattleRec();
    }

    public void refreshActionBattle()
    {
        this.reservationspecial = false;
    }

    public void resetActionHistory()
    {
        this.actionHistory.Clear();
    }

    public void resetParamObject()
    {
        this.paramobjelist.Clear();
    }

    public void resetReducedHp()
    {
        this.reducedhp = 0;
    }

    public void resetRetAttackCount()
    {
        this.restAttackCount = this.maxActNum;
    }

    public void setActionHistory(int actUniqueId, BattleServantActionHistory.TYPE actType, int wavecount)
    {
        BattleServantActionHistory history = new BattleServantActionHistory(actType, actUniqueId, wavecount);
        this.actionHistory.AddLast(history);
    }

    public void setContinue(BattleData data)
    {
        this.status = STATUS.NOMAL;
        this.np = 0;
        this.addNp(this.lineMaxNp, true);
        this.buffData.clearActiveBuff();
        if (this.isUseSkill())
        {
            foreach (BattleSkillInfoData data2 in this.skillInfoList)
            {
                data2.chargeTurn = 0;
            }
        }
        this.transformIndex = -1;
        this.setTransformServant(data.getBattleInfo(), -1);
        this.changeTransformServant();
        this.deckIndex = -1;
        this.hp = this.getMaxHp();
        this.isDeadAnime = false;
    }

    public void setDeadAnimeFlg(bool flg)
    {
        this.isDeadAnime = flg;
    }

    public void setDeadData()
    {
        this.resetParamObject();
    }

    public void setDeckIndex(int index)
    {
        this.deckIndex = index;
    }

    public void setEnemy()
    {
        this.isEnemy = true;
    }

    public void setEscapeMotion()
    {
        this.deadtype = DEADTYPE.ESCAPE;
    }

    public void SetFakeData(Dictionary<string, int> fakeInfo)
    {
        this.index = fakeInfo["index"];
        this.uniqueId = fakeInfo["uniqueId"];
        this.userSvtId = (long) fakeInfo["userSvtId"];
        this.svtId = fakeInfo["svtId"];
        this.limitcount = fakeInfo["limitCount"];
        this.svtdata = ((ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT)).getEntityFromId<ServantEntity>(this.svtId);
        this.svtlimitDispent = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtdata.id, this.limitcount);
    }

    public void setHp(int inhp)
    {
        int num = this.getMaxHp();
        this.hp = inhp;
        if (num < this.hp)
        {
            this.hp = num;
        }
        if (this.hp < 0)
        {
            this.hp = 0;
        }
    }

    public void setInitBattle()
    {
        this.resetReducedHp();
        this.reservationspecial = false;
        this.isDeadAnime = false;
    }

    public void setInitQuest()
    {
        this.hp = this.maxhp;
        this.np = 0;
        this.nexttpturn = this.maxtpturn;
    }

    public void setOverKillTargetId(int Id)
    {
        this.overkillTargetId = Id;
    }

    public void setReadySpecial(bool flg)
    {
        this.reservationspecial = flg;
    }

    public bool setSaveData(string str)
    {
        Debug.Log("setSaveData:" + str);
        SaveData data = JsonManager.Deserialize<SaveData>(str);
        if (data == null)
        {
            Debug.LogError("SaveData");
        }
        this.hp = data.hp;
        this.np = data.np;
        this.deckIndex = data.deckIndex;
        this.nexttpturn = data.nextNp;
        this.isEntry = data.isEntry == 1;
        this.isDeadAnime = data.isDeadAnime == 1;
        this.aiState.setSaveData(data.aistateSave);
        this.buffData.setSaveData(data.buffDataSave);
        foreach (BattleSkillInfoData data2 in this.skillInfoList)
        {
            if (!data2.isPassive)
            {
                for (int i = 0; i < data.skillinfoid_list.Length; i++)
                {
                    if (data.skillinfoid_list[i] == data2.getInfoId())
                    {
                        data2.chargeTurn = data.skillcharge_list[i];
                    }
                }
            }
        }
        if (this.deckIndex < 0)
        {
            this.deckIndex = this.index;
        }
        this.transformIndex = data.transformIndex;
        return true;
    }

    public void setServantData(BattleDeckServantData deckSvt, BattleInfoData battleInfo, int infollowerType = 0)
    {
        this.deckSvt = deckSvt;
        BattleUserServantData data = battleInfo.getUserServantFromID(deckSvt.userSvtId, deckSvt.userId);
        this.index = deckSvt.id - 1;
        this.uniqueId = deckSvt.getUniqueID();
        this.userSvtId = deckSvt.getUserServantID();
        this.exceedCount = data.exceedCount;
        this.limitcount = data.limitCount;
        this.dispLimitCount = data.dispLimitCount;
        this.commandCardLimitCount = data.commandCardLimitCount;
        this.iconLimitCount = data.getIconLimitCount();
        this.frameType = data.getFrameType();
        this.npcSvtClassId = data.npcSvtClassId;
        this.level = data.lv;
        this.maxleve = data.getLevelMax();
        this.atk = data.atk;
        this.maxhp = data.hp;
        this.svtId = data.getBattleSvtId();
        this.maxtpturn = data.chargeTurn;
        this.downstarrate = data.starRate;
        this.downtdrate = data.tdRate;
        this.deathRate = data.deathRate;
        this.criticalRate = data.criticalRate;
        this.svtName = deckSvt.name;
        this.overkillTargetId = -1;
        this.displayType = data.displayType;
        this.npcSvtType = data.npcSvtType;
        this.droplist = deckSvt.dropInfos;
        this.svtIndividuality = data.individuality;
        if (deckSvt.isFollowerSvt)
        {
            this.followerType = Follower.getType(infollowerType);
            this.iconLimitCount = ImageLimitCount.GetImageLimitCount(this.svtId, this.limitcount);
        }
        this.flgEventJoin = data.IsEventJoin();
        this.svtdata = ((ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT)).getEntityFromId<ServantEntity>(data.getBattleSvtId());
        if (this.svtdata == null)
        {
            Debug.LogError("setServantData: Servant not found : " + data.getBattleSvtId());
        }
        this.svtType = this.svtdata.type;
        ServantLimitMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT);
        long[] args = new long[] { (long) this.getSvtId(), (long) this.limitcount };
        if (master2.isEntityExistsFromId(args))
        {
            this.svtlimitent = master2.getEntityFromId<ServantLimitEntity>(this.getSvtId(), this.limitcount);
        }
        else
        {
            this.svtlimitent = master2.getEntityFromId<ServantLimitEntity>(0x18704, 0);
        }
        long[] numArray9 = new long[] { (long) this.getSvtId(), (long) this.getDispLimitCount() };
        if (master2.isEntityExistsFromId(numArray9))
        {
            this.svtlimitDispent = master2.getEntityFromId<ServantLimitEntity>(this.getSvtId(), this.getDispLimitCount());
        }
        else
        {
            this.svtlimitDispent = master2.getEntityFromId<ServantLimitEntity>(0x18704, 0);
        }
        ServantLimitAddMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        long[] numArray10 = new long[] { (long) this.svtdata.id, (long) this.getDispLimitCount() };
        if (master3.isEntityExistsFromId(numArray10))
        {
            this.svtlimitaddent = master3.getEntityFromId<ServantLimitAddEntity>(this.svtdata.id, this.getDispLimitCount());
        }
        else
        {
            this.svtlimitaddent = null;
        }
        long[] numArray = data.getBattleEquipTargetList();
        this.equipList = new BattleUserServantData[numArray.Length];
        this.equipatk = 0;
        this.equiphp = 0;
        for (int i = 0; i < this.equipList.Length; i++)
        {
            this.equipList[i] = battleInfo.getEquipFromID(numArray[i], deckSvt.userId);
            if (this.equipList[i] != null)
            {
                this.equipatk += this.equipList[i].atk;
                this.equiphp += this.equipList[i].hp;
            }
        }
        this.atk += this.equipatk;
        this.maxhp += this.equiphp;
        int[] numArray2 = data.getBattleSkillIdList();
        int[] numArray3 = data.getSkillLevelList();
        for (int j = 0; j < numArray2.Length; j++)
        {
            this.addSkillInfo(BattleSkillInfoData.TYPE.SERVANT_SELF, j, numArray2[j], numArray3[j]);
        }
        int num3 = 0;
        int[] numArray4 = this.SvtEnt.getClassPassive();
        for (int k = 0; k < numArray4.Length; k++)
        {
            this.addSkillInfo(BattleSkillInfoData.TYPE.SERVANT_CLASS, k, numArray4[k], 1);
            num3 = k + 1;
        }
        int[] numArray5 = data.getPassiveSkill();
        for (int m = 0; m < numArray5.Length; m++)
        {
            this.addSkillInfo(BattleSkillInfoData.TYPE.SERVANT_CLASS, num3 + m, numArray5[m], 1);
        }
        int index = 0;
        for (int n = 0; n < this.equipList.Length; n++)
        {
            if (this.equipList[n] != null)
            {
                int[] numArray6 = this.equipList[n].getBattleSkillIdList();
                int[] numArray7 = this.equipList[n].getSkillLevelList();
                for (int num8 = 0; num8 < numArray6.Length; num8++)
                {
                    this.addSkillInfo(BattleSkillInfoData.TYPE.SERVANT_EQUIP, index, numArray6[num8], numArray7[num8]);
                    index++;
                }
            }
        }
        this.InitializeBuff();
        this.treasuredvcId = data.treasureDeviceId;
        this.treasuredvcLevel = data.treasureDeviceLv;
        if (0 < this.treasuredvcId)
        {
            this.TDvc = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.treasuredvcId);
            this.TDvcLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(this.treasuredvcId, this.treasuredvcLevel);
            this.SvtTDvc = ServantTreasureDvcMaster.getEntityFromIDID(this.svtId, this.treasuredvcId);
            this.nplineCount = this.TDvcLv.gaugeCount;
        }
        else
        {
            this.TDvc = null;
            this.nplineCount = 0;
        }
        this.lineMaxNp = ConstantMaster.getValue("FULL_TD_POINT");
        this.commandtable = new Hashtable();
        ServantCardMaster master4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CARD) as ServantCardMaster;
        int[] numArray8 = new int[] { 1, 2, 3, 4, 10, 11 };
        for (int num9 = 0; num9 < numArray8.Length; num9++)
        {
            long[] numArray11 = new long[] { (long) this.svtId, (long) numArray8[num9] };
            if (master4.isEntityExistsFromId(numArray11))
            {
                ServantCardEntity entity = master4.getEntityFromId<ServantCardEntity>(this.svtId, numArray8[num9]);
                this.commandtable[entity.cardId] = master4.getEntityFromId<ServantCardEntity>(this.svtId, entity.cardId);
            }
        }
        this.deadtype = DEADTYPE.NORMAL;
        if (deckSvt.isEscape())
        {
            this.deadtype = DEADTYPE.ESCAPE;
        }
        this.isAppear = deckSvt.isAppear();
        this.roleType = deckSvt.getRoleType();
        this.maxActNum = data.maxActNum;
        this.aiId = data.aiId;
        this.actPriority = data.actPriority;
        this.aiState.Initialize(data.aiId);
    }

    public void setTransformServant(BattleInfoData battleInfo, int transformIndex)
    {
        if (transformIndex == -1)
        {
            BattleDeckServantData data = battleInfo.getDeckServantData(this.uniqueId);
            this.transformIndex = -1;
            this.transformUserSvtId = -1L;
            BattleUserServantData data2 = battleInfo.getUserServantFromID(data.userSvtId, data.userId);
            this.deckSvt = data;
            this.atk = data2.atk;
            this.maxhp = data2.hp;
            this.maxtpturn = data2.chargeTurn;
            if (data2.maxActNum != 0)
            {
                this.maxActNum = data2.maxActNum;
            }
            if (data2.actPriority != 0)
            {
                this.actPriority = data2.actPriority;
            }
            this.atk += this.equipatk;
            this.maxhp += this.equiphp;
            this.downstarrate = data2.starRate;
            this.downtdrate = data2.tdRate;
            this.deathRate = data2.deathRate;
            this.criticalRate = data2.criticalRate;
            this.treasuredvcId = data2.treasureDeviceId;
            this.treasuredvcLevel = data2.treasureDeviceLv;
            this.transformSvtId = data2.getBattleSvtId();
            this.svtName = data.name;
            this.overkillTargetId = -1;
            this.svtIndividuality = data2.individuality;
        }
        else
        {
            BattleDeckServantData data3 = battleInfo.getTransformDeckServantData(this.uniqueId, transformIndex);
            this.transformIndex = transformIndex;
            this.transformUserSvtId = data3.getUserServantID();
            BattleUserServantData data4 = battleInfo.getUserServantFromID(this.transformUserSvtId, data3.userId);
            this.deckSvt = data3;
            this.atk = data4.atk;
            this.maxhp = data4.hp;
            this.maxtpturn = data4.chargeTurn;
            if (data4.maxActNum != 0)
            {
                this.maxActNum = data4.maxActNum;
            }
            if (data4.actPriority != 0)
            {
                this.actPriority = data4.actPriority;
            }
            this.atk += this.equipatk;
            this.maxhp += this.equiphp;
            this.downstarrate = data4.starRate;
            this.downtdrate = data4.tdRate;
            this.deathRate = data4.deathRate;
            this.criticalRate = data4.criticalRate;
            this.treasuredvcId = data4.treasureDeviceId;
            this.treasuredvcLevel = data4.treasureDeviceLv;
            this.transformSvtId = data4.getBattleSvtId();
            this.svtName = data3.name;
            this.overkillTargetId = -1;
            this.svtIndividuality = data4.individuality;
        }
    }

    public void skillChageExtend(int param, int max = 0x3e7)
    {
        foreach (BattleSkillInfoData data in this.skillInfoList)
        {
            data.TurnExtend(param, max);
        }
    }

    public void skillChageShorten(int param, int max = 0)
    {
        foreach (BattleSkillInfoData data in this.skillInfoList)
        {
            data.TurnProgress(param, max);
        }
    }

    public bool subBuffFromIndividualites(int[] individuality)
    {
        int num = this.getMaxHp();
        bool flag = this.buffData.subBuffFromIndividualites(individuality);
        this.checkUpdateUpdownHp(num, false);
        return flag;
    }

    public BattleBuffData.BuffData[] turnBuffProgressing(BattleActionData action = null)
    {
        BattleBuffData.BuffData[] dataArray = this.buffData.turnProgressing();
        this.updateBuff();
        this.updateHp();
        return dataArray;
    }

    public void turnBuffProgressingIncrease()
    {
        this.buffData.turnProgressingIncrease();
    }

    public bool turnProgressing(BattleLogic logic, bool isAlive, BattleActionData actiondata)
    {
        bool flag = false;
        if (!this.isEnemy)
        {
            this.addNp(this.buffData.getReduceNP(), true);
            this.addNp(this.buffData.getRegainNP(), true);
        }
        int digit = 1;
        int heal = (this.buffData.getRegainHp() * this.buffData.getHealMagnification(out digit)) / digit;
        this.provisionalHeal(heal);
        if (0 < heal)
        {
            actiondata.setHealData(this.getUniqueID(), heal, 0, 0);
            flag = true;
        }
        int damage = this.buffData.getReduceHp();
        this.provisionalDamage(damage);
        if (0 < damage)
        {
            if (!isAlive && (this.hp <= damage))
            {
                damage = this.hp - 1;
            }
            BattleActionData.DamageData data = new BattleActionData.DamageData {
                targetId = this.getUniqueID()
            };
            data.damagelist = new int[] { damage };
            data.functionIndex = 0;
            actiondata.setDamageData(data);
            this.setActionHistory(-1, BattleServantActionHistory.TYPE.REDUCE_HP, logic.getWave());
            flag = true;
        }
        if (!this.isEnemy)
        {
            int num = this.buffData.getRegainStar();
            actiondata.addCriticalStar(num);
            logic.data.addCriticalPoint(num);
            logic.perf.statusPerf.updateCriticalPoint();
        }
        if (this.isUseSkill())
        {
            foreach (BattleSkillInfoData data2 in this.skillInfoList)
            {
                this.isChargeSkill |= data2.TurnProgress(1, 0);
            }
        }
        if (!this.isUseNP)
        {
            if ((!this.isTDSeraled() && this.isNobleAction()) && (0 < this.treasuredvcId))
            {
                this.updownNextTDTurn(-1);
            }
            return flag;
        }
        this.isUseNP = false;
        return flag;
    }

    public void updateBuff()
    {
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("updateBuffIconList", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void updateHp()
    {
        if (this.getMaxHp() < this.getNowHp())
        {
            this.hp = this.getMaxHp();
        }
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("changeNp", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void updateNpGauge()
    {
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("updateNp", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void updateTDGauge()
    {
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("updateTDGauge", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void updateView()
    {
        foreach (GameObject obj2 in this.paramobjelist)
        {
            obj2.SendMessage("updateView", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void updownNextTDTurn(int val)
    {
        this.nexttpturn += val;
        if (this.maxtpturn < this.nexttpturn)
        {
            this.nexttpturn = this.maxtpturn;
        }
        if (this.nexttpturn < 0)
        {
            this.nexttpturn = 0;
        }
    }

    public void usedTpWeapon(int addPer)
    {
        this.nexttpturn = this.maxtpturn;
        this.isUseNP = true;
        int num = Mathf.FloorToInt((((float) this.getNp()) / ((float) this.lineMaxNp)) * 100f);
        if (num < 100)
        {
            num = 100;
        }
        int num2 = this.buffData.getUpChageTd();
        this.tmp_npper = (num + addPer) + (num2 * 100);
        if (500 < this.tmp_npper)
        {
            this.tmp_npper = 500;
        }
        this.np = 0;
    }

    public int useGuts() => 
        this.buffData.useGuts();

    public void useSkill(BattleSkillInfoData skillInfo)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
        skillInfo.chargeTurn = entity.chargeTurn;
    }

    public int BattleSize =>
        this.svtdata.battleSize;

    public int commandCardLimitCount { get; set; }

    public int dispLimitCount { get; set; }

    public int frameType { get; set; }

    public int iconLimitCount { get; set; }

    public ServantEntity SvtEnt =>
        this.svtdata;

    public ServantLimitAddEntity SvtLimitAddEnt =>
        this.svtlimitaddent;

    public TreasureDvcEntity TreasureDevice =>
        this.TDvc;

    public int TreasureDvcId =>
        this.treasuredvcId;

    [CompilerGenerated]
    private sealed class <getSelfSkillInfo>c__AnonStorey7E
    {
        internal int index;

        internal bool <>m__9E(BattleSkillInfoData s) => 
            ((s.type == BattleSkillInfoData.TYPE.SERVANT_SELF) && (s.index == this.index));
    }

    [CompilerGenerated]
    private sealed class <getTempSkillInfo>c__AnonStorey7F
    {
        internal int index;

        internal bool <>m__9F(BattleSkillInfoData s) => 
            ((s.type == BattleSkillInfoData.TYPE.TEMP) && (s.index == this.index));
    }

    [CompilerGenerated]
    private sealed class <isUseSelfSkill>c__AnonStorey7D
    {
        internal int index;

        internal bool <>m__9D(BattleSkillInfoData s) => 
            ((s.type == BattleSkillInfoData.TYPE.SERVANT_SELF) && (s.index == this.index));
    }

    public enum DEADTYPE
    {
        NORMAL,
        ESCAPE
    }

    public class SaveData
    {
        public AiState.SaveData aistateSave;
        public BattleBuffData.SaveData buffDataSave;
        public int deckIndex = -1;
        public int hp;
        public int isDeadAnime;
        public int isEntry;
        public int nextNp;
        public int np;
        public int[] skillcharge_list;
        public int[] skillinfoid_list;
        public int transformIndex = -1;
    }

    public enum STATUS
    {
        NOMAL,
        ACT_RESURRECTION,
        ACT_DEAD
    }
}

