using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CombineServantListViewItemDraw : BaseMonoBehaviour
{
    public UIButton baseButton;
    public UISprite faceImg;
    public UISprite frameImg;
    public UIIconLabel iconLabel;
    public UIIconLabel info2IconLabel;
    public UIGrid limitCntGrid;
    public GameObject limitCntInfo;
    public GameObject limitCntInfoObject;
    protected List<GameObject> limitCntObjList = new List<GameObject>();
    public UISprite lockImg;
    public UISprite maskImg;
    public UILabel maskLabel;
    public UISprite maskSprite;
    public GameObject npLvInfo;
    public FlashingIconComponent partyIcon;
    public UISprite partyImg;
    public UISprite removeImg;
    public GameObject selectObject;
    public UILabel selectTextLabel;
    public ServantFaceIconComponent servantFaceIcon;
    public UILabel skillLvLabel;
    public UILabel statusTxtLb;
    public UISprite svtStatusImg;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    public void SetInput(CombineServantListViewItem item, bool isSelectEnable)
    {
        bool flag = false;
        bool flag2 = false;
        bool isSelect = false;
        if (item != null)
        {
            isSelect = item.IsSelect;
            if (item.IsCanNotSelect)
            {
                flag = true;
            }
            if (item.IsCanNotBaseSelect)
            {
            }
            if (item.IsOrganization)
            {
                flag2 = true;
            }
        }
        if (flag)
        {
            if (this.baseButton != null)
            {
                this.baseButton.isEnabled = true;
                this.maskSprite.gameObject.SetActive(true);
                if (item.IsMaxNextLv && !item.IsCanNotSelectMaterial)
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
            if ((((item != null) && (item.ListType == CombineServantListViewItem.Type.MATERIAL)) && (flag2 && !isSelectEnable)) && !isSelect)
            {
                this.maskSprite.gameObject.SetActive(true);
                this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
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

    public void SetItem(CombineServantListViewItem item, DispMode mode, bool isSelectEnable)
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
            int num;
            int num2;
            int num3;
            int num4;
            int num5;
            string str;
            string str2;
            int num6;
            int num7;
            this.servantFaceIcon.Set(item.UserSvtEntity, item.IconInfo);
            this.info2IconLabel.Set(item.IconInfo2);
            item.GetNpInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7);
            this.svtStatusImg.gameObject.SetActive(false);
            this.lockImg.gameObject.SetActive(false);
            this.statusTxtLb.gameObject.SetActive(false);
            this.maskSprite.gameObject.SetActive(false);
            this.maskLabel.text = string.Empty;
            this.skillLvLabel.gameObject.SetActive(false);
            this.partyIcon.Set(false);
            this.removeImg.gameObject.SetActive(false);
            this.npLvInfo.SetActive(false);
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
            this.limitCntInfo.SetActive(false);
            if (item.ListType == CombineServantListViewItem.Type.BASE)
            {
                if (item.IsPaty)
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
                        if ((item.IsLvMax && !item.IsStatusUp) && !item.IsExpUpSvt)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_BASE_ALLMAX");
                        }
                        else if (item.IsStatusUp || item.IsExpUpSvt)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                        }
                        else
                        {
                            this.statusTxtLb.text = string.Empty;
                            this.maskLabel.text = string.Empty;
                        }
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
            else if ((item.ListType == CombineServantListViewItem.Type.MATERIAL) || (item.ListType == CombineServantListViewItem.Type.NP_MATERIAL))
            {
                if (item.ListType == CombineServantListViewItem.Type.NP_MATERIAL)
                {
                    this.iconLabel.Set(IconLabelInfo.IconKind.NP_LEVEL, (num <= 0) ? -1 : num2, num3, 0, 0L, false, false);
                    this.npLvInfo.SetActive(true);
                }
                if (item.IsPaty)
                {
                    this.partyIcon.Set(true);
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("PARTY_MEMBER_TXT");
                }
                if (item.IsFavorite)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("FAVORITE_SERVANT");
                }
                if (item.IsEquip)
                {
                }
                if (item.IsLock)
                {
                    this.lockImg.gameObject.SetActive(true);
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("LOCK_SERVANT");
                }
                if (item.IsLimitTarget)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("SAME_SERVANT");
                }
                if (item.IsHeroine)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                }
                if (item.IsStatusUp && !item.IsCanStatusUp)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                }
                if (item.IsEventJoin)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                }
                if (item.IsUseSupportServant)
                {
                    this.maskSprite.gameObject.SetActive(true);
                    this.maskLabel.text = !item.IsPaty ? LocalizationManager.Get("SUPPORT_MEMBER") : LocalizationManager.Get("PARTY_MEMBER_TXT");
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
                        if ((item.IsMaxNextLv || item.IsBaseLvMax) && !item.IsCanNotSelectMaterial)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL_BASE_LVMAX");
                        }
                        if ((item.ListType == CombineServantListViewItem.Type.NP_MATERIAL) && item.IsBaseSvt)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NPUP_BASE");
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
                    if (((item.ListType == CombineServantListViewItem.Type.MATERIAL) && item.IsOrganization) && (!isSelectEnable && !item.IsSelect))
                    {
                        this.maskSprite.gameObject.SetActive(true);
                        this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                    }
                }
            }
            if (item.ListType == CombineServantListViewItem.Type.LIMITUP_BASE)
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
                if (item.IsPaty)
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
                        if (item.IsHeroine)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                        }
                        else
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_LIMITUP_BASE");
                        }
                    }
                }
                else if (!item.IsLvMax)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    }
                }
                else if (!item.IsLimitUpItemNum)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    }
                }
                else if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    this.maskSprite.gameObject.SetActive(false);
                    this.maskLabel.text = string.Empty;
                    this.statusTxtLb.gameObject.SetActive(true);
                    this.statusTxtLb.text = LocalizationManager.Get("MSG_ABLED_LIMITUP");
                }
                if (item.IsBaseSvt)
                {
                    this.removeImg.gameObject.SetActive(true);
                }
            }
            if (item.ListType == CombineServantListViewItem.Type.SKILL_BASE)
            {
                int[] numArray;
                int[] numArray2;
                int[] numArray3;
                string[] strArray;
                string[] strArray2;
                item.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                List<int> getEnableSkillupList = item.GetEnableSkillupList;
                StringBuilder builder = new StringBuilder();
                int length = numArray2.Length;
                for (int m = 0; m < length; m++)
                {
                    string str3 = (numArray2[m] <= 0) ? LocalizationManager.Get("NONSKILL_TXT") : numArray2[m].ToString();
                    if (m == (length - 1))
                    {
                        string format = !getEnableSkillupList.Contains(numArray[m]) ? LocalizationManager.Get("SKILL_LVDISP_SINGLE_TXT") : LocalizationManager.Get("SKILL_LVDISP_ENABLE_SINGLE_TXT");
                        builder.AppendFormat(format, str3);
                    }
                    else
                    {
                        string str5 = !getEnableSkillupList.Contains(numArray[m]) ? LocalizationManager.Get("SKILL_LVDISP_TXT") : LocalizationManager.Get("SKILL_LVDISP_ENABLE_TXT");
                        builder.AppendFormat(str5, str3);
                    }
                }
                this.skillLvLabel.text = builder.ToString();
                this.skillLvLabel.gameObject.SetActive(true);
                this.info2IconLabel.Clear();
                if (item.IsPaty)
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
                        this.maskLabel.text = LocalizationManager.Get("NONSELECT_SKILLUP_BASE");
                    }
                }
                else if (!item.IsSkillUpItemNum)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    }
                }
                else if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    this.statusTxtLb.gameObject.SetActive(true);
                    this.statusTxtLb.text = LocalizationManager.Get("MSG_ABLED_SKILLUP");
                }
                if (item.IsBaseSvt)
                {
                    this.removeImg.gameObject.SetActive(true);
                }
            }
            if (item.ListType == CombineServantListViewItem.Type.NP_BASE)
            {
                this.iconLabel.Set(IconLabelInfo.IconKind.NP_LEVEL, (num <= 0) ? -1 : num2, num3, 0, 0L, false, false);
                this.npLvInfo.SetActive(true);
                if (item.IsPaty)
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
                        if (item.IsHeroine)
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_MATERIAL");
                        }
                        else
                        {
                            this.maskLabel.text = LocalizationManager.Get("NONSELECT_NPUP_BASE");
                        }
                    }
                }
                else if (!item.IsSameServant && !item.IsBaseSvt)
                {
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    }
                }
                else if (this.baseButton != null)
                {
                    this.baseButton.isEnabled = true;
                    this.baseButton.SetState(UIButtonColor.State.Normal, true);
                    if (item.IsSameServant)
                    {
                        this.statusTxtLb.gameObject.SetActive(true);
                        this.statusTxtLb.text = LocalizationManager.Get("MSG_ABLED_TDUP");
                    }
                }
                if (item.IsBaseSvt)
                {
                    this.removeImg.gameObject.SetActive(true);
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

