using System;
using UnityEngine;

public class BattlePerformanceEnemy : MonoBehaviour
{
    private GameObject[] list_actor;
    private int[] list_ID = new int[3];
    private BattlePerformance perf;
    public BattleServantHeadUpComponent[] svtHeadUpList;
    public BattleServantParamComponent[] svtParamList;

    public void deleteStatus(int index)
    {
        if (this.svtHeadUpList[index] != null)
        {
            this.svtHeadUpList[index].setData(null);
            this.list_ID[index] = -1;
            this.svtHeadUpList[index].gameObject.GetComponent<TrackingMoveCtCComponent>().stopAct();
        }
        if (this.svtParamList[index] != null)
        {
            this.svtParamList[index].setData(null);
        }
    }

    public void endSkill()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setTargetRoot(true);
        }
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        for (int i = 0; i < this.svtParamList.Length; i++)
        {
            this.svtParamList[i].setPerf(this.perf);
        }
    }

    public void setOffTarget()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setTargetMark(-1);
        }
        for (int j = 0; j < this.svtParamList.Length; j++)
        {
            this.svtParamList[j].setTargetMark(-1);
        }
    }

    public void setParam(int index, BattleServantData svtdata, GameObject target)
    {
        this.svtHeadUpList[index].setData(svtdata);
        this.svtHeadUpList[index].index = index;
        this.list_ID[index] = svtdata.getUniqueID();
        TrackingMoveCtCComponent component = this.svtHeadUpList[index].gameObject.GetComponent<TrackingMoveCtCComponent>();
        BattleActorControl control = target.GetComponent<BattleActorControl>();
        component.Set(this.perf.actorcamera, this.perf.uicamera, control.getHeadUpObject(), control.getHeadUpY());
        component.startAct();
        if (this.svtParamList[index] != null)
        {
            this.svtParamList[index].setData(svtdata);
            this.svtParamList[index].index = index;
            this.svtParamList[index].setTouch(true);
            svtdata.addParamObject(this.svtParamList[index].gameObject);
        }
    }

    public void setTarget(BattleServantData svtData)
    {
        if (svtData == null)
        {
            Debug.Log("setTarget  null");
        }
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setTargetMark(svtData.getUniqueID());
        }
        for (int j = 0; j < this.svtParamList.Length; j++)
        {
            this.svtParamList[j].setTargetMark(svtData.getUniqueID());
        }
    }

    public void startAction()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setModeAction();
        }
    }

    public void startCommand()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setModeCommand();
        }
    }

    public void startSkill()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setTargetRoot(false);
        }
    }

    public void startTac()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setModeTac();
        }
    }

    public void startWave()
    {
        for (int i = 0; i < this.svtHeadUpList.Length; i++)
        {
            this.svtHeadUpList[i].setModeWaveStart();
        }
    }

    public void updateBuff()
    {
    }

    public void updateView()
    {
        for (int i = 0; i < this.svtParamList.Length; i++)
        {
            this.svtParamList[i].updateView();
        }
        for (int j = 0; j < this.svtHeadUpList.Length; j++)
        {
            this.svtHeadUpList[j].updateView();
        }
    }
}

