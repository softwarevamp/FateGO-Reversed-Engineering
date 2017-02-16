using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantEquipFramePurchaseMenu : BaseMonoBehaviour
{
    [SerializeField]
    protected ServantEquipFramePurchaseConfirmMenu purchaseConfirmMenu;
    protected System.Action refreshCallbackFunc;
    protected State state;
    protected StoneShopEntity stoneShopEntity;

    protected event CallbackFunc callbackFunc;

    protected void Callback(Result result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(result);
        }
    }

    public void Close()
    {
        if (this.state != State.INIT)
        {
            this.purchaseConfirmMenu.Init();
            base.gameObject.SetActive(false);
            this.stoneShopEntity = null;
            this.refreshCallbackFunc = null;
            this.state = State.INIT;
        }
    }

    protected void EndBuyFrameConfirm(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.REQUEST_BUY_FRAME;
            this.RequestServantEquipFramePurchase();
        }
        else
        {
            this.purchaseConfirmMenu.Close(new System.Action(this.EndCloseBuyFrameConfirmCancel));
        }
    }

    protected void EndBuyFrameResultConfirm(bool isDecide)
    {
        this.purchaseConfirmMenu.Close(new System.Action(this.EndCloseBuyFrameConfirmPurchase));
    }

    protected void EndBuyStoneConfirm(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.INPUT_BUY_STONE;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenStonePurchaseMenu(new StonePurchaseMenu.CallbackFunc(this.SelectedBuyBankItemConfirm), this.refreshCallbackFunc);
        }
        else
        {
            this.purchaseConfirmMenu.Close();
            this.Callback(Result.CANCEL);
        }
    }

    protected void EndCloseBuyFrameConfirmCancel()
    {
        this.Callback(Result.CANCEL);
    }

    protected void EndCloseBuyFrameConfirmPurchase()
    {
        this.Callback(Result.PURCHASE);
    }

    protected void EndMaxFrameConfirm(bool isDecide)
    {
        this.purchaseConfirmMenu.Close();
        this.Callback(Result.CANCEL);
    }

    protected void EndNoShopConfirm(bool isDecide)
    {
        this.purchaseConfirmMenu.Close();
        this.Callback(Result.ERROR);
    }

    protected void EndRequestServantEquipFramePurchase(string result)
    {
        this.state = State.RESULT_BUY_FRAME;
        this.purchaseConfirmMenu.Init();
        this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.PURCHASE, this.stoneShopEntity, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndBuyFrameResultConfirm));
        if (this.refreshCallbackFunc != null)
        {
            this.refreshCallbackFunc();
        }
    }

    public void Open(CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.refreshCallbackFunc = refreshCallback;
            base.gameObject.SetActive(true);
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            StoneShopEntity[] enableEntitiyList = (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STONE_SHOP) as StoneShopMaster).GetEnableEntitiyList(StoneShopEffect.Kind.EXTEND_SVT_EQUIP_MAX);
            if (enableEntitiyList.Length <= 0)
            {
                this.state = State.INPUT_NO_SHOP_CONFIRM;
                this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.NO_INFO, null, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndNoShopConfirm));
            }
            else
            {
                this.stoneShopEntity = enableEntitiyList[0];
                if (entity.svtEquipKeep >= BalanceConfig.ServantEquipFrameMax)
                {
                    this.state = State.INPUT_MAX_FRAME_CONFIRM;
                    this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.MAX_FRAME, null, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndMaxFrameConfirm));
                }
                else if (entity.stone < this.stoneShopEntity.price)
                {
                    this.state = State.INPUT_BUY_STONE_CONFIRM;
                    this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.STONE, this.stoneShopEntity, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndBuyStoneConfirm));
                }
                else
                {
                    this.state = State.INPUT_BUY_FRAME_CONFIRM;
                    this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.FRAME, this.stoneShopEntity, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndBuyFrameConfirm));
                }
            }
        }
    }

    public void RequestServantEquipFramePurchase()
    {
        if (this.stoneShopEntity != null)
        {
            NetworkManager.getRequest<PurchaseByStoneRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestServantEquipFramePurchase)).beginRequest(this.stoneShopEntity.id, 1);
        }
        else
        {
            this.Callback(Result.ERROR);
        }
    }

    protected void SelectedBuyBankItemConfirm(StonePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseStonePurchaseMenu();
        switch (result)
        {
            case StonePurchaseMenu.Result.CANCEL:
            case StonePurchaseMenu.Result.WAIT:
                this.state = State.INPUT_BUY_STONE_CONFIRM;
                this.purchaseConfirmMenu.Init();
                this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.STONE, this.stoneShopEntity, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndBuyStoneConfirm));
                break;

            case StonePurchaseMenu.Result.ERROR:
                this.Callback(Result.ERROR);
                break;

            case StonePurchaseMenu.Result.PURCHASE:
                this.state = State.INPUT_BUY_FRAME_CONFIRM;
                this.purchaseConfirmMenu.Init();
                this.purchaseConfirmMenu.Open(ServantEquipFramePurchaseConfirmMenu.Kind.FRAME, this.stoneShopEntity, new ServantEquipFramePurchaseConfirmMenu.CallbackFunc(this.EndBuyFrameConfirm));
                break;
        }
    }

    public delegate void CallbackFunc(ServantEquipFramePurchaseMenu.Result result);

    public enum Result
    {
        CANCEL,
        ERROR,
        PURCHASE
    }

    protected enum State
    {
        INIT,
        INPUT_NO_SHOP_CONFIRM,
        INPUT_MAX_FRAME_CONFIRM,
        INPUT_BUY_FRAME_CONFIRM,
        INPUT_BUY_STONE_CONFIRM,
        INPUT_BUY_STONE,
        REQUEST_BUY_FRAME,
        RESULT_BUY_FRAME
    }
}

