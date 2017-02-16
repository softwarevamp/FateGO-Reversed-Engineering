using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AccountingManager : SingletonMonoBehaviour<AccountingManager>
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map4;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map5;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map6;
    protected ResultCallbackfunc callbackFunc;
    protected Result callbackResult;
    public static int cumulativeAmount;
    protected static int initializeResult;
    protected static long initializeStartTime;
    protected static bool isEnableStore;
    protected bool IsEnableSuspendDialog;
    protected static bool isMoveStore;
    protected static bool isRecoverStore = true;
    protected static string paymentBaseReceipt = string.Empty;
    protected static string paymentProductId = string.Empty;
    protected static string paymentReceipt = string.Empty;
    protected static string paymentState = string.Empty;
    protected static long paymentTime;

    public bool CheckPaymentStore()
    {
        string paymentState = AccountingManager.paymentState;
        if (paymentState != null)
        {
            int num;
            if (<>f__switch$map4 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(5) {
                    { 
                        "Consume",
                        0
                    },
                    { 
                        "Receipt",
                        0
                    },
                    { 
                        "SendReceipt",
                        0
                    },
                    { 
                        "Deferred",
                        1
                    },
                    { 
                        "Payment",
                        1
                    }
                };
                <>f__switch$map4 = dictionary;
            }
            if (<>f__switch$map4.TryGetValue(paymentState, out num))
            {
                if (num == 0)
                {
                    Debug.Log("CheckPaymentStore [" + AccountingManager.paymentState + "]");
                    return true;
                }
                if (num == 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void ClearAll()
    {
        string paymentHistoryPath = GetPaymentHistoryPath();
        if (Directory.Exists(paymentHistoryPath))
        {
            Directory.Delete(paymentHistoryPath, true);
        }
        ClearPayment();
    }

    public static void ClearPayment()
    {
        paymentState = string.Empty;
        paymentTime = 0L;
        paymentProductId = string.Empty;
        paymentBaseReceipt = string.Empty;
        paymentReceipt = string.Empty;
        cumulativeAmount = 0;
        string paymentFileName = GetPaymentFileName();
        if (File.Exists(paymentFileName))
        {
            File.Delete(paymentFileName);
        }
    }

    protected bool ContinueReceiptProc()
    {
        string receiptJson = ChargeServicePluginScript.GetReceiptJson();
        if (!this.CheckPaymentStore())
        {
            switch (receiptJson)
            {
                case null:
                case string.Empty:
                    return false;
            }
            string jsonstr = "{ \"receipts\": " + receiptJson + " }";
            Debug.Log("ContinueReceiptProc :  " + receiptJson);
            List<object> list = (List<object>) JsonManager.getDictionary(jsonstr)["receipts"];
            if (list.Count > 0)
            {
                string receipt = (string) list[0];
                Debug.Log("ContinueReceiptProc :  count: " + list.Count);
                paymentBaseReceipt = receipt;
                paymentState = "Receipt";
                paymentTime = NetworkManager.getTime();
                paymentReceipt = string.Empty;
                this.WritePayment();
                ChargeServicePluginScript.DelReceipt(receipt);
                this.SendBaseReceipt();
                return true;
            }
        }
        return false;
    }

    public void DeletePayment()
    {
        if (!ManagerConfig.UseMock)
        {
            Debug.Log("Delete(Clear)Payment!!!");
            ClearPayment();
        }
    }

    protected void EndPaymentCheckErrorRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            base.Invoke("PaymentCheck", 1f);
        }
        else
        {
            RecoverStatusReset();
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void EndPaymentConsumeErrorRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            base.Invoke("PaymentConsume", 1f);
        }
        else
        {
            RecoverStatusReset();
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void EndPaymentUnknownErrorRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            base.Invoke("PaymentCheck", 1f);
        }
        else
        {
            RecoverStatusReset();
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void EndPurchaseSuspendDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        ChargeServicePluginScript.RestoreCompletedTransactions();
        base.Invoke("OnSuspendPaymentStore", 3f);
    }

    protected static string GetHistoryFileName(long time) => 
        (GetPaymentHistoryPath() + $"/payment{time:D20}.dat");

    protected static string GetHistoryFileName(string name) => 
        (GetPaymentHistoryPath() + "/" + name);

    public int GetInitializeResult() => 
        initializeResult;

    public static string GetOldPaymentFileName() => 
        (Application.temporaryCachePath + "/paymentsave.dat");

    protected static string GetPaymentFileName() => 
        (Application.persistentDataPath + "/paymentsave.dat");

    public string[] GetPaymentHistoryList()
    {
        FileInfo[] files = new DirectoryInfo(GetPaymentHistoryPath()).GetFiles();
        List<string> list = new List<string>();
        foreach (FileInfo info2 in files)
        {
            string name = info2.Name;
            string extension = info2.Extension;
            char ch = name[0];
            if (!ch.Equals('.') && !extension.Equals(".meta"))
            {
                list.Add(name);
            }
        }
        return list.ToArray();
    }

    protected static string GetPaymentHistoryPath() => 
        (Application.persistentDataPath + "/PaymentHistorys");

    public string[] GetProductList()
    {
        Debug.Log("AccountingManager:GetProductList");
        string[] productList = null;
        if (ManagerConfig.UseAppServer)
        {
            productList = ChargeServicePluginScript.GetProductList();
        }
        Debug.Log("AccountingManager:GetProductList result [" + productList + "]");
        return productList;
    }

    public void HistoryPayment(string state)
    {
    }

    public void Initialize()
    {
        Debug.Log("AccountingManager:Initialize start");
        this.callbackFunc = null;
        this.callbackResult = Result.NONE;
        string paymentHistoryPath = GetPaymentHistoryPath();
        if (!Directory.Exists(paymentHistoryPath))
        {
            Directory.CreateDirectory(paymentHistoryPath);
        }
        initializeStartTime = NetworkManager.getTime();
        if (ManagerConfig.UseAppServer)
        {
            BankShopEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BankShopMaster>(DataNameKind.Kind.BANK_SHOP).GetEnableEntitiyList();
            List<string> list = new List<string>();
            for (int i = 0; i < enableEntitiyList.Length; i++)
            {
                list.Add(enableEntitiyList[i].googleShopId);
            }
            ChargeServicePluginScript.InitializeProduct(NetworkManager.GetApiCode(), list.ToArray());
        }
        Debug.Log("AccountingManager:Initialize end");
    }

    public bool IsBusyInitialize()
    {
        if (initializeResult == 0)
        {
            if (ManagerConfig.UseAppServer)
            {
                if ((NetworkManager.getTime() - initializeStartTime) > ManagerConfig.ACCOUNTING_INITIALIZE_TIMEOUT)
                {
                    initializeResult = -10000;
                    return false;
                }
                if (ChargeServicePluginScript.IsBusyInitializeProduct())
                {
                    return true;
                }
            }
            initializeResult = ChargeServicePluginScript.GetInitializeResult();
        }
        return false;
    }

    public void LogPayment()
    {
        Debug.Log("AccountingManager:LogPayment " + paymentState);
        Debug.Log("    product [" + paymentProductId + "]");
        Debug.Log("    base receipt [" + paymentBaseReceipt + "]");
        Debug.Log("    receipt [" + paymentReceipt + "]");
    }

    protected void OnAppleServiceUnavailable()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("PURCHASE_STONE_APPLE_SERVICE_UNAVAILABLE"), new ErrorDialog.ClickDelegate(this.OnClickUnavailableDialog), false);
    }

    protected void OnApplicationPause(bool isPause)
    {
    }

    public void OnCancelPaymentStore()
    {
        this.DeletePayment();
        if (this.callbackFunc != null)
        {
            ResultCallbackfunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc(Result.CANCEL, 0);
        }
    }

    protected void OnClickUnavailableDialog(bool isRetry)
    {
        if (isRetry)
        {
            this.SendBaseReceipt();
        }
        else
        {
            this.callbackResult = Result.SUSPEND;
            if (this.callbackFunc != null)
            {
                ResultCallbackfunc callbackFunc = this.callbackFunc;
                this.callbackFunc = null;
                callbackFunc(this.callbackResult, cumulativeAmount);
            }
        }
    }

    protected void OnEndPaymentStore(string result)
    {
        this.HistoryPayment(result);
        if (this.callbackFunc != null)
        {
            ResultCallbackfunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc(this.callbackResult, cumulativeAmount);
        }
        else
        {
            MonoBehaviour.print(string.Concat(new object[] { "AccountingManager:OnEndPaymentStore callback null menu open ", this.callbackResult, " ", result }));
            SingletonMonoBehaviour<CommonUI>.Instance.OpenStonePurchaseReciveMenu(this.callbackResult, cumulativeAmount, result);
            this.DeletePayment();
        }
    }

    protected void OnEndSendReceiptData(string result)
    {
        Debug.Log("AccountingManager:OnEndSendReceiptData [" + result + "]");
        MonoBehaviour.print("!!!!!!!!!!!       " + result);
        MonoBehaviour.print("@@@@@@@@@@    " + paymentProductId);
        if (!isMoveStore)
        {
            isMoveStore = true;
            ChargeServicePluginScript.Resume();
        }
        string key = result;
        if (key != null)
        {
            int num;
            if (<>f__switch$map6 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                    { 
                        string.Empty,
                        0
                    },
                    { 
                        "receipt_error",
                        1
                    },
                    { 
                        "apple_server_error",
                        2
                    }
                };
                <>f__switch$map6 = dictionary;
            }
            if (<>f__switch$map6.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        if (paymentProductId != string.Empty)
                        {
                            this.SendPurchaseAction(paymentProductId);
                        }
                        this.callbackResult = Result.SUCCESS;
                        this.OnEndPaymentStore("PaymentOk");
                        return;

                    case 1:
                        this.callbackResult = Result.ERROR;
                        this.OnEndPaymentStore("ReceiptError");
                        return;

                    case 2:
                        this.OnAppleServiceUnavailable();
                        return;
                }
            }
        }
        this.callbackResult = Result.ERROR;
        this.OnEndPaymentStore("Error[" + result + "]");
    }

    protected void OnPaymentCancel(string result)
    {
        Debug.Log("AccountingManager:OnPaymentCancel result [" + result + "]");
        this.OnCancelPaymentStore();
    }

    protected void OnPaymentCheckResult(string result)
    {
        Debug.Log("AccountingManager::OnPaymentCheckResult result [" + result + "]");
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
        if (result != null)
        {
            paymentBaseReceipt = string.Empty;
            paymentReceipt = result;
            paymentState = "Consume";
            this.WritePayment();
            this.PaymentConsume();
        }
        else
        {
            this.OnCancelPaymentStore();
        }
    }

    protected void OnPaymentCheckResultError(string result)
    {
        Debug.Log("AccountingManager::OnPaymentCheckResultError result [" + result + "]");
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
        if (ManagerConfig.UseDebugCommand)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Accounting error for debug", "OnPaymentCheckResultError (" + result + ")", null, false);
        }
        ChargeServicePluginScript.Response response = (ChargeServicePluginScript.Response) int.Parse(result);
        if (response != ChargeServicePluginScript.Response.IABHELPER_VERIFICATION_FAILED)
        {
            if (response != ChargeServicePluginScript.Response.BILLING_RESPONSE_RESULT_OK)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE"), new ErrorDialog.ClickDelegate(this.EndPaymentCheckErrorRetryDialog), false);
            }
            else
            {
                this.OnCancelPaymentStore();
            }
        }
        else
        {
            this.PaymentItemAlreadyOwnedNoData();
        }
    }

    protected void OnPaymentConsume(string result)
    {
        Debug.Log("AccountingManager::OnPaymentConsume result [" + result + "]");
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
        this.SendReceiptData();
    }

    protected void OnPaymentConsumeError(string result)
    {
        Debug.Log("AccountingManager::OnPaymentConsumeError result [" + result + "]");
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
        if (ManagerConfig.UseDebugCommand)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Accounting error for debug", "OnPaymentConsumeError (" + result + ")", null, false);
        }
        ChargeServicePluginScript.Response response = (ChargeServicePluginScript.Response) int.Parse(result);
        if (response != ChargeServicePluginScript.Response.IABHELPER_VERIFICATION_FAILED)
        {
            if (response != ChargeServicePluginScript.Response.BILLING_RESPONSE_RESULT_OK)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE"), new ErrorDialog.ClickDelegate(this.EndPaymentConsumeErrorRetryDialog), false);
            }
            else
            {
                this.SendReceiptData();
            }
        }
        else
        {
            this.SendReceiptData();
        }
    }

    protected void OnPaymentItemAlreadyOwned(string result)
    {
        Debug.Log("AccountingManager::OnPaymentItemAlreadyOwned result [" + result + "]");
        this.PaymentCheck();
    }

    protected void OnPaymentItemOwned(string result)
    {
        Debug.Log("AccountingManager::OnPaymentItemOwned result [" + result + "]");
        paymentBaseReceipt = string.Empty;
        paymentReceipt = ChargeServicePluginScript.GetReceiptData();
        Debug.Log("    paymentReceipt [" + paymentReceipt + "]");
        paymentState = "Consume";
        this.WritePayment();
        this.PaymentConsume();
    }

    protected void OnPaymentPurchased(string result)
    {
        if (this.IsEnableSuspendDialog)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        }
        this.IsEnableSuspendDialog = false;
        Debug.Log("AccountingManager:OnPaymentPurchased result [" + result + "]");
        paymentReceipt = ChargeServicePluginScript.GetReceiptData();
        Debug.Log("OnPaymentPurchased : json : " + ChargeServicePluginScript.GetReceiptJson());
        Debug.Log("    paymentReceipt [" + paymentReceipt + "]");
        paymentBaseReceipt = string.Empty;
        this.SendReceiptData();
        ChargeServicePluginScript.ClearPurchasedTransactionRemain();
    }

    protected void OnPaymentSuspend(string result)
    {
        Debug.Log("AccountingManager:OnPaymentSuspend result [" + result + "]");
        this.IsEnableSuspendDialog = true;
        this.OnSuspendPaymentStore();
    }

    protected void OnPaymentUnknown(string result)
    {
        Debug.Log("AccountingManager:OnPaymentUnknown result [" + result + "]");
        if (ManagerConfig.UseDebugCommand)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Accounting error for debug", "OnPaymentUnknown (" + result + ")", null, false);
        }
        switch (((ChargeServicePluginScript.Response) int.Parse(result)))
        {
            case ChargeServicePluginScript.Response.BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE:
            case ChargeServicePluginScript.Response.BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE:
            case ChargeServicePluginScript.Response.BILLING_RESPONSE_RESULT_DEVELOPER_ERROR:
            case ChargeServicePluginScript.Response.IABHELPER_VERIFICATION_FAILED:
                this.SendReceiptData();
                break;

            case ChargeServicePluginScript.Response.IABHELPER_BAD_RESPONSE:
                base.Invoke("PaymentCheck", 1f);
                break;

            default:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_TIME_OVER_MESSAGE"), new ErrorDialog.ClickDelegate(this.EndPaymentUnknownErrorRetryDialog), false);
                break;
        }
    }

    protected void OnPaymentWait(string result)
    {
        Debug.Log("AccountingManager:OnPaymentWait result [" + result + "]");
        this.OnWaitPaymentStore();
    }

    protected void OnSuspendPaymentStore()
    {
        if (this.IsEnableSuspendDialog)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_PURCHASE_SUSPEND_MESSAGE"), new NotificationDialog.ClickDelegate(this.EndPurchaseSuspendDialog), -1);
        }
    }

    protected void OnWaitPaymentStore()
    {
        this.DeletePayment();
        if (this.callbackFunc != null)
        {
            ResultCallbackfunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc(Result.WAIT, 0);
        }
    }

    protected void Payment()
    {
        Debug.Log("AccountingManager:Payment [" + paymentProductId + "]");
        bool flag = false;
        if (!flag && !ChargeServicePluginScript.PaymentProduct(paymentProductId))
        {
            base.Invoke("OnCancelPaymentStore", 1f);
        }
    }

    protected void PaymentCheck()
    {
        Debug.Log("AccountingManager:PaymentCheck [" + paymentProductId + "]");
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
        if (!ChargeServicePluginScript.PaymentCheck(paymentProductId, "OnPaymentCheckResult", "OnPaymentCheckResultError"))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
            this.OnCancelPaymentStore();
        }
    }

    protected void PaymentConsume()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
        ChargeServicePluginScript.PaymentConsume(paymentProductId, "OnPaymentConsume", "OnPaymentConsumeError");
    }

    protected void PaymentItemAlreadyOwnedNoData()
    {
        paymentBaseReceipt = string.Empty;
        object[] objArray1 = new object[] { "{\"orderId\":\"NoData\",\"packageName\":\"", ManagerConfig.AndroidPackageName, "\",\"productId\":\"", paymentProductId, "\",\"purchaseTime\":", paymentTime, ",\"purchaseState\":0,\"developerPayload\":\"payload\",\"purchaseToken\":\"NoData\"}" };
        paymentReceipt = string.Concat(objArray1);
        paymentState = "Consume";
        this.WritePayment();
        this.PaymentConsume();
    }

    public static string ReadHistory(string name)
    {
        string str4;
        BinaryReader reader = new BinaryReader(File.OpenRead(GetHistoryFileName(name)));
        try
        {
            string str = reader.ReadString();
            try
            {
                return CryptData.Decrypt(str, false);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return str;
            }
            return str4;
        }
        catch (Exception exception2)
        {
            Debug.LogError(exception2.Message);
            return null;
        }
        finally
        {
            if (reader != null)
            {
                ((IDisposable) reader).Dispose();
            }
        }
        return str4;
    }

    public bool ReadOldPayment()
    {
        // This item is obfuscated and can not be translated.
    }

    public bool ReadPayment()
    {
        // This item is obfuscated and can not be translated.
    }

    public void ReciveData(string result)
    {
        this.OnEndSendReceiptData(result);
    }

    protected void RecoverBeforeReceipt()
    {
        Debug.Log("RecoverBeforeReceipt");
        if (ChargeServicePluginScript.IsPurchasedTransactionRemain())
        {
            paymentBaseReceipt = ChargeServicePluginScript.GetPurchasedTransactionRemain();
            Debug.Log("    paymentBaseReceipt [" + paymentBaseReceipt + "]");
            paymentReceipt = string.Empty;
            paymentState = "Receipt";
            this.WritePayment();
            ChargeServicePluginScript.ClearPurchasedTransactionRemain();
            this.SendBaseReceipt();
        }
        else
        {
            Debug.LogError("Could not recover payment transaction!!!");
            paymentState = "BeforeReceipt2";
            this.WritePayment();
        }
    }

    public bool RecoverPaymentStore()
    {
        if (isRecoverStore)
        {
            isRecoverStore = false;
            this.ReadPayment();
            if (this.CheckPaymentStore())
            {
                Debug.Log("RecoverPaymentStore [" + AccountingManager.paymentState + "]");
                Debug.Log("    paymentTime [" + paymentTime + "]");
                Debug.Log("    paymentReceipt [" + paymentReceipt + "]");
                Debug.Log("    paymentBaseReceipt [" + paymentBaseReceipt + "]");
                Debug.Log("    cumulativeAmount [" + cumulativeAmount + "]");
            }
            string paymentState = AccountingManager.paymentState;
            if (paymentState != null)
            {
                int num;
                if (<>f__switch$map5 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(5) {
                        { 
                            "Payment",
                            0
                        },
                        { 
                            "Deferred",
                            1
                        },
                        { 
                            "Consume",
                            2
                        },
                        { 
                            "Receipt",
                            3
                        },
                        { 
                            "SendReceipt",
                            4
                        }
                    };
                    <>f__switch$map5 = dictionary;
                }
                if (<>f__switch$map5.TryGetValue(paymentState, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.PaymentCheck();
                            return true;

                        case 1:
                            return true;

                        case 2:
                            this.PaymentConsume();
                            return true;

                        case 3:
                            this.SendBaseReceipt();
                            return true;

                        case 4:
                            this.RetrySendReceiptData();
                            return true;
                    }
                }
            }
        }
        return false;
    }

    public static void RecoverStatusReset()
    {
        isRecoverStore = true;
    }

    protected void RetrySendReceiptData()
    {
        if (NetworkManager.UserId <= 0L)
        {
            isRecoverStore = true;
        }
        else
        {
            Debug.Log("AccountingManager:RetrySendReceiptData [" + paymentReceipt + "]");
            if (string.IsNullOrEmpty(paymentReceipt))
            {
                this.callbackResult = Result.ERROR;
                this.OnEndPaymentStore("ReceiptError");
            }
            else if (!NetworkManager.getRequest<PurchaseByBankRequest>(new NetworkManager.ResultCallbackFunc(this.OnEndSendReceiptData)).beginRetryRequest(true))
            {
                NetworkManager.getRequest<PurchaseByBankRequest>(new NetworkManager.ResultCallbackFunc(this.OnEndSendReceiptData)).beginRequest(paymentBaseReceipt, paymentReceipt);
            }
        }
    }

    protected void SendBaseReceipt()
    {
        Debug.Log("AccountingManager:SendBaseReceiptData [" + paymentBaseReceipt + "]");
        if (NetworkManager.UserId > 0L)
        {
            NetworkManager.getRequest<PurchaseByBankRequest>(new NetworkManager.ResultCallbackFunc(this.OnEndSendReceiptData)).beginRequest(paymentBaseReceipt, string.Empty);
        }
    }

    protected void SendPurchaseAction(string paymentProductId)
    {
        int id = -1;
        BankShopEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BankShopMaster>(DataNameKind.Kind.BANK_SHOP).GetEnableEntitiyList();
        int googlePrice = 0;
        foreach (BankShopEntity entity in enableEntitiyList)
        {
            if (entity.googleShopId == paymentProductId)
            {
                googlePrice = entity.googlePrice;
                id = entity.id;
                break;
            }
        }
    }

    protected void SendReceiptData()
    {
        Debug.Log("AccountingManager:SendReceiptData [" + paymentReceipt + "]");
        this.WritePayment();
        if (NetworkManager.UserId <= 0L)
        {
            isRecoverStore = true;
        }
    }

    public static void SetEnableStore(bool isEnable)
    {
        isEnableStore = isEnable;
        if (ManagerConfig.UseAppServer && (initializeResult == 0))
        {
            if (isMoveStore != isEnable)
            {
                isMoveStore = isEnable;
                if (isEnable)
                {
                    ChargeServicePluginScript.Resume();
                }
                else
                {
                    ChargeServicePluginScript.Pause();
                }
            }
            if (isEnable && !SingletonMonoBehaviour<AccountingManager>.Instance.RecoverPaymentStore())
            {
                SingletonMonoBehaviour<AccountingManager>.Instance.ContinueReceiptProc();
            }
        }
    }

    public void StartPaymentStore(BankShopEntity bankShop, int cumulativeAmount, ResultCallbackfunc callback)
    {
        Debug.Log("AccountingManager:StartPaymentStore Start [" + cumulativeAmount + "]");
        SetEnableStore(true);
        this.callbackFunc = callback;
        this.callbackResult = Result.BUSY;
        if (int.Parse(SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).getPey()[bankShop.id - 1]) >= bankShop.firstPayId)
        {
            bankShop.m_bfirst = false;
        }
        else
        {
            bankShop.m_bfirst = true;
        }
        NetworkManager.getRequest<PayBiliOrderRequest>(null).beginRequest(bankShop);
    }

    public void WritePayment()
    {
    }

    public delegate void RequestCallbackfunc(ResponseData[] responseList);

    public enum Result
    {
        NONE,
        BUSY,
        SUCCESS,
        WAIT,
        CANCEL,
        ERROR,
        SUSPEND
    }

    public delegate void ResultCallbackfunc(AccountingManager.Result result, int cumulativeAmount);
}

