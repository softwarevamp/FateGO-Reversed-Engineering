using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class BattleLogic : BaseMonoBehaviour
{
    protected string _CurrentBgmName;
    [CompilerGenerated]
    private static Comparison<BattleSkillInfoData> <>f__am$cache2F;
    [CompilerGenerated]
    private static Comparison<BattleSkillInfoData> <>f__am$cache30;
    private UseSkillObject act_useSkill;
    private CommandSpellData actData;
    private Dictionary<BattleLogicTask.ACTIONTYPE, createActionBattle> actionBattleList;
    private ACTTYPE acttype;
    [SerializeField]
    private Bloom bloom;
    [SerializeField]
    private MotionBlur blur;
    public BattleRandom brandom;
    public bool bugfix_overkill;
    public BattleData data;
    public bool debug_battlewin;
    public bool debug_wavewin;
    private int debugCommandUniqueIndex;
    public DebugPanelRootComponent debugmenu;
    private int debugTargetUniqueId = -1;
    private bool enemyNoblePhantasmMode;
    public PlayMakerFSM fsm;
    private Dictionary<LOGICTYPE, taskFunction> funclist;
    [SerializeField]
    private GrayscaleEffect grayscaleFront;
    [SerializeField]
    private GrayscaleEffect grayscaleMiddle;
    public bool isClientMode;
    private LinkedList<BattleLogicTask> list_logic = new LinkedList<BattleLogicTask>();
    private BattleLogicEnemyAi logicEnemyAi;
    private BattleLogicFunction logicFunction;
    public int logicindex;
    public LOGICTYPE[] logiclist;
    private BattleLogicNomal logicnomal;
    private BattleLogicReaction logicReaction;
    private BattleLogicSkill logicSkill;
    private BattleLogicSpecial logicspecial;
    [HideInInspector]
    private BattleLogicTarget logictarget;
    public BattlePerformance perf;
    private float prevScaleTime = 1f;
    private BattleLogicTask prevTask;
    public BattleLogic proc;
    private string resultstring = string.Empty;
    public string setdays;
    private Spawner spawner;
    private bool systemflg_fixdamage;
    private CommandSpellData tmp_usecommandspell;
    private UseSkillObject tmp_useSkill;
    private TutorialStringData[] TSD = new TutorialStringData[] { new TutorialStringData(0f, new Vector2(0f, 24f), 30), new TutorialStringData(0f, new Vector2(0f, 24f), 0x1c), new TutorialStringData(0f, new Vector2(0f, 8f), 0x1c), new TutorialStringData(0f, new Vector2(0f, 24f), 0x1a), new TutorialStringData(0f, new Vector2(0f, 8f), 0x1a), new TutorialStringData(0f, new Vector2(0f, 24f), 30), new TutorialStringData(0f, new Vector2(0f, 24f), 30), new TutorialStringData(0f, new Vector2(0f, 24f), 0x1a), new TutorialStringData(0f, new Vector2(160f, 80f), 0x1a) };
    public Vector2[] TuArrow = new Vector2[] { new Vector2(400f, -50f), new Vector2(-380f, -154f), new Vector2(-275f, 125f) };
    public Rect[] TuSquare = new Rect[] { new Rect(242.75f, -281.5f, 266.5f, 251f), new Rect(-443f, -232f, 120f, 120f), new Rect(-330f, -70f, 270f, 200f) };
    private static string[] unusedGameObjectNames = new string[] { "/Management/Script", "/Management/CommonUI/UI Root/BgCamera" };
    private List<GameObject> unusedGameobjects = new List<GameObject>(10);
    [SerializeField]
    private Vignetting vignetting;

    public void actBattleTask()
    {
        this.data.clearLastActionActor();
        this.acttype = ACTTYPE.LIST;
        this.checkBattleTask();
    }

    public void actCommandSpell()
    {
        this.actData = this.tmp_usecommandspell;
        this.perf.playActionSkill();
        this.startCommandSpellEffect();
    }

    public void actEnemyPtPassiveSkill()
    {
        List<BattleSkillInfoData> list = new List<BattleSkillInfoData>();
        foreach (BattleServantData data in this.data.getEnemyServantList())
        {
            BattleSkillInfoData[] collection = data.getPassiveSkills();
            list.AddRange(collection);
        }
        if (<>f__am$cache30 == null)
        {
            <>f__am$cache30 = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache30);
        foreach (BattleSkillInfoData data2 in list)
        {
            this.logicSkill.actPassiveSkill(data2);
        }
    }

    public void actOneBattleTask(BattleLogicTask[] tasklist)
    {
        this.acttype = ACTTYPE.ONE;
        for (int i = 0; i < tasklist.Length; i++)
        {
            this.list_logic.AddLast(tasklist[i]);
        }
        if (0 < this.list_logic.Count)
        {
            this.fsm.SendEvent("END_PROC");
        }
        this.playTaskBattle();
    }

    public void actPtPassiveSkill()
    {
        List<BattleSkillInfoData> list = new List<BattleSkillInfoData>();
        foreach (BattleServantData data in this.data.getPlayerServantList())
        {
            BattleSkillInfoData[] collection = data.getPassiveSkills();
            list.AddRange(collection);
        }
        if (<>f__am$cache2F == null)
        {
            <>f__am$cache2F = (a, b) => a.priority - b.priority;
        }
        list.Sort(<>f__am$cache2F);
        foreach (BattleSkillInfoData data2 in list)
        {
            this.logicSkill.actPassiveSkill(data2);
        }
    }

    public void addBattleLogicTask(BattleLogicTask[] tasklist)
    {
        foreach (BattleLogicTask task in tasklist)
        {
            this.list_logic.AddLast(task);
        }
    }

    public void addDropItem(int type)
    {
        if (type == 0)
        {
            DropInfo item = new DropInfo {
                type = 1,
                objectId = 0x4969c,
                num = 1
            };
            this.data.addDropItems(item);
        }
        else if (type == 1)
        {
            DropInfo info2 = new DropInfo {
                type = 2,
                objectId = 1,
                num = 100
            };
            this.data.addDropItems(info2);
        }
    }

    private void BattleLiveCallbackRequest(string result)
    {
        DataManager.actionIndex = 0;
        Debug.Log("callbackRequest:" + result);
        if (result.Equals("OK"))
        {
            this.fsm.SendEvent("OK");
        }
    }

    public void BattleRequest()
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.IsBattleLiveOpen(this.data))
        {
            NetworkManager.getRequest<BattleLiveRequest>(new NetworkManager.ResultCallbackFunc(this.BattleLiveCallbackRequest)).beginRequest((long) this.data.globaltargetId, this.data.selectcommandlist, this.data.usedSkilllist.ToArray(), this.data.errorinfo, this.GetBuffInfo());
        }
        else
        {
            this.fsm.SendEvent("OK");
        }
        this.data.usedSkilllist.Clear();
    }

    public void callBackCommandSpell(string ret)
    {
        if (ret.Equals("ok"))
        {
            this.fsm.SendEvent("OK");
        }
        else if (ret.Equals("ng"))
        {
            this.fsm.SendEvent("NG");
        }
    }

    public void callBackNoEntity(bool flg)
    {
        SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
    }

    private void callbackRequest(string result)
    {
        if (result.Equals("ng"))
        {
            this.fsm.SendEvent("NG");
        }
        else
        {
            this.resultstring = result;
            this.fsm.SendEvent("OK");
        }
    }

    public void callbackTutorialAttack02()
    {
        this.data.getFieldPlayerServantList()[0].addNp(ConstantMaster.getValue("FULL_TD_POINT"), true);
        this.data.tutorialState = 10;
    }

    public void callbackTutorialSelectEnemyNext()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(string.Empty, this.TuArrow[0], this.TuSquare[0], (float) 0f, new Vector2(), -1, null);
    }

    public void checkBattleTask()
    {
        if (this.list_logic.Count <= 0)
        {
            if (this.logicindex < this.logiclist.Length)
            {
                LOGICTYPE key = this.logiclist[this.logicindex];
                BattleLogicTask[] taskArray = null;
                if (this.funclist.ContainsKey(key))
                {
                    taskArray = this.funclist[key](key, this.data);
                }
                if (taskArray != null)
                {
                    for (int i = 0; i < taskArray.Length; i++)
                    {
                        this.list_logic.AddLast(taskArray[i]);
                    }
                }
                this.logicindex++;
                this.checkBattleTask();
            }
            else
            {
                this.fsm.SendEvent("END_TASK");
            }
        }
        else
        {
            this.playTaskBattle();
        }
    }

    public void checkCombo()
    {
        BattleCommandData[] dataArray = this.data.getSelectCommands();
        int[] svtid = new int[3];
        int[] typeid = new int[3];
        for (int i = 0; i < 3; i++)
        {
            svtid[i] = dataArray[i].getUniqueId();
            typeid[i] = dataArray[i].getCommandType();
        }
        BattleComboData incombo = this.checkCommandCombo(svtid, typeid);
        this.data.setComboData(incombo);
    }

    public BattleComboData checkCommandCombo(int[] svtid, int[] typeid)
    {
        BattleComboData data = new BattleComboData();
        for (int i = 0; i < typeid.Length; i++)
        {
            if (BattleCommand.isBLANK(typeid[i]))
            {
                return data;
            }
        }
        if ((typeid[0] == typeid[1]) && (typeid[0] == typeid[2]))
        {
            data.flash = true;
            data.flashtype = typeid[0];
        }
        data.samecount = 1;
        if ((svtid[0] == svtid[1]) && (svtid[0] == svtid[2]))
        {
            data.samecount = 3;
            data.sameflg[0] = true;
            data.sameflg[1] = true;
            data.sameflg[2] = true;
        }
        else if (svtid[0] == svtid[1])
        {
            data.samecount = 2;
            data.sameflg[0] = true;
            data.sameflg[1] = true;
        }
        else if (svtid[1] == svtid[2])
        {
            data.samecount = 2;
            data.sameflg[1] = true;
            data.sameflg[2] = true;
        }
        else if (svtid[0] == svtid[2])
        {
            data.samecount = 2;
            data.sameflg[0] = true;
            data.sameflg[2] = true;
        }
        if (data.flash || (data.samecount == 3))
        {
            for (int j = 0; j < svtid.Length; j++)
            {
                if (!this.data.getServantData(svtid[j]).isAction())
                {
                    return new BattleComboData { chainError = true };
                }
            }
        }
        return data;
    }

    public void checkEndBattle(string endproc)
    {
        this.resetTimeAcceleration();
        this.perf.setTargetChangeActive(true);
        if (SingletonMonoBehaviour<BattleSequenceManager>.Instance.BackToTest)
        {
            this.perf.procEndQuest();
            SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.DebugTest, SceneManager.FadeType.BLACK, null);
        }
        else if (!this.data.checkAlivePlayers())
        {
            this.fsm.SendEvent("EVENT_LOSE");
        }
        else if (!this.data.checkAliveEnemys())
        {
            this.fsm.SendEvent("EVENT_WIN");
        }
        else
        {
            this.fsm.SendEvent(endproc);
        }
    }

    public bool checkEnemyTargetFunction(int[] funclist)
    {
        FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
        foreach (int num in funclist)
        {
            if (Target.isEnemy(master.getEntityFromId<FunctionEntity>(num).targetType))
            {
                return true;
            }
        }
        return false;
    }

    public void checkEntryMember(string endproc)
    {
        base.StartCoroutine(this.coroutineEntrySubMember(endproc));
    }

    public void checkNextBattle(string endproc)
    {
        if (this.debug_battlewin)
        {
            this.fsm.SendEvent("EVENT_ENDBATTLE");
        }
        else
        {
            this.debug_wavewin = false;
            if (this.data.checkEndBattle())
            {
                this.perf.effectNextBattle();
                this.fsm.SendEvent("EVENT_NEXTBATTLE");
            }
            else
            {
                this.fsm.SendEvent("EVENT_ENDBATTLE");
            }
        }
    }

    public bool checkPtTargetFunction(int[] funclist)
    {
        FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
        foreach (int num in funclist)
        {
            if (Target.isPlayer(master.getEntityFromId<FunctionEntity>(num).targetType))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkRelationTask(BattleLogicTask task)
    {
        bool flag = false;
        Stack<BattleLogicTask> stack = new Stack<BattleLogicTask>();
        BattleLogicTask[] taskArray = null;
        taskArray = this.logicReaction.checkResurrection();
        if (0 < taskArray.Length)
        {
            foreach (BattleLogicTask task2 in taskArray)
            {
                stack.Push(task2);
            }
        }
        taskArray = this.logicReaction.checkDead();
        if (0 < taskArray.Length)
        {
            foreach (BattleLogicTask task3 in taskArray)
            {
                stack.Push(task3);
            }
        }
        if (!task.checkActorId(this.prevTask))
        {
            if (this.prevTask != null)
            {
                this.perf.playBackStepMotion(this.prevTask.getActorId());
            }
            this.perf.hideOverKillMessage();
        }
        else if (!task.checkTargetId(this.prevTask))
        {
            this.perf.addActionData(this.logicnomal.createBackStep(this.prevTask.getActorId()));
        }
        if (task.isCommandAction() || task.isAddAttack())
        {
            this.prevTask = task;
        }
        else
        {
            this.prevTask = null;
        }
        if (stack.Count <= 0)
        {
            BattleLogicTask[] taskArray4 = this.taskReaction(task);
            if ((taskArray4 != null) && (0 < taskArray4.Length))
            {
                foreach (BattleLogicTask task4 in taskArray4)
                {
                    stack.Push(task4);
                }
            }
        }
        if (0 >= stack.Count)
        {
            return flag;
        }
        while (0 < stack.Count)
        {
            this.list_logic.AddFirst(stack.Pop());
        }
        return true;
    }

    public void checkSelectEnemyClick(int index)
    {
        if ((((this.data.tutorialId == 3) && (this.data.wavecount == 1)) && ((this.data.turnCount == 1) && (this.data.tutorialState == -1))) && (index == 1))
        {
            this.data.tutorialState = 1;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(new System.Action(this.callbackTutorialSelectEnemyNext));
        }
    }

    public bool checkSelectFunctionTarget(int[] funclist)
    {
        FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
        foreach (int num in funclist)
        {
            if (Target.isChoose(master.getEntityFromId<FunctionEntity>(num).targetType))
            {
                return true;
            }
        }
        return false;
    }

    public bool checkSelectTargetFunction(int[] funclist, out bool mainFlg, out bool subFlg)
    {
        mainFlg = false;
        subFlg = false;
        FunctionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionMaster>(DataNameKind.Kind.FUNCTION);
        foreach (int num in funclist)
        {
            FunctionEntity entity = master.getEntityFromId<FunctionEntity>(num);
            if (Target.isChoose(entity.targetType))
            {
                mainFlg = true;
            }
            else if (Target.isSubChoose(entity.targetType))
            {
                subFlg = true;
            }
        }
        return (mainFlg | subFlg);
    }

    public void checkSkillTarget()
    {
        this.act_useSkill = this.tmp_useSkill;
        this.fsm.SendEvent("END_PROC");
    }

    public void checkTurnStart()
    {
        if (this.data.tutorialId == -1)
        {
            this.sendFsmEvent("END_PROC");
        }
        else
        {
            if (this.data.tutorialId == 1)
            {
                if (this.data.turnCount == 1)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_110"), this.TuArrow[0], this.TuSquare[0], this.TSD[0].way, this.TSD[0].pos, this.TSD[0].size, null);
                }
                else if (this.data.turnCount == 2)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_121"), this.TuArrow[0], this.TuSquare[0], this.TSD[1].way, this.TSD[1].pos, this.TSD[1].size, null);
                }
                else if (this.data.turnCount == 3)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_130"), this.TuArrow[0], this.TuSquare[0], this.TSD[2].way, this.TSD[2].pos, this.TSD[2].size, null);
                }
                else if (this.data.turnCount == 4)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_140"), this.TuArrow[0], this.TuSquare[0], this.TSD[3].way, this.TSD[3].pos, this.TSD[3].size, null);
                }
                else if (this.data.turnCount == 5)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_150"), this.TuArrow[0], this.TuSquare[0], this.TSD[4].way, this.TSD[4].pos, this.TSD[4].size, new System.Action(this.callbackTutorialAttack02));
                }
            }
            else if (this.data.tutorialId == 2)
            {
                if (this.data.turnCount == 1)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_210"), this.TuArrow[0], this.TuSquare[0], this.TSD[5].way, this.TSD[5].pos, this.TSD[5].size, null);
                }
                else if ((this.data.turnCount == 2) && (this.data.tutorialState == -1))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_220"), this.TuArrow[1], this.TuSquare[1], this.TSD[6].way, this.TSD[6].pos, this.TSD[6].size, null);
                }
                else if ((this.data.turnCount == 2) && (this.data.tutorialState == 10))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_222"), this.TuArrow[0], this.TuSquare[0], this.TSD[7].way, this.TSD[7].pos, this.TSD[7].size, null);
                }
                else
                {
                    this.sendFsmEvent("END_PROC");
                }
            }
            else if (this.data.tutorialId == 3)
            {
                if ((this.data.wavecount == 1) && (this.data.turnCount == 1))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_320"), this.TuArrow[2], this.TuSquare[2], this.TSD[8].way, this.TSD[8].pos, this.TSD[8].size, null);
                }
                else
                {
                    this.sendFsmEvent("END_PROC");
                }
            }
            else
            {
                this.sendFsmEvent("END_PROC");
            }
            this.sendFsmEvent("OK");
        }
    }

    public void checkUsedBuff()
    {
        foreach (BattleServantData data in this.data.getFieldServantList())
        {
            data.buffData.usedProgressing();
        }
    }

    public void checkUsedGutsBuff(int targetId)
    {
        BattleServantData data = this.data.getServantData(targetId);
        if (data != null)
        {
            data.buffData.usedProgressingGuts();
        }
    }

    [DebuggerHidden]
    private IEnumerator checkWaveFunc() => 
        new <checkWaveFunc>c__Iterator1E { <>f__this = this };

    [DebuggerHidden]
    private IEnumerator colReplaceMember(BattleActionData.ReplaceMember replaceData, System.Action endCallBack) => 
        new <colReplaceMember>c__Iterator1C { 
            replaceData = replaceData,
            endCallBack = endCallBack,
            <$>replaceData = replaceData,
            <$>endCallBack = endCallBack,
            <>f__this = this
        };

    private void connectBattleResult()
    {
        BattleEntity entity = this.data.getBattleEntity();
        if (this.data.win_lose == 1)
        {
            BattleData.SaveBattleAfterTalkResumeInfo(entity.questId, entity.questPhase);
        }
        NetworkManager.getRequest<BattleResultRequest>(new NetworkManager.ResultCallbackFunc(this.callbackRequest)).beginRequest(entity.id, this.data.win_lose, string.Empty, this.data.getCommandHistory(), this.data.VoicePlayedList, 1);
    }

    public void connectCommandSpell()
    {
        NetworkManager.getRequest<BattleCommandSpellRequest>(new NetworkManager.ResultCallbackFunc(this.callBackCommandSpell)).beginRequest(this.data.getBattleEntity().id, this.actData.commandSkillId, false);
    }

    public void connectResultErrorDialog()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_DIALOG_RESULT_ERROR_TITLE"), LocalizationManager.Get("BATTLE_DIALOG_RESULT_ERROR_CONF"), new NotificationDialog.ClickDelegate(this.requestResultErrorDialog), -1);
    }

    [DebuggerHidden]
    private IEnumerator coroutineContinueMember(string endproc) => 
        new <coroutineContinueMember>c__Iterator1D { 
            endproc = endproc,
            <$>endproc = endproc,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator coroutineEntrySubMember(string endproc) => 
        new <coroutineEntrySubMember>c__Iterator1B { 
            endproc = endproc,
            <$>endproc = endproc,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator coroutineInitQuest(string endproc) => 
        new <coroutineInitQuest>c__Iterator20 { 
            endproc = endproc,
            <$>endproc = endproc,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator coroutineLoadNstage(string endproc) => 
        new <coroutineLoadNstage>c__Iterator22 { 
            endproc = endproc,
            <$>endproc = endproc,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator coroutineLoadQuest() => 
        new <coroutineLoadQuest>c__Iterator21 { <>f__this = this };

    [DebuggerHidden]
    private IEnumerator coroutineLoadSaveWave() => 
        new <coroutineLoadSaveWave>c__Iterator23 { <>f__this = this };

    public BattleActionData createLastActorBackStep(BattleLogicTask task)
    {
        int uniqueId = this.data.getLastActionActor();
        BattleServantData data = this.data.getServantData(uniqueId);
        if ((data != null) && data.isAlive())
        {
            return this.logicnomal.createBackStep(uniqueId);
        }
        return null;
    }

    public BattleActionData createSystem(BattleLogicTask task) => 
        new BattleActionData { 
            state = 1,
            systemTime = task.systemTime
        };

    public void debugAct()
    {
        foreach (BattleServantData data in this.data.getFieldEnemyServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = 0x7a,
                turn = 4,
                count = -1,
                param = 500
            };
            data.addBuff(buff, false);
        }
    }

    public void debugAddBuff(int buffId)
    {
        BattleServantData[] dataArray = null;
        if (buffId == 0)
        {
            BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
            dataArray = this.data.getFieldPlayerServantList();
            for (int i = 100; i < 190; i++)
            {
                int num2 = i;
                long[] args = new long[] { (long) num2 };
                if (master.isEntityExistsFromId(args))
                {
                    foreach (BattleServantData data in dataArray)
                    {
                        BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                            buffId = num2,
                            turn = 4,
                            count = -1,
                            param = 100
                        };
                        data.addBuff(buff, false);
                    }
                }
            }
        }
        else
        {
            foreach (BattleServantData data3 in this.data.getFieldPlayerServantList())
            {
                BattleBuffData.BuffData data4 = new BattleBuffData.BuffData {
                    buffId = buffId,
                    turn = 4,
                    count = -1,
                    param = 0x3e8
                };
                data3.addBuff(data4, false);
            }
            foreach (BattleServantData data5 in this.data.getFieldEnemyServantList())
            {
                BattleBuffData.BuffData data6 = new BattleBuffData.BuffData {
                    buffId = buffId,
                    turn = 4,
                    count = -1,
                    param = 0x3e8
                };
                data5.addBuff(data6, false);
            }
        }
    }

    public void debugAddBuffCommandCard(int type)
    {
        int num = 0;
        if (BattleCommand.isARTS(type))
        {
            num = 0x65;
        }
        else if (BattleCommand.isBUSTER(type))
        {
            num = 0x66;
        }
        else if (BattleCommand.isQUICK(type))
        {
            num = 100;
        }
        foreach (BattleServantData data in this.data.getFieldPlayerServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = num,
                turn = 2,
                count = -1,
                param = 0x1388
            };
            data.addBuff(buff, false);
        }
    }

    public void debugAddFullBuff(int type)
    {
        BattleServantData[] dataArray = null;
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        if (type == 0)
        {
            dataArray = this.data.getFieldPlayerServantList();
        }
        else
        {
            dataArray = this.data.getFieldEnemyServantList();
        }
        for (int i = 100; i < 190; i++)
        {
            int id = i;
            long[] args = new long[] { (long) id };
            if (master.isEntityExistsFromId(args) && (master.getEntityFromId<BuffEntity>(id).type != 0x1a))
            {
                foreach (BattleServantData data in dataArray)
                {
                    BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                        buffId = id,
                        turn = 4,
                        count = -1,
                        param = 100
                    };
                    data.addBuff(buff, false);
                }
            }
        }
    }

    public void debugAddFullBuffNoAct(int type)
    {
        BattleServantData[] dataArray = null;
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        if (type == 0)
        {
            dataArray = this.data.getFieldPlayerServantList();
        }
        else
        {
            dataArray = this.data.getFieldEnemyServantList();
        }
        for (int i = 100; i < 190; i++)
        {
            int id = i;
            long[] args = new long[] { (long) id };
            if (master.isEntityExistsFromId(args))
            {
                BuffEntity entity = master.getEntityFromId<BuffEntity>(id);
                if ((entity.type != 0x20) && (entity.type != 0x1a))
                {
                    foreach (BattleServantData data in dataArray)
                    {
                        BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                            buffId = id,
                            turn = 4,
                            count = -1,
                            param = 100
                        };
                        data.addBuff(buff, false);
                    }
                }
            }
        }
    }

    public void debugAddOneBuff(int buffId)
    {
        foreach (BattleServantData data in this.data.getFieldPlayerServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = buffId,
                turn = 2,
                count = -1,
                param = 500
            };
            data.addBuff(buff, false);
            break;
        }
    }

    public void debugAutoSave(bool flg)
    {
        this.data.systemflg_autosave = flg;
        if (flg)
        {
            this.data.SaveTurnData();
        }
        else
        {
            BattleData.deleteSaveData();
        }
    }

    public void debugBattleEnd(int type)
    {
        this.debug_battlewin = true;
        foreach (BattleServantData data in this.data.getEnemyServantList())
        {
            data.buffData.Initialize();
            data.isEntry = true;
            data.hp = 0;
        }
        this.endSelectSkillFaze();
    }

    public void debugChangeAcceleration(int param)
    {
        this.data.systemflg_acceleration = param;
    }

    public void debugChangeAi(int type)
    {
        int groupId = 10;
        int num2 = 0x3ea;
        if (type == 1)
        {
            groupId = 11;
            num2 = 0x3eb;
        }
        else if (type == 2)
        {
            groupId = 12;
            num2 = 0x3ec;
        }
        AiMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<AiMaster>(DataNameKind.Kind.AI);
        long[] args = new long[] { (long) groupId };
        if (!master.isEntityExistsFromId(args))
        {
            AiEntity aiEnt = new AiEntity {
                id = groupId,
                idx = 1,
                actNum = -1,
                priority = 1,
                probability = 100,
                cond = 0,
                vals = new int[0],
                aiActId = num2,
                avals = new int[2],
                infoText = string.Empty
            };
            master.debugUpdate(aiEnt);
        }
        BattleServantData data = this.data.getServantData(this.debugTargetUniqueId);
        if (data != null)
        {
            data.getAiState().Initialize(groupId);
        }
    }

    public void debugChangeAmbient(int index)
    {
        this.perf.setAmbientColor(index);
    }

    public void debugChangeAutoBattle(bool flg)
    {
        this.data.systemflg_showautobutton = flg;
    }

    public void debugChangeBg(string bgCombinedId)
    {
        int num2;
        char[] separator = new char[] { ':' };
        string[] strArray = bgCombinedId.Split(separator);
        int no = int.Parse(strArray[0]);
        if (!int.TryParse(strArray[1], out num2))
        {
            num2 = 0;
        }
        bool parentCamera = (no >= 0x15f90) && (no != 0xf423f);
        this.perf.changeBg(no, num2, Vector3.zero, Vector3.zero, false, parentCamera, null);
    }

    public void debugChangeBloom(bool flg)
    {
        this.bloom.enabled = flg;
    }

    public void debugChangeBlur(bool flg)
    {
        this.blur.enabled = flg;
    }

    private void debugChangeEnemyNoblePhantasm(bool flg)
    {
        for (int i = 0; i < this.logiclist.Length; i++)
        {
            if (this.logiclist[i] == LOGICTYPE.ENEMY_ATTACK_1)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_SP1_ALWAYS;
            }
            else if (this.logiclist[i] == LOGICTYPE.ENEMY_SP1_ALWAYS)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_ATTACK_1;
            }
            if (this.logiclist[i] == LOGICTYPE.ENEMY_ATTACK_2)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_SP2_ALWAYS;
            }
            else if (this.logiclist[i] == LOGICTYPE.ENEMY_SP2_ALWAYS)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_ATTACK_2;
            }
            if (this.logiclist[i] == LOGICTYPE.ENEMY_ATTACK_3)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_SP3_ALWAYS;
            }
            else if (this.logiclist[i] == LOGICTYPE.ENEMY_SP3_ALWAYS)
            {
                this.logiclist[i] = LOGICTYPE.ENEMY_ATTACK_3;
            }
        }
        this.enemyNoblePhantasmMode = !flg;
    }

    public void debugChangeFieldEffect(int type)
    {
        this.perf.setFieldEffect(type);
    }

    public void debugChangeFog(bool flg)
    {
        RenderSettings.fog = flg;
    }

    public void debugChangeGrayScale(bool flg)
    {
        this.grayscaleMiddle.enabled = flg;
        this.grayscaleFront.enabled = flg;
    }

    public void debugChangeMem(bool flg)
    {
        AllocMem component = GameObject.Find("Management").GetComponent<AllocMem>();
        component.enabled = flg;
        component.show = flg;
    }

    public void debugChangeSlow(int param)
    {
        Time.timeScale = 1f / ((float) param);
    }

    public void debugChangeVignetting(bool flg)
    {
        this.vignetting.enabled = flg;
    }

    public void debugCommandChange(int type)
    {
        foreach (BattleCommandData data in this.data.draw_commandlist)
        {
            data.type = type;
        }
        this.perf.updateCommandCard();
        this.perf.registCommandCard(this.data.draw_commandlist);
        this.perf.commandPerf.openCommandCard();
    }

    public void debugDrawCommandCard(int uniqueId)
    {
        BattleServantData data = null;
        BattleServantData[] dataArray = this.data.getFieldPlayerServantList();
        if (this.debugCommandUniqueIndex < dataArray.Length)
        {
            data = dataArray[this.debugCommandUniqueIndex];
            this.debugCommandUniqueIndex++;
        }
        else
        {
            data = dataArray[0];
            this.debugCommandUniqueIndex = 1;
        }
        List<BattleCommandData> list = new List<BattleCommandData>();
        foreach (int num in data.getCommandList())
        {
            BattleCommandData item = new BattleCommandData {
                type = num,
                svtId = data.svtId,
                svtlimit = data.getCommandDispLimitCount(),
                uniqueId = data.getUniqueID(),
                attri = data.getAttri(),
                flgEventJoin = data.flgEventJoin
            };
            item.setFollowerType(data.followerType);
            list.Add(item);
        }
        this.data.draw_commandlist = list.ToArray();
        this.perf.updateCommandCard();
        this.perf.registCommandCard(this.data.draw_commandlist);
        this.perf.commandPerf.openCommandCard();
    }

    public void debugEnemyAddBuff(int buffId)
    {
        foreach (BattleServantData data in this.data.getFieldEnemyServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = buffId,
                turn = 2,
                count = -1,
                param = 250
            };
            data.addBuff(buff, false);
            break;
        }
    }

    public void debugEnemyAddCountBuff(int buffId)
    {
        foreach (BattleServantData data in this.data.getFieldEnemyServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = buffId,
                turn = -1,
                count = 1,
                param = 100
            };
            data.addBuff(buff, false);
        }
    }

    public void debugFixDamage(bool flg)
    {
        this.systemflg_fixdamage = flg;
    }

    public void debugFixOverKill(bool flg)
    {
        this.bugfix_overkill = flg;
    }

    public void debugPlayerAddBuff(int buffId)
    {
        foreach (BattleServantData data in this.data.getPlayerServantList())
        {
            BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
                buffId = buffId,
                turn = -1,
                count = -1,
                param = 1
            };
            buff.vals = new int[] { 0x3e8, 1 };
            data.addBuff(buff, false);
        }
        buffId = 0x7b;
        foreach (BattleServantData data3 in this.data.getEnemyServantList())
        {
            BattleBuffData.BuffData data4 = new BattleBuffData.BuffData {
                buffId = buffId,
                turn = -1,
                count = -1,
                param = 0xc350
            };
            data4.vals = new int[] { 0x3e8, 1 };
            data3.addBuff(data4, false);
        }
    }

    public void debugProc(int intype)
    {
        DebugType type = (DebugType) intype;
        switch (type)
        {
            case DebugType.ENEMY_DYING:
            case DebugType.PLAYER_DYING:
            {
                BattleServantData[] dataArray = (type != DebugType.ENEMY_DYING) ? this.data.getFieldPlayerServantList() : this.data.getFieldEnemyServantList();
                foreach (BattleServantData data in dataArray)
                {
                    data.addDamage(data.hp - 1);
                }
                break;
            }
            case DebugType.ENEMY_RECOVER:
            case DebugType.PLAYER_RECOVER:
            {
                BattleServantData[] dataArray3 = (type != DebugType.ENEMY_RECOVER) ? this.data.getFieldPlayerServantList() : this.data.getFieldEnemyServantList();
                foreach (BattleServantData data2 in dataArray3)
                {
                    data2.healHp(data2.getMaxHp());
                }
                break;
            }
            case DebugType.PLAYER_RECOVER_ONE:
                foreach (BattleServantData data3 in this.data.getFieldPlayerServantList())
                {
                    data3.healHp(data3.getMaxHp());
                    break;
                }
                break;

            case DebugType.PLAYER_NPFULL:
                foreach (BattleServantData data4 in this.data.getFieldPlayerServantList())
                {
                    data4.addNp(ConstantMaster.getValue("FULL_TD_POINT"), true);
                }
                break;

            case DebugType.CRITICAL_FULL:
                this.perf.addCriticalPont(12);
                break;

            case DebugType.ENEMY_NPFULL:
                foreach (BattleServantData data5 in this.data.getFieldEnemyServantList())
                {
                    if (data5.hasTreasureDvc())
                    {
                        data5.nexttpturn = 0;
                    }
                    data5.updateTDGauge();
                }
                break;

            case DebugType.ENEMY_NPHASTE:
                foreach (BattleServantData data6 in this.data.getFieldEnemyServantList())
                {
                    if (data6.hasTreasureDvc())
                    {
                        data6.nexttpturn--;
                        if (data6.nexttpturn < 0)
                        {
                            data6.nexttpturn = 0;
                        }
                    }
                    data6.updateTDGauge();
                }
                break;
        }
    }

    public void debugShortenSkill()
    {
        foreach (BattleServantData data in this.data.getFieldPlayerServantList())
        {
            foreach (BattleSkillInfoData data2 in data.getActiveSkillInfos())
            {
                if (0 < data2.chargeTurn)
                {
                    data2.chargeTurn = 0;
                }
            }
        }
        foreach (BattleSkillInfoData data3 in this.data.getMasterSkillInfos())
        {
            if (0 < data3.chargeTurn)
            {
                data3.chargeTurn = 0;
            }
        }
    }

    public void debugShowState(int type)
    {
        StringBuilder builder = new StringBuilder();
        BattleActorControl component = null;
        if (type == 0)
        {
            builder.AppendLine($"BattleFsm
[{this.fsm.ActiveStateName}]");
            builder.AppendLine($"Field
[{this.perf.fieldmotionfsm.ActiveStateName}]");
            builder.AppendLine($"Camera
[{this.perf.camerafsm.ActiveStateName}]");
            foreach (GameObject obj2 in this.perf.PlayerActorList)
            {
                if (obj2 != null)
                {
                    component = obj2.GetComponent<BattleActorControl>();
                    builder.AppendLine($"
UniId:{component.uniqueID}
WeaponG:{component.getWeaponGroup()}
> weapon:{component.motionFSM[0].ActiveStateName}
> common:{component.motionFSM[1].ActiveStateName}");
                }
            }
            foreach (GameObject obj3 in this.perf.EnemyActorList)
            {
                if (obj3 != null)
                {
                    component = obj3.GetComponent<BattleActorControl>();
                    builder.AppendLine($"
UniId{component.uniqueID}
UniId{component.getWeaponGroup()}
> weapon[{component.motionFSM[0].ActiveStateName}]
> common[{component.motionFSM[1].ActiveStateName}]");
                }
            }
        }
        this.debugmenu.setLog(builder.ToString());
    }

    public void debugShowSvtState(int type)
    {
        BattleActorControl control = null;
        List<BattleActorControl> list = new List<BattleActorControl>();
        foreach (GameObject obj2 in this.perf.PlayerActorList)
        {
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    list.Add(component);
                }
            }
        }
        foreach (GameObject obj3 in this.perf.EnemyActorList)
        {
            if (obj3 != null)
            {
                BattleActorControl item = obj3.GetComponent<BattleActorControl>();
                if (item != null)
                {
                    list.Add(item);
                }
            }
        }
        bool flag = false;
        foreach (BattleActorControl control4 in list)
        {
            if (flag)
            {
                control = control4;
                break;
            }
            if (control4.uniqueID == this.debugTargetUniqueId)
            {
                flag = true;
            }
        }
        if ((control == null) && (list != null))
        {
            control = list[0];
        }
        if (control != null)
        {
            this.debugmenu.setLog(control.getlog());
            this.debugTargetUniqueId = control.uniqueID;
        }
    }

    [DebuggerHidden]
    private IEnumerator debugStartTips() => 
        new <debugStartTips>c__Iterator1F();

    public void debugTips(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_TIP);
        base.StartCoroutine(this.debugStartTips());
    }

    public void debugTutorialArrowMessage(bool flg)
    {
        if (flg)
        {
            Vector2[] posList = new Vector2[] { new Vector2(-167f, 165f), new Vector2(-200f, -80f), new Vector2(-400f, -80f) };
            Rect[] rects = new Rect[] { new Rect(-270f, -30f, 210f, 250f), new Rect(-500f, -230f, 400f, 250f) };
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow("TEST\nメッセージと矢印を同時に出す", posList, rects, (float) 0f, new Vector2(200f, 100f), 0x17, null);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
        }
    }

    public void debugWaveEnd(int type)
    {
        this.debug_wavewin = true;
        foreach (BattleServantData data in this.data.getEnemyServantList())
        {
            data.buffData.Initialize();
            data.isEntry = true;
            data.hp = 0;
        }
        this.endSelectSkillFaze();
    }

    public void drawCommand()
    {
        List<BattleCommandData> list = new List<BattleCommandData>();
        BattleCommandData item = null;
        if (this.data.isTutorialCard())
        {
            list.AddRange(this.getTutorialCommandCards());
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                item = this.data.getShuffleCommand();
                list.Add(item);
            }
        }
        this.data.draw_commandlist = list.ToArray();
    }

    public void endActionData()
    {
        if (this.acttype == ACTTYPE.LIST)
        {
            this.checkBattleTask();
        }
        else if (this.acttype == ACTTYPE.ONE)
        {
            if (0 < this.list_logic.Count)
            {
                this.playTaskBattle();
            }
            else
            {
                this.fsm.SendEvent("END_PROC");
            }
        }
    }

    public void endCommandSpell()
    {
        this.perf.updateCommandCard();
        this.perf.checkRedrawCommandCard();
        this.resetTimeAcceleration();
        this.perf.setTargetChangeActive(true);
        this.data.SaveTurnData();
        int spellImageId = UserGameMaster.getSelfUserGame().SpellImageId;
        AssetManager.releaseAssetStorage($"CommandSpellEffect/ef_commandspell{spellImageId:D4}");
        this.perf.endCommandSpell();
        this.fsm.SendEvent("END_PROC");
    }

    protected void endLoadCommandSPell(AssetData data)
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        int spellImageId = entity.SpellImageId;
        int num2 = entity.getCommandSpell();
        GameObject prefab = data.GetObject<GameObject>($"ef_commandspell{spellImageId:D4}");
        Animation component = base.createObject(prefab, this.perf.popupTr, null).GetComponent<Animation>();
        string animation = $"ef_commandspell_{num2:D2}";
        component.Play(animation);
        this.perf.playMasterCommandSpellCutIn();
        this.setTimeAcceleration();
        this.perf.setTargetChangeActive(false);
        int[] ptTargetList = new int[] { this.actData.ptTargetId };
        BattleLogicTask[] tasklist = this.logicSkill.taskCommandSpell(this.actData.commandSkillId, 1, ptTargetList);
        this.actOneBattleTask(tasklist);
    }

    public void endSelectCommand()
    {
        this.checkCombo();
        this.data.addLogCommand();
        this.fsm.SendEvent("END_SELECTCARD");
    }

    public void endSelectSkillFaze()
    {
        if (((((this.data.tutorialId != 1) || (this.data.turnCount != 5)) || (this.data.tutorialState != -1)) && (((this.data.tutorialId != 2) || (this.data.turnCount != 2)) || (this.data.tutorialState != -1))) && (((this.data.tutorialId != 3) || (this.data.wavecount != 1)) || ((this.data.turnCount != 1) || (this.data.tutorialState != -1))))
        {
            if (SingletonMonoBehaviour<BattleSequenceManager>.Instance.testMode)
            {
                this.fsm.SendEvent("PLAY_TREASUREARMS");
            }
            else
            {
                this.fsm.SendEvent("END_ABILITYFAZE");
            }
        }
    }

    public void endSkill()
    {
        this.perf.updateCommandCard();
        this.perf.checkRedrawCommandCard();
        this.perf.endActionSkill();
        this.resetTimeAcceleration();
        this.perf.setTargetChangeActive(true);
        this.data.SaveTurnData();
        if ((this.data.tutorialId == 2) && (this.data.tutorialState == 5))
        {
            this.data.tutorialState = 10;
            this.fsm.SendEvent("TUTORIAL");
        }
        else
        {
            this.fsm.SendEvent("END_PROC");
        }
    }

    public void endStartWaveAction()
    {
        this.resetTimeAcceleration();
    }

    public void finishActionBattle(string endproc)
    {
        for (int i = 0; i < this.data.p_entryid.Length; i++)
        {
            if (0 < this.data.p_entryid[i])
            {
                this.data.getPlayerServantData(this.data.p_entryid[i]).refreshActionBattle();
            }
        }
        this.perf.updateStatus();
        this.fsm.SendEvent(endproc);
    }

    public int getAttackNp(BattleServantData actor, BattleServantData target, BattleCommandData command, bool critical, bool isNoble)
    {
        float f = 0f;
        f = actor.getAttackBaseNp(command, isNoble);
        f *= actor.getCommandCardNP(command, target);
        f *= target.getTdRate();
        f *= actor.getUpDownDropNp(target);
        if (critical)
        {
            f *= ConstantMaster.getRateValue("CRITICAL_TD_POINT_RATE");
        }
        if (f < 0f)
        {
            f = 0f;
        }
        return Mathf.FloorToInt(f);
    }

    public int getAttackStart(BattleServantData actor, BattleServantData target, BattleCommandData command, bool critical)
    {
        float num = 0f;
        num += ((float) actor.getBaseStarRate()) / 1000f;
        num += actor.getCommandStar(command, target);
        num += ((float) target.getDownBaseStarRate()) / 1000f;
        float num2 = actor.getUpdownDropStar(command);
        num += num2;
        float num3 = target.getUpdownDropStar(command);
        num -= num3;
        if (critical)
        {
            num += ((float) ConstantMaster.getValue("CRITICAL_STAR_RATE")) / 1000f;
        }
        if (num < 0f)
        {
            num = 0f;
        }
        return Mathf.FloorToInt(num * 1000f);
    }

    public string getBattleBgmName()
    {
        BattleEntity entity = this.data.getBattleEntity();
        int questId = entity.questId;
        int questPhase = entity.questPhase;
        int wavecount = this.data.wavecount;
        StageEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STAGE).getEntityFromId<StageEntity>(questId, questPhase, wavecount + 1);
        if (entity2 != null)
        {
            int bgmId = entity2.bgmId;
            BgmEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BGM).getEntityFromId<BgmEntity>(bgmId);
            if (entity3 != null)
            {
                return entity3.fileName;
            }
            Debug.LogWarning("Can not found bgm:" + bgmId);
        }
        return "BGM_BATTLE_1";
    }

    private string GetBuffInfo()
    {
        List<BuffInfoData> list = new List<BuffInfoData>();
        foreach (BattleServantData data in this.data.player_datalist)
        {
            BuffInfoData item = new BuffInfoData {
                svtId = data.svtId,
                uniqueId = data.uniqueId
            };
            List<int> svtBuffList = new List<int>();
            svtBuffList = this.GetSvtBuffList(data.buffData.getActiveList());
            if ((svtBuffList != null) && (svtBuffList.Count != 0))
            {
                item.buffList = JsonManager.toJson(this.GetSvtBuffList(data.buffData.getActiveList()).ToArray());
                list.Add(item);
            }
        }
        return JsonManager.toJson(list.ToArray());
    }

    public BattleCommandData getCommandData(BattleServantData svtData, BattleCommand.TYPE type)
    {
        BattleCommandData data = new BattleCommandData {
            type = (int) type,
            svtId = svtData.svtId,
            svtlimit = svtData.getCommandDispLimitCount(),
            loadSvtLimit = svtData.getDispLimitCount(),
            uniqueId = svtData.getUniqueID(),
            attri = svtData.getAttri()
        };
        data.setFollowerType(svtData.followerType);
        return data;
    }

    public BattleActionData.DamageData getDamagelist(BattleServantData actor, BattleServantData target, BattleCommandData command)
    {
        int[] perlist = new int[] { 0x3e8 };
        return this.getDamagelist(actor, target, command, DamageType.NOMAL, 0, perlist, null, null);
    }

    public BattleActionData.DamageData getDamagelist(BattleServantData actor, BattleServantData target, BattleCommandData command, DamageType nobleType, int funcIndex, int[] perlist, int[] svtIndv, int[] buffIndv)
    {
        BattleActionData.DamageData data = new BattleActionData.DamageData();
        List<int> list = new List<int>();
        bool critical = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        actor.recordUse();
        target.recordUse();
        int[] numArray = actor.getAttackRaito(command);
        if (nobleType != DamageType.NOMAL)
        {
            numArray = actor.getTreasureDvcHitRaito();
        }
        int num = 0;
        foreach (int num2 in numArray)
        {
            num += num2;
        }
        float f = 0f;
        bool flag6 = false;
        f = actor.getBaseATK();
        float num5 = this.getPerlist(perlist, 0);
        if (nobleType == DamageType.NOBLE_HPRATIO_HIGH)
        {
            num5 += this.getPerlist(perlist, 1) * (((float) actor.getNowHp()) / ((float) actor.getMaxHp()));
        }
        else if (nobleType == DamageType.NOBLE_HPRATIO_LOW)
        {
            num5 += this.getPerlist(perlist, 1) * (1f - (((float) actor.getNowHp()) / ((float) actor.getMaxHp())));
        }
        f *= num5 / 1000f;
        f *= actor.getCommandCardATK(command, target);
        f *= actor.getClassAtk();
        float num6 = SvtClassAttri.getMagnification(actor.getClassId(), target.getClassId());
        if (1f < num6)
        {
            flag2 = true;
        }
        else if (num6 < 1f)
        {
            flag3 = true;
        }
        f *= num6;
        f *= SvtAttri.getMagnification(actor.getAttri(), target.getAttri());
        float num7 = ((float) BattleRandom.getRandom(ConstantMaster.getValue("ATTACK_RATE_RANDOM_MIN"), ConstantMaster.getValue("ATTACK_RATE_RANDOM_MAX"))) / 1000f;
        if (this.systemflg_fixdamage)
        {
            num7 = 1f;
        }
        f *= num7;
        f *= ((float) ConstantMaster.getValue("ATTACK_RATE")) / 1000f;
        float num8 = 1f;
        float num9 = (1f + actor.getUpDownAtk(target)) - target.getUpDownDef(actor, nobleType == DamageType.NOBLE_PIERCE);
        if (num9 < 0f)
        {
            num9 = 0f;
        }
        num8 *= num9;
        if (command.isCritical())
        {
            critical = true;
            float num10 = ((float) ConstantMaster.getValue("CRITICAL_ATTACK_RATE")) / 1000f;
            num8 *= num10;
        }
        if (command.isAddAttack() && command.isSingle())
        {
            num8 *= ((float) ConstantMaster.getValue("EXTRA_ATTACK_RATE_SINGLE")) / 1000f;
        }
        if (command.isAddAttack() && command.isGrand())
        {
            num8 *= ((float) ConstantMaster.getValue("EXTRA_ATTACK_RATE_GRAND")) / 1000f;
        }
        float num11 = 1f - target.getUpDownSpecialDef(actor);
        if (num11 < 0f)
        {
            num11 = 0f;
        }
        num8 *= num11;
        f *= num8;
        float num12 = 1f;
        num12 += actor.getUpdownPower(target);
        num12 += target.getUpdownSelfDamage(actor);
        if (critical)
        {
            num12 += actor.getUpdownCriticalAtk(target);
        }
        if (nobleType != DamageType.NOMAL)
        {
            num12 += actor.getUpdownNpAtk(target);
        }
        f *= num12;
        if (nobleType != DamageType.NOMAL)
        {
            float num13 = 1f;
            float num14 = 0f;
            if (nobleType == DamageType.NOBLE_INDIVIDUAL)
            {
                if (target.checkIndividualities(svtIndv))
                {
                    num13 *= ((float) this.getPerlist(perlist, 1)) / 1000f;
                }
            }
            else if ((nobleType == DamageType.NOBLE_STATE_INDIVIDUAL) && target.checkBuffIndividualities(buffIndv))
            {
                num13 *= ((float) this.getPerlist(perlist, 1)) / 1000f;
            }
            f += num14;
            f *= num13;
        }
        float num15 = 0f;
        num15 += actor.getBuffDamageValue(target);
        num15 += target.getBuffSelfDamageValue(actor);
        if (command.isBusterChain())
        {
            num15 += actor.getBaseATK() * ConstantMaster.getRateValue("CHAINBONUS_BUSTER_RATE");
        }
        f += num15;
        if (0f < f)
        {
            flag6 = true;
        }
        else
        {
            f = 0f;
        }
        if (this.data.isTutorial() && (nobleType != DamageType.NOMAL))
        {
            f = target.hp;
        }
        if (actor.checkPierceInvincible(target))
        {
            if (target.checkInvincible(actor) || !target.checkAvoidance(actor))
            {
            }
        }
        else if (target.checkInvincible(actor))
        {
            f = 0f;
            flag6 = false;
            flag4 = true;
            if (!actor.checkBreakAvoidance(target))
            {
            }
        }
        else if (actor.checkBreakAvoidance(target))
        {
            if (!target.checkAvoidance(actor))
            {
            }
        }
        else if (target.checkAvoidance(actor))
        {
            f = 0f;
            flag6 = false;
            flag5 = true;
        }
        int item = Mathf.FloorToInt(f);
        int num17 = item;
        int num18 = 0;
        int num19 = this.getAttackNp(actor, target, command, critical, this.isNoble(nobleType));
        int num20 = this.getDefenseNp(actor, target, command);
        int num21 = this.getAttackStart(actor, target, command, critical);
        for (int i = 0; i < (numArray.Length - 1); i++)
        {
            int num23 = (num17 * numArray[i]) / num;
            if ((num23 <= 0) && flag6)
            {
                num23 = 1;
                num17--;
                if (num17 <= 0)
                {
                    num17 = 0;
                }
            }
            list.Add(num23);
            num18 += num23;
            item -= num23;
            if (item <= 0)
            {
                item = 0;
            }
        }
        if ((item <= 0) && flag6)
        {
            item = 1;
        }
        list.Add(item);
        num18 += item;
        data.targetId = target.getUniqueID();
        data.functionIndex = funcIndex;
        data.critical = critical;
        data.weak = flag2;
        data.regist = flag3;
        data.invincible = flag4;
        data.avoidance = flag5;
        data.sphit = actor.isSphitBuff();
        data.atkbufflist = actor.getRecBuffList();
        data.defbufflist = target.getRecBuffList();
        data.damagelist = list.ToArray();
        data.atknplist = new int[list.Count];
        data.defnplist = new int[list.Count];
        data.starlist = new int[list.Count];
        bool flag7 = target.provisionalDamage(0);
        bool flag8 = false;
        for (int j = 0; j < list.Count; j++)
        {
            int damage = list[j];
            flag8 = target.provisionalDamage(damage);
            if (flag7 && flag8)
            {
                data.overkillIndex = j;
                flag7 = false;
            }
            if (flag8)
            {
                data.atknplist[j] = num19;
                data.defnplist[j] = num20;
                data.starlist[j] = 0;
                for (int k = Math.Min(num21, ConstantMaster.getValue("STAR_RATE_MAX")); k > 0; k -= 0x3e8)
                {
                    data.starlist[j] += (BattleRandom.getNext(0x3e8) >= k) ? 0 : 1;
                }
            }
            else
            {
                data.atknplist[j] = Mathf.FloorToInt(num19 * ConstantMaster.getRateValue("OVER_KILL_NP_RATE"));
                data.defnplist[j] = Mathf.FloorToInt(num20 * ConstantMaster.getRateValue("OVER_KILL_NP_RATE"));
                data.starlist[j] = 0;
                int num27 = Mathf.FloorToInt(num21 * ConstantMaster.getRateValue("OVER_KILL_STAR_RATE")) + ConstantMaster.getValue("OVER_KILL_STAR_ADD");
                for (num27 = Math.Min(num27, ConstantMaster.getValue("STAR_RATE_MAX")); num27 > 0; num27 -= 0x3e8)
                {
                    data.starlist[j] += (BattleRandom.getNext(0x3e8) >= num27) ? 0 : 1;
                }
            }
        }
        if (nobleType == DamageType.NOMAL)
        {
            target.setActionHistory(actor.getUniqueID(), BattleServantActionHistory.TYPE.DAMAGE_COMMAND, this.data.wavecount);
            return data;
        }
        target.setActionHistory(actor.getUniqueID(), BattleServantActionHistory.TYPE.DAMAGE_TD, this.data.wavecount);
        return data;
    }

    public int getDefenseNp(BattleServantData actor, BattleServantData target, BattleCommandData command)
    {
        float f = 0f;
        f = target.getDefenceBaseNp();
        f *= actor.getTdRate();
        f *= target.getUpDownDropNp(target);
        f *= target.getUpDownDamageDropNp(target);
        if (f < 0f)
        {
            f = 0f;
        }
        return Mathf.FloorToInt(f);
    }

    public BattleActionData.DamageData getFunctionDamagelist(BattleServantData actor, BattleServantData target, int per, int funcIndex)
    {
        BattleCommandData command = new BattleCommandData {
            type = 3
        };
        int[] perlist = new int[] { per };
        return this.getDamagelist(actor, target, command, DamageType.NOMAL, funcIndex, perlist, null, null);
    }

    public BattleActionData.DamageData getFunctionNpDamagelist(BattleServantData actor, BattleServantData target, int[] perlist, int funcIndex, DamageType type, int[] svtIndv, int[] buffIndv)
    {
        BattleCommandData command = new BattleCommandData {
            type = actor.getTreasureDvcCardId()
        };
        return this.getDamagelist(actor, target, command, type, funcIndex, perlist, svtIndv, buffIndv);
    }

    public int getPerlist(int[] list, int index)
    {
        if (index < list.Length)
        {
            return list[index];
        }
        return 0;
    }

    private List<int> GetSvtBuffList(BattleBuffData.BuffData[] info)
    {
        if ((info == null) || (info.Length == 0))
        {
            return null;
        }
        List<int> list = new List<int>();
        foreach (BattleBuffData.BuffData data in info)
        {
            list.Add(data.buffId);
        }
        return list;
    }

    public int getTurn() => 
        this.data.turnCount;

    public BattleCommandData[] getTutorialCommandCards()
    {
        List<BattleCommandData> list = new List<BattleCommandData>();
        BattleServantData[] dataArray = this.data.getFieldPlayerServantList();
        int[] numArray1 = new int[5];
        numArray1[3] = 1;
        numArray1[4] = 2;
        int[] numArray = numArray1;
        BattleCommand.TYPE[] typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS };
        if (this.data.tutorialId == 1)
        {
            if (this.data.turnCount == 1)
            {
                numArray = new int[] { 0, 1, 2, 1, 2 };
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.QUICK };
            }
            else if (this.data.turnCount == 2)
            {
                int[] numArray2 = new int[5];
                numArray2[3] = 1;
                numArray2[4] = 2;
                numArray = numArray2;
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.ARTS };
            }
            else if (this.data.turnCount == 3)
            {
                numArray = new int[] { 2, 1, 1, 0, 2 };
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BUSTER };
            }
            else if (this.data.turnCount == 4)
            {
                int[] numArray3 = new int[5];
                numArray3[3] = 1;
                numArray3[4] = 2;
                numArray = numArray3;
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.ARTS };
            }
            else
            {
                numArray = new int[] { 2, 1, 2, 1, 0 };
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BUSTER };
            }
        }
        else if (this.data.tutorialId == 2)
        {
            if (this.data.turnCount == 1)
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
            else if (this.data.turnCount == 2)
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
            else
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
        }
        else if (this.data.tutorialId == 3)
        {
            if (this.data.wavecount == 0)
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.ARTS, BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
            else if (this.data.turnCount == 1)
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
            else if (this.data.turnCount == 2)
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.QUICK, BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
            else
            {
                numArray = new int[5];
                typeArray = new BattleCommand.TYPE[] { BattleCommand.TYPE.ARTS, BattleCommand.TYPE.BUSTER, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK, BattleCommand.TYPE.BLANK };
            }
        }
        for (int i = 0; i < 5; i++)
        {
            list.Add(this.getCommandData(dataArray[numArray[i]], typeArray[i]));
        }
        return list.ToArray();
    }

    public int getTutorialId() => 
        this.data.tutorialId;

    public int getWave() => 
        this.data.wavecount;

    public void initCommandBattle(string endproc)
    {
        this.logicindex = 0;
        this.list_logic.Clear();
        this.logicEnemyAi.resetAct();
        this.data.setInitCommandBattle();
        this.perf.setInitpos();
        this.setTimeAcceleration();
        this.perf.setTargetChangeActive(true);
        this.fsm.SendEvent("END_PROC");
    }

    public void initDebugMenu()
    {
        if (this.debugmenu != null)
        {
            this.debugmenu.ClearMenu();
            AllocMem component = GameObject.Find("Management").GetComponent<AllocMem>();
            this.debugmenu.setMenu("Print Mem", new DebugPanelRootComponent.tgrDelegate(this.debugChangeMem), component.show);
            this.debugmenu.setMenu("Battle Win", new DebugPanelRootComponent.paramDelegate(this.debugBattleEnd), 0);
            this.debugmenu.setMenu("Wave Clear", new DebugPanelRootComponent.paramDelegate(this.debugWaveEnd), 0);
            this.debugmenu.setMenu("Fix Damage", new DebugPanelRootComponent.tgrDelegate(this.debugFixDamage), this.systemflg_fixdamage);
            this.debugmenu.setMenu("DebugAct:", new DebugPanelRootComponent.menuDelegate(this.debugAct));
            this.debugmenu.setMenu("SetDays:" + this.setdays, null);
            this.debugmenu.setMenu("BugFix OverKill", new DebugPanelRootComponent.tgrDelegate(this.debugFixOverKill), this.bugfix_overkill);
            this.debugmenu.setMenu("Shorten Skill", new DebugPanelRootComponent.menuDelegate(this.debugShortenSkill));
            this.debugmenu.setMenu("Show FsmState", new DebugPanelRootComponent.paramDelegate(this.debugShowState), 0);
            this.debugmenu.setMenu("Show SvtState", new DebugPanelRootComponent.paramDelegate(this.debugShowSvtState), 0);
            this.debugmenu.setMenu("Change AI NomalAtacck", new DebugPanelRootComponent.paramDelegate(this.debugChangeAi), 0);
            this.debugmenu.setMenu("Change AI CriticalAtacck", new DebugPanelRootComponent.paramDelegate(this.debugChangeAi), 1);
            this.debugmenu.setMenu("Change AI RandomSkill", new DebugPanelRootComponent.paramDelegate(this.debugChangeAi), 2);
            this.debugmenu.setMenu("PlayeAllBuff", new DebugPanelRootComponent.paramDelegate(this.debugAddFullBuff), 0);
            this.debugmenu.setMenu("EnemyAllBuff", new DebugPanelRootComponent.paramDelegate(this.debugAddFullBuff), 1);
            this.debugmenu.setMenu("PlayeAllBuff - act", new DebugPanelRootComponent.paramDelegate(this.debugAddFullBuffNoAct), 0);
            this.debugmenu.setMenu("EnemyAllBuff - act", new DebugPanelRootComponent.paramDelegate(this.debugAddFullBuffNoAct), 1);
            this.debugmenu.setMenu("Super A Up 500%", new DebugPanelRootComponent.paramDelegate(this.debugAddBuffCommandCard), 1);
            this.debugmenu.setMenu("Super B Up 500%", new DebugPanelRootComponent.paramDelegate(this.debugAddBuffCommandCard), 2);
            this.debugmenu.setMenu("Super Q Up 500%", new DebugPanelRootComponent.paramDelegate(this.debugAddBuffCommandCard), 3);
            this.debugmenu.setMenu("Super DefUp 100%", new DebugPanelRootComponent.paramDelegate(this.debugAddBuff), 0x92);
            this.debugmenu.setMenu("Super INVINCIBLE", new DebugPanelRootComponent.paramDelegate(this.debugAddBuff), 0x9b);
            this.debugmenu.setMenu("addBuff - 189", new DebugPanelRootComponent.paramDelegate(this.debugAddBuff), 0xbd);
            this.debugmenu.setMenu("addBuff - 178", new DebugPanelRootComponent.paramDelegate(this.debugAddBuff), 0xb2);
            this.debugmenu.setMenu("addBuff - 180", new DebugPanelRootComponent.paramDelegate(this.debugAddBuff), 180);
            this.debugmenu.setMenu("EnemyaddBuff - 178", new DebugPanelRootComponent.paramDelegate(this.debugEnemyAddBuff), 0xb2);
            this.debugmenu.setMenu("Draw CommandCard", new DebugPanelRootComponent.paramDelegate(this.debugDrawCommandCard), 0);
            this.debugmenu.setMenu("CommandChange - Quick", new DebugPanelRootComponent.paramDelegate(this.debugCommandChange), 3);
            this.debugmenu.setMenu("CommandChange - Arts", new DebugPanelRootComponent.paramDelegate(this.debugCommandChange), 1);
            this.debugmenu.setMenu("CommandChange - Buster", new DebugPanelRootComponent.paramDelegate(this.debugCommandChange), 2);
            this.debugmenu.setMenu("Command Nomal", new DebugPanelRootComponent.paramDelegate(this.debugCommandChange), 0);
            this.debugmenu.setMenu("Enemy to dying", new DebugPanelRootComponent.paramDelegate(this.debugProc), 1);
            this.debugmenu.setMenu("Player to dying", new DebugPanelRootComponent.paramDelegate(this.debugProc), 2);
            this.debugmenu.setMenu("Enemy to Recover", new DebugPanelRootComponent.paramDelegate(this.debugProc), 3);
            this.debugmenu.setMenu("Player to Recover", new DebugPanelRootComponent.paramDelegate(this.debugProc), 4);
            this.debugmenu.setMenu("Player to RecoverOne", new DebugPanelRootComponent.paramDelegate(this.debugProc), 9);
            this.debugmenu.setMenu("Player to Noble Ready", new DebugPanelRootComponent.paramDelegate(this.debugProc), 5);
            this.debugmenu.setMenu("Enemy to Noble Ready", new DebugPanelRootComponent.paramDelegate(this.debugProc), 7);
            this.debugmenu.setMenu("Enemy to Noble Haste", new DebugPanelRootComponent.paramDelegate(this.debugProc), 8);
            this.debugmenu.setMenu("Critical Up", new DebugPanelRootComponent.paramDelegate(this.debugProc), 6);
            this.debugmenu.setMenu("Fog Mode", new DebugPanelRootComponent.tgrDelegate(this.debugChangeFog), RenderSettings.fog);
            this.debugmenu.setMenu("Show Tips", new DebugPanelRootComponent.tgrDelegate(this.debugTips), true);
            int num = this.perf.getAmbientColors();
            for (int i = 0; i < num; i++)
            {
                this.debugmenu.setMenu("Ambient Color index[" + i + "]", new DebugPanelRootComponent.paramDelegate(this.debugChangeAmbient), i);
            }
            this.debugmenu.setMenu("FieldEffect None ", new DebugPanelRootComponent.paramDelegate(this.debugChangeFieldEffect), -1);
            string[] strArray = this.perf.getFieldEffects();
            for (int j = 0; j < strArray.Length; j++)
            {
                this.debugmenu.setMenu("FieldEffect > " + strArray[j], new DebugPanelRootComponent.paramDelegate(this.debugChangeFieldEffect), j);
            }
            string[] strArray2 = this.perf.getChangeBgList();
            for (int k = 0; k < strArray2.Length; k++)
            {
                this.debugmenu.setMenu("Bg > " + strArray2[k], new DebugPanelRootComponent.paramStrDelegate(this.debugChangeBg), strArray2[k]);
            }
            this.debugmenu.setMenu("Bloom ", new DebugPanelRootComponent.tgrDelegate(this.debugChangeBloom), this.bloom.enabled);
            this.debugmenu.setMenu("Motion Blur ", new DebugPanelRootComponent.tgrDelegate(this.debugChangeBlur), this.blur.enabled);
            this.debugmenu.setMenu("Vignetting ", new DebugPanelRootComponent.tgrDelegate(this.debugChangeVignetting), this.vignetting.enabled);
            this.debugmenu.setMenu("Grayscale ", new DebugPanelRootComponent.tgrDelegate(this.debugChangeGrayScale), this.grayscaleMiddle.enabled);
            this.debugmenu.setMenu("CardCancel? ", new DebugPanelRootComponent.tgrDelegate(this.systemSelectCardFlg), this.data.systemflg_selectcancel);
            this.debugmenu.setMenu("Acceleration", new DebugPanelRootComponent.paramDelegate(this.debugChangeAcceleration), this.data.systemflg_acceleration, 1, 4);
            this.debugmenu.setMenu("ShowAutoBattle", new DebugPanelRootComponent.tgrDelegate(this.debugChangeAutoBattle), this.data.systemflg_showautobutton);
            this.debugmenu.setMenu("Slow", new DebugPanelRootComponent.paramDelegate(this.debugChangeSlow), 1, 1, 10);
            this.debugmenu.setMenu("TutorialArrowMessage", new DebugPanelRootComponent.tgrDelegate(this.debugTutorialArrowMessage), false);
        }
    }

    public void Initialize()
    {
        this.logictarget = new BattleLogicTarget();
        this.logicEnemyAi = new BattleLogicEnemyAi();
        this.logicFunction = new BattleLogicFunction();
        this.logicnomal = new BattleLogicNomal();
        this.logicspecial = new BattleLogicSpecial();
        this.logicSkill = new BattleLogicSkill();
        this.logicReaction = new BattleLogicReaction();
        this.brandom = base.gameObject.AddComponent<BattleRandom>();
        Dictionary<LOGICTYPE, taskFunction> dictionary = new Dictionary<LOGICTYPE, taskFunction> {
            { 
                LOGICTYPE.COMMAND_BEFORE,
                new taskFunction(this.logicnomal.taskComboOrderBefore)
            },
            { 
                LOGICTYPE.COMMAND_ATTACK_1,
                new taskFunction(this.logicnomal.taskCommandAttack)
            },
            { 
                LOGICTYPE.COMMAND_ATTACK_2,
                new taskFunction(this.logicnomal.taskCommandAttack)
            },
            { 
                LOGICTYPE.COMMAND_ATTACK_3,
                new taskFunction(this.logicnomal.taskCommandAttack)
            },
            { 
                LOGICTYPE.COMMAND_AFTER,
                new taskFunction(this.logicnomal.taskComboOrderAfter)
            },
            { 
                LOGICTYPE.COMMAND_ADDATTACK,
                new taskFunction(this.logicnomal.taskAddCommandAttack)
            },
            { 
                LOGICTYPE.ENEMY_ATTACK_1,
                new taskFunction(this.logicEnemyAi.taskAIAttack)
            },
            { 
                LOGICTYPE.ENEMY_ATTACK_2,
                new taskFunction(this.logicEnemyAi.taskAIAttack)
            },
            { 
                LOGICTYPE.ENEMY_ATTACK_3,
                new taskFunction(this.logicEnemyAi.taskAIAttack)
            },
            { 
                LOGICTYPE.PLAYER_SPECIAL_1,
                new taskFunction(this.logicspecial.taskTresureDvc)
            },
            { 
                LOGICTYPE.PLAYER_SPECIAL_2,
                new taskFunction(this.logicspecial.taskTresureDvc)
            },
            { 
                LOGICTYPE.PLAYER_SPECIAL_3,
                new taskFunction(this.logicspecial.taskTresureDvc)
            },
            { 
                LOGICTYPE.ENEMY_SPECIAL_1,
                new taskFunction(this.logicspecial.taskEnemyTresureDvc)
            },
            { 
                LOGICTYPE.ENEMY_SPECIAL_2,
                new taskFunction(this.logicspecial.taskEnemyTresureDvc)
            },
            { 
                LOGICTYPE.ENEMY_SPECIAL_3,
                new taskFunction(this.logicspecial.taskEnemyTresureDvc)
            },
            { 
                LOGICTYPE.PLAYER_SP1_ALWAYS,
                new taskFunction(this.logicspecial.taskTresureDvcAlways)
            },
            { 
                LOGICTYPE.PLAYER_SP2_ALWAYS,
                new taskFunction(this.logicspecial.taskTresureDvcAlways)
            },
            { 
                LOGICTYPE.PLAYER_SP3_ALWAYS,
                new taskFunction(this.logicspecial.taskTresureDvcAlways)
            },
            { 
                LOGICTYPE.ENEMY_SP1_ALWAYS,
                new taskFunction(this.logicspecial.taskEnemyTresureDvcAlways)
            },
            { 
                LOGICTYPE.ENEMY_SP2_ALWAYS,
                new taskFunction(this.logicspecial.taskEnemyTresureDvcAlways)
            },
            { 
                LOGICTYPE.ENEMY_SP3_ALWAYS,
                new taskFunction(this.logicspecial.taskEnemyTresureDvcAlways)
            },
            { 
                LOGICTYPE.COMMAND_WAIT,
                new taskFunction(this.taskCommandWait)
            },
            { 
                LOGICTYPE.LAST_BACKSTEP,
                new taskFunction(this.taskLastActorBackStep)
            },
            { 
                LOGICTYPE.GET_DROPITEM,
                new taskFunction(this.taskGetCriticalPoint)
            },
            { 
                LOGICTYPE.PLAYER_ENDTURN,
                new taskFunction(this.taskEndPlayerTurn)
            },
            { 
                LOGICTYPE.ENEMY_ENDTURN,
                new taskFunction(this.taskEndEnemyTurn)
            },
            { 
                LOGICTYPE.ENEMY_ENDWAIT,
                new taskFunction(this.taskEnemyEndWait)
            },
            { 
                LOGICTYPE.REACTION_PLAYERACTIONEND,
                new taskFunction(this.logicReaction.checkPlayerActionEnd)
            },
            { 
                LOGICTYPE.BUFF_ADDPARAM_PLAYER,
                new taskFunction(this.logicnomal.taskBuffAdd)
            },
            { 
                LOGICTYPE.BUFF_ADDPARAM_ENEMY,
                new taskFunction(this.logicnomal.taskBuffAdd)
            },
            { 
                LOGICTYPE.START_PLAYERTURN,
                new taskFunction(this.taskStartPlayerTurn)
            },
            { 
                LOGICTYPE.START_ENEMYTURN,
                new taskFunction(this.taskStartEnemyTurn)
            },
            { 
                LOGICTYPE.TACTICAL_START,
                new taskFunction(this.taskStartTactical)
            },
            { 
                LOGICTYPE.REACTION_STARTENEMY,
                new taskFunction(this.logicReaction.checkEnemyStartTurn)
            },
            { 
                LOGICTYPE.REACTION_ENDENEMY,
                new taskFunction(this.logicReaction.checkEnemyEndTurn)
            }
        };
        this.funclist = dictionary;
        Dictionary<BattleLogicTask.ACTIONTYPE, createActionBattle> dictionary2 = new Dictionary<BattleLogicTask.ACTIONTYPE, createActionBattle> {
            { 
                BattleLogicTask.ACTIONTYPE.COMMAND_BATTLE,
                new createActionBattle(this.logicnomal.createCommandBattle)
            },
            { 
                BattleLogicTask.ACTIONTYPE.ADDATTACK,
                new createActionBattle(this.logicnomal.createCommandBattle)
            },
            { 
                BattleLogicTask.ACTIONTYPE.SKILL,
                new createActionBattle(this.logicSkill.createSkillData)
            },
            { 
                BattleLogicTask.ACTIONTYPE.TREASURE_DEVICE,
                new createActionBattle(this.logicspecial.createSpecialData)
            },
            { 
                BattleLogicTask.ACTIONTYPE.BACKSTEP,
                new createActionBattle(this.createLastActorBackStep)
            },
            { 
                BattleLogicTask.ACTIONTYPE.SYSTEM,
                new createActionBattle(this.createSystem)
            },
            { 
                BattleLogicTask.ACTIONTYPE.COMBO_ORDER,
                new createActionBattle(this.logicnomal.createComboOrder)
            },
            { 
                BattleLogicTask.ACTIONTYPE.COMMAND_SPELL,
                new createActionBattle(this.logicSkill.createCommandSpell)
            },
            { 
                BattleLogicTask.ACTIONTYPE.PLAY_MOTION,
                new createActionBattle(this.logicnomal.createPlayMotion)
            },
            { 
                BattleLogicTask.ACTIONTYPE.ENDTURN_PLAYER,
                new createActionBattle(this.logicnomal.createEndTurnPlayer)
            },
            { 
                BattleLogicTask.ACTIONTYPE.ENDTURN_ENEMY,
                new createActionBattle(this.logicnomal.createEndTurnEnemy)
            },
            { 
                BattleLogicTask.ACTIONTYPE.RESURRECTION,
                new createActionBattle(this.logicReaction.createResurrection)
            },
            { 
                BattleLogicTask.ACTIONTYPE.DEAD,
                new createActionBattle(this.logicReaction.createDeadMotion)
            },
            { 
                BattleLogicTask.ACTIONTYPE.BUFF_ADD_PLAYER,
                new createActionBattle(this.logicnomal.createBuffAddPlayer)
            },
            { 
                BattleLogicTask.ACTIONTYPE.BUFF_ADD_ENEMY,
                new createActionBattle(this.logicnomal.createBuffAddEnemy)
            },
            { 
                BattleLogicTask.ACTIONTYPE.STARTTURN_PLAYER,
                new createActionBattle(this.logicnomal.createStartTurn)
            },
            { 
                BattleLogicTask.ACTIONTYPE.STARTTURN_ENEMY,
                new createActionBattle(this.logicnomal.createStartTurn)
            }
        };
        this.actionBattleList = dictionary2;
        LOGICTYPE[] logictypeArray1 = new LOGICTYPE[0x18];
        logictypeArray1[0] = LOGICTYPE.TACTICAL_START;
        logictypeArray1[1] = LOGICTYPE.START_PLAYERTURN;
        logictypeArray1[3] = LOGICTYPE.COMMAND_ATTACK_1;
        logictypeArray1[4] = LOGICTYPE.COMMAND_ATTACK_2;
        logictypeArray1[5] = LOGICTYPE.COMMAND_ATTACK_3;
        logictypeArray1[6] = LOGICTYPE.COMMAND_ADDATTACK;
        logictypeArray1[7] = LOGICTYPE.COMMAND_AFTER;
        logictypeArray1[8] = LOGICTYPE.GET_DROPITEM;
        logictypeArray1[9] = LOGICTYPE.COMMAND_WAIT;
        logictypeArray1[10] = LOGICTYPE.REACTION_PLAYERACTIONEND;
        logictypeArray1[11] = LOGICTYPE.PLAYER_ENDTURN;
        logictypeArray1[12] = LOGICTYPE.BUFF_ADDPARAM_ENEMY;
        logictypeArray1[13] = LOGICTYPE.START_ENEMYTURN;
        logictypeArray1[14] = LOGICTYPE.REACTION_STARTENEMY;
        logictypeArray1[15] = LOGICTYPE.ENEMY_ATTACK_1;
        logictypeArray1[0x10] = LOGICTYPE.ENEMY_ATTACK_2;
        logictypeArray1[0x11] = LOGICTYPE.ENEMY_ATTACK_3;
        logictypeArray1[0x12] = LOGICTYPE.REACTION_ENDENEMY;
        logictypeArray1[0x13] = LOGICTYPE.LAST_BACKSTEP;
        logictypeArray1[20] = LOGICTYPE.ENEMY_ENDTURN;
        logictypeArray1[0x15] = LOGICTYPE.BUFF_ADDPARAM_PLAYER;
        logictypeArray1[0x16] = LOGICTYPE.ENEMY_ENDWAIT;
        logictypeArray1[0x17] = LOGICTYPE.GET_DROPITEM;
        this.logiclist = logictypeArray1;
        this.logictarget.setInit(this.data);
        this.logicnomal.logic = this;
        this.logicnomal.data = this.data;
        this.logicnomal.logictarget = this.logictarget;
        this.logicnomal.logicfunction = this.logicFunction;
        this.logicnomal.logicskill = this.logicSkill;
        this.logicEnemyAi.logic = this;
        this.logicEnemyAi.data = this.data;
        this.logicEnemyAi.logictarget = this.logictarget;
        this.logicEnemyAi.logicNomal = this.logicnomal;
        this.logicFunction.logic = this;
        this.logicFunction.data = this.data;
        this.logicFunction.logictarget = this.logictarget;
        this.logicSkill.logic = this;
        this.logicSkill.data = this.data;
        this.logicSkill.logictarget = this.logictarget;
        this.logicSkill.logicfunction = this.logicFunction;
        this.logicspecial.logic = this;
        this.logicspecial.data = this.data;
        this.logicspecial.logictarget = this.logictarget;
        this.logicspecial.logicfunction = this.logicFunction;
        this.logicReaction.logic = this;
        this.logicReaction.data = this.data;
        this.logicReaction.perf = this.perf;
        this.logicReaction.logicskill = this.logicSkill;
        this.logicReaction.logicfunction = this.logicFunction;
        this.spawner = SingletonMonoBehaviour<Spawner>.Instance;
        this.setActiviteUnusedGameobject(false);
        this.initDebugMenu();
    }

    public void initNextTurnData()
    {
        if (this.data.initturn == 1)
        {
            this.data.initturn = 0;
        }
        else
        {
            this.data.turnCount++;
            this.drawCommand();
        }
    }

    public void initQuest(string endproc)
    {
        if (SingletonMonoBehaviour<DataManager>.getInstance().getSingleEntity<BattleEntity>(DataNameKind.Kind.BATTLE) == null)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(LocalizationManager.Get("BATTLE_NOENTITY_TITLE"), LocalizationManager.Get("BATTLE_NOENTITY_CONF"), new ErrorDialog.ClickDelegate(this.callBackNoEntity), true);
        }
        else
        {
            this.data.flg_resumebattle = false;
            if (this.data.checkTurnData())
            {
                this.data.flg_resumebattle = true;
                this.sendFsmEvent("IS_SAVE");
            }
            else
            {
                base.StartCoroutine(this.coroutineInitQuest(endproc));
            }
        }
    }

    public bool isNoble(DamageType inType) => 
        (inType != DamageType.NOMAL);

    public bool isTimingUseSkill()
    {
        string activeStateName = this.fsm.ActiveStateName;
        if (this.perf.fieldmotion.isStep())
        {
            return false;
        }
        if (((this.data.tutorialId == 3) && (this.data.wavecount == 1)) && (this.data.turnCount == 1))
        {
            return false;
        }
        if (!activeStateName.Equals("wait_tuto") && !activeStateName.Equals("Faze_Tactical"))
        {
            return false;
        }
        return true;
    }

    public bool isTutorial() => 
        this.data.isTutorial();

    public bool isTutorialMasterStatus()
    {
        if (!this.isTutorial())
        {
            return false;
        }
        if ((this.data.tutorialId == 2) && (this.data.turnCount == 3))
        {
            return false;
        }
        if (this.data.tutorialId == 3)
        {
            return ((this.data.wavecount == 1) && (this.data.turnCount == 1));
        }
        if (this.data.tutorialId == 4)
        {
            return false;
        }
        return true;
    }

    public void loadBgmName()
    {
        string str = this.getBattleBgmName();
        this.fsm.Fsm.GetFsmString("BgmName").Value = str;
        this._CurrentBgmName = str;
    }

    public void loadNextstage(string endproc)
    {
        base.StartCoroutine(this.coroutineLoadNstage(endproc));
    }

    public void loadNstage(string endproc)
    {
        base.StartCoroutine(this.coroutineLoadNstage(endproc));
    }

    public void loadSaveBattle()
    {
        base.StartCoroutine(this.coroutineLoadQuest());
    }

    public void loadSaveWave()
    {
        base.StartCoroutine(this.coroutineLoadSaveWave());
    }

    private void OnDestroy()
    {
        this.setActiviteUnusedGameobject(true);
    }

    public void openConnectErrorDialog()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_DIALOG_USECOMMANDSPELL_ERROR_TITLE"), LocalizationManager.Get("BATTLE_DIALOG_USECOMMANDSPELL_ERROR_CONF"), new NotificationDialog.ClickDelegate(this.requestErrorDialog), -1);
    }

    public void playRetire()
    {
        this.fsm.SendEvent("PROC_RETIRE");
    }

    public void playTaskBattle()
    {
        BattleLogicTask task = this.list_logic.First.Value;
        if (this.checkRelationTask(task))
        {
            if (0 < this.perf.checkActionCount())
            {
                this.perf.playActionData(null);
            }
            else
            {
                this.endActionData();
            }
        }
        else
        {
            BattleActionData adddata = this.procBattleTask(task);
            if ((((SingletonMonoBehaviour<DataManager>.Instance.IsBattleLiveOpen(this.data) && this.data.bReceiveServerData) && ((adddata != null) && (adddata.actorId != 0))) && (((task.actiontype == BattleLogicTask.ACTIONTYPE.ADDATTACK) || (task.actiontype == BattleLogicTask.ACTIONTYPE.COMMAND_BATTLE)) || ((task.actiontype == BattleLogicTask.ACTIONTYPE.TREASURE_DEVICE) || (task.actiontype == BattleLogicTask.ACTIONTYPE.SKILL)))) && !this.data.bBattleLiveError)
            {
                BattleError error;
                if (DataManager.actionIndex >= SingletonMonoBehaviour<DataManager>.Instance.GetbattleActionData().Length)
                {
                    this.data.bBattleLiveError = true;
                    error = new BattleError {
                        bError = 1,
                        type = 1,
                        value1 = DataManager.actionIndex,
                        value2 = SingletonMonoBehaviour<DataManager>.Instance.GetbattleActionData().Length
                    };
                    this.data.errorinfo = error;
                }
                else
                {
                    BattleActionData data2 = SingletonMonoBehaviour<DataManager>.Instance.GetbattleActionData()[DataManager.actionIndex];
                    if (this.data.getServantData(data2.targetId) == null)
                    {
                        this.data.bBattleLiveError = true;
                        error = new BattleError {
                            bError = 1,
                            type = 2,
                            value1 = adddata.targetId,
                            value2 = data2.targetId
                        };
                        this.data.errorinfo = error;
                    }
                    else
                    {
                        Debug.LogError(string.Concat(new object[] { "length:", SingletonMonoBehaviour<DataManager>.Instance.GetbattleActionData().Length, "  index: ", DataManager.actionIndex }));
                        adddata.SetDataFromServer(data2.actorId, data2.targetId, data2.pttargetIds, data2.motionId, data2.type, data2.motionname, data2.flash, data2.pair, data2.three, data2.prevattackme, data2.nextattackme, data2.actionIndex, data2.attackcount, data2.chainCount, data2.commandattack, data2.treasureDvcId, data2.systemTime, data2.skillMessage, data2.motionMessage, data2.addCriticalStars, data2.redrawCommandCard, data2.effectlist, data2.state, data2.damagedatalist, data2.buffdatalist, data2.transformServantlist, data2.replacememberlist, data2.healdatalist);
                        DataManager.actionIndex++;
                    }
                }
            }
            this.list_logic.RemoveFirst();
            if ((adddata != null) || (0 < this.perf.checkActionCount()))
            {
                this.perf.playActionData(adddata);
            }
            else
            {
                this.endActionData();
            }
        }
    }

    public BattleActionData procBattleTask(BattleLogicTask task)
    {
        BattleActionData data = null;
        if (this.actionBattleList.ContainsKey(task.actiontype))
        {
            data = this.actionBattleList[task.actiontype](task);
        }
        if (data == null)
        {
            Debug.Log("retData is null");
        }
        return data;
    }

    public void procComboAct()
    {
        BattleComboData combodata = this.data.combodata;
        if (combodata.flash)
        {
            if (BattleCommand.isARTS(combodata.flashtype))
            {
                List<int> list = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    int item = this.data.getSelectCommand(i).getUniqueId();
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
                foreach (int num3 in list)
                {
                    this.data.getServantData(num3).addNpPer(ConstantMaster.getRateValue("CHAINBONUS_ARTS_RATE"));
                }
            }
            else if (BattleCommand.isQUICK(combodata.flashtype))
            {
                SoundManager.playSe("ba12");
                this.data.addCriticalPoint(ConstantMaster.getValue("CHAINBONUS_QUICK"));
                this.perf.statusPerf.updateCriticalPoint();
            }
        }
    }

    public void procEndBattle()
    {
        BattleData.deleteSaveData();
    }

    public void procLose()
    {
        this.data.win_lose = 2;
    }

    public void procRetire()
    {
        this.data.win_lose = 2;
        this.StartResultRequest("END_PROC");
    }

    public void procWin()
    {
        this.setActiviteUnusedGameobject(true);
        this.data.win_lose = 1;
    }

    public void replaceMember(BattleActionData.ReplaceMember replaceData, System.Action endCallBack)
    {
        base.StartCoroutine(this.colReplaceMember(replaceData, endCallBack));
    }

    public void requestErrorDialog(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.fsm.SendEvent("END_PROC");
    }

    public void requestResultErrorDialog(bool flg)
    {
        this.fsm.SendEvent("END_PROC");
    }

    public void resetCriticalPoint()
    {
    }

    public void resetOverKill()
    {
        foreach (BattleServantData data in this.data.getFieldPlayerServantList())
        {
            data.setOverKillTargetId(-1);
        }
        foreach (BattleServantData data2 in this.data.getFieldEnemyServantList())
        {
            data2.setOverKillTargetId(-1);
        }
    }

    public void resetReducedHpAll()
    {
        foreach (BattleServantData data in this.data.getFieldServantList())
        {
            data.resetReducedHp();
        }
    }

    public void resetTimeAcceleration()
    {
        Time.timeScale = this.prevScaleTime;
    }

    public void responseResultBattle()
    {
        BattleData.deleteSaveData();
        this.fsm.SendEvent("END_PROC");
    }

    private string returnarray(int[] a)
    {
        string str = string.Empty;
        if ((a != null) && (a.Length > 0))
        {
            foreach (int num in a)
            {
                str = str + num + "'";
            }
        }
        return str;
    }

    public void selectTactical(string endproc)
    {
        if (this.isTutorial())
        {
            endproc = "TUTORIAL";
        }
        else if (this.data.systemflg_autobattle)
        {
            endproc = "SKIP";
        }
        foreach (BattleServantData data in this.data.getFieldPlayerServantList())
        {
            data.initTacticalFaze();
        }
        foreach (BattleServantData data2 in this.data.getFieldEnemyServantList())
        {
            data2.initTacticalFaze();
        }
        BattleServantData data3 = this.data.getServantData(this.data.globaltargetId);
        if ((data3 == null) || !data3.isAlive())
        {
            int[] numArray = this.data.getFieldEnemyServantIDList();
            if (0 < numArray.Length)
            {
                this.data.globaltargetId = numArray[0];
            }
        }
        if (this.data.initturn == 1)
        {
            if (!this.data.checkAliveEnemys())
            {
                endproc = "RESUME_CONT";
            }
            this.data.initturn = 0;
        }
        else if (this.data.tutorialId != 1)
        {
            this.data.SaveTurnData();
        }
        this.perf.startTac(endproc);
    }

    public void sendFsmEvent(string evstr)
    {
        this.fsm.SendEvent(evstr);
    }

    private void setActiviteUnusedGameobject(bool active = true)
    {
        if (!active)
        {
            int length = unusedGameObjectNames.Length;
            this.unusedGameobjects.Clear();
            for (int i = 0; i < length; i++)
            {
                GameObject item = GameObject.Find(unusedGameObjectNames[i]);
                item.SetActive(active);
                this.unusedGameobjects.Add(item);
            }
        }
        else
        {
            int count = this.unusedGameobjects.Count;
            for (int j = 0; j < count; j++)
            {
                if (this.unusedGameobjects[j] != null)
                {
                    this.unusedGameobjects[j].SetActive(active);
                }
            }
        }
    }

    public void setDrawCard()
    {
        this.perf.registCommandCard(this.data.draw_commandlist);
    }

    public void setNextBattle(string endproc)
    {
        this.data.setNextStage();
    }

    public bool setTargetIndex(int index) => 
        this.data.setTargetIndex(index);

    public void setTimeAcceleration()
    {
        this.prevScaleTime = Time.timeScale;
        Time.timeScale = this.data.systemflg_acceleration * 1f;
    }

    public void startCommand(string endproc)
    {
        SoundManager.playSe("ba10a");
        List<BattleCommandData> list = new List<BattleCommandData>();
        BattleCommandData item = null;
        this.perf.closeSelectSvtWindow();
        this.perf.closeSelectMainSubSvtWindow();
        WeightRate<int> rate = new WeightRate<int>();
        BattleCommandData[] dataArray = new BattleCommandData[5];
        int[] numArray1 = new int[5];
        numArray1[0] = 50;
        numArray1[1] = 20;
        numArray1[2] = 20;
        int[] numArray = numArray1;
        numArray = BattleRandom.getShuffle<int>(numArray);
        for (int i = 0; i < 5; i++)
        {
            item = this.data.draw_commandlist[i];
            list.Add(item);
            BattleServantData data2 = this.data.getServantData(item.getUniqueId());
            if ((data2 != null) && !BattleCommand.isBLANK(item.type))
            {
                rate.setWeight(data2.getCriticalWeight(item.getIndividualities()) + numArray[i], i);
            }
            dataArray[i] = item;
            dataArray[i].starcount = 0;
            dataArray[i].starBonus = 0;
        }
        Debug.Log("this.data.getCriticalPoint ():" + this.data.getCriticalPoint());
        Debug.Log(string.Empty + ConstantMaster.getValue("CRITICAL_RATE_PER_STAR"));
        for (int j = 0; j < this.data.getCriticalPoint(); j++)
        {
            if (rate.checkWeight())
            {
                int keywieght = BattleRandom.getRandom(0, rate.getTotalWeight());
                int index = rate.getData(keywieght);
                dataArray[index].addCriticalPoint(ConstantMaster.getValue("CRITICAL_RATE_PER_STAR"));
                if (dataArray[index].checkCriticalMax())
                {
                    rate.removeWeight(index);
                }
            }
        }
        this.data.resetCriticalPoint();
        BattleServantData[] dataArray2 = this.data.getFieldPlayerServantList();
        for (int k = 0; k < dataArray2.Length; k++)
        {
            if (dataArray2[k].checkUseTDvc(false))
            {
                item = new BattleCommandData {
                    type = dataArray2[k].getTreasureDvcCardId(),
                    svtlimit = dataArray2[k].getCommandDispLimitCount(),
                    loadSvtLimit = dataArray2[k].getDispLimitCount(),
                    uniqueId = dataArray2[k].getUniqueID(),
                    svtId = dataArray2[k].getSvtId(),
                    treasureDvc = dataArray2[k].getTreasureDvcId()
                };
                item.setFollowerType(dataArray2[k].followerType);
                item.flgEventJoin = dataArray2[k].flgEventJoin;
                list.Add(item);
            }
            else
            {
                list.Add(null);
            }
        }
        this.perf.setCommandCard(list.ToArray(), this.data.p_maxcommand);
        SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
        this.fsm.SendEvent(endproc);
    }

    private void startCommandSpellEffect()
    {
        int spellImageId = UserGameMaster.getSelfUserGame().SpellImageId;
        AssetManager.loadAssetStorage($"CommandSpellEffect/ef_commandspell{spellImageId:D4}", new AssetLoader.LoadEndDataHandler(this.endLoadCommandSPell));
    }

    public void startContinue()
    {
        this.perf.startContinue();
    }

    public void startEntryMember(string endproc)
    {
        int id = -1;
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        for (int i = 0; i < this.data.p_entryid.Length; i++)
        {
            id = this.data.p_entryid[i];
            BattleServantData svtData = this.data.getPlayerServantData(id);
            if ((svtData != null) && svtData.isWaitRepop)
            {
                svtData.isWaitRepop = false;
                svtData.flgEntryFunction = true;
                BattleLogicTask[] collection = this.logicnomal.taskPlaySubEntryMotion(svtData, this.perf.getRepopPlayerPos(i));
                list.AddRange(collection);
            }
        }
        for (int j = 0; j < this.data.e_entryid.Length; j++)
        {
            id = this.data.e_entryid[j];
            BattleServantData data2 = this.data.getEnemyServantData(id);
            if ((data2 != null) && data2.isWaitRepop)
            {
                data2.isWaitRepop = false;
                data2.flgEntryFunction = true;
                BattleLogicTask[] taskArray2 = this.logicnomal.taskPlaySubEntryMotion(data2, this.perf.getRepopEnemyPos(j));
                list.AddRange(taskArray2);
            }
        }
        if (list.Count > 0)
        {
            BattleLogicTask item = new BattleLogicTask();
            item.setCheckEntryFunction();
            list.Add(item);
            this.actOneBattleTask(list.ToArray());
        }
    }

    public void startRecoverPT()
    {
        base.StartCoroutine(this.coroutineContinueMember("END_PROC"));
    }

    public void StartResultRequest(string endEvent)
    {
        if (this.data.getBattleEntity().questId == ConstantMaster.getValue("TUTORIAL_QUEST_ID1"))
        {
        }
        this.fsm.SendEvent("END_PROC");
    }

    public void StartShowResult(string endEvent)
    {
        if (this.data.isTutorial() && (this.data.tutorialId == 1))
        {
            this.fsm.SendEvent(endEvent);
        }
        else if (this.data.win_lose == 1)
        {
            this.perf.setResult(this.resultstring);
            this.perf.showResult(base.gameObject, endEvent);
        }
        else
        {
            this.fsm.SendEvent(endEvent);
        }
    }

    public void startWaveAction()
    {
        base.StartCoroutine(this.checkWaveFunc());
    }

    public void systemSelectCardFlg(bool flg)
    {
        this.data.systemflg_selectcancel = flg;
    }

    public BattleLogicTask[] taskCommandWait(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setSystem();
        item.systemTime = 0.8f;
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskEndEnemyTurn(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setEndTurnEnemy();
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskEndPlayerTurn(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        this.perf.commandPerf.fadeOutAllCard();
        BattleLogicTask item = new BattleLogicTask();
        item.setEndTurnPlayer();
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskEnemyEndWait(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setSystem();
        item.systemTime = 0.2f;
        data.bReceiveServerData = false;
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskGetCriticalPoint(LOGICTYPE ltype, BattleData data)
    {
        this.perf.startMovePopObject();
        return new BattleLogicTask[0];
    }

    public BattleLogicTask[] taskLastActorBackStep(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setBackStep();
        list.Add(item);
        return list.ToArray();
    }

    public BattleLogicTask[] taskReaction(BattleLogicTask task)
    {
        BattleLogicTask[] taskArray = new BattleLogicTask[0];
        if (task.status == 0)
        {
            task.status = 1;
            if (task.isDead())
            {
                BattleServantData data = this.data.getServantData(task.getActorId());
                if (!data.isAlive())
                {
                    taskArray = this.logicReaction.createTaskDead(data.getUniqueID());
                }
                return taskArray;
            }
            if (task.isEnemyLogicDead())
            {
                BattleServantData data2 = this.data.getServantData(task.getActorId());
                if (!data2.isAlive())
                {
                    taskArray = this.logicEnemyAi.taskAIDead(data2.getUniqueID());
                }
                return taskArray;
            }
            if (task.isEnemyLogicPlayerActionEnd())
            {
                BattleServantData data3 = this.data.getServantData(task.getActorId());
                if (data3.isAlive())
                {
                    taskArray = this.logicEnemyAi.taskAIPlayerActionEnd(data3.getUniqueID());
                }
                return taskArray;
            }
            if (task.isEnemyLogicStartTurn())
            {
                BattleServantData data4 = this.data.getServantData(task.getActorId());
                if (data4.isAlive())
                {
                    taskArray = this.logicEnemyAi.taskAIEnemyStartTurn(data4.getUniqueID());
                }
                return taskArray;
            }
            if (task.isEnemyLogicEndTurn())
            {
                BattleServantData data5 = this.data.getServantData(task.getActorId());
                if (data5.isAlive())
                {
                    taskArray = this.logicEnemyAi.taskAIEnemyEndTurn(data5.getUniqueID());
                }
                return taskArray;
            }
            if (task.isCheckEntryFunction())
            {
                List<BattleLogicTask> list = new List<BattleLogicTask>();
                foreach (BattleServantData data6 in this.data.getFieldServantList())
                {
                    if (data6.flgEntryFunction)
                    {
                        data6.flgEntryFunction = false;
                        BattleBuffData.BuffData[] dataArray3 = data6.buffData.getEntrySideEffectFunction();
                        if (0 < dataArray3.Length)
                        {
                            foreach (BattleBuffData.BuffData data7 in dataArray3)
                            {
                                BattleSkillInfoData inSkillInfo = new BattleSkillInfoData {
                                    svtUniqueId = data6.getUniqueID(),
                                    skillId = data7.vals[0],
                                    skilllv = data7.vals[1]
                                };
                                BattleLogicTask item = new BattleLogicTask();
                                item.setReservationSkill(inSkillInfo);
                                list.Add(item);
                            }
                        }
                        data6.buffData.usedProgressing();
                    }
                }
                return list.ToArray();
            }
            if (task.isReservationSkill())
            {
                List<BattleLogicTask> list2 = new List<BattleLogicTask>();
                int svtUniqueId = task.skillInfo.svtUniqueId;
                BattleServantData data9 = this.data.getServantData(svtUniqueId);
                if (data9.isAlive() && data9.isUseSkill())
                {
                    int[] ptTargetList = new int[] { data9.getUniqueID() };
                    taskArray = this.logicSkill.taskSkill(task.skillInfo, ptTargetList, null);
                }
            }
        }
        return taskArray;
    }

    public BattleLogicTask[] taskStartEnemyTurn(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setStartTurnEnemy();
        list.Add(item);
        this.data.typeTurn = BattleData.TYPETURN.ENEMY;
        return list.ToArray();
    }

    public BattleLogicTask[] taskStartPlayerTurn(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleLogicTask item = new BattleLogicTask();
        item.setStartTurnPlayer();
        list.Add(item);
        data.bReceiveServerData = true;
        this.data.typeTurn = BattleData.TYPETURN.PLAYER;
        return list.ToArray();
    }

    public BattleLogicTask[] taskStartTactical(LOGICTYPE ltype, BattleData data)
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        foreach (BattleServantData data2 in data.getPlayerServantList())
        {
            data2.isBuffProgressFlg = false;
        }
        foreach (BattleServantData data3 in data.getEnemyServantList())
        {
            data3.isBuffProgressFlg = false;
        }
        this.data.typeTurn = BattleData.TYPETURN.PLAYER;
        return list.ToArray();
    }

    public void turnProgressing(string endproc)
    {
        this.data.turnProgressing();
        this.perf.updateStatus();
        this.fsm.SendEvent(endproc);
    }

    public void useCommandSpell(int commandSkillId, int ptTargetId)
    {
        this.tmp_usecommandspell = new CommandSpellData(commandSkillId, ptTargetId);
        this.fsm.SendEvent("SELECT_COMMANDSPELL");
    }

    public void useSkill()
    {
        List<BattleLogicTask> list = new List<BattleLogicTask>();
        BattleSkillInfoData skillInfo = this.act_useSkill.skillInfo;
        BattleServantData data2 = this.data.getPlayerServantData(skillInfo.svtUniqueId);
        if (data2 != null)
        {
            data2.useSkill(skillInfo);
        }
        else
        {
            this.data.useMasterSkill(skillInfo);
            this.perf.playMasterCutIn();
        }
        this.data.usedSkilllist.Add(new BattleUsedSkills(skillInfo.svtUniqueId, skillInfo.skillId, this.act_useSkill.ptTarget));
        this.perf.playActionSkill();
        this.setTimeAcceleration();
        this.perf.setTargetChangeActive(false);
        list.AddRange(this.logicSkill.taskSkill(skillInfo, this.act_useSkill.ptTarget, null));
        BattleLogicTask item = new BattleLogicTask();
        item.setCheckEntryFunction();
        list.Add(item);
        this.actOneBattleTask(list.ToArray());
    }

    public void wantUseSkill(BattleSkillInfoData skillInfo, int pttarget, int subtarget = -1)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(null);
        if (subtarget == -1)
        {
            this.tmp_useSkill = new UseSkillObject(skillInfo, pttarget);
        }
        else
        {
            this.tmp_useSkill = new UseSkillObject(skillInfo, pttarget, subtarget);
        }
        this.fsm.SendEvent("SELECT_SKILL");
    }

    [HideInInspector]
    public string CurrentBgmName =>
        this._CurrentBgmName;

    [CompilerGenerated]
    private sealed class <checkWaveFunc>c__Iterator1E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleServantData[] <$s_927>__3;
        internal int <$s_928>__4;
        internal BattleBuffData.BuffData[] <$s_929>__7;
        internal int <$s_930>__8;
        internal BattleLogic <>f__this;
        internal BattleBuffData.BuffData <buffData>__9;
        internal BattleBuffData.BuffData[] <buffList>__6;
        internal BuffMaster <buffMst>__0;
        internal BattleSkillInfoData <skillInfo>__10;
        internal BattleServantData <svtData>__5;
        internal BattleServantData[] <svtList>__2;
        internal List<BattleLogicTask> <tmpLogicList>__1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<buffMst>__0 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
                    this.<tmpLogicList>__1 = new List<BattleLogicTask>();
                    this.<svtList>__2 = this.<>f__this.data.getFieldServantList();
                    this.<$s_927>__3 = this.<svtList>__2;
                    this.<$s_928>__4 = 0;
                    while (this.<$s_928>__4 < this.<$s_927>__3.Length)
                    {
                        this.<svtData>__5 = this.<$s_927>__3[this.<$s_928>__4];
                        if (this.<svtData>__5.isAlive())
                        {
                            this.<buffList>__6 = this.<svtData>__5.getStartWaveBuff();
                            this.<$s_929>__7 = this.<buffList>__6;
                            this.<$s_930>__8 = 0;
                            while (this.<$s_930>__8 < this.<$s_929>__7.Length)
                            {
                                this.<buffData>__9 = this.<$s_929>__7[this.<$s_930>__8];
                                this.<skillInfo>__10 = new BattleSkillInfoData();
                                this.<skillInfo>__10.svtUniqueId = this.<svtData>__5.getUniqueID();
                                this.<skillInfo>__10.skillId = this.<buffData>__9.vals[0];
                                this.<skillInfo>__10.skilllv = this.<buffData>__9.vals[1];
                                int[] ptTargetList = new int[] { this.<svtData>__5.getUniqueID() };
                                this.<tmpLogicList>__1.AddRange(this.<>f__this.logicSkill.taskSkill(this.<skillInfo>__10, ptTargetList, null));
                                this.<$s_930>__8++;
                            }
                            this.<svtData>__5.buffData.usedProgressing();
                        }
                        this.<$s_928>__4++;
                    }
                    if (0 >= this.<tmpLogicList>__1.Count)
                    {
                        this.<>f__this.fsm.SendEvent("SKIP");
                        goto Label_0227;
                    }
                    break;

                case 1:
                    break;

                default:
                    goto Label_022E;
            }
            while (this.<>f__this.perf.fieldmotion.isStep())
            {
                this.$current = 0;
                this.$PC = 1;
                return true;
            }
            this.<>f__this.setTimeAcceleration();
            this.<>f__this.actOneBattleTask(this.<tmpLogicList>__1.ToArray());
        Label_0227:
            this.$PC = -1;
        Label_022E:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <colReplaceMember>c__Iterator1C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal System.Action <$>endCallBack;
        internal BattleActionData.ReplaceMember <$>replaceData;
        internal BattleLogic <>f__this;
        internal BattleServantData <svtdata>__0;
        internal System.Action endCallBack;
        internal BattleActionData.ReplaceMember replaceData;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<svtdata>__0 = this.<>f__this.data.getServantData(this.replaceData.outUniqeId);
                    this.<svtdata>__0.isEntry = false;
                    this.<>f__this.perf.deletePlayerActor(this.replaceData.index);
                    ServantAssetLoadManager.unloadServant(this.<svtdata>__0.getSvtId(), this.<svtdata>__0.getDispLimitCount());
                    ServantAssetLoadManager.unloadWeaponGroupEffect(this.<svtdata>__0.getWeaponGroup(), this.<svtdata>__0.getEffectFolder());
                    this.$current = 0;
                    this.$PC = 1;
                    goto Label_01FB;

                case 1:
                    this.<svtdata>__0 = this.<>f__this.data.getServantData(this.replaceData.inUniqeId);
                    this.<svtdata>__0.isEntry = true;
                    this.<svtdata>__0.flgEntryFunction = true;
                    ServantAssetLoadManager.preloadServant(this.<svtdata>__0.getSvtId(), this.<svtdata>__0.getDispLimitCount());
                    ServantAssetLoadManager.preloadActorMotion(this.<svtdata>__0.getWeaponGroup());
                    break;

                case 2:
                    break;

                case 3:
                    this.<>f__this.data.replaceCommandCard(this.replaceData.inUniqeId, this.replaceData.outUniqeId);
                    this.<>f__this.setDrawCard();
                    this.$current = 0;
                    this.$PC = 4;
                    goto Label_01FB;

                case 4:
                    this.endCallBack();
                    this.$PC = -1;
                    goto Label_01F9;

                default:
                    goto Label_01F9;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 2;
            }
            else
            {
                this.<>f__this.perf.loadPlayerActor(this.replaceData.index);
                this.<>f__this.perf.replacePlayerActor(this.replaceData.index);
                this.$current = 0;
                this.$PC = 3;
            }
            goto Label_01FB;
        Label_01F9:
            return false;
        Label_01FB:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineContinueMember>c__Iterator1D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>endproc;
        internal List<BattleServantData>.Enumerator <$s_907>__1;
        internal BattleLogic <>f__this;
        internal int <i>__0;
        internal BattleServantData <svtData>__2;
        internal string endproc;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_907>__1.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<i>__0 = 0;
                    goto Label_0142;

                case 1:
                    break;

                case 2:
                    this.<>f__this.fsm.SendEvent(this.endproc);
                    this.$PC = -1;
                    goto Label_0198;

                default:
                    goto Label_0198;
            }
        Label_0051:
            try
            {
                switch (num)
                {
                    case 1:
                        goto Label_00D3;
                }
                while (this.<$s_907>__1.MoveNext())
                {
                    this.<svtData>__2 = this.<$s_907>__1.Current;
                    if (this.<i>__0 != this.<svtData>__2.index)
                    {
                        continue;
                    }
                    ServantAssetLoadManager.preloadServant(this.<svtData>__2.getSvtId(), this.<svtData>__2.getDispLimitCount());
                    ServantAssetLoadManager.preloadActorMotion(this.<svtData>__2.getWeaponGroup());
                Label_00D3:
                    while (ServantAssetLoadManager.checkLoad())
                    {
                        this.$current = 0;
                        this.$PC = 1;
                        flag = true;
                        goto Label_019A;
                    }
                    this.<>f__this.perf.loadPlayerActor(this.<i>__0);
                    this.<>f__this.perf.continuePlayerActor(this.<i>__0);
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_907>__1.Dispose();
            }
            this.<i>__0++;
        Label_0142:
            if (this.<i>__0 < this.<>f__this.data.p_entryid.Length)
            {
                this.<$s_907>__1 = this.<>f__this.data.player_datalist.GetEnumerator();
                num = 0xfffffffd;
                goto Label_0051;
            }
            this.$current = new WaitForSeconds(1.4f);
            this.$PC = 2;
            goto Label_019A;
        Label_0198:
            return false;
        Label_019A:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineEntrySubMember>c__Iterator1B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>endproc;
        internal BattleLogic <>f__this;
        internal bool <createflg>__0;
        internal int <i>__2;
        internal int <i>__5;
        internal bool <isentry>__1;
        internal BattleServantData <subdata>__4;
        internal BattleServantData <subdata>__7;
        internal BattleServantData <svtdata>__3;
        internal BattleServantData <svtdata>__6;
        internal string endproc;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<createflg>__0 = false;
                    this.<isentry>__1 = false;
                    this.<i>__2 = 0;
                    goto Label_0219;

                case 1:
                    this.<>f__this.data.p_entryid[this.<i>__2] = this.<subdata>__4.getUniqueID();
                    this.<subdata>__4.isEntry = true;
                    this.<subdata>__4.isWaitRepop = true;
                    this.<subdata>__4.isBuffProgressFlg = true;
                    this.<subdata>__4.setDeckIndex(this.<i>__2);
                    ServantAssetLoadManager.preloadServant(this.<subdata>__4.getSvtId(), this.<subdata>__4.getDispLimitCount());
                    ServantAssetLoadManager.preloadActorMotion(this.<subdata>__4.getWeaponGroup());
                    break;

                case 2:
                    break;

                case 3:
                    this.<i>__5 = 0;
                    goto Label_0435;

                case 4:
                    goto Label_03FF;

                default:
                    goto Label_0494;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 2;
                goto Label_0496;
            }
            this.<>f__this.perf.loadPlayerActor(this.<i>__2);
            this.<isentry>__1 = true;
        Label_020B:
            this.<i>__2++;
        Label_0219:
            if (this.<i>__2 < this.<>f__this.data.p_entryid.Length)
            {
                this.<svtdata>__3 = this.<>f__this.data.getPlayerServantData(this.<>f__this.data.p_entryid[this.<i>__2]);
                if ((this.<svtdata>__3 == null) || !this.<svtdata>__3.isDead())
                {
                    goto Label_020B;
                }
                this.<createflg>__0 = true;
                this.<svtdata>__3.setDeadData();
                this.<>f__this.perf.deletePlayerActor(this.<i>__2);
                ServantAssetLoadManager.unloadServant(this.<svtdata>__3.getSvtId(), this.<svtdata>__3.getDispLimitCount());
                ServantAssetLoadManager.unloadWeaponGroupEffect(this.<svtdata>__3.getWeaponGroup(), this.<svtdata>__3.getEffectFolder());
                this.<subdata>__4 = this.<>f__this.data.getPlayerSubServantData();
                if (this.<subdata>__4 == null)
                {
                    this.<>f__this.data.p_entryid[this.<i>__2] = -1;
                    goto Label_020B;
                }
                this.$current = 0;
                this.$PC = 1;
            }
            else
            {
                if (this.<createflg>__0)
                {
                    this.<>f__this.data.createCommandCard();
                    this.<>f__this.data.shuffleCommand();
                }
                this.$current = 0;
                this.$PC = 3;
            }
            goto Label_0496;
        Label_03FF:
            while (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 4;
                goto Label_0496;
            }
            this.<>f__this.perf.loadEnemyActor(this.<i>__5);
            this.<isentry>__1 = true;
        Label_0427:
            this.<i>__5++;
        Label_0435:
            if (this.<i>__5 < this.<>f__this.data.e_entryid.Length)
            {
                this.<svtdata>__6 = this.<>f__this.data.getEnemyServantData(this.<>f__this.data.e_entryid[this.<i>__5]);
                if ((this.<svtdata>__6 == null) || !this.<svtdata>__6.isDead())
                {
                    goto Label_0427;
                }
                this.<svtdata>__6.setDeadData();
                if (this.<svtdata>__6.getSvtId() != ConstantMaster.getValue("FOUR_PILLARS"))
                {
                    this.<>f__this.perf.deleteEnemyActor(this.<i>__5);
                    ServantAssetLoadManager.unloadServant(this.<svtdata>__6.getSvtId(), this.<svtdata>__6.getDispLimitCount());
                    ServantAssetLoadManager.unloadWeaponGroupEffect(this.<svtdata>__6.getWeaponGroup(), this.<svtdata>__6.getEffectFolder());
                }
                this.<subdata>__7 = this.<>f__this.data.getEnemySubServantData();
                if (this.<subdata>__7 == null)
                {
                    this.<>f__this.data.e_entryid[this.<i>__5] = -1;
                    goto Label_0427;
                }
                this.<>f__this.data.e_entryid[this.<i>__5] = this.<subdata>__7.getUniqueID();
                this.<subdata>__7.isEntry = true;
                this.<subdata>__7.isWaitRepop = true;
                ServantAssetLoadManager.preloadServant(this.<subdata>__7.getSvtId(), this.<subdata>__7.getDispLimitCount());
                ServantAssetLoadManager.preloadActorMotion(this.<subdata>__7.getWeaponGroup());
                goto Label_03FF;
            }
            if (this.<isentry>__1)
            {
                this.<>f__this.fsm.SendEvent("OK");
            }
            else
            {
                this.<>f__this.fsm.SendEvent(this.endproc);
            }
            this.$PC = -1;
        Label_0494:
            return false;
        Label_0496:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineInitQuest>c__Iterator20 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>endproc;
        internal BattleLogic <>f__this;
        internal string endproc;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.data.initQuest();
                    this.<>f__this.actPtPassiveSkill();
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_008C;

                case 3:
                    goto Label_00CE;

                case 4:
                    this.<>f__this.sendFsmEvent(this.endproc);
                    this.$PC = -1;
                    goto Label_011D;

                default:
                    goto Label_011D;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 1;
                goto Label_011F;
            }
        Label_008C:
            while (!this.<>f__this.spawner.HasCached())
            {
                this.$current = 0;
                this.$PC = 2;
                goto Label_011F;
            }
            this.<>f__this.perf.initQuest();
        Label_00CE:
            while (this.<>f__this.perf.bgPerf.IsBusy)
            {
                this.$current = 0;
                this.$PC = 3;
                goto Label_011F;
            }
            GC.Collect();
            this.$current = 0;
            this.$PC = 4;
            goto Label_011F;
        Label_011D:
            return false;
        Label_011F:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineLoadNstage>c__Iterator22 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>endproc;
        internal BattleLogic <>f__this;
        internal string endproc;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.data.loadNstage(this.<>f__this.data.wavecount);
                    break;

                case 1:
                    break;

                case 2:
                    this.<>f__this.fsm.SendEvent(this.endproc);
                    this.$PC = -1;
                    goto Label_00C1;

                default:
                    goto Label_00C1;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 1;
            }
            else
            {
                this.<>f__this.perf.loadNStage(this.<>f__this.data.wavecount);
                this.$current = 0;
                this.$PC = 2;
            }
            return true;
        Label_00C1:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineLoadQuest>c__Iterator21 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleLogic <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.data.loadQuest();
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_007D;

                case 3:
                    this.<>f__this.sendFsmEvent("END_PROC");
                    this.$PC = -1;
                    goto Label_00D6;

                default:
                    goto Label_00D6;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 1;
                goto Label_00D8;
            }
        Label_007D:
            while (!this.<>f__this.spawner.HasCached())
            {
                this.$current = 0;
                this.$PC = 2;
                goto Label_00D8;
            }
            this.<>f__this.perf.initQuest();
            GC.Collect();
            this.$current = 0;
            this.$PC = 3;
            goto Label_00D8;
        Label_00D6:
            return false;
        Label_00D8:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <coroutineLoadSaveWave>c__Iterator23 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleServantData[] <$s_1018>__1;
        internal int <$s_1019>__2;
        internal BattleSkillInfoData[] <$s_1020>__5;
        internal int <$s_1021>__6;
        internal BattleLogic <>f__this;
        internal BattleSkillInfoData <skillInfo>__7;
        internal BattleSkillInfoData[] <skillInfoList>__4;
        internal BattleServantData <svtData>__3;
        internal BattleServantData[] <svtDataList>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.data.loadSaveTurnNstage();
                    break;

                case 1:
                    break;

                case 2:
                    BattleRandom.adjustment();
                    if (((this.<>f__this.data.tutorialId == 2) && (this.<>f__this.data.turnCount == 2)) && (this.<>f__this.data.wavecount == 0))
                    {
                        this.<svtDataList>__0 = this.<>f__this.data.getFieldPlayerServantList();
                        this.<$s_1018>__1 = this.<svtDataList>__0;
                        this.<$s_1019>__2 = 0;
                        while (this.<$s_1019>__2 < this.<$s_1018>__1.Length)
                        {
                            this.<svtData>__3 = this.<$s_1018>__1[this.<$s_1019>__2];
                            if (this.<svtData>__3 != null)
                            {
                                this.<skillInfoList>__4 = this.<svtData>__3.getActiveSkillInfos();
                                this.<$s_1020>__5 = this.<skillInfoList>__4;
                                this.<$s_1021>__6 = 0;
                                while (this.<$s_1021>__6 < this.<$s_1020>__5.Length)
                                {
                                    this.<skillInfo>__7 = this.<$s_1020>__5[this.<$s_1021>__6];
                                    if (this.<skillInfo>__7.getChargeTurn() > 0)
                                    {
                                        this.<>f__this.data.tutorialState = 10;
                                        break;
                                    }
                                    this.<$s_1021>__6++;
                                }
                            }
                            this.<$s_1019>__2++;
                        }
                    }
                    this.<>f__this.fsm.SendEvent("END_PROC");
                    this.$PC = -1;
                    goto Label_01E8;

                default:
                    goto Label_01E8;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 1;
            }
            else
            {
                this.<>f__this.perf.loadNStage(this.<>f__this.data.wavecount);
                this.$current = 0;
                this.$PC = 2;
            }
            return true;
        Label_01E8:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <debugStartTips>c__Iterator1F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(10f);
                    this.$PC = 1;
                    return true;

                case 1:
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    private enum ACTTYPE
    {
        NONE,
        LIST,
        ONE
    }

    private class BuffInfoData
    {
        public string buffList;
        public int svtId;
        public int uniqueId;
    }

    private class CommandSpellData
    {
        public int commandSkillId;
        public int ptTargetId;

        public CommandSpellData(int id, int tg)
        {
            this.commandSkillId = id;
            this.ptTargetId = tg;
        }
    }

    private delegate BattleActionData createActionBattle(BattleLogicTask task);

    public enum DamageType
    {
        NOMAL,
        NOBLE_NOMAL,
        NOBLE_PIERCE,
        NOBLE_INDIVIDUAL,
        NOBLE_STATE_INDIVIDUAL,
        NOBLE_HPRATIO_HIGH,
        NOBLE_HPRATIO_LOW
    }

    private enum DebugType
    {
        CRITICAL_FULL = 6,
        ENEMY_DYING = 1,
        ENEMY_NPFULL = 7,
        ENEMY_NPHASTE = 8,
        ENEMY_RECOVER = 3,
        PLAYER_DYING = 2,
        PLAYER_NPFULL = 5,
        PLAYER_RECOVER = 4,
        PLAYER_RECOVER_ONE = 9
    }

    public enum LOGICTYPE
    {
        COMMAND_BEFORE,
        COMMAND_ATTACK_1,
        COMMAND_ATTACK_2,
        COMMAND_ATTACK_3,
        COMMAND_ADDATTACK,
        COMMAND_AFTER,
        PLAYER_SPECIAL_1,
        PLAYER_SPECIAL_2,
        PLAYER_SPECIAL_3,
        COMMAND_WAIT,
        ENEMY_ATTACK_1,
        ENEMY_ATTACK_2,
        ENEMY_ATTACK_3,
        LAST_BACKSTEP,
        ENEMY_SPECIAL_1,
        ENEMY_SPECIAL_2,
        ENEMY_SPECIAL_3,
        DEBUG,
        PLAYER_SP1_ALWAYS,
        PLAYER_SP2_ALWAYS,
        PLAYER_SP3_ALWAYS,
        ENEMY_SP1_ALWAYS,
        ENEMY_SP2_ALWAYS,
        ENEMY_SP3_ALWAYS,
        GET_DROPITEM,
        PLAYER_ENDTURN,
        ENEMY_ENDTURN,
        TOTAL_ENDTURN,
        AFTER_SKILL,
        AFTER_COMMAND,
        REACTION_STARTENEMY,
        REACTION_ENDENEMY,
        ENEMY_ENDWAIT,
        REACTION_PLAYERACTIONEND,
        BUFF_ADDPARAM_PLAYER,
        BUFF_ADDPARAM_ENEMY,
        START_PLAYERTURN,
        START_ENEMYTURN,
        TACTICAL_START
    }

    private delegate BattleLogicTask[] taskFunction(BattleLogic.LOGICTYPE ltype, BattleData data);

    public enum TuStates
    {
        Attack,
        Skill,
        SelectEnemy
    }

    public class TutorialStringData
    {
        public Vector2 pos;
        public int size;
        public float way;

        public TutorialStringData(float way, Vector2 pos, int size)
        {
            this.way = way;
            this.pos = pos;
            this.size = size;
        }
    }

    public class UseSkillObject
    {
        public int[] ptTarget;
        public BattleSkillInfoData skillInfo;

        public UseSkillObject(BattleSkillInfoData skillInfo)
        {
            this.skillInfo = skillInfo;
            this.ptTarget = new int[] { -1 };
        }

        public UseSkillObject(BattleSkillInfoData skillInfo, int pttarget)
        {
            this.skillInfo = skillInfo;
            this.ptTarget = new int[] { pttarget };
        }

        public UseSkillObject(BattleSkillInfoData skillInfo, int pttarget, int subtarget)
        {
            this.skillInfo = skillInfo;
            this.ptTarget = new int[] { pttarget, subtarget };
        }
    }
}

