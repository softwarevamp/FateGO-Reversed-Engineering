using System;
using System.Collections.Generic;
using UnityEngine;

public class LimitCntUpControl : BaseMonoBehaviour
{
    public UILabel afterMaxLvLb;
    private UserServantEntity baseData;
    private long baseId;
    public UISprite baseSelectImg;
    public GameObject baseSelectInfoLb;
    public UILabel baseSelectLb;
    private Vector3 center;
    private UICharaGraphTexture charaGraph;
    public GameObject charaGraphBase;
    public UIButton combineBtn;
    public UISprite combineBtnBg;
    public UILabel combineBtnTxt;
    private List<EventInfoData> combineEventList;
    public CombineInfoComponent combineInfoComp;
    public UISprite combineTxtImg;
    public UILabel currentLvLb;
    public UILabel currentMaxLvLb;
    public UILabel detailInfoLb;
    public UISprite eventNoticeImg;
    public SetRarityDialogControl exeCombineDlg;
    public UILabel haveQpLb;
    private int haveQpVal;
    private bool isExeCombine;
    private bool isItemNum;
    private bool isMaxLv;
    private bool isQpNum;
    private bool isSelectBase;
    private List<LimitCntUpItemComponent> itemInfoList;
    public GameObject itemInfoPrefab;
    public UIGrid itemListGrid;
    public GameObject itemListInfo;
    public GameObject lvInfo;
    private List<GameObject> materialItemObjList;
    public GameObject maxLvInfo;
    public UILabel maxLvLb;
    public MenuListControl menuListCtr;
    public UIPanel mPanel;
    public UILabel needQpLb;
    public UILabel preSelectBaseLb;
    public UILabel qpLb;
    private int spendQpVal;
    public PlayMakerFSM targetFsm;

    public void checkIsSelectBaseSvt(long selectBaseId)
    {
        if (selectBaseId > 0L)
        {
            if ((this.baseId > 0L) && (this.baseId != selectBaseId))
            {
                Debug.Log("***!!! checkIsSelectBaseSvt baseId: " + this.baseId);
                this.initItemInfo();
            }
            this.isSelectBase = true;
        }
        else
        {
            this.initItemInfo();
            this.isSelectBase = false;
        }
        this.baseId = selectBaseId;
    }

    private bool checkItemHaveNum()
    {
        for (int i = 0; i < this.itemInfoList.Count; i++)
        {
            LimitCntUpItemComponent component = this.itemInfoList[i];
            if (!component.checkItemNum())
            {
                return false;
            }
        }
        return true;
    }

    public void clearItemList()
    {
        int childCount = this.itemListGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.itemListGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public bool getExeBtnState() => 
        this.isExeCombine;

    public List<LimitCntUpItemComponent> getItemInfoList() => 
        this.itemInfoList;

    public void initItemInfo()
    {
        this.currentLvLb.text = string.Empty;
        this.maxLvLb.text = string.Empty;
        this.lvInfo.SetActive(false);
        this.currentMaxLvLb.text = string.Empty;
        this.afterMaxLvLb.text = string.Empty;
        this.maxLvInfo.SetActive(false);
        this.setHaveQpIno();
        this.spendQpVal = 0;
        this.qpLb.text = this.spendQpVal.ToString();
        this.qpLb.color = Color.white;
        this.isMaxLv = true;
        this.isQpNum = true;
        this.isItemNum = true;
        this.clearItemList();
        this.materialItemObjList = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject item = base.createObject(this.itemInfoPrefab, this.itemListGrid.transform, null);
            this.materialItemObjList.Add(item);
        }
        this.itemListGrid.repositionNow = true;
        this.isExeCombine = false;
        this.setExeBtnState();
        if (this.charaGraph != null)
        {
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
        this.combineInfoComp.initStatusInfo(CombineInfoComponent.DispType.LIMITCNT_UP);
    }

    public void initLimitUp()
    {
        this.needQpLb.text = LocalizationManager.Get("NEED_QP_INFO");
        this.isSelectBase = false;
        this.baseId = 0L;
        this.initItemInfo();
        this.isExeCombine = false;
        this.setExeBtnState();
        this.isMaxLv = true;
        this.isQpNum = true;
        this.isItemNum = true;
        this.eventNoticeImg.gameObject.SetActive(false);
        this.preSelectBaseLb.text = LocalizationManager.Get("MSG_PRESELECT_BASE_LIMIT");
        this.preSelectBaseLb.gameObject.SetActive(true);
        this.baseSelectInfoLb.SetActive(true);
        this.combineEventList = this.menuListCtr.getCombineEventList();
        if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
        {
            foreach (EventInfoData data in this.combineEventList)
            {
                if (data.type == 6)
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

    public void OnClickExeLimitUp()
    {
        string msg = LocalizationManager.Get("CONFIRM_TITLE_LIMITUP");
        this.exeCombineDlg.setConfirmCombine(this.baseData, msg, this.spendQpVal, this.haveQpVal, false);
    }

    public void OnLongPushListView()
    {
        if (this.baseId > 0L)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.targetFsm.SendEvent("SHOW_SVT_STATUS");
        }
    }

    public void setBaseSvtCardImg(UserServantEntity usrSvtData)
    {
        this.baseData = usrSvtData;
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

    private void setCenter()
    {
        Vector3[] worldCorners = this.mPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mPanel.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        Debug.Log("**!! Center: " + this.center);
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

    private void setHaveQpIno()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.haveQpVal = entity.qp;
        this.haveQpLb.text = this.haveQpVal.ToString("N0");
    }

    private void setLimitUpInfo()
    {
        int num7;
        int num8;
        float num9;
        int svtId = this.baseData.svtId;
        int lv = this.baseData.lv;
        int num3 = this.baseData.getLevelMax();
        int limitCount = this.baseData.limitCount;
        int num5 = limitCount + 1;
        int lvMax = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, num5).lvMax;
        this.currentLvLb.text = lv.ToString();
        this.maxLvLb.text = num3.ToString();
        this.lvInfo.SetActive(true);
        this.currentMaxLvLb.text = num3.ToString();
        this.afterMaxLvLb.text = lvMax.ToString();
        this.maxLvInfo.SetActive(true);
        this.combineInfoComp.setCurrentStatusInfo(this.baseData);
        CombineSvtData resSvtData = new CombineSvtData {
            baseSvtData = this.baseData,
            combineResSvtLv = this.baseData.lv,
            combineResLimitCnt = num5,
            combineResSvtMaxLv = lvMax
        };
        bool flag = this.baseData.getExpInfo(out num7, out num8, out num9);
        resSvtData.combineResExpBarVal = num9;
        resSvtData.combineResNextExp = num8;
        resSvtData.combineResHp = this.baseData.hp;
        resSvtData.hpAdjustVal = this.baseData.adjustHp;
        resSvtData.combineResAtk = this.baseData.atk;
        resSvtData.atkAdjustVal = this.baseData.adjustAtk;
        this.combineInfoComp.setCombineResStatusInfo(resSvtData);
        int combineLimitId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId).combineLimitId;
        CombineLimitEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CombineLimitMaster>(DataNameKind.Kind.COMBINE_LIMIT).getEntityFromId<CombineLimitEntity>(combineLimitId, limitCount);
        this.spendQpVal = entity3.qp;
        int[] itemIds = entity3.itemIds;
        int[] itemNums = entity3.itemNums;
        if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
        {
            foreach (EventInfoData data2 in this.combineEventList)
            {
                if (data2.type == 6)
                {
                    this.spendQpVal = (int) (this.spendQpVal * data2.value);
                }
            }
        }
        this.qpLb.text = this.spendQpVal.ToString("N0");
        int count = this.materialItemObjList.Count;
        int length = itemIds.Length;
        this.itemInfoList = new List<LimitCntUpItemComponent>();
        for (int i = 0; i < count; i++)
        {
            LimitCntUpItemComponent item = this.materialItemObjList[i].GetComponent<LimitCntUpItemComponent>();
            if (i <= (length - 1))
            {
                item.setLimitUpItemInfo(this.baseData.userId, itemIds[i], itemNums[i], i);
                this.itemInfoList.Add(item);
            }
        }
        this.itemListGrid.repositionNow = true;
        this.qpLb.color = Color.white;
        this.isExeCombine = true;
        if (lv < num3)
        {
            this.isMaxLv = false;
            this.isExeCombine = false;
        }
        if (this.haveQpVal < this.spendQpVal)
        {
            this.isQpNum = false;
            this.qpLb.color = Color.red;
            this.isExeCombine = false;
        }
        if (!this.checkItemHaveNum())
        {
            this.isItemNum = false;
            this.isExeCombine = false;
        }
        this.setExeBtnState();
    }

    public void setStateInfoMsg(CombineRootComponent.StateType state)
    {
        UIWidget component = this.detailInfoLb.GetComponent<UIWidget>();
        component.color = new Color(0f, 0.8789063f, 0.9882813f);
        string str = string.Empty;
        switch (state)
        {
            case CombineRootComponent.StateType.SELECT_BASE:
                str = LocalizationManager.Get("INFO_MSG_LIMITUP");
                break;

            case CombineRootComponent.StateType.EXE_COMBINE:
                if (this.isExeCombine || this.isMaxLv)
                {
                    if (!this.isExeCombine && !this.isItemNum)
                    {
                        component.color = Color.white;
                        str = LocalizationManager.Get("SHORT_ITEM_INFO_MSG");
                    }
                    else if (!this.isExeCombine && !this.isQpNum)
                    {
                        component.color = Color.white;
                        str = LocalizationManager.Get("SHORT_QP_INFO_MSG");
                    }
                    else
                    {
                        str = LocalizationManager.Get("EXE_SUMMON_COMBINE_TXT");
                    }
                    break;
                }
                component.color = Color.white;
                str = LocalizationManager.Get("MSG_MAXLV_LIMITUP_BASE");
                break;
        }
        this.detailInfoLb.text = str;
    }

    public void showItemListInfo()
    {
        this.preSelectBaseLb.gameObject.SetActive(!this.isSelectBase);
        this.baseSelectInfoLb.SetActive(!this.isSelectBase);
        this.itemListInfo.SetActive(true);
        if (this.isSelectBase)
        {
            this.setLimitUpInfo();
        }
    }
}

