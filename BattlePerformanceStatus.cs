using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattlePerformanceStatus : BaseMonoBehaviour
{
    public BattleBuffConfWindowComponent buffConfWindow;
    public DrumRollLabel criticalpointlabel;
    public Transform criticalpointTr;
    private BattleData data;
    public Transform dropItemTr;
    public GameObject effect_takecri;
    public BattlePerformanceEnemy enemyPref;
    public PlayMakerFSM fsm;
    public BattlePerformanceInfoComponent InfoComp;
    private BattleLogic logic;
    public BattlePerformanceMaster masterPerf;
    public UILabel maxWaveLabel;
    public UILabel nokoriEnemyCountLabel;
    public UILabel nowWaveLabel;
    private BattlePerformance perf;
    public BattlePerformancePlayer playerPerf;
    public BattleSelectMainSubServantWindow selectMainSubSvtWindow;
    public BattleSelectServantWindow selectSvtWindow;
    private TouchEventDelegate tapEvent;
    public GameObject tapObject;
    public BattleServantParamComponent targetparamlist;
    public GameObject waveShowObject;

    public void actionTouchEvent()
    {
        if (this.tapEvent != null)
        {
            this.tapEvent();
        }
    }

    public void changeLayer(GameObject obj)
    {
        Vector3 position = this.perf.actorcamera.WorldToViewportPoint(obj.transform.position);
        Vector3 vector2 = this.perf.uicamera.ViewportToWorldPoint(position);
        obj.transform.position = vector2;
    }

    public void clickServantWindow(int index)
    {
        this.fsm.FsmVariables.GetFsmInt("openSvtIndex").Value = index;
        this.fsm.SendEvent("CLICK_SVTWINDOW");
    }

    public void CloseBuffConf()
    {
        this.buffConfWindow.Close(null);
    }

    public void deleteEnemyStatus(int index)
    {
        this.enemyPref.deleteStatus(index);
    }

    public void deletePlayerStatus(int index)
    {
        this.playerPerf.deleteStatus(index);
    }

    public void endSkill()
    {
        this.perf.changeAttackButton(true, true, true);
        this.playerPerf.endSkill();
        this.enemyPref.endSkill();
        this.masterPerf.endSkill();
    }

    public Transform getCollectDropTransform() => 
        this.dropItemTr;

    public int getSelectedUniqueID() => 
        0;

    public BattleSelectMainSubServantWindow getSelectMainSubSvtWindow() => 
        this.selectMainSubSvtWindow;

    public BattleSelectServantWindow getSelectSvtWindow() => 
        this.selectSvtWindow;

    private void initActionTouch()
    {
        this.tapObject.SetActive(false);
        this.tapEvent = null;
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.data = indata;
        this.logic = inlogic;
        this.playerPerf.Initialize(this.perf, this.data, this.logic);
        this.masterPerf.Initialize(this.perf, this.data, this.logic);
        this.enemyPref.Initialize(this.perf, this.data, this.logic);
        this.selectSvtWindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.selectSvtWindow.setClose();
        this.selectMainSubSvtWindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.selectMainSubSvtWindow.setClose();
        this.buffConfWindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.buffConfWindow.setClose();
        if (this.targetparamlist != null)
        {
            this.targetparamlist.gameObject.SetActive(false);
        }
        this.InfoComp.Initialize();
        if (this.waveShowObject != null)
        {
            this.waveShowObject.SetActive(false);
        }
        this.initActionTouch();
    }

    public void loadMaster()
    {
        this.masterPerf.loadData();
    }

    public void modeAction()
    {
        this.enemyPref.startAction();
    }

    public void modeCommand()
    {
        this.playerPerf.startCommand();
        this.masterPerf.startCommand();
        this.enemyPref.startCommand();
        this.perf.changeAttackButton(false, false, true);
    }

    public void modeStartWave()
    {
        this.enemyPref.startWave();
    }

    public void modeTactical()
    {
        this.playerPerf.startTac();
        this.masterPerf.startTac();
        this.enemyPref.startTac();
        this.perf.changeAttackButton(true, true, true);
    }

    public void OpenBuffConf(int Id)
    {
        if (this.playerPerf.confwindowComp.isOpen())
        {
            if (this.buffConfWindow.checkBuffId(Id))
            {
                this.buffConfWindow.Close(null);
            }
            else
            {
                this.buffConfWindow.setClose();
                this.buffConfWindow.setData(Id);
                this.buffConfWindow.Open(null);
            }
        }
    }

    public void playAttackEffect(int uniqueID)
    {
        this.playerPerf.playAttackEffect(uniqueID);
    }

    public void setEnemyParam(int index, BattleServantData svtdata, GameObject obj)
    {
        this.enemyPref.setParam(index, svtdata, obj);
    }

    public void setOffTarget()
    {
        this.enemyPref.setOffTarget();
    }

    public void setPlayerParam(int index, BattleServantData svtdata)
    {
        this.playerPerf.setParam(index, svtdata);
    }

    public void setShowTurn(BattleData bdata, int addturn = 0)
    {
        this.InfoComp.setShowTurn(bdata, addturn);
    }

    public void setShowWave(int now, int max)
    {
        if (this.waveShowObject != null)
        {
            this.waveShowObject.SetActive(true);
            this.nowWaveLabel.text = $"{now + 1:D}";
            this.maxWaveLabel.text = $"{max + 1:D}";
        }
    }

    public void setTargetParam(BattleServantData svtdata)
    {
        this.enemyPref.setTarget(svtdata);
    }

    public void setTouchOff()
    {
        this.tapObject.SetActive(false);
        this.tapEvent = null;
    }

    public void setTouchOn(TouchEventDelegate inTapEvent)
    {
        this.tapObject.SetActive(false);
        this.tapEvent = inTapEvent;
        this.tapObject.SetActive(true);
    }

    public void startSkill()
    {
        this.perf.changeAttackButton(false, false, false);
        this.playerPerf.startSkill();
        this.enemyPref.startSkill();
        this.masterPerf.startSkill(0);
    }

    public void updateBuff()
    {
        this.playerPerf.updateBuff();
        this.enemyPref.updateBuff();
    }

    public void updateCriticalPoint()
    {
        int nextparam = this.data.getCriticalPoint();
        if (nextparam != this.criticalpointlabel.getCount())
        {
            if (0 < nextparam)
            {
                this.criticalpointlabel.changeParam(nextparam, null);
                base.createObject(this.effect_takecri, this.criticalpointTr, null).addNguiDepth(100, false);
            }
            else
            {
                this.criticalpointlabel.changeParam(nextparam, null);
            }
        }
    }

    public void updateDropItemCount()
    {
        this.masterPerf.updateDropItemCount();
    }

    public void updateNokoriEnemyCount()
    {
        BattleServantData[] dataArray = this.data.getEnemyServantList();
        int num = 0;
        foreach (BattleServantData data in dataArray)
        {
            if (data.isAlive())
            {
                num++;
            }
        }
        string key = "BATTLE_NOKORIENEMY";
        string format = LocalizationManager.Get(key);
        if (!format.Equals(key))
        {
            this.nokoriEnemyCountLabel.text = string.Format(format, num);
        }
        else
        {
            this.nokoriEnemyCountLabel.text = $"残り{num}体";
        }
    }

    public void updateView()
    {
        this.playerPerf.updateView();
        this.enemyPref.updateView();
    }

    public delegate void TouchEventDelegate();
}

