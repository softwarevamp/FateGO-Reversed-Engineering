using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupportServantEquipListMenu : BaseMenu
{
    private int classPos;
    protected System.Action closeCallbackFunc;
    protected int selectNum;
    protected State state;
    [SerializeField]
    protected SupportServantEquipListViewManager supportServantEquipListViewManager;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    private UserServantEntity userServantEntity;

    protected event CallbackFunc callbackFunc;

    protected void Callback(SupportSelectRootComponent.ResultKind result, int classPos, SupportServantEquipListViewItem item)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            callbackFunc(result, classPos, item);
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
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        this.state = State.SELECTED;
        SupportServantEquipListViewItem item = (this.selectNum < 0) ? null : this.supportServantEquipListViewManager.GetItem(this.selectNum);
        this.Callback(SupportSelectRootComponent.ResultKind.DECIDE, this.classPos, item);
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
        this.Callback(SupportSelectRootComponent.ResultKind.DECIDE, this.classPos, null);
    }

    protected void EndCloseList()
    {
    }

    protected void EndCloseShowServantEquip()
    {
        this.state = State.INPUT;
        this.supportServantEquipListViewManager.SetMode(SupportServantEquipListViewManager.InitMode.INPUT, new SupportServantEquipListViewManager.CallbackFunc(this.OnSelectServantEquip));
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
            this.supportServantEquipListViewManager.ModifyList();
            this.supportServantEquipListViewManager.SetMode(SupportServantEquipListViewManager.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(new System.Action(this.EndCloseShowServantEquip));
    }

    public void Init()
    {
        this.supportServantEquipListViewManager.DestroyList();
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(SupportSelectRootComponent.ResultKind.CANCEL, this.classPos, null);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            SupportServantEquipListViewItem selectItem = this.supportServantEquipListViewManager.GetSelectItem();
            this.selectNum = (selectItem == null) ? -1 : selectItem.Index;
            this.Decide();
        }
    }

    protected void OnSelectServantEquip(SupportServantEquipListViewManager.ResultKind kind, int n)
    {
        if (this.state == State.INPUT)
        {
            this.selectNum = n;
            SupportServantEquipListViewItem item = (n < 0) ? null : this.supportServantEquipListViewManager.GetItem(n);
            SupportServantEquipListViewManager.ResultKind kind2 = kind;
            if (kind2 == SupportServantEquipListViewManager.ResultKind.DECIDE)
            {
                this.Decide();
            }
            else if (kind2 == SupportServantEquipListViewManager.ResultKind.SHOW_STATUS)
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
                    this.supportServantEquipListViewManager.SetMode(SupportServantEquipListViewManager.InitMode.INPUT, new SupportServantEquipListViewManager.CallbackFunc(this.OnSelectServantEquip));
                }
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.state = State.SELECTED;
                this.Callback(SupportSelectRootComponent.ResultKind.DECIDE, this.classPos, null);
            }
        }
    }

    public void Open(SupportServantData supportServantData, int classPos, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.classPos = classPos;
            this.userServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(supportServantData.getServant(classPos));
            base.gameObject.SetActive(true);
            this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.FORM_SVT_EQUIP_SELECT);
            this.titleInfo.setBackBtnSprite(true);
            this.supportServantEquipListViewManager.CreateList(supportServantData, classPos);
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
            this.supportServantEquipListViewManager.SetMode(SupportServantEquipListViewManager.InitMode.INPUT, new SupportServantEquipListViewManager.CallbackFunc(this.OnSelectServantEquip));
        }
        else
        {
            this.supportServantEquipListViewManager.SetMode(SupportServantEquipListViewManager.InitMode.VALID);
        }
    }

    public delegate void CallbackFunc(SupportSelectRootComponent.ResultKind result, int classPos, SupportServantEquipListViewItem item);

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

