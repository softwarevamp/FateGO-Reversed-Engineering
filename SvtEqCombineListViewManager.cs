using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SvtEqCombineListViewManager : ListViewManager
{
    [SerializeField]
    protected UIButton allReleaseButton;
    protected UserServantEntity baseUsrSvtData;
    protected long baseUsrSvtId;
    protected int callbackCount;
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
    protected UISprite getExpBgImg;
    [SerializeField]
    protected GameObject getExpInfo;
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
    private bool isSelectMaterial;
    protected bool isSortInfoInit;
    private SvtEqCombineListViewItem.Type itemType;
    [SerializeField]
    protected UISprite levelUpInfoImg;
    public MenuListControl menuListCtr;
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
    private List<SvtEqCombineListViewItem> selectedMtSvtList;
    protected int selectExp;
    [SerializeField]
    protected UILabel selectInfoLabel;
    [SerializeField]
    protected int selectMax = 5;
    protected int selectQp;
    protected int selectSum;
    private UserServantEntity selectUsrSvtEntity;
    [SerializeField]
    protected UILabel servantInfoLabel;
    protected const string SORT_SAVE_KEY = "SvtEqCombine";
    protected static ListViewSort[] sortStatusList;
    [SerializeField]
    protected UISprite spendQpBg;
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
    private List<SvtEqCombineListViewItem> tempMtSvtList = new List<SvtEqCombineListViewItem>();
    private int totalExp;
    protected int userQP;
    protected UserServantMaster userServantMaster;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

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
                Debug.Log("**!! checkIncrementLv increLv : " + this.increLv);
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

    public void CreateList(SvtEqCombineListViewItem.Type type)
    {
        if (!isInitSystem)
        {
            sortStatusList = new ListViewSort[2];
            for (int i = 0; i < 2; i++)
            {
                sortStatusList[i] = new ListViewSort("SvtEqCombine" + (i + 1), ListViewSort.SortKind.LEVEL, false);
            }
            isInitSystem = true;
        }
        base.sort = sortStatusList[(int) type];
        base.sort.Load();
        switch (type)
        {
            case SvtEqCombineListViewItem.Type.SVTEQ_BASE:
                this.combineInfoMsgLb.text = LocalizationManager.Get("HEADER_MSG_SVTEQ_BASE");
                this.setDispActive(false);
                this.setBtnEnable(false);
                this.setServantList(type);
                this.selectInfoLabel.gameObject.SetActive(false);
                break;

            case SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL:
                this.combineInfoMsgLb.text = LocalizationManager.Get("HEADER_MSG_SVTEQ_MATERIAL");
                this.setDispActive(true);
                this.setBtnEnable(false);
                this.setServantList(type);
                this.selectMax = 5;
                this.selectInfoLabel.gameObject.SetActive(true);
                this.getExpInfo.SetActive(true);
                this.spendQpInfoLabel.text = LocalizationManager.Get("NEED_QP_INFO");
                this.getExpInfoLabel.text = LocalizationManager.Get("GET_EXP_INFO");
                break;
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    protected void EndCloseSelectSortKind()
    {
    }

    protected void EndSelectSortKind(bool isDecide)
    {
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantSortSelectMenu(new System.Action(this.EndCloseSelectSortKind));
    }

    public long GetAmountSortValue(int svtId)
    {
        int count = base.itemList.Count;
        long num2 = 0L;
        for (int i = 0; i < count; i++)
        {
            SvtEqCombineListViewItem item = base.itemList[i] as SvtEqCombineListViewItem;
            if (item.SvtId == svtId)
            {
                num2 += 1L;
            }
        }
        return num2;
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

    public SvtEqCombineListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as SvtEqCombineListViewItem);
        }
        return null;
    }

    public UserServantEntity GetSelectBaseSvtData() => 
        this.baseUsrSvtData;

    public SetCombineData getSelectCombineData() => 
        this.combineData;

    public UserServantEntity getSelectUsrSvtEntity() => 
        this.selectUsrSvtEntity;

    public void ModifyItem()
    {
        if (base.itemList != null)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            foreach (ListViewItem item in base.itemList)
            {
                SvtEqCombineListViewItem item2 = item as SvtEqCombineListViewItem;
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
            this.selectedMtSvtList = new List<SvtEqCombineListViewItem>();
            this.tempMtSvtList.Clear();
            List<int> list = new List<int>();
            List<long> list2 = new List<long>();
            foreach (ListViewItem item in base.itemList)
            {
                if (item.IsSelect)
                {
                    list.Add(item.Index);
                    SvtEqCombineListViewItem item2 = this.GetItem(item.Index);
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

    protected void OnClickListView(ListViewObject obj)
    {
    }

    public void OnClickReleaseAll()
    {
        Debug.Log("Manager ListView OnClickReleaseAll " + this.selectSum);
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        if (this.selectSum > 0)
        {
            foreach (ListViewItem item in base.itemList)
            {
                item.IsSelect = false;
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
        SvtEqCombineListViewItem item = (obj as SvtEqCombineListViewObject).GetItem();
        if ((this.baseUsrSvtData != null) && (this.baseUsrSvtData.id == item.UserSvtId))
        {
            Debug.Log(string.Concat(new object[] { " OnClickSelectBase Remove : ", this.baseUsrSvtData.id, " _SelectUsrSvtId: ", item.UserSvtId }));
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.baseUsrSvtData = null;
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
        }
        this.targetFSM.SendEvent("SELECT_BASE_SVT");
    }

    protected void OnClickSelectMaterial(ListViewObject obj)
    {
        SvtEqCombineListViewItem item = (obj as SvtEqCombineListViewObject).GetItem();
        if (item.IsSelect)
        {
            int selectNum = item.SelectNum;
            item.IsSelect = false;
            this.selectSum--;
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
            item.SelectNum = this.selectSum;
            this.selectSum++;
            this.RefrashListDisp();
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
                    base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    break;

                case ListViewSort.SortKind.RARITY:
                    base.sort.SetKind(ListViewSort.SortKind.AMOUNT);
                    break;

                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.COST);
                    break;

                case ListViewSort.SortKind.COST:
                    base.sort.SetKind(ListViewSort.SortKind.RARITY);
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
        SvtEqCombineListViewItem item = (obj as SvtEqCombineListViewObject).GetItem();
        this.isSelectMaterial = false;
        if ((this.selectedMtSvtList != null) && (this.selectedMtSvtList.Count > 0))
        {
            foreach (SvtEqCombineListViewItem item2 in this.selectedMtSvtList)
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
        List<SvtEqCombineListViewObject> objectList = this.ObjectList;
        this.selectQp = 0;
        this.selectExp = 0;
        this.getHpUpVal = 0;
        this.getAtkUpVal = 0;
        this.isAllUpMax = false;
        int num = 0;
        if ((this.baseUsrSvtData != null) && (this.itemType == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL))
        {
            num = this.baseUsrSvtData.getCombineQpSvtEq();
        }
        foreach (ListViewItem item in base.itemList)
        {
            SvtEqCombineListViewItem item2 = item as SvtEqCombineListViewItem;
            if (item.IsSelect)
            {
                this.selectQp += num;
                if (!this.baseUsrSvtData.isLevelMax())
                {
                    this.selectExp += item2.GetMaterialExp;
                }
            }
            else
            {
                item2.IsSelectMax = this.selectSum >= this.selectMax;
            }
        }
        this.selectInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.selectSum, this.selectMax);
        List<EventInfoData> list2 = this.menuListCtr.getCombineEventList();
        if ((list2 != null) && (list2.Count > 0))
        {
            foreach (EventInfoData data in list2)
            {
                if (data.type == 0x11)
                {
                    Debug.Log("***!! EventInfoData Exp: " + data.value);
                    this.selectExp = Mathf.CeilToInt(this.selectExp * data.value);
                }
                if (data.type == 0x10)
                {
                    Debug.Log("***!! EventInfoData Qp: " + data.value);
                    this.selectQp = Mathf.CeilToInt(this.selectQp * data.value);
                }
            }
        }
        this.spendQpLabel.text = $"{this.selectQp:N0}";
        if (this.itemType == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL)
        {
            this.spendQpLabel.color = (this.selectQp <= this.userQP) ? Color.white : Color.red;
        }
        this.getExpLabel.text = $"{this.selectExp:N0}";
        if ((this.baseUsrSvtData != null) && (this.itemType == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL))
        {
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseUsrSvtData.svtId);
            this.expType = entity.expType;
            this.totalExp = this.selectExp + this.baseUsrSvtData.exp;
            this.checkLv = this.baseUsrSvtData.lv;
            int num2 = this.baseUsrSvtData.getLevelMax();
            if (this.checkLv != num2)
            {
                while (!this.checkIncrementLv(this.checkLv))
                {
                }
                int num3 = this.increLv - this.baseUsrSvtData.lv;
                Debug.Log("***!! increAmount: " + num3);
                if (num3 >= 1)
                {
                    this.levelUpInfoImg.gameObject.SetActive(true);
                }
                else
                {
                    this.levelUpInfoImg.gameObject.SetActive(false);
                }
            }
            bool flag2 = this.increLv >= num2;
            foreach (ListViewItem item3 in base.itemList)
            {
                SvtEqCombineListViewItem item4 = item3 as SvtEqCombineListViewItem;
                if (!item4.IsSelect)
                {
                    item4.IsMaxNextLv = flag2;
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
        List<SvtEqCombineListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            SvtEqCombineListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(SvtEqCombineListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f * (i + 1));
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

    protected void RequestListObject(SvtEqCombineListViewObject.InitMode mode)
    {
        List<SvtEqCombineListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SvtEqCombineListViewObject obj2 in objectList)
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

    protected void RequestListObject(SvtEqCombineListViewObject.InitMode mode, float delay)
    {
        List<SvtEqCombineListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (SvtEqCombineListViewObject obj2 in objectList)
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
        this.getExpBgImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.getExpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.haveQpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpBg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpInfoImg.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
        this.nextExpLabel.GetComponent<UIWidget>().color = !isShow ? color : Color.white;
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
                    SvtEqCombineListViewItem item2 = item as SvtEqCombineListViewItem;
                    if (item2.IsSelect && item2.IsCanNotSelect)
                    {
                        int selectNum = item2.SelectNum;
                        item2.IsSelect = false;
                        this.selectSum--;
                        flag = true;
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
                    this.RequestListObject(SvtEqCombineListViewObject.InitMode.INPUT);
                }
                else
                {
                    this.RequestListObject(SvtEqCombineListViewObject.InitMode.VALID);
                }
                break;
            }
            case InitMode.INTO:
                base.Invoke("RequestInto", 0f);
                break;

            case InitMode.ENTER:
            {
                List<SvtEqCombineListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    clippingObjectList[i].Init(SvtEqCombineListViewObject.InitMode.ENTER, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
            case InitMode.EXIT:
            {
                List<SvtEqCombineListViewObject> list2 = this.ClippingObjectList;
                if (list2.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = list2.Count;
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].Init(SvtEqCombineListViewObject.InitMode.EXIT, new System.Action(this.OnMoveEnd), 0.1f);
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
        SvtEqCombineListViewObject obj2 = obj as SvtEqCombineListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(SvtEqCombineListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(SvtEqCombineListViewObject.InitMode.VALID);
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
        this.combineData.getHpAdjustVal = this.getHpUpVal;
        this.combineData.getAtkAdjustVal = this.getAtkUpVal;
        this.combineData.isAdjustMax = this.isAllUpMax;
        List<long> list2 = new List<long>();
        for (int i = 0; i < list.Length; i++)
        {
            SvtEqCombineListViewItem item = this.GetItem(list[i]);
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

    private void setServantList(SvtEqCombineListViewItem.Type type)
    {
        int num3;
        float num5;
        this.emptyListNoticeLabel.gameObject.SetActive(false);
        this.itemType = type;
        this.userServantMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        UserDeckEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).getDeckList(entity.userId);
        UserServantEntity[] collection = null;
        base.CreateList(0);
        int index = 0;
        int length = 0;
        this.userQP = entity.qp;
        this.haveQpLabel.text = $"{this.userQP:N0}";
        this.selectSum = 0;
        this.selectQp = 0;
        this.selectExp = 0;
        this.getHpUpVal = 0;
        this.getAtkUpVal = 0;
        this.isAllUpMax = false;
        int lateExp = 0;
        if (type == SvtEqCombineListViewItem.Type.SVTEQ_BASE)
        {
            this.levelUpInfoImg.gameObject.SetActive(false);
            collection = this.userServantMaster.getServantEquipList();
            length = collection.Length;
            if (length <= 0)
            {
                this.emptyListNoticeLabel.gameObject.SetActive(true);
            }
            List<UserServantEntity> list = new List<UserServantEntity>(collection);
            List<UserServantEntity> list2 = new List<UserServantEntity>();
            if (this.baseUsrSvtData != null)
            {
                for (int j = 0; j < collection.Length; j++)
                {
                    UserServantEntity entity2 = collection[j];
                    if (entity2.id == this.baseUsrSvtData.id)
                    {
                        list2.Add(entity2);
                        list.Remove(entity2);
                    }
                }
                list2.AddRange(list);
                collection = list2.ToArray();
                bool flag = this.baseUsrSvtData.getExpInfo(out num3, out lateExp, out num5);
                this.nextExpLabel.text = $"{lateExp:N0}";
            }
        }
        if (type == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL)
        {
            collection = this.userServantMaster.getServantEquipList();
            List<UserServantEntity> list3 = new List<UserServantEntity>(collection);
            List<UserServantEntity> list4 = new List<UserServantEntity>();
            if (this.baseUsrSvtData != null)
            {
                for (int k = 0; k < list3.Count; k++)
                {
                    UserServantEntity entity3 = list3[k];
                    if (entity3.id == this.baseUsrSvtData.id)
                    {
                        list3.RemoveAt(k);
                    }
                }
                collection = list3.ToArray();
            }
            if ((this.selectedMtSvtList != null) && (this.selectedMtSvtList.Count > 0))
            {
                for (int m = 0; m < this.selectedMtSvtList.Count; m++)
                {
                    SvtEqCombineListViewItem item = this.selectedMtSvtList[m];
                    for (int n = 0; n < list3.Count; n++)
                    {
                        UserServantEntity entity4 = collection[n];
                        if (item.UserSvtEntity.id == entity4.id)
                        {
                            list4.Add(item.UserSvtEntity);
                            list3.Remove(item.UserSvtEntity);
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
            bool flag2 = this.baseUsrSvtData.getExpInfo(out num3, out lateExp, out num5);
            this.nextExpLabel.text = $"{lateExp:N0}";
        }
        for (int i = 0; i < collection.Length; i++)
        {
            int num11 = 0;
            long id = collection[i].id;
            bool isPaty = false;
            bool isMtSvt = false;
            for (int num13 = 0; num13 < entityArray.Length; num13++)
            {
                UserDeckEntity entity5 = entityArray[num13];
                for (int num14 = 0; num14 < entity5.deckInfo.svts.Length; num14++)
                {
                    if (entity5.deckInfo.svts[num14].userSvtId == id)
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
            if (((type == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL) && (this.selectedMtSvtList != null)) && (this.selectedMtSvtList.Count > 0))
            {
                for (int num15 = 0; num15 < this.selectedMtSvtList.Count; num15++)
                {
                    SvtEqCombineListViewItem item2 = this.selectedMtSvtList[num15];
                    if (item2.UserSvtId == id)
                    {
                        num11 = num15;
                        Debug.Log("!!!! *** selectedMtSvtList SelectNum: " + num11);
                        isMtSvt = true;
                    }
                }
            }
            SvtEqCombineListViewItem item3 = new SvtEqCombineListViewItem(type, index, collection[i], id == entity.favoriteUserSvtId, isPaty, this.baseUsrSvtData, isMtSvt);
            if ((type == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL) && isMtSvt)
            {
                item3.SelectNum = num11;
                this.selectSum++;
            }
            base.itemList.Add(item3);
            index++;
        }
        this.RefrashListDisp();
        this.servantInfoLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), length, entity.svtEquipKeep);
        base.SortItem(-1, false, -1);
    }

    public void showRareSvtDlg()
    {
    }

    public List<SvtEqCombineListViewObject> ClippingObjectList
    {
        get
        {
            List<SvtEqCombineListViewObject> list = new List<SvtEqCombineListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SvtEqCombineListViewObject component = obj2.GetComponent<SvtEqCombineListViewObject>();
                    SvtEqCombineListViewItem item = component.GetItem();
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

    public List<SvtEqCombineListViewObject> ObjectList
    {
        get
        {
            List<SvtEqCombineListViewObject> list = new List<SvtEqCombineListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    SvtEqCombineListViewObject component = obj2.GetComponent<SvtEqCombineListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(SvtEqCombineListViewManager.ResultKind kind, int[] list);

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

