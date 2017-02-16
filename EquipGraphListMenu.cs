using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EquipGraphListMenu : BaseMenu
{
    protected System.Action closeCallbackFunc;
    protected int selectNum;
    [SerializeField]
    protected EquipGraphListViewManager servantEquipListViewManager;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int n = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            EquipGraphListViewItem item = (n < 0) ? null : this.servantEquipListViewManager.GetItem(n);
            this.callbackFunc = null;
            callbackFunc(result, item);
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
        this.SetListManagerStatus();
        base.Close(new System.Action(this.EndClose));
    }

    protected void Decide()
    {
        this.state = State.WARNING;
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        this.state = State.SELECTED;
        EquipGraphServantItem servantItemInfo = this.servantEquipListViewManager.GetServantItemInfo();
        if (((this.selectNum < 0) ? null : this.servantEquipListViewManager.GetItem(this.selectNum)) != null)
        {
            EventServantEntity entity = servantItemInfo.UserServant.getEventServant();
            if (servantItemInfo.UserServant.IsEventJoin() && (entity != null))
            {
                string message = string.Format(LocalizationManager.Get("EQUIP_GRAPH_EVENT_SERVANT_WARNING_MESSAGE"), entity.getEndTimeStr());
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("EQUIP_GRAPH_EVENT_SERVANT_WARNING_TITLE"), message, new System.Action(this.EndCloseEventServantWarning), -1);
                return;
            }
        }
        this.Callback(ResultKind.DECIDE, this.selectNum);
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

    protected void EndCloseEventServantWarning()
    {
        this.Callback(ResultKind.DECIDE, this.selectNum);
    }

    protected void EndCloseList()
    {
    }

    protected void EndCloseShowServantEquip()
    {
        this.state = State.INPUT;
        this.servantEquipListViewManager.SetMode(EquipGraphListViewManager.InitMode.INPUT, new EquipGraphListViewManager.CallbackFunc(this.OnSelectServantEquip));
    }

    protected void EndOpen()
    {
        if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_DECK_SVT_EQUIP))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.EQUIP_INFO_2, TutorialFlag.Id.TUTORIAL_LABEL_DECK_SVT_EQUIP, null);
        }
        this.state = State.INPUT;
        this.SetListManagerStatus();
    }

    protected void EndShowServantEquip(bool isDecide)
    {
        if (isDecide)
        {
            this.servantEquipListViewManager.ModifyList();
            this.servantEquipListViewManager.SetMode(EquipGraphListViewManager.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(new System.Action(this.EndCloseShowServantEquip));
    }

    public void Init()
    {
        this.servantEquipListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            EquipGraphListViewItem selectItem = this.servantEquipListViewManager.GetSelectItem();
            this.selectNum = (selectItem == null) ? -1 : selectItem.Index;
            this.Decide();
        }
    }

    protected void OnSelectServantEquip(EquipGraphListViewManager.ResultKind kind, int n)
    {
        if (this.state == State.INPUT)
        {
            this.selectNum = n;
            EquipGraphListViewItem item = (n < 0) ? null : this.servantEquipListViewManager.GetItem(n);
            EquipGraphListViewManager.ResultKind kind2 = kind;
            if (kind2 == EquipGraphListViewManager.ResultKind.DECIDE)
            {
                this.Decide();
            }
            else if (kind2 == EquipGraphListViewManager.ResultKind.SHOW_STATUS)
            {
                if (item != null)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.state = State.SHOW_STATUS;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.NORMAL, item.UserServant, item.IsUse || item.IsBase, new ServantStatusDialog.ClickDelegate(this.EndShowServantEquip));
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.state = State.INPUT;
                    this.servantEquipListViewManager.SetMode(EquipGraphListViewManager.InitMode.INPUT, new EquipGraphListViewManager.CallbackFunc(this.OnSelectServantEquip));
                }
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.state = State.SELECTED;
                this.Callback(ResultKind.CANCEL, -1);
            }
        }
    }

    public void Open(PartyListViewItem partyItem, int member, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.FORM_SVT_EQUIP_SELECT);
            this.titleInfo.setBackBtnSprite(true);
            this.servantEquipListViewManager.CreateList(partyItem, member);
            this.state = State.OPEN;
            this.SetListManagerStatus();
            base.Open(new System.Action(this.EndOpen));
        }
        else if ((this.state == State.INPUT) || (this.state == State.SELECTED))
        {
            this.callbackFunc = callback;
            this.state = State.INPUT;
            this.SetListManagerStatus();
        }
    }

    protected void SetListManagerStatus()
    {
        if (this.state == State.INPUT)
        {
            this.servantEquipListViewManager.SetMode(EquipGraphListViewManager.InitMode.INPUT, new EquipGraphListViewManager.CallbackFunc(this.OnSelectServantEquip));
        }
        else
        {
            this.servantEquipListViewManager.SetMode(EquipGraphListViewManager.InitMode.VALID);
        }
    }

    public delegate void CallbackFunc(EquipGraphListMenu.ResultKind result, EquipGraphListViewItem item);

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
        SHOW_STATUS,
        WARNING
    }
}

