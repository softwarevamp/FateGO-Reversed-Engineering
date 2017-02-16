using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleRootComponent : SceneRootComponent
{
    protected callBackBeginResume callbackBeginResumeFunc;
    public BattleData data;
    public GameObject debugButton;
    public BattleLogic logic;
    public TextAsset[] mockfile;
    public SceneList.Type nextscenetype;
    public BattlePerformance perf;
    public string works;

    public override void beginInitialize()
    {
        base.beginInitialize();
        GC.Collect();
        if (SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.BATTLE).getSingleEntity<BattleEntity>() != null)
        {
            this.mockfile = null;
            Debug.Log("***HTTP);");
        }
        else if (ManagerConfig.UseDebugCommand)
        {
            this.mockfile = new List<TextAsset>(this.mockfile) { (Resources.Load("Mock/TestBattle/MockSetUpBattle2") as TextAsset) }.ToArray();
        }
        BattleFBXComponent.EnableEvent = true;
        BattlePerformance.CameraFlip = false;
        this.data.Initialize();
        this.logic.Initialize();
        this.perf.Initialize();
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("Battle", delegate {
            Debug.Log("END_beginInitialize");
            SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        }, SoundManager.CueType.ALL);
        if (this.debugButton != null)
        {
            this.debugButton.SetActive(false);
            if (ManagerConfig.UseDebugCommand)
            {
                this.debugButton.SetActive(true);
            }
        }
    }

    public override void beginResume(object data)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        if (this.callbackBeginResumeFunc != null)
        {
            callBackBeginResume callbackBeginResumeFunc = this.callbackBeginResumeFunc;
            this.callbackBeginResumeFunc = null;
            callbackBeginResumeFunc();
        }
    }

    public override void beginStartUp(object data)
    {
        if (data != null)
        {
            Dictionary<string, int> dictionary = (Dictionary<string, int>) data;
            SingletonMonoBehaviour<BattleSequenceManager>.Instance.servantId = (int) ((long) dictionary["servantId"]);
            SingletonMonoBehaviour<BattleSequenceManager>.Instance.limitCount = (int) ((long) dictionary["limitCount"]);
            SingletonMonoBehaviour<BattleSequenceManager>.Instance.testMode = true;
            SingletonMonoBehaviour<BattleSequenceManager>.Instance.BackToTest = true;
            this.logic.logiclist = new BattleLogic.LOGICTYPE[] { BattleLogic.LOGICTYPE.PLAYER_SP1_ALWAYS, BattleLogic.LOGICTYPE.ENEMY_SP1_ALWAYS };
        }
        if (this.mockfile != null)
        {
            foreach (TextAsset asset in this.mockfile)
            {
                SingletonMonoBehaviour<DataManager>.Instance.updateJsonData(JsonManager.getDictionary(asset.text));
            }
        }
        base.beginStartUp();
        SingletonMonoBehaviour<AtlasManager>.getInstance().LoadBuffIconAtlas(new System.Action(this.endLoadIcon));
    }

    public void endLoadIcon()
    {
        base.myFSM.SendEvent("QUEST_START");
    }

    public void endQuest()
    {
        Time.timeScale = 1f;
        this.perf.procEndQuest();
        SingletonMonoBehaviour<AtlasManager>.getInstance().UnloadBuffIconAtlas();
        BattleData.FinishBattleInfo data = new BattleData.FinishBattleInfo {
            questId = this.data.getBattleEntity().questId,
            questPhase = this.data.getBattleEntity().questPhase,
            winLoseInfo = this.data.win_lose
        };
        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(this.nextscenetype, SceneManager.FadeType.BLACK, data);
    }

    public void setCallbackBeginResume(callBackBeginResume func)
    {
        this.callbackBeginResumeFunc = func;
    }

    public delegate void callBackBeginResume();
}

