using System;
using System.Collections.Generic;
using UnityEngine;

public class ServantCombineControl : BaseMonoBehaviour
{
    private UserServantEntity baseData;
    private long baseId;
    public UISprite baseSelectImg;
    public GameObject baseSelectInfoLb;
    protected UICharaGraphTexture charaGraph;
    public GameObject charaGraphBase;
    private int checkLv;
    protected static readonly Color COLOR_VAL = new Color(0.99f, 0.945f, 0.316f);
    public UIButton combineBtn;
    public UISprite combineBtnBg;
    public UILabel combineBtnTxt;
    public CombineInfoComponent combineInfoComp;
    public CheckCombineResStatus combineResStatus;
    public UISprite combineTxtImg;
    public UIIconLabel currentAdjustAtkIconLabel;
    public UILabel currentAdjustAtkMaxLabel;
    public UIIconLabel currentAdjustHpIconLabel;
    public UILabel currentAdjustHpMaxLabel;
    public UILabel currentLvLb;
    public GameObject currentLvObj;
    public UILabel detailInfoLb;
    public UISprite eventNoticeImg;
    public SetRarityDialogControl exeCombineDlg;
    public UILabel expLb;
    private int expType;
    public UILabel getExpLb;
    private int getExpVal;
    public UILabel getSkillLb;
    public UILabel haveQpLb;
    private int haveQpVal;
    private long[] highRarityList;
    private int increAmount;
    private int increLv;
    public UILabel increLvLb;
    public GameObject increLvObj;
    public UILabel increValLb;
    private bool isExeCombine;
    private bool isSelectBase;
    public MenuListControl menuListCtr;
    public UILabel needQpLb;
    public UILabel preSelectBaseLb;
    public UILabel qpLb;
    public UIIconLabel resAdjustAtkIconLabel;
    public UILabel resAdjustAtkMaxLabel;
    public UIIconLabel resAdjustHpIconLabel;
    public UILabel resAdjustHpMaxLabel;
    public GameObject resAdjustInfo;
    public GameObject selectAddGridObj;
    public GameObject selectDetailInfoObj;
    public UIGrid selectGrid;
    public UIButton selectMaterialSvtBtn;
    private UserServantEntity selectMaterialUsrSvtEntity;
    public UILabel selectSumLb;
    private long[] selectUsrSvtIdList;
    private int spendQpVal;
    public GameObject svtFaceInfo;
    public PlayMakerFSM targetFsm;
    public UILabel testUsrSvtIdLb;
    private int totalExp;
    protected TargetType type;

    private bool checkIncrementLv(int lv)
    {
        int num = this.baseData.getLevelMax();
        Debug.Log("***!!*** checkIncrementLv totalExp : " + this.totalExp);
        if (lv < num)
        {
            ServantExpEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_EXP).getEntityFromId<ServantExpEntity>(this.expType, lv);
            Debug.Log(string.Concat(new object[] { "**!! getIncrementCombineLv IncrementLevel limitCount: ", this.baseData.limitCount, " _Level: ", lv }));
            Debug.Log(string.Concat(new object[] { "**!! getIncrementCombineLv IncrementLevel Exp: ", entity.exp, " _GetExp: ", this.totalExp }));
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

    public void checkIsSelectBaseSvt(long selectBaseId)
    {
        if (selectBaseId > 0L)
        {
            if ((this.baseId > 0L) && (this.baseId != selectBaseId))
            {
                Debug.Log("***!!! checkIsSelectBaseSvt baseId: " + this.baseId);
                this.initMaterialSvtInfo();
            }
            this.isSelectBase = true;
        }
        else
        {
            this.initMaterialSvtInfo();
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
        this.selectUsrSvtIdList = null;
    }

    public int getCombineExpVal() => 
        this.getExpVal;

    public bool getExeBtnState() => 
        this.isExeCombine;

    public UserServantEntity getMaterialUsrSvtData()
    {
        long id = this.selectMaterialUsrSvtEntity.id;
        return (this.selectMaterialUsrSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id));
    }

    public int getSpendQpVal() => 
        this.spendQpVal;

    public TargetType getTargetType() => 
        this.type;

    public void initMaterialSvtInfo()
    {
        this.testUsrSvtIdLb.text = string.Empty;
        this.currentLvObj.SetActive(false);
        this.increLvObj.SetActive(false);
        this.increValLb.gameObject.SetActive(false);
        this.resAdjustInfo.SetActive(false);
        this.currentLvLb.text = string.Empty;
        this.increLvLb.text = string.Empty;
        this.increValLb.text = string.Empty;
        this.getSkillLb.text = string.Empty;
        this.setHaveQpInfo();
        this.spendQpVal = 0;
        this.qpLb.text = this.spendQpVal.ToString("N0");
        this.qpLb.color = Color.white;
        this.getExpVal = 0;
        this.expLb.text = this.getExpVal.ToString();
        this.destroyGrid();
        this.isExeCombine = false;
        this.setExeBtnState();
        if (this.charaGraph != null)
        {
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
        this.combineInfoComp.initStatusInfo(CombineInfoComponent.DispType.SVT_COMBINE);
    }

    public void initSvtCombine()
    {
        this.needQpLb.text = LocalizationManager.Get("NEED_QP_INFO");
        this.getExpLb.text = LocalizationManager.Get("GET_EXP_INFO");
        this.isSelectBase = false;
        this.baseId = 0L;
        this.selectUsrSvtIdList = null;
        this.initMaterialSvtInfo();
        this.selectMaterialSvtBtn.enabled = false;
        this.preSelectBaseLb.text = LocalizationManager.Get("MSG_PRESELECT_BASE_SVT");
        this.preSelectBaseLb.gameObject.SetActive(true);
        this.baseSelectInfoLb.SetActive(true);
        this.isExeCombine = false;
        this.setExeBtnState();
        this.type = TargetType.BASE_STATUS;
        this.eventNoticeImg.gameObject.SetActive(false);
        List<EventInfoData> list = this.menuListCtr.getCombineEventList();
        if ((list != null) && (list.Count > 0))
        {
            foreach (EventInfoData data in list)
            {
                if (data.type == 2)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 1)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 4)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
                else if (data.type == 5)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
            }
        }
        this.currentAdjustHpIconLabel.Clear();
        this.currentAdjustHpMaxLabel.text = string.Empty;
        this.currentAdjustAtkIconLabel.Clear();
        this.currentAdjustAtkMaxLabel.text = string.Empty;
        this.resAdjustInfo.SetActive(false);
    }

    public void OnClickBase()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.targetFsm.SendEvent("SELECT_BASE");
    }

    public void OnClickExeCombine()
    {
        int qp = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).qp;
        string msg = LocalizationManager.Get("CONFIRM_TITLE_SVT_COMBINE");
        bool isStatusUp = false;
        for (int i = 0; i < this.selectUsrSvtIdList.Length; i++)
        {
            long id = this.selectUsrSvtIdList[i];
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id).IsStatusUp())
            {
                isStatusUp = true;
                break;
            }
        }
        this.exeCombineDlg.setConfirmCombine(this.baseData, msg, this.spendQpVal, qp, isStatusUp);
    }

    public void OnClickMaterial(TargetType type, long selectUsrSvtId)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (type == TargetType.MATERIAL_SELECT)
        {
            this.targetFsm.SendEvent("SELECT_MATERIAL");
        }
        if (type == TargetType.MATERIAL_STATUS)
        {
            this.type = TargetType.MATERIAL_STATUS;
            this.selectMaterialUsrSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(selectUsrSvtId);
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

    public void setBaseSvtCardImg(UserServantEntity usrSvtData)
    {
        int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(usrSvtData.svtId, usrSvtData.limitCount, true, true);
        if (this.charaGraph == null)
        {
            this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, usrSvtData, imageLimitCount, 10, null);
        }
        else
        {
            this.charaGraph.SetCharacter(usrSvtData, imageLimitCount, null);
        }
        this.baseData = usrSvtData;
        this.combineInfoComp.setCurrentStatusInfo(this.baseData);
        int maxAjustHp = 0;
        int maxAjustAtk = 0;
        this.baseData.GetAdjustMax(out maxAjustHp, out maxAjustAtk);
        if (this.baseData.adjustHp < maxAjustHp)
        {
            this.currentAdjustHpIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_HP, this.baseData.adjustHp * BalanceConfig.StatusUpAdjustHp, maxAjustHp * BalanceConfig.StatusUpAdjustHp, 0, 0L, false, false);
            this.currentAdjustHpMaxLabel.text = string.Empty;
        }
        else
        {
            this.currentAdjustHpIconLabel.Clear();
            this.currentAdjustHpMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
        }
        if (this.baseData.adjustAtk < maxAjustAtk)
        {
            this.currentAdjustAtkIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_ATK, this.baseData.adjustAtk * BalanceConfig.StatusUpAdjustAtk, maxAjustAtk * BalanceConfig.StatusUpAdjustAtk, 0, 0L, false, false);
            this.currentAdjustAtkMaxLabel.text = string.Empty;
        }
        else
        {
            this.currentAdjustAtkIconLabel.Clear();
            this.currentAdjustAtkMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
        }
    }

    public void setCombineData(SetCombineData data)
    {
        this.destroyGrid();
        this.selectUsrSvtIdList = data.materialUsrSvtIdList;
        int length = data.materialUsrSvtIdList.Length;
        this.baseData = data.baseSvtData;
        int limitCount = this.baseData.limitCount;
        List<long> list = new List<long>();
        if (length > 0)
        {
            int num7;
            int num8;
            int[] numArray;
            string[] strArray;
            float num11;
            int num12;
            for (int i = 0; i < length; i++)
            {
                long selectUsrSvtId = data.materialUsrSvtIdList[i];
                GameObject obj2 = base.createObject(this.svtFaceInfo, this.selectGrid.transform, null);
                obj2.transform.localPosition = Vector3.zero;
                obj2.GetComponent<MaterialSvtInfo>().setMaterialSvtInfo(i, this.baseData, selectUsrSvtId, true, false, new MaterialSvtInfo.ClickDelegate(this.OnClickMaterial));
                UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(selectUsrSvtId);
                if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(entity.svtId, entity.limitCount).rarity >= 3)
                {
                    list.Add(selectUsrSvtId);
                }
            }
            this.highRarityList = list.ToArray();
            this.selectGrid.repositionNow = true;
            this.spendQpVal = data.spendQp;
            this.qpLb.text = this.spendQpVal.ToString("N0");
            this.getExpVal = data.getExp;
            this.expLb.text = this.getExpVal.ToString();
            ServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseData.svtId);
            this.expType = entity3.expType;
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
            this.increValLb.gameObject.SetActive(true);
            this.currentLvLb.text = this.baseData.lv.ToString();
            this.increLvLb.text = this.increLv.ToString();
            this.increAmount = this.increLv - this.baseData.lv;
            this.increValLb.text = string.Format(LocalizationManager.Get("INCREMENT_SVTLEVEL"), this.increAmount);
            this.combineResStatus.getCombineResStatus(out num7, out num8, this.baseData, this.increLv);
            num7 += data.getHpAdjustVal;
            num8 += data.getAtkAdjustVal;
            this.baseData.getNextUseSkillInfo(out numArray, out strArray, this.increLv, limitCount);
            if ((strArray != null) && (strArray[0] != null))
            {
                string str = strArray[0];
                this.getSkillLb.text = str;
            }
            int num10 = this.baseData.limitCount;
            CombineSvtData resSvtData = new CombineSvtData {
                baseSvtData = this.baseData,
                combineResSvtLv = this.increLv,
                combineResLimitCnt = num10,
                combineResSvtMaxLv = maxLv
            };
            this.combineResStatus.setSvtExp(out num11, out num12, this.totalExp, this.baseData.lv, maxLv, this.expType);
            resSvtData.combineResExpBarVal = num11;
            resSvtData.combineResNextExp = num12;
            resSvtData.combineResHp = num7;
            resSvtData.hpAdjustVal = data.getHpAdjustVal;
            resSvtData.combineResAtk = num8;
            resSvtData.atkAdjustVal = data.getAtkAdjustVal;
            this.combineInfoComp.setCombineResStatusInfo(resSvtData);
            int maxAjustHp = 0;
            int maxAjustAtk = 0;
            this.baseData.GetAdjustMax(out maxAjustHp, out maxAjustAtk);
            this.resAdjustInfo.SetActive(true);
            UIWidget component = this.resAdjustHpIconLabel.GetComponent<UIWidget>();
            component.color = Color.white;
            UIWidget widget2 = this.resAdjustAtkIconLabel.GetComponent<UIWidget>();
            widget2.color = Color.white;
            int num15 = this.baseData.adjustHp * BalanceConfig.StatusUpAdjustHp;
            int num16 = data.getHpAdjustVal + num15;
            if (num16 < (maxAjustHp * BalanceConfig.StatusUpAdjustHp))
            {
                int num17 = (data.getHpAdjustVal > 0) ? num16 : num15;
                this.resAdjustHpIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_HP, num17, maxAjustHp * BalanceConfig.StatusUpAdjustHp, 0, 0L, false, false);
                component.color = !num15.Equals(num17) ? COLOR_VAL : Color.white;
                this.resAdjustHpMaxLabel.text = string.Empty;
            }
            else
            {
                this.resAdjustHpIconLabel.Clear();
                this.resAdjustHpMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
            }
            int num18 = this.baseData.adjustAtk * BalanceConfig.StatusUpAdjustAtk;
            int num19 = data.getAtkAdjustVal + num18;
            if (num19 < (maxAjustAtk * BalanceConfig.StatusUpAdjustAtk))
            {
                int num20 = (data.getAtkAdjustVal > 0) ? num19 : num18;
                this.resAdjustAtkIconLabel.Set(IconLabelInfo.IconKind.REINFORCEMENT_ATK, num20, maxAjustAtk * BalanceConfig.StatusUpAdjustAtk, 0, 0L, false, false);
                widget2.color = !num18.Equals(num20) ? COLOR_VAL : Color.white;
                this.resAdjustAtkMaxLabel.text = string.Empty;
            }
            else
            {
                this.resAdjustAtkIconLabel.Clear();
                this.resAdjustAtkMaxLabel.text = LocalizationManager.Get("COMMON_MAX");
            }
            if (this.haveQpVal < this.spendQpVal)
            {
                this.qpLb.color = Color.red;
                this.isExeCombine = false;
                this.setExeBtnState();
            }
            else
            {
                this.qpLb.color = Color.white;
                this.isExeCombine = true;
                this.setExeBtnState();
            }
        }
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
        this.preSelectBaseLb.gameObject.SetActive(!this.isSelectBase);
        this.selectMaterialSvtBtn.enabled = this.isSelectBase;
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
                str = LocalizationManager.Get("INFO_MSG_SVTCOMBINE_BASE");
                break;

            case CombineRootComponent.StateType.SELECT_MATERIAL:
                str = LocalizationManager.Get("INFO_MSG_COMBINE_MATERIAL");
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

    public void setTestLb(string usrSvtId)
    {
        this.testUsrSvtIdLb.text = usrSvtId;
    }

    public void showRareSvtDlg()
    {
        string msg = LocalizationManager.Get("CONFIRM_TITLE_SVT_COMBINE");
        int qp = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).qp;
        bool isStatusUp = false;
        for (int i = 0; i < this.selectUsrSvtIdList.Length; i++)
        {
            long id = this.selectUsrSvtIdList[i];
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id).IsStatusUp())
            {
                isStatusUp = true;
                break;
            }
        }
        this.exeCombineDlg.setConfirmInfo(this.baseData, this.highRarityList, msg, this.spendQpVal, qp, isStatusUp);
    }

    public enum TargetType
    {
        BASE_STATUS,
        MATERIAL_SELECT,
        MATERIAL_STATUS
    }
}

