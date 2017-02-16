using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MaterialCollectionMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected bool isInitTab;
    [SerializeField]
    protected MaterialCollectionServantListViewManager materialCollectionServantListViewManager;
    protected System.Action mOpenedAct;
    protected int[] newSvtIdList;
    protected int selectIndex;
    [SerializeField]
    protected UICommonButton servantEquipTabButton;
    [SerializeField]
    protected UILabel servantEquipTabLabelL;
    [SerializeField]
    protected UILabel servantEquipTabLabelR;
    [SerializeField]
    protected UISprite servantEquipTabSprite;
    [SerializeField]
    protected UISprite servantEquipTabStrSp;
    [SerializeField]
    protected UICommonButton servantTabButton;
    [SerializeField]
    protected UILabel servantTabLabelL;
    [SerializeField]
    protected UILabel servantTabLabelR;
    [SerializeField]
    protected UISprite servantTabSprite;
    [SerializeField]
    protected UISprite servantTabStrSp;
    protected State state;
    protected TabKind tabKind;

    protected event CallbackFunc callbackFunc;

    protected void Callback()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc();
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
        base.Invoke("OnStartClose", 0.1f);
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

    protected void EndCloseShowServant()
    {
        this.materialCollectionServantListViewManager.ModifyItem(this.selectIndex);
        this.materialCollectionServantListViewManager.SetMode(MaterialCollectionServantListViewManager.InitMode.INPUT, new MaterialCollectionServantListViewManager.CallbackFunc(this.OnSelectServant));
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
        this.materialCollectionServantListViewManager.SetMode(MaterialCollectionServantListViewManager.InitMode.INPUT, new MaterialCollectionServantListViewManager.CallbackFunc(this.OnSelectServant));
        this.mOpenedAct.Call();
    }

    protected void EndShowServant(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public void Init()
    {
        this.state = State.INIT;
        this.materialCollectionServantListViewManager.DestroyList();
        this.materialCollectionServantListViewManager.gameObject.SetActive(false);
        this.tabKind = TabKind.SERVANT;
        this.isInitTab = false;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback();
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.Callback();
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

    protected void OnMoveEnd()
    {
    }

    protected void OnSelectServant(MaterialCollectionServantListViewManager.ResultKind kind, int index)
    {
        if (this.state == State.INPUT)
        {
            this.selectIndex = index;
            if (kind == MaterialCollectionServantListViewManager.ResultKind.SERVANT_STATUS)
            {
                MaterialCollectionServantListViewItem item = this.materialCollectionServantListViewManager.GetItem(index);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.COLLECTION, item.UserServantCollection, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
            }
        }
    }

    protected void OnStartClose()
    {
        base.Close(new System.Action(this.EndClose));
    }

    protected void OnStartOpen()
    {
        base.Open(new System.Action(this.EndOpen));
    }

    public void Open(System.Action opened_act, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.mOpenedAct = opened_act;
            this.callbackFunc = callback;
            this.newSvtIdList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).GetNewList();
            base.gameObject.SetActive(true);
            this.materialCollectionServantListViewManager.gameObject.SetActive(true);
            this.materialCollectionServantListViewManager.CreateList(MaterialCollectionServantListViewManager.Kind.SERVANT);
            this.servantTabButton.SetState(UICommonButtonColor.State.Normal, true);
            this.servantTabButton.enabled = false;
            this.servantEquipTabButton.SetState(UICommonButtonColor.State.Disabled, true);
            this.SetTabKind(this.tabKind, true);
            this.state = State.OPEN;
            this.materialCollectionServantListViewManager.SetMode(MaterialCollectionServantListViewManager.InitMode.VALID);
            base.Invoke("OnStartOpen", 0.5f);
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
        UserServantCollectionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION);
        UserServantCollectionEntity[] entityArray = null;
        TabKind kind2 = kind;
        this.servantTabButton.isEnabled = true;
        this.servantTabButton.enabled = kind != TabKind.SERVANT;
        this.servantTabSprite.spriteName = (kind == TabKind.SERVANT) ? "btn_bg_09" : "btn_bg_08";
        this.servantTabSprite.MakePixelPerfect();
        this.servantTabStrSp.spriteName = (kind == TabKind.SERVANT) ? "btn_txt_servant_on" : "btn_txt_servant_off";
        this.servantTabStrSp.MakePixelPerfect();
        entityArray = master.getCollectionList(out num, out num2, false);
        this.servantTabLabelL.text = string.Empty + num2;
        this.servantTabLabelR.text = string.Empty + entityArray.Length;
        this.servantTabButton.SetState(UICommonButtonColor.State.Normal, isInit);
        this.servantEquipTabButton.isEnabled = true;
        this.servantEquipTabButton.enabled = kind != TabKind.SERVANT_EQUIP;
        this.servantEquipTabSprite.spriteName = (kind == TabKind.SERVANT_EQUIP) ? "btn_bg_09" : "btn_bg_08";
        this.servantEquipTabSprite.MakePixelPerfect();
        this.servantEquipTabStrSp.spriteName = (kind == TabKind.SERVANT_EQUIP) ? "btn_txt_craftessence_on" : "btn_txt_craftessence_off";
        this.servantEquipTabStrSp.MakePixelPerfect();
        entityArray = master.getCollectionList(out num, out num2, true);
        this.servantEquipTabLabelL.text = string.Empty + num2;
        this.servantEquipTabLabelR.text = string.Empty + entityArray.Length;
        this.servantEquipTabButton.SetState(UICommonButtonColor.State.Normal, isInit);
        if (isInit || (kind != this.tabKind))
        {
            switch (kind)
            {
                case TabKind.SERVANT:
                    this.materialCollectionServantListViewManager.CreateList(MaterialCollectionServantListViewManager.Kind.SERVANT);
                    break;

                case TabKind.SERVANT_EQUIP:
                    this.materialCollectionServantListViewManager.CreateList(MaterialCollectionServantListViewManager.Kind.SERVANT_EQUIP);
                    break;
            }
        }
        if (this.state == State.INPUT)
        {
            this.materialCollectionServantListViewManager.SetMode(MaterialCollectionServantListViewManager.InitMode.INPUT, new MaterialCollectionServantListViewManager.CallbackFunc(this.OnSelectServant));
        }
        else
        {
            this.materialCollectionServantListViewManager.SetMode(MaterialCollectionServantListViewManager.InitMode.VALID);
        }
        this.isInitTab = true;
        this.tabKind = kind;
    }

    public delegate void CallbackFunc();

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }

    public enum TabKind
    {
        SERVANT,
        SERVANT_EQUIP,
        SUM
    }
}

