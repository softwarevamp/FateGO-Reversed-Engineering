using System;
using System.Collections.Generic;
using UnityEngine;

public class NpCombineControl : BaseMonoBehaviour
{
    public UILabel afterInfoLb;
    private UserServantEntity baseData;
    public GameObject baseLvInfo;
    public UISprite baseSelectImg;
    public GameObject baseSelectInfoLb;
    private int baseSvtId;
    protected string[] battleLoadList;
    private UICharaGraphTexture charaGraph;
    public GameObject charaGraphBase;
    public UIButton combineBtn;
    public UISprite combineBtnBg;
    public UILabel combineBtnTxt;
    private List<EventInfoData> combineEventList;
    public UIGrid combineTargetGrid;
    public UISprite combineTxtImg;
    public UILabel currentInfoLb;
    public UILabel detailInfoLb;
    public UISprite eventNoticeImg;
    public SetRarityDialogControl exeCombineDlg;
    public UILabel haveQpLb;
    private int haveQpVal;
    public UIIconLabel iconLabel;
    public UIGrid infoGrid;
    private bool isExceed;
    private bool isExeCombine;
    private bool isSelectBase;
    private SetLevelUpData lvUpData;
    public UIGrid materialGrid;
    public UILabel materialSelectLb;
    public GameObject materialSvtPrefab;
    public GameObject maxLvStatusInfo;
    public MenuListControl menuListCtr;
    public UILabel needQpLb;
    public GameObject npCombineInfoPrefab;
    private SvtUseNpData npData;
    public UILabel preSelectBaseLb;
    public UILabel qpLb;
    public GameObject selectAddGridObj;
    public GameObject selectDetailInfoObj;
    public UIButton selectMaterialSvtBtn;
    private UserServantEntity selectMaterialUsrSvtEntity;
    private List<long> selectMtUsrSvtIdList;
    private int spendQpVal;
    private SvtUseNpData svtNpData;
    public PlayMakerFSM targetFsm;
    private List<GameObject> targetList;
    private List<GameObject> tdList;
    private TargetType type;
    private int userQP;

    public void changeTargetInfo(bool isdecide, int idx)
    {
        if (this.baseSvtId > 0)
        {
            for (int i = 0; i < this.targetList.Count; i++)
            {
                ServantNpInfoIconComponent component = this.targetList[i].GetComponent<ServantNpInfoIconComponent>();
                if (i == idx)
                {
                    component.setDispSelectMskImg(true);
                }
                else
                {
                    component.setDispSelectMskImg(false);
                }
            }
            this.setSvtNpCombineData(idx);
        }
    }

    public bool checkTdLvMax(UserServantEntity resData)
    {
        int currentId = this.lvUpData.currentId;
        int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(currentId).maxLv;
        return (resData.getTreasureDeviceLv() >= maxLv);
    }

    public void destroyGrid()
    {
        int childCount = this.combineTargetGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.combineTargetGrid.transform.GetChild(i).gameObject);
            }
        }
        if (this.battleLoadList != null)
        {
            AssetManager.releaseAssetStorage(this.battleLoadList);
            this.battleLoadList = null;
        }
    }

    public void destroyMaterialGrid()
    {
        int childCount = this.materialGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.materialGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public UserServantEntity getBaseUsrSvtData()
    {
        long id = this.baseData.id;
        return (this.baseData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id));
    }

    public bool getExeBtnState() => 
        this.isExeCombine;

    public UserServantEntity getMaterialUsrSvtData()
    {
        long id = this.selectMaterialUsrSvtEntity.id;
        return (this.selectMaterialUsrSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id));
    }

    private SvtUseNpData getSvtNpData(UserServantEntity usrSvtData)
    {
        int[] numArray;
        int[] numArray2;
        int[] numArray3;
        int[] numArray4;
        int[] numArray5;
        string[] strArray;
        string[] strArray2;
        int[] numArray6;
        int[] numArray7;
        bool[] flagArray;
        usrSvtData.getTreasureDeviceInfo(out numArray, out numArray2, out numArray3, out numArray4, out numArray5, out strArray, out strArray2, out numArray6, out numArray7, out flagArray, -1);
        List<string> list = new List<string>();
        List<string> list2 = new List<string>();
        for (int i = 0; i < numArray.Length; i++)
        {
            if (numArray[i] > 0)
            {
                TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(numArray[i]);
                list.Add(entity.name);
                list2.Add(entity.ruby);
            }
        }
        this.npData = new SvtUseNpData();
        this.npData.svtUseNpIdList = numArray;
        this.npData.svtNpLvList = numArray2;
        this.npData.npRubyList = list2.ToArray();
        this.npData.svtNpNameList = list.ToArray();
        this.npData.npCardIdList = numArray7;
        this.npData.svtNpdetailList = strArray2;
        return this.npData;
    }

    public SetLevelUpData getTargetData() => 
        this.lvUpData;

    public TargetType getTargetType() => 
        this.type;

    private void initDispCombineInfo()
    {
        this.currentInfoLb.text = "-";
        this.afterInfoLb.text = "-";
        this.maxLvStatusInfo.SetActive(false);
        this.needQpLb.text = LocalizationManager.Get("NEED_QP_INFO");
        this.spendQpVal = 0;
        this.qpLb.text = this.spendQpVal.ToString();
        this.setHaveQpInfo();
        this.setSelectMaterialEnable();
        this.tdList = new List<GameObject>();
        for (int i = 0; i < 1; i++)
        {
            GameObject item = base.createObject(this.npCombineInfoPrefab, this.combineTargetGrid.transform, null);
            this.tdList.Add(item);
        }
        this.combineTargetGrid.repositionNow = true;
    }

    public void initSvtNpCombine()
    {
        this.isSelectBase = false;
        this.baseSvtId = 0;
        this.destroyGrid();
        this.destroyMaterialGrid();
        this.initDispCombineInfo();
        this.isExeCombine = false;
        this.setExeBtnState();
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.userQP = entity.qp;
        if (this.charaGraph != null)
        {
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
        this.eventNoticeImg.gameObject.SetActive(false);
        this.preSelectBaseLb.text = LocalizationManager.Get("MSG_PRESELECT_BASE_SVT");
        this.preSelectBaseLb.gameObject.SetActive(true);
        this.baseSelectInfoLb.SetActive(true);
        this.combineEventList = this.menuListCtr.getCombineEventList();
        if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
        {
            foreach (EventInfoData data in this.combineEventList)
            {
                if (data.type == 10)
                {
                    this.menuListCtr.setBannerIcon(this.eventNoticeImg, data.eventEntity);
                }
            }
        }
        this.baseLvInfo.SetActive(false);
    }

    public void OnClickBase()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.targetFsm.SendEvent("SELECT_BASE");
    }

    public void OnClickExeCombine()
    {
        this.exeCombineDlg.setNpCombineInfo(this.baseData, this.selectMtUsrSvtIdList[0], this.lvUpData, this.isExceed);
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
        if (this.baseSvtId > 0)
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
    }

    public void setBaseSvtNpInfo(UserServantEntity usrSvtEn)
    {
        if (usrSvtEn != null)
        {
            this.isSelectBase = true;
            this.baseSvtId = usrSvtEn.svtId;
            this.baseData = usrSvtEn;
            this.iconLabel.Set(IconLabelInfo.IconKind.LEVEL, usrSvtEn.lv, usrSvtEn.getLevelMax(), 0, 0L, false, false);
            this.baseLvInfo.SetActive(true);
            this.svtNpData = this.getSvtNpData(usrSvtEn);
            this.battleLoadList = new string[] { "Servants/" + this.baseData.getResourceFolder() };
            this.setCombineNpList();
            AssetManager.loadAssetStorage(this.battleLoadList, new System.Action(this.setCombineNpList));
        }
    }

    private void setCombineNpList()
    {
        int[] svtUseNpIdList = this.svtNpData.svtUseNpIdList;
        this.targetList = new List<GameObject>();
        for (int i = 0; i < svtUseNpIdList.Length; i++)
        {
            int npId = svtUseNpIdList[i];
            if (npId > 0)
            {
                int npLv = this.npData.svtNpLvList[i];
                int npCardId = this.npData.npCardIdList[i];
                string npRuby = this.npData.npRubyList[i];
                string npName = this.npData.svtNpNameList[i];
                string npDetail = this.npData.svtNpdetailList[i];
                GameObject item = this.tdList[i];
                item.GetComponent<ServantNpInfoIconComponent>().SetNpInfo(i, this.baseData, npId, npLv, npRuby, npName, npDetail, npCardId, new ServantNpInfoIconComponent.ClickDelegate(this.changeTargetInfo));
                this.infoGrid.transform.GetChild(i).gameObject.SetActive(false);
                this.targetList.Add(item);
            }
        }
        this.combineTargetGrid.repositionNow = true;
        this.changeTargetInfo(true, 0);
    }

    public void setEnableCombineBtn(bool isCombine)
    {
        this.combineBtn.isEnabled = isCombine;
        Color color = !isCombine ? Color.gray : Color.white;
        this.combineBtnTxt.color = color;
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

    public void setNpCombineData(SetCombineData data)
    {
        this.destroyMaterialGrid();
        int length = data.materialUsrSvtIdList.Length;
        this.selectMtUsrSvtIdList = new List<long>();
        if (length > 0)
        {
            int num4;
            int num5;
            for (int i = 0; i < length; i++)
            {
                long selectUsrSvtId = data.materialUsrSvtIdList[i];
                GameObject obj2 = base.createObject(this.materialSvtPrefab, this.materialGrid.transform, null);
                obj2.transform.localPosition = this.materialGrid.transform.localPosition;
                obj2.transform.localPosition = Vector3.zero;
                obj2.GetComponent<NpMaterialSvtInfo>().setMaterialSvtInfo(i, this.baseData, selectUsrSvtId, new NpMaterialSvtInfo.ClickDelegate(this.OnClickMaterial));
                this.selectMtUsrSvtIdList.Add(selectUsrSvtId);
            }
            this.materialGrid.repositionNow = true;
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.selectMtUsrSvtIdList[0]).getTreasureDeviceInfo(out num4, out num5);
            int num6 = this.baseData.checkTreasureDeviceLevelUp(num4);
            int currentLv = this.lvUpData.currentLv;
            int num8 = 0;
            for (int j = currentLv; j < num6; j++)
            {
                TreasureDvcLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(this.lvUpData.currentId, j);
                if (entity2 != null)
                {
                    num8 += entity2.qp;
                }
            }
            this.spendQpVal = num8;
            if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
            {
                foreach (EventInfoData data2 in this.combineEventList)
                {
                    if (data2.type == 10)
                    {
                        this.spendQpVal = (int) (this.spendQpVal * data2.value);
                    }
                }
            }
            this.qpLb.text = this.spendQpVal.ToString("N0");
            Color color = (this.haveQpVal >= this.spendQpVal) ? Color.white : Color.red;
            this.qpLb.color = color;
            this.lvUpData.nextLv = num6;
            this.lvUpData.spendQp = this.spendQpVal;
            this.isExceed = this.lvUpData.currentLv < num4;
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

    public void setSelectMaterialEnable()
    {
        this.preSelectBaseLb.gameObject.SetActive(!this.isSelectBase);
        this.baseSelectInfoLb.SetActive(!this.isSelectBase);
        this.selectMaterialSvtBtn.enabled = this.isSelectBase;
    }

    public void setStateInfoMsg(CombineRootComponent.StateType state)
    {
        UIWidget component = this.detailInfoLb.GetComponent<UIWidget>();
        component.color = new Color(0f, 0.8789063f, 0.9882813f);
        string str = string.Empty;
        switch (state)
        {
            case CombineRootComponent.StateType.SELECT_BASE:
                str = LocalizationManager.Get("INFO_MSG_NPUP_BASE");
                break;

            case CombineRootComponent.StateType.SELECT_MATERIAL:
                str = LocalizationManager.Get("HEADER_MSG_NPUP_MATERIAL");
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

    public void setSvtNpCombineData(int idx)
    {
        int id = this.npData.svtUseNpIdList[idx];
        int num2 = this.npData.svtNpLvList[idx];
        if (id > 0)
        {
            this.maxLvStatusInfo.SetActive(false);
            TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(id);
            int maxLv = entity.maxLv;
            this.currentInfoLb.text = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(id, num2).getDetail();
            if (num2 >= maxLv)
            {
                this.isExeCombine = false;
                this.setExeBtnState();
                this.qpLb.text = "0";
                this.maxLvStatusInfo.SetActive(true);
            }
            else
            {
                int num4 = num2 + 1;
                this.spendQpVal = 0;
                this.lvUpData = new SetLevelUpData();
                this.lvUpData.currentId = id;
                this.lvUpData.currentIndex = idx + 1;
                this.lvUpData.realIndex = idx;
                this.lvUpData.targetRuby = entity.ruby;
                this.lvUpData.targetName = entity.name;
                this.lvUpData.currentLv = num2;
                this.lvUpData.nextLv = num4;
                this.lvUpData.spendQp = this.spendQpVal;
                this.lvUpData.haveQp = this.userQP;
                this.lvUpData.combineItemIds = null;
            }
        }
    }

    public enum TargetType
    {
        BASE_STATUS,
        MATERIAL_SELECT,
        MATERIAL_STATUS
    }
}

