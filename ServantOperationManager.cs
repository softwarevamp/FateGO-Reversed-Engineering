using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantOperationManager : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton allReleaseButton;
    protected int currentListViewCnt;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UICommonButton filterButton;
    protected InitMode initMode;
    [SerializeField]
    protected UILabel mpDataLabel;
    [SerializeField]
    protected UILabel qpDataLabel;
    [SerializeField]
    protected UILabel selectDoneLabel;
    [SerializeField]
    protected ServantOperationListViewManager[] servantOperationListViewManager;
    protected const string SORT_SAVE_KEY = "ServantOperation";
    protected int totalMana;
    protected int totalQP;
    protected int totalSum;

    protected event CallbackFunc callbackFunc;

    public void ChangeList(Kind kind)
    {
        this.currentListViewCnt = (int) kind;
        bool flag = kind == Kind.SERVANT;
        this.servantOperationListViewManager[0].gameObject.SetActive(flag);
        this.servantOperationListViewManager[1].gameObject.SetActive(!flag);
        this.servantOperationListViewManager[this.currentListViewCnt].changeSortKindDisp();
    }

    public void CreateList(Kind kind)
    {
        this.totalSum = 0;
        this.totalQP = 0;
        this.totalMana = 0;
        this.allReleaseButton.SetState(UICommonButtonColor.State.Disabled, false);
        this.decideButton.SetState(UICommonButtonColor.State.Disabled, false);
        this.qpDataLabel.text = LocalizationManager.GetNumberFormat(this.totalQP);
        this.mpDataLabel.text = LocalizationManager.GetNumberFormat(this.totalMana);
        this.selectDoneLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.totalSum, BalanceConfig.ServantSellSelectMax);
        this.servantOperationListViewManager[0].gameObject.SetActive(true);
        this.servantOperationListViewManager[0].CreateList(ServantOperationListViewManager.Kind.SERVANT);
        this.servantOperationListViewManager[1].gameObject.SetActive(true);
        this.servantOperationListViewManager[1].CreateList(ServantOperationListViewManager.Kind.SERVANT_EQUIP);
        this.ChangeList(kind);
    }

    public void DestroyList()
    {
        foreach (ServantOperationListViewManager manager in this.servantOperationListViewManager)
        {
            manager.DestroyList();
        }
    }

    protected void EndCloseSelectFilterKind()
    {
    }

    protected void EndSelectFilterKind(bool isDecide)
    {
        this.servantOperationListViewManager[this.currentListViewCnt].EndSelectFilterKind(isDecide);
    }

    public void filterButtonState(UICommonButtonColor.State state, bool animation)
    {
        this.filterButton.SetState(state, animation);
    }

    public long GetAmountSortValue(int svtId) => 
        this.servantOperationListViewManager[this.currentListViewCnt].GetAmountSortValue(svtId);

    public void ModifyItem(long userSvtId)
    {
        this.servantOperationListViewManager[this.currentListViewCnt].ModifyItem(userSvtId);
    }

    public void numberAdjustment(int selectNum)
    {
        foreach (ServantOperationListViewManager manager in this.servantOperationListViewManager)
        {
            manager.decrementNumber(selectNum);
        }
    }

    public void OnClickDecide()
    {
        Debug.Log("ServantOperationManager::OnClickDecide " + this.totalSum);
        if (this.totalSum > 0)
        {
            List<long> list = new List<long>();
            foreach (ServantOperationListViewManager manager in this.servantOperationListViewManager)
            {
                manager.getSelectList(list);
            }
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ActionKind.SELECT_LIST, list.ToArray());
            }
        }
    }

    public void OnClickFilterKind()
    {
        if (this.currentListViewCnt == 1)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
        else
        {
            this.servantOperationListViewManager[this.currentListViewCnt].OnClickFilterKind();
        }
    }

    public void OnClickReleaseAll()
    {
        Debug.Log("ServantOperationManager::OnClickReleaseAll " + this.totalSum);
        if (this.totalSum > 0)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            foreach (ServantOperationListViewManager manager in this.servantOperationListViewManager)
            {
                manager.releaseAll();
            }
            this.totalSum = 0;
            this.selectDoneLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.totalSum, BalanceConfig.ServantSellSelectMax);
            this.RefrashListDisp();
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void OnClickSortAscendingOrder()
    {
        this.servantOperationListViewManager[this.currentListViewCnt].OnClickSortAscendingOrder();
    }

    public void OnClickSortKind()
    {
        this.servantOperationListViewManager[this.currentListViewCnt].OnClickSortKind();
    }

    protected void OnSelectServant(long svtId)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        List<long> list = new List<long> {
            svtId
        };
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ActionKind.SERVANT_STATUS, list.ToArray());
        }
    }

    public void RefrashListDisp()
    {
        this.totalQP = 0;
        this.totalMana = 0;
        foreach (ServantOperationListViewManager manager in this.servantOperationListViewManager)
        {
            int num;
            int num2;
            manager.sumItems(out num, out num2);
            this.totalQP += num;
            this.totalMana += num2;
        }
        this.qpDataLabel.text = LocalizationManager.GetNumberFormat(this.totalQP);
        this.mpDataLabel.text = LocalizationManager.GetNumberFormat(this.totalMana);
        this.allReleaseButton.SetState((this.totalSum <= 0) ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
        this.decideButton.SetState((this.totalSum <= 0) ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, true);
    }

    public void SetMode(ServantOperationListViewManager.InitMode mode)
    {
        Debug.Log("ServantOperationManager::SetMode " + mode);
        this.servantOperationListViewManager[this.currentListViewCnt].SetMode(mode, new ServantOperationListViewManager.CallbackFunc(this.OnSelectServant));
    }

    public void SetMode(ServantOperationListViewManager.InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    public void setServant(bool flag)
    {
        if (flag)
        {
            this.totalSum++;
        }
        else
        {
            this.totalSum--;
        }
    }

    public int TotalSum =>
        this.totalSum;

    public enum ActionKind
    {
        NONE,
        SELECT_LIST,
        SERVANT_STATUS
    }

    public delegate void CallbackFunc(ServantOperationManager.ActionKind kind, long[] list);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        ENTER,
        EXIT
    }

    public enum Kind
    {
        SERVANT,
        SERVANT_EQUIP
    }
}

