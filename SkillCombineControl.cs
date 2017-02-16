using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SkillCombineControl : BaseMonoBehaviour
{
    public UILabel afterInfoLb;
    public GameObject baseSelectInfoLb;
    public UILabel baseSelectLb;
    private int baseSvtId;
    private UICharaGraphTexture charaGraph;
    public GameObject charaGraphBase;
    public UIButton combineBtn;
    public UISprite combineBtnBg;
    public UILabel combineBtnTxt;
    private List<EventInfoData> combineEventList;
    public UIGrid combineTargetGrid;
    public UISprite combineTargetNameSprite;
    public UISprite combineTxtImg;
    private int currentIdx;
    public UILabel currentInfoLb;
    public UIPanel currentTxtPanel;
    public UILabel detailInfoLb;
    public static int dispitemNum = 5;
    public UISprite eventNoticeImg;
    public SetRarityDialogControl exeCombineDlg;
    private int gridChildCnt;
    public UILabel haveNumItemLb;
    public UILabel haveQpLb;
    private int haveQpVal;
    private bool isExeCombine;
    public UISprite itemImgSprite;
    public UIGrid itemInfoGrid;
    private List<LimitCntUpItemComponent> itemInfoList;
    public GameObject itemInfoPrefab;
    public UILabel itemNameLb;
    private SetLevelUpData lvUpData;
    public GameObject materialBgObj;
    public GameObject maxLvStatusInfo;
    public MenuListControl menuListCtr;
    public MoveLabelTextControl moveTxtCtr;
    public UIPanel mPanel;
    public UILabel needNumItemLb;
    public UILabel needQpLb;
    public GameObject npCombineInfo;
    private SvtUseNpData npData;
    public UILabel preSelectBaseLb;
    public UILabel qpLb;
    public UILabel selectSkillHelpLb;
    public GameObject selectSkillInfo;
    public GameObject skillCombineInfo;
    private SvtUseSkillData skillData;
    private List<GameObject> skillItemList;
    private List<GameObject> skillList;
    private int spendQpVal;
    public GameObject statusObj;
    public PlayMakerFSM targetFsm;
    private List<GameObject> targetList;
    private TargetType targetType;
    private long userId;
    private int userQP;
    private int[] useSkillIdList;

    public bool checkIsMaxLvSkills(UserServantEntity resData)
    {
        int[] numArray = resData.getSkillIdList();
        int[] numArray2 = resData.getSkillLevelList();
        for (int i = 0; i < numArray.Length; i++)
        {
            int id = numArray[i];
            int num3 = numArray2[i];
            if (id > 0)
            {
                int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id).maxLv;
                if (num3 < maxLv)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void clearItemList()
    {
        int childCount = this.itemInfoGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.itemInfoGrid.transform.GetChild(i).gameObject);
            }
        }
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
    }

    public bool getExeBtnState() => 
        this.isExeCombine;

    private SvtUseSkillData getSvtSkillData(UserServantEntity usrSvtData)
    {
        this.skillData = new SvtUseSkillData();
        this.skillData.svtUseSkillIdList = usrSvtData.getSkillIdList();
        this.skillData.svtSkillLvList = usrSvtData.getSkillLevelList();
        return this.skillData;
    }

    public SetLevelUpData getTargetData() => 
        this.lvUpData;

    private void initDispCombineInfo()
    {
        this.maxLvStatusInfo.SetActive(false);
        this.preSelectBaseLb.text = LocalizationManager.Get("MSG_PRESELECT_BASE_SVT");
        this.preSelectBaseLb.gameObject.SetActive(true);
        this.baseSelectInfoLb.SetActive(true);
        this.selectSkillInfo.SetActive(false);
        this.setHaveQpInfo();
        this.selectSkillHelpLb.text = LocalizationManager.Get("MSG_SKILL_SELECT");
        this.needQpLb.text = LocalizationManager.Get("NEED_QP_INFO");
        this.spendQpVal = 0;
        this.qpLb.text = this.spendQpVal.ToString();
        this.qpLb.color = Color.white;
        this.skillList = new List<GameObject>();
        for (int i = 0; i < BalanceConfig.SvtSkillListMax; i++)
        {
            GameObject item = base.createObject(this.skillCombineInfo, this.combineTargetGrid.transform, null);
            this.skillList.Add(item);
        }
        this.combineTargetGrid.repositionNow = true;
        this.skillItemList = new List<GameObject>();
        for (int j = 0; j < 5; j++)
        {
            GameObject obj3 = base.createObject(this.itemInfoPrefab, this.itemInfoGrid.transform, null);
            this.skillItemList.Add(obj3);
        }
        this.itemInfoGrid.repositionNow = true;
    }

    public void initSvtSkillCombine(TargetType type)
    {
        this.targetType = type;
        this.baseSvtId = 0;
        this.destroyGrid();
        this.clearItemList();
        this.initDispCombineInfo();
        this.isExeCombine = false;
        this.setExeBtnState();
        this.combineTxtImg.spriteName = "buttontxt_synthesis";
        this.combineTxtImg.MakePixelPerfect();
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.userQP = entity.qp;
        if (this.charaGraph != null)
        {
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
        this.eventNoticeImg.gameObject.SetActive(false);
        this.combineEventList = this.menuListCtr.getCombineEventList();
        if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
        {
            foreach (EventInfoData data in this.combineEventList)
            {
                if (data.type == 8)
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
        string titleMsg = LocalizationManager.Get("CONFIRM_TITLE_SKILL_COMBINE");
        this.exeCombineDlg.setSkillNpCombineInfo(this.lvUpData, titleMsg);
    }

    public void OnClickInfo(bool isdecide, int idx)
    {
        this.setNeedItemInfo(idx);
    }

    public void OnLongPushListView()
    {
        if (this.baseSvtId > 0)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
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

    public void setBaseSvtSkillInfo(UserServantEntity usrSvtEn, int idx = 0)
    {
        if (usrSvtEn != null)
        {
            this.userId = usrSvtEn.userId;
            this.baseSvtId = usrSvtEn.svtId;
            this.currentIdx = idx;
            if (this.targetType == TargetType.SKILL_COMBINE)
            {
                this.preSelectBaseLb.gameObject.SetActive(false);
                this.baseSelectInfoLb.SetActive(false);
                this.setCombineSkillList(this.getSvtSkillData(usrSvtEn), this.skillCombineInfo);
            }
        }
    }

    private void setCombineSkillList(SvtUseSkillData skillData, GameObject targetGo)
    {
        this.useSkillIdList = skillData.svtUseSkillIdList;
        int length = this.useSkillIdList.Length;
        this.targetList = new List<GameObject>();
        for (int i = 0; i < length; i++)
        {
            int id = this.useSkillIdList[i];
            if (id > 0)
            {
                int skillLv = skillData.svtSkillLvList[i];
                GameObject item = this.skillList[i];
                SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id);
                int skillIconId = 400;
                string skillName = "-";
                int skillMaxLv = 10;
                if (entity != null)
                {
                    skillIconId = entity.iconId;
                    skillName = entity.name;
                    skillMaxLv = entity.maxLv;
                }
                item.GetComponent<ServantSkillInfoIconComponent>().SetSkillInfo(i, id, skillLv, skillMaxLv, skillName, skillIconId, new ServantSkillInfoIconComponent.ClickDelegate(this.OnClickInfo));
                this.targetList.Add(item);
            }
        }
        this.combineTargetGrid.repositionNow = true;
        this.itemInfoList = new List<LimitCntUpItemComponent>();
        for (int j = 0; j < dispitemNum; j++)
        {
            LimitCntUpItemComponent component = this.skillItemList[j].GetComponent<LimitCntUpItemComponent>();
            this.itemInfoList.Add(component);
        }
        this.itemInfoGrid.repositionNow = true;
        this.setNeedItemInfo(this.currentIdx);
    }

    private void setDispStatusInfo(bool isCombine)
    {
        this.statusObj.SetActive(!isCombine);
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

    private void setNeedItemInfo(int idx)
    {
        if ((this.baseSvtId > 0) && (this.targetType == TargetType.SKILL_COMBINE))
        {
            bool flag = this.targetList.Count > 1;
            this.selectSkillInfo.SetActive(true);
            for (int i = 0; i < this.targetList.Count; i++)
            {
                ServantSkillInfoIconComponent component = this.targetList[i].GetComponent<ServantSkillInfoIconComponent>();
                if (this.useSkillIdList[idx] == component.getSkillInfo())
                {
                    component.setDispSelectMskImg(true);
                }
                else
                {
                    component.setDispSelectMskImg(false);
                }
            }
            this.setSvtSkillCombineData(idx);
        }
    }

    public void setStateInfoMsg(CombineRootComponent.StateType state)
    {
        this.detailInfoLb.GetComponent<UIWidget>().color = new Color(0f, 0.8789063f, 0.9882813f);
        string str = string.Empty;
        if (state == CombineRootComponent.StateType.SELECT_BASE)
        {
            str = LocalizationManager.Get("INFO_MSG_SKILLUP");
        }
        this.detailInfoLb.text = str;
    }

    private void setSvtSkillCombineData(int idx)
    {
        int id = this.skillData.svtUseSkillIdList[idx];
        int num2 = this.skillData.svtSkillLvList[idx];
        if (id > 0)
        {
            SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id);
            int maxLv = 10;
            string name = string.Empty;
            if (entity != null)
            {
                maxLv = entity.maxLv;
                name = entity.name;
            }
            this.maxLvStatusInfo.SetActive(false);
            this.currentInfoLb.text = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(id, num2).getDetail();
            for (int i = 0; i < this.itemInfoList.Count; i++)
            {
                this.itemInfoList[i].enableDispItemInfo();
            }
            UIWidget component = this.detailInfoLb.GetComponent<UIWidget>();
            component.color = new Color(0f, 0.8789063f, 0.9882813f);
            string str2 = LocalizationManager.Get("EXE_SUMMON_COMBINE_TXT");
            if (num2 >= maxLv)
            {
                this.isExeCombine = false;
                this.setExeBtnState();
                this.qpLb.text = "0";
                this.combineTxtImg.spriteName = "buttontxt_notsynthesis";
                this.combineTxtImg.MakePixelPerfect();
            }
            else
            {
                this.combineTxtImg.spriteName = "buttontxt_synthesis";
                this.combineTxtImg.MakePixelPerfect();
                int num5 = num2 + 1;
                ServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseSvtId);
                CombineSkillEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.COMBINE_SKILL).getEntityFromId<CombineSkillEntity>(entity3.combineSkillId, num2);
                if (entity4 != null)
                {
                    this.spendQpVal = entity4.qp;
                    if ((this.combineEventList != null) && (this.combineEventList.Count > 0))
                    {
                        foreach (EventInfoData data in this.combineEventList)
                        {
                            if (data.type == 8)
                            {
                                this.spendQpVal = (int) (this.spendQpVal * data.value);
                            }
                        }
                    }
                    this.qpLb.text = this.spendQpVal.ToString("N0");
                }
                int[] itemIds = entity4.itemIds;
                int[] itemNums = entity4.itemNums;
                int length = itemIds.Length;
                int num7 = 5;
                bool flag = true;
                for (int j = 0; j < num7; j++)
                {
                    if (j <= (length - 1))
                    {
                        LimitCntUpItemComponent component2 = this.itemInfoList[j];
                        component2.setLimitUpItemInfo(this.userId, itemIds[j], itemNums[j], j);
                        if (!component2.checkItemNum())
                        {
                            flag = false;
                        }
                    }
                }
                this.qpLb.color = Color.white;
                this.isExeCombine = true;
                if (this.haveQpVal < this.spendQpVal)
                {
                    component.color = Color.white;
                    str2 = LocalizationManager.Get("SHORT_QP_INFO_MSG");
                    this.qpLb.color = Color.red;
                    this.isExeCombine = false;
                }
                if (!flag)
                {
                    component.color = Color.white;
                    str2 = LocalizationManager.Get("SHORT_ITEM_INFO_MSG");
                    this.isExeCombine = false;
                }
                this.setExeBtnState();
                this.detailInfoLb.text = str2;
                this.lvUpData = new SetLevelUpData();
                this.lvUpData.currentId = id;
                this.lvUpData.currentIndex = idx + 1;
                this.lvUpData.realIndex = idx;
                this.lvUpData.targetRuby = string.Empty;
                this.lvUpData.targetName = name;
                this.lvUpData.currentLv = num2;
                this.lvUpData.nextLv = num5;
                this.lvUpData.spendQp = this.spendQpVal;
                this.lvUpData.haveQp = this.userQP;
                this.lvUpData.combineItemIds = itemIds;
            }
        }
    }

    public enum TargetType
    {
        SKILL_COMBINE,
        NP_COMBINE
    }
}

