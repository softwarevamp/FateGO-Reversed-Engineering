using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleServantParamComponent : BaseMonoBehaviour
{
    public UISprite bg;
    public ServantClassIconComponent clsIconComponent;
    private BattleServantData data;
    private GameObject[] effectobj = new GameObject[1];
    public UITexture facetex;
    public UISprite friendIcon;
    public BattleHpGaugeBarComponent hpGauge;
    public UILabel hplabel;
    public int index;
    public UILabel levelLabel;
    public UILabel maxhplabel;
    private WINDOW_MODE modeWindow;
    public UILabel nameLabel;
    public BattleNextTDgaugeComponent nextTdGauge;
    public BattleNpGaugeSystemComponent npcomp;
    private BattlePerformance perf;
    public UISprite roleTypeSprite;
    public GameObject root;
    public UILabel shortNameLabel;
    public BattleServantShowBuffComponent showBuffComponent;
    public BattleServantSkillIConComponent[] skillIcon;
    public Transform skillRoot;
    public GameObject target;
    public GameObject targetMark;
    public UILabel totalhplabel;
    public int uniqueID;

    public void callSkillIcon()
    {
        if ((this.data != null) && this.data.isChargeSkill)
        {
            SoundManager.playSe("ba3");
            this.data.isChargeSkill = false;
            for (int i = 0; i < this.skillIcon.Length; i++)
            {
                this.skillIcon[i].showChageEffect();
            }
        }
    }

    public void changeHp(BattleServantData svtdata)
    {
        this.updateHplabel(svtdata.getNowHp(), svtdata.getMaxHp());
        this.updateHpbar(svtdata.getNowHp(), svtdata.getMaxHp());
    }

    public bool checkId(int Id) => 
        this.data?.checkID(Id);

    public void clickSkillIcon(BattleSkillInfoData skillInfo)
    {
        if (this.target != null)
        {
            this.target.GetComponent<BattlePerformancePlayer>().onClickSkillIcon(skillInfo);
        }
    }

    public void fixUpdateStatus()
    {
        this.initUpdateView();
    }

    public BattleServantData getData() => 
        this.data;

    public void initUpdateView()
    {
        if (this.data != null)
        {
            if (this.facetex != null)
            {
                this.facetex = ServantAssetLoadManager.loadStatusFace(this.facetex, this.data.getSvtId(), this.data.getDispLimitCount());
            }
            this.setClassIcon();
            if (this.levelLabel != null)
            {
                this.levelLabel.text = this.data.getLevelLabel();
            }
            if (this.nameLabel != null)
            {
                this.nameLabel.text = this.data.getServantName();
            }
            if (this.shortNameLabel != null)
            {
                this.shortNameLabel.text = this.data.getServantShortName();
            }
            if (this.hpGauge != null)
            {
                this.hpGauge.setInitValue(this.data.getNowHp(), this.data.getMaxHp());
            }
            this.updateHplabel(this.data.getNowHp(), this.data.getMaxHp());
            if (this.npcomp != null)
            {
                this.npcomp.setLineCount(3);
                this.npcomp.setMaxParam(this.data.getCountMaxNp());
                this.npcomp.setNowParam(this.data.np);
                this.npcomp.setUseNp(this.data.isAddNpGauge());
            }
            if (this.nextTdGauge != null)
            {
                this.nextTdGauge.setInitGauge(this.data.getNextTDTurn(), this.data.getMaxNextTDTurn());
            }
            this.updateSkillIcon(false);
            if (this.friendIcon != null)
            {
                this.friendIcon.gameObject.SetActive(false);
                if (this.data.followerType != Follower.Type.NONE)
                {
                    this.friendIcon.spriteName = FileName.friendIconName;
                    this.friendIcon.gameObject.SetActive(true);
                }
                if (this.data.flgEventJoin)
                {
                    this.friendIcon.spriteName = FileName.eventJoinIconName;
                    this.friendIcon.gameObject.SetActive(true);
                }
            }
            this.setRoleTyoe();
        }
    }

    public bool isNone() => 
        (this.uniqueID == -1);

    public void OnClick()
    {
        Debug.Log("OnClick::");
        if (((this.target != null) && !this.isNone()) && (this.data != null))
        {
            this.target.SendMessage("onClickServant", this.data.getUniqueID());
        }
    }

    public void onClickEnemyTarget()
    {
        if (this.perf.statusPerf.masterPerf.isCloseEnemyConf())
        {
            this.perf.clickTarget(this.index);
        }
    }

    public void onLongPressEnemyTarget()
    {
        this.perf.statusPerf.masterPerf.showEnemyServant(this.index);
    }

    public void playAttackEffect()
    {
        if (this.effectobj[0] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[0]);
        }
        if (this.facetex != null)
        {
            this.effectobj[0] = base.createObject("effect/ef_cwflash01", this.facetex.gameObject.transform, null);
        }
    }

    public void playCloseSelectServant()
    {
        if (this.facetex != null)
        {
            TweenColor.Begin(this.facetex.gameObject, 0.4f, Color.gray);
        }
        this.setTouch(true);
    }

    public void playEndShowServant()
    {
        if (this.facetex != null)
        {
            TweenColor.Begin(this.facetex.gameObject, 0.4f, Color.white);
        }
        this.setTouch(true);
    }

    public void playSelectServant()
    {
        if (this.facetex != null)
        {
            TweenColor.Begin(this.facetex.gameObject, 0.4f, Color.white);
        }
        this.setTouch(true);
    }

    public void playStartShowServant()
    {
    }

    public void setClassIcon()
    {
        if (this.clsIconComponent != null)
        {
            int classId = this.data.getClassId();
            int frameType = this.data.frameType;
            this.clsIconComponent.setImage(classId, frameType);
        }
    }

    public void setCloseMode()
    {
        this.setSkillFlash(false);
        if (this.facetex != null)
        {
            TweenColor.Begin(this.facetex.gameObject, 0.4f, Color.clear);
        }
        if (this.modeWindow == WINDOW_MODE.OPEN)
        {
            base.GetComponent<Animation>()["SvtW_StartClose"].time = 0f;
            base.GetComponent<Animation>()["SvtW_StartClose"].speed = 1f;
            base.GetComponent<Animation>().Play("SvtW_StartClose");
        }
        else if (this.modeWindow != WINDOW_MODE.CLOSE)
        {
            base.GetComponent<Animation>()["SvtW_StartClose"].time = 0f;
            base.GetComponent<Animation>()["SvtW_StartClose"].speed = 1f;
            base.GetComponent<Animation>().Play("SvtW_StartClose");
        }
        this.modeWindow = WINDOW_MODE.CLOSE;
    }

    public void setData(BattleServantData data)
    {
        this.modeWindow = WINDOW_MODE.INIT;
        this.data = data;
        if (this.data != null)
        {
            this.uniqueID = this.data.getUniqueID();
        }
        else
        {
            if (this.npcomp != null)
            {
                this.npcomp.resetSlider();
            }
            this.uniqueID = -1;
        }
        this.initUpdateView();
        this.updateView();
        this.setTargetMark(-1);
    }

    public void setOpenMode()
    {
        this.setSkillFlash(true);
        if (this.facetex != null)
        {
            TweenColor.Begin(this.facetex.gameObject, 0.4f, Color.white);
        }
        if (this.modeWindow == WINDOW_MODE.INIT)
        {
            base.GetComponent<Animation>()["SvtW_StartClose"].time = base.GetComponent<Animation>()["SvtW_StartClose"].length;
            base.GetComponent<Animation>()["SvtW_StartClose"].speed = -1f;
            base.GetComponent<Animation>().Play("SvtW_StartClose");
            this.setTouch(true);
        }
        else if (this.modeWindow != WINDOW_MODE.OPEN)
        {
            base.GetComponent<Animation>()["SvtW_StartClose"].time = base.GetComponent<Animation>()["SvtW_StartClose"].length;
            base.GetComponent<Animation>()["SvtW_StartClose"].speed = -1f;
            base.GetComponent<Animation>().Play("SvtW_StartClose");
        }
        this.modeWindow = WINDOW_MODE.OPEN;
        this.updateSkillIcon(false);
    }

    public void setPerf(BattlePerformance inperf)
    {
        this.perf = inperf;
    }

    public void setRoleTyoe()
    {
        if (this.roleTypeSprite != null)
        {
            if (this.data.isEnemy)
            {
                if (this.data.roleType == 2)
                {
                    this.roleTypeSprite.spriteName = "enemy_icon_leader";
                    this.roleTypeSprite.transform.localPosition = Vector3.zero;
                    this.roleTypeSprite.gameObject.SetActive(true);
                }
                else if (this.data.roleType == 3)
                {
                    this.roleTypeSprite.spriteName = "servant_icon";
                    this.roleTypeSprite.transform.localPosition = Vector3.zero;
                    this.roleTypeSprite.gameObject.SetActive(true);
                }
                else
                {
                    this.roleTypeSprite.gameObject.SetActive(false);
                }
            }
            else
            {
                this.roleTypeSprite.gameObject.SetActive(false);
            }
        }
    }

    public void setSkillFlash(bool flg)
    {
        for (int i = 0; i < this.skillIcon.Length; i++)
        {
            this.skillIcon[i].setflashFlg(flg);
        }
    }

    public void setTargetMark(int uniqueId)
    {
        if ((this.data != null) && (this.targetMark != null))
        {
            this.targetMark.gameObject.SetActive(this.data.getUniqueID() == uniqueId);
        }
    }

    public void setTouch(bool flg)
    {
        Collider component = base.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = flg;
        }
    }

    public void setVisible(bool flg)
    {
        this.root.SetActive(flg);
        Collider component = base.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = flg;
        }
    }

    public void updateBuffIcon(BattleBuffData buffData)
    {
        if (this.showBuffComponent != null)
        {
            this.showBuffComponent.setBuffList(buffData.getShowBuffList());
        }
    }

    public void updateBuffIconList(BattleServantData svtData)
    {
        if (((svtData != null) && (this.data != null)) && (svtData.getUniqueID() == this.data.getUniqueID()))
        {
            this.updateBuffIcon(svtData.buffData);
        }
    }

    public void updateHpbar(int now, int max)
    {
        if (0 <= this.data.hp)
        {
            if (this.hpGauge != null)
            {
                this.hpGauge.setValue(now, max);
            }
        }
        else if (this.hpGauge != null)
        {
            this.hpGauge.setZero();
        }
    }

    public void updateHplabel(int now, int max)
    {
        if (this.hplabel != null)
        {
            this.hplabel.text = string.Empty + now;
        }
        if (this.maxhplabel != null)
        {
            this.maxhplabel.text = string.Empty + max;
        }
        if (this.totalhplabel != null)
        {
            this.totalhplabel.text = $"{now}/{max}";
        }
    }

    public void updateNp(BattleServantData svtdata)
    {
        if ((svtdata.getUniqueID() == this.data.getUniqueID()) && (this.npcomp != null))
        {
            this.npcomp.changeParam(svtdata.np);
        }
    }

    public void updateSkillIcon(bool flg = false)
    {
        if (this.data != null)
        {
            BattleSkillInfoData[] dataArray = this.data.getActiveSkillInfos();
            for (int i = 0; i < this.skillIcon.Length; i++)
            {
                if (i < dataArray.Length)
                {
                    BattleSkillInfoData skillInfo = dataArray[i];
                    this.skillIcon[i].SetSkillInfo(skillInfo, this.data.isUseSkill());
                }
                else
                {
                    this.skillIcon[i].setNoSkill(0);
                }
            }
        }
    }

    public void updateTDGauge(BattleServantData svtData = null)
    {
        if (this.nextTdGauge != null)
        {
            if (this.data.hasTreasureDvc())
            {
                this.nextTdGauge.setValue(this.data.getNextTDTurn());
            }
            else
            {
                this.nextTdGauge.setHide();
            }
        }
    }

    public void updateView()
    {
        if (this.data == null)
        {
            this.setTouch(false);
            this.root.SetActive(false);
        }
        else if (!this.data.isAlive())
        {
            this.setTouch(false);
            this.root.SetActive(false);
        }
        else
        {
            this.root.SetActive(true);
            this.updateHplabel(this.data.getNowHp(), this.data.getMaxHp());
            this.updateHpbar(this.data.getNowHp(), this.data.getMaxHp());
            if (this.npcomp != null)
            {
                this.npcomp.setNowParam(this.data.np);
            }
            this.updateTDGauge(null);
            this.updateSkillIcon(true);
            this.updateBuffIcon(this.data.buffData);
        }
    }

    private enum WINDOW_MODE
    {
        NONE,
        INIT,
        OPEN,
        CLOSE
    }
}

