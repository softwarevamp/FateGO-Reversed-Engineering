using System;
using UnityEngine;

public class FriendOperationItemListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton acceptButton;
    [SerializeField]
    protected UISprite addRangeSprite;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UICommonButton cancelButton;
    [SerializeField]
    protected UILabel cancelLabel;
    [SerializeField]
    protected UILabel loginDataLabel;
    [SerializeField]
    protected UICommonButton offerButton;
    [SerializeField]
    protected UIIconLabel playerLevelIconLabel;
    [SerializeField]
    protected UILabel playerNameLabel;
    [SerializeField]
    protected UISprite rangeSprite;
    [SerializeField]
    protected UICommonButton rejectButton;
    [SerializeField]
    protected UICommonButton removeButton;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UILabel servantNameLabel;
    [SerializeField]
    protected GameObject[] skillBaseList = new GameObject[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UICommonButton[] skillButtonList;
    [SerializeField]
    protected SkillIconComponent[] skillIconList = new SkillIconComponent[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UIIconLabel[] skillLevelIconLabelList = new UIIconLabel[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected GameObject skillMainBase;
    [SerializeField]
    protected GameObject skillType1Base;
    [SerializeField]
    protected GameObject skillType2Base;
    [SerializeField]
    protected UILabel svtNpTitleLabel;
    [SerializeField]
    protected UISprite tdLevelIconSprite;

    public void SetInput(FriendOperationItemListViewItem item, bool isInput)
    {
        if (item == null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(false);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(false);
            }
        }
        if (this.skillButtonList != null)
        {
            int length = this.skillButtonList.Length;
            for (int i = 0; i < length; i++)
            {
                UICommonButton button = this.skillButtonList[i];
                button.isEnabled = true;
                button.SetState(UICommonButtonColor.State.Normal, false);
                button.enabled = isInput;
            }
        }
        if (this.offerButton.GetComponent<Collider>() != null)
        {
            this.offerButton.GetComponent<Collider>().enabled = false;
            this.acceptButton.GetComponent<Collider>().enabled = false;
            this.rejectButton.GetComponent<Collider>().enabled = false;
            this.cancelButton.GetComponent<Collider>().enabled = false;
            this.removeButton.GetComponent<Collider>().enabled = false;
            if (this.baseButton != null)
            {
                this.baseButton.GetComponent<Collider>().enabled = isInput;
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            if (item != null)
            {
                switch (item.Kind)
                {
                    case FriendStatus.Kind.SEARCH:
                        this.offerButton.GetComponent<Collider>().enabled = isInput;
                        this.offerButton.SetState(UICommonButtonColor.State.Normal, true);
                        break;

                    case FriendStatus.Kind.OFFER:
                        this.cancelButton.GetComponent<Collider>().enabled = isInput;
                        this.cancelButton.SetState(UICommonButtonColor.State.Normal, true);
                        break;

                    case FriendStatus.Kind.OFFERED:
                        this.acceptButton.GetComponent<Collider>().enabled = isInput;
                        this.rejectButton.GetComponent<Collider>().enabled = isInput;
                        this.acceptButton.SetState(UICommonButtonColor.State.Normal, true);
                        this.rejectButton.SetState(UICommonButtonColor.State.Normal, true);
                        break;

                    case FriendStatus.Kind.FRIEND:
                        this.removeButton.GetComponent<Collider>().enabled = isInput;
                        this.removeButton.SetState(UICommonButtonColor.State.Normal, true);
                        break;
                }
            }
        }
    }

    public void SetItem(FriendOperationItemListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(false);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(mode == DispMode.INVISIBLE);
            }
            if (this.addRangeSprite != null)
            {
                this.addRangeSprite.gameObject.SetActive(item.IsTerminationSpace);
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
                bool isNewIconDisp = false;
                if (item.Kind == FriendStatus.Kind.FRIEND)
                {
                    isNewIconDisp = true;
                }
                ServantLeaderInfo servantLeaderInfo = item.GameUser.getServantLeaderInfo(item.ClassPos);
                if ((servantLeaderInfo != null) && (servantLeaderInfo.userSvtId == 0))
                {
                    servantLeaderInfo = null;
                }
                this.servantFaceIcon.Set(servantLeaderInfo, item.IconInfo, isNewIconDisp);
                this.playerNameLabel.text = item.PlayerNameText;
                this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, item.PlayerLevel, 0, 0, 0L, false, false);
                this.servantNameLabel.text = item.SvtNameText;
                item.GetNpInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7);
                int treasureDeviceLevelIcon = item.GetTreasureDeviceLevelIcon();
                if (item.SvtEntity != null)
                {
                    this.svtNpTitleLabel.text = string.Format(LocalizationManager.Get((treasureDeviceLevelIcon <= 1) ? "NP_COLOR_NAME" : "NP_MAX_COLOR_NAME"), str);
                }
                else
                {
                    this.svtNpTitleLabel.text = LocalizationManager.Get("NO_ENTRY_NAME");
                }
                if (treasureDeviceLevelIcon > 1)
                {
                    this.tdLevelIconSprite.spriteName = "icon_nplv_2";
                }
                else if (treasureDeviceLevelIcon == 1)
                {
                    this.tdLevelIconSprite.spriteName = "icon_nplv_1";
                }
                else
                {
                    this.tdLevelIconSprite.spriteName = null;
                }
                this.loginDataLabel.text = string.Format(LocalizationManager.Get("TIME_BEFORE_TITLE_COLOR"), LocalizationManager.GetBeforeTime(item.LoginTime));
                if (item.SvtEntity != null)
                {
                    int[] numArray;
                    int[] numArray2;
                    int[] numArray3;
                    string[] strArray;
                    string[] strArray2;
                    item.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                    int num9 = 1;
                    for (int i = 0; i < this.skillIconList.Length; i++)
                    {
                        if ((i < numArray.Length) && (numArray[i] > 0))
                        {
                            num9 = i + 1;
                        }
                    }
                    for (int j = 0; j < this.skillIconList.Length; j++)
                    {
                        if (j < num9)
                        {
                            this.skillBaseList[j].SetActive(true);
                            this.skillIconList[j].Set(numArray[j], numArray2[j]);
                            this.skillLevelIconLabelList[j].Set(IconLabelInfo.IconKind.FOLLOWER_SKILL_LEVEL, numArray2[j], 0, 0, 0L, false, false);
                        }
                        else
                        {
                            this.skillBaseList[j].SetActive(false);
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < this.skillIconList.Length; k++)
                    {
                        this.skillBaseList[k].SetActive(false);
                    }
                }
                Transform transform = this.skillType1Base.transform;
                switch (item.Kind)
                {
                    case FriendStatus.Kind.SEARCH:
                        this.offerButton.gameObject.SetActive(true);
                        this.acceptButton.gameObject.SetActive(false);
                        this.rejectButton.gameObject.SetActive(false);
                        this.cancelButton.gameObject.SetActive(false);
                        this.removeButton.gameObject.SetActive(false);
                        break;

                    case FriendStatus.Kind.OFFER:
                        this.offerButton.gameObject.SetActive(false);
                        this.acceptButton.gameObject.SetActive(false);
                        this.rejectButton.gameObject.SetActive(false);
                        this.cancelButton.gameObject.SetActive(true);
                        this.removeButton.gameObject.SetActive(false);
                        this.cancelLabel.text = LocalizationManager.Get("FRIEND_BUTTON_CANCEL");
                        break;

                    case FriendStatus.Kind.OFFERED:
                        transform = this.skillType2Base.transform;
                        this.offerButton.gameObject.SetActive(false);
                        this.acceptButton.gameObject.SetActive(true);
                        this.rejectButton.gameObject.SetActive(true);
                        this.cancelButton.gameObject.SetActive(false);
                        this.removeButton.gameObject.SetActive(false);
                        break;

                    case FriendStatus.Kind.FRIEND:
                        this.offerButton.gameObject.SetActive(false);
                        this.acceptButton.gameObject.SetActive(false);
                        this.rejectButton.gameObject.SetActive(false);
                        this.cancelButton.gameObject.SetActive(false);
                        this.removeButton.gameObject.SetActive(true);
                        break;
                }
                this.skillMainBase.transform.localPosition = transform.localPosition;
                this.skillMainBase.transform.localScale = transform.localScale;
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

