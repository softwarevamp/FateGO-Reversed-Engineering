using System;
using System.Collections.Generic;
using UnityEngine;

public class SvtEquipCombineControl : BaseMonoBehaviour
{
    private UserServantEntity baseData;
    private long baseId;
    public UISprite baseSelectImg;
    public GameObject baseSelectInfoLb;
    public UILabel baseSelectLb;
    private UICharaGraphTexture charaGraph;
    public GameObject charaGraphBase;
    private int checkLv;
    public UIButton combineBtn;
    public UISprite combineBtnBg;
    public UILabel combineBtnTxt;
    public CombineInfoComponent combineInfoComp;
    public CheckCombineResStatus combineResStatus;
    public UISprite combineTxtImg;
    public UILabel currentLvLb;
    public GameObject currentLvObj;
    public UILabel detailInfoLb;
    public UISprite eventNoticeImg;
    public SetRarityDialogControl exeCombineDlg;
    private int expType;
    private int getExpVal;
    public UILabel getSkillLb;
    public UILabel haveQpLb;
    public UILabel haveQpTitleLb;
    private int haveQpVal;
    private long[] highRarityList;
    private int increAmount;
    private int increLv;
    public UILabel increLvLb;
    public GameObject increLvObj;
    public UILabel increValLb;
    public GameObject increValObj;
    private bool isExeCombine;
    private bool isSelectBase;
    public GameObject materialBgObj;
    public UILabel materialSelectLb;
    public MenuListControl menuListCtr;
    public UILabel needQpLb;
    public UILabel needQpTitleLb;
    public UILabel preSelectBaseLb;
    public GameObject selectAddGridObj;
    public GameObject selectDetailInfoObj;
    public UIGrid selectGrid;
    public UIButton selectMaterialSvtBtn;
    private UserServantEntity selectMaterialSvtEqEntity;
    private long[] selectMtSvtEqList;
    public UILabel selectSumLb;
    private int spendQpVal;
    public GameObject svtFaceInfo;
    public PlayMakerFSM targetFsm;
    private int totalExp;
    private TargetType type;

    private bool checkIncrementLv(int lv)
    {
        int num = this.baseData.getLevelMax();
        if (lv < num)
        {
            ServantExpEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_EXP).getEntityFromId<ServantExpEntity>(this.expType, lv);
            if (entity.exp > this.totalExp)
            {
                this.increLv = entity.lv;
                return true;
            }
            if (entity.exp == this.totalExp)
            {
                this.increLv = entity.lv + 1;
                return true;
            }
            this.checkLv++;
            return false;
        }
        this.increLv = num;
        return true;
    }

    public void checkIsSelectBaseSvtEq(long selectBaseId)
    {
        if (selectBaseId > 0L)
        {
            if ((this.baseId > 0L) && (this.baseId != selectBaseId))
            {
                this.initMaterialSvtEqInfo();
            }
            this.isSelectBase = true;
        }
        else
        {
            this.initMaterialSvtEqInfo();
            this.isSelectBase = false;
        }
        this.baseId = selectBaseId;
    }

    public void checkRareSvt()
    {
        if (this.highRarityList.Length > 0)
        {
            this.targetFsm.SendEvent("SHOW_RAREDLG");
        }
        else
        {
            this.targetFsm.SendEvent("NO_RARESVT");
        }
    }

    public void destroyGrid()
    {
        int childCount = this.selectGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.selectGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public int getCombineExpVal() => 
        this.getExpVal;

    public bool getExeBtnState() => 
        this.isExeCombine;

    public UserServantEntity getMaterialUsrSvtData()
    {
        Debug.Log("******!! OnClickMaterial getMaterialUsrSvtData : " + this.selectMaterialSvtEqEntity.svtId);
        return this.selectMaterialSvtEqEntity;
    }

    public int getSpendQpVal() => 
        this.spendQpVal;

    public TargetType getTargetType() => 
        this.type;

    public void initMaterialSvtEqInfo()
    {
        this.isExeCombine = false;
        this.setExeBtnState();
        this.currentLvObj.SetActive(false);
        this.increLvObj.SetActive(false);
        this.increValObj.SetActive(false);
        this.currentLvLb.text = string.Empty;
        this.increLvLb.text = string.Empty;
        this.increValLb.text = string.Empty;
        this.getSkillLb.text = string.Empty;
        this.selectSumLb.text = string.Format(LocalizationManager.Get("SUM_INFO"), 0, BalanceConfig.ServantCombineMax);
        this.setHaveQpInfo();
        this.spendQpVal = 0;
        this.needQpLb.text = this.spendQpVal.ToString("N0");
        this.needQpLb.color = Color.white;
        this.getExpVal = 0;
        this.destroyGrid();
        this.isExeCombine = false;
        this.setExeBtnState();
        if (this.charaGraph != null)
        {
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
        this.combineInfoComp.initStatusInfo(CombineInfoComponent.DispType.SVTEQ_COMBINE);
    }

    public void initSvtEqCombine()
    {
        this.haveQpTitleLb.text = LocalizationManager.Get("QP_TAKE");
        this.needQpTitleLb.text = LocalizationManager.Get("NEED_QP_INFO");
        this.isSelectBase = false;
        this.baseId = 0L;
        this.initMaterialSvtEqInfo();
        this.selectMaterialSvtBtn.enabled = false;
        this.materialBgObj.SetActive(true);
        this.baseSelectInfoLb.SetActive(true);
        this.type = TargetType.BASE_STATUS;
        this.eventNoticeImg.gameObject.SetActive(false);
        this.preSelectBaseLb.text = LocalizationManager.Get("MSG_PRESELECT_BASE_SVTEQ");
        List<EventInfoData> list = this.menuListCtr.getCombineEventList();
        if ((list != null) && (list.Count > 0))
        {
            foreach (EventInfoData data in list)
            {
                if (data.type == 0x11)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 0x10)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 0x12)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 0x13)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
            }
        }
    }

    public void OnClickBase()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.targetFsm.SendEvent("SELECT_BASE");
    }

    public void OnClickExeCombine()
    {
        string msg = LocalizationManager.Get("CONFIRM_TITLE_SVTEQ_COMBINE");
        this.exeCombineDlg.setConfirmCombine(this.baseData, msg, this.spendQpVal, this.haveQpVal, false);
    }

    public void OnClickMaterial(ServantCombineControl.TargetType type, long selectUsrSvtId)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (type == ServantCombineControl.TargetType.MATERIAL_SELECT)
        {
            this.targetFsm.SendEvent("SELECT_MATERIAL");
        }
        if (type == ServantCombineControl.TargetType.MATERIAL_STATUS)
        {
            this.type = TargetType.MATERIAL_STATUS;
            this.selectMaterialSvtEqEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(selectUsrSvtId);
            this.targetFsm.SendEvent("SHOW_SVT_STATUS");
        }
    }

    public void OnLongPushListView()
    {
        if (this.baseId > 0L)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.type = TargetType.BASE_STATUS;
            this.targetFsm.SendEvent("SHOW_SVT_STATUS");
        }
    }

    public void setBaseSvtEqCardImg(UserServantEntity usrSvtData)
    {
        if (this.charaGraph == null)
        {
            this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, usrSvtData, true, 10, null);
        }
        else
        {
            this.charaGraph.SetCharacter(usrSvtData, true, null);
        }
        this.baseData = usrSvtData;
        this.combineInfoComp.setCurrentStatusInfo(this.baseData);
    }

    private void setExeBtnState()
    {
        UIWidget component = this.combineBtnBg.GetComponent<UIWidget>();
        TweenScale scale = this.combineTxtImg.GetComponent<TweenScale>();
        if (this.isExeCombine)
        {
            this.combineBtnTxt.color = Color.white;
            component.color = Color.white;
            scale.enabled = true;
            scale.PlayForward();
        }
        else
        {
            this.combineBtnTxt.color = Color.gray;
            component.color = Color.gray;
            scale.enabled = false;
        }
    }

    private void setHaveQpInfo()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.haveQpVal = entity.qp;
        this.haveQpLb.text = this.haveQpVal.ToString("N0");
    }

    public void setSelectMaterialEnable()
    {
        this.selectAddGridObj.SetActive(true);
        this.selectMaterialSvtBtn.enabled = this.isSelectBase;
        this.materialBgObj.SetActive(!this.isSelectBase);
        this.baseSelectInfoLb.SetActive(!this.isSelectBase);
    }

    public void setStateInfoMsg(CombineRootComponent.StateType state)
    {
        UIWidget component = this.detailInfoLb.GetComponent<UIWidget>();
        component.color = new Color(0f, 0.8789063f, 0.9882813f);
        string str = string.Empty;
        switch (state)
        {
            case CombineRootComponent.StateType.SELECT_BASE:
                str = LocalizationManager.Get("INFO_MSG_SVTEQ_BASE");
                break;

            case CombineRootComponent.StateType.SELECT_MATERIAL:
                str = LocalizationManager.Get("INFO_MSG_SVTEQ_MATERIAL");
                break;

            case CombineRootComponent.StateType.EXE_COMBINE:
                if (!this.isExeCombine)
                {
                    component.color = Color.white;
                    str = LocalizationManager.Get("SHORT_QP_INFO_MSG");
                    break;
                }
                str = LocalizationManager.Get("EXE_SUMMON_COMBINE_TXT");
                break;
        }
        this.detailInfoLb.text = str;
    }

    public void setSvtEqCombineData(SetCombineData data)
    {
        this.destroyGrid();
        int length = data.materialUsrSvtIdList.Length;
        this.selectSumLb.text = string.Format(LocalizationManager.Get("SUM_INFO"), length, BalanceConfig.ServantCombineMax);
        this.baseData = data.baseSvtData;
        int limitCount = this.baseData.limitCount;
        List<long> list = new List<long>();
        ServantLimitMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT);
        if (length > 0)
        {
            int num8;
            int num9;
            float num12;
            int num13;
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseData.svtId);
            int limitMax = entity.limitMax;
            for (int i = 0; i < length; i++)
            {
                long id = data.materialUsrSvtIdList[i];
                GameObject obj2 = base.createObject(this.svtFaceInfo, this.selectGrid.transform, null);
                obj2.transform.localPosition = Vector3.zero;
                UserServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id);
                if (master.getEntityFromId<ServantLimitEntity>(entity2.svtId, entity2.limitCount).rarity >= 3)
                {
                    list.Add(id);
                }
                bool overFlg = false;
                if ((this.baseData.svtId == entity2.svtId) && !this.baseData.isLimitCountMax())
                {
                    limitCount++;
                    if (limitCount > limitMax)
                    {
                        overFlg = true;
                    }
                }
                obj2.GetComponent<MaterialSvtInfo>().setMaterialSvtInfo(i, this.baseData, id, true, overFlg, new MaterialSvtInfo.ClickDelegate(this.OnClickMaterial));
            }
            this.highRarityList = list.ToArray();
            this.selectGrid.repositionNow = true;
            if (limitCount >= limitMax)
            {
                limitCount = limitMax;
            }
            this.getExpVal = data.getExp;
            this.expType = entity.expType;
            this.totalExp = this.getExpVal + this.baseData.exp;
            this.checkLv = this.baseData.lv;
            int maxLv = this.baseData.getLevelMax();
            if (this.checkLv != maxLv)
            {
                while (!this.checkIncrementLv(this.checkLv))
                {
                }
            }
            else
            {
                this.increLv = maxLv;
            }
            this.currentLvObj.SetActive(true);
            this.increLvObj.SetActive(true);
            this.increValObj.SetActive(true);
            this.currentLvLb.text = this.baseData.lv.ToString();
            this.increLvLb.text = this.increLv.ToString();
            this.increAmount = this.increLv - this.baseData.lv;
            this.increValLb.text = string.Format(LocalizationManager.Get("INCREMENT_SVTLEVEL"), this.increAmount);
            this.getSkillLb.text = this.combineResStatus.getSvtEqSkillByCombine(this.baseData, this.increLv, limitCount);
            this.spendQpVal = data.spendQp;
            this.needQpLb.text = this.spendQpVal.ToString("N0");
            this.combineResStatus.getCombineResStatus(out num8, out num9, this.baseData, this.increLv);
            num8 += data.getHpAdjustVal;
            num9 += data.getAtkAdjustVal;
            int num10 = limitCount;
            int lvMax = master.getEntityFromId<ServantLimitEntity>(this.baseData.svtId, num10).lvMax;
            CombineSvtData resSvtData = new CombineSvtData {
                baseSvtData = this.baseData,
                combineResSvtLv = this.increLv,
                combineResLimitCnt = num10,
                combineResSvtMaxLv = lvMax
            };
            this.combineResStatus.setSvtExp(out num12, out num13, this.totalExp, this.baseData.lv, maxLv, this.expType);
            resSvtData.combineResExpBarVal = num12;
            resSvtData.combineResNextExp = num13;
            resSvtData.combineResHp = num8;
            resSvtData.hpAdjustVal = data.getHpAdjustVal;
            resSvtData.combineResAtk = num9;
            resSvtData.atkAdjustVal = data.getAtkAdjustVal;
            this.combineInfoComp.setCombineResStatusInfo(resSvtData);
            if (this.haveQpVal < this.spendQpVal)
            {
                this.needQpLb.color = Color.red;
                this.isExeCombine = false;
                this.setExeBtnState();
            }
            else
            {
                this.needQpLb.color = Color.white;
                this.isExeCombine = true;
                this.setExeBtnState();
            }
        }
    }

    public void showRareSvtDlg()
    {
        string msg = LocalizationManager.Get("CONFIRM_TITLE_SVTEQ_COMBINE");
        int qp = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).qp;
        this.exeCombineDlg.setConfirmRarityInfo(this.baseData, this.highRarityList, msg, this.spendQpVal, qp);
    }

    public enum TargetType
    {
        BASE_STATUS,
        MATERIAL_SELECT,
        MATERIAL_STATUS
    }
}

