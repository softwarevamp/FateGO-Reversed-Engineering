using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FavoriteChangeListViewObject : ListViewObject
{
    protected FavoriteChangeListViewItemDraw.DispMode dispMode;
    protected GameObject dragObject;
    protected FavoriteChangeListViewItemDraw itemDraw;
    protected State state;

    protected event System.Action callbackFunc;

    protected void Awake()
    {
        base.Awake();
        this.itemDraw = base.dispObject.GetComponent<FavoriteChangeListViewItemDraw>();
    }

    protected void callbackConfirmDlg(bool isDecide)
    {
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(() => base.manager.SendMessage("OnClickSelectListView", this));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        }
    }

    public override GameObject CreateDragObject()
    {
        GameObject obj2 = base.CreateDragObject();
        obj2.GetComponent<FavoriteChangeListViewObject>().Init(InitMode.VALID);
        return obj2;
    }

    public FavoriteChangeListViewItem GetItem() => 
        (base.linkItem as FavoriteChangeListViewItem);

    public void Init(InitMode initMode)
    {
        this.Init(initMode, null, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc)
    {
        this.Init(initMode, callbackFunc, 0f, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay)
    {
        this.Init(initMode, callbackFunc, delay, Vector3.zero);
    }

    public void Init(InitMode initMode, System.Action callbackFunc, float delay, Vector3 position)
    {
        FavoriteChangeListViewItem linkItem = base.linkItem as FavoriteChangeListViewItem;
        FavoriteChangeListViewItemDraw.DispMode dispMode = this.dispMode;
        bool flag = this.state == State.INIT;
        if (linkItem == null)
        {
            initMode = InitMode.INVISIBLE;
        }
        base.SetVisible(initMode != InitMode.INVISIBLE);
        this.SetInput(initMode == InitMode.INPUT);
        base.transform.localPosition = base.basePosition;
        base.transform.localScale = base.baseScale;
        this.callbackFunc = callbackFunc;
        switch (initMode)
        {
            case InitMode.INVISIBLE:
                this.dispMode = FavoriteChangeListViewItemDraw.DispMode.INVISIBLE;
                this.state = State.IDLE;
                break;

            case InitMode.INVALID:
                this.dispMode = FavoriteChangeListViewItemDraw.DispMode.INVALID;
                this.state = State.IDLE;
                break;

            case InitMode.VALID:
                this.dispMode = FavoriteChangeListViewItemDraw.DispMode.VALID;
                this.state = State.IDLE;
                break;

            case InitMode.INPUT:
                this.dispMode = FavoriteChangeListViewItemDraw.DispMode.VALID;
                this.state = State.INPUT;
                break;

            case InitMode.MODIFY:
                this.dispMode = FavoriteChangeListViewItemDraw.DispMode.VALID;
                flag = true;
                this.state = State.IDLE;
                break;
        }
        if (flag || (dispMode != this.dispMode))
        {
            this.SetupDisp();
        }
        if (this.callbackFunc != null)
        {
            System.Action action = this.callbackFunc;
            this.callbackFunc = null;
            action();
        }
    }

    protected void InitItem()
    {
        this.state = State.INIT;
    }

    public void OnClickSelect()
    {
        if (base.linkItem != null)
        {
            FavoriteChangeListViewItem linkItem = base.linkItem as FavoriteChangeListViewItem;
            if (!linkItem.IsCanNotSelect)
            {
                Debug.Log("OnClickSelect ListView " + base.linkItem.Index);
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                ServantEntity servant = linkItem.Servant;
                long favoriteUserSvtId = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).favoriteUserSvtId;
                UserServantEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(favoriteUserSvtId);
                ServantEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity3.svtId);
                Debug.Log("Manager ListView OnClick Servant Name: " + servant.name + " _Svt Class Name: " + servant.getClassName());
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDecideDlg(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_TITLE"), string.Format(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_MESSAGE"), new object[] { entity4.getClassName(), entity4.name, servant.getClassName(), servant.name }), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_DECIDE"), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.callbackConfirmDlg));
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    private void OnDestroy()
    {
        if (this.dragObject != null)
        {
            NGUITools.Destroy(this.dragObject);
            this.dragObject = null;
        }
    }

    public void OnLongPush()
    {
        if (base.linkItem != null)
        {
            Debug.Log("OnLongPush ListView " + base.linkItem.Index);
            base.gameObject.SendMessage("OnPressCancel");
            base.manager.SendMessage("OnLongPushListView", this);
        }
    }

    public override void SetInput(bool isInput)
    {
        base.SetInput(isInput);
        FavoriteChangeListViewItem linkItem = base.linkItem as FavoriteChangeListViewItem;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetInput(linkItem, isInput);
        }
        if (base.mDragDrop != null)
        {
            base.mDragDrop.SetEnable(isInput);
        }
    }

    public override void SetItem(ListViewItem item)
    {
        base.SetItem(item);
        this.InitItem();
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        base.SetItem(item, seed);
        this.InitItem();
    }

    protected void SetupDisp()
    {
        FavoriteChangeListViewItem linkItem = base.linkItem as FavoriteChangeListViewItem;
        base.SetVisible((linkItem != null) && (this.dispMode != FavoriteChangeListViewItemDraw.DispMode.INVISIBLE));
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(linkItem, this.dispMode);
        }
    }

    public enum InitMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        MODIFY
    }

    protected enum State
    {
        INIT,
        IDLE,
        MOVE,
        INPUT
    }
}

