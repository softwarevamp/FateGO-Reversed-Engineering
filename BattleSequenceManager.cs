using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WellFired;

public class BattleSequenceManager : SingletonMonoBehaviour<BattleSequenceManager>
{
    public GameObject actor;
    private List<bool> ActorActiveList;
    public Camera actorCamera;
    private int actorLimitImgCount;
    public bool BackToTest;
    protected float backupBgmLocalVolume;
    private string BackupBgmName;
    [HideInInspector]
    public int BackupBgmTime;
    protected float backupBgmVolume;
    private System.Action BgChangedCallback;
    public GameObject bgObject;
    private Dictionary<string, bool> cameraEffectsStatus;
    private int chrId;
    private List<GameObject> createdObjects;
    public Camera cutInCamera;
    public GameObject CutInRoot;
    protected List<System.Action> DelayInvokeMethodList;
    protected float delayInvokeTimer;
    private bool DemoMode;
    public Camera effectCamera;
    public GameObject[] enemyActors;
    private Face.Type faceType;
    protected BattlePerformance.BattleUIPanel[] FadeTargetPanelIndexs;
    protected bool IsBgBusy;
    public bool IsPlaying;
    public int limitCount;
    protected float NoblePhantasmBgmVolume;
    [SerializeField]
    protected static readonly float NoblePhantasmBgmVolumeRate = 0.5f;
    protected Action<USSequencer> OnCompleteActorCallback;
    private Transform originalCameraRoot;
    private BattlePerformance performance;
    public GameObject[] playerActors;
    protected List<string> ReleaseSoundNames;
    public GameObject seqObject;
    public GameObject sequenceManager;
    public int servantId = 0x18704;
    public GameObject SingleTarget;
    private UIStandFigureM standFigure;
    public bool testMode;
    public int testNpPer = 500;
    private int treasureDeviceId;
    private bool whiteInFlg;

    public BattleSequenceManager()
    {
        BattlePerformance.BattleUIPanel[] panelArray1 = new BattlePerformance.BattleUIPanel[3];
        panelArray1[1] = BattlePerformance.BattleUIPanel.Command;
        panelArray1[2] = BattlePerformance.BattleUIPanel.FrontCom;
        this.FadeTargetPanelIndexs = panelArray1;
        this.ReleaseSoundNames = new List<string>();
        this.DelayInvokeMethodList = new List<System.Action>();
        this.whiteInFlg = true;
        this.createdObjects = new List<GameObject>();
    }

    public void changeBg(int id, int tp, Vector3 pos, Vector3 rot, bool parentCamera = false, System.Action callback = null)
    {
        this.IsBgBusy = true;
        this.BgChangedCallback = callback;
        this.performance.changeBg(id, tp, pos, rot, true, parentCamera, new System.Action(this.OnBgChanged));
    }

    protected void FadeBattleUI(float time, float targetAlpha)
    {
        foreach (BattlePerformance.BattleUIPanel panel in this.FadeTargetPanelIndexs)
        {
            UIPanel panel2 = this.performance.getPanel((int) panel);
            if (panel2 != null)
            {
                TweenAlpha.Begin(panel2.gameObject, time, targetAlpha);
            }
        }
        if (this.performance.root_drop != null)
        {
            if (targetAlpha == 1f)
            {
                this.performance.root_drop.gameObject.SetActive(true);
            }
            else
            {
                this.performance.root_drop.gameObject.SetActive(false);
            }
        }
    }

    public UIStandFigureM FetchMeshPrefab(int svtId, int limitImgCnt, Face.Type faceType)
    {
        if (((this.chrId == svtId) && (this.actorLimitImgCount == limitImgCnt)) && (this.faceType == faceType))
        {
            return this.standFigure;
        }
        return null;
    }

    public void init(BattlePerformance performance, GameObject actor, GameObject[] players, GameObject[] enemies, Camera camera, GameObject bg)
    {
        this.performance = performance;
        this.actor = actor;
        this.playerActors = players;
        this.enemyActors = enemies;
        this.actorCamera = camera;
        this.effectCamera = performance.middleCamera;
        this.cutInCamera = performance.cutIncamera;
        this.originalCameraRoot = this.actorCamera.transform.parent;
        this.bgObject = bg;
        this.IsPlaying = true;
        this.FadeBattleUI(0.3f, 0f);
    }

    private bool isValidObject(string name) => 
        ((name != "Actor") && (name != "animCamLoc"));

    public void loadSequence(int chrId, int treasureDeviceId, int limitCount, onGameObjectLoadComplete onComplete)
    {
        <loadSequence>c__AnonStorey7B storeyb = new <loadSequence>c__AnonStorey7B {
            chrId = chrId,
            limitCount = limitCount,
            onComplete = onComplete,
            <>f__this = this
        };
        this.chrId = storeyb.chrId;
        this.treasureDeviceId = treasureDeviceId;
        this.actorLimitImgCount = this.actor.GetComponent<BattleActorControl>().LimitImageIndex;
        this.faceType = Face.Type.CRY;
        ServantAssetLoadManager.LoadNoblePhantasm(base.gameObject, storeyb.chrId, storeyb.limitCount, treasureDeviceId, new ServantAssetLoadManager.onGameObjectLoadComplete(storeyb.<>m__98));
    }

    public void npTest()
    {
    }

    protected void OnBgChanged()
    {
        this.IsBgBusy = false;
        if (this.BgChangedCallback != null)
        {
            this.BgChangedCallback();
            this.BgChangedCallback = null;
        }
    }

    protected void OnChangeBgmVolume(float newValue)
    {
        SingletonMonoBehaviour<BgmManager>.Instance.PlayerVolume = newValue;
    }

    private void OnNoblePhantasmPlayComplete(USSequencer seq)
    {
        base.StartCoroutine(this.WaitEndSequence());
    }

    private void OnNoblePhantasmPlayCompleteProc()
    {
        if (this.actorCamera != null)
        {
            this.actorCamera.transform.parent = this.originalCameraRoot;
        }
        if (this.BackupBgmName != SingletonMonoBehaviour<BgmManager>.Instance.BgmName)
        {
            BgmManager.PlayBgm(this.BackupBgmName, this.backupBgmLocalVolume, 0.5f, (long) this.BackupBgmTime);
        }
        else
        {
            object[] args = new object[] { "from", this.NoblePhantasmBgmVolume, "to", this.backupBgmVolume, "time", 0.5f, "onupdate", "OnChangeBgmVolume" };
            iTween.ValueTo(base.gameObject, iTween.Hash(args));
        }
        this.performance.setCameraEffectStatus(this.cameraEffectsStatus);
        foreach (GameObject obj2 in this.createdObjects)
        {
            if (obj2 != null)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
        this.createdObjects.Clear();
        if (this.seqObject != null)
        {
            UnityEngine.Object.Destroy(this.seqObject);
            this.seqObject = null;
        }
        this.performance.FlipAll(false);
        if (this.ActorActiveList.Count == 6)
        {
            int num = 0;
            foreach (GameObject obj3 in this.performance.PlayerActorList)
            {
                if (obj3 != null)
                {
                    obj3.SetActive(this.ActorActiveList[num]);
                }
                num++;
            }
            foreach (GameObject obj4 in this.performance.EnemyActorList)
            {
                if (obj4 != null)
                {
                    obj4.SetActive(this.ActorActiveList[num]);
                }
                num++;
            }
        }
        this.performance.InitActorPosition();
        foreach (GameObject obj5 in this.performance.PlayerActorList)
        {
            if (obj5 != null)
            {
                BattleActorControl component = obj5.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    component.playAnimation("wait");
                    if (!this.DemoMode)
                    {
                        component.ChangeShadowColor(Color.clear, 0.3f);
                    }
                    component.SetMaterialColor(Color.black, 0f);
                }
            }
        }
        foreach (GameObject obj6 in this.performance.EnemyActorList)
        {
            if (obj6 != null)
            {
                BattleActorControl control2 = obj6.GetComponent<BattleActorControl>();
                if (control2 != null)
                {
                    control2.playAnimation("wait");
                    if (!this.DemoMode)
                    {
                        control2.ChangeShadowColor(Color.clear, 0.3f);
                    }
                    control2.SetMaterialColor(Color.black, 0f);
                }
            }
        }
        if (this.standFigure != null)
        {
            UnityEngine.Object.Destroy(this.standFigure);
            this.standFigure = null;
        }
        this.ReleaseSoundNames.Add("NoblePhantasm_" + this.chrId);
        this.DelayInvokeMethodList.Add(delegate {
            foreach (string str in this.ReleaseSoundNames)
            {
                SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage(str);
            }
            this.ReleaseSoundNames.Clear();
        });
        this.delayInvokeTimer = 1f;
        if (this.whiteInFlg)
        {
            NGUIFader fader = this.performance.fadeObject.GetComponent<NGUIFader>();
            if (fader.isFaded())
            {
                fader.FadeStart(Color.white, 0.2f, true, null, true);
            }
            this.FadeBattleUI(0.3f, 1f);
        }
        this.performance.InitAmbientLight();
        this.actorCamera.cullingMask |= ((int) 1) << LayerMask.NameToLayer("Battle2D");
        this.effectCamera.cullingMask &= ~(((int) 1) << LayerMask.NameToLayer("Battle2D"));
        this.performance.UpdateAllShadow();
        this.IsPlaying = false;
        if (this.OnCompleteActorCallback != null)
        {
            USSequencer sequencer = null;
            if (this.seqObject != null)
            {
                sequencer = this.seqObject.GetComponent<USSequencer>();
            }
            this.OnCompleteActorCallback(sequencer);
        }
    }

    public void play(bool isOpponent, bool isDemoMode, Action<USSequencer> callback)
    {
        if (this.seqObject != null)
        {
            if (!isDemoMode)
            {
                base.Invoke("ShowNobleInfo", 0.7f);
            }
            this.DemoMode = isDemoMode;
            this.cameraEffectsStatus = this.performance.getCameraEffectStatus();
            this.actorCamera.cullingMask &= ~(((int) 1) << LayerMask.NameToLayer("Battle2D"));
            this.effectCamera.cullingMask |= ((int) 1) << LayerMask.NameToLayer("Battle2D");
            this.ActorActiveList = new List<bool>();
            foreach (GameObject obj2 in this.performance.PlayerActorList)
            {
                if (obj2 != null)
                {
                    this.ActorActiveList.Add(obj2.activeSelf);
                }
                else
                {
                    this.ActorActiveList.Add(false);
                }
            }
            foreach (GameObject obj3 in this.performance.EnemyActorList)
            {
                if (obj3 != null)
                {
                    this.ActorActiveList.Add(obj3.activeSelf);
                }
                else
                {
                    this.ActorActiveList.Add(false);
                }
            }
            if (!isDemoMode)
            {
                this.SearchTargetAndModifyPositions();
            }
            this.backupBgmVolume = SingletonMonoBehaviour<BgmManager>.Instance.PlayerVolume;
            this.backupBgmLocalVolume = SingletonMonoBehaviour<BgmManager>.Instance.BgmVolume;
            this.NoblePhantasmBgmVolume = this.backupBgmVolume * NoblePhantasmBgmVolumeRate;
            object[] args = new object[] { "from", this.backupBgmVolume, "to", this.NoblePhantasmBgmVolume, "time", 0.3f, "onupdate", "OnChangeBgmVolume" };
            iTween.ValueTo(base.gameObject, iTween.Hash(args));
            this.BackupBgmName = SingletonMonoBehaviour<BgmManager>.Instance.BgmName;
            this.seqObject.SetActive(true);
            USSequencer component = this.seqObject.GetComponent<USSequencer>();
            component.PlaybackFinished = (USSequencer.PlaybackDelegate) Delegate.Combine(component.PlaybackFinished, new USSequencer.PlaybackDelegate(this.OnNoblePhantasmPlayComplete));
            this.OnCompleteActorCallback = callback;
            component.Play();
        }
    }

    private GameObject searchPrefab(int chrId, string name)
    {
        Debug.Log(string.Concat(new object[] { "SEARCH PREFAB[", this.chrId, ":", name, "]" }));
        GameObject original = ServantAssetLoadManager.loadNoblePhantasmEffect(this.treasureDeviceId, name);
        Debug.Log("PREFAB OBJ:" + original);
        if (original != null)
        {
            return UnityEngine.Object.Instantiate<GameObject>(original);
        }
        Debug.Log("Battle/Prefab/" + name);
        original = Resources.Load("Battle/Prefab/" + name) as GameObject;
        if (original != null)
        {
            return UnityEngine.Object.Instantiate<GameObject>(original);
        }
        return null;
    }

    protected void SearchTargetAndModifyPositions()
    {
        BattleActorControl component = this.actor.GetComponent<BattleActorControl>();
        if (((component != null) && (this.SingleTarget != component.gameObject)) && (this.SingleTarget != null))
        {
            GameObject[] enemyActorList;
            string str;
            BattleActorControl control2 = this.SingleTarget.GetComponent<BattleActorControl>();
            if (control2.IsEnemy == component.IsEnemy)
            {
                str = "N_Player2";
            }
            else
            {
                str = "N_Enemy2";
            }
            if (control2.IsEnemy)
            {
                enemyActorList = this.performance.EnemyActorList;
            }
            else
            {
                enemyActorList = this.performance.PlayerActorList;
            }
            foreach (GameObject obj2 in enemyActorList)
            {
                if (obj2 == this.SingleTarget)
                {
                    Transform transform = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition(str);
                    obj2.transform.position = transform.position;
                }
                else if (obj2 != null)
                {
                    obj2.SetActive(false);
                }
            }
        }
    }

    private USTimelineContainer searchTimeline(USSequencer seq, string name)
    {
        IEnumerator enumerator = seq.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                USTimelineContainer component = current.gameObject.GetComponent<USTimelineContainer>();
                if (component.AffectedObjectPath == ("/" + name))
                {
                    return component;
                }
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
        return null;
    }

    public void setup(System.Action callback, bool isDemoMode = false)
    {
        USSequencer component = this.seqObject.GetComponent<USSequencer>();
        USTimelineContainer container = this.searchTimeline(component, "Actor");
        if (container != null)
        {
            container.AffectedObject = this.actor.transform;
        }
        USTimelineContainer container2 = this.searchTimeline(component, "FGOSequenceManager");
        GameObject item = null;
        List<USFGOPlayCutInEvent> evs = new List<USFGOPlayCutInEvent>();
        if (container2 != null)
        {
            container2.AffectedObject = UnityEngine.Object.Instantiate<GameObject>(this.sequenceManager).transform;
            this.createdObjects.Add(container2.AffectedObject.gameObject);
            FGOSequenceManager manager = container2.AffectedObject.GetComponent<FGOSequenceManager>();
            manager.mainCamera = this.actorCamera;
            manager.effectCamera = this.effectCamera;
            manager.cutInCamera = this.cutInCamera;
            manager.faderObject = this.performance.fadeObject;
            if (!isDemoMode)
            {
                this.SetupTargetInfo();
            }
            foreach (USTimelineEvent event2 in container2.GetComponentsInChildren<USTimelineEvent>(true))
            {
                for (int i = 0; i < event2.EventCount; i++)
                {
                    USEventBase base2 = event2.Event(i);
                    if (base2.name == "USFGOAnimationCameraEvent")
                    {
                        if (item == null)
                        {
                            item = UnityEngine.Object.Instantiate<GameObject>(ServantAssetLoadManager.loadNoblePhantasmEffect(this.treasureDeviceId, "animCamLoc"));
                            this.createdObjects.Add(item);
                        }
                        USFGOAnimationCameraEvent event3 = (USFGOAnimationCameraEvent) base2;
                        event3.animCamLoc = item;
                        event3.camera = this.actorCamera;
                        event3.isOpponent = false;
                        item.transform.position = this.actor.transform.position;
                    }
                    else if (base2.name == "USFGOPlayCutInEvent")
                    {
                        evs.Add(base2 as USFGOPlayCutInEvent);
                    }
                    else if (base2.name == "USFGOSetCameraEvent")
                    {
                        USFGOSetCameraEvent event4 = (USFGOSetCameraEvent) base2;
                        event4.camera = this.actorCamera;
                        event4.cameraRoot = this.actorCamera.transform.parent;
                    }
                    else if (base2.name == "USFGOChangeBgEvent")
                    {
                        USFGOChangeBgEvent event5 = (USFGOChangeBgEvent) base2;
                        if ((event5.bgName == "0") || (event5.bgName == string.Empty))
                        {
                            event5.bgName = this.performance.CurrentGroundNo.ToString();
                            event5.bgType = this.performance.CurrentGroundType.ToString();
                        }
                    }
                }
            }
        }
        IEnumerator enumerator = component.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                USTimelineContainer container3 = current.gameObject.GetComponent<USTimelineContainer>();
                Debug.Log("L:>>>> " + container3.AffectedObjectPath);
                string name = null;
                bool flag = false;
                if (container3.AffectedObjectPath.StartsWith("/AllEffects/"))
                {
                    name = container3.AffectedObjectPath.Substring(12);
                }
                if (container3.AffectedObjectPath.StartsWith("/CutIns/"))
                {
                    name = container3.AffectedObjectPath.Substring(8);
                    flag = true;
                }
                if (container3.AffectedObjectPath.StartsWith("/BattleCamera") || container3.AffectedObjectPath.StartsWith("/Cameras/BattleCamera"))
                {
                    container3.AffectedObject = this.actorCamera.transform;
                }
                else if (name != null)
                {
                    if (this.isValidObject(name))
                    {
                        GameObject obj4 = this.searchPrefab(this.chrId, name);
                        if (obj4 != null)
                        {
                            this.createdObjects.Add(obj4);
                            obj4.SetActive(false);
                            container3.AffectedObject = obj4.transform;
                            if (flag)
                            {
                                Vector3 localPosition = obj4.transform.localPosition;
                                obj4.transform.parent = this.CutInRoot.transform;
                                obj4.transform.localPosition = localPosition;
                                this.updateCutInEvents(evs, name, obj4);
                                NGUITools.SetLayer(obj4, LayerMask.NameToLayer("BattleCutIn"));
                            }
                            else if (obj4.layer != LayerMask.NameToLayer("BattleBG"))
                            {
                                NGUITools.SetLayer(obj4, LayerMask.NameToLayer("Battle2D"));
                            }
                        }
                    }
                    foreach (USTimelineEvent event6 in container3.GetComponentsInChildren<USTimelineEvent>(true))
                    {
                        for (int j = 0; j < event6.EventCount; j++)
                        {
                            USEventBase base3 = event6.Event(j);
                            if (base3.name == "USFGOAttachToParentEvent")
                            {
                                USFGOAttachToParentEvent event7 = (USFGOAttachToParentEvent) base3;
                                if (event7.nodeName == "BattleCamera")
                                {
                                    event7.parentObject = this.actorCamera.transform;
                                }
                                else
                                {
                                    event7.SetupTarget(this.performance, this.actor);
                                }
                            }
                        }
                    }
                }
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
        BattleActorControl control = this.actor.GetComponent<BattleActorControl>();
        if ((((control != null) && !control.IsEnemy) && ((control.BattleSvtData.followerType == Follower.Type.NONE) && (this.Performance != null))) && (this.Performance.data != null))
        {
            int condValue = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<VoiceMaster>(DataNameKind.Kind.VOICE).getFlagRequestNumber(this.chrId, "_" + Voice.getFileName(Voice.BATTLE.HOUGU1_1), false);
            if (condValue != 0)
            {
                this.Performance.data.AddServantVoicePlayed(this.chrId, condValue);
            }
        }
        GC.Collect();
    }

    public void SetupDemoActor(GameObject[] playerList, GameObject[] enemyList)
    {
        USSequencer component = this.seqObject.GetComponent<USSequencer>();
        for (int i = 0; i < 3; i++)
        {
            USTimelineContainer container = this.searchTimeline(component, "Actors/Actor" + (i + 1));
            if ((container != null) && (i < playerList.Length))
            {
                container.AffectedObject = playerList[i].transform;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            USTimelineContainer container2 = this.searchTimeline(component, "Actors/Actor" + (j + 4));
            if ((container2 != null) && (j < enemyList.Length))
            {
                container2.AffectedObject = enemyList[j].transform;
            }
        }
    }

    protected void SetupTargetInfo()
    {
        this.SingleTarget = null;
        BattleActorControl component = this.actor.GetComponent<BattleActorControl>();
        if (component != null)
        {
            BattleActionData actionData = component.ActionData;
            HashSet<int> set = new HashSet<int>();
            foreach (BattleActionData.DamageData data2 in actionData.getDamageList(-1))
            {
                if (data2.targetId != 0)
                {
                    set.Add(data2.targetId);
                }
            }
            foreach (BattleActionData.BuffData data3 in actionData.getBuffList(-1))
            {
                if (data3.targetId != 0)
                {
                    set.Add(data3.targetId);
                }
            }
            foreach (BattleActionData.HealData data4 in actionData.getHealList(-1))
            {
                if (data4.targetId != 0)
                {
                    set.Add(data4.targetId);
                }
            }
            int num4 = 0;
            int num5 = 0;
            bool isEnemy = component.IsEnemy;
            HashSet<int> set2 = new HashSet<int>();
            foreach (int num6 in set)
            {
                if (this.performance.getServantGameObject(num6).GetComponent<BattleActorControl>().IsEnemy != isEnemy)
                {
                    num4++;
                    set2.Add(num6);
                }
                else
                {
                    num5++;
                }
            }
            if ((num4 == 1) && (num5 != 0))
            {
                set = set2;
            }
            if (set.Count == 0)
            {
                Debug.LogError("SearchTargetAndModifyPositions: can not found target!!");
            }
            else if (set.Count == 1)
            {
                int uniqueId = -1;
                foreach (int num8 in set)
                {
                    uniqueId = num8;
                }
                GameObject obj3 = this.performance.getServantGameObject(uniqueId);
                BattleActorControl control3 = obj3.GetComponent<BattleActorControl>();
                this.SingleTarget = obj3;
            }
        }
    }

    public void setWhiteInFlg(bool flg)
    {
        this.whiteInFlg = flg;
    }

    protected void ShowNobleInfo()
    {
        this.performance.showNobleInfo(-1, 1, 100, false);
    }

    private void Start()
    {
    }

    public void startFadeBattleUI(float time, float targetAlpha)
    {
        this.FadeBattleUI(time, targetAlpha);
    }

    private void Update()
    {
        if (this.delayInvokeTimer > 0f)
        {
            this.delayInvokeTimer -= Time.deltaTime;
            if (this.delayInvokeTimer <= 0f)
            {
                foreach (System.Action action in this.DelayInvokeMethodList)
                {
                    if (action != null)
                    {
                        action();
                    }
                }
                this.DelayInvokeMethodList.Clear();
            }
        }
    }

    private bool updateCutInEvents(List<USFGOPlayCutInEvent> evs, string cutInName, GameObject obj)
    {
        foreach (USFGOPlayCutInEvent event2 in evs)
        {
            if (event2.cutInName == cutInName)
            {
                event2.cutInObject = obj;
                return true;
            }
        }
        return false;
    }

    [DebuggerHidden]
    private IEnumerator WaitEndSequence() => 
        new <WaitEndSequence>c__Iterator19 { <>f__this = this };

    public bool IsAccelerateMode =>
        (((this.performance != null) && (this.performance.data != null)) && (this.performance.data.systemflg_acceleration > 1));

    [HideInInspector]
    public BattlePerformance Performance =>
        this.performance;

    [CompilerGenerated]
    private sealed class <loadSequence>c__AnonStorey7B
    {
        internal BattleSequenceManager <>f__this;
        internal int chrId;
        internal int limitCount;
        internal BattleSequenceManager.onGameObjectLoadComplete onComplete;

        internal void <>m__98(GameObject obj)
        {
            <loadSequence>c__AnonStorey7C storeyc = new <loadSequence>c__AnonStorey7C {
                <>f__ref$123 = this,
                obj = obj
            };
            this.<>f__this.seqObject = storeyc.obj;
            int num = SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.getVoiceId(this.chrId, this.limitCount);
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("NoblePhantasm_" + num, new System.Action(storeyc.<>m__9A), SoundManager.CueType.ALL);
        }

        private sealed class <loadSequence>c__AnonStorey7C
        {
            internal BattleSequenceManager.<loadSequence>c__AnonStorey7B <>f__ref$123;
            internal GameObject obj;

            internal void <>m__9A()
            {
                this.<>f__ref$123.<>f__this.standFigure = StandFigureManager.CreateMeshPrefab(this.<>f__ref$123.<>f__this.performance.root_field.gameObject, this.<>f__ref$123.chrId, this.<>f__ref$123.<>f__this.actorLimitImgCount, this.<>f__ref$123.<>f__this.faceType, 0, delegate {
                    if (this.<>f__ref$123.<>f__this.standFigure != null)
                    {
                        this.<>f__ref$123.<>f__this.standFigure.SetActive(false);
                    }
                    if (this.<>f__ref$123.onComplete != null)
                    {
                        this.<>f__ref$123.onComplete(this.obj);
                    }
                });
                this.<>f__ref$123.<>f__this.standFigure.SetActive(false);
            }

            internal void <>m__9B()
            {
                if (this.<>f__ref$123.<>f__this.standFigure != null)
                {
                    this.<>f__ref$123.<>f__this.standFigure.SetActive(false);
                }
                if (this.<>f__ref$123.onComplete != null)
                {
                    this.<>f__ref$123.onComplete(this.obj);
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <WaitEndSequence>c__Iterator19 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleSequenceManager <>f__this;

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
                case 1:
                    if (this.<>f__this.IsBgBusy)
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        return true;
                    }
                    this.<>f__this.OnNoblePhantasmPlayCompleteProc();
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

    public delegate void onGameObjectLoadComplete(GameObject obj);
}

