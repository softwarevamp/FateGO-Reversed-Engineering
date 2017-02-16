using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupportServantSelectMenu : BaseMenu
{
    private int classPos;
    protected State state;
    [SerializeField]
    protected SupportServantListViewManager supportServantListViewManager;
    [SerializeField]
    protected TitleInfoControl titleInfo;

    protected event CallbackFunc callbackFunc;

    protected void Callback(SupportSelectRootComponent.ResultKind result, int classPos, UserServantEntity entity)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, classPos, entity);
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
        this.supportServantListViewManager.SetMode(SupportServantListViewManager.InitMode.INPUT, new SupportServantListViewManager.CallbackFunc(this.OnSelectItem));
    }

    public void Init()
    {
        this.supportServantListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void ModifyItem()
    {
        this.supportServantListViewManager.SetMode(SupportServantListViewManager.InitMode.MODIFY);
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Init();
            base.gameObject.SetActive(false);
            this.Callback(SupportSelectRootComponent.ResultKind.CANCEL, -1, null);
        }
    }

    protected void OnSelectItem(SupportServantListViewManager.ResultKind kind, int offset)
    {
        if (this.state == State.INPUT)
        {
            SupportServantListViewItem item = (offset < 0) ? null : this.supportServantListViewManager.GetItem(offset);
            if (kind == SupportServantListViewManager.ResultKind.DECIDE)
            {
                if (item.IsBase)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.Init();
                    base.gameObject.SetActive(false);
                    this.Callback(SupportSelectRootComponent.ResultKind.REMOVE, this.classPos, item.UserServant);
                }
                else if (item.IsUseServant)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.supportServantListViewManager.SetMode(SupportServantListViewManager.InitMode.INPUT, new SupportServantListViewManager.CallbackFunc(this.OnSelectItem));
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.Init();
                    base.gameObject.SetActive(false);
                    this.Callback(SupportSelectRootComponent.ResultKind.DECIDE, this.classPos, item.UserServant);
                }
            }
        }
    }

    public void Open(SupportServantData supportServantData, int classPos, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.classPos = classPos;
        this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.PARTY_ORGANIZATION_SERVANT);
        this.titleInfo.setBackBtnSprite(true);
        this.supportServantListViewManager.CreateList(supportServantData, classPos);
        this.supportServantListViewManager.SetMode(SupportServantListViewManager.InitMode.VALID, new SupportServantListViewManager.CallbackFunc(this.OnSelectItem));
        this.state = State.INPUT;
        base.Open(new System.Action(this.EndOpen));
    }

    public delegate void CallbackFunc(SupportSelectRootComponent.ResultKind result, int classPos, UserServantEntity entity);

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

