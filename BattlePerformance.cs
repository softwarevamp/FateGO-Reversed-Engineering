using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattlePerformance : BaseMonoBehaviour
{
    protected static bool _never;
    [CompilerGenerated]
    private static System.Action <>f__am$cache3E;
    public Camera actorcamera;
    public GameObject actorprefab;
    public GameObject attackButton;
    protected System.Action BgLoadCallback;
    public BattlePerformanceBg bgPerf;
    public static bool CameraFlip;
    public PlayMakerFSM camerafsm;
    public BattlePerformanceCommandCard commandPerf;
    public GameObject commonMotionPrefab;
    public BattlePerformanceContinue contPerf;
    public float currentFov = DefaultFov;
    private int currentGroundNo;
    private int currentGroundType;
    public Camera cutIncamera;
    public Transform cutinRootTr;
    public Transform cutinTargetTr;
    public BattleData data;
    public static readonly float DefaultFov = 40f;
    public float distanceofactor = 1.5f;
    private GameObject[] e_actorlist;
    public Transform[] e_actorTr;
    public BattleEffectControl effectcontrol;
    public GameObject enemyStage;
    public GameObject fadeBlackObject;
    public GameObject fadeObject;
    public Transform fadeoverTr;
    public BattleFieldEffectComponent fieldEffect;
    public BattleFieldMotionComponent fieldmotion;
    public PlayMakerFSM fieldmotionfsm;
    [SerializeField]
    protected List<float> FovTable;
    public PlayMakerFSM fsm;
    public BattleHitStopControl hitstopcontrol;
    public BattleInformationComponent infoComp;
    private bool istactical;
    private bool isTargetChange = true;
    public Transform itemintoTr;
    public BattleLogic logic;
    public GameObject masterRoot;
    public Camera middleCamera;
    private BattleActionData nowAction;
    private GameObject[] p_actorlist;
    public Transform[] p_actorTr;
    public UIPanel[] panelList;
    public BattlePerformance perf;
    public GameObject playerStage;
    protected string PlaySeName;
    public Transform pop_enemyTr;
    public Transform pop_playerTr;
    private List<GameObject> popitemlist = new List<GameObject>();
    public Transform popupTr;
    private PositionMode positionMode = PositionMode.TACTICAL;
    private Queue<BattleActionData> queue_action = new Queue<BattleActionData>();
    public BattleResultComponent resultwindow;
    private GameObject roleobject;
    public Transform root_drop;
    public Transform root_field;
    public List<GameObject> StandFigures;
    public BattlePerformanceStatus statusPerf;
    public GameObject tacticBg;
    public Camera uicamera;
    public Transform uirootTr;

    public void addActionData(BattleActionData[] adddatalist)
    {
        if (adddatalist != null)
        {
            for (int i = 0; i < adddatalist.Length; i++)
            {
                this.queue_action.Enqueue(adddatalist[i]);
            }
        }
    }

    public void addActionData(BattleActionData adddata)
    {
        Debug.Log("addActionData << adddata");
        if (adddata != null)
        {
            Debug.Log("add");
            this.queue_action.Enqueue(adddata);
        }
    }

    public void addCriticalPont(int count)
    {
        this.data.addCriticalPoint(count);
        this.statusPerf.updateCriticalPoint();
    }

    public void callNpDamageVoice()
    {
        int[] damageTargets = this.getActionData().GetDamageTargets(-1);
        if (damageTargets.Length <= 0)
        {
            Debug.Log("no damage target");
        }
        else
        {
            int uniqueId = 0;
            int index = UnityEngine.Random.Range(0, damageTargets.Length);
            if (index < damageTargets.Length)
            {
                uniqueId = damageTargets[index];
            }
            GameObject obj2 = this.getServantGameObject(uniqueId);
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().playVoice(Voice.BATTLE.HDAMAGE1, 1f, null);
            }
        }
    }

    public void changeAttackButton(bool flg, bool use, bool card)
    {
        if (flg)
        {
            TweenColor.Begin(this.attackButton, 0.3f, Color.white);
        }
        else
        {
            TweenColor.Begin(this.attackButton, 0.3f, Color.clear);
        }
        this.attackButton.GetComponent<Collider>().enabled = use;
        this.commandPerf.hideCommandCard(card);
    }

    public void changeBg(int No, int tp, Vector3 pos, Vector3 rot, bool changeDirect = false, bool parentCamera = false, System.Action callback = null)
    {
        this.BgLoadCallback = callback;
        this.bgPerf.changeBg(No, tp, pos, rot, changeDirect, parentCamera, new System.Action(this.OnEndChangeBg));
    }

    public int checkActionCount() => 
        this.queue_action.Count;

    public void checkRedrawCommandCard()
    {
        BattleActionData data = this.getActionData();
        if ((data != null) && data.redrawCommandCard)
        {
            this.commandPerf.openCommandCard();
        }
    }

    public bool checkTimeScaleDead(BattleServantData svt) => 
        svt.checkScriptValue("speed1_dead", 1);

    public bool checkTimeScaleTreasureDevice(BattleServantData svt) => 
        svt.checkScriptValue("speed1_treasure_device", 1);

    public void clickTarget(int index)
    {
        this.logic.checkSelectEnemyClick(index);
        if ((((!this.data.isTutorialclickTarget() && this.statusPerf.masterPerf.isCloseEnemyConf()) && !this.statusPerf.playerPerf.isOpenSvtConf()) && this.isTargetChange) && (this.istactical && this.logic.setTargetIndex(index)))
        {
            SoundManager.playSe("ba18");
        }
    }

    public void closeSelectMainSubSvtWindow()
    {
        this.getSelectMainSubSvtWindow().Close(null);
    }

    public void closeSelectSvtWindow()
    {
        this.getSelectSvtWindow().Close(null);
    }

    public void continuePlayerActor(int index)
    {
        BattleActorControl component = this.p_actorlist[index].GetComponent<BattleActorControl>();
        component.setTargetObject(this.fieldmotion.getPlayerPopTr(index).gameObject);
        component.playMotion("MOTION_RECOVER");
    }

    [DebuggerHidden]
    private IEnumerator corAfterMultiHit(BattleActionData actData, int index, int count, int funcIndex, bool isRandomDamage, Vector3 damageNumberPosition, bool isNoDamageMotion) => 
        new <corAfterMultiHit>c__Iterator28 { 
            count = count,
            funcIndex = funcIndex,
            actData = actData,
            index = index,
            isRandomDamage = isRandomDamage,
            damageNumberPosition = damageNumberPosition,
            isNoDamageMotion = isNoDamageMotion,
            <$>count = count,
            <$>funcIndex = funcIndex,
            <$>actData = actData,
            <$>index = index,
            <$>isRandomDamage = isRandomDamage,
            <$>damageNumberPosition = damageNumberPosition,
            <$>isNoDamageMotion = isNoDamageMotion,
            <>f__this = this
        };

    private GameObject createPopEffect(BattleActorControl actor, BattleEffectControl.ID id)
    {
        GameObject obj2 = this.getEffectInstantiate(id, this.popupTr);
        TrackingMoveCtCComponent component = obj2.GetComponent<TrackingMoveCtCComponent>();
        if (component == null)
        {
            component = obj2.AddComponent<TrackingMoveCtCComponent>();
        }
        component.Set(this.perf.actorcamera, this.perf.uicamera, actor.getHeadUpObject(), actor.getHeadUpY());
        component.startAct();
        return obj2;
    }

    private void DebugLog(string str)
    {
    }

    [DebuggerHidden]
    private IEnumerator delayDestory(GameObject obj, float time) => 
        new <delayDestory>c__Iterator2C { 
            time = time,
            obj = obj,
            <$>time = time,
            <$>obj = obj
        };

    public bool deleteEnemyActor(int index)
    {
        if (this.e_actorlist[index] != null)
        {
            UnityEngine.Object.Destroy(this.e_actorlist[index]);
            return true;
        }
        return false;
    }

    public bool deletePlayerActor(int index)
    {
        if (this.p_actorlist[index] != null)
        {
            UnityEngine.Object.Destroy(this.p_actorlist[index]);
            this.statusPerf.deletePlayerStatus(index);
            return true;
        }
        return false;
    }

    public void destroyInstantiate(GameObject obj)
    {
        this.effectcontrol.destroyInstantiate(obj);
    }

    public void dropCriticalPoint(Transform tr)
    {
        GameObject obj2 = this.getEffectInstantiate(BattleEffectControl.ID.STAR, this.root_drop);
        obj2.transform.ChangeChildsLayer(LayerMask.NameToLayer("Battle2D"));
        obj2.transform.position = tr.position;
        Transform transform = obj2.transform;
        transform.localPosition += new Vector3(UnityEngine.Random.Range((float) -0.3f, (float) 0.3f), UnityEngine.Random.Range((float) 0.4f, (float) 1.2f));
        Animation componentInChildren = obj2.GetComponentInChildren<Animation>();
        if (componentInChildren != null)
        {
            componentInChildren["critobj01"].time = UnityEngine.Random.Range(0f, componentInChildren.clip.length);
            float num = UnityEngine.Random.Range((float) 0.5f, (float) 2f);
            IEnumerator enumerator = componentInChildren.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    current.speed = num;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }
        Rigidbody component = obj2.GetComponent<Rigidbody>();
        component.velocity = new Vector3(UnityEngine.Random.Range((float) -1f, (float) 1f), UnityEngine.Random.Range((float) 2f, (float) 4f), UnityEngine.Random.Range((float) -1f, (float) 1f));
        component.useGravity = true;
        BattleMoveObject obj3 = obj2.GetComponent<BattleMoveObject>();
        obj3.setAction(tr.position);
        obj3.setTargetTr(this.commandPerf.getCollectCriticalTransform());
        obj3.setParam(this, 1);
        this.setPopObject(obj2);
    }

    public void dropGetItem(Transform tr, DropInfo dropInfo, int max, int index)
    {
        GameObject obj2 = null;
        obj2 = this.effectcontrol.getTreasureObject(dropInfo.rarity, this.root_drop);
        if (obj2 == null)
        {
            Debug.Log("No Object");
        }
        else
        {
            if (0x3e8 <= dropInfo.rarity)
            {
                ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(dropInfo.objectId);
                UISprite componentInChildren = obj2.GetComponentInChildren<UISprite>();
                if (componentInChildren != null)
                {
                    AtlasManager.SetItem(componentInChildren, entity.imageId);
                }
            }
            obj2.transform.ChangeChildsLayer(LayerMask.NameToLayer("Battle2D"));
            obj2.transform.position = tr.position;
            Transform transform = obj2.transform;
            transform.localPosition += new Vector3((UnityEngine.Random.Range((float) -0.1f, (float) 0.1f) + (index * 0.4f)) - ((max - 1) * 0.2f), UnityEngine.Random.Range((float) 0.4f, (float) 1.2f));
            BattleMoveObject obj3 = obj2.AddComponent<BattleMoveObject>();
            obj3.setAction(tr.position);
            obj3.setParam(this, 0);
            obj3.setTypeItem();
            this.setPopObject(obj2);
        }
    }

    public void dropGetServant(Transform tr, DropInfo dropInfo)
    {
        GameObject obj2 = this.effectcontrol.getTreasureObject(dropInfo.rarity, this.root_drop);
        if (obj2 == null)
        {
            Debug.Log("No Object");
        }
        else
        {
            obj2.transform.ChangeChildsLayer(LayerMask.NameToLayer("Battle2D"));
            obj2.transform.position = tr.position;
            Transform transform = obj2.transform;
            transform.localPosition += new Vector3(UnityEngine.Random.Range((float) -0.3f, (float) 0.3f), UnityEngine.Random.Range((float) 0.4f, (float) 1.2f));
            BattleMoveObject obj3 = obj2.AddComponent<BattleMoveObject>();
            obj3.setAction(tr.position);
            obj3.setParam(this, 0);
            obj3.setTypeItem();
            this.setPopObject(obj2);
        }
    }

    public void effectBlack()
    {
        this.fadeBlackObject.transform.parent.gameObject.SetActive(true);
        this.fadeBlackObject.transform.localPosition = new Vector3(0f, 0f);
        this.fadeBlackObject.GetComponent<Animation>().Play("black");
    }

    public void effectClearQuest()
    {
        this.endProc();
    }

    public void effectFadeIn()
    {
        this.fadeBlackObject.transform.parent.gameObject.SetActive(true);
        this.fadeBlackObject.transform.localPosition = new Vector3(0f, 0f);
        this.fadeBlackObject.GetComponent<Animation>().Play("black_out");
    }

    public void effectFadeOut()
    {
        this.fadeBlackObject.transform.parent.gameObject.SetActive(true);
        this.fadeBlackObject.transform.localPosition = new Vector3(0f, 0f);
        this.fadeBlackObject.GetComponent<Animation>().Play("black_in");
    }

    public void effectLoseBattle()
    {
        this.endProc();
    }

    public void effectNextBattle()
    {
        base.StartCoroutine(this.waitEndProc(null, 1.5f));
    }

    public void effectOverQuest()
    {
        this.effectFadeOut();
        base.StartCoroutine(this.waitEndProc(null, 1.5f));
    }

    public void effectStartBattle()
    {
        this.effectFadeIn();
        if (this.data.isResumeBattle())
        {
            SoundManager.playSe("ba2");
            this.fieldmotion.sendEvent("RESUME");
        }
        else if (this.data.isLastStage())
        {
            SoundManager.playSe("ba2");
            this.fieldmotion.sendEvent("QUEST_BOSS");
        }
        else if (this.data.isFirstStage())
        {
            SoundManager.playSe("ba1");
            this.fieldmotion.sendEvent("QUEST_START");
        }
        else
        {
            SoundManager.playSe("ba1");
            this.fieldmotion.sendEvent("QUEST_ROUTE");
        }
    }

    public void effectStartQuest()
    {
        this.statusPerf.updateView();
        this.statusPerf.setShowTurn(this.data, 0);
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
        BattleEntity entity = this.data.getBattleEntity();
        int questId = entity.questId;
        int questPhase = entity.questPhase;
        int wavecount = this.data.wavecount;
        int startEffectId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STAGE).getEntityFromId<StageEntity>(questId, questPhase, wavecount + 1).startEffectId;
        if (this.fieldmotion.myFsm.Fsm.Variables.FindFsmInt("StartEffectId") != null)
        {
            this.fieldmotion.myFsm.Fsm.Variables.FindFsmInt("StartEffectId").Value = startEffectId;
        }
        GameObject obj2 = base.createObject($"effect/BitEffect/bit_battlestart_3_{startEffectId:0}", this.fadeoverTr, null);
        GameObject gameObject = this.fadeoverTr.gameObject;
        gameObject.SetActive(true);
        obj2.AddComponent<BattlePanelEvent>().Panel = gameObject;
        Transform transform = obj2.transform.getNodeFromName("Label_numerator", true);
        Transform transform2 = obj2.transform.getNodeFromName("Label_donominator", true);
        if ((transform != null) && (transform2 != null))
        {
            UILabel component = transform.GetComponent<UILabel>();
            UILabel label2 = transform2.GetComponent<UILabel>();
            component.text = (wavecount + 1).ToString();
            label2.text = (this.data.getLastWave() + 1).ToString();
        }
        this.statusPerf.modeStartWave();
        base.StartCoroutine(this.waitEndProc(obj2, 1.5f));
    }

    public void effectWinBattle()
    {
        this.fadeoverTr.gameObject.SetActive(true);
        GameObject obj2 = base.createObject("effect/BitEffect/bit_battlefinish01", this.fadeoverTr, null);
        base.StartCoroutine(this.delayDestory(obj2, 5.5f));
    }

    public void endActionData()
    {
        if (this.nowAction.isEndCamera())
        {
            this.camerafsm.SendEvent(this.nowAction.getEndCamera());
        }
        BattleServantData svt = this.data.getServantData(this.nowAction.actorId);
        if (this.nowAction.type == BattleActionData.TYPE_DEAD)
        {
            if (this.checkTimeScaleDead(svt))
            {
                this.logic.setTimeAcceleration();
            }
        }
        else if ((this.nowAction.type == BattleActionData.TYPE_TW) && this.checkTimeScaleTreasureDevice(svt))
        {
            this.logic.setTimeAcceleration();
        }
        this.playActionData(null);
    }

    public void endActionSkill()
    {
        this.statusPerf.endSkill();
        this.changeAttackButton(true, true, true);
    }

    public void endCommandSpell()
    {
        this.statusPerf.endSkill();
        this.changeAttackButton(true, true, true);
    }

    public void endNoblePhantasm()
    {
        if (this.data.systemflg_TdConstantvelocity)
        {
            this.logic.setTimeAcceleration();
        }
    }

    public void endProc()
    {
        if (this.fsm != null)
        {
            this.fsm.SendEvent("END_PROC");
        }
    }

    public void endSelectAbilityButton()
    {
        this.logic.endSelectSkillFaze();
    }

    public void endSelectCommand()
    {
        this.tacticBg.gameObject.SetActive(false);
    }

    protected GameObject FetchStandFigure(int svtId, int limitCount, bool removeFromList = true)
    {
        GameObject item = null;
        foreach (GameObject obj3 in this.StandFigures)
        {
            UIStandFigureR component = obj3.GetComponent<UIStandFigureR>();
            if (component != null)
            {
                int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
                if ((component.SvtId == svtId) && (component.ImageLimitCountValue == imageLimitCount))
                {
                    item = obj3;
                    break;
                }
            }
        }
        if ((item != null) && removeFromList)
        {
            this.StandFigures.Remove(item);
            item.SetActive(true);
        }
        return item;
    }

    private void FingerGestures_OnGestureEvent(Gesture gesture)
    {
        GameObject startSelection = gesture.StartSelection;
        if ((startSelection != null) && (gesture is SwipeGesture))
        {
            startSelection.SendMessage("OnSwipe", (SwipeGesture) gesture, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void FlipAll(bool isFlip)
    {
        CameraFlip = isFlip;
        if (isFlip)
        {
            this.fieldmotionfsm.SendEvent("START_NP_ENEMY");
            if (this.bgPerf.bgobject != null)
            {
                this.bgPerf.bgobject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else
        {
            this.fieldmotionfsm.SendEvent("START_NP_PLAYER");
            if (this.bgPerf.bgobject != null)
            {
                this.bgPerf.bgobject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    public BattleActionData getActionData() => 
        this.nowAction;

    public int getAmbientColors() => 
        this.fieldEffect.getAmbientColors();

    public Dictionary<string, bool> getCameraEffectStatus()
    {
        Camera middleCamera = this.middleCamera;
        if (middleCamera == null)
        {
            return null;
        }
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
        Bloom component = middleCamera.GetComponent<Bloom>();
        if (component != null)
        {
            dictionary["Bloom"] = component.enabled;
        }
        MotionBlur blur = middleCamera.GetComponent<MotionBlur>();
        if (blur != null)
        {
            dictionary["MotionBlur"] = blur.enabled;
        }
        Vignetting vignetting = middleCamera.GetComponent<Vignetting>();
        if (vignetting != null)
        {
            dictionary["Vignetting"] = vignetting.enabled;
        }
        GrayscaleEffect effect = middleCamera.GetComponent<GrayscaleEffect>();
        if (effect != null)
        {
            dictionary["GrayscaleEffect"] = effect.enabled;
        }
        return dictionary;
    }

    public string[] getChangeBgList() => 
        this.bgPerf.getChangeBgList();

    public GameObject getEffectInstantiate(BattleEffectControl.ID id, Transform parent)
    {
        GameObject obj2 = this.effectcontrol.getEffectInstantiate(id);
        obj2.transform.SetParent(parent, false);
        return obj2;
    }

    public string[] getFieldEffects() => 
        this.fieldEffect.getFieldEffects();

    public GameObject getListEffect(BattleEffectControl.ID id) => 
        this.effectcontrol.getListEffect(id);

    public float GetMaxFov()
    {
        if ((this.FovTable == null) || (this.FovTable.Count == 0))
        {
            List<float> list = new List<float> { 
                40f,
                40f,
                40f,
                50f
            };
            this.FovTable = list;
        }
        float defaultFov = DefaultFov;
        foreach (BattleServantData data in this.data.player_datalist)
        {
            float num2 = this.FovTable[data.npcSvtType];
            if (num2 > defaultFov)
            {
                defaultFov = num2;
            }
        }
        foreach (BattleServantData data2 in this.data.e_svlist)
        {
            float num4 = this.FovTable[data2.npcSvtType];
            if (num4 > defaultFov)
            {
                defaultFov = num4;
            }
        }
        return defaultFov;
    }

    public UIPanel getPanel(int index)
    {
        if ((this.panelList != null) && (index < this.panelList.Length))
        {
            return this.panelList[index];
        }
        return null;
    }

    public Transform getRepopEnemyPos(int index) => 
        this.fieldmotion.getEnemyPopPoint(index);

    public Transform getRepopPlayerPos(int index) => 
        this.fieldmotion.getPlayerPopTr(index);

    public BattleSelectMainSubServantWindow getSelectMainSubSvtWindow() => 
        this.statusPerf.getSelectMainSubSvtWindow();

    public BattleSelectServantWindow getSelectSvtWindow() => 
        this.statusPerf.getSelectSvtWindow();

    public GameObject getServantGameObject(int uniqueId)
    {
        BattleActorControl component;
        for (int i = 0; i < this.p_actorlist.Length; i++)
        {
            if (this.p_actorlist[i] != null)
            {
                component = this.p_actorlist[i].GetComponent<BattleActorControl>();
                if ((component != null) && component.checkID(uniqueId))
                {
                    return this.p_actorlist[i];
                }
            }
        }
        for (int j = 0; j < this.e_actorlist.Length; j++)
        {
            if (this.e_actorlist[j] != null)
            {
                component = this.e_actorlist[j].GetComponent<BattleActorControl>();
                if ((component != null) && component.checkID(uniqueId))
                {
                    return this.e_actorlist[j];
                }
            }
        }
        return null;
    }

    public void hideOverKillMessage()
    {
        this.infoComp.hideOverKillMessage();
    }

    public void hideRUComboCutIn()
    {
        if (this.roleobject != null)
        {
            UnityEngine.Object.Destroy(this.roleobject);
        }
        this.roleobject = null;
    }

    public void initActionBattle()
    {
        Debug.Log("playActionBattle____**********");
        for (int i = 0; i < this.p_actorlist.Length; i++)
        {
            if (this.p_actorlist[i] != null)
            {
                this.p_actorlist[i].GetComponent<BattleActorControl>().setInitActionBattle();
            }
        }
        for (int j = 0; j < this.e_actorlist.Length; j++)
        {
            if (this.e_actorlist[j] != null)
            {
                BattleActorControl component = this.e_actorlist[j].GetComponent<BattleActorControl>();
                component.setCriteriaPos();
                component.setInitActionBattle();
            }
        }
        this.resetPopObject();
        Debug.Log("**********____playActionBattle");
    }

    public void InitActorPosition()
    {
        this.fieldmotionfsm.SendEvent("END_NP");
    }

    public void InitAmbientLight()
    {
        RenderSettings.ambientLight = Color.white;
    }

    public void Initialize()
    {
        this.fieldmotionfsm.Fsm.Variables.GetFsmGameObject("CameraFsm").Value = this.camerafsm.gameObject;
        this.fieldmotionfsm.Fsm.Variables.GetFsmGameObject("BattleFSM").Value = this.fsm.gameObject;
        this.fieldmotion.setPerf(this);
        this.camerafsm.FsmVariables.GetFsmGameObject("FadeObject").Value = this.fadeObject;
        this.commandPerf.Initialize(this, this.data, this.logic);
        this.commandPerf.createCommandCard();
        this.statusPerf.Initialize(this, this.data, this.logic);
        this.infoComp.Initialize(this, this.data, this.logic);
        this.contPerf.Initialize(this, this.data, this.logic);
        if (this.p_actorlist != null)
        {
            for (int i = 0; i < this.p_actorlist.Length; i++)
            {
                if (this.p_actorlist[i] != null)
                {
                    UnityEngine.Object.Destroy(this.p_actorlist[i]);
                }
                this.p_actorlist[i] = null;
            }
        }
        if (this.e_actorlist != null)
        {
            for (int j = 0; j < this.e_actorlist.Length; j++)
            {
                if (this.e_actorlist[j] != null)
                {
                    UnityEngine.Object.Destroy(this.e_actorlist[j]);
                }
                this.e_actorlist[j] = null;
            }
        }
        this.e_actorlist = new GameObject[3];
        if (this.roleobject != null)
        {
            UnityEngine.Object.Destroy(this.roleobject);
            this.roleobject = null;
        }
        this.resultwindow.Init();
        ServantAssetLoadManager.clearServantlist();
        this.StandFigures = new List<GameObject>();
    }

    public void initQuest()
    {
        Debug.Log("initQuest>>");
        this.loadMaster();
        this.commandPerf.initQuest();
        this.masterRoot.SetActive(true);
        if (this.data.tutorialId == 1)
        {
            this.masterRoot.SetActive(false);
        }
        this.p_actorlist = new GameObject[3];
        for (int i = 0; i < this.p_actorlist.Length; i++)
        {
            this.loadPlayerActor(i);
        }
        this.currentGroundNo = this.data.getGroundNo();
        this.currentGroundType = this.data.getGroundType();
        this.loadBg(this.currentGroundNo, this.currentGroundType);
        this.updateDropItemCount();
    }

    [DebuggerHidden]
    private IEnumerator InvokeAction(System.Action func, float waitTime) => 
        new <InvokeAction>c__Iterator29 { 
            waitTime = waitTime,
            func = func,
            <$>waitTime = waitTime,
            <$>func = func
        };

    public bool isPositionBattle() => 
        (this.positionMode == PositionMode.BATTLE);

    public bool isPositionTactical() => 
        (this.positionMode == PositionMode.TACTICAL);

    public void loadBg(int No, int tp = 0)
    {
        this.bgPerf.loadBg(No, tp);
    }

    public bool loadEnemyActor(int index)
    {
        this.deleteEnemyActor(index);
        int id = this.data.e_entryid[index];
        Debug.Log("actorId:" + id);
        if (id < 0)
        {
            this.statusPerf.deleteEnemyStatus(index);
            return false;
        }
        BattleServantData svtdata = this.data.getEnemyServantData(id);
        this.e_actorlist[index] = base.createObject(this.actorprefab, this.root_field, null);
        this.e_actorlist[index].transform.position = this.fieldmotion.getPopEnemy().position;
        this.fieldmotionfsm.Fsm.Variables.GetFsmGameObject("ENEMY" + (index + 1)).Value = this.e_actorlist[index];
        BattleActorControl component = this.e_actorlist[index].GetComponent<BattleActorControl>();
        component.setPerformance(this);
        component.setTypeEnemy();
        component.setServantData(svtdata);
        component.setDirRight();
        component.setCriteriaPos();
        component.setID(id);
        component.setCamera(this.actorcamera);
        component.setMotionlist("_ENEMY", this.actorcamera.gameObject, this.camerafsm.gameObject);
        component.setMyStage(this.enemyStage);
        component.setEnemyStage(this.playerStage);
        svtdata.resetParamObject();
        svtdata.addParamObject(this.e_actorlist[index]);
        this.statusPerf.setEnemyParam(index, this.data.getEnemyServantData(id), this.e_actorlist[index]);
        return true;
    }

    public void loadMaster()
    {
        this.statusPerf.loadMaster();
    }

    public void loadNStage(int stagecount)
    {
        Debug.Log("loadNStage");
        for (int i = 0; i < this.data.e_entryid.Length; i++)
        {
            this.loadEnemyActor(i);
        }
        float maxFov = this.GetMaxFov();
        this.setupCameraFov(maxFov);
        this.updateView();
        this.UpdateAllShadow();
    }

    public bool loadPlayerActor(int index)
    {
        if (this.p_actorlist[index] != null)
        {
            UnityEngine.Object.Destroy(this.p_actorlist[index]);
        }
        int id = this.data.p_entryid[index];
        if (id < 0)
        {
            this.statusPerf.deletePlayerStatus(index);
            return false;
        }
        BattleServantData svtdata = this.data.getPlayerServantData(id);
        this.p_actorlist[index] = base.createObject(this.actorprefab, this.root_field, null);
        this.p_actorlist[index].transform.position = this.fieldmotion.getPopPlayer().position;
        this.fieldmotionfsm.Fsm.Variables.GetFsmGameObject("PLAYER" + (index + 1)).Value = this.p_actorlist[index];
        BattleActorControl component = this.p_actorlist[index].GetComponent<BattleActorControl>();
        component.setPerformance(this);
        component.setServantData(svtdata);
        component.setTypePlayer();
        component.setDirLeft();
        component.setCriteriaPos();
        component.setID(id);
        component.setCamera(this.actorcamera);
        component.setMotionlist("_PLAYER", this.actorcamera.gameObject, this.camerafsm.gameObject);
        component.setMyStage(this.playerStage);
        component.setEnemyStage(this.enemyStage);
        svtdata.resetParamObject();
        svtdata.addParamObject(this.p_actorlist[index]);
        this.statusPerf.setPlayerParam(index, this.data.getPlayerServantData(id));
        this.p_actorlist[index].SendMessage("loadEvents");
        return true;
    }

    public void longPressTarget(int index)
    {
        this.statusPerf.masterPerf.showEnemyServant(index);
    }

    public void moveNextBattle(string endstr)
    {
        this.fieldmotionfsm.SendEvent("MOVE_NEXTBATTLE");
        this.effectFadeOut();
    }

    public void movePositionToBattle()
    {
        this.fieldmotionfsm.SendEvent("ACTION_START");
        this.positionMode = PositionMode.BATTLE;
    }

    public void movePositionToTactical()
    {
        this.commandPerf.resetCommandCard();
        this.fieldmotionfsm.SendEvent("TACTICAL_START");
        this.positionMode = PositionMode.TACTICAL;
    }

    private void OnDestroy()
    {
        FingerGestures.OnGestureEvent -= new Gesture.EventHandler(this.FingerGestures_OnGestureEvent);
    }

    protected void OnEndChangeBg()
    {
        if ((SingletonMonoBehaviour<BattleSequenceManager>.Instance == null) || !SingletonMonoBehaviour<BattleSequenceManager>.Instance.IsPlaying)
        {
            this.UpdateAllShadow();
        }
        if (this.BgLoadCallback != null)
        {
            this.BgLoadCallback();
            this.BgLoadCallback = null;
        }
    }

    public void playActionData(BattleActionData adddata)
    {
        if (adddata != null)
        {
            this.DebugLog("<<adddata:" + adddata.type);
            this.queue_action.Enqueue(adddata);
        }
        if (this.queue_action.Count <= 0)
        {
            this.DebugLog("no queue");
            this.logic.endActionData();
        }
        else
        {
            this.DebugLog("queue_action.Count:" + this.queue_action.Count);
            this.nowAction = this.queue_action.Dequeue();
            this.infoComp.showCommonMessage(this.nowAction);
            this.DebugLog(" Start Action ");
            if (this.nowAction.isActors())
            {
                this.DebugLog(" isActors ");
                BattleServantData svt = this.data.getServantData(this.nowAction.actorId);
                if (this.nowAction.type == BattleActionData.TYPE_DEAD)
                {
                    if (this.checkTimeScaleDead(svt))
                    {
                        this.logic.resetTimeAcceleration();
                    }
                }
                else if ((this.nowAction.type == BattleActionData.TYPE_TW) && this.checkTimeScaleTreasureDevice(svt))
                {
                    this.logic.resetTimeAcceleration();
                }
                BattleActorControl component = this.getServantGameObject(this.nowAction.actorId).GetComponent<BattleActorControl>();
                component.setTargetObject(this.getServantGameObject(this.nowAction.targetId));
                component.playBattleActionData(this.nowAction);
                this.playBattleActionCutIn(this.nowAction);
            }
            else if (this.nowAction.isField())
            {
                this.DebugLog(" isField ");
                this.fieldmotion.playBattleActionData(this.nowAction);
                this.playBattleActionCutIn(this.nowAction);
            }
            else if (this.nowAction.isSystem())
            {
                this.DebugLog(" isSystem ");
                base.StartCoroutine(this.waitEndActionData(this.nowAction));
            }
            else if (this.nowAction.isMotion())
            {
                this.DebugLog(" isMotion ");
                BattleActorControl control2 = this.getServantGameObject(this.nowAction.actorId).GetComponent<BattleActorControl>();
                control2.setTargetObject(this.nowAction.targetObject);
                control2.playBattleActionData(this.nowAction);
            }
            this.data.setLastActionActor(this.nowAction);
        }
    }

    public void playActionSkill()
    {
        this.statusPerf.startSkill();
        this.changeAttackButton(false, false, false);
    }

    public GameObject playActorBigCutIn(string filename, Vector3 inpos, bool flg = true)
    {
        this.cutinRootTr.gameObject.SetActive(true);
        GameObject obj2 = null;
        if (flg)
        {
            obj2 = base.createObject("effect/BitEffect/" + filename, this.cutinRootTr, null);
        }
        else
        {
            obj2 = base.createObject("effect/" + filename, this.cutinRootTr, null);
        }
        obj2.transform.localPosition = inpos;
        obj2.AddComponent<BattlePanelEvent>().Panel = this.cutinRootTr.gameObject;
        return obj2;
    }

    public void PlayActorsVoice(float rate, BattleActorControl actor, ActorGroup actorGroup, Voice.BATTLE[] voices, int[] weightlist, float volume, System.Action callback)
    {
        if (_never)
        {
            Debug.Log(string.Empty + new Voice.BATTLE[1]);
        }
        if (UnityEngine.Random.Range(0, 100) >= rate)
        {
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            BattleServantData battleSvtData = actor.BattleSvtData;
            BattlePerformance performance = actor.performance;
            List<BattleActorControl> list = new List<BattleActorControl>();
            ActorGroup enemies = actorGroup;
            switch (actorGroup)
            {
                case ActorGroup.Actor:
                    if (battleSvtData.isEntry)
                    {
                        list.Add(actor);
                    }
                    break;

                case ActorGroup.ActorsParty:
                    if (!actor.IsEnemy)
                    {
                        enemies = ActorGroup.Players;
                        break;
                    }
                    enemies = ActorGroup.Enemies;
                    break;

                case ActorGroup.ActorsEnemy:
                    if (!actor.IsEnemy)
                    {
                        enemies = ActorGroup.Enemies;
                        break;
                    }
                    enemies = ActorGroup.Players;
                    break;
            }
            switch (enemies)
            {
                case ActorGroup.Players:
                case ActorGroup.All:
                    foreach (GameObject obj2 in performance.PlayerActorList)
                    {
                        if (obj2 != null)
                        {
                            BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                            if (((component != null) && component.BattleSvtData.isEntry) && (component.BattleSvtData.isAlive() && !list.Contains(component)))
                            {
                                list.Add(component);
                            }
                        }
                    }
                    break;
            }
            if ((enemies == ActorGroup.Enemies) || (enemies == ActorGroup.All))
            {
                foreach (GameObject obj3 in performance.EnemyActorList)
                {
                    if (obj3 != null)
                    {
                        BattleActorControl item = obj3.GetComponent<BattleActorControl>();
                        if (((item != null) && item.BattleSvtData.isEntry) && (item.BattleSvtData.isAlive() && !list.Contains(item)))
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            if (list.Count == 0)
            {
                if (callback != null)
                {
                    callback();
                }
            }
            else
            {
                int num3 = UnityEngine.Random.Range(0, list.Count);
                actor = list[num3];
                WeightRate<int> rate2 = new WeightRate<int>();
                for (int i = 0; i < voices.Length; i++)
                {
                    if (actor.checkVoice(voices[i]))
                    {
                        rate2.setWeight(weightlist[i], i);
                    }
                }
                if (rate2.getTotalWeight() == 0)
                {
                    Debug.LogWarning(string.Concat(new object[] { "PlayActorsVoice:", actor.uniqueID, ":", actor.getServantId(), ": not found voice" }));
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    int index = rate2.getData(UnityEngine.Random.Range(0, rate2.getTotalWeight()));
                    Voice.BATTLE type = voices[index];
                    if (type == Voice.BATTLE.NONE)
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                    }
                    else
                    {
                        actor.playVoice(type, volume, callback);
                    }
                }
            }
        }
    }

    public void playBackStepMotion(int uniqueID)
    {
        GameObject obj2 = this.getServantGameObject(uniqueID);
        if (obj2 != null)
        {
            obj2.GetComponent<BattleActorControl>().playNoActionDataMotion("MOTION_BACK");
        }
    }

    public void playBattleActionCutIn(BattleActionData adata)
    {
        BattleServantData data = this.data.getServantData(adata.actorId);
        BattleServantData svtdata = this.data.getServantData(adata.targetId);
        if (this.data.isPlayerID(adata.actorId))
        {
            if (adata.isTypeTA())
            {
                data.updateNpGauge();
                this.commandPerf.playNobleCardEffect(adata.commandattack);
                this.statusPerf.playAttackEffect(adata.actorId);
            }
            else if (0 <= adata.commandattack)
            {
                this.statusPerf.setTargetParam(svtdata);
                this.statusPerf.playAttackEffect(adata.actorId);
            }
        }
        else if (!this.data.isEnemyID(adata.actorId))
        {
            if (adata.isTypeOrderArts())
            {
                this.commandPerf.playTypeEffect(false);
                this.playActorBigCutIn("bit_ramification04", Vector3.zero, true);
            }
            else if (adata.isTypeOrderBuster())
            {
                this.commandPerf.playTypeEffect(true);
                this.playActorBigCutIn("bit_ramification07", Vector3.zero, true);
            }
            else if (adata.isTypeOrderQuick())
            {
                this.commandPerf.playTypeEffect(true);
                this.playActorBigCutIn("bit_ramification01", Vector3.zero, true);
            }
        }
    }

    public void playBigCutIn(int Id, int type, Vector3 inpos)
    {
        this.cutinRootTr.gameObject.SetActive(true);
        BattleServantData data = this.data.getServantData(Id);
        GameObject obj2 = base.createObject($"effect/BitEffect/bit_cutin{type:00}", this.cutinRootTr, null);
        obj2.GetComponent<EffectComponent>().setFigure(data.svtId, data.level, null);
        obj2.transform.localPosition = inpos;
        obj2.AddComponent<BattlePanelEvent>().Panel = this.cutinRootTr.gameObject;
    }

    public void playLoseMotion()
    {
        this.fieldmotionfsm.SendEvent("BATTLE_LOSE");
    }

    public void playMasterCommandSpellCutIn()
    {
        GameObject gameObject = this.playActorBigCutIn("bit_mastercutin01", Vector3.zero, true).transform.getNodeFromName("masterfigure", true).gameObject;
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        UserEquipEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(entity.userEquipId);
        MasterFigureManagerOld.CreatePrefab(gameObject, UIMasterFigureRenderOld.DispType.FULL, entity.genderType, entity2.equipId, 1, null);
    }

    public void playMasterCutIn()
    {
        GameObject gameObject = this.playActorBigCutIn("bit_mastercutin01", Vector3.zero, true).transform.getNodeFromName("masterfigure", true).gameObject;
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        UserEquipEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(entity.userEquipId);
        MasterFigureManagerOld.CreatePrefab(gameObject, UIMasterFigureRenderOld.DispType.FULL, entity.genderType, entity2.equipId, 1, null);
    }

    public void playNoblePhantasm()
    {
        this.fieldmotion.sendEvent("ACTION_START");
        this.fieldmotion.sendEvent("TEST_NOBLE_PHANTASM");
    }

    public void playWinMotion()
    {
        BgmEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BGM).getEntityFromId<BgmEntity>(0x15);
        if (entity != null)
        {
            SoundManager.playBgm(entity.fileName);
        }
        this.statusPerf.setShowTurn(this.data, 1);
        this.effectWinBattle();
        BattleActorControl actor = null;
        foreach (GameObject obj2 in this.PlayerActorList)
        {
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (((component != null) && component.BattleSvtData.isEntry) && component.BattleSvtData.isAlive())
                {
                    actor = component;
                }
            }
        }
        if (actor != null)
        {
            Voice.BATTLE[] voices = new Voice.BATTLE[] { Voice.BATTLE.WIN1, Voice.BATTLE.WIN2 };
            int[] weightlist = new int[] { 50, 50 };
            this.PlayActorsVoice(100f, actor, ActorGroup.Players, voices, weightlist, 1f, null);
        }
        this.fieldmotionfsm.SendEvent("BATTLE_WIN");
    }

    public void popBuffLabel(BattleActorControl actor, BattleActionData.BuffData buffData)
    {
        GameObject obj2 = null;
        obj2 = base.createObject(this.effectcontrol.getBuffTextObject(buffData.popColor), this.popupTr, null);
        if (obj2 == null)
        {
            Debug.LogError("popBuffLabel(" + buffData.popColor);
        }
        else
        {
            UILabel componentInChildren = obj2.GetComponentInChildren<UILabel>();
            if (componentInChildren != null)
            {
                componentInChildren.text = buffData.popLabel;
            }
            BattleServantBuffIconComponent component = obj2.GetComponentInChildren<BattleServantBuffIconComponent>();
            if (component != null)
            {
                component.setImageId(buffData.popIcon);
                component.gameObject.transform.localPosition = new Vector3(-28f + (-14f * componentInChildren.text.Length), 0f);
            }
            TrackingMoveCtCComponent component2 = obj2.AddComponent<TrackingMoveCtCComponent>();
            if (actor.IsEnemy)
            {
                component2.Set(this.perf.actorcamera, this.perf.uicamera, actor.getHeadUpObject(), actor.getHeadUpY());
            }
            else
            {
                component2.Set(this.perf.actorcamera, this.perf.uicamera, actor.getHeadUpObject(), actor.getHeadUpY());
            }
            component2.startAct();
        }
    }

    public void popCriticalTitle(BattleActorControl actor)
    {
        this.createPopEffect(actor, BattleEffectControl.ID.TITLE_CRITICAL);
    }

    public void popDamageObject(BattleActorControl actor, int damage, BattleActionData.DamageData damageData, bool damageMotion, bool isRandomDamage, Vector3 damageNumberPosition, bool isNoDamageMotion, bool isRandomTiming, float randomTime)
    {
        <popDamageObject>c__AnonStorey82 storey = new <popDamageObject>c__AnonStorey82 {
            damageMotion = damageMotion,
            isNoDamageMotion = isNoDamageMotion,
            actor = actor,
            damage = damage,
            damageData = damageData,
            isRandomDamage = isRandomDamage,
            damageNumberPosition = damageNumberPosition,
            <>f__this = this
        };
        storey.critical = storey.damageData.critical;
        storey.weak = storey.damageData.weak;
        storey.resist = storey.damageData.regist;
        storey.invincible = storey.damageData.invincible;
        storey.avoidance = storey.damageData.avoidance;
        if (!SingletonMonoBehaviour<BattleSequenceManager>.Instance.IsPlaying)
        {
            storey.isRandomDamage = false;
            storey.damageNumberPosition = Vector3.zero;
        }
        System.Action func = new System.Action(storey.<>m__B0);
        if (isRandomTiming)
        {
            base.StartCoroutine(this.InvokeAction(func, randomTime));
        }
        else
        {
            func();
        }
    }

    public void popHealObject(BattleActorControl actor, int heal)
    {
        this.createPopEffect(actor, BattleEffectControl.ID.HEAL_NO).GetComponent<EffectComponent>().setLabel($"{heal:#,0}");
    }

    public void popRegistTitle(BattleActorControl actor)
    {
        this.createPopEffect(actor, BattleEffectControl.ID.REGIST);
    }

    public void popWeakTitle(BattleActorControl actor)
    {
        this.createPopEffect(actor, BattleEffectControl.ID.TITLE_WEEK);
    }

    public void PreloadFace(int svtId, int limitCount)
    {
        GameObject obj2 = this.FetchStandFigure(svtId, limitCount, false);
        if (obj2 != null)
        {
            obj2.SetActive(false);
        }
        else
        {
            int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
            if (<>f__am$cache3E == null)
            {
                <>f__am$cache3E = delegate {
                };
            }
            UIStandFigureR er = StandFigureManager.CreateRenderPrefab(base.gameObject, svtId, imageLimitCount, Face.Type.CRY, 0, <>f__am$cache3E);
            er.gameObject.SetActive(false);
            this.StandFigures.Add(er.gameObject);
        }
    }

    public void procEndQuest()
    {
        Debug.Log("procEndQuest");
        foreach (GameObject obj2 in this.p_actorlist)
        {
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                UnityEngine.Object.Destroy(obj2);
                ServantAssetLoadManager.unloadServant(component.getServantId(), component.getLimitCount());
                ServantAssetLoadManager.unloadWeaponGroupEffect(component.getWeaponGroup(), component.getEffectFolder());
            }
        }
        foreach (GameObject obj3 in this.e_actorlist)
        {
            if (obj3 != null)
            {
                BattleActorControl control2 = obj3.GetComponent<BattleActorControl>();
                UnityEngine.Object.Destroy(obj3);
                ServantAssetLoadManager.unloadServant(control2.getServantId(), control2.getLimitCount());
                ServantAssetLoadManager.unloadWeaponGroupEffect(control2.getWeaponGroup(), control2.getEffectFolder());
            }
        }
        this.bgPerf.ReleaseBg();
        SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage("Battle");
    }

    public void registCommandCard(BattleCommandData[] datalist)
    {
        this.commandPerf.registCommandCard(datalist);
    }

    public void replaceMember(BattleActionData.ReplaceMember replaceData, System.Action endCallBack)
    {
        this.logic.replaceMember(replaceData, endCallBack);
    }

    public void replacePlayerActor(int index)
    {
        BattleActorControl component = this.p_actorlist[index].GetComponent<BattleActorControl>();
        component.setTargetObject(this.fieldmotion.getPlayerTacticalTr(index).gameObject);
        component.playMotion("MOTION_CHANGE");
    }

    public void repopEnemyActor(int index)
    {
        BattleActorControl component = this.e_actorlist[index].GetComponent<BattleActorControl>();
        component.setTargetObject(this.fieldmotion.getEnemyPopPoint(index).gameObject);
        component.playMotion("MOTION_ENTRY");
    }

    public void repopPlayerActor(int index)
    {
        BattleActorControl component = this.p_actorlist[index].GetComponent<BattleActorControl>();
        component.setTargetObject(this.fieldmotion.getPlayerPopTr(index).gameObject);
        component.playMotion("MOTION_ENTRY");
    }

    public void resetPopObject()
    {
        foreach (GameObject obj2 in this.popitemlist)
        {
            this.destroyInstantiate(obj2);
        }
        this.popitemlist.Clear();
        Debug.Log("**Clear:" + this.popitemlist.Count);
    }

    public void reViewTargetMarks()
    {
        this.setEnemyTarget(this.data.globaltargetId);
    }

    public void setAmbientColor(int index)
    {
        this.fieldEffect.setAmbientColor(index);
    }

    public void setBattleSpeed(float time)
    {
        if (this.fsm.ActiveStateName.Equals("ActionBattle"))
        {
            Time.timeScale = time;
        }
    }

    public void setCameraEffectStatus(Dictionary<string, bool> stat)
    {
        Camera middleCamera = this.middleCamera;
        if (middleCamera != null)
        {
            Bloom component = middleCamera.GetComponent<Bloom>();
            if (component != null)
            {
                component.enabled = stat["Bloom"];
            }
            MotionBlur blur = middleCamera.GetComponent<MotionBlur>();
            if (blur != null)
            {
                blur.enabled = stat["MotionBlur"];
            }
            Vignetting vignetting = middleCamera.GetComponent<Vignetting>();
            if (vignetting != null)
            {
                vignetting.enabled = stat["Vignetting"];
            }
            GrayscaleEffect effect = middleCamera.GetComponent<GrayscaleEffect>();
            if (effect != null)
            {
                effect.enabled = stat["GrayscaleEffect"];
            }
        }
    }

    public void SetCameraFlip(bool flg)
    {
        CameraFlip = flg;
    }

    public void setCommandCard(BattleCommandData[] datalist, int maxcount)
    {
        this.commandPerf.setCommandCard(datalist, maxcount);
        this.commandPerf.setCountRemaining(3);
    }

    public void setDamageVoiceFlg(bool flg)
    {
        BattleActionData data = this.getActionData();
        if (data != null)
        {
            foreach (int num in data.GetDamageTargets(-1))
            {
                this.getServantGameObject(num).GetComponent<BattleActorControl>().setNpDamageVoice(flg);
            }
        }
    }

    public void setEnemyTarget(int uniqueId)
    {
        this.statusPerf.setTargetParam(this.data.getServantData(uniqueId));
    }

    public void setFieldEffect(int type)
    {
        this.fieldEffect.setFieldEffect(type);
    }

    public void setInitpos()
    {
        for (int i = 0; i < this.p_actorlist.Length; i++)
        {
            if (this.p_actorlist[i] != null)
            {
                BattleActorControl component = this.p_actorlist[i].GetComponent<BattleActorControl>();
                if (component != null)
                {
                    component.setCriteriaPos();
                }
            }
        }
        for (int j = 0; j < this.e_actorlist.Length; j++)
        {
            if (this.e_actorlist[j] != null)
            {
                this.e_actorlist[j].GetComponent<BattleActorControl>().setCriteriaPos();
            }
        }
    }

    public void setOffTarget()
    {
        this.statusPerf.setOffTarget();
    }

    public void setPopObject(GameObject obj)
    {
        if (!this.popitemlist.Contains(obj))
        {
            this.popitemlist.Add(obj);
        }
    }

    public void setResult(string str)
    {
        this.resultwindow.gameObject.SetActive(true);
        this.resultwindow.Set(str);
        SingletonTemplate<MissionNotifyManager>.Instance.CancelPause();
    }

    public void setSelectTargetFlg(bool flg)
    {
        this.istactical = flg;
    }

    public void setTargetChangeActive(bool isChange)
    {
        this.isTargetChange = isChange;
    }

    public void setTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void setupCameraFov(float fov)
    {
        this.currentFov = fov;
        float currentFov = this.currentFov;
        this.actorcamera.fieldOfView = currentFov;
        this.middleCamera.fieldOfView = currentFov;
        this.cutIncamera.fieldOfView = DefaultFov;
    }

    public void showActionMessage()
    {
    }

    public void showActionNobleTitle()
    {
        BattleActionData actionData = this.getActionData();
        if (actionData != null)
        {
            this.infoComp.showSpecialName(actionData);
        }
    }

    public void ShowBuff(GameObject actObj, int funcIndex)
    {
        BattleActionData actionData = this.getActionData();
        BattleActorControl component = actObj.GetComponent<BattleActorControl>();
        if (component != null)
        {
            actionData = component.ActionData;
        }
        foreach (BattleActionData.BuffData data2 in actionData.getBuffList(funcIndex))
        {
            GameObject targetObject = this.perf.getServantGameObject(data2.targetId);
            BattleServantData data3 = this.data.getServantData(data2.targetId);
            this.statusPerf.updateBuff();
            if (targetObject != null)
            {
                int[] effectList = data2.effectList;
                if (effectList != null)
                {
                    foreach (int num2 in effectList)
                    {
                        GameObject obj3 = BattleEffectUtility.loadEffectToNode(targetObject, num2, actObj);
                        if (obj3 != null)
                        {
                            obj3.SetActive(true);
                        }
                    }
                }
                if (data2.popLabel != null)
                {
                    BattleActorControl actor = targetObject.GetComponent<BattleActorControl>();
                    if (actor != null)
                    {
                        this.popBuffLabel(actor, data2);
                    }
                }
            }
            if (data2.procType == BattleActionData.BuffData.BuffProcType.INSTANT_DEATH)
            {
                data3.addDamage(data3.getNowHp());
            }
            else if (data2.procType == BattleActionData.BuffData.BuffProcType.UPDATE_CRITICAL)
            {
                this.statusPerf.updateCriticalPoint();
            }
            else if (data2.procType == BattleActionData.BuffData.BuffProcType.UPDATE_HP)
            {
                data3.addDamage(0);
            }
            else if (data2.procType == BattleActionData.BuffData.BuffProcType.UPDATE_NP)
            {
                data3.addNp(0, true);
            }
            else if (data2.procType == BattleActionData.BuffData.BuffProcType.UPDATE_NPTURN)
            {
                data3.updateTDGauge();
            }
            if (data3 != null)
            {
                data3.updateBuff();
            }
        }
    }

    public void showCutIn(GameObject prefab)
    {
        this.camerafsm.SendEvent("CUTIN");
        base.createObject(prefab, this.cutinTargetTr, null);
    }

    public void ShowDamage(GameObject actObj, int nomalEffectId, int criticalEffectId, string attachNodename, int functionIndex, int startValue, int countValue, bool isRandomDamage, Vector3 damageNumberPosition, bool isNoDamageMotion = false)
    {
        BattleActionData bactData = this.getActionData();
        BattleActorControl component = actObj.GetComponent<BattleActorControl>();
        if (component != null)
        {
            bactData = component.ActionData;
        }
        if (bactData == null)
        {
            Debug.LogError("no actiondata");
        }
        else
        {
            if (nomalEffectId != -1)
            {
                this.PlaySeName = BattleEffectUtility.getEffectSeName(nomalEffectId);
            }
            else
            {
                this.PlaySeName = null;
            }
            BattleActionData.DamageData[] dataArray = this.showDamageView(functionIndex, bactData, startValue, true, isRandomDamage, damageNumberPosition, isNoDamageMotion);
            if ((startValue == 0) && bactData.isCommandAttack())
            {
                this.ShowBuff(actObj, -1);
                this.showHeal(actObj, -1);
            }
            if (1 < countValue)
            {
                base.StartCoroutine(this.corAfterMultiHit(bactData, startValue + 1, countValue - 1, functionIndex, isRandomDamage, damageNumberPosition, isNoDamageMotion));
            }
            else
            {
                this.PlaySeName = null;
            }
            if (dataArray != null)
            {
                foreach (BattleActionData.DamageData data2 in dataArray)
                {
                    this.DebugLog("damageData:" + data2.targetId);
                    Transform root = this.perf.getServantGameObject(data2.targetId).transform.getNodeFromLvName(attachNodename, -1);
                    if ((nomalEffectId != -1) || (criticalEffectId != -1))
                    {
                        GameObject obj3 = null;
                        if (data2.critical)
                        {
                            obj3 = BattleEffectUtility.getEffectObject(criticalEffectId, actObj);
                        }
                        else
                        {
                            obj3 = BattleEffectUtility.getEffectObject(nomalEffectId, actObj);
                        }
                        if (obj3 != null)
                        {
                            obj3.transform.parent = root;
                            obj3.transform.localPosition = Vector3.zero;
                            obj3.transform.eulerAngles = Vector3.up;
                            obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                            obj3.transform.parent = this.root_field;
                            obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                            obj3.SetActive(true);
                        }
                        if (data2.sphit)
                        {
                            obj3 = base.createObject("effect/ef_hit_special", root, null);
                            obj3.transform.localPosition = Vector3.zero;
                            obj3.transform.eulerAngles = Vector3.up;
                            obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                            obj3.transform.parent = this.root_field;
                            obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                            obj3.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public BattleActionData.DamageData[] showDamageView(int funcIndex, BattleActionData bactData, int index, bool damageMotion, bool isRandomDamage, Vector3 damageNumberPosition, bool isNoDamageMotion)
    {
        List<BattleActionData.DamageData> list = new List<BattleActionData.DamageData>();
        BattleActionData.DamageData[] dataArray = bactData.getDamageList(funcIndex);
        BattleServantData data = this.data.getServantData(bactData.actorId);
        foreach (BattleActionData.DamageData data2 in dataArray)
        {
            int damage = data2.getDamage(index);
            if (0 <= damage)
            {
                int targetId = data2.targetId;
                list.Add(data2);
                BattleServantData data3 = this.data.getServantData(targetId);
                BattleActorControl component = this.getServantGameObject(targetId).GetComponent<BattleActorControl>();
                bool isRandomTiming = (isRandomDamage && SingletonMonoBehaviour<BattleSequenceManager>.Instance.IsPlaying) && (SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget == null);
                float randomTime = UnityEngine.Random.Range((float) 0f, (float) 0.35f);
                this.popDamageObject(component, damage, data2, damageMotion, isRandomDamage, damageNumberPosition, isNoDamageMotion, isRandomTiming, randomTime);
                if (data != null)
                {
                    data.addNp(data2.getAtkNp(index), true);
                }
                data3.addNp(data2.getDefNp(index), true);
                if ((data != null) && this.data.isPlayerID(data.getUniqueID()))
                {
                    int num5 = data2.getCriticalPointCount(index);
                    for (int i = 0; i < num5; i++)
                    {
                        this.dropCriticalPoint(component.getDropTransform());
                    }
                }
                if (data3.isDead())
                {
                    this.infoComp.showOverKillMessage(this.getActionData());
                }
                this.data.addDamage(targetId, damage);
            }
        }
        return list.ToArray();
    }

    public void showHeal(GameObject actObj, int funcIndex)
    {
        foreach (BattleActionData.HealData data2 in this.getActionData().getHealList(funcIndex))
        {
            GameObject obj2 = this.getServantGameObject(data2.targetId);
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    this.popHealObject(component, data2.healPoint);
                }
                this.data.addHeal(data2.targetId, data2.healPoint);
            }
        }
    }

    public void showNobleInfo(int treasureDeviceId = -1, int treasureDeviceLevel = 1, int treasureDevicePer = 100, bool isEnemy = false)
    {
        if (treasureDeviceId < 0)
        {
            BattleServantData data = this.data.getServantData(this.getActionData().actorId);
            treasureDeviceId = data.getTreasureDvcId();
            treasureDeviceLevel = data.getTreasureDvcLevel();
            treasureDevicePer = data.getNpPer();
            isEnemy = data.checkEnemy();
        }
        this.infoComp.showNoblePhantasmInfo(treasureDeviceId, treasureDeviceLevel, treasureDevicePer, isEnemy);
    }

    public void showResult(GameObject target, string endevent)
    {
        this.resultwindow.gameObject.SetActive(true);
        this.resultwindow.StartResult(target, endevent, BattleResultComponent.BattleResultType.BATTLE_NORMAL, this);
    }

    public void showTotalDamage()
    {
        this.infoComp.showTotalDamage(this.getActionData());
    }

    [SerializeField]
    private void Start()
    {
        FingerGestures.OnGestureEvent += new Gesture.EventHandler(this.FingerGestures_OnGestureEvent);
    }

    [DebuggerHidden]
    private IEnumerator startAddAttackEffect(int actorId) => 
        new <startAddAttackEffect>c__Iterator2A { 
            actorId = actorId,
            <$>actorId = actorId,
            <>f__this = this
        };

    public void startBattleUIFade(float time, float targetAlpha)
    {
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.startFadeBattleUI(time, targetAlpha);
    }

    public void startContinue()
    {
        this.contPerf.gameObject.SetActive(true);
        this.contPerf.StartContinue();
    }

    public void startMovePopObject()
    {
        foreach (GameObject obj2 in this.popitemlist)
        {
            if (obj2 != null)
            {
                if (obj2.activeSelf)
                {
                    foreach (ParticleSystem system in obj2.GetComponentsInChildren<ParticleSystem>())
                    {
                        if (system.simulationSpace == ParticleSystemSimulationSpace.World)
                        {
                            system.Stop();
                            system.gameObject.transform.parent = obj2.transform.parent;
                            UnityEngine.Object.Destroy(system.gameObject, system.duration);
                        }
                    }
                    Vector3 vector = this.uicamera.ViewportToWorldPoint(this.actorcamera.WorldToViewportPoint(obj2.transform.position));
                    obj2.SetActive(false);
                    obj2.transform.position = vector;
                    obj2.transform.parent = this.uirootTr;
                    obj2.transform.ChangeChildsLayer(this.uicamera.gameObject.layer);
                    BattleMoveObject component = obj2.GetComponent<BattleMoveObject>();
                    if (component.isMoveToItems())
                    {
                        component.setTargetTr(this.statusPerf.getCollectDropTransform());
                    }
                    else
                    {
                        component.setTargetTr(this.commandPerf.getCollectCriticalTransform());
                    }
                    obj2.SetActive(true);
                    component.startMoveTarget();
                }
                else
                {
                    this.destroyInstantiate(obj2);
                }
            }
        }
    }

    public void startNoblePhantasm()
    {
        if (this.data.systemflg_TdConstantvelocity)
        {
            this.logic.resetTimeAcceleration();
        }
    }

    public void startTac(string fsmevent)
    {
        this.statusPerf.updateView();
        this.tacticBg.gameObject.SetActive(true);
        this.istactical = true;
        this.updateView();
        this.reViewTargetMarks();
        base.StartCoroutine(this.StartTacCoroutine(fsmevent));
    }

    [DebuggerHidden]
    private IEnumerator StartTacCoroutine(string endevent) => 
        new <StartTacCoroutine>c__Iterator26 { 
            endevent = endevent,
            <$>endevent = endevent,
            <>f__this = this
        };

    public void TestDispLog()
    {
        Debug.Log(">>>>>>>>>>>>>>>>>>>> TestDispLog");
        ServantEntity entity = ((ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT)).getEntityFromId<ServantEntity>(0x18704);
        Debug.Log(string.Concat(new object[] { ">>>>> ", entity.name, ":", entity.limitMax }));
    }

    public void UpdateAllShadow()
    {
        foreach (GameObject obj2 in this.p_actorlist)
        {
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().UpdateShadow();
            }
        }
        foreach (GameObject obj3 in this.e_actorlist)
        {
            if (obj3 != null)
            {
                obj3.GetComponent<BattleActorControl>().UpdateShadow();
            }
        }
    }

    public void updateCommandCard()
    {
        this.commandPerf.updateCard();
    }

    public void updateDropItemCount()
    {
        this.perf.statusPerf.updateDropItemCount();
    }

    public void updateStatus()
    {
        this.statusPerf.updateView();
    }

    public void updateView()
    {
        this.statusPerf.updateView();
        this.statusPerf.updateCriticalPoint();
        this.statusPerf.setShowWave(this.data.wavecount, this.data.getLastWave());
        this.statusPerf.setShowTurn(this.data, 0);
        this.statusPerf.updateNokoriEnemyCount();
    }

    [DebuggerHidden]
    private IEnumerator waitEndActionData(BattleActionData battleActionData) => 
        new <waitEndActionData>c__Iterator27 { 
            battleActionData = battleActionData,
            <$>battleActionData = battleActionData,
            <>f__this = this
        };

    [DebuggerHidden]
    private IEnumerator waitEndProc(GameObject obj, float time) => 
        new <waitEndProc>c__Iterator2B { 
            time = time,
            obj = obj,
            <$>time = time,
            <$>obj = obj,
            <>f__this = this
        };

    public int CurrentGroundNo
    {
        get => 
            this.currentGroundNo;
        set
        {
            this.currentGroundNo = value;
        }
    }

    public int CurrentGroundType
    {
        get => 
            this.currentGroundType;
        set
        {
            this.currentGroundType = value;
        }
    }

    public GameObject[] EnemyActorList
    {
        get => 
            this.e_actorlist;
        set
        {
            this.e_actorlist = value;
        }
    }

    public GameObject[] PlayerActorList
    {
        get => 
            this.p_actorlist;
        set
        {
            this.p_actorlist = value;
        }
    }

    [CompilerGenerated]
    private sealed class <corAfterMultiHit>c__Iterator28 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleActionData <$>actData;
        internal int <$>count;
        internal Vector3 <$>damageNumberPosition;
        internal int <$>funcIndex;
        internal int <$>index;
        internal bool <$>isNoDamageMotion;
        internal bool <$>isRandomDamage;
        internal BattlePerformance <>f__this;
        internal int <i>__0;
        internal BattleActionData actData;
        internal int count;
        internal Vector3 damageNumberPosition;
        internal int funcIndex;
        internal int index;
        internal bool isNoDamageMotion;
        internal bool isRandomDamage;

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
                    this.<i>__0 = 0;
                    break;

                case 1:
                    this.<>f__this.showDamageView(this.funcIndex, this.actData, this.index + this.<i>__0, false, this.isRandomDamage, this.damageNumberPosition, this.isNoDamageMotion);
                    this.<i>__0++;
                    break;

                default:
                    goto Label_00B3;
            }
            if (this.<i>__0 < this.count)
            {
                this.$current = new WaitForSeconds(0.1f);
                this.$PC = 1;
                return true;
            }
            this.<>f__this.PlaySeName = null;
            this.$PC = -1;
        Label_00B3:
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
    private sealed class <delayDestory>c__Iterator2C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal float <$>time;
        internal GameObject obj;
        internal float time;

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
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.obj != null)
                    {
                        UnityEngine.Object.Destroy(this.obj);
                    }
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

    [CompilerGenerated]
    private sealed class <InvokeAction>c__Iterator29 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal System.Action <$>func;
        internal float <$>waitTime;
        internal System.Action func;
        internal float waitTime;

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
                    this.$current = new WaitForSeconds(this.waitTime);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.func != null)
                    {
                        this.func();
                    }
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

    [CompilerGenerated]
    private sealed class <popDamageObject>c__AnonStorey82
    {
        internal BattlePerformance <>f__this;
        internal BattleActorControl actor;
        internal bool avoidance;
        internal bool critical;
        internal int damage;
        internal BattleActionData.DamageData damageData;
        internal bool damageMotion;
        internal Vector3 damageNumberPosition;
        internal bool invincible;
        internal bool isNoDamageMotion;
        internal bool isRandomDamage;
        internal bool resist;
        internal bool weak;

        internal void <>m__B0()
        {
            if (this.<>f__this.PlaySeName != null)
            {
                SoundManager.playSe("Battle", this.<>f__this.PlaySeName);
            }
            GameObject obj2 = null;
            if (this.invincible)
            {
                obj2 = this.<>f__this.createObject(this.<>f__this.effectcontrol.getinvincibleObject(), this.<>f__this.popupTr, null);
            }
            else if (this.avoidance)
            {
                obj2 = this.<>f__this.createObject(this.<>f__this.effectcontrol.getavoidanceObject(), this.<>f__this.popupTr, null);
            }
            else
            {
                if (this.damageMotion && !this.isNoDamageMotion)
                {
                    this.actor.sendDamageEvent();
                }
                if (this.critical)
                {
                    this.<>f__this.popCriticalTitle(this.actor);
                }
                if (this.weak)
                {
                    this.<>f__this.popWeakTitle(this.actor);
                }
                if (this.resist)
                {
                    this.<>f__this.popRegistTitle(this.actor);
                }
                string text = $"{this.damage:#,0} ";
                obj2 = this.<>f__this.effectcontrol.getDamageObject(this.critical, this.weak, this.resist);
                obj2.transform.SetParent(this.<>f__this.popupTr, false);
                EffectComponent component = obj2.GetComponent<EffectComponent>();
                component.setLabel(text);
                GameObject[] children = component.label.gameObject.GetChildren(false);
                for (int i = 0; i < children.Length; i++)
                {
                    UnityEngine.Object.Destroy(children[i]);
                }
                int fontSize = component.label.fontSize;
                GameObject obj3 = this.<>f__this.effectcontrol.setBuffIconObject(this.damageData.atkbufflist);
                if (obj3 != null)
                {
                    obj3.transform.parent = component.label.gameObject.transform;
                    obj3.transform.localPosition = new Vector3((float) -((((fontSize / 2) * text.Length) / 2) + 8), 11f);
                    obj3.transform.localScale = new Vector3(0.4f, 0.4f);
                }
                GameObject obj4 = this.<>f__this.effectcontrol.setBuffIconObject(this.damageData.defbufflist);
                if (obj4 != null)
                {
                    obj4.transform.parent = component.label.gameObject.transform;
                    obj4.transform.localPosition = new Vector3((float) -((((fontSize / 2) * text.Length) / 2) + 8), -7f);
                    obj4.transform.localScale = new Vector3(0.4f, 0.4f);
                }
            }
            Vector3 zero = Vector3.zero;
            if (this.isRandomDamage)
            {
                zero = new Vector3(UnityEngine.Random.Range((float) -0.35f, (float) 0.35f), UnityEngine.Random.Range((float) -0.5f, (float) 0.35f), UnityEngine.Random.Range((float) -0.35f, (float) 0.35f));
            }
            TrackingMoveCtCComponent component2 = obj2.GetComponent<TrackingMoveCtCComponent>();
            if (component2 == null)
            {
                component2 = obj2.AddComponent<TrackingMoveCtCComponent>();
            }
            component2.Set(this.<>f__this.perf.actorcamera, this.<>f__this.perf.uicamera, this.actor.getHeadUpObject(), (this.actor.getHeadUpY() + zero) + this.damageNumberPosition);
            component2.startAct();
        }
    }

    [CompilerGenerated]
    private sealed class <startAddAttackEffect>c__Iterator2A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>actorId;
        internal BattlePerformance <>f__this;
        internal int actorId;

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
                    this.<>f__this.statusPerf.playAttackEffect(this.actorId);
                    this.$current = new WaitForSeconds(0.6f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.statusPerf.playAttackEffect(this.actorId);
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

    [CompilerGenerated]
    private sealed class <StartTacCoroutine>c__Iterator26 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>endevent;
        internal BattlePerformance <>f__this;
        internal string endevent;

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
                    this.$current = 5.2f;
                    this.$PC = 1;
                    goto Label_008C;

                case 1:
                    this.<>f__this.commandPerf.openCommandCard();
                    this.$current = 0.5f;
                    this.$PC = 2;
                    goto Label_008C;

                case 2:
                    this.<>f__this.logic.sendFsmEvent(this.endevent);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_008C:
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
    private sealed class <waitEndActionData>c__Iterator27 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleActionData <$>battleActionData;
        internal BattlePerformance <>f__this;
        internal BattleActionData battleActionData;

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
                    if (0f >= this.battleActionData.systemTime)
                    {
                        this.$current = new WaitForSeconds(1.2f);
                        this.$PC = 2;
                    }
                    else
                    {
                        this.$current = new WaitForSeconds(this.battleActionData.systemTime);
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                case 2:
                    this.<>f__this.endActionData();
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

    [CompilerGenerated]
    private sealed class <waitEndProc>c__Iterator2B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal float <$>time;
        internal BattlePerformance <>f__this;
        internal GameObject obj;
        internal float time;

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
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.obj != null)
                    {
                        UnityEngine.Object.Destroy(this.obj);
                    }
                    this.<>f__this.endProc();
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

    public enum ActorGroup
    {
        Actor,
        ActorsParty,
        ActorsEnemy,
        Players,
        Enemies,
        All
    }

    public enum BattleUIPanel
    {
        BackCom,
        Command,
        FrontCom,
        PopUp
    }

    private enum PositionMode
    {
        BATTLE,
        TACTICAL
    }
}

