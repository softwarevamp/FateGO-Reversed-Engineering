using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using WellFired;

public class BattleDemoRootComponent : SceneRootComponent
{
    [SerializeField]
    protected ServantAssetLoadManager _ServantAssetLoadManager;
    [CompilerGenerated]
    private static System.Action <>f__am$cache18;
    [SerializeField]
    protected Camera actorCamera;
    [SerializeField]
    protected GameObject actorPrefab;
    protected float backupFov;
    protected System.Action BattleDemoCallback;
    protected BattleSetupInfo battleSetupInfo;
    [SerializeField]
    protected int BgId;
    [SerializeField]
    protected string BgmName;
    protected int BgType;
    [SerializeField]
    protected PlayMakerFSM CameraFsm;
    [SerializeField]
    protected GameObject CommonMotionPrefab;
    protected int CueSheetCount;
    [SerializeField]
    protected List<string> CueSheetList;
    [SerializeField]
    protected int DemoSequenceNumber;
    [SerializeField]
    protected int DemoSequenceServantId;
    protected GameObject[] EnemyActorList;
    [SerializeField]
    protected string[] EnemyList;
    [SerializeField]
    protected GameObject Field;
    [SerializeField]
    protected PlayMakerFSM FieldMotion;
    protected System.Action InitializeCallback;
    [SerializeField]
    protected BattlePerformanceBg perfBg;
    [SerializeField]
    protected BattlePerformance performance;
    protected GameObject[] PlayerActorList;
    [SerializeField]
    protected string[] PlayerList;

    public override void beginFinish()
    {
    }

    public override void beginInitialize()
    {
        Debug.Log("beginInitialize");
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, 0f, null);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp(object data)
    {
        string demoInfo = null;
        if (data is BattleSetupInfo)
        {
            this.battleSetupInfo = (BattleSetupInfo) data;
            demoInfo = this.battleSetupInfo.demoInfo;
        }
        this.InitializeBattleDemo(demoInfo, delegate {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
            base.sendMessageStartUp();
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = delegate {
                };
            }
            this.StartDemo(<>f__am$cache18);
        });
    }

    protected void CreateActorObject(int uniqueIdStart, string[] chrInfos, GameObject[] actors, bool isPlayer)
    {
        BattleFieldMotionComponent component = this.FieldMotion.gameObject.GetComponent<BattleFieldMotionComponent>();
        int num = uniqueIdStart;
        foreach (string str in chrInfos)
        {
            if (str != null)
            {
                Transform transform;
                int index = num - uniqueIdStart;
                int servantIdByInfo = this.GetServantIdByInfo(str);
                int limitCountByInfo = this.GetLimitCountByInfo(str);
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.actorPrefab);
                actors[index] = obj2;
                obj2.transform.parent = this.Field.transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localEulerAngles = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                BattleActorControl control = obj2.GetComponent<BattleActorControl>();
                control.setPerformance(this.performance);
                BattleServantData svtdata = new BattleServantData();
                Dictionary<string, int> fakeInfo = new Dictionary<string, int> {
                    ["index"] = num,
                    ["uniqueId"] = num,
                    ["userSvtId"] = num,
                    ["svtId"] = servantIdByInfo,
                    ["limitCount"] = limitCountByInfo
                };
                svtdata.SetFakeData(fakeInfo);
                if (isPlayer)
                {
                    control.setTypePlayer();
                }
                else
                {
                    control.setTypeEnemy();
                }
                control.setServantData(svtdata);
                if (isPlayer)
                {
                    control.setDirLeft();
                    control.setMotionListForDemo("_PLAYER", this.actorCamera.gameObject, this.CameraFsm.gameObject, this.CommonMotionPrefab);
                    transform = component.getPlayerPopTr(index);
                }
                else
                {
                    control.setDirRight();
                    control.setMotionListForDemo("_ENEMY", this.actorCamera.gameObject, this.CameraFsm.gameObject, this.CommonMotionPrefab);
                    transform = component.getEnemyPopPoint(index);
                }
                control.setCamera(this.actorCamera);
                control.setTargetObject(transform.gameObject);
                num++;
            }
        }
    }

    protected void CreateActors()
    {
        this.PlayerActorList = new GameObject[this.PlayerList.Length];
        this.EnemyActorList = new GameObject[this.EnemyList.Length];
        this.performance.PlayerActorList = this.PlayerActorList;
        this.performance.EnemyActorList = this.EnemyActorList;
        this.CreateActorObject(1, this.PlayerList, this.PlayerActorList, true);
        this.CreateActorObject(4, this.EnemyList, this.EnemyActorList, false);
        for (int i = 0; i < this.PlayerActorList.Length; i++)
        {
            this.FieldMotion.Fsm.Variables.GetFsmGameObject("PLAYER" + (i + 1)).Value = this.PlayerActorList[i];
        }
        for (int j = 0; j < this.EnemyActorList.Length; j++)
        {
            this.FieldMotion.Fsm.Variables.GetFsmGameObject("ENEMY" + (j + 1)).Value = this.EnemyActorList[j];
        }
        this.FieldMotion.Fsm.Variables.GetFsmObject("CameraFsm").Value = this.CameraFsm;
        foreach (GameObject obj2 in this.PlayerActorList)
        {
            if (obj2 != null)
            {
                obj2.GetComponent<BattleActorControl>().playMotion("MOTION_ENTRY");
            }
        }
        foreach (GameObject obj3 in this.EnemyActorList)
        {
            if (obj3 != null)
            {
                obj3.GetComponent<BattleActorControl>().playMotion("MOTION_ENTRY");
            }
        }
        this.FieldMotion.SendEvent("END_NP");
    }

    protected void EndRequestBattleSetup(string result)
    {
        Debug.Log("endBattleRequest:" + result);
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected int GetLimitCountByInfo(string info)
    {
        char[] separator = new char[] { ':' };
        return int.Parse(info.Split(separator)[1]);
    }

    protected int GetServantIdByInfo(string info)
    {
        char[] separator = new char[] { ':' };
        return int.Parse(info.Split(separator)[0]);
    }

    public bool GoToBattleEndTalk()
    {
        ScriptManager.PlayBattleEnd(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, new ScriptManager.CallbackFunc(this.OnBattleEndScript), false);
        return true;
    }

    public bool GoToTerminal()
    {
        BattleData.DeleteBattleAfterTalkResumeInfo();
        TerminalPramsManager.IsAutoResume = true;
        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
        return true;
    }

    public void InitializeBattleDemo(string demoInfo, System.Action callback)
    {
        this.ReleaseBattleDemo();
        if (demoInfo != null)
        {
            Dictionary<string, object> dictionary = Json.Deserialize(demoInfo) as Dictionary<string, object>;
            this.PlayerList = new string[3];
            this.EnemyList = new string[3];
            for (int i = 0; i < 3; i++)
            {
                string key = "Player" + (1 + i);
                if (dictionary.ContainsKey(key))
                {
                    this.PlayerList[i] = (string) dictionary[key];
                }
            }
            for (int j = 0; j < 3; j++)
            {
                string str2 = "Enemy" + (1 + j);
                if (dictionary.ContainsKey(str2))
                {
                    this.EnemyList[j] = (string) dictionary[str2];
                }
            }
            this.CueSheetList = new List<string>();
            for (int k = 0; k < 5; k++)
            {
                string str3 = "Sound" + (k + 1);
                if (dictionary.ContainsKey(str3))
                {
                    this.CueSheetList.Add((string) dictionary[str3]);
                }
            }
            this.BgId = (int) ((long) dictionary["Bg"]);
            this.BgType = (int) ((long) dictionary["BgType"]);
            this.BgmName = (string) dictionary["Bgm"];
            this.DemoSequenceServantId = (int) ((long) dictionary["Sequence"]);
        }
        this.InitializeCallback = callback;
        base.StartCoroutine(this.SetupBattleDemo());
    }

    protected void LoadBattleChrs(string[] chrlist)
    {
        HashSet<int> set = new HashSet<int>();
        foreach (string str in chrlist)
        {
            if (str != null)
            {
                int servantIdByInfo = this.GetServantIdByInfo(str);
                int limitCountByInfo = this.GetLimitCountByInfo(str);
                ServantAssetLoadManager.preloadServant(servantIdByInfo, limitCountByInfo);
                int weaponGroup = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(servantIdByInfo, limitCountByInfo).weaponGroup;
                set.Add(weaponGroup);
            }
        }
        foreach (int num5 in set)
        {
            ServantAssetLoadManager.preloadActorMotion(num5);
        }
    }

    protected void LoadCueSheet()
    {
        if (this.CueSheetList != null)
        {
            foreach (string str in this.CueSheetList)
            {
                this.CueSheetCount++;
                SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(str, (System.Action) (() => this.CueSheetCount--), SoundManager.CueType.ALL);
            }
        }
    }

    public void OnBattleDemoLoadComplete(GameObject obj)
    {
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.setup(null, true);
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.SetupDemoActor(this.PlayerActorList, this.EnemyActorList);
        base.StartCoroutine(this.WaitToNoblePhantasmPlay());
    }

    protected void OnBattleEndScript(bool isExit)
    {
        ScriptManager.LoadBattleEndGameDemo(this.battleSetupInfo.questId, this.battleSetupInfo.questPhase, false, delegate (string demoInfo) {
            if (demoInfo != null)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                this.battleSetupInfo.isBefore = false;
                this.beginStartUp(this.battleSetupInfo);
            }
            else
            {
                base.myFSM.SendEvent("GOTO_TERMINAL");
            }
        });
    }

    private void OnNoblePhantasmPlayComplete(USSequencer seq)
    {
        if (this.BattleDemoCallback != null)
        {
            BattleFBXComponent.EnableEvent = true;
            this.BattleDemoCallback();
        }
        this.performance.setupCameraFov(this.backupFov);
        foreach (GameObject obj2 in this.PlayerActorList)
        {
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                int svtId = component.getServantId();
                int limitCount = component.getLimitCount();
                ServantAssetLoadManager.unloadServant(svtId, limitCount);
                UnityEngine.Object.Destroy(obj2);
            }
        }
        foreach (GameObject obj3 in this.EnemyActorList)
        {
            if (obj3 != null)
            {
                BattleActorControl control2 = obj3.GetComponent<BattleActorControl>();
                int num5 = control2.getServantId();
                int num6 = control2.getLimitCount();
                ServantAssetLoadManager.unloadServant(num5, num6);
                UnityEngine.Object.Destroy(obj3);
            }
        }
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.releaseNoblePhantasm();
        this.ReleaseCueSheet();
        Resources.UnloadUnusedAssets();
        GC.Collect();
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, 1f, null);
        if (this.battleSetupInfo != null)
        {
            if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
            {
                SingletonMonoBehaviour<SceneManager>.Instance.popSceneRefresh(SceneManager.FadeType.BLACK, this.battleSetupInfo);
            }
            else if (this.battleSetupInfo.battleBefore)
            {
                if (!this.battleSetupInfo.isBefore)
                {
                    base.myFSM.SendEvent("GOTO_BATTLE");
                }
            }
            else if (this.battleSetupInfo.isBefore)
            {
                base.myFSM.SendEvent("GOTO_BATTLENDTALK");
            }
            else
            {
                base.myFSM.SendEvent("GOTO_TERMINAL");
            }
        }
        else
        {
            base.myFSM.SendEvent("END_DEMO");
        }
    }

    protected void ReleaseBattleDemo()
    {
    }

    protected void ReleaseCueSheet()
    {
        if (this.CueSheetList != null)
        {
            foreach (string str in this.CueSheetList)
            {
                SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage(str);
            }
            this.CueSheetList = null;
        }
    }

    public bool RequestBattleSetup()
    {
        if (this.battleSetupInfo != null)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_TIP);
            BattleSetupInfo battleSetupInfo = this.battleSetupInfo;
            this.battleSetupInfo = null;
            NetworkManager.getRequest<BattleSetupRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestBattleSetup)).beginRequest(battleSetupInfo.questId, battleSetupInfo.questPhase, battleSetupInfo.deckId, battleSetupInfo.followerId, battleSetupInfo.followerClassId, 1);
        }
        return true;
    }

    [DebuggerHidden]
    protected IEnumerator SetupBattleDemo() => 
        new <SetupBattleDemo>c__Iterator30 { <>f__this = this };

    public bool StartBattle()
    {
        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Battle, SceneManager.FadeType.BLACK, null);
        return true;
    }

    public void StartDemo(System.Action callback)
    {
        this.BattleDemoCallback = callback;
        GameObject actor = this.PlayerActorList[0];
        BattleActorControl component = actor.GetComponent<BattleActorControl>();
        this.FieldMotion.Fsm.Variables.GetFsmGameObject("NPACTOR").Value = actor;
        BattleFBXComponent.EnableEvent = false;
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.init(this.performance, actor, this.PlayerActorList, this.EnemyActorList, this.actorCamera, this.perfBg.bgobject);
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.loadSequence(this.DemoSequenceServantId, this.DemoSequenceServantId, component.getLimitCount(), new BattleSequenceManager.onGameObjectLoadComplete(this.OnBattleDemoLoadComplete));
    }

    private void Update()
    {
    }

    [DebuggerHidden]
    private IEnumerator WaitToNoblePhantasmPlay() => 
        new <WaitToNoblePhantasmPlay>c__Iterator31 { <>f__this = this };

    [CompilerGenerated]
    private sealed class <SetupBattleDemo>c__Iterator30 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleDemoRootComponent <>f__this;

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
                    this.<>f__this.performance.loadBg(this.<>f__this.BgId, this.<>f__this.BgType);
                    this.<>f__this.performance.CurrentGroundNo = this.<>f__this.BgId;
                    this.<>f__this.performance.CurrentGroundType = this.<>f__this.BgType;
                    this.<>f__this.LoadBattleChrs(this.<>f__this.PlayerList);
                    this.<>f__this.LoadBattleChrs(this.<>f__this.EnemyList);
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0100;

                case 3:
                    if (this.<>f__this.InitializeCallback != null)
                    {
                        this.<>f__this.InitializeCallback();
                    }
                    this.$PC = -1;
                    goto Label_0183;

                default:
                    goto Label_0183;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 1;
                goto Label_0185;
            }
            this.<>f__this.LoadCueSheet();
        Label_0100:
            while (this.<>f__this.CueSheetCount != 0)
            {
                this.$current = 0;
                this.$PC = 2;
                goto Label_0185;
            }
            this.<>f__this.CreateActors();
            this.<>f__this.CameraFsm.SendEvent("RESET_CAMERA_NOBLEEND");
            SoundManager.playBgm(this.<>f__this.BgmName);
            this.$current = new WaitForSeconds(1f);
            this.$PC = 3;
            goto Label_0185;
        Label_0183:
            return false;
        Label_0185:
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
    private sealed class <WaitToNoblePhantasmPlay>c__Iterator31 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleDemoRootComponent <>f__this;

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
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.performance.FlipAll(false);
                    this.<>f__this.backupFov = this.<>f__this.performance.currentFov;
                    this.<>f__this.performance.setupCameraFov(BattlePerformance.DefaultFov);
                    this.<>f__this.performance.actorcamera.transform.localEulerAngles = Vector3.zero;
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(1f, null);
                    SingletonMonoBehaviour<BattleSequenceManager>.Instance.play(false, true, new Action<USSequencer>(this.<>f__this.OnNoblePhantasmPlayComplete));
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
}

