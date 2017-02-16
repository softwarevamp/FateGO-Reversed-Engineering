using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIIconLabel : BaseMonoBehaviour
{
    [SerializeField]
    protected int blankSize = 2;
    [SerializeField]
    protected UILabel extentionTextLabel;
    [SerializeField]
    protected UISprite iconSprite;
    protected bool isCustmAtlas;
    [SerializeField]
    protected UILabel textLabel;

    public void Clear()
    {
        this.textLabel.text = string.Empty;
        if (this.extentionTextLabel != null)
        {
            this.extentionTextLabel.text = string.Empty;
        }
        if (this.iconSprite != null)
        {
            this.iconSprite.spriteName = null;
        }
    }

    protected Color GetPurchaseDecisionColor(int dispData, int compareData)
    {
        if ((compareData >= 0) && (dispData > compareData))
        {
            return Color.gray;
        }
        return Color.white;
    }

    public void Set(IconLabelInfo info)
    {
        if (info != null)
        {
            this.Set(info.Kind, info.Data, info.AdjustData, info.EquipData, info.TimeData, info.IsHide, info.IsMaxHide);
        }
        else
        {
            this.Clear();
        }
    }

    public void Set(IconLabelInfo info, bool isHide)
    {
        if (info != null)
        {
            this.Set(info.Kind, info.Data, info.AdjustData, info.EquipData, info.TimeData, isHide, info.IsMaxHide);
        }
        else
        {
            this.Clear();
        }
    }

    public void Set(IconLabelInfo.IconKind iconKind, int data = 0, int adjustData = 0, int equipData = 0, long time = 0, bool isHide = false, bool isMaxHide = false)
    {
        string str = null;
        int itemImageId = 0;
        bool flag = false;
        bool flag2 = true;
        string priceInfo = (data + equipData).ToString();
        string str3 = string.Empty;
        Color white = (adjustData <= 0) ? Color.white : Color.yellow;
        switch (iconKind)
        {
            case IconLabelInfo.IconKind.CLEAR:
                priceInfo = string.Empty;
                goto Label_06B2;

            case IconLabelInfo.IconKind.LEVEL:
                if (data <= 0)
                {
                    priceInfo = string.Empty;
                    goto Label_06B2;
                }
                str = "img_list_lv";
                if (adjustData <= 0)
                {
                    if (isHide)
                    {
                        priceInfo = "??";
                    }
                    goto Label_06B2;
                }
                if (!isHide)
                {
                    if (isMaxHide)
                    {
                        priceInfo = data.ToString();
                    }
                    else
                    {
                        priceInfo = data.ToString() + "/" + adjustData.ToString();
                    }
                    break;
                }
                priceInfo = "??/??";
                break;

            case IconLabelInfo.IconKind.HP:
                str = "img_list_hp";
                priceInfo = (data + equipData).ToString("#,0");
                if (isHide)
                {
                    priceInfo = "???";
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.ATK:
                str = "img_list_atk";
                priceInfo = (data + equipData).ToString("#,0");
                if (isHide)
                {
                    priceInfo = "???";
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.COST:
                str = "img_list_cost";
                if (equipData > 0)
                {
                    priceInfo = data.ToString() + "+" + equipData.ToString();
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.ID:
                str = "img_list_no";
                goto Label_06B2;

            case IconLabelInfo.IconKind.LIMIT_COUNT:
                if (data <= 0)
                {
                    priceInfo = string.Empty;
                }
                else
                {
                    str = "img_list_limitcount";
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.FREE:
                priceInfo = LocalizationManager.Get("FREE_NAME");
                goto Label_06B2;

            case IconLabelInfo.IconKind.UNIT:
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("UNIT_INFO"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.STONE_UNIT:
                itemImageId = 6;
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("STONE_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.MANA_UNIT:
                itemImageId = 7;
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("MANA_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.QP_UNIT:
                itemImageId = 5;
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("QP_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.BANK_UNIT:
                priceInfo = LocalizationManager.GetPriceInfo(data);
                goto Label_06B2;

            case IconLabelInfo.IconKind.FRIEND_POINT_UNIT:
                priceInfo = string.Format(LocalizationManager.Get("FRIENDSHIP_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.FRIEND_RANK_UNIT:
                priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                goto Label_06B2;

            case IconLabelInfo.IconKind.EVENT_ITEM_UNIT:
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("EVENT_ITEM_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.STONE_FRAGMENTS_UNIT:
                itemImageId = 6;
                flag2 = false;
                priceInfo = string.Format(LocalizationManager.Get("STONE_FRAGMENTS_UNIT"), LocalizationManager.GetNumberFormat(priceInfo));
                goto Label_06B2;

            case IconLabelInfo.IconKind.STONE:
                itemImageId = 6;
                flag2 = false;
                priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                white = this.GetPurchaseDecisionColor(data, adjustData);
                goto Label_06B2;

            case IconLabelInfo.IconKind.MANA:
                itemImageId = 7;
                flag2 = false;
                priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                white = this.GetPurchaseDecisionColor(data, adjustData);
                goto Label_06B2;

            case IconLabelInfo.IconKind.QP:
                itemImageId = 5;
                flag2 = false;
                priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                white = this.GetPurchaseDecisionColor(data, adjustData);
                goto Label_06B2;

            case IconLabelInfo.IconKind.BANK:
            case IconLabelInfo.IconKind.FRIEND_POINT:
                priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                white = this.GetPurchaseDecisionColor(data, adjustData);
                goto Label_06B2;

            case IconLabelInfo.IconKind.EVENT_ITEM:
                flag2 = false;
                if (!isHide)
                {
                    priceInfo = LocalizationManager.GetNumberFormat(priceInfo);
                }
                else
                {
                    priceInfo = "?";
                }
                white = this.GetPurchaseDecisionColor(data, adjustData);
                goto Label_06B2;

            case IconLabelInfo.IconKind.FRIEND_RANK:
                if (data < 0)
                {
                    priceInfo = string.Empty;
                }
                else
                {
                    str = "img_txt_bondslevel";
                    flag2 = false;
                    if (str != null)
                    {
                        this.iconSprite.width = 40;
                        this.iconSprite.height = 0x12;
                    }
                    if (adjustData > 0)
                    {
                        priceInfo = data.ToString() + "/" + adjustData.ToString();
                        white = Color.white;
                    }
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.NP_LEVEL:
                if (data <= 0)
                {
                    priceInfo = string.Empty;
                }
                else
                {
                    str = "img_nplv";
                    flag2 = false;
                    if (str != null)
                    {
                        this.iconSprite.width = 0x37;
                        this.iconSprite.height = 0x12;
                    }
                    if (adjustData > 0)
                    {
                        priceInfo = data.ToString() + "/" + adjustData.ToString();
                        white = Color.white;
                    }
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.RARITY:
                flag = true;
                AtlasManager.SetRarityIcon(this.iconSprite, data);
                priceInfo = string.Empty;
                goto Label_06B2;

            case IconLabelInfo.IconKind.RARITY_EXCEED:
                flag = true;
                AtlasManager.SetRarityIcon(this.iconSprite, data, adjustData);
                priceInfo = string.Empty;
                goto Label_06B2;

            case IconLabelInfo.IconKind.ADD_HP:
            case IconLabelInfo.IconKind.ADD_ATK:
            case IconLabelInfo.IconKind.ADD_COST:
                if (data > 0)
                {
                    priceInfo = "+" + data.ToString("#,0");
                }
                else if (data < 0)
                {
                    priceInfo = data.ToString("#,0");
                }
                else
                {
                    priceInfo = string.Empty;
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.DIFFER_HP:
            case IconLabelInfo.IconKind.DIFFER_ATK:
            case IconLabelInfo.IconKind.DIFFER_COST:
                if (data < 0)
                {
                    priceInfo = "(" + data.ToString("#,0") + ")";
                }
                else
                {
                    priceInfo = "(+" + data.ToString("#,0") + ")";
                }
                if (data > adjustData)
                {
                    white = Color.cyan;
                }
                else if (data < adjustData)
                {
                    white = Color.red;
                }
                else
                {
                    white = Color.white;
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.REINFORCEMENT_HP:
            case IconLabelInfo.IconKind.REINFORCEMENT_ATK:
                if (data < 0)
                {
                    priceInfo = string.Empty;
                }
                else if (adjustData > 0)
                {
                    if (data < adjustData)
                    {
                        priceInfo = data.ToString() + "/" + adjustData.ToString();
                        white = Color.white;
                    }
                    else
                    {
                        priceInfo = LocalizationManager.Get("COMMON_MAX");
                        white = Color.white;
                    }
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.EXTENTION_HP:
                str = "img_list_hp";
                priceInfo = data.ToString();
                if (equipData < 0)
                {
                    str3 = equipData.ToString();
                }
                else
                {
                    str3 = "+" + equipData.ToString();
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.EXTENTION_ATK:
                str = "img_list_atk";
                priceInfo = data.ToString();
                if (equipData < 0)
                {
                    str3 = equipData.ToString();
                }
                else
                {
                    str3 = "+" + equipData.ToString();
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.FOLLOWER_SKILL_LEVEL:
                if (data <= 0)
                {
                    priceInfo = string.Empty;
                }
                goto Label_06B2;

            case IconLabelInfo.IconKind.YEAR_DATE:
                priceInfo = LocalizationManager.GetDate(time);
                goto Label_06B2;

            default:
                goto Label_06B2;
        }
        white = Color.white;
    Label_06B2:
        this.textLabel.text = priceInfo;
        this.textLabel.color = white;
        if (this.extentionTextLabel != null)
        {
            this.extentionTextLabel.text = str3;
        }
        if (this.iconSprite != null)
        {
            if (flag)
            {
                this.isCustmAtlas = true;
            }
            else if (itemImageId > 0)
            {
                this.isCustmAtlas = true;
                AtlasManager.SetItem(this.iconSprite, itemImageId);
            }
            else
            {
                if (this.isCustmAtlas)
                {
                    this.isCustmAtlas = false;
                    AtlasManager.SetCommon(this.iconSprite);
                }
                this.iconSprite.spriteName = str;
            }
            if ((str != null) && flag2)
            {
                this.iconSprite.MakePixelPerfect();
            }
            if (this.iconSprite.transform.parent == this.textLabel.transform)
            {
                Vector2 printedSize = this.textLabel.printedSize;
                Vector3 localPosition = this.iconSprite.transform.localPosition;
                if (priceInfo == string.Empty)
                {
                    printedSize.x -= this.textLabel.spacingX;
                }
                NGUIText.Alignment alignment = this.textLabel.alignment;
                if (alignment != NGUIText.Alignment.Center)
                {
                    if (alignment != NGUIText.Alignment.Right)
                    {
                        localPosition.x = -this.blankSize;
                    }
                    else
                    {
                        localPosition.x = -(printedSize.x + this.blankSize);
                    }
                }
                else
                {
                    localPosition.x = -((printedSize.x / 2f) + this.blankSize);
                }
                this.iconSprite.transform.localPosition = localPosition;
            }
        }
    }

    public void SetCombineResTxt(int data, int adjustData)
    {
        this.textLabel.text = string.Format(LocalizationManager.Get("COMBINE_RES_INFO"), data, adjustData);
    }

    public void SetEventQuestion()
    {
        this.Set(IconLabelInfo.IconKind.EVENT_ITEM, 0, 0, 0, 0L, true, false);
    }

    public void SetPurchaseDecision(IconLabelInfo.IconKind iconKind, int price = 0, int holdCount = -1)
    {
        this.Set(iconKind, price, holdCount, 0, 0L, false, false);
    }
}

