using System;
using UnityEngine;

public class FollowerSelectItemListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected ShiningIconComponent bounusIcon;
    [SerializeField]
    protected UILabel loginDataLabel;
    [SerializeField]
    protected UIIconLabel playerLevelIconLabel;
    [SerializeField]
    protected UILabel playerNameLabel;
    [SerializeField]
    protected UILabel pointDataLabel;
    [SerializeField]
    protected UISprite rangeSprite;
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
    protected int[] skillIdList;
    [SerializeField]
    protected UIIconLabel[] skillLevelIconLabelList = new UIIconLabel[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UICommonButton supportInfoButton;
    [SerializeField]
    protected UILabel svtNpLockTextLabel;
    [SerializeField]
    protected UILabel svtNpTitleLabel;
    [SerializeField]
    protected UISprite typeTextSprite;

    public void SetInput(FollowerSelectItemListViewItem item, bool isInput, bool isTutorial)
    {
        if (this.baseButton != null)
        {
            this.baseButton.isEnabled = true;
            this.baseButton.SetState(UICommonButtonColor.State.Normal, false);
            this.baseButton.enabled = isInput;
        }
        if (this.skillButtonList != null)
        {
            int length = this.skillButtonList.Length;
            for (int i = 0; i < length; i++)
            {
                UICommonButton button = this.skillButtonList[i];
                UITouchPress component = button.GetComponent<UITouchPress>();
                if (((this.skillIdList != null) && (this.skillIdList[i] > 0)) && !isTutorial)
                {
                    button.isEnabled = true;
                    button.SetState(UICommonButtonColor.State.Normal, false);
                    button.enabled = isInput;
                    component.IsEnabled = true;
                }
                else
                {
                    button.isEnabled = false;
                    button.SetState(UICommonButtonColor.State.Normal, false);
                    button.enabled = false;
                    component.IsEnabled = false;
                }
            }
        }
        if (this.supportInfoButton.gameObject.activeSelf)
        {
            this.supportInfoButton.isEnabled = true;
            this.supportInfoButton.SetState(UICommonButtonColor.State.Normal, false);
            this.supportInfoButton.enabled = isInput;
        }
    }

    public void SetItem(FollowerSelectItemListViewItem item, DispMode mode)
    {
        if (item != null)
        {
            if (this.rangeSprite != null)
            {
                this.rangeSprite.gameObject.SetActive(mode == DispMode.INVISIBLE);
            }
            if (mode != DispMode.INVISIBLE)
            {
                ServantLeaderInfo servantLeader = item.ServantLeader;
                if ((servantLeader == null) || (servantLeader.svtId <= 0))
                {
                    EquipTargetInfo equipInfo = item.EquipInfo;
                    this.servantFaceIcon.SetEquipDangling(equipInfo);
                    this.servantNameLabel.text = LocalizationManager.Get("COMMON_NO_ENTRY");
                    switch (item.Type)
                    {
                        case Follower.Type.FRIEND:
                            this.baseSprite.spriteName = "img_listbg_01";
                            this.playerNameLabel.text = item.PlayerNameText;
                            this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, item.PlayerLevel, 0, 0, 0L, false, false);
                            this.loginDataLabel.text = string.Format(LocalizationManager.Get("TIME_BEFORE_TITLE_COLOR"), LocalizationManager.GetBeforeTime(item.LoginTime));
                            this.typeTextSprite.spriteName = "icon_friend";
                            this.typeTextSprite.MakePixelPerfect();
                            this.supportInfoButton.gameObject.SetActive(true);
                            break;

                        case Follower.Type.NOT_FRIEND:
                            this.baseSprite.spriteName = "img_listbg_01";
                            this.playerNameLabel.text = item.PlayerNameText;
                            this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, item.PlayerLevel, 0, 0, 0L, false, false);
                            this.loginDataLabel.text = string.Format(LocalizationManager.Get("TIME_BEFORE_TITLE_COLOR"), LocalizationManager.GetBeforeTime(item.LoginTime));
                            this.typeTextSprite.spriteName = null;
                            this.supportInfoButton.gameObject.SetActive(true);
                            break;

                        case Follower.Type.NPC:
                            this.baseSprite.spriteName = "img_listbg_02";
                            this.playerNameLabel.text = string.Empty;
                            this.playerLevelIconLabel.Clear();
                            this.loginDataLabel.text = string.Empty;
                            this.typeTextSprite.spriteName = "icon_support_01";
                            this.typeTextSprite.MakePixelPerfect();
                            this.supportInfoButton.gameObject.SetActive(false);
                            break;
                    }
                    this.svtNpLockTextLabel.text = string.Empty;
                    this.svtNpTitleLabel.text = LocalizationManager.Get("NO_ENTRY_NAME");
                    this.pointDataLabel.text = string.Empty;
                    if (this.bounusIcon != null)
                    {
                        this.bounusIcon.Clear();
                    }
                    if (this.skillIdList == null)
                    {
                        this.skillIdList = new int[this.skillIconList.Length];
                    }
                    for (int i = 0; i < this.skillIconList.Length; i++)
                    {
                        this.skillIdList[i] = 0;
                        this.skillBaseList[i].SetActive(false);
                    }
                }
                else
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
                    int[] numArray;
                    int[] numArray2;
                    int[] numArray3;
                    string[] strArray;
                    string[] strArray2;
                    this.servantFaceIcon.Set(servantLeader, item.IconInfo, false);
                    this.servantNameLabel.text = item.SvtNameText;
                    item.GetNpInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7);
                    int treasureDeviceLevelIcon = item.GetTreasureDeviceLevelIcon();
                    int followerPointFriend = 0;
                    switch (item.Type)
                    {
                        case Follower.Type.FRIEND:
                            this.baseSprite.spriteName = "img_listbg_01";
                            this.playerNameLabel.text = item.PlayerNameText;
                            this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, item.PlayerLevel, 0, 0, 0L, false, false);
                            this.loginDataLabel.text = string.Format(LocalizationManager.Get("TIME_BEFORE_TITLE_COLOR"), LocalizationManager.GetBeforeTime(item.LoginTime));
                            this.typeTextSprite.spriteName = "icon_friend";
                            this.typeTextSprite.MakePixelPerfect();
                            followerPointFriend = BalanceConfig.FollowerPointFriend;
                            this.svtNpLockTextLabel.text = string.Empty;
                            this.svtNpTitleLabel.text = string.Format(LocalizationManager.Get((treasureDeviceLevelIcon <= 1) ? "NP_COLOR_NAME" : "NP_MAX_COLOR_NAME"), str);
                            this.supportInfoButton.gameObject.SetActive(true);
                            break;

                        case Follower.Type.NOT_FRIEND:
                            this.baseSprite.spriteName = "img_listbg_01";
                            this.playerNameLabel.text = item.PlayerNameText;
                            this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, item.PlayerLevel, 0, 0, 0L, false, false);
                            this.loginDataLabel.text = string.Format(LocalizationManager.Get("TIME_BEFORE_TITLE_COLOR"), LocalizationManager.GetBeforeTime(item.LoginTime));
                            this.typeTextSprite.spriteName = null;
                            followerPointFriend = BalanceConfig.FollowerPointNotFriend;
                            this.svtNpLockTextLabel.text = string.Empty;
                            this.svtNpTitleLabel.text = string.Format(LocalizationManager.Get("NP_DISABLE_COLOR_NAME"), str);
                            this.supportInfoButton.gameObject.SetActive(true);
                            break;

                        case Follower.Type.NPC:
                            this.baseSprite.spriteName = "img_listbg_02";
                            this.playerNameLabel.text = string.Empty;
                            this.playerLevelIconLabel.Clear();
                            this.loginDataLabel.text = string.Empty;
                            this.typeTextSprite.spriteName = "icon_support_01";
                            this.typeTextSprite.MakePixelPerfect();
                            followerPointFriend = BalanceConfig.FollowerPointNpc;
                            this.svtNpLockTextLabel.text = string.Empty;
                            this.svtNpTitleLabel.text = string.Format(LocalizationManager.Get((treasureDeviceLevelIcon <= 1) ? "NP_COLOR_NAME" : "NP_MAX_COLOR_NAME"), str);
                            this.supportInfoButton.gameObject.SetActive(false);
                            break;
                    }
                    if (servantLeader.IsHideSupport())
                    {
                        this.servantNameLabel.text = LocalizationManager.Get("SERVANT_HIDE_NAME");
                        this.playerNameLabel.text = string.Empty;
                        this.svtNpTitleLabel.text = LocalizationManager.Get("NP_HIDE_NAME_LEVEL");
                    }
                    else
                    {
                        this.servantNameLabel.text = item.SvtNameText;
                    }
                    int friendPointUpVal = item.GetFriendPointUpVal();
                    if (friendPointUpVal > 0)
                    {
                        this.pointDataLabel.text = string.Format(LocalizationManager.Get("FOLLOWER_SELECT_TYPE_POINT_UP"), followerPointFriend, friendPointUpVal);
                    }
                    else
                    {
                        this.pointDataLabel.text = string.Format(LocalizationManager.Get("FOLLOWER_SELECT_TYPE_POINT"), followerPointFriend);
                    }
                    if (this.bounusIcon != null)
                    {
                        this.bounusIcon.Set(item.IsEventUpVal());
                    }
                    item.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                    int num11 = 1;
                    if (this.skillIdList == null)
                    {
                        this.skillIdList = new int[this.skillIconList.Length];
                    }
                    for (int j = 0; j < this.skillIconList.Length; j++)
                    {
                        if (j < numArray.Length)
                        {
                            this.skillIdList[j] = numArray[j];
                            if (numArray[j] > 0)
                            {
                                num11 = j + 1;
                            }
                        }
                        else
                        {
                            this.skillIdList[j] = 0;
                        }
                    }
                    for (int k = 0; k < this.skillIconList.Length; k++)
                    {
                        if (k < num11)
                        {
                            this.skillBaseList[k].SetActive(true);
                            if (servantLeader.IsHideSupport())
                            {
                                this.skillIdList[k] = 0;
                                this.skillIconList[k].SetHide();
                                this.skillLevelIconLabelList[k].Clear();
                            }
                            else
                            {
                                this.skillIconList[k].Set(numArray[k], numArray2[k]);
                                this.skillLevelIconLabelList[k].Set(IconLabelInfo.IconKind.FOLLOWER_SKILL_LEVEL, numArray2[k], 0, 0, 0L, false, false);
                            }
                        }
                        else
                        {
                            this.skillBaseList[k].SetActive(false);
                        }
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

