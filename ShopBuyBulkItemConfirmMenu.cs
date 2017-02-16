using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopBuyBulkItemConfirmMenu : BaseDialog
{
    private const int BuyCancel = 0;
    protected int buyCount;
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
    protected GameObject exchangeBase;
    [SerializeField]
    protected UILabel exchangeDestination;
    [SerializeField]
    protected UILabel exchangeDestinationCount;
    [SerializeField]
    protected UILabel exchangeDestinationCountKind;
    [SerializeField]
    protected UILabel exchangeDestinationItemName;
    [SerializeField]
    protected UILabel exchangeOrigin;
    [SerializeField]
    protected UILabel[] exchangeOriginCountKinds;
    [SerializeField]
    protected UILabel[] exchangeOriginCounts;
    [SerializeField]
    protected GameObject exchangeOriginDispObject1;
    [SerializeField]
    protected GameObject exchangeOriginDispObject2;
    [SerializeField]
    protected UILabel[] exchangeOriginItemNames;
    protected int itemCount;
    [SerializeField]
    protected GameObject itemInfo;
    [SerializeField]
    protected UISprite itemMaskeSprite;
    [SerializeField]
    public UISliderWithButton itemSlider;
    [SerializeField]
    protected UICommonButton minusButton;
    [SerializeField]
    protected UILabel numTextLabel;
    [SerializeField]
    protected UILabel numTitleLabel;
    [SerializeField]
    protected UICommonButton plusButton;
    [SerializeField]
    protected UIIconLabel priceIconLabel;
    [SerializeField]
    protected UIIconLabel[] priceIconLabels;
    [SerializeField]
    protected GameObject priceInfo1;
    [SerializeField]
    protected GameObject priceInfo2;
    protected ShopEntity shopEntity;
    protected ShopBuyItemListViewItem shopItem;
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
            if ((this.shopEntity != null) && !this.shopEntity.IsEnable(0L))
            {
                this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_CONFIRM_PERIOD_WARNING");
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.decideButton.SetState(UICommonButtonColor.State.Disabled, false);
                this.warningLabel.gameObject.SetActive(true);
                this.exchangeBase.SetActive(false);
            }
            else
            {
                this.state = State.SELECTED;
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                this.Callback(this.buyCount);
            }
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
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void Open(ShopEntity entity, ShopBuyItemListViewItem item, CallbackFunc callback)
    {
        int num;
        string str;
        if (this.state != State.INIT)
        {
            return;
        }
        this.callbackFunc = callback;
        this.shopEntity = entity;
        this.shopItem = item;
        this.stoneShopEntity = null;
        base.gameObject.SetActive(true);
        this.buyCount = 1;
        this.itemCount = this.shopEntity.GetItemCount();
        if (this.itemCount == 1)
        {
            this.exchangeOriginDispObject2.SetActive(false);
        }
        else
        {
            this.exchangeOriginDispObject2.SetActive(true);
        }
        this.exchangeOriginDispObject1.SetActive(true);
        this.exchangeOrigin.text = LocalizationManager.Get("SHOP_BULK_WINDOW_ORIGIN");
        this.exchangeOriginCountKinds[0].text = LocalizationManager.Get("SHOP_BULK_WINDOW_ORIGIN_KIND");
        if (this.itemCount > 1)
        {
            this.exchangeOriginCountKinds[1].text = LocalizationManager.Get("SHOP_BULK_WINDOW_ORIGIN_KIND");
        }
        this.exchangeDestination.text = LocalizationManager.Get("SHOP_BULK_WINDOW_DESTINATION");
        this.exchangeDestinationCountKind.text = LocalizationManager.Get("SHOP_BULK_WINDOW_DESTINATION_KIND");
        this.UpdateCountValue(this.buyCount);
        UserGameEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.itemInfo.GetComponent<ShopBuyItemListViewItemDraw>().SetItem(item, ShopBuyItemListViewItemDraw.DispMode.VALID);
        this.exchangeDestinationItemName.text = this.shopEntity.name;
        this.warningLabel.text = string.Empty;
        bool flag = true;
        bool flag2 = this.shopEntity.IsPreparation(out str);
        if (flag2)
        {
            flag = false;
            this.warningLabel.text = str;
            this.itemSlider.grayMode();
        }
        else
        {
            this.itemSlider.normalMode();
        }
        this.itemMaskeSprite.gameObject.SetActive(flag2);
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
            case PayType.Type.MANA:
                num = entity2.mana / this.shopEntity.GetPrice();
                if ((num > (this.shopEntity.limitNum - item.ToTalNum)) && (this.shopEntity.limitNum != 0))
                {
                    num = this.shopEntity.limitNum - item.ToTalNum;
                }
                this.itemSlider.init(num);
                this.exchangeOriginItemNames[0].text = LocalizationManager.Get("MANA_NAME");
                goto Label_05B8;

            case PayType.Type.EVENT_ITEM:
            {
                ItemEntity entity3;
                UserItemEntity entity4;
                int itemCount = this.shopEntity.GetItemCount();
                if (itemCount != 1)
                {
                    this.priceInfo1.SetActive(false);
                    this.priceInfo2.SetActive(true);
                    int[] numArray = new int[2];
                    for (int i = 0; i < itemCount; i++)
                    {
                        int index = (i != 0) ? 0 : 1;
                        entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.shopEntity.itemIds[i]);
                        entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.shopEntity.itemIds[i]);
                        this.priceIconLabels[i].Set(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrices(i), 0, 0, 0L, false, false);
                        this.eventItemIcons[i].SetItem(entity3.imageId, -1);
                        this.exchangeOriginItemNames[index].text = entity3.name;
                        numArray[i] = entity4.num / this.shopEntity.GetPrices(i);
                    }
                    if (numArray[0] > numArray[1])
                    {
                        num = numArray[1];
                    }
                    else
                    {
                        num = numArray[0];
                    }
                    if (num == 0)
                    {
                        flag = false;
                        this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_EVENT_ITEM_NOT_ENOUGH_WARNING");
                    }
                    else if ((num > (this.shopEntity.limitNum - this.shopItem.ToTalNum)) && (this.shopEntity.limitNum != 0))
                    {
                        num = this.shopEntity.limitNum - this.shopItem.ToTalNum;
                    }
                    break;
                }
                entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.shopEntity.itemIds[0]);
                entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, this.shopEntity.itemIds[0]);
                this.priceInfo1.SetActive(true);
                this.priceInfo2.SetActive(false);
                this.priceIconLabel.Set(this.shopEntity.GetPriceIcon(), this.shopEntity.GetPrice(), entity4.num, 0, 0L, false, false);
                this.eventItemIcon.SetItem(entity3.imageId, -1);
                this.exchangeOriginItemNames[0].text = entity3.name;
                num = entity4.num / this.shopEntity.GetPrices(0);
                if ((num > (this.shopEntity.limitNum - item.ToTalNum)) && (this.shopEntity.limitNum != 0))
                {
                    num = this.shopEntity.limitNum - item.ToTalNum;
                }
                break;
            }
            default:
                goto Label_05B8;
        }
        this.itemSlider.init(num);
    Label_05B8:
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
                int num9 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getSum(NetworkManager.UserId);
                if (num9 >= BalanceConfig.UserItemMax)
                {
                    flag = false;
                    this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_FULL_WARNING");
                }
                else if ((num9 + buyItemNum) > BalanceConfig.UserItemMax)
                {
                    flag = false;
                    this.warningLabel.text = LocalizationManager.Get("SHOP_BUY_ITEM_OVER_WARNING");
                }
            }
        }
        this.decideButton.SetState(!flag ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
        if (this.warningLabel.text != string.Empty)
        {
            this.warningLabel.gameObject.SetActive(true);
            this.exchangeBase.SetActive(false);
        }
        else
        {
            this.warningLabel.gameObject.SetActive(false);
            this.exchangeBase.SetActive(true);
        }
        this.state = State.OPEN;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void sliderValueChange()
    {
        this.buyCount = this.itemSlider.sliderValueChange();
        this.UpdateCountValue(this.buyCount);
    }

    private void UpdateCountValue(int count)
    {
        if (this.itemCount > 1)
        {
            this.exchangeOriginCounts[0].text = LocalizationManager.GetNumberFormat((int) (count * this.shopEntity.GetPrices(1)));
            this.exchangeOriginCounts[1].text = LocalizationManager.GetNumberFormat((int) (count * this.shopEntity.GetPrices(0)));
        }
        else
        {
            this.exchangeOriginCounts[0].text = LocalizationManager.GetNumberFormat((int) (count * this.shopEntity.GetPrices(0)));
        }
        this.exchangeDestinationCount.text = LocalizationManager.GetNumberFormat((int) (count * this.shopEntity.setNum));
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

