using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemDetailInfoComponent : BaseDialog
{
    [SerializeField]
    protected GameObject closeBtnObject;
    protected System.Action closeCallbackFunc;
    public CombineItemPlaceListViewManager combineItemPlaceListViewManager;
    [SerializeField]
    protected UILabel freeStoneNumLabel;
    [SerializeField]
    protected UILabel freeStoneTitleLabel;
    [SerializeField]
    protected GameObject itemDetailInfo;
    [SerializeField]
    protected UILabel itemDetailLabel;
    [SerializeField]
    protected UILabel itemNameLabel;
    [SerializeField]
    protected UILabel payStoneNumLabel;
    [SerializeField]
    protected UILabel payStoneTitleLabel;
    protected State state;
    [SerializeField]
    protected GameObject stoneDetailBtnObject;
    [SerializeField]
    protected GameObject stoneDetailInfo;
    [SerializeField]
    protected UILabel stoneDetailLabel;
    [SerializeField]
    protected UILabel stoneNoticeLabel;
    [SerializeField]
    protected UILabel stoneTitleLabel;

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
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
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
        if (this.state == State.NORMALOPEN)
        {
            this.state = State.INIT;
        }
    }

    public void Init()
    {
        this.itemDetailInfo.SetActive(true);
        this.itemNameLabel.text = string.Empty;
        this.itemDetailLabel.text = string.Empty;
        this.stoneDetailInfo.SetActive(false);
        this.payStoneNumLabel.text = string.Empty;
        this.freeStoneNumLabel.text = string.Empty;
        base.gameObject.SetActive(false);
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        this.Callback(false);
    }

    public void OnClickDecide()
    {
        this.Callback(true);
    }

    public void Open(ItemEntity itemData, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.itemNameLabel.text = itemData.name;
            this.itemDetailLabel.text = itemData.detail;
            this.state = State.NORMALOPEN;
            this.setBtnInfoActive();
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenCombine(ItemEntity itemData, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.itemNameLabel.text = itemData.name;
            this.itemDetailLabel.text = itemData.detail;
            this.state = State.NORMALOPEN;
            this.setBtnInfoActive();
            base.Open(new System.Action(this.EndOpen), true);
            this.combineItemPlaceListViewManager.Init(itemData);
            this.combineItemPlaceListViewManager.CreateList();
        }
    }

    public void OpenItemMsgInfo(string name, string detail, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.itemNameLabel.text = name;
            this.itemDetailLabel.text = detail;
            this.state = State.NORMALOPEN;
            this.setBtnInfoActive();
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenUserItemInfo(UserItemData itemData, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.itemNameLabel.text = itemData.name;
            this.itemDetailLabel.text = itemData.detail;
            this.state = State.NORMALOPEN;
            this.setBtnInfoActive();
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    private void setBtnInfoActive()
    {
        if (this.state == State.NORMALOPEN)
        {
            if (this.stoneDetailBtnObject.activeSelf)
            {
                this.stoneDetailBtnObject.SetActive(false);
            }
            this.closeBtnObject.SetActive(true);
        }
    }

    public void ShowStoneDetail(string name, string detail, UserGameEntity userData, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.callbackFunc = callback;
            this.itemDetailInfo.SetActive(false);
            this.stoneDetailInfo.SetActive(true);
            this.stoneTitleLabel.text = name;
            this.stoneDetailLabel.text = detail;
            this.payStoneTitleLabel.text = LocalizationManager.Get("PAYSTONE_INFO_TITLE");
            this.payStoneNumLabel.text = LocalizationManager.GetUnitInfo(userData.chargeStone);
            this.freeStoneTitleLabel.text = LocalizationManager.Get("FREESTONE_INFO_TITLE");
            this.freeStoneNumLabel.text = LocalizationManager.GetUnitInfo(userData.freeStone);
            this.stoneNoticeLabel.text = LocalizationManager.Get("STONE_INFO_NOTICE");
            this.state = State.NORMALOPEN;
            this.setBtnInfoActive();
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        NORMALOPEN,
        STONEOPEN,
        STONEINFO
    }
}

