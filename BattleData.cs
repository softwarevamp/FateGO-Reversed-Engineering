using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<BattleServantData> <>f__am$cache3D;
    [CompilerGenerated]
    private static Comparison<BattleServantData> <>f__am$cache3E;
    private static readonly string AfterTalkResumeKey = "TalkResume";
    private static readonly string AfterTalkResumeVersion = "1.0";
    private BattleEntity battle_ent;
    private BattleInfoData battle_info;
    public bool bBattleLiveError;
    public bool bReceiveServerData;
    public BattleComboData combodata;
    public List<CommandHistory> comhistory;
    private const string continueBattleSaveKey = "CONTINUE_BATTLE_SAVEKEY";
    public int countEnemyAttack;
    public int criticalstars;
    public BattleData data;
    public BattleCommandData[] draw_commandlist;
    public List<BattleDropItem> droplist;
    public int[] e_entryid;
    public BattleServantData[] e_svlist;
    public int enemyActCount = StageEntity.DEFAULT_ENEMY_ACTION_COUNT;
    public BattleError errorinfo;
    public bool flg_resumebattle;
    public PlayMakerFSM fsm;
    public int globaltargetId;
    public int initturn;
    private int lastactorId;
    public int limitAct;
    public int limitTurnCount;
    public BattleLogic logic;
    public List<BattleSkillInfoData> masterSkillInfo;
    public int[] p_changedecklist;
    public BattleCommandData[] p_cmlist;
    public int[] p_entryid;
    public int p_maxcommand;
    public BattleCommandData[] p_shcmlist;
    public BattlePerformance perf;
    public List<BattleServantData> player_datalist;
    private long prevactId;
    private long prevtargetId;
    private QuestPhaseEntity questphase_ent;
    private const string questPhaseSaveKey = "QUESTPHASEID";
    private const string questSaveKey = "QUESTID";
    private const string reserveSavekey = "RESERVE_BATTKE_SAVEKEY";
    public PlayMakerFSM rootfsm;
    private const string savekey = "SAVEKEY_";
    public BattleCommandData[] selectcommandlist;
    public int shuffleindex;
    private static readonly string skipKey = "skipSkill";
    private static readonly string speedKey = "hightSpeed";
    public int stateshowturn;
    public int systemflg_acceleration = 1;
    public bool systemflg_autobattle;
    public bool systemflg_autosave = true;
    public bool systemflg_selectcancel = true;
    public bool systemflg_showautobutton;
    public bool systemflg_skipskillconf;
    public bool systemflg_TdConstantvelocity;
    private static readonly string TdCvKey = "TdConstantVel";
    public int totalTurnCount;
    public int turnCount;
    public int[] turnEffect = new int[0];
    public int turnEffectType;
    public int tutorialId = -1;
    public int tutorialState = -1;
    public TYPETURN typeTurn = TYPETURN.PLAYER;
    public List<BattleUsedSkills> usedSkilllist = new List<BattleUsedSkills>();
    private const string version = "0.6.17.1";
    protected Dictionary<string, object> voicePlayedInfo;
    public int wavecount;
    public int win_lose;

    public void addCriticalPoint(int count)
    {
        this.criticalstars += count;
        if (0x63 < this.criticalstars)
        {
            this.criticalstars = 0x63;
        }
    }

    public void addDamage(int targetId, int damage)
    {
        this.getServantData(targetId).addDamage(damage);
    }

    public void addDropItems(DropInfo item)
    {
        BattleDropItem item2 = new BattleDropItem();
        item2.setData(item);
        this.droplist.Add(item2);
    }

    public void addHeal(int targetId, int heal)
    {
        this.getServantData(targetId).healHp(heal);
    }

    public void addLogCommand()
    {
        for (int i = 0; i < this.selectcommandlist.Length; i++)
        {
            CommandHistory item = new CommandHistory {
                uid = this.selectcommandlist[i].getUniqueId(),
                ty = this.selectcommandlist[i].getCommandType()
            };
            this.comhistory.Add(item);
        }
    }

    public void addMasterSkillInfo(BattleSkillInfoData.TYPE type, int index, int skillId, int skillLv)
    {
        BattleSkillInfoData item = new BattleSkillInfoData {
            type = type,
            svtUniqueId = -1,
            index = index,
            skillId = skillId,
            skilllv = skillLv
        };
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillId);
        if (entity != null)
        {
            item.isUseSkill = true;
            item.isPassive = entity.isPassive();
            item.chargeTurn = 0;
        }
        else
        {
            item.isUseSkill = false;
        }
        this.masterSkillInfo.Add(item);
    }

    public void AddServantVoicePlayed(int svtId, int condValue)
    {
        string key = svtId.ToString();
        if (!this.voicePlayedInfo.ContainsKey(key))
        {
            this.voicePlayedInfo[key] = 0L;
        }
        this.voicePlayedInfo[key] = ((long) this.voicePlayedInfo[key]) | (((long) 1L) << condValue);
    }

    public bool checkAliveEnemys()
    {
        bool flag = false;
        for (int i = 0; i < this.e_svlist.Length; i++)
        {
            flag |= this.e_svlist[i].isAlive();
        }
        return flag;
    }

    public bool checkAliveOther(int uniqueId)
    {
        if (this.data.isPlayerID(uniqueId))
        {
            return this.checkAliveEnemys();
        }
        return (this.data.isEnemyID(uniqueId) && this.checkAlivePlayers());
    }

    public bool checkAlivePlayers()
    {
        bool flag = false;
        foreach (BattleServantData data in this.player_datalist)
        {
            flag |= data.isAlive();
        }
        return flag;
    }

    public bool checkEndBattle() => 
        this.battle_info.isNextBattle(this.wavecount);

    public bool checkLimitTurn() => 
        (0 < this.limitTurnCount);

    public bool checkTurnData()
    {
        if (!SingletonMonoBehaviour<BattleSequenceManager>.Instance.testMode)
        {
            if (EncryptedPlayerPrefs.HasKey("SAVEKEY_"))
            {
                if (!"0.6.17.1".Equals(EncryptedPlayerPrefs.GetString("Version")))
                {
                    if ("Initial".Equals(EncryptedPlayerPrefs.GetString("Version")))
                    {
                        return false;
                    }
                }
                else if (EncryptedPlayerPrefs.HasKey("QUESTID"))
                {
                    int @int = EncryptedPlayerPrefs.GetInt("QUESTID");
                    if (SingletonMonoBehaviour<DataManager>.getInstance().getSingleEntity<BattleEntity>(DataNameKind.Kind.BATTLE).questId == @int)
                    {
                        return true;
                    }
                }
            }
            EncryptedPlayerPrefs.DeleteKey("SAVEKEY_");
            PlayerPrefs.Save();
        }
        return false;
    }

    public void checkTutorialData()
    {
        this.tutorialId = -1;
        BattleEntity entity = this.getBattleEntity();
        if (entity.questId == ConstantMaster.getValue("TUTORIAL_QUEST_ID1"))
        {
            this.tutorialId = 1;
        }
        else if (entity.questId == ConstantMaster.getValue("TUTORIAL_QUEST_ID2"))
        {
            this.tutorialId = 2;
        }
        else if (entity.questId == ConstantMaster.getValue("TUTORIAL_QUEST_ID3"))
        {
            this.tutorialId = 3;
        }
        else if ((entity.questId == ConstantMaster.getValue("TUTORIAL_QUEST_ID4")) && (entity.questPhase == ConstantMaster.getValue("TUTORIAL_QUEST_ID4_PHASE")))
        {
            this.tutorialId = 4;
        }
    }

    public void clearLastActionActor()
    {
        this.lastactorId = 0;
    }

    public void commonQuestLoad()
    {
        this.battle_ent = SingletonMonoBehaviour<DataManager>.getInstance().getSingleEntity<BattleEntity>(DataNameKind.Kind.BATTLE);
        this.battle_info = this.battle_ent.battleInfo;
        this.questphase_ent = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestPhaseMaster>(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(this.battle_ent.questId, this.battle_ent.questPhase);
        BattleRandom.setSeed(this.battle_ent.seed);
        this.checkTutorialData();
        this.wavecount = 0;
        this.turnCount = 0;
        this.totalTurnCount = 0;
        this.criticalstars = 0;
    }

    public void createCommandCard()
    {
        List<BattleCommandData> list = new List<BattleCommandData>();
        for (int i = 0; i < this.p_entryid.Length; i++)
        {
            if (0 < this.p_entryid[i])
            {
                Debug.Log(string.Concat(new object[] { "p_entryid[", i, "]:", this.p_entryid[i] }));
                BattleServantData data = this.getPlayerServantData(this.p_entryid[i]);
                int[] numArray = data.getCommandList();
                for (int k = 0; k < numArray.Length; k++)
                {
                    BattleCommandData item = new BattleCommandData {
                        type = numArray[k],
                        svtId = data.svtId,
                        svtlimit = data.getCommandDispLimitCount(),
                        loadSvtLimit = data.getDispLimitCount(),
                        uniqueId = data.getUniqueID(),
                        attri = data.getAttri(),
                        flgEventJoin = data.flgEventJoin
                    };
                    item.setFollowerType(data.followerType);
                    list.Add(item);
                }
            }
        }
        for (int j = 0; j < this.battle_info.blankCardCnt; j++)
        {
            BattleCommandData data3 = new BattleCommandData {
                type = 5,
                svtId = -1,
                uniqueId = -1
            };
            list.Add(data3);
        }
        this.p_cmlist = list.ToArray();
    }

    public static void DeleteBattleAfterTalkResumeInfo()
    {
        EncryptedPlayerPrefs.DeleteKey(AfterTalkResumeKey);
    }

    public static bool deleteSaveData()
    {
        EncryptedPlayerPrefs.DeleteKey("CONTINUE_BATTLE_SAVEKEY");
        EncryptedPlayerPrefs.DeleteKey("SAVEKEY_");
        PlayerPrefs.Save();
        return true;
    }

    public static bool GetBattleAfterTalkResumeInfo(out int questId, out int phaseId)
    {
        questId = -1;
        phaseId = -1;
        if (!EncryptedPlayerPrefs.HasKey(AfterTalkResumeKey))
        {
            return false;
        }
        string jsonstr = EncryptedPlayerPrefs.GetString(AfterTalkResumeKey, null);
        if (jsonstr == null)
        {
            return false;
        }
        Dictionary<string, object> dictionary = JsonManager.getDictionary(jsonstr);
        if (((string) dictionary["version"]) != AfterTalkResumeVersion)
        {
            return false;
        }
        questId = (int) ((long) dictionary["questId"]);
        phaseId = (int) ((long) dictionary["phase"]);
        return true;
    }

    public BattleEntity getBattleEntity() => 
        this.battle_ent;

    public BattleInfoData getBattleInfo() => 
        this.battle_info;

    public int getChangeType(int[] outList, int[] inList, int targetType)
    {
        for (int i = 0; i < outList.Length; i++)
        {
            if (targetType == outList[i])
            {
                outList[i] = -1;
                return inList[i];
            }
        }
        return inList[0];
    }

    public string getCommandHistory() => 
        ("{ \"logs\":" + JsonManager.toJson(this.comhistory.ToArray()) + "}");

    public static int getContinueBattleFlg()
    {
        if (EncryptedPlayerPrefs.HasKey("CONTINUE_BATTLE_SAVEKEY"))
        {
            return EncryptedPlayerPrefs.GetInt("CONTINUE_BATTLE_SAVEKEY");
        }
        return 0;
    }

    public int getCriticalPoint() => 
        this.criticalstars;

    public BattleDropItem[] getDropItems() => 
        this.droplist.ToArray();

    public BattleServantData getEnemyServantData(int Id)
    {
        for (int i = 0; i < this.e_svlist.Length; i++)
        {
            if (this.e_svlist[i].checkID(Id))
            {
                return this.e_svlist[i];
            }
        }
        return null;
    }

    public BattleServantData getEnemyServantDataIndex(int index)
    {
        int uniqueId = this.e_entryid[index];
        Debug.Log("setTargetIndex:" + uniqueId);
        if (uniqueId != -1)
        {
            return this.getServantData(uniqueId);
        }
        return null;
    }

    public int[] getEnemyServantIDList(bool ckDead = true)
    {
        List<int> list = new List<int>();
        foreach (BattleServantData data in this.e_svlist)
        {
            if ((data != null) && (!ckDead || !data.isDead()))
            {
                list.Add(data.getUniqueID());
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getEnemyServantList() => 
        this.e_svlist;

    public BattleServantData getEnemySubServantData()
    {
        for (int i = 0; i < this.e_svlist.Length; i++)
        {
            if (!this.e_svlist[i].isEntry)
            {
                return this.e_svlist[i];
            }
        }
        return null;
    }

    public int[] getFieldEnemyServantIDList()
    {
        List<int> list = new List<int>();
        foreach (int num in this.e_entryid)
        {
            if (num > 0)
            {
                list.Add(num);
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getFieldEnemyServantList()
    {
        List<BattleServantData> list = new List<BattleServantData>();
        foreach (int num in this.e_entryid)
        {
            if (num > 0)
            {
                for (int i = 0; i < this.e_svlist.Length; i++)
                {
                    if (this.e_svlist[i].checkID(num))
                    {
                        list.Add(this.e_svlist[i]);
                    }
                }
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getFieldOpponentList(int uniqueId)
    {
        if (this.isEnemyID(uniqueId))
        {
            return this.getFieldPlayerServantList();
        }
        return this.getFieldEnemyServantList();
    }

    public int[] getFieldPlayerServantIDList()
    {
        List<int> list = new List<int>();
        foreach (int num in this.p_entryid)
        {
            if (num > 0)
            {
                list.Add(num);
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getFieldPlayerServantList()
    {
        List<BattleServantData> list = new List<BattleServantData>();
        foreach (int num in this.p_entryid)
        {
            if (num > 0)
            {
                foreach (BattleServantData data in this.player_datalist)
                {
                    if (data.checkID(num))
                    {
                        list.Add(data);
                    }
                }
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getFieldPTList(int uniqueId)
    {
        if (this.isEnemyID(uniqueId))
        {
            return this.getFieldEnemyServantList();
        }
        return this.getFieldPlayerServantList();
    }

    public BattleServantData[] getFieldServantList()
    {
        List<BattleServantData> list = new List<BattleServantData>();
        list.AddRange(this.getFieldEnemyServantList());
        list.AddRange(this.getFieldPlayerServantList());
        return list.ToArray();
    }

    public int getGroundNo()
    {
        if (this.questphase_ent != null)
        {
            return this.questphase_ent.battleBgId;
        }
        return 0x13948;
    }

    public Color getGroundShadowColor()
    {
        uint maxValue = uint.MaxValue;
        float num2 = 0.003921569f;
        Color black = Color.black;
        black.r = num2 * ((maxValue >> 0x18) & 0xff);
        black.g = num2 * ((maxValue >> 0x10) & 0xff);
        black.b = num2 * ((maxValue >> 8) & 0xff);
        black.a = num2 * (maxValue & 0xff);
        return black;
    }

    public int getGroundType() => 
        this.questphase_ent.battleBgType;

    public int getLastActionActor() => 
        this.lastactorId;

    public int getLastWave() => 
        this.battle_info.getLastWave();

    public int getMasterEquipId()
    {
        UserEquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.battle_info.userEquipId);
        int equipId = 0;
        if (entity != null)
        {
            equipId = entity.equipId;
        }
        return equipId;
    }

    public BattleSkillInfoData getMasterSkillInfo(int index)
    {
        <getMasterSkillInfo>c__AnonStorey78 storey = new <getMasterSkillInfo>c__AnonStorey78 {
            index = index
        };
        return this.masterSkillInfo.Find(new Predicate<BattleSkillInfoData>(storey.<>m__8F));
    }

    public BattleSkillInfoData[] getMasterSkillInfos() => 
        this.masterSkillInfo.ToArray();

    public int getMasterUserEquipId() => 
        this.battle_info.userEquipId;

    public BattleServantData getPlayerServantData(int Id)
    {
        foreach (BattleServantData data in this.player_datalist)
        {
            if (data.checkID(Id))
            {
                return data;
            }
        }
        return null;
    }

    public int[] getPlayerServantIDList(bool ckDead = true)
    {
        List<int> list = new List<int>();
        foreach (BattleServantData data in this.player_datalist)
        {
            if ((data != null) && (!ckDead || !data.isDead()))
            {
                list.Add(data.getUniqueID());
            }
        }
        return list.ToArray();
    }

    public BattleServantData[] getPlayerServantList() => 
        this.player_datalist.ToArray();

    public BattleServantData getPlayerSubServantData()
    {
        if (<>f__am$cache3D == null)
        {
            <>f__am$cache3D = (a, b) => a.getDeckIndex() - b.getDeckIndex();
        }
        this.player_datalist.Sort(<>f__am$cache3D);
        foreach (BattleServantData data in this.player_datalist)
        {
            if (!data.isEntry)
            {
                return data;
            }
        }
        return null;
    }

    public int[] getQuestIndividualitie()
    {
        if (this.questphase_ent != null)
        {
            return this.questphase_ent.getIndividuality();
        }
        return new int[0];
    }

    public static long getResumeBattleId()
    {
        if (EncryptedPlayerPrefs.HasKey("SAVEKEY_"))
        {
            return EncryptedPlayerPrefs.GetLong("SAVEKEY_");
        }
        return -1L;
    }

    public static int getSavedQuestId()
    {
        if (EncryptedPlayerPrefs.HasKey("QUESTID"))
        {
            return EncryptedPlayerPrefs.GetInt("QUESTID");
        }
        return 0;
    }

    public static int getSavedQuestPhase()
    {
        if (EncryptedPlayerPrefs.HasKey("QUESTPHASEID"))
        {
            return EncryptedPlayerPrefs.GetInt("QUESTPHASEID");
        }
        return 0;
    }

    public BattleComboData getSelectCombo() => 
        this.combodata;

    public BattleCommandData getSelectCommand(int index) => 
        this.selectcommandlist[index];

    public BattleCommandData[] getSelectCommands() => 
        this.selectcommandlist;

    public BattleServantData getServantData(int uniqueId)
    {
        BattleServantData data = this.getPlayerServantData(uniqueId);
        if (data != null)
        {
            return data;
        }
        data = this.getEnemyServantData(uniqueId);
        if (data != null)
        {
            return data;
        }
        return null;
    }

    public BattleCommandData getShuffleCommand()
    {
        BattleCommandData data = this.p_shcmlist[this.shuffleindex];
        this.shuffleindex++;
        if (this.p_cmlist.Length <= this.shuffleindex)
        {
            this.shuffleCommand();
        }
        return data;
    }

    public BattleServantData[] getSubPlayerServantList()
    {
        if (<>f__am$cache3E == null)
        {
            <>f__am$cache3E = (a, b) => a.getDeckIndex() - b.getDeckIndex();
        }
        this.player_datalist.Sort(<>f__am$cache3E);
        List<BattleServantData> list = new List<BattleServantData>();
        foreach (BattleServantData data in this.player_datalist)
        {
            if (!data.isEntry)
            {
                list.Add(data);
            }
        }
        return list.ToArray();
    }

    public void Initialize()
    {
        this.fsm.Fsm.GetFsmGameObject("CameraFSM").Value = this.perf.camerafsm.gameObject;
        this.fsm.Fsm.GetFsmGameObject("CommandFSM").Value = this.perf.commandPerf.gameObject;
        this.fsm.Fsm.GetFsmGameObject("FieldmotionFSM").Value = this.perf.fieldmotionfsm.gameObject;
        this.fsm.Fsm.GetFsmGameObject("Logic").Value = this.logic.gameObject;
        this.fsm.Fsm.GetFsmGameObject("Performance").Value = this.perf.gameObject;
        this.fsm.Fsm.GetFsmGameObject("RootFSM").Value = this.rootfsm.gameObject;
        this.fsm.Fsm.GetFsmGameObject("UIFSM").Value = this.perf.statusPerf.gameObject;
        this.p_maxcommand = 3;
        this.wavecount = 0;
        this.turnCount = 1;
        this.globaltargetId = -1;
        this.criticalstars = 0;
        this.countEnemyAttack = 0;
        this.voicePlayedInfo = new Dictionary<string, object>();
        this.masterSkillInfo = new List<BattleSkillInfoData>();
        this.p_entryid = new int[3];
        for (int i = 0; i < this.p_entryid.Length; i++)
        {
            this.p_entryid[i] = -1;
        }
        this.e_entryid = new int[3];
        for (int j = 0; j < this.e_entryid.Length; j++)
        {
            this.e_entryid[j] = -1;
        }
        this.win_lose = 0;
        this.comhistory = new List<CommandHistory>();
        this.droplist = new List<BattleDropItem>();
        this.p_shcmlist = new BattleCommandData[0];
        this.draw_commandlist = new BattleCommandData[0];
        this.e_svlist = new BattleServantData[0];
        this.loadSkipSkillConf();
        this.loadHighSpeedMode();
        this.loadTdConstantVelocity();
    }

    public void initQuest()
    {
        Debug.Log(" BattleData.initQuest() ");
        this.commonQuestLoad();
        if (SingletonMonoBehaviour<BattleSequenceManager>.Instance.testMode)
        {
            BattleEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getSingleEntity<BattleEntity>(DataNameKind.Kind.BATTLE);
            BattleUserServantData data = entity.battleInfo.getUserServantFromID(entity.battleInfo.myDeck.svts[0].userSvtId, 0L);
            data.svtId = SingletonMonoBehaviour<BattleSequenceManager>.Instance.servantId;
            data.limitCount = SingletonMonoBehaviour<BattleSequenceManager>.Instance.limitCount;
            ServantTreasureDvcEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_TREASUREDEVICE).getEntityFromId<ServantTreasureDvcEntity>(data.svtId, 1, 0x65);
            Debug.LogError(string.Concat(new object[] { "svtTdEnt:", entity2, ":", data.svtId }));
            data.treasureDeviceId = entity2.treasureDeviceId;
            data.treasureDeviceLv = 1;
            data.hp = 0xf4240;
            data = entity.battleInfo.getUserServantFromID(entity.battleInfo.enemyDeck[0].svts[0].userSvtId, 0L);
            data.svtId = SingletonMonoBehaviour<BattleSequenceManager>.Instance.servantId;
            data.limitCount = SingletonMonoBehaviour<BattleSequenceManager>.Instance.limitCount;
            data.treasureDeviceId = entity2.treasureDeviceId;
            data.treasureDeviceLv = 1;
            data.hp = 0xf4240;
        }
        DeckData myDeck = this.battle_info.myDeck;
        this.player_datalist = new List<BattleServantData>();
        foreach (BattleDeckServantData data3 in myDeck.svts)
        {
            int index = data3.id - 1;
            Debug.Log("index:" + index);
            BattleServantData item = new BattleServantData();
            item.setServantData(data3, this.battle_info, this.battle_ent.followerType);
            item.setInitQuest();
            if (index <= 2)
            {
                this.p_entryid[index] = data3.getUniqueID();
                item.isEntry = true;
                ServantAssetLoadManager.preloadServant(item.getSvtId(), item.getDispLimitCount());
                ServantAssetLoadManager.preloadActorMotion(item.getWeaponGroup());
            }
            Debug.Log("UniqueID:" + item.getUniqueID());
            this.player_datalist.Add(item);
        }
        this.createCommandCard();
        this.shuffleCommand();
        this.loadMasterSkill();
    }

    public bool isEnemyID(int Id) => 
        (this.getEnemyServantData(Id) != null);

    public bool isFirstStage() => 
        (this.wavecount == 0);

    public bool isLastStage() => 
        this.battle_info.isLastStage(this.wavecount);

    public bool isLimitTurn() => 
        (0 < this.limitTurnCount);

    public bool isPlayerID(int Id) => 
        ((Id == -1) || (this.getPlayerServantData(Id) != null));

    public bool isPrevAttackMe(int actId, int targetId) => 
        ((this.prevactId == actId) && (this.prevtargetId == targetId));

    public static bool isReserveResumeBattle() => 
        (EncryptedPlayerPrefs.HasKey("RESERVE_BATTKE_SAVEKEY") && (EncryptedPlayerPrefs.GetInt("RESERVE_BATTKE_SAVEKEY") != 0));

    public bool isResumeBattle() => 
        this.flg_resumebattle;

    public bool isShowTurn() => 
        (0 < this.stateshowturn);

    public bool isTutorial() => 
        (this.tutorialId != -1);

    public bool isTutorialCard() => 
        ((this.tutorialId != -1) && (this.tutorialId != 4));

    public bool isTutorialclickTarget() => 
        (((this.tutorialId == 1) || (this.tutorialId == 2)) || (((this.tutorialId == 3) && (this.wavecount == 1)) && ((this.turnCount == 1) && (this.data.tutorialState == 1))));

    public bool isTutorialSelectsvtCancel() => 
        (this.tutorialId == 2);

    public void loadHighSpeedMode()
    {
        this.systemflg_acceleration = 1;
        if (EncryptedPlayerPrefs.HasKey(speedKey) && (EncryptedPlayerPrefs.GetInt(speedKey) == 2))
        {
            this.systemflg_acceleration = 2;
        }
    }

    public void loadMasterSkill()
    {
        int id = this.getMasterUserEquipId();
        UserEquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(id);
        if (entity != null)
        {
            int[] numArray = entity.getSkillIdList();
            for (int i = 0; i < numArray.Length; i++)
            {
                this.addMasterSkillInfo(BattleSkillInfoData.TYPE.MASTER_EQUIP, i, numArray[i], entity.getSkillLv(i));
            }
        }
    }

    public void loadNstage(int inwavecount)
    {
        for (int i = 0; i < this.e_entryid.Length; i++)
        {
            this.e_entryid[i] = -1;
        }
        this.loadStageData();
        foreach (BattleServantData data in this.player_datalist)
        {
            data.setInitBattle();
        }
        DeckData data2 = this.battle_info.getEnemyDeck(inwavecount);
        this.e_svlist = new BattleServantData[data2.svts.Length];
        foreach (BattleDeckServantData data3 in data2.svts)
        {
            int index = data3.id - 1;
            this.e_svlist[index] = new BattleServantData();
            this.e_svlist[index].setServantData(data3, this.battle_info, 0);
            this.e_svlist[index].setEnemy();
            this.e_svlist[index].setInitQuest();
            this.e_svlist[index].setInitBattle();
            if (index <= 2)
            {
                if (this.globaltargetId == -1)
                {
                    this.globaltargetId = data3.getUniqueID();
                }
                this.e_entryid[index] = data3.getUniqueID();
                this.e_svlist[index].isEntry = true;
                ServantAssetLoadManager.preloadServant(this.e_svlist[index].getSvtId(), this.e_svlist[index].getDispLimitCount());
                ServantAssetLoadManager.preloadActorMotion(this.e_svlist[index].getWeaponGroup());
            }
        }
        this.logic.actEnemyPtPassiveSkill();
    }

    public void loadQuest()
    {
        Debug.Log("BattleData::LoadTurnData");
        this.commonQuestLoad();
        Debug.Log("::LoadTurnData 0:" + EncryptedPlayerPrefs.GetString("Battle"));
        SaveData[] dataArray = JsonManager.DeserializeArray<SaveData>("[" + EncryptedPlayerPrefs.GetString("Battle") + "]");
        this.initturn = dataArray[0].initturn;
        this.wavecount = dataArray[0].wavecount;
        this.turnCount = dataArray[0].turnCount;
        this.totalTurnCount = dataArray[0].totalTurnCount;
        this.globaltargetId = dataArray[0].globaltargetId;
        this.criticalstars = dataArray[0].criticalstars;
        this.p_entryid = dataArray[0].p_entryid;
        this.e_entryid = dataArray[0].e_entryid;
        this.shuffleindex = dataArray[0].shuffleIndex;
        this.draw_commandlist = dataArray[0].drawcard;
        this.p_shcmlist = dataArray[0].shufflecard;
        this.voicePlayedInfo = dataArray[0].voicePlayedList;
        if (SingletonMonoBehaviour<BattleRandom>.getInstance() != null)
        {
            SingletonMonoBehaviour<BattleRandom>.getInstance().logicCount = dataArray[0].randLogicCount;
            SingletonMonoBehaviour<BattleRandom>.getInstance().gutsCount = dataArray[0].randGutsCount;
        }
        if (dataArray[0].history != null)
        {
            this.comhistory = new List<CommandHistory>(dataArray[0].history);
        }
        else
        {
            this.comhistory = new List<CommandHistory>();
        }
        if (dataArray[0].drop != null)
        {
            this.droplist = new List<BattleDropItem>(dataArray[0].drop);
        }
        else
        {
            this.droplist = new List<BattleDropItem>();
        }
        DeckData myDeck = this.battle_info.myDeck;
        this.player_datalist = new List<BattleServantData>();
        foreach (BattleDeckServantData data2 in myDeck.svts)
        {
            int num2 = data2.id - 1;
            Debug.Log("::LoadTurnData 9-" + num2);
            BattleServantData item = new BattleServantData();
            item.setServantData(data2, this.battle_info, this.battle_ent.followerType);
            item.setInitQuest();
            item.setSaveData(EncryptedPlayerPrefs.GetString("p_svlist" + num2));
            item.loadTransformServant(this.battle_info);
            for (int i = 0; i < this.p_entryid.Length; i++)
            {
                if (this.p_entryid[i] == item.getUniqueID())
                {
                    ServantAssetLoadManager.preloadServant(item.getSvtId(), item.getDispLimitCount());
                    ServantAssetLoadManager.preloadActorMotion(item.getWeaponGroup());
                }
            }
            this.player_datalist.Add(item);
        }
        this.loadMasterSkill();
        foreach (BattleSkillInfoData data4 in this.masterSkillInfo)
        {
            if (!data4.isPassive)
            {
                for (int j = 0; j < dataArray[0].master_infoId.Length; j++)
                {
                    if (dataArray[0].master_infoId[j] == data4.getInfoId())
                    {
                        data4.chargeTurn = dataArray[0].master_skillTurn[j];
                    }
                }
            }
        }
        this.createCommandCard();
        Debug.Log("::LoadTurnData");
    }

    public void loadSaveTurnNstage()
    {
        this.loadStageData();
        foreach (BattleServantData data in this.player_datalist)
        {
            data.setInitBattle();
        }
        DeckData data2 = this.battle_info.getEnemyDeck(this.wavecount);
        this.e_svlist = new BattleServantData[data2.svts.Length];
        foreach (BattleDeckServantData data3 in data2.svts)
        {
            int index = data3.id - 1;
            this.e_svlist[index] = new BattleServantData();
            this.e_svlist[index].setServantData(data3, this.battle_info, 0);
            this.e_svlist[index].setEnemy();
            this.e_svlist[index].setSaveData(EncryptedPlayerPrefs.GetString("e_svlist" + index));
            this.e_svlist[index].loadTransformServant(this.battle_info);
            if (!this.e_svlist[index].isDeadAnimation())
            {
                for (int i = 0; i < this.e_entryid.Length; i++)
                {
                    if (this.e_entryid[i] == this.e_svlist[index].getUniqueID())
                    {
                        ServantAssetLoadManager.preloadServant(this.e_svlist[index].getSvtId(), this.e_svlist[index].getDispLimitCount());
                        ServantAssetLoadManager.preloadActorMotion(this.e_svlist[index].getWeaponGroup());
                    }
                }
            }
        }
    }

    public void loadSkipSkillConf()
    {
        this.systemflg_skipskillconf = false;
        if (EncryptedPlayerPrefs.HasKey(skipKey) && (EncryptedPlayerPrefs.GetInt(skipKey) == 1))
        {
            this.systemflg_skipskillconf = true;
        }
    }

    public void loadStageData()
    {
        BattleEntity entity = this.getBattleEntity();
        int questId = entity.questId;
        int questPhase = entity.questPhase;
        int wavecount = this.data.wavecount;
        StageEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STAGE).getEntityFromId<StageEntity>(questId, questPhase, wavecount + 1);
        this.limitTurnCount = entity2.getLimitTurn();
        this.limitAct = entity2.getLimitAct();
        this.stateshowturn = entity2.getShowTurnState();
        this.turnEffect = entity2.getTurnEffect(this.limitTurnCount);
        this.turnEffectType = entity2.getTurnEffectType();
        this.enemyActCount = entity2.getEnemyActCount();
    }

    public void loadTdConstantVelocity()
    {
        this.systemflg_TdConstantvelocity = false;
        if (EncryptedPlayerPrefs.HasKey(TdCvKey) && (EncryptedPlayerPrefs.GetInt(TdCvKey) == 1))
        {
            this.systemflg_TdConstantvelocity = true;
        }
    }

    public void procPlayerContinue()
    {
        foreach (BattleServantData data in this.player_datalist)
        {
            data.setContinue(this);
            data.isEntry = false;
        }
        for (int i = 0; i < this.data.p_entryid.Length; i++)
        {
            foreach (BattleServantData data2 in this.player_datalist)
            {
                if (i == data2.index)
                {
                    this.data.p_entryid[i] = data2.getUniqueID();
                    data2.isEntry = true;
                }
            }
        }
        this.data.createCommandCard();
        this.data.shuffleCommand();
        this.turnCount++;
        this.logic.drawCommand();
        this.initturn = 1;
        this.SaveTurnData();
    }

    public void replaceCommandCard(int inUniqueID, int outUniqueID)
    {
        BattleServantData data = this.getServantData(outUniqueID);
        int[] array = new int[data.getCommandList().Length];
        data.getCommandList().CopyTo(array, 0);
        BattleServantData data2 = this.getServantData(inUniqueID);
        int[] numArray2 = new int[data2.getCommandList().Length];
        data2.getCommandList().CopyTo(numArray2, 0);
        foreach (BattleCommandData data3 in this.p_cmlist)
        {
            if (data3.getUniqueId() == data.getUniqueID())
            {
                data3.type = this.getChangeType(array, numArray2, data3.type);
                data3.svtId = data2.svtId;
                data3.svtlimit = data2.getCommandDispLimitCount();
                data3.loadSvtLimit = data2.getDispLimitCount();
                data3.uniqueId = data2.getUniqueID();
                data3.attri = data2.getAttri();
                data3.flgEventJoin = data2.flgEventJoin;
                data3.setFollowerType(data2.followerType);
            }
        }
        data.getCommandList().CopyTo(array, 0);
        foreach (BattleCommandData data4 in this.p_shcmlist)
        {
            if (data4.getUniqueId() == data.getUniqueID())
            {
                data4.type = this.getChangeType(array, numArray2, data4.type);
                data4.svtId = data2.svtId;
                data4.svtlimit = data2.getCommandDispLimitCount();
                data4.loadSvtLimit = data2.getDispLimitCount();
                data4.uniqueId = data2.getUniqueID();
                data4.attri = data2.getAttri();
                data4.flgEventJoin = data2.flgEventJoin;
                data4.setFollowerType(data2.followerType);
            }
        }
        if (5 <= this.shuffleindex)
        {
            for (int i = 0; i < 5; i++)
            {
                if ((((this.shuffleindex - 5) + i) < this.p_shcmlist.Length) && (this.draw_commandlist[i].getUniqueId() == data.getUniqueID()))
                {
                    this.draw_commandlist[i].type = this.p_shcmlist[(this.shuffleindex - 5) + i].type;
                    this.draw_commandlist[i].svtId = data2.svtId;
                    this.draw_commandlist[i].svtlimit = data2.getCommandDispLimitCount();
                    this.draw_commandlist[i].loadSvtLimit = data2.getDispLimitCount();
                    this.draw_commandlist[i].uniqueId = data2.getUniqueID();
                    this.draw_commandlist[i].attri = data2.getAttri();
                    this.draw_commandlist[i].flgEventJoin = data2.flgEventJoin;
                    this.draw_commandlist[i].setFollowerType(data2.followerType);
                }
            }
        }
        else
        {
            data.getCommandList().CopyTo(array, 0);
            foreach (BattleCommandData data5 in this.draw_commandlist)
            {
                if (data5.getUniqueId() == data.getUniqueID())
                {
                    data5.type = this.getChangeType(array, numArray2, data5.type);
                    data5.svtId = data2.svtId;
                    data5.svtlimit = data2.getCommandDispLimitCount();
                    data5.loadSvtLimit = data2.getDispLimitCount();
                    data5.uniqueId = data2.getUniqueID();
                    data5.attri = data2.getAttri();
                    data5.flgEventJoin = data2.flgEventJoin;
                    data5.setFollowerType(data2.followerType);
                }
            }
        }
    }

    public void resetCriticalPoint()
    {
        this.criticalstars = 0;
    }

    public static void SaveBattleAfterTalkResumeInfo(int questId, int phaseId)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object> {
            ["questId"] = questId,
            ["phase"] = phaseId,
            ["version"] = AfterTalkResumeVersion
        };
        string str = JsonManager.toJson(dictionary);
        EncryptedPlayerPrefs.SetString(AfterTalkResumeKey, str);
        PlayerPrefs.Save();
    }

    public void SaveTurnData()
    {
        if (this.systemflg_autosave)
        {
            Debug.Log("BattleData::SaveTurnData");
            SaveData data = new SaveData {
                initturn = this.initturn,
                wavecount = this.wavecount,
                turnCount = this.turnCount,
                totalTurnCount = this.totalTurnCount,
                globaltargetId = this.globaltargetId,
                criticalstars = this.criticalstars,
                p_entryid = this.p_entryid,
                e_entryid = this.e_entryid,
                history = this.comhistory.ToArray(),
                shuffleIndex = this.shuffleindex,
                drawcard = this.draw_commandlist,
                shufflecard = this.p_shcmlist,
                voicePlayedList = this.voicePlayedInfo
            };
            if (SingletonMonoBehaviour<BattleRandom>.getInstance() != null)
            {
                data.randLogicCount = SingletonMonoBehaviour<BattleRandom>.getInstance().logicCount;
                data.randGutsCount = SingletonMonoBehaviour<BattleRandom>.getInstance().gutsCount;
            }
            data.master_infoId = new int[this.masterSkillInfo.Count];
            data.master_skillTurn = new int[this.masterSkillInfo.Count];
            for (int i = 0; i < this.masterSkillInfo.Count; i++)
            {
                data.master_infoId[i] = this.masterSkillInfo[i].getInfoId();
                data.master_skillTurn[i] = this.masterSkillInfo[i].chargeTurn;
            }
            data.drop = this.droplist.ToArray();
            EncryptedPlayerPrefs.SetString("Battle", JsonManager.toJson(data));
            foreach (BattleServantData data2 in this.player_datalist)
            {
                EncryptedPlayerPrefs.SetString("p_svlist" + data2.index, data2.getSaveData());
            }
            for (int j = 0; j < this.e_svlist.Length; j++)
            {
                EncryptedPlayerPrefs.SetString("e_svlist" + j, this.e_svlist[j].getSaveData());
            }
            EncryptedPlayerPrefs.SetInt("QUESTID", this.battle_ent.questId);
            EncryptedPlayerPrefs.SetString("Version", "0.6.17.1");
            EncryptedPlayerPrefs.SetLong("SAVEKEY_", this.battle_ent.id);
            PlayerPrefs.Save();
            Debug.Log("::SaveTurnData");
        }
    }

    public void setComboData(BattleComboData incombo)
    {
        this.combodata = incombo;
    }

    public void setCommandAttack(int actId, int targetId)
    {
        this.prevactId = actId;
        this.prevtargetId = targetId;
    }

    public void setCommandData(BattleCommandData[] list)
    {
        this.selectcommandlist = list;
    }

    public static void setContinueBattleFlg(int continueFlg, bool save = true)
    {
        EncryptedPlayerPrefs.SetInt("CONTINUE_BATTLE_SAVEKEY", continueFlg);
        if (save)
        {
            PlayerPrefs.Save();
        }
    }

    public void setFirstBonus(int index, int type)
    {
        CardEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CardMaster>(DataNameKind.Kind.CARD).getEntityFromId<CardEntity>(type, index + 1);
        this.combodata.addAtk += entity.addAtk;
        this.combodata.addCritical += entity.addCritical;
        this.combodata.addTdGauge += entity.addTdGauge;
    }

    public void setInitCommandBattle()
    {
        this.prevactId = 0L;
        this.prevtargetId = 0L;
    }

    public void setLastActionActor(BattleActionData adata)
    {
        this.lastactorId = adata.actorId;
    }

    public void setNextStage()
    {
        this.wavecount++;
        this.totalTurnCount += this.turnCount;
        this.turnCount = 0;
        this.globaltargetId = -1;
    }

    public static void setReserveResumeBattle(bool isResume, int questId = 0, int questPhase = 0)
    {
        EncryptedPlayerPrefs.SetInt("RESERVE_BATTKE_SAVEKEY", !isResume ? 0 : 1);
        if (questId != 0)
        {
            EncryptedPlayerPrefs.SetInt("QUESTID", questId);
        }
        if (questPhase != 0)
        {
            EncryptedPlayerPrefs.SetInt("QUESTPHASEID", questPhase);
        }
        PlayerPrefs.Save();
    }

    public static void setResumeBattleId(long battleId, int questId, int questPhase)
    {
        EncryptedPlayerPrefs.SetLong("SAVEKEY_", battleId);
        EncryptedPlayerPrefs.SetString("Version", "Initial");
        EncryptedPlayerPrefs.SetInt("QUESTID", questId);
        EncryptedPlayerPrefs.SetInt("QUESTPHASEID", questPhase);
        PlayerPrefs.Save();
    }

    public bool setTargetIndex(int index)
    {
        int num = this.e_entryid[index];
        Debug.Log("setTargetIndex:" + num);
        if (num == -1)
        {
            return false;
        }
        this.globaltargetId = num;
        this.perf.setEnemyTarget(this.globaltargetId);
        this.perf.commandPerf.updateCardMag();
        return true;
    }

    public void setTDChain(int addIndex)
    {
        if (0 < addIndex)
        {
            this.combodata.tdChain += addIndex;
        }
        else
        {
            this.combodata.tdChain = addIndex;
        }
    }

    public void shuffleCommand()
    {
        this.p_shcmlist = BattleRandom.getShuffle<BattleCommandData>(this.p_cmlist);
        this.shuffleindex = 0;
    }

    public void toggleHighSpeedMode()
    {
        if (this.systemflg_acceleration != 1)
        {
            this.systemflg_acceleration = 1;
        }
        else
        {
            this.systemflg_acceleration = 2;
        }
        if (!this.isTutorial())
        {
            EncryptedPlayerPrefs.SetInt(speedKey, this.systemflg_acceleration);
        }
    }

    public void toggleSkipSkillConf()
    {
        this.systemflg_skipskillconf = !this.systemflg_skipskillconf;
        EncryptedPlayerPrefs.SetInt(skipKey, !this.systemflg_skipskillconf ? 0 : 1);
    }

    public void toggleTdConstantVelocity()
    {
        this.systemflg_TdConstantvelocity = !this.systemflg_TdConstantvelocity;
        EncryptedPlayerPrefs.SetInt(TdCvKey, !this.systemflg_TdConstantvelocity ? 0 : 1);
    }

    public void transformSvtCommand(BattleServantData svtData)
    {
        foreach (BattleCommandData data in this.p_cmlist)
        {
            if (data.getUniqueId() == svtData.getUniqueID())
            {
                data.svtId = svtData.svtId;
                data.svtlimit = svtData.getCommandDispLimitCount();
                data.loadSvtLimit = svtData.getDispLimitCount();
                data.attri = svtData.getAttri();
            }
        }
        foreach (BattleCommandData data2 in this.p_shcmlist)
        {
            if (data2.getUniqueId() == svtData.getUniqueID())
            {
                data2.svtId = svtData.svtId;
                data2.svtlimit = svtData.getCommandDispLimitCount();
                data2.loadSvtLimit = svtData.getDispLimitCount();
                data2.attri = svtData.getAttri();
            }
        }
    }

    public void turnProgressing()
    {
        foreach (BattleSkillInfoData data in this.masterSkillInfo)
        {
            data.TurnProgress(1, 0);
        }
    }

    public void useMasterSkill(BattleSkillInfoData skillInfo)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
        skillInfo.chargeTurn = entity.chargeTurn;
    }

    public bool AddAttack
    {
        get
        {
            BattleComboData data = this.getSelectCombo();
            return ((data != null) && (3 <= data.samecount));
        }
    }

    public int Questid =>
        this.questphase_ent.questId;

    public int[][] VoicePlayedList
    {
        get
        {
            List<int[]> list = new List<int[]>();
            foreach (KeyValuePair<string, object> pair in this.voicePlayedInfo)
            {
                for (int i = 0; i < 0x20; i++)
                {
                    if ((((long) pair.Value) & (((long) 1L) << i)) != 0)
                    {
                        int[] item = new int[] { int.Parse(pair.Key), i };
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }
    }

    [CompilerGenerated]
    private sealed class <getMasterSkillInfo>c__AnonStorey78
    {
        internal int index;

        internal bool <>m__8F(BattleSkillInfoData s) => 
            ((s.type == BattleSkillInfoData.TYPE.MASTER_EQUIP) && (s.index == this.index));
    }

    public class CommandHistory
    {
        public int ty;
        public int uid;
    }

    public class FinishBattleInfo
    {
        public int questId;
        public int questPhase;
        public int winLoseInfo;
    }

    public class SaveData
    {
        public int criticalstars;
        public BattleCommandData[] drawcard;
        public BattleDropItem[] drop;
        public int[] e_entryid;
        public int globaltargetId;
        public BattleData.CommandHistory[] history;
        public int initturn;
        public int[] master_infoId;
        public int[] master_skillTurn;
        public int[] p_entryid;
        public int randGutsCount;
        public int randLogicCount;
        public BattleCommandData[] shufflecard;
        public int shuffleIndex;
        public int totalTurnCount;
        public int turnCount;
        public Dictionary<string, object> voicePlayedList;
        public int wavecount;
    }

    public enum TYPETURN
    {
        ENEMY = 2,
        PLAYER = 1
    }
}

