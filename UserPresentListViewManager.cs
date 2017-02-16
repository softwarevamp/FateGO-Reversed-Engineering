using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserPresentListViewManager : ListViewManager
{
    [CompilerGenerated]
    private static ReceiveCallbackFunc <>f__am$cache10;
    protected int callbackCount;
    private List<long> checkedIdList;
    [SerializeField]
    private UserPresentBoxErrorDialog dialog;
    protected InitMode initMode;
    private KIND kind;
    [SerializeField]
    protected UserPresentBoxWindow m_parent;
    private List<long> presentIds;
    private UserPresentListViewItem selectItem;
    protected UserPresentBoxEntity selectPresentData;
    protected const string SORT_SAVE_KEY = "UserPresent";
    [SerializeField]
    protected UISprite sortExplanationSprite;
    protected static ListViewSort sortInfo = new ListViewSort("UserPresent", ListViewSort.SortKind.CREATE, false);

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    protected event ReceiveCallbackFunc receivecCallbackFunc;

    public void CreateList(UserPresentBoxEntity[] presentList)
    {
        base.sort = sortInfo;
        base.sort.Load();
        base.CreateList(0);
        this.selectPresentData = null;
        this.presentIds = null;
        if (this.checkedIdList == null)
        {
            for (int i = 0; i < presentList.Length; i++)
            {
                UserPresentListViewItem item = new UserPresentListViewItem(i, presentList[i]);
                base.itemList.Add(item);
            }
        }
        else
        {
            List<long> list = new List<long>(this.checkedIdList);
            for (int j = 0; j < presentList.Length; j++)
            {
                list.Remove(presentList[j].presentId);
                UserPresentListViewItem item2 = new UserPresentListViewItem(j, presentList[j]);
                base.itemList.Add(item2);
            }
            for (int k = 0; k < list.Count; k++)
            {
                this.checkedIdList.Remove(list[k]);
            }
            if (this.checkedIdList.Count > 0)
            {
                this.m_parent.SetCheckedItemsButtonEnable(true, true);
                this.updateCheckStatus();
            }
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    private void EndNoticeDlg(SceneList.Type scene)
    {
        this.dialog.OnErrorDialogClosed -= new Action<SceneList.Type>(this.EndNoticeDlg);
        if (scene != SceneList.Type.None)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseUsrPresentList();
        }
        if (this.receivecCallbackFunc != null)
        {
            this.receivecCallbackFunc(true);
        }
    }

    public UserPresentListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as UserPresentListViewItem);
        }
        return null;
    }

    protected void OnClickListCheck(UserPresentListViewObject obj)
    {
        this.selectItem = obj.GetItem();
        Debug.Log("OnClickListView : " + this.selectItem.PresentId);
        this.selectPresentData = this.selectItem.UserPresentEntity;
        if (this.checkedIdList == null)
        {
            this.checkedIdList = new List<long>();
        }
        if (this.selectItem.PresentId > 0L)
        {
            int count = this.checkedIdList.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.checkedIdList[i] == this.selectItem.PresentId)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    this.checkedIdList.RemoveAt(i);
                    this.updateCheckStatus();
                    return;
                }
            }
            if (this.selectPresentData.giftType == 2)
            {
                long[] args = new long[] { NetworkManager.UserId, (long) this.selectPresentData.objectId };
                UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
                if (entity != null)
                {
                    int num3 = entity.getUserItemNum() + this.selectPresentData.num;
                    if (num3 > BalanceConfig.UserItemMax)
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                        return;
                    }
                }
            }
            if (this.checkedIdList.Count >= BalanceConfig.PresentBoxCheckMax)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.checkedIdList.Add(this.selectItem.PresentId);
                if (this.checkedIdList.Count >= BalanceConfig.PresentBoxCheckMax)
                {
                    this.updateCheckStatus();
                }
                else
                {
                    obj.GetItem().setCheckBoxed(true, this.checkedIdList.Count);
                }
                this.m_parent.SetCheckedItemsButtonEnable(true, true);
            }
        }
    }

    protected void OnClickListView(UserPresentListViewObject obj)
    {
        this.kind = KIND.NORMAL;
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.selectItem = obj.GetItem();
        Debug.Log("OnClickListView : " + this.selectItem.PresentId);
        this.selectPresentData = this.selectItem.UserPresentEntity;
        if (this.selectPresentData.IsExpired())
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, LocalizationManager.Get("PRESENT_EXPIRED_ERROR_MESSAGE"), delegate {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, 0.5f, delegate {
                    this.DestroyList();
                    this.m_parent.SetReDispFromExpiredPresent();
                    base.scrollView.ResetPosition();
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.5f, null);
                });
            }, -1);
        }
        else
        {
            Debug.LogError((Gift.Type) this.selectPresentData.giftType);
            if (this.selectPresentData.giftType == 2)
            {
                long[] args = new long[] { NetworkManager.UserId, (long) this.selectPresentData.objectId };
                UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
                if (entity != null)
                {
                    int num = entity.getUserItemNum() + this.selectPresentData.num;
                    if (num > BalanceConfig.UserItemMax)
                    {
                        if (<>f__am$cache10 == null)
                        {
                            <>f__am$cache10 = delegate (bool res) {
                            };
                        }
                        this.showErrorResultDlg(<>f__am$cache10);
                        return;
                    }
                }
            }
            this.select_idx = this.selectItem.Index;
            this.presentIds = new List<long>();
            if (this.selectItem.PresentId > 0L)
            {
                this.presentIds.Add(this.selectItem.PresentId);
            }
            long[] presentIds = this.presentIds.ToArray();
            this.m_parent.receivePresent(presentIds);
            this.SetMode(InitMode.HOLD);
        }
    }

    public void OnClickSortAscendingOrder()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
            base.SortItem(-1, false, -1);
        }
    }

    protected void OnMoveEnd()
    {
        Debug.Log("OnMoveEnd " + this.callbackCount);
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
                System.Action action = this.callbackFunc2;
                this.callbackFunc2 = null;
                if (action != null)
                {
                    action();
                }
            }
        }
    }

    private void OpenNoticeDlg(string msg)
    {
        Debug.Log("OpenNoticeDlg");
        this.dialog.Open(msg);
        this.dialog.OnErrorDialogClosed += new Action<SceneList.Type>(this.EndNoticeDlg);
    }

    public void ReceiveMultiPresent(KIND kind)
    {
        this.kind = kind;
        this.presentIds = new List<long>();
        long num = NetworkManager.getTime();
        List<string> list = new List<string>();
        foreach (UserPresentListViewItem item in base.itemList)
        {
            switch (kind)
            {
                case KIND.ALL:
                    break;

                case KIND.ITEM:
                {
                    if (!Gift.IsServant(item.ListType))
                    {
                        break;
                    }
                    continue;
                }
                case KIND.CHECK:
                {
                    if (this.checkedIdList.Contains(item.PresentId))
                    {
                        break;
                    }
                    continue;
                }
                default:
                {
                    continue;
                }
            }
            this.presentIds.Add(item.PresentId);
        }
        this.expiredPresents = string.Empty;
        while (list.Count > 0)
        {
            this.expiredPresents = this.expiredPresents + "[" + list[0] + "]";
            list.RemoveAt(0);
        }
        long[] presentIds = this.presentIds.ToArray();
        this.m_parent.receivePresent(presentIds);
    }

    protected void RequestListObject(UserPresentListViewObject.InitMode mode)
    {
        List<UserPresentListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (UserPresentListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd));
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(UserPresentListViewObject.InitMode mode, float delay)
    {
        List<UserPresentListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (UserPresentListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", delay);
        }
    }

    public void resetCheckStatus()
    {
        if (this.checkedIdList != null)
        {
            this.checkedIdList.Clear();
        }
    }

    public void SetDefaultSort()
    {
        base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
    }

    public void SetMode(InitMode mode)
    {
        this.initMode = mode;
        this.callbackCount = base.objectList.Count;
        base.IsInput = mode == InitMode.INPUT;
        switch (mode)
        {
            case InitMode.INPUT:
                this.RequestListObject(UserPresentListViewObject.InitMode.INPUT);
                break;

            case InitMode.HOLD:
                this.RequestListObject(UserPresentListViewObject.InitMode.HOLD);
                break;
        }
        Debug.Log("SetMode " + mode);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        UserPresentListViewObject obj2 = obj as UserPresentListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(UserPresentListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(UserPresentListViewObject.InitMode.VALID);
        }
    }

    protected override void SetSortButtonImage()
    {
        if (base.sortKindLabel != null)
        {
            base.sortKindLabel.text = base.sort.GetKindButtonText();
        }
        if (base.sortOrderSprite != null)
        {
            if ((base.sort.Kind == ListViewSort.SortKind.LOGIN_ACCESS) || (base.sort.Kind == ListViewSort.SortKind.CREATE))
            {
                base.sortOrderSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_sort_up" : "btn_sort_down";
            }
            else
            {
                base.sortOrderSprite.spriteName = !base.sort.IsAscendingOrder ? "btn_sort_down" : "btn_sort_up";
            }
        }
    }

    public void showErrorResultDlg(ReceiveCallbackFunc callback)
    {
        this.receivecCallbackFunc = callback;
        string msg = string.Empty;
        switch (this.kind)
        {
            case KIND.NORMAL:
                msg = LocalizationManager.Get("REJECT_NORMAL_TXT");
                break;

            case KIND.ALL:
            case KIND.ITEM:
            case KIND.CHECK:
                msg = LocalizationManager.Get("REJECT_ALL_TXT");
                break;
        }
        Debug.Log("showErrorResultDlg OpenNoticeDlg");
        this.OpenNoticeDlg(msg);
    }

    public void updateCheckStatus()
    {
        if ((this.checkedIdList == null) || (this.checkedIdList.Count == 0))
        {
            this.m_parent.SetCheckedItemsButtonEnable(false, true);
        }
        bool blocked = this.checkedIdList.Count >= BalanceConfig.PresentBoxCheckMax;
        List<long> list = new List<long>(this.checkedIdList);
        for (int i = 0; i < base.itemList.Count; i++)
        {
            UserPresentListViewItem item = (UserPresentListViewItem) base.itemList[i];
            long presentId = item.PresentId;
            if (list.Remove(presentId))
            {
                item.setCheckBoxed(true, this.checkedIdList.IndexOf(presentId) + 1);
            }
            else
            {
                item.setCheckBoxed(false, -1);
                item.setBlocked(blocked);
            }
        }
    }

    public List<UserPresentListViewObject> ClippingObjectList
    {
        get
        {
            List<UserPresentListViewObject> list = new List<UserPresentListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    UserPresentListViewObject component = obj2.GetComponent<UserPresentListViewObject>();
                    UserPresentListViewItem item = component.GetItem();
                    if (item.IsTermination)
                    {
                        if (base.ClippingItem(item))
                        {
                            list.Add(component);
                        }
                    }
                    else
                    {
                        list.Add(component);
                    }
                }
            }
            return list;
        }
    }

    public string expiredPresents { get; set; }

    public List<UserPresentListViewObject> ObjectList
    {
        get
        {
            List<UserPresentListViewObject> list = new List<UserPresentListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    UserPresentListViewObject component = obj2.GetComponent<UserPresentListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public int select_idx { get; private set; }

    public delegate void CallbackFunc(string result);

    public enum InitMode
    {
        NONE,
        INPUT,
        HOLD
    }

    public enum KIND
    {
        NORMAL,
        ALL,
        ITEM,
        CHECK
    }

    public delegate void ReceiveCallbackFunc(bool isReceive);
}

