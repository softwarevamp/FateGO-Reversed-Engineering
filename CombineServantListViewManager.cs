using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombineServantListViewManager : ListViewManager
{
    [SerializeField]
    protected UIButton allReleaseButton;
    protected UserServantEntity baseUsrSvtData;
    protected long baseUsrSvtId;
    protected int callbackCount;
    [SerializeField]
    protected UILabel cardInfoLabel;
    [SerializeField]
    protected GameObject cardNumInfo;
    private int checkLv;
    protected static readonly float COLOR_VAL = 0.375f;
    private SetCombineData combineData;
    [SerializeField]
    protected UILabel combineInfoMsgLb;
    private CombineInitData combineIntData;
    [SerializeField]
    protected GameObject combineViewInfo;
    [SerializeField]
    protected UISprite decideBtnBg;
    [SerializeField]
    protected UIButton decideButton;
    [SerializeField]
    protected UILabel emptyListNoticeLabel;
    private int expType;
    private int getAtkUpVal;
    [SerializeField]
    protected UISprite getExpBg;
    [SerializeField]
    protected GameObject getExpInfo;
    [SerializeField]
    protected UISprite getExpInfoImg;
    [SerializeField]
    protected UILabel getExpInfoLabel;
    [SerializeField]
    protected UILabel getExpLabel;
    [SerializeField]
    protected GameObject getExpMask;
    private int getHpUpVal;
    [SerializeField]
    protected UISprite haveQpBg;
    [SerializeField]
    protected GameObject haveQpInfo;
    [SerializeField]
    protected UISprite haveQpInfoImg;
    [SerializeField]
    protected UILabel haveQpLabel;
    [SerializeField]
    protected GameObject haveQpMask;
    protected long[] highRarityList;
    private int increLv;
    protected InitMode initMode;
    private bool isAllUpMax;
    private bool isDecideFlg;
    protected static bool isInitSystem;
    protected bool isSelectMaterial;
    private CombineServantListViewItem.Type itemType;
    [SerializeField]
    protected UISprite levelUpInfoImg;
    public MenuListControl menuListCtr;
    protected int minimumKeep = 1;
    [SerializeField]
    protected UISprite nextExpBg;
    [SerializeField]
    protected GameObject nextExpInfo;
    [SerializeField]
    protected UISprite nextExpInfoImg;
    [SerializeField]
    protected UILabel nextExpLabel;
    [SerializeField]
    protected GameObject nextExpMask;
    private int nextLv;
    [SerializeField]
    protected SetRarityDialogControl rarityDlg;
    protected int selectBaseIndex;
    private List<CombineServantListViewItem> selectedMtSvtList;
    protected int selectExp;
    [SerializeField]
    protected UILabel selectInfoLabel;
    [SerializeField]
    protected int selectMax = 5;
    protected int selectQp;
    protected int selectSum;
    private UserServantEntity selectUsrSvtEntity;
    protected int sellEnableRestCnt;
    [SerializeField]
    protected UILabel servantInfoLabel;
    [SerializeField]
    protected GameObject servantNumInfo;
    protected const string SORT_SAVE_KEY = "CombineServant";
    [SerializeField]
    protected ServantSortSelectMenu sortSelectMenu;
    protected static ListViewSort[] sortStatusList;
    [SerializeField]
    protected UISprite spendQpBg;
    [SerializeField]
    protected GameObject spendQpInfo;
    [SerializeField]
    protected UISprite spendQpInfoImg;
    [SerializeField]
    protected UILabel spendQpInfoLabel;
    [SerializeField]
    protected UILabel spendQpLabel;
    [SerializeField]
    protected GameObject spendQpMask;
    [SerializeField]
    protected PlayMakerFSM targetFSM;
    private List<CombineServantListViewItem> tempMtSvtList = new List<CombineServantListViewItem>();
    private int totalExp;
    protected int userQP;
    protected UserServantMaster userServantMaster;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    private void changeCombineEnableRestCnt(bool isPlus, CombineServantListViewItem item)
    {
        if (((this.itemType == CombineServantListViewItem.Type.MATERIAL) && item.IsOrganization) && item.IsCombineEnableServant)
        {
            if (isPlus)
            {
                if (!item.IsSelect)
                {
                    this.sellEnableRestCnt++;
                }
            }
            else
            {
                this.sellEnableRestCnt--;
            }
        }
    }

    private bool checkIncrementLv(int lv)
    {
        int num = this.baseUsrSvtData.getLevelMax();
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

    public bool checkIsSelectMaterial() => 
        this.isSelectMaterial;

    public void checkRareSvt()
    {
        if (this.highRarityList.Length > 0)
        {
            this.targetFSM.SendEvent("SHOW_RAREDLG");
        }
        else
        {
            this.targetFSM.SendEvent("NO_RARESVT");
        }
    }

    public void checkSpendQp()
    {
        if (this.selectQp > this.userQP)
        {
            this.targetFSM.SendEvent("SHOW_QPDLG");
        }
        else
        {
            this.targetFSM.SendEvent("NO_SHORTQP");
        }
    }

    public void clearSelectedSvtList()
    {
        if (this.selectedMtSvtList != null)
        {
            this.selectedMtSvtList.Clear();
        }
    }

    public void CreateList(CombineServantListViewItem.Type type)
    {
        this.combineViewInfo.SetActive(true);
        if (!isInitSystem)
        {
            sortStatusList = new ListViewSort[9];
            for (int i = 0; i < 9; i++)
            {
                sortStatusList[i] = new ListViewSort("CombineServant" + (i + 1), ListViewSort.SortKind.LEVEL, false);
            }
            isInitSystem = true;
        }
        base.sort = sortStatusList[(int) type];
        base.sort.Load();
        switch (type)
        {
            case CombineServantListViewItem.Type.BASE:
            case CombineServantListViewItem.Type.LIMITUP_BASE:
            case CombineServantListViewItem.Type.SKILL_BASE:
            case CombineServantListViewItem.Type.NP_BASE:
                this.setDispActive(false);
                this.setBtnEnable(false);
                this.setServantList(type);
                this.servantNumInfo.SetActive(true);
                this.cardNumInfo.SetActive(false);
                this.selectInfoLabel.gameObject.SetActive(false);
                break;

            case CombineServantListViewItem.Type.MATERIAL:
            case CombineServantListViewItem.Type.SVTEQ_MATERIAL:
                this.setDispActive(true);
                this.setBtnEnable(false);
                this.setServantList(type);
                this.selectMax = 5;
                this.servantNumInfo.SetActive(false);
                this.cardNumInfo.SetActive(true);
                this.selectInfoLabel.gameObject.SetActive(true);
                this.getExpInfo.SetActive(true);
                this.spendQpInfoLabel.text = LocalizationManager.Get("NEED_QP_INFO");
                this.getExpInfoLabel.text = LocalizationManager.Get("GET_EXP_INFO");
                break;

            case CombineServantListViewItem.Type.NP_MATERIAL:
                this.setDispActive(true);
                this.setBtnEnable(false);
                this.setServantList(type);
                this.servantNumInfo.SetActive(false);
                this.cardNumInfo.SetActive(true);
                this.spendQpInfoLabel.text = LocalizationManager.Get("NEED_QP_INFO");
                this.combineViewInfo.SetActive(false);
                this.selectMax = 1;
                break;
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    protected void EndCloseSelectFilterKind()
    {
    }

    public void EndSelectFilterKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFilterSelectMenu(new System.Action(this.EndCloseSelectFilterKind));
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            CombineServantListViewItem item = base.itemList[i] as CombineServantListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
    }

    public int GetBaseCollectionLimitCnt()
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.baseUsrSvtData.userId, this.baseUsrSvtData.svtId);
        int maxLimitCount = -1;
        if (entity != null)
        {
            maxLimitCount = entity.maxLimitCount;
        }
        return maxLimitCount;
    }

    public int GetBaseCollectionLv()
    {
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.baseUsrSvtData.userId, this.baseUsrSvtData.svtId);
        int maxLv = 0;
        if (entity != null)
        {
            maxLv = entity.maxLv;
        }
        return maxLv;
    }

    public CombineServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as CombineServantListViewItem);
        }
        return null;
    }

    public CombineServantListViewItem.Type getItemType() => 
        this.itemType;

    public UserServantEntity GetSelectBaseSvtData()
    {
        if (this.baseUsrSvtData != null)
        {
            long id = this.baseUsrSvtData.id;
            return (this.baseUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id));
        }
        return this.baseUsrSvtData;
    }

    public SetCombineData getSelectCombineData() => 
        this.combineData;

    public UserServantEntity getSelectUsrSvtEntity() => 
        this.selectUsrSvtEntity;

    public bool IsSelectEnable()
    {
        bool flag = false;
        if (this.selectSum < this.selectMax)
        {
            flag = true;
        }
        if (this.sellEnableRestCnt <= this.minimumKeep)
        {
            flag = false;
        }
        return flag;
    }

    public void ModifyItem()
    {
        if (base.itemList != null)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            foreach (ListViewItem item in base.itemList)
            {
                CombineServantListViewItem item2 = item as CombineServantListViewItem;
                item2.ModifyItem(item2.UserSvtId == entity.favoriteUserSvtId);
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.SetItem(item2);
                }
            }
        }
    }

    public void OnClickDecide()
    {
        if (this.isDecideFlg)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.selectedMtSvtList = new List<CombineServantListViewItem>();
            this.tempMtSvtList.Clear();
            List<int> list = new List<int>();
            List<long> list2 = new List<long>();
            foreach (ListViewItem item in base.itemList)
            {
                if (item.IsSelect)
                {
                    list.Add(item.Index);
                    CombineServantListViewItem item2 = this.GetItem(item.Index);
                    if (item2.SvtRariry >= 3)
                    {
                        list2.Add(item2.UserSvtId);
                    }
                    this.tempMtSvtList.Add(item2);
                    this.selectedMtSvtList.Add(item2);
                }
            }
            this.highRarityList = list2.ToArray();
            this.setSelectMaterialList(list.ToArray());
            this.targetFSM.SendEvent("SELECT_MATERIAL_SVT");
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void OnClickFilterKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFilterSelectMenu(ServantFilterSelectMenu.Kind.SERVANT, base.sort, new ServantFilterSelectMenu.CallbackFunc(this.EndSelectFilterKind));
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    public void OnClickReleaseAll()
    {
        Debug.Log("Manager ListView OnClickReleaseAll " + this.selectSum);
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        if (this.selectSum > 0)
        {
            if (this.itemType == CombineServantListViewItem.Type.MATERIAL)
            {
                this.sellEnableRestCnt = 0;
            }
            foreach (ListViewItem item in base.itemList)
            {
                item.IsSelect = false;
                CombineServantListViewItem item2 = item as CombineServantListViewItem;
                this.changeCombineEnableRestCnt(true, item2);
            }
            this.selectSum = 0;
            this.levelUpInfoImg.gameObject.SetActive(false);
            this.totalExp = 0;
            this.increLv = 0;
            this.checkLv = 0;
            this.RefrashListDisp();
        }
    }

    protected void OnClickSelectBase(ListViewObject obj)
    {
        CombineServantListViewItem item = (obj as CombineServantListViewObject).GetItem();
        if ((this.baseUsrSvtData != null) && (this.baseUsrSvtData.id == item.UserSvtId))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.baseUsrSvtData = null;
            this.targetFSM.Fsm.Variables.GetFsmString("TestUsrSvtId").Value = string.Empty;
            this.tempMtSvtList.Clear();
        }
        else
        {
            if (item.IsCanNotBaseSelect)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
            else if (!item.IsCanNotBaseSelect)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            this.baseUsrSvtData = item.UserSvtEntity;
            this.targetFSM.Fsm.Variables.GetFsmString("TestUsrSvtId").Value = item.NameText;
        }
        this.targetFSM.SendEvent("SELECT_BASE_SVT");
    }

    protected void OnClickSelectMaterial(ListViewObject obj)
    {
        CombineServantListViewItem item = (obj as CombineServantListViewObject).GetItem();
        if (item.IsSelect)
        {
            int selectNum = item.SelectNum;
            item.IsSelect = false;
            this.selectSum--;
            this.changeCombineEnableRestCnt(true, item);
            foreach (ListViewItem item2 in base.itemList)
            {
                int num2 = item2.SelectNum;
                if (num2 > selectNum)
                {
                    item2.SelectNum = num2 - 1;
                }
            }
            this.RefrashListDisp();
        }
        else if (this.selectSum < this.selectMax)
        {
            if (((this.itemType == CombineServantListViewItem.Type.MATERIAL) && (this.sellEnableRestCnt > 0)) && (item.IsOrganization && !this.IsSelectEnable()))
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
            else
            {
                item.SelectNum = this.selectSum;
                this.selectSum++;
                this.changeCombineEnableRestCnt(false, item);
                this.RefrashListDisp();
            }
        }
    }

    public void OnClickSortAscendingOrder()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
            base.SortItem(-1, false, -1);
        }
    }

    public void OnClickSortKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            switch (base.sort.Kind)
            {
                case ListViewSort.SortKind.CREATE:
                    base.sort.SetKind(ListViewSort.SortKind.CLASS);
                    break;

                case ListViewSort.SortKind.RARITY:
                    base.sort.SetKind(ListViewSort.SortKind.AMOUNT);
                    break;

                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.NP_LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.COST);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.FRIENDSHIP);
                    break;

                case ListViewSort.SortKind.COST:
                    base.sort.SetKind(ListViewSort.SortKind.RARITY);
                    break;

                case ListViewSort.SortKind.CLASS:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;

                case ListViewSort.SortKind.FRIENDSHIP:
                    base.sort.SetKind(ListViewSort.SortKind.NP_LEVEL);
                    break;

                case ListViewSort.SortKind.AMOUNT:
                    base.sort.SetKind(ListViewSort.SortKind.CREATE);
                    break;

                default:
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;
            }
            base.SortItem(-1, false, -1);
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        CombineServantListViewItem item = (obj as CombineServantListViewObject).GetItem();
        this.isSelectMaterial = false;
        if ((this.selectedMtSvtList != null) && (this.selectedMtSvtList.Count > 0))
        {
            foreach (CombineServantListViewItem item2 in this.selectedMtSvtList)
            {
                if (item2.UserSvtId == item.UserSvtId)
                {
                    this.isSelectMaterial = true;
                }
            }
        }
        this.selectUsrSvtEntity = item.UserSvtEntity;
        this.targetFSM.SendEvent("SHOW_SVT_STATUS");
    }

    protected void OnMoveEnd()
    {
        Debug.Log("OnMoveEnd " + this.callbackCount);
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
                System.Action action = this.callbackFunc2;
                this.callbackFunc2 = null;
                if (action != null)
                {
                    action();
                }
            }
        }
    }

    protected void RefrashListDisp()
    {
        List<CombineServantListViewObject> objectList = this.ObjectList;
        this.selectQp = 0;
        this.selectExp = 0;
        this.getHpUpVal = 0;
        this.getAtkUpVal = 0;
        this.isAllUpMax = false;
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int maxAjustHp = 0;
        int maxAjustAtk = 0;
        bool flag = false;
        bool flag2 = false;
        if (this.baseUsrSvtData != null)
        {
            if (this.itemType == CombineServantListViewItem.Type.MATERIAL)
            {
                num = this.baseUsrSvtData.getCombineQp();
                num2 = this.baseUsrSvtData.getAdjustHp();
                num3 = this.baseUsrSvtData.getAdjustAtk();
                this.baseUsrSvtData.GetAdjustMax(out maxAjustHp, out maxAjustAtk);
                flag = this.baseUsrSvtData.isAdjustHpMax();
                flag2 = this.baseUsrSvtData.isAdjustAtkMax();
                this.isAllUpMax = flag && flag2;
            }
            else if (this.itemType == CombineServantListViewItem.Type.SVTEQ_MATERIAL)
            {
                num = this.baseUsrSvtData.getCombineQpSvtEq();
            }
            Debug.Log("**** !!! RefrashListDisp selectedUsrSvtId _CombineQP: " + num);
        }
        foreach (ListViewItem item in base.itemList)
        {
            CombineServantListViewItem item2 = item as CombineServantListViewItem;
            if (item.IsSelect)
            {
                if (item2.IsStatusUp)
                {
                    int getHpUpVal = item2.GetHpUpVal;
                    int getAtkUpVal = item2.GetAtkUpVal;
                    if (!this.isAllUpMax)
                    {
                        if ((getHpUpVal > 0) && !flag)
                        {
                            this.getHpUpVal += item2.GetHpUpVal;
                        }
                        if ((getAtkUpVal > 0) && !flag2)
                        {
                            this.getAtkUpVal += item2.GetAtkUpVal;
                        }
                    }
                }
                if (!this.baseUsrSvtData.isLevelMax())
                {
                    this.selectExp += item2.GetMaterialExp;
                }
                this.selectQp += num;
            }
            else
            {
                item2.IsSelectMax = this.selectSum >= this.selectMax;
            }
        }
        if ((num2 + this.getHpUpVal) > maxAjustHp)
        {
            this.getHpUpVal = maxAjustHp - num2;
        }
        if ((num3 + this.getAtkUpVal) > maxAjustAtk)
        {
            this.getAtkUpVal = maxAjustAtk - num3;
        }
        this.selectInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.selectSum, this.selectMax);
        List<EventInfoData> list2 = this.menuListCtr.getCombineEventList();
        if ((list2 != null) && (list2.Count > 0))
        {
            foreach (EventInfoData data in list2)
            {
                if (this.itemType == CombineServantListViewItem.Type.MATERIAL)
                {
                    if (data.type == 2)
                    {
                        Debug.Log("***!! EventInfoData Exp: " + data.value);
                        this.selectExp = Mathf.CeilToInt(this.selectExp * data.value);
                    }
                    if (data.type == 1)
                    {
                        this.selectQp = Mathf.CeilToInt(this.selectQp * data.value);
                    }
                }
            }
        }
        this.spendQpLabel.text = $"{this.selectQp:N0}";
        if (this.itemType == CombineServantListViewItem.Type.MATERIAL)
        {
            this.spendQpLabel.color = (this.selectQp <= this.userQP) ? Color.white : Color.red;
        }
        this.getExpLabel.text = $"{this.selectExp:N0}";
        if ((this.baseUsrSvtData != null) && (this.itemType == CombineServantListViewItem.Type.MATERIAL))
        {
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseUsrSvtData.svtId);
            this.expType = entity.expType;
            this.totalExp = this.selectExp + this.baseUsrSvtData.exp;
            this.checkLv = this.baseUsrSvtData.lv;
            int num8 = this.baseUsrSvtData.getLevelMax();
            if (this.checkLv != num8)
            {
                while (!this.checkIncrementLv(this.checkLv))
                {
                }
                int num9 = this.increLv - this.baseUsrSvtData.lv;
                Debug.Log("***!! increAmount: " + num9);
                if (num9 >= 1)
                {
                    this.levelUpInfoImg.gameObject.SetActive(true);
                }
                else
                {
                    this.levelUpInfoImg.gameObject.SetActive(false);
                }
            }
            bool flag4 = this.increLv >= num8;
            foreach (ListViewItem item3 in base.itemList)
            {
                CombineServantListViewItem item4 = item3 as CombineServantListViewItem;
                if (!item4.IsSelect)
                {
                    item4.IsMaxNextLv = flag4;
                }
            }
        }
        if (this.selectSum > 0)
        {
            this.setBtnEnable(true);
        }
        else
        {
            this.setBtnEnable(false);
        }
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].SetInput(base.isInput);
            }
        }
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        List<CombineServantListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            CombineServantListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(CombineServantListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(CombineServantListViewObject.InitMode mode)
    {
        List<CombineServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (CombineServantListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd));
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(CombineServantListViewObject.InitMode mode, float delay)
    {
        List<CombineServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (CombineServantListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", delay);
        }
    }

    public void ResetInit()
    {
        this.baseUsrSvtData = null;
        if (this.selectedMtSvtList != null)
        {
            this.selectedMtSvtList.Clear();
        }
        this.tempMtSvtList.Clear();
        if (base.itemList != null)
        {
            foreach (ListViewItem item in base.itemList)
            {
                item.IsSelect = false;
            }
        }
        this.selectSum = 0;
        this.levelUpInfoImg.gameObject.SetActive(false);
    }

    private void selectDlg(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        if (isDecide)
        {
            this.targetFSM.SendEvent("GOTO_SVTSELL");
        }
        else
        {
            this.targetFSM.SendEvent("CLOSE_QPDLG");
        }
    }

    private void setBtnEnable(bool isEnable)
    {
        this.allReleaseButton.isEnabled = isEnable;
        this.isDecideFlg = isEnable;
        this.decideBtnBg.GetComponent<UIWidget>().color = !this.isDecideFlg ? Color.gray : Color.white;
    }

    private void setDispActive(bool isShow)
    {
        this.allReleaseButton.gameObject.SetActive(isShow);
        this.decideButton.gameObject.SetActive(isShow);
        Color color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        this.spendQpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.spendQpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.spendQpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.getExpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.getExpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.getExpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
    }

    private void setHeaderMsg(CombineServantListViewItem.Type type)
    {
        string str = string.Empty;
        switch (type)
        {
            case CombineServantListViewItem.Type.BASE:
                str = LocalizationManager.Get("HEADER_MSG_SVTCOMBINE_BASE");
                break;

            case CombineServantListViewItem.Type.MATERIAL:
                str = LocalizationManager.Get("HEADER_MSG_COMBINE_MATERIAL");
                break;

            case CombineServantListViewItem.Type.LIMITUP_BASE:
                str = LocalizationManager.Get("HEADER_MSG_LIMITUP");
                break;

            case CombineServantListViewItem.Type.SKILL_BASE:
                str = LocalizationManager.Get("HEADER_MSG_SKILLUP");
                break;

            case CombineServantListViewItem.Type.NP_BASE:
                str = LocalizationManager.Get("HEADER_MSG_NPUP_BASE");
                break;

            case CombineServantListViewItem.Type.NP_MATERIAL:
                str = LocalizationManager.Get("INFO_MSG_COMBINE_MATERIAL");
                break;
        }
        this.combineInfoMsgLb.text = str;
    }

    public void SetMode(InitMode mode)
    {
        this.initMode = mode;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        switch (mode)
        {
            case InitMode.VALID:
            case InitMode.INPUT:
            {
                bool flag = false;
                foreach (ListViewItem item in base.itemList)
                {
                    CombineServantListViewItem item2 = item as CombineServantListViewItem;
                    if (item2.IsSelect && item2.IsCanNotSelect)
                    {
                        int selectNum = item2.SelectNum;
                        item2.IsSelect = false;
                        this.selectSum--;
                        flag = true;
                        this.changeCombineEnableRestCnt(true, item2);
                        foreach (ListViewItem item3 in base.itemList)
                        {
                            int num2 = item3.SelectNum;
                            if (num2 > selectNum)
                            {
                                item3.SelectNum = num2 - 1;
                            }
                        }
                    }
                }
                if (flag)
                {
                    this.RefrashListDisp();
                }
                if (mode == InitMode.INPUT)
                {
                    this.RequestListObject(CombineServantListViewObject.InitMode.INPUT);
                }
                else
                {
                    this.RequestListObject(CombineServantListViewObject.InitMode.VALID);
                }
                break;
            }
            case InitMode.INTO:
                base.Invoke("RequestInto", 0f);
                break;

            case InitMode.ENTER:
            {
                List<CombineServantListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(CombineServantListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.EXIT:
            {
                List<CombineServantListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = list2.Count;
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(CombineServantListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
        }
        Debug.Log("SetMode " + mode);
    }

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        CombineServantListViewObject obj2 = obj as CombineServantListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(CombineServantListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(CombineServantListViewObject.InitMode.VALID);
        }
    }

    public void SetSelectBaseSvtData(UserServantEntity resData)
    {
        this.baseUsrSvtData = resData;
    }

    public void setSelectedSvtList()
    {
        if (this.tempMtSvtList != null)
        {
            this.selectedMtSvtList = this.tempMtSvtList;
        }
    }

    private void setSelectMaterialList(int[] list)
    {
        this.combineData = new SetCombineData();
        this.combineData.baseSvtData = this.baseUsrSvtData;
        this.combineData.selectSum = this.selectSum;
        this.combineData.spendQp = this.selectQp;
        this.combineData.getExp = this.selectExp;
        this.combineData.getHpAdjustVal = this.getHpUpVal * BalanceConfig.StatusUpAdjustHp;
        this.combineData.getAtkAdjustVal = this.getAtkUpVal * BalanceConfig.StatusUpAdjustAtk;
        this.combineData.isAdjustMax = this.isAllUpMax;
        List<long> list2 = new List<long>();
        for (int i = 0; i < list.Length; i++)
        {
            CombineServantListViewItem item = this.GetItem(list[i]);
            if (item.UserSvtId > 0L)
            {
                list2.Add(item.UserSvtId);
            }
        }
        if (list2.Count > 0)
        {
            this.combineData.materialUsrSvtIdList = list2.ToArray();
        }
    }

    private void setServantList(CombineServantListViewItem.Type type)
    {
        int num5;
        float num7;
        this.setHeaderMsg(type);
        if (!isInitSystem)
        {
            sortStatusList = new ListViewSort[9];
            for (int j = 0; j < 9; j++)
            {
                sortStatusList[j] = new ListViewSort("CombineServant" + (j + 1), ListViewSort.SortKind.LEVEL, false);
            }
            isInitSystem = true;
        }
        base.sort = sortStatusList[(int) type];
        base.sort.Load();
        this.emptyListNoticeLabel.gameObject.SetActive(false);
        this.itemType = type;
        this.userServantMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserDeckEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getDeckList(entity.userId);
        UserServantEntity[] collection = null;
        base.CreateList(0);
        int index = 0;
        int length = 0;
        int num4 = 0;
        this.userQP = entity.qp;
        this.haveQpLabel.text = $"{this.userQP:N0}";
        this.selectSum = 0;
        this.selectQp = 0;
        this.selectExp = 0;
        this.getHpUpVal = 0;
        this.getAtkUpVal = 0;
        this.isAllUpMax = false;
        this.sellEnableRestCnt = 0;
        int lateExp = 0;
        this.spendQpMask.GetComponent<UIWidget>().color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        this.getExpMask.GetComponent<UIWidget>().color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        this.haveQpMask.GetComponent<UIWidget>().color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        this.nextExpMask.GetComponent<UIWidget>().color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        if (((type == CombineServantListViewItem.Type.BASE) || (type == CombineServantListViewItem.Type.LIMITUP_BASE)) || ((type == CombineServantListViewItem.Type.SKILL_BASE) || (type == CombineServantListViewItem.Type.NP_BASE)))
        {
            this.levelUpInfoImg.gameObject.SetActive(false);
            collection = this.userServantMaster.getOrganizationList();
            if (type == CombineServantListViewItem.Type.BASE)
            {
                collection = this.userServantMaster.getCombineMaterialList();
            }
            length = collection.Length;
            if (length <= 0)
            {
                this.emptyListNoticeLabel.gameObject.SetActive(true);
            }
            List<UserServantEntity> list = new List<UserServantEntity>(collection);
            List<UserServantEntity> list2 = new List<UserServantEntity>();
            if (this.baseUsrSvtData != null)
            {
                for (int k = 0; k < collection.Length; k++)
                {
                    UserServantEntity entity2 = collection[k];
                    if (entity2.id == this.baseUsrSvtData.id)
                    {
                        list2.Add(entity2);
                        list.Remove(entity2);
                    }
                }
                list2.AddRange(list);
                collection = list2.ToArray();
                bool flag = this.baseUsrSvtData.getExpInfo(out num5, out lateExp, out num7);
                this.nextExpLabel.text = $"{lateExp:N0}";
            }
        }
        if (type == CombineServantListViewItem.Type.MATERIAL)
        {
            collection = this.userServantMaster.getCombineMaterialList();
            length = collection.Length;
            List<UserServantEntity> list3 = new List<UserServantEntity>(collection);
            List<UserServantEntity> list4 = new List<UserServantEntity>();
            if (this.baseUsrSvtData != null)
            {
                for (int m = 0; m < list3.Count; m++)
                {
                    UserServantEntity entity3 = list3[m];
                    if (entity3.id == this.baseUsrSvtData.id)
                    {
                        list3.RemoveAt(m);
                    }
                }
                collection = list3.ToArray();
            }
            if ((this.selectedMtSvtList != null) && (this.selectedMtSvtList.Count > 0))
            {
                for (int n = 0; n < this.selectedMtSvtList.Count; n++)
                {
                    <setServantList>c__AnonStorey84 storey = new <setServantList>c__AnonStorey84 {
                        data = this.selectedMtSvtList[n]
                    };
                    for (int num11 = 0; num11 < list3.Count; num11++)
                    {
                        UserServantEntity entity4 = collection[num11];
                        if (storey.data.UserSvtEntity.id == entity4.id)
                        {
                            list4.Add(storey.data.UserSvtEntity);
                            list3.RemoveAll(new Predicate<UserServantEntity>(storey.<>m__E7));
                        }
                    }
                }
                list4.AddRange(list3);
                collection = list4.ToArray();
            }
            length = collection.Length;
            if (length <= 0)
            {
                this.emptyListNoticeLabel.gameObject.SetActive(true);
            }
            bool flag2 = this.baseUsrSvtData.getExpInfo(out num5, out lateExp, out num7);
            this.nextExpLabel.text = $"{lateExp:N0}";
        }
        if ((type == CombineServantListViewItem.Type.NP_MATERIAL) && (this.baseUsrSvtData != null))
        {
            collection = this.userServantMaster.getNpUpServantList(this.baseUsrSvtData);
            num4 = collection.Length;
            length = this.userServantMaster.getList(entity.userId).Length;
            length = collection.Length;
            if (length <= 0)
            {
                this.emptyListNoticeLabel.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < collection.Length; i++)
        {
            int num13 = 0;
            long id = collection[i].id;
            bool isPaty = false;
            bool isMtSvt = false;
            for (int num15 = 0; num15 < entityArray.Length; num15++)
            {
                UserDeckEntity entity5 = entityArray[num15];
                for (int num16 = 0; num16 < entity5.deckInfo.svts.Length; num16++)
                {
                    if (entity5.deckInfo.svts[num16].userSvtId == id)
                    {
                        isPaty = true;
                        break;
                    }
                }
                if (isPaty)
                {
                    break;
                }
            }
            if (((type == CombineServantListViewItem.Type.MATERIAL) || (type == CombineServantListViewItem.Type.NP_MATERIAL)) && ((this.selectedMtSvtList != null) && (this.selectedMtSvtList.Count > 0)))
            {
                for (int num17 = 0; num17 < this.selectedMtSvtList.Count; num17++)
                {
                    CombineServantListViewItem item = this.selectedMtSvtList[num17];
                    if (item.UserSvtId == id)
                    {
                        num13 = num17;
                        Debug.Log("!!!! *** selectedMtSvtList SelectNum: " + num13);
                        isMtSvt = true;
                    }
                }
            }
            CombineServantListViewItem item2 = new CombineServantListViewItem(type, index, collection[i], id == entity.favoriteUserSvtId, isPaty, this.baseUsrSvtData, isMtSvt);
            if (((type == CombineServantListViewItem.Type.MATERIAL) || (type == CombineServantListViewItem.Type.NP_MATERIAL)) && isMtSvt)
            {
                item2.SelectNum = num13;
                this.selectSum++;
            }
            base.itemList.Add(item2);
            index++;
            this.changeCombineEnableRestCnt(true, item2);
        }
        this.RefrashListDisp();
        this.servantInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), length, entity.svtKeep);
        this.cardInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), length, entity.svtKeep);
        if (type == CombineServantListViewItem.Type.NP_MATERIAL)
        {
            this.servantInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), num4, length);
            this.cardInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), num4, length);
        }
        base.SortItem(-1, false, -1);
    }

    public void showQpShortDlg()
    {
    }

    public void showRareSvtDlg()
    {
    }

    public List<CombineServantListViewObject> ClippingObjectList
    {
        get
        {
            List<CombineServantListViewObject> list = new List<CombineServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    CombineServantListViewObject component = obj2.GetComponent<CombineServantListViewObject>();
                    CombineServantListViewItem item = component.GetItem();
                    if (item.IsTermination)
                    {
                        if (base.ClippingItem(item))
                        {
                            list.Add(component);
                        }
                    }
                    else
                    {
                        list.Add(component);
                    }
                }
            }
            return list;
        }
    }

    public List<CombineServantListViewObject> ObjectList
    {
        get
        {
            List<CombineServantListViewObject> list = new List<CombineServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    CombineServantListViewObject component = obj2.GetComponent<CombineServantListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    [CompilerGenerated]
    private sealed class <setServantList>c__AnonStorey84
    {
        internal CombineServantListViewItem data;

        internal bool <>m__E7(UserServantEntity item) => 
            (item.id == this.data.UserSvtEntity.id);
    }

    public delegate void CallbackFunc(CombineServantListViewManager.ResultKind kind, int[] list);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        ENTER,
        EXIT
    }

    public enum ResultKind
    {
        NONE,
        SELECT_LIST,
        SERVANT_STATUS
    }
}

