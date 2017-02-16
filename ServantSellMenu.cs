using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantSellMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected System.Action openCallbackFunc;
    [SerializeField]
    protected UICommonButton servantEquipTabButton;
    [SerializeField]
    protected UILabel servantEquipTabLabel;
    [SerializeField]
    protected UISprite servantEquipTabSprite;
    [SerializeField]
    protected UISprite servantEquipTabTitleSprite;
    [SerializeField]
    protected ServantOperationManager servantOperationManager;
    private long servantStatusId;
    [SerializeField]
    protected UICommonButton servantTabButton;
    [SerializeField]
    protected UILabel servantTabLabel;
    [SerializeField]
    protected UISprite servantTabSprite;
    [SerializeField]
    protected UISprite servantTabTitleSprite;
    protected State state;
    protected ServantOperationManager.Kind tabKind;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, long[] list = null)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, list);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.VALID);
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndCloseShowServant()
    {
        this.state = State.INPUT;
        this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.INPUT, new ServantOperationManager.CallbackFunc(this.OnSelectSellServant));
    }

    protected void EndOpen()
    {
        if (this.callbackFunc != null)
        {
            this.state = State.INPUT;
            this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.INPUT, new ServantOperationManager.CallbackFunc(this.OnSelectSellServant));
        }
        else
        {
            this.state = State.SELECTED;
        }
        if (this.openCallbackFunc != null)
        {
            System.Action openCallbackFunc = this.openCallbackFunc;
            this.openCallbackFunc = null;
            openCallbackFunc();
        }
    }

    protected void EndShowServant(bool isDecide)
    {
        long userSvtId = -1L;
        if (isDecide)
        {
            userSvtId = this.servantStatusId;
            this.servantOperationManager.ModifyItem(userSvtId);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public ServantOperationManager.Kind GetTabKind() => 
        this.tabKind;

    public void Init()
    {
        this.servantOperationManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void Init(ServantOperationManager.Kind tabToInit)
    {
        this.servantOperationManager.DestroyList();
        this.state = State.INIT;
        this.tabKind = tabToInit;
        base.Init();
    }

    public void OnClickTabServant()
    {
        Debug.Log("OnClickTabServantEquip: " + this.state);
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(ServantOperationManager.Kind.SERVANT, false);
            this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.INPUT);
        }
    }

    public void OnClickTabServantEquip()
    {
        Debug.Log("OnClickTabServantEquip: " + this.state);
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(ServantOperationManager.Kind.SERVANT_EQUIP, false);
            this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.INPUT);
        }
    }

    protected void OnSelectSellServant(ServantOperationManager.ActionKind kind, long[] list)
    {
        if (this.state == State.INPUT)
        {
            switch (kind)
            {
                case ServantOperationManager.ActionKind.SELECT_LIST:
                    this.Callback(ResultKind.DECIDE, list);
                    break;

                case ServantOperationManager.ActionKind.SERVANT_STATUS:
                    this.state = State.SHOW_SERVANT_DETAIL;
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.servantStatusId = list[0];
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, list[0], new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                    break;
            }
        }
    }

    public void Open(CallbackFunc callback, System.Action openCallback = null)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            base.gameObject.SetActive(true);
            this.SetTabKind(this.tabKind, true);
            this.servantOperationManager.SetMode(ServantOperationListViewManager.InitMode.VALID);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if ((this.state == State.INPUT) || (this.state == State.SELECTED))
        {
            this.callbackFunc = callback;
            this.openCallbackFunc = openCallback;
            this.EndOpen();
        }
    }

    protected void SetTabKind(ServantOperationManager.Kind kind, bool isInit = false)
    {
        int num;
        int num2;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getCount(out num, out num2);
        this.servantTabLabel.text = string.Format(LocalizationManager.Get("CHARA_GRAPH_TAB_SERVANT"), num, entity.svtKeep);
        this.servantEquipTabLabel.text = string.Format(LocalizationManager.Get("CHARA_GRAPH_TAB_SERVANT_EQUIP"), num2, entity.svtEquipKeep);
        this.servantTabButton.isEnabled = true;
        this.servantTabButton.enabled = kind != ServantOperationManager.Kind.SERVANT;
        this.servantTabTitleSprite.spriteName = (kind == ServantOperationManager.Kind.SERVANT) ? "btn_txt_servant_on" : "btn_txt_servant_off";
        this.servantTabSprite.spriteName = (kind == ServantOperationManager.Kind.SERVANT) ? "btn_bg_09" : "btn_bg_08";
        this.servantTabButton.SetState(UICommonButtonColor.State.Normal, isInit);
        this.servantEquipTabButton.isEnabled = true;
        this.servantEquipTabButton.enabled = kind != ServantOperationManager.Kind.SERVANT_EQUIP;
        this.servantEquipTabTitleSprite.spriteName = (kind == ServantOperationManager.Kind.SERVANT_EQUIP) ? "btn_txt_craftessence_on" : "btn_txt_craftessence_off";
        this.servantEquipTabSprite.spriteName = (kind == ServantOperationManager.Kind.SERVANT_EQUIP) ? "btn_bg_09" : "btn_bg_08";
        this.servantEquipTabButton.SetState(UICommonButtonColor.State.Normal, isInit);
        if (isInit)
        {
            this.servantOperationManager.CreateList(kind);
        }
        else if (this.tabKind != kind)
        {
            switch (kind)
            {
                case ServantOperationManager.Kind.SERVANT:
                    this.servantOperationManager.ChangeList(ServantOperationManager.Kind.SERVANT);
                    this.servantOperationManager.filterButtonState(UICommonButtonColor.State.Normal, true);
                    break;

                case ServantOperationManager.Kind.SERVANT_EQUIP:
                    this.servantOperationManager.ChangeList(ServantOperationManager.Kind.SERVANT_EQUIP);
                    this.servantOperationManager.filterButtonState(UICommonButtonColor.State.Disabled, true);
                    break;
            }
        }
        this.tabKind = kind;
    }

    public delegate void CallbackFunc(ServantSellMenu.ResultKind result, long[] list);

    public enum ResultKind
    {
        CANCEL,
        DECIDE
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE,
        SHOW_SERVANT_DETAIL
    }
}

