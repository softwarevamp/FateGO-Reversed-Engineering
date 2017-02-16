using System;
using System.Collections.Generic;
using UnityEngine;

public class SvtEqCombineListViewItemDraw : BaseMonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected UISprite faceImg;
    [SerializeField]
    protected UISprite frameImg;
    public UIGrid limitCntGrid;
    public GameObject limitCntInfo;
    public GameObject limitCntInfoObject;
    protected List<GameObject> limitCntObjList = new List<GameObject>();
    [SerializeField]
    protected UISprite lockImg;
    [SerializeField]
    protected GameObject lockObject;
    [SerializeField]
    protected UILabel maskLabel;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected FlashingIconComponent partyIcon;
    [SerializeField]
    protected UISprite partyImg;
    [SerializeField]
    protected UISprite removeImg;
    [SerializeField]
    protected GameObject selectObject;
    [SerializeField]
    protected UILabel selectTextLabel;
    [SerializeField]
    private ServantFaceIconComponent servantface;
    [SerializeField]
    protected UILabel statusTextLabel;
    [SerializeField]
    protected UISprite svtStatusImg;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetInput(SvtEqCombineListViewItem item, bool isInput)
    {
        bool flag = false;
        if (item != null)
        {
            if (item.IsCanNotSelect)
            {
                flag = true;
            }
            if (item.IsCanNotBaseSelect)
            {
            }
        }
        else
        {
            isInput = false;
        }
        if (flag)
        {
            if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.maskSprite.gameObject.SetActive(true);
                if ((item.IsMaxNextLv && !item.IsLock) && !item.IsEquip)
                {
                    this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL_BASE_LVMAX");
                }
            }
        }
        else
        {
            if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.baseButton.SetState(UIButtonColor.State.Normal, true);
                this.maskSprite.gameObject.SetActive(false);
            }
            if ((item != null) && item.IsSelectMax)
            {
                if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UIButtonColor.State.Normal, true);
                }
            }
            else if (((item != null) && !item.IsSelectMax) && (this.baseButton != null))
            {
                this.baseButton.isEnabled = true;
                this.baseButton.SetState(UIButtonColor.State.Normal, true);
            }
        }
        if (this.selectObject != null)
        {
            if ((item != null) && item.IsSelect)
            {
                this.selectObject.SetActive(true);
                this.selectTextLabel.text = $"{item.SelectNum + 1}";
            }
            else
            {
                this.selectObject.SetActive(false);
                this.selectTextLabel.text = string.Empty;
            }
        }
    }

    public void SetItem(SvtEqCombineListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (item != null)
        {
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.servantface.Set(item.UserSvtEntity.id, item.IconInfo);
            this.svtStatusImg.gameObject.SetActive(false);
            this.lockImg.gameObject.SetActive(false);
            this.partyIcon.Set(false);
            this.removeImg.gameObject.SetActive(false);
            this.statusTextLabel.gameObject.SetActive(false);
            this.statusTextLabel.text = string.Empty;
            this.maskSprite.gameObject.SetActive(false);
            this.maskLabel.text = string.Empty;
            this.limitCntInfo.SetActive(false);
            int getMaxLimitCnt = item.GetMaxLimitCnt;
            if (this.limitCntObjList.Count != getMaxLimitCnt)
            {
                if (this.limitCntObjList.Count > 0)
                {
                    for (int i = 0; i < this.limitCntObjList.Count; i++)
                    {
                        UnityEngine.Object.Destroy(this.limitCntObjList[i]);
                    }
                    this.limitCntObjList.Clear();
                }
                if (getMaxLimitCnt > 0)
                {
                    for (int j = 0; j < getMaxLimitCnt; j++)
                    {
                        GameObject obj2 = base.createObject(this.limitCntInfoObject, this.limitCntGrid.transform, null);
                        obj2.transform.localPosition = Vector3.zero;
                        this.limitCntObjList.Add(obj2);
                    }
                    this.limitCntGrid.repositionNow = true;
                }
            }
            if (item.ListType == SvtEqCombineListViewItem.Type.SVTEQ_BASE)
            {
                int getCurrentLimitCnt = item.GetCurrentLimitCnt;
                for (int k = 0; k < this.limitCntObjList.Count; k++)
                {
                    SetLimitCntInfoComponent component = this.limitCntObjList[k].GetComponent<SetLimitCntInfoComponent>();
                    component.setEnableOnImg(false);
                    if (k <= (getCurrentLimitCnt - 1))
                    {
                        component.setEnableOnImg(true);
                    }
                }
                this.limitCntInfo.SetActive(true);
                if (item.IsEquip)
                {
                    this.partyIcon.Set(true);
                }
                if (item.IsLock)
                {
                    this.lockImg.gameObject.SetActive(true);
                }
                if (item.IsCanNotBaseSelect)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                        this.maskSprite.gameObject.SetActive(true);
                        this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                    }
                }
                else if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    this.maskSprite.gameObject.SetActive(false);
                    this.maskLabel.text = string.Empty;
                }
                if (item.IsBaseSvt)
                {
                    this.removeImg.gameObject.SetActive(true);
                }
            }
            if (item.ListType == SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL)
            {
                int num6 = item.GetCurrentLimitCnt;
                for (int m = 0; m < this.limitCntObjList.Count; m++)
                {
                    SetLimitCntInfoComponent component2 = this.limitCntObjList[m].GetComponent<SetLimitCntInfoComponent>();
                    component2.setEnableOnImg(false);
                    if (m <= (num6 - 1))
                    {
                        component2.setEnableOnImg(true);
                    }
                }
                this.limitCntInfo.SetActive(true);
                if (item.IsLimitTarget)
                {
                    this.statusTextLabel.gameObject.SetActive(true);
                    this.statusTextLabel.text = LocalizationManager.Get("MSG_ABLED_SVTEP_LIMITUP");
                }
                if (item.IsEquip)
                {
                    this.partyIcon.Set(true);
                    this.maskLabel.text = LocalizationManager.Get("SELECT_PARTY_EQUIP");
                    this.statusTextLabel.gameObject.SetActive(false);
                }
                if (item.IsLock)
                {
                    this.lockImg.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("LOCK_SERVANT");
                    this.statusTextLabel.gameObject.SetActive(false);
                }
                if (item.IsMtSelect)
                {
                }
                if (this.selectObject != null)
                {
                    if ((item != null) && item.IsSelect)
                    {
                        this.selectObject.SetActive(true);
                        this.selectTextLabel.text = $"{item.SelectNum + 1}";
                    }
                    else
                    {
                        this.selectObject.SetActive(false);
                        this.selectTextLabel.text = string.Empty;
                    }
                }
                if (item.IsCanNotSelect)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                        this.maskSprite.gameObject.SetActive(true);
                        if (item.IsUseSupportEquip && !item.IsEquip)
                        {
                            this.maskLabel.text = LocalizationManager.Get("SUPPORT_EQUIP");
                            this.statusTextLabel.gameObject.SetActive(false);
                        }
                        if ((item.IsMaxNextLv || item.IsBaseLvMax) && (!item.IsLock && !item.IsEquip))
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL_BASE_LVMAX");
                        }
                    }
                }
                else
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                        this.maskSprite.gameObject.SetActive(false);
                    }
                    if ((item != null) && item.IsSelectMax)
                    {
                        if (this.baseButton != null)
                        {
                            this.baseButton.isEnabled = true;
                            this.baseButton.SetState(UIButtonColor.State.Normal, true);
                        }
                    }
                    else if (((item != null) && !item.IsSelectMax) && (this.baseButton != null))
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    }
                }
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

