using System;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewSort")]
public class ListViewSort
{
    protected SortKind defaultSortKind;
    protected const string FILTER_SAVE_KEY = "SortFilter-";
    protected bool isAscendingOrder;
    protected bool isDefaultAscendingOrder;
    protected bool[] isFilterList;
    protected bool isRequestLoad;
    protected bool isRequestSave;
    protected bool isServantEquip;
    protected const string KIND_SAVE_KEY = "SortKind-";
    protected ListViewManager manager;
    protected const string ORDER_SAVE_KEY = "SortOrder-";
    protected string saveKey;
    protected SortKind sortKind;

    public ListViewSort(ListViewSort o)
    {
        this.isFilterList = new bool[0x12];
        this.Set(o);
    }

    public ListViewSort(SortKind sortKind, bool isAscendingOrder = true)
    {
        this.isFilterList = new bool[0x12];
        this.defaultSortKind = sortKind;
        this.isDefaultAscendingOrder = isAscendingOrder;
        this.Clear();
        this.sortKind = sortKind;
        this.isAscendingOrder = isAscendingOrder;
    }

    public ListViewSort(string saveKey, SortKind sortKind, bool isAscendingOrder = true)
    {
        this.isFilterList = new bool[0x12];
        this.defaultSortKind = sortKind;
        this.isDefaultAscendingOrder = isAscendingOrder;
        this.Clear();
        this.saveKey = saveKey;
        this.sortKind = sortKind;
        this.isAscendingOrder = isAscendingOrder;
        if (!string.IsNullOrEmpty(this.saveKey))
        {
            this.isRequestLoad = true;
        }
    }

    public void ClassFilterOFF()
    {
        for (int i = 0; i < 8; i++)
        {
            this.isFilterList[i] = false;
        }
    }

    public void Clear()
    {
        this.sortKind = SortKind.LEVEL;
        this.isAscendingOrder = true;
        this.isServantEquip = false;
        this.ClearFilter();
    }

    public void ClearFilter()
    {
        int num = 0x12;
        for (int i = 0; i < num; i++)
        {
            this.isFilterList[i] = true;
        }
    }

    ~ListViewSort()
    {
    }

    public bool GetFilter(FilterKind kind) => 
        this.isFilterList[(int) kind];

    public string GetKindButtonText() => 
        this.GetKindText(this.sortKind);

    public string GetKindText() => 
        this.GetKindText(this.sortKind);

    public string GetKindText(SortKind kind)
    {
        switch (kind)
        {
            case SortKind.PARTY:
                return LocalizationManager.Get("SERVANT_SORT_KIND_PARTY");

            case SortKind.CREATE:
                return LocalizationManager.Get("SERVANT_SORT_KIND_CREATE");

            case SortKind.RARITY:
                return LocalizationManager.Get("SERVANT_SORT_KIND_RARITY");

            case SortKind.LEVEL:
                return LocalizationManager.Get("SERVANT_SORT_KIND_LEVEL");

            case SortKind.NP_LEVEL:
                return LocalizationManager.Get("SERVANT_SORT_KIND_NP_LEVEL");

            case SortKind.HP:
                return LocalizationManager.Get("SERVANT_SORT_KIND_HP");

            case SortKind.ATK:
                return LocalizationManager.Get("SERVANT_SORT_KIND_ATK");

            case SortKind.COST:
                return LocalizationManager.Get("SERVANT_SORT_KIND_COST");

            case SortKind.CLASS:
                return LocalizationManager.Get("SERVANT_SORT_KIND_CLASS");

            case SortKind.LIMIT_COUNT:
                return LocalizationManager.Get("SERVANT_SORT_KIND_LIMIT_COUNT");

            case SortKind.FRIENDSHIP:
                return LocalizationManager.Get("SERVANT_SORT_KIND_FRIENDSHIP");

            case SortKind.LOGIN_ACCESS:
                return LocalizationManager.Get("SERVANT_SORT_KIND_LOGIN_ACCESS");

            case SortKind.USER_LEVEL:
                return LocalizationManager.Get("SERVANT_SORT_KIND_USER_LEVEL");

            case SortKind.ID:
                return LocalizationManager.Get("SERVANT_SORT_KIND_ID");

            case SortKind.AMOUNT:
                return LocalizationManager.Get("SERVANT_SORT_KIND_AMOUNT");
        }
        return LocalizationManager.GetUnknownName();
    }

    public ListViewManager GetManager() => 
        this.manager;

    public void InitLoad()
    {
        this.isRequestLoad = true;
        this.isRequestSave = false;
        this.Clear();
        this.sortKind = this.defaultSortKind;
        this.isAscendingOrder = this.isDefaultAscendingOrder;
    }

    public void Load()
    {
        if (this.isRequestLoad)
        {
            this.isRequestLoad = false;
            this.isRequestSave = true;
            this.sortKind = (SortKind) PlayerPrefs.GetInt("SortKind-" + this.saveKey, (int) this.defaultSortKind);
            this.isAscendingOrder = PlayerPrefs.GetInt("SortOrder-" + this.saveKey, !this.isDefaultAscendingOrder ? 0 : 1) != 0;
            int @int = PlayerPrefs.GetInt("SortFilter-" + this.saveKey, 0);
            int num2 = 0x12;
            for (int i = 0; i < num2; i++)
            {
                this.isFilterList[i] = (@int & 1) == 0;
                @int = @int >> 1;
            }
        }
    }

    public void Save()
    {
        if (this.isRequestSave)
        {
            SortKind @int = (SortKind) PlayerPrefs.GetInt("SortKind-" + this.saveKey, (int) this.defaultSortKind);
            bool flag = PlayerPrefs.GetInt("SortOrder-" + this.saveKey, !this.isDefaultAscendingOrder ? 0 : 1) != 0;
            int num = PlayerPrefs.GetInt("SortFilter-" + this.saveKey, 0);
            int num2 = 0;
            for (int i = 0x11; i >= 0; i--)
            {
                num2 = num2 << 1;
                if (!this.isFilterList[i])
                {
                    num2 |= 1;
                }
            }
            if (((this.sortKind != @int) || (this.isAscendingOrder != flag)) || (num2 != num))
            {
                PlayerPrefs.SetInt("SortKind-" + this.saveKey, (int) this.sortKind);
                PlayerPrefs.SetInt("SortOrder-" + this.saveKey, !this.isAscendingOrder ? 0 : 1);
                PlayerPrefs.SetInt("SortFilter-" + this.saveKey, num2);
                PlayerPrefs.Save();
            }
        }
    }

    public void Set(ListViewSort o)
    {
        this.sortKind = o.sortKind;
        this.isAscendingOrder = o.isAscendingOrder;
        this.isServantEquip = o.isServantEquip;
        int num = 0x12;
        for (int i = 0; i < num; i++)
        {
            this.isFilterList[i] = o.isFilterList[i];
        }
    }

    public void SetAscendingOrder(bool flag)
    {
        this.isAscendingOrder = flag;
    }

    public void SetFilter(FilterKind kind, bool isEnable)
    {
        this.isFilterList[(int) kind] = isEnable;
    }

    public void SetKind(SortKind kind)
    {
        this.sortKind = kind;
    }

    public void SetManager(ListViewManager manager)
    {
        this.manager = manager;
    }

    public void SetServantEquip(bool flag)
    {
        this.isServantEquip = flag;
    }

    public void SwitchFilter(FilterKind kind)
    {
        this.isFilterList[(int) kind] = !this.isFilterList[(int) kind];
    }

    public bool IsAscendingOrder =>
        this.isAscendingOrder;

    public bool IsRequestLoad =>
        this.isRequestLoad;

    public bool IsRequestSave =>
        this.isRequestSave;

    public bool IsServantEquip =>
        this.isServantEquip;

    public SortKind Kind =>
        this.sortKind;

    public enum FilterKind
    {
        CLASS_1_13,
        CLASS_2_14,
        CLASS_3_15,
        CLASS_4_16,
        CLASS_5_17,
        CLASS_6_18,
        CLASS_7_19,
        CLASS_ETC,
        CLASS_EXP_UP,
        CLASS_STATUS_UP,
        COLLECTION_NOT_GET,
        COLLECTION_FIND,
        COLLECTION_GET,
        MISSION_CLEAR,
        MISSION_PROGRESS,
        MISSION_NOSTART,
        MISSION_COMPLETE,
        MISSION_END,
        SUM
    }

    public enum SortKind
    {
        PARTY,
        CREATE,
        RARITY,
        LEVEL,
        NP_LEVEL,
        HP,
        ATK,
        COST,
        CLASS,
        LIMIT_COUNT,
        FRIENDSHIP,
        LOGIN_ACCESS,
        USER_LEVEL,
        ID,
        AMOUNT
    }
}

