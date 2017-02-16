using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopBuyItemConfirmMenu : BaseDialog
{
    private const int BuyCancel = 0;
    private const int BuyDecide = 1;
    [SerializeField]
    protected UICommonButton cancelButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected ItemIconComponent eventItemIcon;
    [SerializeField]
    protected ItemIconComponent[] eventItemIcons;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UISprite itemMaskeSprite;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected UICrossNarrowLabel nameLabel;
    [SerializeField]
    protected NoTitleDialog noTitleDialog;
    [SerializeField]
    protected UILabel numTextLabel;
    [SerializeField]
    protected UILabel numTitleLabel;
    [SerializeField]
    protected UILabel priceDataLabel;
    [SerializeField]
    protected UILabel[] priceDataLabels;
    [SerializeField]
    protected UIIconLabel priceIconLabel;
    [SerializeField]
    protected UIIconLabel[] priceIconLabels;
    [SerializeField]
    protected GameObject priceInfo1;
    [SerializeField]
    protected GameObject priceInfo2;
    protected ShopEntity shopEntity;
    protected State state;
    protected StoneShopEntity stoneShopEntity;
    [SerializeField]
    protected UILabel warningLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(int result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.nameLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.warningLabel.text = string.Empty;
        this.itemIcon.Clear();
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        this.shopEntity = null;
        this.stoneShopEntity = null;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(0);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).IsOpenNoQuestEvent(this.shopEntity))
            {
                this.noTitleDialog.Open(string.Format(this.shopEntity.warningMessage, this.shopEntity.name), LocalizationManager.Get("NO_QUEST_EVENT_SHOP_DECIDE_BUTTON"), LocalizationManager.Get("COMMON_CONFIRM_CANCEL"), new NoTitleDialog.ClickDelegate(this.returnWarning));
            }
            else if ((this.shopEntity != null) && !this.shopEntity.IsEnable(0L))
            {
                this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_CONFIRM_PERIOD_WARNING");
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.decideButton.SetState(UICommonButtonColor.State.Disabled, false);
            }
            else
            {
                this.state = State.SELECTED;
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                this.Callback(1);
            }
        }
    }

    public void Open(ShopEntity entity, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            string str;
            this.callbackFunc = callback;
            this.shopEntity = entity;
            this.stoneShopEntity = null;
            base.gameObject.SetActive(true);
            this.warningLabel.text = string.Empty;
            this.nameLabel.SetCrossNarrowText(entity.name);
            this.messageLabel.text = "[000000]" + entity.detail;
            UserGameEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            bool flag = true;
            bool flag2 = this.shopEntity.IsPreparation(out str);
            if (flag2)
            {
                flag = false;
                this.warningLabel.text = str;
            }
            this.itemMaskeSprite.gameObject.SetActive(flag2);
            if (this.shopEntity.IsSoldOut())
            {
                flag = false;
                this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_CONFIRM_SOLD_OUT_WARNING");
            }
            if (flag && !this.shopEntity.IsEnable(0L))
            {
                flag = false;
                this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_CONFIRM_PERIOD_WARNING");
            }
            if (this.shopEntity.checkHoldDisp())
            {
                if (this.numTitleLabel != null)
                {
                    this.numTitleLabel.gameObject.SetActive(true);
                    this.numTitleLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_HOLD");
                }
                if (this.numTextLabel != null)
                {
                    this.numTextLabel.gameObject.SetActive(true);
                    this.numTextLabel.text = LocalizationManager.GetNumberFormat(this.shopEntity.getHoldCount());
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
            switch (this.shopEntity.GetPayType())
            {
                case PayType.Type.STONE:
                    this.priceInfo1.SetActive(true);
                    this.priceInfo2.SetActive(false);
                    this.priceDataLabel.text = string.Format(LocalizationManager.Get("STONE_NEED_INFO"), this.shopEntity.GetPrice());
                    this.priceIconLabel.SetPurchaseDecision(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrice(), entity2.stone);
                    this.eventItemIcon.Clear();
                    if (flag && (this.shopEntity.GetPrice() > entity2.stone))
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_STONE_CONFIRM_WARNING");
                    }
                    break;

                case PayType.Type.MANA:
                    this.priceInfo1.SetActive(true);
                    this.priceInfo2.SetActive(false);
                    this.priceDataLabel.text = string.Format(LocalizationManager.Get("MANA_NEED_INFO"), this.shopEntity.GetPrice());
                    this.priceIconLabel.SetPurchaseDecision(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrice(), entity2.mana);
                    this.eventItemIcon.Clear();
                    if (flag && (this.shopEntity.GetPrice() > entity2.mana))
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_MANA_CONFIRM_WARNING");
                    }
                    break;

                case PayType.Type.EVENT_ITEM:
                {
                    ItemEntity entity3;
                    UserItemEntity entity4;
                    int itemCount = this.shopEntity.GetItemCount();
                    if (itemCount != 1)
                    {
                        this.priceInfo1.SetActive(false);
                        this.priceInfo2.SetActive(true);
                        bool flag3 = false;
                        for (int i = 0; i < itemCount; i++)
                        {
                            entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.shopEntity.itemIds[i]);
                            entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.shopEntity.itemIds[i]);
                            this.priceDataLabels[i].text = string.Format(LocalizationManager.Get("EVENT_ITEM_NEED_INFO"), this.shopEntity.GetPrices(i), entity3.name);
                            this.priceIconLabels[i].Set(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrices(i), 0, 0, 0L, false, false);
                            this.eventItemIcons[i].SetItem(entity3.imageId, -1);
                            if (this.shopEntity.GetPrices(i) > entity4.num)
                            {
                                flag3 = true;
                            }
                        }
                        if (flag && flag3)
                        {
                            flag = false;
                            this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_EVENT_ITEM_NOT_ENOUGH_WARNING");
                        }
                        break;
                    }
                    entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.shopEntity.itemIds[0]);
                    entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.shopEntity.itemIds[0]);
                    this.priceInfo1.SetActive(true);
                    this.priceInfo2.SetActive(false);
                    this.priceDataLabel.text = string.Format(LocalizationManager.Get("EVENT_ITEM_NEED_INFO"), this.shopEntity.GetPrice(), entity3.name);
                    this.priceIconLabel.Set(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrice(), entity4.num, 0, 0L, false, false);
                    this.eventItemIcon.SetItem(entity3.imageId, -1);
                    if (flag && (this.shopEntity.GetPrice() > entity4.num))
                    {
                        flag = false;
                        this.warningLabel.text = string.Format(LocalizationManager.Get("SHOP_BUY_EVENT_ITEM_CONFIRM_WARNING"), entity3.name);
                    }
                    break;
                }
            }
            if (flag)
            {
                int buyItemNum = 0;
                int buyServantNum = 0;
                int buyServantEquipNum = 0;
                this.shopEntity.GetSum(out buyItemNum, out buyServantNum, out buyServantEquipNum);
                if ((buyServantNum + buyServantEquipNum) > 0)
                {
                    int length = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(entity2.userId).Length;
                    if (length >= BalanceConfig.PresentBoxMax)
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_PRESENT_BOX_FULL_WARNING");
                    }
                    else if (((length + buyServantNum) + buyServantEquipNum) > BalanceConfig.PresentBoxMax)
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_PRESENT_BOX_OVER_WARNING");
                    }
                }
                if (flag && (buyItemNum > 0))
                {
                    int num7 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getSum(NetworkManager.UserId);
                    if (num7 >= BalanceConfig.UserItemMax)
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_FULL_WARNING");
                    }
                    else if ((num7 + buyItemNum) > BalanceConfig.UserItemMax)
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_OVER_WARNING");
                    }
                }
            }
            this.decideButton.SetState(!flag ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
            this.itemIcon.SetPurchase((Purchase.Type) entity.purchaseType, entity.targetId, entity.imageId);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void Open(StoneShopEntity entity, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.stoneShopEntity = entity;
            this.shopEntity = null;
            base.gameObject.SetActive(true);
            this.nameLabel.SetCrossNarrowText(entity.name);
            this.messageLabel.text = "[000000]" + entity.detail;
            this.priceDataLabel.text = string.Format(LocalizationManager.Get("STONE_NEED_INFO"), entity.GetPrice());
            this.priceIconLabel.Set(this.stoneShopEntity.GetPriceIcon(), this.stoneShopEntity.GetPrice(), 0, 0, 0L, false, false);
            this.itemIcon.SetItemImage(ImageItem.Id.STONE);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void returnWarning(bool isDecide)
    {
        this.noTitleDialog.Close();
        if (isDecide)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.Callback(1);
        }
    }

    public delegate void CallbackFunc(int result);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

