using System;
using UnityEngine;

public class FavoriteChangeComponent : MonoBehaviour
{
    [SerializeField]
    protected FavoriteChangeListViewManager favoriteChangeManager;
    [SerializeField]
    protected UILabel infoLb;
    [SerializeField]
    protected PlayMakerFSM myRoomFsm;

    private void closeSvtDetail(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(delegate {
            this.favoriteChangeManager.ModifyItem();
            this.favoriteChangeManager.SortItem(-1, false, -1);
            this.favoriteChangeManager.SetMode(FavoriteChangeListViewManager.InitMode.INPUT, new FavoriteChangeListViewManager.CallbackFunc(this.OnClickServant));
        });
    }

    public void dispSvtList()
    {
        base.gameObject.SetActive(true);
    }

    private void EndCloseConfirmSelectFavorite()
    {
        this.favoriteChangeManager.SetMode(FavoriteChangeListViewManager.InitMode.INPUT, new FavoriteChangeListViewManager.CallbackFunc(this.OnClickServant));
    }

    private void EndeCardFavoriteRequest(string result)
    {
        this.favoriteChangeManager.ModifyItem();
        this.favoriteChangeManager.SortItem(-1, false, -1);
        this.favoriteChangeManager.SetMode(FavoriteChangeListViewManager.InitMode.INPUT, new FavoriteChangeListViewManager.CallbackFunc(this.OnClickServant));
    }

    public void hideFavoriteChangeInfo()
    {
        this.favoriteChangeManager.DestroyList();
        base.gameObject.SetActive(false);
    }

    private void OnClickServant(FavoriteChangeListViewManager.ResultKind kind, int n)
    {
        FavoriteChangeListViewItem selectItem = (n < 0) ? null : this.favoriteChangeManager.GetItem(n);
        switch (kind)
        {
            case FavoriteChangeListViewManager.ResultKind.DECIDE:
                if (selectItem.IsEventJoin)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_EVENT_JOIN_TITLE"), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_EVENT_JOIN_MESSAGE"), new System.Action(this.EndCloseConfirmSelectFavorite), -1);
                }
                else
                {
                    this.setFavoriteRequest(selectItem);
                }
                break;

            case FavoriteChangeListViewManager.ResultKind.SHOW_STATUS:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, selectItem.UserServant, new ServantStatusDialog.ClickDelegate(this.closeSvtDetail));
                break;
        }
    }

    public void setFavoriteRequest(FavoriteChangeListViewItem selectItem)
    {
        NetworkManager.getRequest<CardFavoriteRequest>(new NetworkManager.ResultCallbackFunc(this.EndeCardFavoriteRequest)).beginRequest(selectItem.UserServant.id, selectItem.UserServant.imageLimitCount, selectItem.UserServant.dispLimitCount, selectItem.UserServant.commandCardLimitCount, selectItem.UserServant.iconLimitCount, true);
    }

    public void showFavoriteChangeInfo()
    {
        this.infoLb.text = LocalizationManager.Get("HEADER_MSG_FAVORITE");
        base.gameObject.SetActive(true);
        this.favoriteChangeManager.CreateList();
        this.favoriteChangeManager.SetMode(FavoriteChangeListViewManager.InitMode.INPUT, new FavoriteChangeListViewManager.CallbackFunc(this.OnClickServant));
    }
}

