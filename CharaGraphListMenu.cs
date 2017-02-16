using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharaGraphListMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected bool isInitTab;
    protected Kind kind;
    protected int selectNum;
    [SerializeField]
    protected UICommonButton servantEquipTabButton;
    [SerializeField]
    protected UILabel servantEquipTabLabel;
    [SerializeField]
    protected UISprite servantEquipTabSprite;
    [SerializeField]
    protected UISprite servantEquipTabTitleSprite;
    [SerializeField]
    protected ServantListViewManager servantListViewManager;
    [SerializeField]
    protected UICommonButton servantTabButton;
    [SerializeField]
    protected UILabel servantTabLabel;
    [SerializeField]
    protected UISprite servantTabSprite;
    [SerializeField]
    protected UISprite servantTabTitleSprite;
    protected State state;
    protected TabKind tabKind;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int n = -1)
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
        this.state = State.CLOSE;
        this.SetTabKind(this.tabKind, false);
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

    protected void EndCloseList()
    {
    }

    protected void EndCloseShowServant()
    {
        this.state = State.INPUT;
        this.servantListViewManager.SetMode(ServantListViewManager.InitMode.INPUT, new ServantListViewManager.CallbackFunc(this.OnSelectServant));
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
        this.SetTabKind(this.tabKind, false);
    }

    protected void EndShowServant(bool isDecide)
    {
        if (isDecide)
        {
            this.servantListViewManager.ModifyList();
            this.servantListViewManager.SetMode(ServantListViewManager.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public void Init()
    {
        this.servantListViewManager.DestroyList();
        this.state = State.INIT;
        this.tabKind = TabKind.SERVANT;
        this.isInitTab = false;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.DECIDE, -1);
        }
    }

    public void OnClickTabServant()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.SERVANT, false);
        }
    }

    public void OnClickTabServantEquip()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.SERVANT_EQUIP, false);
        }
    }

    protected void OnSelectServant(ServantListViewManager.ResultKind kind, int n)
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.selectNum = n;
            ServantListViewItem item = (n < 0) ? null : this.servantListViewManager.GetItem(n);
            if (kind == ServantListViewManager.ResultKind.DECIDE)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.state = State.SHOW_STATUS;
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, item.UserServant, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
            }
            else
            {
                this.Callback(ResultKind.CANCEL, -1);
            }
        }
    }

    public void Open(Kind kind, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.kind = kind;
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.SetTabKind(this.tabKind, true);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen));
        }
        else if ((this.state == State.INPUT) || (this.state == State.SELECTED))
        {
            this.kind = kind;
            this.callbackFunc = callback;
            this.SetTabKind(this.tabKind, false);
            this.EndOpen();
        }
    }

    protected void SetTabKind(TabKind kind, bool isInit = false)
    {
        int num;
        int num2;
        if (!this.isInitTab)
        {
            isInit = true;
        }
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getCount(out num, out num2);
        this.servantTabLabel.text = string.Format(LocalizationManager.Get("CHARA_GRAPH_TAB_SERVANT"), num, entity.svtKeep);
        this.servantEquipTabLabel.text = string.Format(LocalizationManager.Get("CHARA_GRAPH_TAB_SERVANT_EQUIP"), num2, entity.svtEquipKeep);
        Kind kind2 = this.kind;
        this.servantTabButton.isEnabled = true;
        this.servantTabButton.enabled = kind != TabKind.SERVANT;
        this.servantTabTitleSprite.spriteName = (kind == TabKind.SERVANT) ? "btn_txt_servant_on" : "btn_txt_servant_off";
        this.servantTabSprite.spriteName = (kind == TabKind.SERVANT) ? "btn_bg_09" : "btn_bg_08";
        this.servantTabButton.SetState(UICommonButtonColor.State.Normal, !isInit);
        this.servantEquipTabButton.isEnabled = true;
        this.servantEquipTabButton.enabled = kind != TabKind.SERVANT_EQUIP;
        this.servantEquipTabTitleSprite.spriteName = (kind == TabKind.SERVANT_EQUIP) ? "btn_txt_craftessence_on" : "btn_txt_craftessence_off";
        this.servantEquipTabSprite.spriteName = (kind == TabKind.SERVANT_EQUIP) ? "btn_bg_09" : "btn_bg_08";
        this.servantEquipTabButton.SetState(UICommonButtonColor.State.Normal, !isInit);
        if (isInit || (kind != this.tabKind))
        {
            switch (kind)
            {
                case TabKind.SERVANT:
                    this.servantListViewManager.CreateList(ServantListViewManager.Kind.SERVANT);
                    this.servantListViewManager.filterButtonState(UICommonButtonColor.State.Normal, true);
                    break;

                case TabKind.SERVANT_EQUIP:
                    this.servantListViewManager.CreateList(ServantListViewManager.Kind.SERVANT_EQUIP);
                    this.servantListViewManager.filterButtonState(UICommonButtonColor.State.Disabled, true);
                    break;
            }
        }
        if (this.state == State.INPUT)
        {
            this.servantListViewManager.SetMode(ServantListViewManager.InitMode.INPUT, new ServantListViewManager.CallbackFunc(this.OnSelectServant));
        }
        else
        {
            this.servantListViewManager.SetMode(ServantListViewManager.InitMode.VALID);
        }
        this.isInitTab = true;
        this.tabKind = kind;
    }

    public delegate void CallbackFunc(CharaGraphListMenu.ResultKind result);

    public enum Kind
    {
        EQUIP
    }

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
        EQUIP_SELECT,
        SHOW_STATUS
    }

    public enum TabKind
    {
        SERVANT,
        SERVANT_EQUIP,
        SUM
    }
}

