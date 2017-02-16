using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UserGameActRecoverMenu : BaseMonoBehaviour
{
    [SerializeField]
    protected UserGameActRecoverConfirmMenu recoverConfirmMenu;
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
            this.recoverConfirmMenu.Init();
            base.gameObject.SetActive(false);
            this.stoneShopEntity = null;
            this.refreshCallbackFunc = null;
            this.state = State.INIT;
        }
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
            this.recoverConfirmMenu.Close(new System.Action(this.EndCloseBuyStoneConfirm));
        }
    }

    protected void EndCloseBuyStoneConfirm()
    {
        this.Callback(Result.CANCEL);
    }

    protected void EndCloseMaxFrameConfirm()
    {
        this.Callback(Result.CANCEL);
    }

    protected void EndCloseNoShopConfirm()
    {
        this.Callback(Result.ERROR);
    }

    protected void EndCloseRecoverUserGameActConfirm()
    {
        this.Callback(Result.CANCEL);
    }

    protected void EndCloseRecoverUserGameActResultConfirm()
    {
        this.Callback(Result.RECOVER);
    }

    protected void EndMaxFrameConfirm(bool isDecide)
    {
        this.recoverConfirmMenu.Close(new System.Action(this.EndCloseMaxFrameConfirm));
    }

    protected void EndNoShopConfirm(bool isDecide)
    {
        this.recoverConfirmMenu.Close(new System.Action(this.EndCloseNoShopConfirm));
    }

    protected void EndRecoverUserGameActConfirm(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.REQUEST_BUY_FRAME;
            this.RequestUserGameActRecover();
        }
        else
        {
            this.recoverConfirmMenu.Close(new System.Action(this.EndCloseRecoverUserGameActConfirm));
        }
    }

    protected void EndRecoverUserGameActResultConfirm(bool isDecide)
    {
        this.recoverConfirmMenu.Close(new System.Action(this.EndCloseRecoverUserGameActResultConfirm));
    }

    protected void EndRequestUserGameActRecover(string result)
    {
        this.state = State.RESULT_BUY_FRAME;
        this.recoverConfirmMenu.Init();
        this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.RECOVER, this.stoneShopEntity, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndRecoverUserGameActResultConfirm));
        if (this.refreshCallbackFunc != null)
        {
            this.refreshCallbackFunc();
        }
    }

    private void OnMoveEnd()
    {
    }

    public void Open(CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.refreshCallbackFunc = refreshCallback;
            base.gameObject.SetActive(true);
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            StoneShopEntity[] enableEntitiyList = (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STONE_SHOP) as StoneShopMaster).GetEnableEntitiyList(StoneShopEffect.Kind.ACT_RECOVER);
            if (enableEntitiyList.Length <= 0)
            {
                this.state = State.INPUT_NO_SHOP_CONFIRM;
                this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.NO_INFO, null, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndNoShopConfirm));
            }
            else
            {
                this.stoneShopEntity = enableEntitiyList[0];
                if (!entity.IsNeedRecoverAct())
                {
                    this.state = State.INPUT_MAX_FRAME_CONFIRM;
                    this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.MAX_ACT, null, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndMaxFrameConfirm));
                }
                else if (entity.stone < this.stoneShopEntity.price)
                {
                    this.state = State.INPUT_BUY_STONE_CONFIRM;
                    this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.STONE, this.stoneShopEntity, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndBuyStoneConfirm));
                }
                else
                {
                    this.state = State.INPUT_BUY_FRAME_CONFIRM;
                    this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.USER_GAME_ACT, this.stoneShopEntity, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndRecoverUserGameActConfirm));
                }
            }
            base.Invoke("OnMoveEnd", 0.1f);
        }
    }

    protected void RequestUserGameActRecover()
    {
        if (this.stoneShopEntity != null)
        {
            NetworkManager.getRequest<PurchaseByStoneRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestUserGameActRecover)).beginRequest(this.stoneShopEntity.id, 1);
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
                this.recoverConfirmMenu.Init();
                this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.STONE, this.stoneShopEntity, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndBuyStoneConfirm));
                break;

            case StonePurchaseMenu.Result.ERROR:
                this.Callback(Result.ERROR);
                break;

            case StonePurchaseMenu.Result.PURCHASE:
                this.state = State.INPUT_BUY_FRAME_CONFIRM;
                this.recoverConfirmMenu.Init();
                this.recoverConfirmMenu.Open(UserGameActRecoverConfirmMenu.Kind.USER_GAME_ACT, this.stoneShopEntity, new UserGameActRecoverConfirmMenu.CallbackFunc(this.EndRecoverUserGameActConfirm));
                break;
        }
    }

    public delegate void CallbackFunc(UserGameActRecoverMenu.Result result);

    public enum Result
    {
        CANCEL,
        ERROR,
        RECOVER
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

