using System;
using UnityEngine;

public class ShopBuyItemListViewItemDraw : MonoBehaviour
{
    protected long activeTime;
    [SerializeField]
    protected UISprite addRangeSprite;
    protected UIAtlas baseAtlas;
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UISprite baseSprite;
    protected string baseSpriteName;
    [SerializeField]
    protected ItemIconComponent eventItemIcon;
    [SerializeField]
    protected ItemIconComponent eventItemIcon1;
    [SerializeField]
    protected ItemIconComponent eventItemIcon2;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UISprite maskSprite;
    [SerializeField]
    protected UILabel messageTextLabel;
    [SerializeField]
    protected UICrossNarrowLabel nameTextLabel;
    [SerializeField]
    protected UILabel numTextLabel;
    [SerializeField]
    protected UILabel numTitleLabel;
    [SerializeField]
    protected UIIconLabel priceIconLabel;
    [SerializeField]
    protected UIIconLabel priceIconLabel1;
    [SerializeField]
    protected UIIconLabel priceIconLabel2;
    [SerializeField]
    protected GameObject priceInfo1;
    [SerializeField]
    protected GameObject priceInfo2;
    [SerializeField]
    protected UISprite rangeSprite;
    [SerializeField]
    protected UILabel restCountLabel;
    [SerializeField]
    protected UILabel restTimeLabel;

    protected void Awake()
    {
        if (this.baseSprite != null)
        {
            this.baseAtlas = this.baseSprite.atlas;
            this.baseSpriteName = this.baseSprite.spriteName;
        }
    }

    public void SetItem(ShopBuyItemListViewItem item, DispMode mode)
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
                string str;
                if (this.baseSprite != null)
                {
                    bool flag = false;
                    if (((item.Shop != null) && (item.Shop.bgImageId > 0)) && AtlasManager.SetShopBanner(this.baseSprite, "shop_item_menu_" + item.Shop.bgImageId))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        this.baseSprite.atlas = this.baseAtlas;
                        this.baseSprite.spriteName = this.baseSpriteName;
                    }
                }
                this.itemIcon.SetPurchase(item.PurchaseType, item.TargetId, item.ImageId);
                this.nameTextLabel.SetCrossNarrowText(item.NameText);
                if (item.Shop.checkHoldDisp())
                {
                    if (this.numTitleLabel != null)
                    {
                        this.numTitleLabel.gameObject.SetActive(true);
                        this.numTitleLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_HOLD");
                    }
                    if (this.numTextLabel != null)
                    {
                        this.numTextLabel.gameObject.SetActive(true);
                        this.numTextLabel.text = LocalizationManager.GetNumberFormat(item.Shop.getHoldCount());
                    }
                }
                else
                {
                    if (this.numTitleLabel != null)
                    {
                        this.numTitleLabel.gameObject.SetActive(false);
                    }
                    if (this.numTextLabel != null)
                    {
                        this.numTextLabel.gameObject.SetActive(false);
                    }
                }
                if (item.IsPreparation(out str))
                {
                    this.messageTextLabel.text = "[000000]" + str;
                    this.restCountLabel.text = LocalizationManager.Get("SHOP_BUY_PREPARATION");
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = false;
                        this.baseButton.SetColliderEnable(true, true);
                        this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                    }
                    else if (this.baseSprite != null)
                    {
                        this.baseSprite.color = UICommonButtonColor.normal;
                    }
                    if (this.maskSprite != null)
                    {
                        this.maskSprite.gameObject.SetActive(true);
                    }
                }
                else
                {
                    this.messageTextLabel.text = "[000000]" + item.DetailText;
                    if (this.baseButton != null)
                    {
                        this.baseButton.isEnabled = true;
                        this.baseButton.SetColliderEnable(mode == DispMode.INPUT, true);
                        this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
                    }
                    else if (this.baseSprite != null)
                    {
                        this.baseSprite.color = UICommonButtonColor.normal;
                    }
                    if (this.maskSprite != null)
                    {
                        this.maskSprite.gameObject.SetActive(item.IsSoldOut);
                    }
                    if (item.LimitNum > 0)
                    {
                        int data = item.LimitNum - item.ToTalNum;
                        if (data > 0)
                        {
                            this.restCountLabel.text = string.Format(LocalizationManager.Get("COUNT_INFO"), LocalizationManager.GetNumberFormat(data));
                        }
                        else
                        {
                            this.restCountLabel.text = LocalizationManager.Get("SHOP_BUY_SOLD_OUT");
                        }
                    }
                    else
                    {
                        this.restCountLabel.text = LocalizationManager.Get("UNIT_REST_NONE");
                    }
                }
                this.activeTime = item.ActiveTime;
                if (this.activeTime > 0L)
                {
                    this.restTimeLabel.text = LocalizationManager.GetRestTime2(this.activeTime);
                }
                else
                {
                    this.restTimeLabel.text = LocalizationManager.Get("TIME_REST2_NONE");
                }
                if (this.priceInfo1 != null)
                {
                    int[] numArray = new int[item.ItemCount];
                    UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                    switch (item.Shop.GetPayType())
                    {
                        case PayType.Type.STONE:
                            numArray[0] = entity.stone;
                            break;

                        case PayType.Type.MANA:
                            numArray[0] = entity.mana;
                            break;

                        case PayType.Type.EVENT_ITEM:
                            for (int i = 0; i < item.ItemCount; i++)
                            {
                                UserItemEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, item.Shop.GetItemIDs(i));
                                numArray[i] = entity2.num;
                            }
                            break;
                    }
                    if (item.ItemCount > 1)
                    {
                        this.priceInfo2.SetActive(true);
                        this.priceInfo1.SetActive(false);
                        this.priceIconLabel1.Set(item.PriceIcon, item.eventPrice(0), 0, 0, 0L, false, false);
                        this.priceIconLabel2.Set(item.PriceIcon, item.eventPrice(1), 0, 0, 0L, false, false);
                        if (item.EventItem[0] != null)
                        {
                            this.eventItemIcon1.SetItem(item.EventItem[0].imageId, -1);
                        }
                        else
                        {
                            this.eventItemIcon1.Clear();
                        }
                        if (item.EventItem[1] != null)
                        {
                            this.eventItemIcon2.SetItem(item.EventItem[1].imageId, -1);
                        }
                        else
                        {
                            this.eventItemIcon2.Clear();
                        }
                    }
                    else
                    {
                        this.priceInfo1.SetActive(true);
                        this.priceInfo2.SetActive(false);
                        this.priceIconLabel.SetPurchaseDecision(item.PriceIcon, item.Price, numArray[0]);
                        if (item.EventItem[0] != null)
                        {
                            this.eventItemIcon.SetItem(item.EventItem[0].imageId, -1);
                        }
                        else
                        {
                            this.eventItemIcon.Clear();
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

