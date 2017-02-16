using System;
using UnityEngine;

public class CombineInfoComponent : BaseMonoBehaviour
{
    private UserServantEntity baseSvtData;
    protected static readonly Color COLOR_VAL = new Color(0.99f, 0.945f, 0.316f);
    [SerializeField]
    protected UILabel currentAtkLb;
    [SerializeField]
    protected UILabel currentCostLb;
    [SerializeField]
    protected UISlider currentExpBar;
    [SerializeField]
    protected UILabel currentExpValLb;
    [SerializeField]
    protected UILabel currentHpLb;
    [SerializeField]
    protected UIGrid currentLimitCntGrid;
    [SerializeField]
    protected UIExtrusionLabel currentLvLb;
    [SerializeField]
    protected UILabel currentMaxLvLb;
    [SerializeField]
    protected GameObject currentStatusInfo;
    private DispType dispType;
    [SerializeField]
    protected UILabel infoLb;
    private static string INIT_VAL_TXT = string.Empty;
    [SerializeField]
    protected UISprite levelUpImg;
    [SerializeField]
    protected GameObject levelUpInfo;
    [SerializeField]
    protected GameObject limitCntInfoObject;
    [SerializeField]
    protected UILabel resAtkLb;
    [SerializeField]
    protected UILabel resCostLb;
    [SerializeField]
    protected UISlider resCrExpBar;
    [SerializeField]
    protected UISlider resExpBar;
    [SerializeField]
    protected UILabel resExpValLb;
    [SerializeField]
    protected UILabel resHpLb;
    [SerializeField]
    protected UIGrid resLimitCntGrid;
    [SerializeField]
    protected UIExtrusionLabel resLvLb;
    [SerializeField]
    protected UILabel resMaxLvLb;
    [SerializeField]
    protected GameObject resStatusInfo;
    private ServantEntity svtEntity;

    private void destroyCurrentStatusGrid()
    {
        int childCount = this.currentLimitCntGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.currentLimitCntGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    private void destroyResStatusGrid()
    {
        int childCount = this.resLimitCntGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.resLimitCntGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public void initStatusInfo(DispType type)
    {
        this.currentLvLb.text = INIT_VAL_TXT;
        this.currentMaxLvLb.text = INIT_VAL_TXT;
        this.currentExpBar.value = 0f;
        this.currentExpValLb.text = INIT_VAL_TXT;
        this.destroyCurrentStatusGrid();
        this.setLimitCntInfo(BalanceConfig.ServantLimitMax, 0, this.currentLimitCntGrid.transform);
        this.currentLimitCntGrid.repositionNow = true;
        this.levelUpInfo.SetActive(false);
        this.currentHpLb.text = INIT_VAL_TXT;
        this.currentAtkLb.text = INIT_VAL_TXT;
        this.currentStatusInfo.SetActive(true);
        this.resStatusInfo.SetActive(false);
        this.dispType = type;
        this.setInitInfoLb();
    }

    public void setCombineResStatusInfo(CombineSvtData resSvtData)
    {
        int num3;
        int num4;
        float num5;
        UserServantEntity baseSvtData = resSvtData.baseSvtData;
        this.destroyResStatusGrid();
        int lv = baseSvtData.lv;
        int combineResSvtLv = resSvtData.combineResSvtLv;
        bool flag = this.baseSvtData.getExpInfo(out num3, out num4, out num5);
        this.resCrExpBar.value = num5;
        UIWidget component = this.resLvLb.GetComponent<UIWidget>();
        UIWidget widget2 = this.resMaxLvLb.GetComponent<UIWidget>();
        UIWidget widget3 = this.resHpLb.GetComponent<UIWidget>();
        UIWidget widget4 = this.resAtkLb.GetComponent<UIWidget>();
        component.color = Color.white;
        widget2.color = Color.white;
        widget3.color = Color.white;
        widget4.color = Color.white;
        int num6 = combineResSvtLv - lv;
        if (num6 >= 1)
        {
            this.resExpBar.value = 1f;
            this.resCrExpBar.gameObject.SetActive(false);
            this.levelUpInfo.SetActive(true);
            component.color = COLOR_VAL;
        }
        else
        {
            this.resExpBar.value = resSvtData.combineResExpBarVal;
            this.resCrExpBar.gameObject.SetActive(true);
            this.levelUpInfo.SetActive(false);
        }
        switch (this.dispType)
        {
            case DispType.SVT_COMBINE:
                this.resLvLb.text = combineResSvtLv.ToString();
                this.resMaxLvLb.text = resSvtData.combineResSvtMaxLv.ToString();
                if (!baseSvtData.isLevelMax())
                {
                    this.infoLb.text = INIT_VAL_TXT;
                    break;
                }
                this.infoLb.text = LocalizationManager.Get("SVTEQ_LVMAX_INFO_TXT");
                break;

            case DispType.LIMITCNT_UP:
                this.resLvLb.text = baseSvtData.lv.ToString();
                this.resMaxLvLb.text = resSvtData.combineResSvtMaxLv.ToString();
                widget2.color = COLOR_VAL;
                this.infoLb.text = string.Format(LocalizationManager.Get("MAX_LVUP_TXT"), baseSvtData.getLevelMax(), resSvtData.combineResSvtMaxLv);
                this.resExpValLb.text = resSvtData.combineResNextExp.ToString();
                break;

            case DispType.SVTEQ_COMBINE:
            {
                this.resLvLb.text = combineResSvtLv.ToString();
                this.resMaxLvLb.text = resSvtData.combineResSvtMaxLv.ToString();
                int limitCount = baseSvtData.limitCount;
                int combineResLimitCnt = resSvtData.combineResLimitCnt;
                if (limitCount.Equals(combineResLimitCnt))
                {
                    if (baseSvtData.isLevelMax())
                    {
                        this.infoLb.text = LocalizationManager.Get("SVTEQ_LVMAX_INFO_TXT");
                    }
                    else
                    {
                        this.infoLb.text = INIT_VAL_TXT;
                    }
                    break;
                }
                this.infoLb.text = string.Format(LocalizationManager.Get("MAX_LVUP_TXT"), baseSvtData.getLevelMax(), resSvtData.combineResSvtMaxLv);
                widget2.color = COLOR_VAL;
                break;
            }
        }
        int maxLimitCnt = baseSvtData.getLimitCntMax();
        this.setLimitCntInfo(maxLimitCnt, resSvtData.combineResLimitCnt, this.resLimitCntGrid.transform);
        this.resLimitCntGrid.repositionNow = true;
        int combineResHp = resSvtData.combineResHp;
        this.resHpLb.text = combineResHp.ToString();
        if (baseSvtData.hp != combineResHp)
        {
            widget3.color = COLOR_VAL;
        }
        int combineResAtk = resSvtData.combineResAtk;
        this.resAtkLb.text = combineResAtk.ToString();
        if (baseSvtData.atk != combineResAtk)
        {
            widget4.color = COLOR_VAL;
        }
        this.resStatusInfo.SetActive(true);
    }

    public void setCurrentStatusInfo(UserServantEntity baseData)
    {
        int num;
        int num2;
        float num3;
        this.destroyCurrentStatusGrid();
        this.baseSvtData = baseData;
        switch (this.dispType)
        {
            case DispType.SVT_COMBINE:
            case DispType.SVTEQ_COMBINE:
                this.currentLvLb.text = this.baseSvtData.lv.ToString();
                this.currentMaxLvLb.text = this.baseSvtData.getLevelMax().ToString();
                break;

            case DispType.LIMITCNT_UP:
                this.currentLvLb.text = this.baseSvtData.lv.ToString();
                this.currentMaxLvLb.text = this.baseSvtData.getLevelMax().ToString();
                break;
        }
        bool flag = this.baseSvtData.getExpInfo(out num, out num2, out num3);
        this.currentExpBar.value = num3;
        this.currentExpValLb.text = num2.ToString();
        int maxLimitCnt = this.baseSvtData.getLimitCntMax();
        this.setLimitCntInfo(maxLimitCnt, this.baseSvtData.limitCount, this.currentLimitCntGrid.transform);
        this.currentLimitCntGrid.repositionNow = true;
        this.currentHpLb.text = this.baseSvtData.hp.ToString();
        this.currentAtkLb.text = this.baseSvtData.atk.ToString();
    }

    private void setInitInfoLb()
    {
        switch (this.dispType)
        {
            case DispType.SVT_COMBINE:
            case DispType.SVTEQ_COMBINE:
                this.infoLb.text = INIT_VAL_TXT;
                break;

            case DispType.LIMITCNT_UP:
                this.infoLb.text = "等级上限：";
                break;
        }
    }

    private void setLimitCntInfo(int maxLimitCnt, int svtLimitCnt, Transform root)
    {
        if (maxLimitCnt > 0)
        {
            for (int i = 0; i < maxLimitCnt; i++)
            {
                GameObject obj2 = base.createObject(this.limitCntInfoObject, root, null);
                obj2.transform.localPosition = Vector3.zero;
                SetLimitCntInfoComponent component = obj2.GetComponent<SetLimitCntInfoComponent>();
                component.setEnableOnImg(true);
                if (i > (svtLimitCnt - 1))
                {
                    component.setEnableOnImg(false);
                }
            }
        }
    }

    public enum DispType
    {
        SVT_COMBINE,
        LIMITCNT_UP,
        SVTEQ_COMBINE
    }
}

