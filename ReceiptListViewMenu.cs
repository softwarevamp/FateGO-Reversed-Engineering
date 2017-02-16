using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ReceiptListViewMenu")]
public class ReceiptListViewMenu : MonoBehaviour
{
    public ReceiptListViewManager listViewManager;
    public ReceiptViewMenu receiptViewMenu;
    public UIButton scriptTestAssetCancelButton;
    public GameObject scriptTestAssetRootObject;
    protected string selectReceiptPath;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
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
        this.EndInput();
        if (this.state != State.INIT)
        {
            this.listViewManager.DestroyList();
            this.state = State.INIT;
        }
        this.scriptTestAssetRootObject.SetActive(false);
    }

    public void EndInput()
    {
        if (this.state != State.INIT)
        {
            this.listViewManager.IsInput = false;
            this.scriptTestAssetCancelButton.enabled = false;
        }
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.EndInput();
            this.state = State.SELECTED;
            this.Callback(false);
        }
    }

    protected void OnClickItem()
    {
        if (this.state == State.INPUT)
        {
            int clickResult = this.listViewManager.GetClickResult();
            if (clickResult >= 0)
            {
                ReceiptListViewItem item = this.listViewManager.GetItem(clickResult);
                this.selectReceiptPath = item.Path;
                Debug.Log("ReceiptRead [" + this.selectReceiptPath + "]");
                string message = AccountingManager.ReadHistory(this.selectReceiptPath);
                Debug.Log(message);
                if (message != null)
                {
                    this.state = State.CHECK_RECEIPT_VIEW;
                    this.receiptViewMenu.Open(message, new ReceiptViewMenu.CallbackFunc(this.OnEndCheckReceipt));
                }
            }
        }
    }

    protected void OnEndCheckReceipt()
    {
        if (this.state == State.CHECK_RECEIPT_VIEW)
        {
            this.receiptViewMenu.Close();
            this.state = State.INPUT;
            this.listViewManager.SetMode(ReceiptListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
            this.scriptTestAssetCancelButton.enabled = true;
        }
    }

    protected void OnMoveEnd()
    {
        if (this.state == State.INIT_MOVE)
        {
            this.state = State.INPUT;
            this.listViewManager.SetMode(ReceiptListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
            this.scriptTestAssetCancelButton.enabled = true;
        }
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.scriptTestAssetRootObject.SetActive(true);
            this.listViewManager.IsInput = false;
            this.scriptTestAssetCancelButton.enabled = false;
            this.listViewManager.CreateList();
        }
        this.state = State.INIT_MOVE;
        this.listViewManager.SetMode(ReceiptListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INIT_MOVE,
        INPUT,
        CHECK_RECEIPT_VIEW,
        SELECTED,
        CLOSE
    }
}

