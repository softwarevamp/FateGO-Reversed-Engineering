using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleServantHeadUpComponent : MonoBehaviour
{
    private const float buff_changeTime = 10f;
    private float buff_time;
    public BattleServantBuffIconComponent buffIcon;
    public ServantClassIconComponent clsIconComponent;
    private bool flgTarget;
    public GameObject hpclassRoot;
    public BattleHpGaugeBarComponent hpGauge;
    public int index;
    private MODE mode;
    public BattleNextTDgaugeComponent nextTdGauge;
    public UISprite roleTypeSprite;
    public GameObject rootObject;
    public GameObject targetMark;
    public GameObject targetRoot;
    public BattleServantData tmp_svtData;
    private float viewTime;
    private int wk_buffindex;
    private int[] wk_bufflist = new int[0];

    public void changeHp(BattleServantData svtdata)
    {
        if (this.updateHpbar(svtdata.getNowHp(), svtdata.getMaxHp()))
        {
            this.ShowStatus(true);
            this.setViewTime(2f);
        }
    }

    public void setClassIcon(int clsId, int rarity)
    {
        if (this.clsIconComponent != null)
        {
            this.clsIconComponent.Set(clsId, rarity);
        }
    }

    public void setData(BattleServantData svtData)
    {
        this.tmp_svtData = svtData;
        if (this.tmp_svtData == null)
        {
            this.ShowStatus(false);
        }
        else
        {
            this.ShowStatus(true);
            this.targetMark.gameObject.SetActive(false);
            this.setClassIcon(svtData.getClassId(), svtData.getRarity());
            this.changeHp(this.tmp_svtData);
            this.updateTDGauge(null);
            this.updateBuff();
            if (this.roleTypeSprite != null)
            {
                if (svtData.roleType == 2)
                {
                    this.roleTypeSprite.spriteName = "enemy_icon_leader";
                    this.roleTypeSprite.SetRect(0f, 0f, (float) this.roleTypeSprite.GetAtlasSprite().width, (float) this.roleTypeSprite.GetAtlasSprite().height);
                    this.roleTypeSprite.gameObject.SetActive(true);
                }
                else if (svtData.roleType == 3)
                {
                    this.roleTypeSprite.spriteName = "servant_icon";
                    this.roleTypeSprite.SetRect(0f, 0f, (float) this.roleTypeSprite.GetAtlasSprite().width, (float) this.roleTypeSprite.GetAtlasSprite().height);
                    this.roleTypeSprite.gameObject.SetActive(true);
                }
                else
                {
                    this.roleTypeSprite.gameObject.SetActive(false);
                }
            }
        }
    }

    public void setHpClassRoot(bool flg)
    {
        if (this.hpclassRoot != null)
        {
            this.hpclassRoot.SetActive(flg);
        }
    }

    public void setModeAction()
    {
        this.ShowStatus(false);
        this.mode = MODE.ACTION;
        this.setHpClassRoot(true);
        this.setTargetRoot(false);
    }

    public void setModeCommand()
    {
        this.ShowStatus(true);
        this.mode = MODE.COMMAND;
        this.setHpClassRoot(false);
        this.setTargetRoot(true);
    }

    public void setModeTac()
    {
        this.ShowStatus(true);
        this.mode = MODE.TAC;
        this.setHpClassRoot(true);
        this.setTargetRoot(true);
    }

    public void setModeWaveStart()
    {
        this.viewTime = 0f;
        this.ShowStatus(false);
        this.mode = MODE.WAVE_START;
    }

    public void setTargetMark(int uniqueId)
    {
        if (this.tmp_svtData != null)
        {
            this.targetMark.gameObject.SetActive(this.tmp_svtData.getUniqueID() == uniqueId);
        }
    }

    public void setTargetRoot(bool flg)
    {
        if (this.targetRoot != null)
        {
            this.targetRoot.SetActive(flg);
        }
    }

    public void setViewTime(float time)
    {
        this.ShowStatus(true);
        this.viewTime = time;
    }

    public void ShowStatus(bool flg)
    {
        if (this.tmp_svtData == null)
        {
            this.rootObject.SetActive(false);
        }
        else if (!this.tmp_svtData.isAlive())
        {
            this.rootObject.SetActive(false);
        }
        else
        {
            this.rootObject.SetActive(flg);
        }
    }

    public void Update()
    {
        if (this.tmp_svtData != null)
        {
            if (0f < this.viewTime)
            {
                this.viewTime -= Time.deltaTime;
            }
            if (0f < this.viewTime)
            {
                this.ShowStatus(true);
            }
            else if (this.mode == MODE.ACTION)
            {
                this.ShowStatus(false);
            }
            if (this.wk_bufflist.Length >= 2)
            {
                this.buff_time += Time.deltaTime;
                if (10f < this.buff_time)
                {
                    this.buff_time = 0f;
                    this.wk_buffindex = ((this.wk_buffindex + 1) >= this.wk_bufflist.Length) ? 0 : (this.wk_buffindex + 1);
                }
            }
        }
    }

    public void updateBuff()
    {
        if (this.tmp_svtData != null)
        {
            this.wk_buffindex = 0;
            this.buff_time = 0f;
            BattleBuffData.BuffData[] dataArray = this.tmp_svtData.getBuffData().getActiveList();
            int[] numArray = new int[dataArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = dataArray[i].buffId;
            }
            bool flag = false;
            flag = numArray.Length != this.wk_bufflist.Length;
            if (!flag)
            {
                for (int j = 0; j < numArray.Length; j++)
                {
                    flag |= numArray[j] != this.wk_bufflist[j];
                }
            }
            if (flag)
            {
                this.wk_bufflist = numArray;
                if (this.wk_bufflist.Length <= this.wk_buffindex)
                {
                    this.wk_buffindex = this.wk_bufflist.Length - 1;
                }
                this.setViewTime(2f);
            }
        }
    }

    public void updateBuffIconList(BattleServantData svtData)
    {
        this.updateBuff();
    }

    public bool updateHpbar(int now, int max)
    {
        if (0 <= now)
        {
        }
        return false;
    }

    public void updateTDGauge(BattleServantData svtData = null)
    {
        if (this.tmp_svtData.hasTreasureDvc())
        {
        }
    }

    public void updateView()
    {
        if (this.tmp_svtData != null)
        {
            if (!this.tmp_svtData.isAlive())
            {
                this.ShowStatus(false);
            }
            else
            {
                this.updateTDGauge(null);
                this.updateBuff();
                this.changeHp(this.tmp_svtData);
            }
        }
    }

    private enum MODE
    {
        NONE,
        WAVE_START,
        COMMAND,
        TAC,
        ACTION
    }
}

