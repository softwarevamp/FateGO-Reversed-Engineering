using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApRecoverDlgComponent : BaseDialog
{
    [CompilerGenerated]
    private static Comparison<ApRecoverEntity> <>f__am$cache7;
    [SerializeField]
    protected UIButton closeBtn;
    private bool isClosed;
    [SerializeField]
    protected UIGrid itemListInfoGrid;
    [SerializeField]
    protected GameObject itemListObj;
    [SerializeField]
    protected UILabel titleDetailLabel;
    [SerializeField]
    protected UILabel titleLabel;

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

    protected void clearInfoGrid()
    {
        int childCount = this.itemListInfoGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.itemListInfoGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public void Close()
    {
        this.Close(new System.Action(this.EndClose));
    }

    public void Close(System.Action callback)
    {
        <Close>c__AnonStorey48 storey = new <Close>c__AnonStorey48 {
            callback = callback,
            <>f__this = this
        };
        this.isClosed = true;
        base.Close(new System.Action(storey.<>m__4));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
    }

    protected void EndOpen()
    {
    }

    private void EndRequestUserGameActRecover(string result)
    {
        this.Callback(Result.RECOVER);
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        base.Init();
    }

    public void OnClickClose()
    {
        if (!this.isClosed)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback(Result.CANCEL);
        }
    }

    public void OpenApRecvItemDlg(int needAp, CallbackFunc callback)
    {
        this.clearInfoGrid();
        this.callbackFunc = callback;
        this.titleLabel.text = LocalizationManager.Get("APRECV_TITILE_TXT");
        this.titleDetailLabel.text = LocalizationManager.Get("APRECV_TITILE_DETAIL_TXT");
        base.gameObject.SetActive(true);
        this.isClosed = false;
        List<ApRecoverEntity> list = new List<ApRecoverEntity>(SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ApRecoverMaster>(DataNameKind.Kind.AP_RECOVER).getEntitys<ApRecoverEntity>());
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = (a, b) => b.priority - a.priority;
        }
        list.Sort(<>f__am$cache7);
        foreach (ApRecoverEntity entity in list.ToArray())
        {
            GameObject obj2 = base.createObject(this.itemListObj, this.itemListInfoGrid.transform, null);
            obj2.transform.localPosition = Vector3.zero;
            obj2.GetComponent<ApRecoverItemComponent>().setRecvItemInfo(entity, needAp, new ApRecoverItemComponent.CallbackFunc(this.requestApItem));
            obj2.SetActive(true);
        }
        this.itemListInfoGrid.repositionNow = true;
        base.Open(new System.Action(this.EndOpen), true);
    }

    private void requestApItem(RecoverType.Type type, int id, int num)
    {
        Debug.Log($"requestApItem Param: {type}, {id}, {num}");
        if (type == RecoverType.Type.STONE)
        {
            NetworkManager.getRequest<PurchaseByStoneRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestUserGameActRecover)).beginRequest(id, num);
        }
        if (type == RecoverType.Type.ITEM)
        {
            NetworkManager.getRequest<ApRecoverUseItemRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestUserGameActRecover)).beginRequest(id, num);
        }
        if (type == RecoverType.Type.COMMAND_SPELL)
        {
            NetworkManager.getRequest<ApRecoverCmdSpellRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestUserGameActRecover)).beginRequest(id);
        }
    }

    [CompilerGenerated]
    private sealed class <Close>c__AnonStorey48
    {
        internal ApRecoverDlgComponent <>f__this;
        internal System.Action callback;

        internal void <>m__4()
        {
            this.<>f__this.EndClose();
            this.callback.Call();
        }
    }

    public delegate void CallbackFunc(ApRecoverDlgComponent.Result result);

    public enum Result
    {
        CANCEL,
        ERROR,
        RECOVER
    }
}

