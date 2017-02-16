using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class StonePurchaseMenu : BaseDialog
{
    [SerializeField]
    protected AgeVerificationMenu ageVerificationMenu;
    protected int ageVerificationResult;
    [SerializeField]
    protected GameObject buyItemMask;
    protected System.Action closeCallbackFunc;
    protected int cumulativeAmount;
    [SerializeField]
    protected StonePurchaseListViewManager ItemListViewManager;
    protected System.Action refreshCallbackFunc;
    protected int selectItemNum;
    [SerializeField]
    protected SpendLimitMenu spendLimitMenu;
    protected State state;
    [SerializeField]
    protected UILabel stoneDataLabel;
    [SerializeField]
    protected StonePurchaseNotificationMenu stonePurchaseNotificationMenu;
    [SerializeField]
    protected ErrorDialog warningDialog;

    protected event CallbackFunc callbackFunc;

    public void BackBuyBankItem()
    {
        if (this.state == State.INPUT_BUY_STONE)
        {
            this.state = State.QUIT_BUY_STONE;
            base.Invoke("OnMoveEnd", 0.1f);
        }
    }

    public void BackBuyBankItemConfirm()
    {
        if (this.state == State.INPUT_BUY_STONE_CONFIRM)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            this.buyItemMask.SetActive(false);
            this.ItemListViewManager.SetMode(StonePurchaseListViewManager.InitMode.INPUT, new StonePurchaseListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_STONE;
        }
    }

    protected void Callback(Result result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(result);
        }
    }

    public void Close(System.Action callback)
    {
        if (this.state != State.INIT)
        {
            this.closeCallbackFunc = callback;
            this.state = State.QUIT_BUY_STONE;
            base.Close(new System.Action(this.OnMoveEnd));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    protected void EndBuyBankItemNotificationCancel()
    {
        this.stonePurchaseNotificationMenu.Close();
        this.BackBuyBankItemConfirm();
    }

    protected void EndBuyBankItemNotificationSuccess()
    {
        this.stonePurchaseNotificationMenu.Close();
        this.Callback(Result.PURCHASE);
    }

    protected void EndBuyBankItemNotificationSuspend()
    {
        this.stonePurchaseNotificationMenu.Close();
        this.Callback(Result.ERROR);
    }

    protected void EndBuyBankItemNotificationWait(bool isDecide)
    {
        if (this.state == State.INPUT_BUY_STONE_WARNING)
        {
            this.warningDialog.Close();
            this.Callback(Result.WAIT);
        }
    }

    protected void EndSpendLimit()
    {
        this.spendLimitMenu.Close();
        this.ItemListViewManager.SetMode(StonePurchaseListViewManager.InitMode.INPUT, new StonePurchaseListViewManager.CallbackFunc(this.OnSelectBuyItem));
        this.state = State.INPUT_BUY_STONE;
    }

    public void Init()
    {
        this.callbackFunc = null;
        this.refreshCallbackFunc = null;
        this.stoneDataLabel.text = string.Empty;
        this.ItemListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickBack()
    {
        if (this.state == State.INPUT_BUY_STONE)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(Result.CANCEL);
        }
    }

    public void OnClickExplanation()
    {
        if (this.state == State.INPUT_BUY_STONE)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            WebViewManager.OpenView(LocalizationManager.Get("WEB_VIEW_TITLE_STONE_PURCHASE"), "spdeal/index.html", new System.Action(this.OnEndWebView));
        }
    }

    public void OnEndWebView()
    {
    }

    protected void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_BUY_STONE:
                this.state = State.INPUT_BUY_STONE;
                this.ItemListViewManager.SetMode(StonePurchaseListViewManager.InitMode.INPUT, new StonePurchaseListViewManager.CallbackFunc(this.OnSelectBuyItem));
                return;

            case State.QUIT_BUY_STONE:
                this.Init();
                if (this.closeCallbackFunc != null)
                {
                    System.Action closeCallbackFunc = this.closeCallbackFunc;
                    this.closeCallbackFunc = null;
                    closeCallbackFunc();
                }
                return;

            case State.INIT_AGE_VERIFICATION:
                this.state = State.INPUT_AGE_VERIFICATION;
                break;

            case State.INPUT_AGE_VERIFICATION:
                this.OnSelectAgeVerification(this.ageVerificationResult);
                break;
        }
    }

    protected void OnSelectAgeVerification(int result)
    {
        if (this.state == State.INIT_AGE_VERIFICATION)
        {
            this.state = State.INPUT_AGE_VERIFICATION;
            this.ageVerificationResult = result;
        }
        else if (this.state == State.INPUT_AGE_VERIFICATION)
        {
            this.state = State.INPUT_BUY_STONE;
            this.ageVerificationMenu.Close();
            if (result > 0)
            {
                this.ItemListViewManager.SetMode(StonePurchaseListViewManager.InitMode.INPUT, new StonePurchaseListViewManager.CallbackFunc(this.OnSelectBuyItem));
            }
            else
            {
                this.Callback(Result.CANCEL);
            }
        }
    }

    protected void OnSelectBuyItem(int n)
    {
        if (this.state == State.INPUT_BUY_STONE)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.selectItemNum = n;
            this.SelectBuyBankItemConfirm();
        }
    }

    protected void OnSelectWarning(bool isDecide)
    {
        if (this.state == State.INPUT_BUY_STONE_WARNING)
        {
            this.warningDialog.Close();
            this.Callback(Result.WAIT);
        }
    }

    public void Open(CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.refreshCallbackFunc = refreshCallback;
            this.ItemListViewManager.IsInput = false;
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            this.stoneDataLabel.text = LocalizationManager.GetNumberFormat(entity.stone);
            this.state = State.INIT_BUY_STONE;
            this.ItemListViewManager.CreateList(StonePurchaseListViewManager.Kind.BANK);
            AccountingManager.SetEnableStore(true);
            AgeVerificationMenu.Concent(3);
            if (SingletonMonoBehaviour<AccountingManager>.Instance.CheckPaymentStore())
            {
                this.state = State.INPUT_BUY_STONE_WARNING;
                this.warningDialog.Open(null, LocalizationManager.Get("STONE_PURCHASE_BUSY_STORE"), new ErrorDialog.ClickDelegate(this.OnSelectWarning));
            }
            else if (AgeVerificationMenu.IsConcent())
            {
                this.state = State.INPUT_BUY_STONE;
                this.ItemListViewManager.SetMode(StonePurchaseListViewManager.InitMode.INPUT, new StonePurchaseListViewManager.CallbackFunc(this.OnSelectBuyItem));
            }
            else
            {
                this.state = State.INIT_AGE_VERIFICATION;
                this.ageVerificationMenu.Open(new AgeVerificationMenu.CallbackFunc(this.OnSelectAgeVerification));
            }
            base.Open(new System.Action(this.OnMoveEnd), true);
        }
    }

    public void SelectBuyBankItemConfirm()
    {
        if (this.state == State.INPUT_BUY_STONE)
        {
            StonePurchaseListViewItem item = this.ItemListViewManager.GetItem(this.selectItemNum);
            this.cumulativeAmount = AgeVerificationMenu.GetCumulativeAmount() + item.BankShop.GetPrice();
            if (this.spendLimitMenu.IsLimitOver(this.cumulativeAmount))
            {
                this.state = State.INPUT_SPEND_LIMIT;
                this.spendLimitMenu.Open(this.cumulativeAmount - item.BankShop.GetPrice(), new SpendLimitMenu.CallbackFunc(this.EndSpendLimit));
            }
            else
            {
                this.state = State.INPUT_BUY_STONE_CONFIRM;
                SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
                this.buyItemMask.SetActive(true);
                TweenAlpha.Begin(this.buyItemMask, 0.2f, 1f);
                SingletonMonoBehaviour<AccountingManager>.Instance.StartPaymentStore(item.BankShop, this.cumulativeAmount, new AccountingManager.ResultCallbackfunc(this.SelectedBuyBankItemConfirm));
            }
        }
    }

    protected void SelectedBuyBankItemConfirm(AccountingManager.Result result, int cumulativeAmount)
    {
        TweenAlpha.Begin(this.buyItemMask, 0.2f, 0f);
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
        switch (result)
        {
            case AccountingManager.Result.SUCCESS:
                AgeVerificationMenu.SaveCumulativeAmount(cumulativeAmount);
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.SUCCESS, new System.Action(this.EndBuyBankItemNotificationSuccess));
                break;

            case AccountingManager.Result.WAIT:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.state = State.INPUT_BUY_STONE_WARNING;
                this.warningDialog.Open(null, LocalizationManager.Get("STONE_PURCHASE_RESULT_WAIT"), new ErrorDialog.ClickDelegate(this.EndBuyBankItemNotificationWait));
                break;

            case AccountingManager.Result.CANCEL:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.CANCEL, new System.Action(this.EndBuyBankItemNotificationCancel));
                break;

            case AccountingManager.Result.SUSPEND:
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.SUSPEND, new System.Action(this.EndBuyBankItemNotificationSuspend));
                break;

            default:
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.FAIL, new System.Action(this.EndBuyBankItemNotificationCancel));
                break;
        }
        if (this.refreshCallbackFunc != null)
        {
            this.refreshCallbackFunc();
        }
    }

    public delegate void CallbackFunc(StonePurchaseMenu.Result result);

    public enum Result
    {
        CANCEL,
        ERROR,
        PURCHASE,
        WAIT
    }

    protected enum State
    {
        INIT,
        INIT_BUY_STONE,
        INPUT_BUY_STONE,
        QUIT_BUY_STONE,
        INIT_BUY_STONE_CONFIRM,
        INPUT_BUY_STONE_CONFIRM,
        QUIT_BUY_STONE_CONFIRM,
        INIT_AGE_VERIFICATION,
        INIT_AGE_VERIFICATION2,
        INPUT_AGE_VERIFICATION,
        INPUT_SPEND_LIMIT,
        INIT_RECIVE_BUY_STONE_CONFIRM,
        INPUT_RECIVE_BUY_STONE_CONFIRM,
        QUIT_RECIVE_BUY_STONE_CONFIRM,
        INPUT_BUY_STONE_WARNING
    }
}

