using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SupportSelectMenu : BaseMenu
{
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected GameObject explanationBase;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UICommonButton partyDecideButton;
    [SerializeField]
    protected UICommonButton partyRemoveButton;
    [SerializeField]
    protected SupportSelectObject[] supportSelectObject = new SupportSelectObject[BalanceConfig.SupportDeckMax];
    protected SupportServantData supportServantData;

    protected event CallbackFunc callbackFunc;

    public void Active()
    {
        base.gameObject.SetActive(true);
    }

    protected void Callback(ResultKind result, int n = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            callbackFunc(result, n);
        }
    }

    protected void ClearItem(int classPos = -1)
    {
        if (classPos == -1)
        {
            for (int i = 0; i < this.supportSelectObject.Length; i++)
            {
                this.supportSelectObject[i].ClearItem();
            }
        }
        else
        {
            this.supportSelectObject[classPos].ClearItem();
        }
    }

    public void Close()
    {
        this.Init();
        this.Callback(ResultKind.CLOSE, -1);
    }

    public void Init()
    {
        this.ClearItem(-1);
        this.supportServantData = null;
        base.Init();
    }

    public void OnClickDecide()
    {
        this.Callback(ResultKind.DECIDE, -1);
    }

    protected void OnClickItem(SupportSelectObject.ResultKind result, int classPos)
    {
        Debug.Log("SupportSelectMenu : OnClickItem " + classPos);
        base.gameObject.SetActive(false);
        if (this.supportServantData.IsFriendInfo)
        {
            if (this.supportServantData.IsSelectServant)
            {
                this.Callback(ResultKind.SELECT_FOLLOWER, classPos);
            }
            else if (result == SupportSelectObject.ResultKind.SELECT_EQUIP)
            {
                this.Callback(ResultKind.SUPPORT_INFO_EQUIP, classPos);
            }
            else
            {
                this.Callback(ResultKind.SUPPORT_INFO_SERVANT, classPos);
            }
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (result == SupportSelectObject.ResultKind.SELECT_EQUIP)
            {
                this.Callback(ResultKind.SELECT_EQUIP, classPos);
            }
            else
            {
                this.Callback(ResultKind.SELECT_SERVANT, classPos);
            }
        }
    }

    public void Open(SupportServantData supportServantData, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.supportServantData = supportServantData;
        if (this.supportServantData.IsFriendInfo)
        {
            this.explanationLabel.text = LocalizationManager.Get(!this.supportServantData.IsSelectServant ? "SUPPORT_INFO_HELP" : "SUPPORT_FOLLOWER_HELP");
            this.decideButton.gameObject.SetActive(false);
        }
        else
        {
            this.explanationLabel.text = LocalizationManager.Get("SUPPORT_SELECT_HELP");
            this.decideButton.gameObject.SetActive(true);
        }
        base.gameObject.SetActive(true);
        this.SetItem(-1);
        base.Open(null);
    }

    public void Redisp()
    {
        base.gameObject.SetActive(true);
        this.ClearItem(-1);
        this.SetItem(-1);
    }

    public void Reset(int classPos = -1)
    {
        base.gameObject.SetActive(true);
        this.ClearItem(classPos);
        this.SetItem(classPos);
    }

    protected void SetItem(int classPos = -1)
    {
        if (classPos == -1)
        {
            for (int i = 0; i < this.supportSelectObject.Length; i++)
            {
                this.supportSelectObject[i].SetItem(this.supportServantData, i, new SupportSelectObject.CallbackFunc(this.OnClickItem));
            }
        }
        else
        {
            this.supportSelectObject[classPos].SetItem(this.supportServantData, classPos, new SupportSelectObject.CallbackFunc(this.OnClickItem));
        }
    }

    public delegate void CallbackFunc(SupportSelectMenu.ResultKind result, int n);

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        CLOSE,
        SELECT_SERVANT,
        SELECT_EQUIP,
        SELECT_FOLLOWER,
        SUPPORT_INFO_SERVANT,
        SUPPORT_INFO_EQUIP
    }
}

